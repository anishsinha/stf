using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using PrimS.Telnet;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.Repository.Mappers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.Repository
{
    public class DemandCaptureRepository : IDemandCaptureRepository
    {
        private readonly DemandCaptureContext context = new DemandCaptureContext();
        private readonly MdbContext mdbContext;
        public DemandCaptureRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public DemandCaptureRepository(MdbContext _context)
        {
            mdbContext = _context;
        }
        public async Task<long> SaveDemandFileInfo(string fileName, long uid, DipTestMethod dipTestMethod)
        {
            long response;
            try
            {
                var sourcefile = new SourceFile { CreationDate = DateTimeOffset.Now, FileName = fileName, Uid = uid, DataSourceType = (int)dipTestMethod };
                context.SourceFiles.Add(sourcefile);
                await context.SaveChangesAsync();
                response = sourcefile.Id;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "SaveDemandFileInfo", ex.Message, ex);
                throw;
            }
            return response;
        }
        public async Task<bool> IsIS360FileExists(string fileName, DipTestMethod dipTestMethod)
        {
            bool response;
            try
            {
                var srcLst = await context.SourceFiles.Where(w => w.FileName == fileName && w.DataSourceType == (int)dipTestMethod).ToListAsync();
                if (srcLst != null && srcLst.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "IsIS360FileExists", ex.Message, ex);
                throw;
            }
            return response;
        }
        public async Task<LongStatusModel> GetLastProcessedUid()
        {
            LongStatusModel response = new LongStatusModel();
            try
            {
                var Uid = await context.SourceFiles.MaxAsync(t => t.Uid);
                response.Result = Uid;
                response.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetLastProcessedUid", ex.Message, ex);
                throw;
            }
            return response;
        }
        private void CreateDemandTable(List<Demand> demandList, out DataTable demands)
        {
            demands = new DataTable("Demands");

            // Define all the columns once.
            DataColumn[] cols ={
                                    new DataColumn("SiteId",typeof(String)){ AllowDBNull = true },
                                    new DataColumn("TankId",typeof(String)){ AllowDBNull = true },
                                    new DataColumn("StorageId",typeof(String)){ AllowDBNull = true },
                                    new DataColumn("Level",typeof(Decimal)),
                                    new DataColumn("Ullage",typeof(Decimal)),
                                    new DataColumn("GrossVolume",typeof(Decimal)),
                                    new DataColumn("NetVolume",typeof(Decimal)),
                                    new DataColumn("WaterNetLevel",typeof(Decimal)),
                                    new DataColumn("WaterGrossLevel",typeof(Decimal)),
                                    new DataColumn("CaptureTime",typeof(DateTime)),
                                    new DataColumn("ProductName",typeof(String)){ AllowDBNull = true },
                                    new DataColumn("DataSourceTypeId",typeof(int)),
                                    new DataColumn("SupplierId",typeof(int)),
                                    new DataColumn("SourceFileId",typeof(long)){ AllowDBNull = true },
                                    new DataColumn("IsProcessed",typeof(bool)),
                                    new DataColumn("DipTestValue",typeof(Decimal)),
                                    new DataColumn("DipTestUoM",typeof(int)),
                                    new DataColumn("IsActive",typeof(bool))
                                    };

            demands.Columns.AddRange(cols);


            foreach (var item in demandList.Distinct().ToList())
            {
                var row = demands.NewRow();
                row["SiteId"] = item.SiteId;
                row["TankId"] = item.TankId;
                row["StorageId"] = item.StorageId;
                row["Level"] = item.Level;
                row["Ullage"] = item.Ullage;
                row["GrossVolume"] = item.GrossVolume;
                row["NetVolume"] = item.NetVolume;
                row["WaterNetLevel"] = item.WaterNetLevel;
                row["WaterGrossLevel"] = item.WaterGrossLevel;
                row["CaptureTime"] = item.CaptureTime;
                row["ProductName"] = item.ProductName;
                row["DataSourceTypeId"] = item.DataSourceTypeId;
                row["SupplierId"] = item.SupplierId;
                row["SourceFileId"] = DBNull.Value;
                if (item.SourceFileId.HasValue)
                {
                    row["SourceFileId"] = item.SourceFileId.Value;
                }
                row["IsProcessed"] = item.IsProcessed;
                row["DipTestValue"] = item.DipTestValue;
                row["DipTestUoM"] = (int)item.DipTestUoM;
                row["IsActive"] = item.IsActive;
                demands.Rows.Add(row);
            }

        }

        public async Task<int> CreateDemand(List<DemandModel> demandModels, int supplierId, long? fileId = null)
        {
            int response = 0;
            try
            {
                var demands = demandModels.Select(x => DemandCaptureMapper.ToEntity(x, fileId, supplierId));

                DataTable tableDemands;
                CreateDemandTable(demands.ToList(), out tableDemands);

                var tableDemandsParam = new SqlParameter("@TableDemands", SqlDbType.Structured);
                tableDemandsParam.Value = tableDemands;
                tableDemandsParam.TypeName = "dbo.DemandsTypes";
                response = await context.Database.SqlQuery<int>("usp_InsertIntoDemands @TableDemands", tableDemandsParam).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "CreateDemand", ex.Message + "supplierId:" + supplierId + "fileId:" + fileId, ex);
                throw;
            }
            return response;
        }

        public async Task<int> CreateTankDipTest(List<DemandModel> dipTestModels, int supplierId)
        {
            int response = 0;
            int diptestModelCount = dipTestModels.Count;
            try
            {

                var demands = dipTestModels.Select(x => DemandCaptureMapper.ToEntity(x, null, supplierId));
                DataTable tableDemands;
                CreateDemandTable(demands.ToList(), out tableDemands);

                var tableDemandsParam = new SqlParameter("@TableDemands", SqlDbType.Structured);
                tableDemandsParam.Value = tableDemands;
                tableDemandsParam.TypeName = "dbo.DemandsTypes";
                response = await context.Database.SqlQuery<int>("usp_InsertIntoDemands @TableDemands", tableDemandsParam).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "CreateTankDipTest", ex.Message + "supplierId:" + supplierId + "diptestModelCount:" + diptestModelCount, ex);
            }
            return response;
        }

        public async Task<List<List<DemandCaptureChartViewModel>>> GetDemandCaptureChartDataByTankAndSite(List<int?> assetIds, string siteId, int noOfDays)
        {
            var responseFinal = new List<List<DemandCaptureChartViewModel>>();
            foreach (var assetId in assetIds)
            {
                if (assetId != null && assetId > 0)
                {
                    var response = new List<DemandCaptureChartViewModel>();

                    var tankIds = new List<string>();
                    var storageIds = new List<string>();
                    var tankName = "";

                    var jobTanks = mdbContext.JobAdditionalDetails.Find(x => x.Tanks.Any(x1 => x1.TfxAssetId == assetId)).Project(d => new { Tanks = d.Tanks.Where(d1 => d1.TfxAssetId == assetId) }.Tanks.FirstOrDefault()).ToList();

                    if (jobTanks.Any())
                    {
                        storageIds = jobTanks.Select(t => t.StorageId).ToList();
                        tankIds = jobTanks.Select(t => t.StorageTypeId).ToList();
                        tankName = jobTanks.Select(t => t.TankName).FirstOrDefault();

                        DateTimeOffset date = DateTimeOffset.Now.AddDays(-noOfDays);

                        var data = await context.Demands
                            .Where(dc => dc.SiteId == siteId && tankIds.Contains(dc.TankId) && storageIds.Contains(dc.StorageId) && dc.CaptureTime >= date)
                                   .OrderBy(dc => dc.CaptureTime)
                                   .Select(dc => new
                                   {
                                       dc.TankId,
                                       dc.StorageId,
                                       dc.CaptureTime,
                                       dc.NetVolume,
                                       dc.Ullage,
                                       dc.ProductName
                                   }).ToListAsync();

                        data.ForEach(dc => response.Add(
                            new DemandCaptureChartViewModel()
                            {
                                TankId = dc.TankId,
                                StorageId = dc.StorageId,
                                CaptureTime = dc.CaptureTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                                NetVolume = dc.NetVolume,
                                Ullage = dc.Ullage,
                                ProductName = dc.ProductName,
                                TankName = tankName
                            }));

                        if (response.Any())
                        {
                            responseFinal.Add(response);
                        }
                    }
                }
            }
            return responseFinal;
        }

        public async Task<CreateDRTankModel> GetTankDetailsWithDipTestData(int jobId, int sourceTypeId, int companyId, bool isCreateDR)
        {
            CreateDRTankModel response = new CreateDRTankModel();
            var allSitesDemands = new List<ProductModelToCreateDR>();
            response.FavoriteProducts = await GetRegionFavProducts(jobId, companyId, null);
            List<int> regProductTypes = GetRegionFavProductTypes(jobId, companyId, null, response.FavoriteProducts);
            var isAllProductTypesValid = regProductTypes == null || !regProductTypes.Any();
            var job = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == jobId).Project(t =>
            new JobWithProductsModel()
            {
                TfxJobId = t.TfxJobId,
                TfxDisplayJobId = t.TfxDisplayJobId,
                Products = t.Tanks.Where(t1 => isAllProductTypesValid || regProductTypes.Contains(t1.TfxProductTypeId))
                                    .OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).GroupBy(t1 => t1.TfxProductTypeId)
                                    .Select(t1 => new ProductTypesWithTanks { ProductTypeId = t1.Key, Tanks = t1.ToList(), ProductTypeName = t1.Select(t2 => t2.TfxProductTypeName).FirstOrDefault() }).ToList()
            }).FirstOrDefaultAsync();
            if (job != null && job.Products != null && job.Products.Any())
            {
                var jobTanks = job.Products.SelectMany(t => t.Tanks);
                var tankIdList = jobTanks.Where(t => t.StorageTypeId != null).Select(t => t.StorageTypeId).ToList();
                var storageList = jobTanks.Where(t => t.StorageId != null).Select(t => t.StorageId).ToList();
                string siteId = job.TfxDisplayJobId;
                List<Demand> dipTestData = null;
                if (tankIdList.Any() && storageList.Any() && !string.IsNullOrWhiteSpace(siteId))
                {
                    dipTestData = await GetDemands(new List<string>() { siteId }, tankIdList, storageList, sourceTypeId);
                }
                SetTankDetails(new List<JobWithProductsModel>() { job }, dipTestData, allSitesDemands);
                if (isCreateDR)
                {
                    await CheckIfDrExists(allSitesDemands, new List<int>() { jobId }, tankIdList, storageList);
                }
            }
            response.Tanks = allSitesDemands;
            return response;
        }

        private static List<int> GetRegionFavProductTypes(int? jobId, int companyId, string regionId, RegionFavProductModel regFavProduct)
        {
            var regProductTypes = new List<int>();
            if (regFavProduct != null)
            {
                if (regFavProduct.TfxFavProductTypeId == RegionFavProductType.ProductType && regFavProduct.TfxProductTypeIds != null)
                {
                    regProductTypes = regFavProduct.TfxProductTypeIds;
                }
                else if (regFavProduct.TfxFavProductTypeId == RegionFavProductType.FuelType && regFavProduct.TfxFuelTypeIds != null)
                {
                    regProductTypes = regFavProduct.TfxFuelTypeIds.Where(t => t.Code != null).Select(t => int.Parse(t.Code)).ToList();
                }
            }

            return regProductTypes;
        }

        private async Task<RegionFavProductModel> GetRegionFavProducts(int? jobId, int companyId, string regionId)
        {
            return await new RegionRepository(mdbContext).GetRegionFavouriteProducts(jobId, regionId, companyId);
        }

        public async Task<CreateDRTankModel> GetTankDetailsForRegion(int companyId, string regionId, string buyerJobs, int sourceTypeId, bool isCreateDR)
        {
            CreateDRTankModel response = new CreateDRTankModel();
            var allSitesDemands = new List<ProductModelToCreateDR>();
            ObjectId regionObjId = ObjectId.Parse(regionId);
            var jobIds = await mdbContext.Regions.Find(t => t.TfxCompanyId == companyId &&
                     t.Id == regionObjId && t.TfxJobs != null).Project(t => t.TfxJobs.Select(t1 => t1.Id).ToList()).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(buyerJobs))
            {
                var bJobIds = buyerJobs.Split(',').Select(t => int.Parse(t)).ToList();
                jobIds = jobIds.Where(t => bJobIds.Contains(t)).ToList();
            }
            response.FavoriteProducts = await GetRegionFavProducts(null, companyId, regionId);
            var regProductTypes = GetRegionFavProductTypes(null, companyId, regionId, response.FavoriteProducts);

            var jobwithTanks = await mdbContext.JobAdditionalDetails
                                .Find(t => jobIds != null && jobIds.Contains(t.TfxJobId))
                                .Project(t => new { Tanks = t.Tanks.Where(t1 => !regProductTypes.Any() || regProductTypes.Contains(t1.TfxProductTypeId)).ToList(), t.TfxJobId, t.TfxDisplayJobId })
                                .ToListAsync();
            var jobs = new List<JobWithProductsModel>();
            var totalProducts = 5;
            jobwithTanks.ForEach(t =>
            {
                if (totalProducts > 0)
                {
                    if (t.Tanks != null && t.Tanks.Count > 0)
                    {
                        var takeProduct = Math.Min(t.Tanks.GroupBy(t1 => t1.TfxProductTypeId).Count(), totalProducts);
                        jobs.Add(new JobWithProductsModel() { TfxJobId = t.TfxJobId, Products = t.Tanks.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).GroupBy(t2 => t2.TfxProductTypeId).Take(takeProduct).Select(t3 => new ProductTypesWithTanks { ProductTypeId = t3.Key, ProductTypeName = t3.Select(t4 => t4.TfxProductTypeName).FirstOrDefault(), Tanks = t3.ToList() }).ToList(), TfxDisplayJobId = t.TfxDisplayJobId });
                        totalProducts = totalProducts - takeProduct;
                    }
                }
                else
                {
                    return;
                }
            });
            var tanks = jobs.SelectMany(t => t.Products.SelectMany(t2 => t2.Tanks));
            var tankIdList = tanks.Where(t => t.StorageTypeId != null).Select(t => t.StorageTypeId).Distinct().ToList();
            var storageList = tanks.Where(t => t.StorageId != null).Select(t => t.StorageId).Distinct().ToList();
            var siteIdList = jobs.Where(t => t.TfxDisplayJobId != null && t.TfxDisplayJobId != "").Select(t => t.TfxDisplayJobId).ToList();
            List<Demand> dipTestData = null;
            if (tankIdList.Any() && storageList.Any() && siteIdList.Any())
            {
                dipTestData = await GetDemands(siteIdList, tankIdList, storageList, sourceTypeId);
            }
            SetTankDetails(jobs, dipTestData, allSitesDemands);
            if (isCreateDR)
            {
                await CheckIfDrExists(allSitesDemands, jobIds, tankIdList, storageList);
            }
            response.Tanks = allSitesDemands;
            return response;
        }

        private async Task<List<Demand>> GetDemands(List<string> siteIdList, List<string> tankIdList, List<string> storageList, int sourceTypeId)
        {
            List<Demand> demands = new List<Demand>();
            try
            {
                //Create SiteIds Table
                DataTable SiteIds, TankIds, StorageIds;
                IntializeDemandsParameters(siteIdList, tankIdList, storageList, out SiteIds, out TankIds, out StorageIds);
                var siteIdParam = new SqlParameter("@SiteList", SqlDbType.Structured);
                siteIdParam.Value = SiteIds;
                siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                var tankIdsParam = new SqlParameter("@TankList", SqlDbType.Structured);
                tankIdsParam.Value = TankIds;
                tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                var storageIdParams = new SqlParameter("@StorageList", SqlDbType.Structured);
                storageIdParams.Value = StorageIds;
                storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                context.Database.CommandTimeout = 60;
                demands = await context.Database.SqlQuery<Demand>("usp_GeDemands @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetDemands", ex.Message, ex);
            }
            return demands;
        }

        private static void IntializeDemandsParameters(List<string> siteIdList, List<string> tankIdList, List<string> storageList, out DataTable SiteIds, out DataTable TankIds, out DataTable StorageIds)
        {
            SiteIds = CreateTable();
            foreach (var item in siteIdList.Distinct().ToList())
            {
                var row = SiteIds.NewRow();
                row["SearchVar"] = item;
                SiteIds.Rows.Add(row);
            }
            //Create TankIds Table
            TankIds = CreateTable();
            foreach (var item in tankIdList.Distinct().ToList())
            {
                var row = TankIds.NewRow();
                row["SearchVar"] = item;
                TankIds.Rows.Add(row);
            }

            //Create StorageIds Table
            StorageIds = CreateTable();
            foreach (var item in storageList.Distinct().ToList())
            {
                var row = StorageIds.NewRow();
                row["SearchVar"] = item;
                StorageIds.Rows.Add(row);
            }
        }

        public async Task CheckIfDrExists(List<ProductModelToCreateDR> demands, List<int> jobIdList, List<string> tankIdList, List<string> storageList)
        {
            try
            {
                var pendingDrForAllTanks = await mdbContext.DeliveryRequests
                   .Find(t => storageList.Contains(t.StorageId) && jobIdList.Contains(t.TfxJobId)
                                && !t.GroupChildDRs.Any()
                               && (t.ScheduleShiftEndDateTime == null || t.ScheduleShiftEndDateTime.Value > DateTimeOffset.UtcNow.AddDays(-2).DateTime)
                               && tankIdList.Contains(t.StorageTypeId) && t.IsActive && !t.IsDeleted
                               && (
                                     (t.CarrierStatus == (int)BrokeredDrCarrierStatus.None || t.CarrierStatus == (int)BrokeredDrCarrierStatus.Recalled) // Supplier
                                     ||
                                     (t.BrokeredParentId != null && t.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted) // Carrier
                               )
                               && (t.Status != DeliveryReqStatus.ScheduleCreated && t.GroupChildDRs.Count() == 0
                                   || (t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed
                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate
                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted
                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Canceled
                                       && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledMissed
                                       && (t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Missed)
                                       && (t.DeliveryRequestType == 0 || t.DeliveryRequestType == 1)))
                                  )
                   .Project(t => new { t.Id, t.IsRecurringSchedule, t.ScheduleQuantityType, t.TfxProductTypeId, t.TfxJobId, t.StorageTypeId, t.StorageId, t.RequiredQuantity, t.Priority, t.TfxScheduleStatusName, t.TfxScheduleStatus, t.CreatedOn, t.ParentId, t.CarrierStatus }).ToListAsync();
                foreach (var demand in demands)
                {
                    var pendingDrList = pendingDrForAllTanks.Where(t => t.TfxJobId == demand.JobId && t.TfxProductTypeId == demand.ProductTypeId).ToList();
                    if (pendingDrList.Any())
                    {
                        demand.IsDRExists = true;
                        pendingDrList.ForEach(t =>
                        {
                            var statusName = string.IsNullOrEmpty(t.TfxScheduleStatusName) ? "Not Published" : t.TfxScheduleStatusName;
                            demand.ExistingDR.Add(new PartialDRDetails
                            {
                                Id = t.Id.ToString(),
                                ScheduleQuantityType = t.ScheduleQuantityType,
                                ScheduleQuantityTypeName = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)t.ScheduleQuantityType),
                                IsRecurringSchedule = t.IsRecurringSchedule,
                                Priority = t.Priority,
                                RequiredQuantity = t.RequiredQuantity,
                                ScheduleStatusName = statusName,
                                ScheduleStatusId = t.ParentId != null ? 11 : t.TfxScheduleStatus,
                                CreatedOn = t.CreatedOn.DateTime.ToString(),
                                CreatedDate = t.CreatedOn,
                                IsMissedDr = !string.IsNullOrEmpty(t.ParentId),
                            });
                        });
                        //if (pendingDrList.Any(x => x.CarrierStatus == (int)BrokeredDrCarrierStatus.Accepted))
                        //{
                        //    demand.CarrierStatus = (int)BrokeredDrCarrierStatus.Accepted;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "CheckIfDrExists", ex.Message, ex);
            }
        }

        public async Task<List<Exchange.Utilities.DropdownDisplayExtendedItem>> GetSiteList(int companyId, string regionId, string siteId, int sourceTypeId)
        {
            List<Exchange.Utilities.DropdownDisplayExtendedItem> response = new List<Exchange.Utilities.DropdownDisplayExtendedItem>();

            var carrierObj = await mdbContext.CarrierJobs.Find(t => t.TfxCarrierCompanyId == companyId && t.IsActive).ToListAsync();

            var carrierJobs = carrierObj.Select(t => t.TfxJobId).ToList();
            List<int> validJobs = carrierJobs;

            if (!string.IsNullOrWhiteSpace(regionId))
            {
                var objectIdForRegion = ObjectId.Parse(regionId);
                var regionDoc = await mdbContext.Regions.Find(t => t.Id == objectIdForRegion).FirstOrDefaultAsync();
                var regionJobs = regionDoc.TfxJobs.Select(t => t.Id).ToList();
                if (carrierJobs.Any())
                    validJobs = regionJobs.Union(carrierJobs).Distinct().ToList();
                else
                    validJobs = regionJobs;
            }

            if (validJobs.Any())
            {
                //change jobId to display or thirparty job id
                var jobDetails = await mdbContext.JobAdditionalDetails.Find(t => validJobs.Contains(t.TfxJobId)).ToListAsync();
                var jobIdWithName = jobDetails.Select(t => new { t.TfxJobId, Name = !string.IsNullOrWhiteSpace(t.TfxDisplayJobId) ? $"{t.TfxJobName}-{t.TfxDisplayJobId}" : t.TfxJobName, t.TfxDisplayJobId }).ToList();

                foreach (var item in jobIdWithName)
                {
                    if (item.Name != null)
                        response.Add(new Exchange.Utilities.DropdownDisplayExtendedItem() { Id = item.TfxJobId, Name = item.Name, Code = item.TfxDisplayJobId });
                }
            }
            return response;
        }

        public async Task<List<CustomerJobForCarrierViewModel>> GetJobListForCarrier(int companyId, string regionId, string siteId, int sourceTypeId)
        {
            var response = new List<CustomerJobForCarrierViewModel>();
            if (!string.IsNullOrWhiteSpace(regionId))
            {
                List<string> regionIds = regionId.Split(',').ToList<string>();
                var objIds = new List<ObjectId>();
                regionIds.ForEach(rId => objIds.Add(ObjectId.Parse(rId)));

                var regionJobs = await mdbContext.Regions.Find(w => objIds.Contains(w.Id))
                                        .Project(t => new { t.Id, Jobs = t.TfxJobs.Select(t1 => t1.Id) })
                                        .ToListAsync();
                var jobIds = new List<int>();
                regionJobs.ForEach(t => jobIds.AddRange(t.Jobs));
                jobIds = jobIds.Distinct().ToList();
                foreach (var jobId in jobIds)
                {
                    var jobRegionId = regionJobs.Where(t => t.Jobs.Contains(jobId)).Select(t => t.Id).FirstOrDefault();
                    var cust = new CustomerJobForCarrierViewModel()
                    {
                        RegionId = Convert.ToString(jobRegionId),
                        Job = new DropdownDisplayExtendedItem { Id = jobId }
                    };
                    response.Add(cust);
                }

                //var jobIds  =new  List<int>();
                //    foreach (var reDoc in regionDocList)
                //    {
                //        var reJobs = reDoc.TfxJobs.Select(t => t.Id).ToList();
                //        jobIds = jobIds.Concat(reJobs).ToList();
                //    }
                //var reJobDetails = await mdbContext.JobAdditionalDetails.Find(t =>
                //              jobIds.Contains(t.TfxJobId)).ToListAsync();
                //   var objectIdForRegion = ObjectId.Parse(regionId);
                //    var regionDoc = await mdbContext.Regions.Find(t => t.Id == objectIdForRegion).FirstOrDefaultAsync();
                //  var regionJobs = regionDoc.TfxJobs.Select(t => t.Id).ToList();
                //  var jobDetails = await mdbContext.JobAdditionalDetails.Find(t =>
                //                 regionJobs.Contains(t.TfxJobId)).ToListAsync();

                //var jobIdWithName = reJobDetails.Select(
                //    t => new
                //    {
                //        t.TfxJobId,
                //        Name = !string.IsNullOrWhiteSpace(t.TfxDisplayJobId) ? $"{t.TfxJobName}-{t.TfxDisplayJobId}" : t.TfxJobName,
                //        t.TfxDisplayJobId,
                //        CompanyName = "1",
                //        CompanyId = 1,

                //    }).Distinct().ToList();

                //foreach (var item in jobIdWithName)
                //{
                //    if (item.Name != null)
                //    {
                //        response.Add(
                //        new CustomerJobForCarrierViewModel()
                //        {
                //            CompanyName = item.CompanyName,
                //            CompanyId = item.CompanyId,
                //            Job = new DropdownDisplayExtendedItem()
                //            {
                //                Id = item.TfxJobId,
                //                Name = item.Name,
                //                Code = item.TfxDisplayJobId
                //            }
                //        });
                //    }
                //}
            }
            return response;
        }

        private static void SetReorderValue(TankDipTestModel site, TankDetail details)
        {
            site.ReorderPercent = details.ThresholdDeliveryRequest ?? 0;
            site.ReorderQuantity = ((details.FuelCapacity ?? 0) * site.ReorderPercent) / 100;
        }

        private decimal CalculateProductThreshold(float netVolume, decimal tankCapacity, decimal productThresholdRequest, decimal productFuelCapacity)
        {
            decimal.TryParse(netVolume.ToString(), out decimal currentVolume);
            if (productThresholdRequest > 0 && productFuelCapacity > 0)
            {
                var current = (currentVolume / productFuelCapacity) * 100;
                if (current > tankCapacity)
                    return 100;
                else
                    return current;
            }
            return 0;
        }

        private decimal CalculateThreshold(TankDipTestModel site, TankDetail tankDetail)
        {
            decimal.TryParse(site.NetVolume.ToString(), out decimal currentVolume);
            if (tankDetail.ThresholdDeliveryRequest.HasValue && tankDetail.ThresholdDeliveryRequest.Value > 0 && tankDetail.FuelCapacity.HasValue && tankDetail.FuelCapacity.Value > 0)
            {
                var current = (currentVolume / tankDetail.FuelCapacity.Value) * 100;
                if (current > site.TankCapacity)
                    return 100;
                else
                    return current;
            }
            return 0;
        }

        private float CalculateUllage(TankDipTestModel site, TankDetail tankDetail)
        {
            decimal.TryParse(site.NetVolume.ToString(), out decimal currentVolume);
            if (currentVolume < 0)
                currentVolume = 0;
            if (tankDetail.ThresholdDeliveryRequest.HasValue && tankDetail.ThresholdDeliveryRequest.Value > 0 && tankDetail.FuelCapacity.HasValue && tankDetail.FuelCapacity.Value > 0)
            {
                var maxThreshold = tankDetail.MaxFill ?? 100;
                var ullage = (tankDetail.FuelCapacity.Value * (maxThreshold) / 100) - currentVolume;

                float.TryParse(ullage.ToString(), out float floatUllage);
                if (ullage < 0)
                    return 0;
                else
                    return floatUllage;
            }
            return 0;
        }

        private static void GetDipTestData(Demand item, TankDipTestModel model)
        {
            if (model == null)
            {
                model = new TankDipTestModel();
            }
            model.DisplayCaptureTime = item.CaptureTime.ToString(Resource.constFormatDateTime);
            model.NetVolume = item.NetVolume;
            model.SiteId = item.SiteId;
            model.StorageId = item.StorageId;
            model.TankId = item.TankId;
            model.Ullage = item.Ullage;
            model.WaterLevel = item.WaterNetLevel;
        }

        private DeliveryReqPriority GetDeliveryRequestPriority(decimal currentInventory, decimal? minFill, int? fillType, decimal? runOutLevel, decimal? fuelCapacity, decimal? reOrderLevel)
        {
            //Make this Function Common which is present in DeliveryRequest Repository
            var response = DeliveryReqPriority.None;
            try
            {
                decimal minFillPercent = 0;
                if (minFill.HasValue && fuelCapacity.HasValue && fillType == (int)FillType.UoM)
                {
                    minFillPercent = (minFill.Value * 100) / fuelCapacity.Value;
                }
                else if (minFill.HasValue && fillType == (int)FillType.Percent)
                {
                    minFillPercent = minFill.Value;
                }

                if (currentInventory <= minFillPercent)
                {
                    response = DeliveryReqPriority.MustGo;
                }
                else if (runOutLevel.HasValue && currentInventory <= runOutLevel)
                {
                    response = DeliveryReqPriority.ShouldGo;
                }
                else if (reOrderLevel.HasValue && currentInventory <= reOrderLevel)
                {
                    response = DeliveryReqPriority.CouldGo;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("DemandCaptureRepository", "GetDeliveryRequestPriority", ex.Message, ex);
            }
            return response;
        }

        private void SetTankDetails(List<JobWithProductsModel> jobs, List<Demand> dipTestData, List<ProductModelToCreateDR> allSitesDemands)
        {
            foreach (var job in jobs)
            {
                foreach (var product in job.Products)
                {
                    ProductModelToCreateDR productModel = new ProductModelToCreateDR() { Tanks = new List<TankDipTestModel>() };
                    productModel.JobId = job.TfxJobId;
                    productModel.SiteId = job.TfxDisplayJobId;
                    productModel.ProductTypeId = product.ProductTypeId;
                    productModel.FuelTypeId = product.ProductTypeId;
                    productModel.ProductName = product.ProductTypeName;
                    productModel.NetVolume = 0;
                    productModel.Priority = DeliveryReqPriority.CouldGo;
                    decimal productThresholdRequest = 0, productFuelCapacity = 0;
                    List<string> tankNames = new List<string>();

                    foreach (var tank in product.Tanks)
                    {
                        TankDipTestModel demand = new TankDipTestModel();
                        demand.AssetId = tank.TfxAssetId;
                        demand.TankMaxFill = tank.MaxFill.HasValue ? tank.MaxFill.Value.GetPreciseValue(2) : 0;
                        demand.TankCapacity = tank.FuelCapacity ?? 0;
                        demand.TankName = tank.TankName;
                        tankNames.Add(tank.TankName);
                        productModel.TankId = demand.TankId = tank.StorageTypeId;
                        productModel.StorageId = demand.StorageId = tank.StorageId;
                        demand.DisplayCaptureTime = DateTimeOffset.Now.ToString(Resource.constFormatDateTime);
                        demand.ReorderPercent = tank.ThresholdDeliveryRequest.HasValue ? tank.ThresholdDeliveryRequest.Value.GetPreciseValue(2) : 0;
                        demand.ReorderQuantity = ((tank.FuelCapacity ?? 0) * demand.ReorderPercent) / 100;
                        demand.NetVolume = 0;
                        if (dipTestData != null && !string.IsNullOrWhiteSpace(job.TfxDisplayJobId) && !string.IsNullOrWhiteSpace(tank.StorageId) && !string.IsNullOrWhiteSpace(tank.StorageTypeId))
                        {
                            var dipTest = dipTestData.FirstOrDefault(t => t.SiteId == job.TfxDisplayJobId && t.StorageId == tank.StorageId && t.TankId == tank.StorageTypeId);
                            if (dipTest != null)
                            {
                                GetDipTestData(dipTest, demand);
                                demand.CurrentThreshold = CalculateThreshold(demand, tank);
                                var definedPrioirity = GetDeliveryRequestPriority(demand.CurrentThreshold, tank.MinFill, (int)tank.FillType, tank.RunOutLevel, tank.FuelCapacity, tank.ThresholdDeliveryRequest);
                                demand.Priority = definedPrioirity == DeliveryReqPriority.None ? DeliveryReqPriority.CouldGo : definedPrioirity;
                                demand.Ullage = demand.Ullage > 0 ? demand.Ullage : CalculateUllage(demand, tank);
                                SetReorderValue(demand, tank);
                            }
                        }
                        if (tank.FillType == (int)FillType.Percent)
                        {
                            demand.TankMaxFill = (tank.MaxFill.HasValue && tank.FuelCapacity.HasValue) ? (tank.MaxFill.Value * (tank.FuelCapacity ?? 0) / 100).GetPreciseValue(2) : 0;
                        }

                        if (demand.NetVolume > 0)
                        {
                            productModel.NetVolume += demand.NetVolume;
                        }
                        productModel.TankCapacity += demand.TankCapacity;
                        if (tank.ThresholdDeliveryRequest.HasValue)
                        {
                            productThresholdRequest += tank.ThresholdDeliveryRequest.Value;
                        }
                        if (tank.FuelCapacity.HasValue)
                        {
                            productFuelCapacity += tank.FuelCapacity.Value;
                        }
                        productModel.CurrentThreshold = CalculateProductThreshold(productModel.NetVolume, productModel.TankCapacity, productThresholdRequest, productFuelCapacity);
                        productModel.ReorderQuantity += demand.ReorderQuantity;
                        productModel.TankMaxFill += demand.TankMaxFill;
                        productModel.Priority = demand.Priority != DeliveryReqPriority.None && demand.Priority < productModel.Priority ? demand.Priority : productModel.Priority;
                        productModel.Ullage += demand.Ullage;
                        productModel.Tanks.Add(demand);
                    }

                    productModel.TankName = string.Join(", ", tankNames);
                    allSitesDemands.Add(productModel);
                }
            }
        }

        public async Task<List<TankModel>> GetProcessTanks(DipTestMethod dipTestMethod)
        {
            var response = new List<TankModel>();
            var result = await mdbContext.JobAdditionalDetails
                                .Find(t => t.TfxDisplayJobId != null && t.Tanks != null && t.IsActive
                                && t.Tanks.Any(c => c.DipTestMethod == (int)dipTestMethod && !c.IsStopATGPolling))
                                .Project(t1 => new
                                {
                                    TanksList = t1.Tanks.Where(w => w.DipTestMethod == (int)dipTestMethod && !w.IsStopATGPolling)
                                        .Select(t2 => new TankModel
                                        {
                                            SiteId = t1.TfxDisplayJobId,
                                            StorageId = t2.StorageId,
                                            TankId = t2.StorageTypeId
                                        })
                                }).ToListAsync();
            if (result.AnyAndNotNull())
            {
                response = result.SelectMany(t => t.TanksList).Distinct().ToList();
            }
            return response;
        }
        public async Task<bool> ProcessPedigreeTanks(PedegreeConfigurationModel config)
        {
            bool response = false;
            try
            {
                var currentDateTime = DateTime.Now;
                var result = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxDisplayJobId != null && t.Tanks != null
                                                && t.Tanks.Any(c => c.DipTestMethod == (int)DipTestMethod.Pedigree && !c.IsStopATGPolling && c.PedigreeAssetDBID != null && !String.IsNullOrEmpty(c.PedigreeAssetDBID))
                                                )
                                                .Project(t1 => new
                                                {
                                                    DemandModelList = t1.Tanks.Where(w => w.DipTestMethod == (int)DipTestMethod.Pedigree && !w.IsStopATGPolling && w.PedigreeAssetDBID != null && !String.IsNullOrEmpty(w.PedigreeAssetDBID)).Select(t2 => new DemandModel
                                                    {
                                                        SiteId = t1.TfxDisplayJobId,
                                                        TankId = t2.StorageTypeId,
                                                        StorageId = t2.StorageId,
                                                        CaptureTime = Convert.ToDateTime(currentDateTime),
                                                        ProductId = t2.TfxProductTypeId,
                                                        GrossVolume = 0,
                                                        NetVolume = 0,
                                                        DipTestValue = 0,
                                                        Ullage = 0,
                                                        Level = 0,
                                                        WaterGrossLevel = 0,
                                                        WaterNetLevel = 0,
                                                        DataSourceTypeId = (int)DipTestMethod.Pedigree,
                                                        PedigreeAssetDBID = t2.PedigreeAssetDBID,
                                                        TankMaxFill = t2.MaxFill != null ? t2.MaxFill.Value : 100,
                                                        TankCapacity = t2.FuelCapacity.Value,
                                                        FillType = t2.FillType,
                                                        JobId = t1.TfxJobId
                                                    }).ToList()
                                                })
                                                .ToListAsync();

                if (result != null && result.Any())
                {

                    if (config != null && config.ProductTypeList.Any())
                    {
                        List<DemandModel> demandModelList = new List<DemandModel>();

                        var tasks = new List<Task<PedegreeResponseModel>>();
                        HttpClient MyHttpClient = new HttpClient(); // create single instance of httpClient, why ? https://learnbyinsight.com/2020/06/29/httpclient-single-instance-or-multiple/
                        MyHttpClient.Timeout = TimeSpan.FromSeconds(100);
                        var byteArray = Encoding.ASCII.GetBytes(config.PedigreeUserId + ":" + config.PedigreePassword);
                        MyHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                        foreach (var pedgreeJob in result)
                        {
                            if (pedgreeJob != null && pedgreeJob.DemandModelList.Any())
                            {
                                foreach (var tank in pedgreeJob.DemandModelList)
                                {
                                    tasks.Add(GetPedgreeResponse(config, tank.PedigreeAssetDBID, MyHttpClient));
                                }
                            }
                        }
                        //WhenAll Benift 
                        //1. use WhenAll because it propagates all errors at once. With the multiple awaits, you lose errors if one of 
                        //   the earlier awaits throws.
                        //2. Another important difference is that WhenAll will wait for all tasks to complete even in the presence of 
                        //failures (faulted or canceled tasks). Awaiting manually in sequence would cause unexpected concurrency 
                        //because the part of your program that wants to wait will actually continue early.
                        var responses = await Task.WhenAll(tasks);

                        foreach (var res in responses)
                        {
                            var findTankList = result.SelectMany(t => t.DemandModelList.Where(p => p.PedigreeAssetDBID == res.PedegreeId)).ToList();
                            // default expectation will one record, in case same pdgree id for mulitple tank below code will ensure to update all tanks
                            foreach (var item in findTankList)
                            {
                                ProcessPedigreeResponse(res, item, config, item.JobId, demandModelList);
                            }

                        }
                        if (demandModelList != null && demandModelList.Any())
                        {
                            await CreateDemand(demandModelList, 0, null);
                        }
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "ProcessPedigreeTanks", ex.Message, ex);
                return false;
            }

        }

        private async Task<PedegreeResponseModel> GetPedgreeResponse(PedegreeConfigurationModel config, string PedegreeId, HttpClient client)
        {
            var response = new PedegreeResponseModel();
            try
            {

                var url = config.PedigreeGetAssetVolumnUrl.Replace("{actorId}", PedegreeId);
                HttpResponseMessage apiResponse = await client.GetAsync(url);

                if (apiResponse.IsSuccessStatusCode)
                {
                    var responseString = await apiResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<PedegreeResponseModel>(responseString);
                    response.PedegreeId = PedegreeId;
                }

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetPedgreeResponse", ex.Message, ex);
            }
            return response;
        }


        private void ProcessPedigreeResponse(PedegreeResponseModel res, DemandModel tank, PedegreeConfigurationModel config,
            int JobId, List<DemandModel> demandModelList)
        {

            if (res != null && res.ReadingList != null && res.ReadingList.NameValuePairs != null && res.ReadingList.NameValuePairs.Any())
            {
                var list = res.ReadingList.NameValuePairs;
                var volDtl = list.Find(w => w.Name == "volume");
                if (volDtl != null && volDtl.Value != null)
                {
                    tank.GrossVolume = float.Parse(volDtl.Value);
                    tank.NetVolume = tank.GrossVolume;
                }

                //var ullageDtl = list.Find(w => w.Name == "fillUllage");
                //if (ullageDtl != null && ullageDtl.Value !=null)
                //{
                //    tank.Ullage = float.Parse(ullageDtl.Value);
                //}

                var waterDtl = list.Find(w => w.Name == "water");
                if (waterDtl != null && waterDtl.Value != null)
                {
                    tank.WaterGrossLevel = float.Parse(waterDtl.Value);
                    tank.WaterNetLevel = tank.WaterGrossLevel;
                }
                tank.DipTestValue = tank.GrossVolume;

                tank.ProductName = config.ProductTypeList.Where(w => w.Id == tank.ProductId).Select(s => s.Name).FirstOrDefault();

                if (config.JobUOMList != null && config.JobUOMList.Any() && Convert.ToInt32(config.JobUOMList.Find(x => x.Id == JobId).UoM) == (int)UoM.Gallons)
                    tank.DipTestUoM = TankScaleMeasurement.Gallons;
                else
                    tank.DipTestUoM = TankScaleMeasurement.Litres;

                //Time zone
                var jobTimeZone = config.JobUOMList.Find(x => x.Id == JobId).TimeZoneName;
                if (!String.IsNullOrEmpty(jobTimeZone) && !String.IsNullOrEmpty(res.Time))
                {
                    var currentDate = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(res.Time));
                    currentDate = currentDate.ToTargetDateTimeOffset(jobTimeZone);
                    tank.CaptureTime = currentDate.DateTime;
                }
                //cal ullage
                if (tank.FillType == (int)FillType.UoM)
                {
                    var maxFillPercent = (tank.TankMaxFill * 100) / tank.TankCapacity;
                    maxFillPercent = maxFillPercent / 100;
                    var ullg = (maxFillPercent * tank.TankCapacity) - Convert.ToDecimal(tank.DipTestValue);
                    tank.Ullage = float.Parse(ullg.ToString());
                }
                else
                {
                    var maxFillPercent = tank.TankMaxFill / 100;
                    var ullg = (maxFillPercent * tank.TankCapacity) - Convert.ToDecimal(tank.DipTestValue);
                    tank.Ullage = float.Parse(ullg.ToString());
                }



                demandModelList.Add(tank);
            }
        }



        private async Task<PedegreeConfigurationModel> GetPegedreeConfigurationInfo(string pedigreeIds)
        {
            var pedgreeConfig = new PedegreeConfigurationModel();
            try
            {
                var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemandCaptureConnectionString"].ConnectionString);
                var command = connection.CreateCommand();
                command.CommandText = "[dbo].[Usp_GETPedegreeConnectionInfo]";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pedigreeIds", pedigreeIds);
                command.CommandTimeout = 30;
                connection.Open();
                var reader = await command.ExecuteReaderAsync();

                pedgreeConfig = ((IObjectContextAdapter)context).ObjectContext.Translate<PedegreeConfigurationModel>(reader).FirstOrDefault();
                reader.NextResult();

                pedgreeConfig.ProductTypeList = ((IObjectContextAdapter)context).ObjectContext.Translate<FreightModels.DropdownDisplayItem>(reader).ToList();
                reader.NextResult();
                pedgreeConfig.JobUOMList = ((IObjectContextAdapter)context).ObjectContext.Translate<FreightModels.JobUOMDropDwn>(reader).ToList();
                reader.Close();
                command.Dispose();
                connection.Close();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetPegedreeConfigurationInfo", ex.Message, ex);
            }
            return pedgreeConfig;
        }
        public async Task<List<CustomerJobForCarrierViewModel>> GetBrokerJobListForCarrier(int companyId, string regionId)
        {
            var response = new List<CustomerJobForCarrierViewModel>();
            if (!string.IsNullOrWhiteSpace(regionId))
            {
                List<string> regionIds = regionId.Split(',').ToList<string>();
                var objIds = new List<ObjectId>();
                regionIds.ForEach(rId => objIds.Add(ObjectId.Parse(rId)));

                var regionJobs = await mdbContext.Regions.Find(w => objIds.Contains(w.Id))
                                        .Project(t => t.TfxJobs.Select(t1 => t1.Id))
                                        .ToListAsync();
                var jobIds = new List<int>();
                regionJobs.ForEach(t => jobIds.AddRange(t));
                jobIds = jobIds.Distinct().ToList();
                foreach (var jobId in jobIds)
                {
                    var cust = new CustomerJobForCarrierViewModel()
                    {
                        Job = new DropdownDisplayExtendedItem { Id = jobId }
                    };
                    response.Add(cust);
                }
            }
            if (companyId > 0)
            {

                var brokeredDRJobs = await mdbContext.BrokeredDRJobs.Find(w => w.TfxAssignToCompanyId == companyId && w.IsActive && !w.IsDeleted)
                                        .Project(t => t.TfxJobId)
                                        .ToListAsync();
                var jobIds = new List<int>();
                brokeredDRJobs.ForEach(t => jobIds.Add(t));
                jobIds = jobIds.Distinct().ToList();
                foreach (var jobId in jobIds)
                {
                    var cust = new CustomerJobForCarrierViewModel()
                    {
                        Job = new DropdownDisplayExtendedItem { Id = jobId }
                    };
                    response.Add(cust);
                }

            }
            return response.Distinct().ToList();
        }
        public async Task<List<int>> GetBrokerJobOrderDetails(int companyId, List<int> OrderId)
        {
            var response = new List<int>();
            if (OrderId.Count > 0 && companyId > 0)
            {
                var brokeredDRJobs = await mdbContext.BrokeredDRJobs.Find(w => w.TfxAssignToCompanyId == companyId && OrderId.Contains(w.TfxOrderId.Value) && w.TfxOrderId != null && w.IsActive && !w.IsDeleted)
                                           .Project(t => t.TfxOrderId)
                                           .ToListAsync();
                brokeredDRJobs.ForEach(t => response.Add(t.Value));
            }
            return response.Distinct().ToList();
        }

        //skybitz Datasource
        public async Task<List<DemandModel>> GetTropicOilTanksDemandModel(List<TropicOilCompanyDemandModel> tropicOilList, List<DropdownDisplayExtendedItem> jobWithTimezone)
        {
            List<DemandModel> demandModelList = new List<DemandModel>();
            try
            {

                var currentDateTime = DateTime.Now;
                var result = await mdbContext.JobAdditionalDetails
                                                       .Find(t => t.TfxDisplayJobId != null && t.Tanks != null
                                                       && t.Tanks.Any(c => c.DipTestMethod == (int)DipTestMethod.Skybitz && !c.IsStopATGPolling && c.SkyBitzRTUID != null && !String.IsNullOrEmpty(c.SkyBitzRTUID))
                                                       )
                                                       .Project(t1 => new
                                                       {
                                                           DemandModelList = t1.Tanks.Where(w => w.DipTestMethod == (int)DipTestMethod.Skybitz && !w.IsStopATGPolling && w.SkyBitzRTUID != null && !String.IsNullOrEmpty(w.SkyBitzRTUID)).Select(t2 => new DemandModel
                                                           {
                                                               SiteId = t1.TfxDisplayJobId,
                                                               TankId = t2.StorageTypeId,
                                                               StorageId = t2.StorageId,
                                                               CaptureTime = Convert.ToDateTime(currentDateTime),
                                                               ProductId = t2.TfxProductTypeId,
                                                               GrossVolume = 0,
                                                               NetVolume = 0,
                                                               DipTestValue = 0,
                                                               Ullage = 0,
                                                               Level = 0,
                                                               WaterGrossLevel = 0,
                                                               WaterNetLevel = 0,
                                                               DataSourceTypeId = (int)DipTestMethod.Skybitz,
                                                               SkyBitzRTUID = t2.SkyBitzRTUID,
                                                               TankMaxFill = t2.MaxFill != null ? t2.MaxFill.Value : 100,
                                                               TankCapacity = t2.FuelCapacity.Value,
                                                               FillType = t2.FillType,
                                                               JobId = t1.TfxJobId
                                                           }).ToList()
                                                       })
                                                       .ToListAsync();

                foreach (var job in result)
                {
                    if (job != null && job.DemandModelList.Any())
                    {
                        foreach (var item in job.DemandModelList)
                        {
                            var tropicOilDemand = tropicOilList.Where(w => w.RTUID == item.SkyBitzRTUID).FirstOrDefault();
                            if (tropicOilDemand != null)
                            {
                                item.GrossVolume = String.IsNullOrEmpty(tropicOilDemand.Inventory) ? 0 : float.Parse(tropicOilDemand.Inventory);
                                item.NetVolume = String.IsNullOrEmpty(tropicOilDemand.Inventory) ? 0 : float.Parse(tropicOilDemand.Inventory);
                                item.DipTestValue = String.IsNullOrEmpty(tropicOilDemand.Inventory) ? 0 : float.Parse(tropicOilDemand.Inventory);
                                item.Level = String.IsNullOrEmpty(tropicOilDemand.Level) ? 0 : Convert.ToDecimal(tropicOilDemand.Level);
                                item.DipTestUoM = TankScaleMeasurement.Gallons;
                                item.ProductName = tropicOilDemand.Product;
                                //Time zone
                                var jobTimeZone = jobWithTimezone.Find(x => x.Id == item.JobId).Name;//timezone
                                if (!String.IsNullOrEmpty(jobTimeZone) && !String.IsNullOrEmpty(tropicOilDemand.InventoryTimeUTC))
                                {
                                    DateTime dt;
                                    DateTime.TryParse(tropicOilDemand.InventoryTimeUTC, out dt);
                                    if (dt != DateTime.MinValue)
                                    {
                                        DateTimeOffset currentDate = Convert.ToDateTime(dt);
                                        currentDate = currentDate.ToTargetDateTimeOffset(jobTimeZone);
                                        item.CaptureTime = currentDate.DateTime;
                                    }
                                    else
                                        item.CaptureTime = DateTime.Now;

                                }

                                //cal ullage
                                if (item.FillType == (int)FillType.UoM)
                                {
                                    var maxFillPercent = (item.TankMaxFill * 100) / item.TankCapacity;
                                    maxFillPercent = maxFillPercent / 100;
                                    var ullg = (maxFillPercent * item.TankCapacity) - Convert.ToDecimal(item.DipTestValue);
                                    item.Ullage = float.Parse(ullg.ToString());
                                }
                                else
                                {
                                    var maxFillPercent = item.TankMaxFill / 100;
                                    var ullg = (maxFillPercent * item.TankCapacity) - Convert.ToDecimal(item.DipTestValue);
                                    item.Ullage = float.Parse(ullg.ToString());
                                }

                                demandModelList.Add(item);
                            }
                        }
                    }
                }
                return demandModelList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<DemandModel>> GetTropicOilAPITanksDemandModel(List<Inventory> tropicOilList, List<DropdownDisplayExtendedItem> jobWithTimezone)
        {
            List<DemandModel> demandModelList = new List<DemandModel>();
            try
            {

                var currentDateTime = DateTime.Now;
                var result = await mdbContext.JobAdditionalDetails
                                                       .Find(t => t.TfxDisplayJobId != null && t.Tanks != null
                                                       && t.Tanks.Any(c => c.SkyBitzRTUID != null && !String.IsNullOrEmpty(c.SkyBitzRTUID))
                                                       )
                                                       .Project(t1 => new
                                                       {
                                                           DemandModelList = t1.Tanks.Where(w => w.SkyBitzRTUID != null && !String.IsNullOrEmpty(w.SkyBitzRTUID)).Select(t2 => new DemandModel
                                                           {
                                                               SiteId = t1.TfxDisplayJobId,
                                                               TankId = t2.StorageTypeId,
                                                               StorageId = t2.StorageId,
                                                               CaptureTime = Convert.ToDateTime(currentDateTime),
                                                               ProductId = t2.TfxProductTypeId,
                                                               GrossVolume = 0,
                                                               NetVolume = 0,
                                                               DipTestValue = 0,
                                                               Ullage = 0,
                                                               Level = 0,
                                                               WaterGrossLevel = 0,
                                                               WaterNetLevel = 0,
                                                               DataSourceTypeId = (int)DipTestMethod.Skybitz,
                                                               SkyBitzRTUID = t2.SkyBitzRTUID,
                                                               TankMaxFill = t2.MaxFill != null ? t2.MaxFill.Value : 100,
                                                               TankCapacity = t2.FuelCapacity.Value,
                                                               FillType = t2.FillType,
                                                               JobId = t1.TfxJobId
                                                           }).ToList()
                                                       })
                                                       .ToListAsync();

                foreach (var job in result)
                {
                    if (job != null && job.DemandModelList.Any())
                    {
                        foreach (var item in job.DemandModelList)
                        {
                            var tropicOilDemand = tropicOilList.Where(w => w.sSerialNumber == item.SkyBitzRTUID).FirstOrDefault();
                            if (tropicOilDemand != null)
                            {
                                item.GrossVolume = String.IsNullOrEmpty(tropicOilDemand.dGrossVolume) ? 0 : float.Parse(tropicOilDemand.dGrossVolume);
                                item.NetVolume = String.IsNullOrEmpty(tropicOilDemand.dNetVolume) ? 0 : float.Parse(tropicOilDemand.dNetVolume);
                                item.DipTestValue = String.IsNullOrEmpty(tropicOilDemand.dNetVolume) ? 0 : float.Parse(tropicOilDemand.dNetVolume);
                                item.Level = String.IsNullOrEmpty(tropicOilDemand.dLevel) ? 0 : Convert.ToDecimal(tropicOilDemand.dLevel);
                                item.DipTestUoM = TankScaleMeasurement.Gallons;
                                item.ProductName = tropicOilDemand.sProductName;
                                //Time zone
                                var jobTimeZone = jobWithTimezone.Find(x => x.Id == item.JobId).Name;//timezone
                                if (!String.IsNullOrEmpty(jobTimeZone) && !String.IsNullOrEmpty(tropicOilDemand.sUTCInventoryTime))
                                {
                                    DateTimeOffset currentDate = Convert.ToDateTime(tropicOilDemand.sUTCInventoryTime);
                                    currentDate = currentDate.ToTargetDateTimeOffset(jobTimeZone);
                                    item.CaptureTime = currentDate.DateTime;
                                }

                                //cal ullage
                                if (item.FillType == (int)FillType.UoM)
                                {
                                    var maxFillPercent = (item.TankMaxFill * 100) / item.TankCapacity;
                                    maxFillPercent = maxFillPercent / 100;
                                    var ullg = (maxFillPercent * item.TankCapacity) - Convert.ToDecimal(item.DipTestValue);
                                    item.Ullage = float.Parse(ullg.ToString());
                                }
                                else
                                {
                                    var maxFillPercent = item.TankMaxFill / 100;
                                    var ullg = (maxFillPercent * item.TankCapacity) - Convert.ToDecimal(item.DipTestValue);
                                    item.Ullage = float.Parse(ullg.ToString());
                                }

                                demandModelList.Add(item);
                            }
                        }
                    }
                }
                return demandModelList;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetTropicOilAPITanksDemandModel", ex.Message, ex);
                throw;
            }

        }
        //is360 Datasource
        public async Task<List<DemandModel>> GetIS360TanksDemandModel(List<Is360DemandModel> Is360DemandList, ExternalTankConfigurationModel model)
        {
            List<DemandModel> demandModelList = new List<DemandModel>();
            try
            {
                var currentDateTime = DateTime.Now;
                var result = await mdbContext.JobAdditionalDetails
                                                       .Find(t => t.TfxDisplayJobId != null && t.Tanks != null
                                                       && t.Tanks.Any(c => c.DipTestMethod == (int)DipTestMethod.Insight360 && !c.IsStopATGPolling && c.ExternalTankId != null)
                                                       )
                                                       .Project(t1 => new
                                                       {
                                                           DemandModelList = t1.Tanks.Where(w => w.DipTestMethod == (int)DipTestMethod.Insight360 && !w.IsStopATGPolling && w.ExternalTankId != null).Select(t2 => new DemandModel
                                                           {
                                                               SiteId = t1.TfxDisplayJobId,
                                                               TankId = t2.StorageTypeId,
                                                               StorageId = t2.StorageId,
                                                               CaptureTime = Convert.ToDateTime(currentDateTime),
                                                               ProductId = t2.TfxProductTypeId,
                                                               GrossVolume = 0,
                                                               NetVolume = 0,
                                                               DipTestValue = 0,
                                                               Ullage = 0,
                                                               Level = 0,
                                                               WaterGrossLevel = 0,
                                                               WaterNetLevel = 0,
                                                               DataSourceTypeId = (int)DipTestMethod.Insight360,
                                                               ExternalTankId = t2.ExternalTankId,
                                                               TankMaxFill = t2.MaxFill != null ? t2.MaxFill.Value : 100,
                                                               TankCapacity = t2.FuelCapacity.Value,
                                                               FillType = t2.FillType,
                                                               JobId = t1.TfxJobId
                                                           }).ToList()
                                                       })
                                                       .ToListAsync();

                if (result != null)
                {
                    string formatString = "yyyyMMddHHmmss";
                    foreach (var job in result)
                    {
                        if (job != null && job.DemandModelList.Any())
                        {
                            foreach (var item in job.DemandModelList)
                            {
                                var Is360Demand = Is360DemandList.Where(w => w.TankLegacyId == item.ExternalTankId).FirstOrDefault();
                                if (Is360Demand != null)
                                {
                                    item.GrossVolume = String.IsNullOrEmpty(Is360Demand.InventoryVolume) ? 0 : float.Parse(Is360Demand.InventoryVolume);
                                    item.NetVolume = String.IsNullOrEmpty(Is360Demand.InventoryVolume) ? 0 : float.Parse(Is360Demand.InventoryVolume);
                                    item.DipTestValue = String.IsNullOrEmpty(Is360Demand.InventoryVolume) ? 0 : float.Parse(Is360Demand.InventoryVolume);
                                    item.WaterNetLevel = String.IsNullOrEmpty(Is360Demand.WaterLevel) ? 0 : float.Parse(Is360Demand.WaterLevel);
                                    item.WaterGrossLevel = item.WaterNetLevel;
                                    item.DipTestUoM = TankScaleMeasurement.Gallons;
                                    item.ProductName = model.ProductTypeList.Where(w => w.Id == item.ProductId).Select(s => s.Name).FirstOrDefault();
                                    //Time zone
                                    if (!String.IsNullOrEmpty(Is360Demand.InventoryReadingDate))
                                    {
                                        DateTime dt = DateTime.ParseExact(Is360Demand.InventoryReadingDate, formatString, null);
                                        item.CaptureTime = dt;
                                    }
                                    //var jobTimeZone = model.JobUOMTimeZoneList.Find(x => x.Id == item.JobId).TimeZoneName;
                                    //if (!String.IsNullOrEmpty(jobTimeZone) && !String.IsNullOrEmpty(Is360Demand.InventoryReadingDate))
                                    //{
                                    //    var currentDate = DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(Is360Demand.InventoryReadingDate));
                                    //    currentDate = currentDate.ToTargetDateTimeOffset(jobTimeZone);
                                    //    item.CaptureTime = currentDate.DateTime;
                                    //}

                                    //cal ullage
                                    if (item.FillType == (int)FillType.UoM)
                                    {
                                        var maxFillPercent = (item.TankMaxFill * 100) / item.TankCapacity;
                                        maxFillPercent = maxFillPercent / 100;
                                        var ullg = (maxFillPercent * item.TankCapacity) - Convert.ToDecimal(item.DipTestValue);
                                        item.Ullage = float.Parse(ullg.ToString());
                                    }
                                    else
                                    {
                                        var maxFillPercent = item.TankMaxFill / 100;
                                        var ullg = (maxFillPercent * item.TankCapacity) - Convert.ToDecimal(item.DipTestValue);
                                        item.Ullage = float.Parse(ullg.ToString());
                                    }
                                    demandModelList.Add(item);
                                }
                            }
                        }
                    }
                }
                return demandModelList;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureDomain", "GetIS360TanksDemandModel", ex.Message, ex);

            }
            return demandModelList;
        }

        //vedor root
        public async Task<bool> ProcessVedorRootTanks()
        {
            try
            {
                //tfx job with tanks
                var jobs = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxDisplayJobId != null && t.Tanks != null
                                                && t.Tanks.Any(c => c.DipTestMethod == (int)DipTestMethod.VeederRoot && !c.IsStopATGPolling && c.ExternalTankId != null && c.Port != null && c.VeederRootIPAddress != null)
                                                )
                                                .Project(t1 => new
                                                {
                                                    JobId = t1.TfxJobId,
                                                    DemandModelList = t1.Tanks.Where(c => !c.IsStopATGPolling && c.DipTestMethod == (int)DipTestMethod.VeederRoot && c.ExternalTankId != null && c.Port != null && c.VeederRootIPAddress != null).Select(t2 => new DemandModel
                                                    {
                                                        SiteId = t1.TfxDisplayJobId,
                                                        TankId = t2.StorageTypeId,
                                                        StorageId = t2.StorageId,
                                                        // CaptureTime = Convert.ToDateTime(currentDateTime),
                                                        ProductId = t2.TfxProductTypeId,
                                                        GrossVolume = 0,
                                                        NetVolume = 0,
                                                        DipTestValue = 0,
                                                        Ullage = 0,
                                                        Level = 0,
                                                        WaterGrossLevel = 0,
                                                        WaterNetLevel = 0,
                                                        DataSourceTypeId = (int)DipTestMethod.VeederRoot,
                                                        ExternalTankId = t2.ExternalTankId,
                                                        TankMaxFill = t2.MaxFill != null ? t2.MaxFill.Value : 100,
                                                        TankCapacity = t2.FuelCapacity.Value,
                                                        FillType = t2.FillType,
                                                        JobId = t1.TfxJobId,
                                                        VeederRootIPAddress = t2.VeederRootIPAddress,
                                                        Port = t2.Port

                                                    }).ToList()
                                                })
                                                .ToListAsync();

                if (jobs != null && jobs.Any())
                {
                    List<DemandModel> demandModelList = new List<DemandModel>();
                    var tasks = new List<Task<VedorRootResponse>>();

                    var duplicateIPAddressJobIds = new List<string>();
                    List<VedorRootResponse> duplicateIpsResponse = new List<VedorRootResponse>();
                    foreach (var Job in jobs)
                    {
                        if (Job != null && Job.DemandModelList.Any())
                        {
                            var config = Job.DemandModelList.Where(w => w.VeederRootIPAddress != null && w.Port != null).FirstOrDefault();
                            if (!duplicateIPAddressJobIds.Contains(config.VeederRootIPAddress))
                            {
                                tasks.Add(GetVedorRootResponse(config));
                                duplicateIPAddressJobIds.Add(config.VeederRootIPAddress);
                            }
                            else
                            {
                                duplicateIpsResponse.Add(new VedorRootResponse() { TFXJobId = config.JobId, IP = config.VeederRootIPAddress });
                            }

                        }
                    }
                    var JobsWithTanksResult = await Task.WhenAll(tasks);
                    var response = new List<VedorRootResponse>();

                    if (JobsWithTanksResult != null && JobsWithTanksResult.Any())
                        response.AddRange(JobsWithTanksResult.ToList());

                    foreach (var item in duplicateIpsResponse)
                    {
                        VedorRootResponse dJob = JobsWithTanksResult.Where(t => t.IP == item.IP).FirstOrDefault();
                        if (dJob != null)
                        {
                            response.Add(new VedorRootResponse() { TFXJobId = item.TFXJobId, LastInventoryDate = dJob.LastInventoryDate, Tanks = dJob.Tanks });
                        }
                    }
                    //Response from vedor root
                    foreach (var item in response)
                    {
                        var jobwithTanks = jobs.Where(w => w.JobId == item.TFXJobId).FirstOrDefault();
                        if (jobwithTanks != null && jobwithTanks.DemandModelList.Any() && item != null && item.Tanks.Any())
                        {
                            foreach (var demand in jobwithTanks.DemandModelList)
                            {
                                var responseTnk = item.Tanks.Where(w => w.TankId == demand.ExternalTankId).FirstOrDefault();
                                if (responseTnk != null)
                                {
                                    demand.GrossVolume = float.Parse(responseTnk.CurrentInventory);
                                    demand.NetVolume = demand.GrossVolume;
                                    demand.DipTestValue = demand.GrossVolume;
                                    DateTime dt;
                                    DateTime.TryParse(item.LastInventoryDate, out dt);
                                    if (dt != DateTime.MinValue)
                                    {
                                        demand.CaptureTime = dt;
                                    }
                                    else
                                        //demand.CaptureTime =Convert.ToDateTime(item.LastInventoryDate);
                                        demand.CaptureTime = DateTime.Now;
                                    //uom
                                    demand.DipTestUoM = TankScaleMeasurement.Gallons;
                                    //cal ullage
                                    if (demand.FillType == (int)FillType.UoM)
                                    {
                                        var maxFillPercent = (demand.TankMaxFill * 100) / demand.TankCapacity;
                                        maxFillPercent = maxFillPercent / 100;
                                        var ullg = (maxFillPercent * demand.TankCapacity) - Convert.ToDecimal(demand.DipTestValue);
                                        demand.Ullage = float.Parse(ullg.ToString());
                                    }
                                    else
                                    {
                                        var maxFillPercent = demand.TankMaxFill / 100;
                                        var ullg = (maxFillPercent * demand.TankCapacity) - Convert.ToDecimal(demand.DipTestValue);
                                        demand.Ullage = float.Parse(ullg.ToString());
                                    }
                                    demandModelList.Add(demand);
                                }
                            }

                        }

                    }
                    if (demandModelList != null && demandModelList.Any())
                    {
                        await CreateDemand(demandModelList, 0, null);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "ProcessVedorRootTanks", ex.Message, ex);
                return false;
            }

        }
        private async Task<VedorRootResponse> GetVedorRootResponse(DemandModel config)
        {
            var response = new VedorRootResponse();
            try
            {
                if (int.TryParse(config.Port, out int portNum))
                {
                    using (var client = new Client(config.VeederRootIPAddress, portNum, new System.Threading.CancellationToken()))
                    {
                        await client.Write("\x01");
                        await client.Write("200");
                        Thread.Sleep(3000);
                        var data = await client.ReadAsync();
                        Thread.Sleep(12000);
                        Console.WriteLine(data);
                        using (StringReader reader = new StringReader(data))
                        {
                            response.TFXJobId = config.JobId;
                            response.IP = config.VeederRootIPAddress;
                            int i = 0;
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                //read row
                                if (!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                                {

                                    if (i == 9)//get capture date
                                        response.LastInventoryDate = line;
                                    string[] words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                    int wordsCount = words.Count();
                                    //read col in row
                                    if (wordsCount >= 7 && int.TryParse(words[0], out _))
                                    {
                                        var tank = new VedorRootTanks();
                                        tank.TankId = words[0];
                                        StringBuilder pName = new StringBuilder();
                                        for (int j = 1; j < wordsCount - 5; j++) // soluttion for product name contaning space like dsl 1 , dsl 2
                                        {
                                            if (j > 1)
                                            { pName.Append(" "); }
                                            pName.Append(words[j]);
                                        }
                                        tank.ProductName = pName.ToString();

                                        for (int x = wordsCount - 5; x <= wordsCount; x++)
                                        {
                                            if (decimal.TryParse(words[x], out _))
                                            {
                                                tank.CurrentInventory = words[x];
                                                break;
                                            }
                                            else
                                                tank.ProductName = tank.ProductName + " " + words[x];

                                        }
                                        response.Tanks.Add(tank);
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DemandCaptureRepository", "GetVedorRootResponse", ex.Message, ex);
            }
            return response;
        }
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SearchVar", typeof(string));
            return dt;
        }
    }
}

public class JobWithTanksModel
{
    public int TfxJobId { get; set; }
    public string TfxDisplayJobId { get; set; }
    public List<TankDetail> Tanks { get; set; }
}

public class JobWithProductsModel
{
    public int TfxJobId { get; set; }
    public string TfxDisplayJobId { get; set; }
    public List<ProductTypesWithTanks> Products { get; set; }
}

public class ProductTypesWithTanks
{
    public int ProductTypeId { get; set; }
    public string ProductTypeName { get; set; }
    public List<TankDetail> Tanks { get; set; }
}

public class VedorRootResponse
{
    public List<VedorRootTanks> Tanks { get; set; } = new List<VedorRootTanks>();
    public int TFXJobId { get; set; }
    public string LastInventoryDate { get; set; }

    public string IP { get; set; }
}

public class VedorRootTanks
{
    public string TankId { get; set; }
    public string ProductName { get; set; }
    public string CurrentInventory { get; set; }
    public string WaterLevel { get; set; }
}
