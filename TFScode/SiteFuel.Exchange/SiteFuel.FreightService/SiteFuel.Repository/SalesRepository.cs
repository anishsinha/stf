using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.FreightRepository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess;
using System.Data.Entity;
using SiteFuel.Exchange.Logger;
using SiteFuel.MdbDataAccess;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Infrastructure;
using TrueFill.ExchangeDataAccess.DataAccess;
using TrueFill.DemandCaptureDataAccess.Entities;
using MongoDB.Bson.Serialization.Conventions;
using System.Data;
using System.Data.SqlClient;

namespace SiteFuel.FreightRepository
{
    public class SalesRepository : ISalesRepository
    {
        private readonly DemandCaptureContext context = new DemandCaptureContext();
        private readonly MdbContext mdbContext;

        public SalesRepository(MdbContext _context)
        {
            mdbContext = _context;
        }

        public async Task<LocationTanksResponseModel> GetLocationTanks(LocationTanksRequestModel requestModel)
        {
            var result = new LocationTanksResponseModel();
            try
            {
                List<SalesDataDaysRemainingModel> respData = new List<SalesDataDaysRemainingModel>();
                var regJobIds = new List<int>();
                var filterJobIds = requestModel.JobIds;

                if (!string.IsNullOrEmpty(requestModel.RegionIds))
                {
                    regJobIds = GetRegionJobIds(requestModel.RegionIds);
                    filterJobIds = filterJobIds.Where(t => regJobIds.Contains(t)).ToList();
                }

                //for buyer wally board - sales - create dr
                var jobRegions = await mdbContext.Regions.Find(t => t.TfxJobs.Any()
                                                           && t.TfxJobs.Any(x => filterJobIds.Contains(x.Id))
                                                           && t.TfxJobs.Any()
                                                           && t.IsActive).Project(t =>
                                                           new
                                                           {
                                                               t.Id,
                                                               t.TfxCompanyId,
                                                               t.TfxJobs
                                                           }).ToListAsync();

                result.LocationDetails = await mdbContext.JobAdditionalDetails
                                                .Find(t => filterJobIds.Contains(t.TfxJobId)
                                                           && !string.IsNullOrEmpty(t.TfxDisplayJobId)
                                                           && t.Tanks.Any()
                                                           && t.IsActive)
                                                .Project(t => new LocationTankDetailsModel
                                                {
                                                    SiteId = t.TfxDisplayJobId,
                                                    JobId = t.TfxJobId,
                                                    Tanks = t.Tanks.OrderBy(x => x.TankSequence == null || x.TankSequence == 0 ? 99999 : x.TankSequence).Select(t1 =>
                                                            new TankDetailModel
                                                            {
                                                                StorageId = t1.StorageId,
                                                                TankId = t1.StorageTypeId,
                                                                Name = t1.TankName,
                                                                TfxProductTypeId = t1.TfxProductTypeId,
                                                                TankSequence = t1.TankSequence
                                                            }).ToList()
                                                })
                                                .ToListAsync();
                List<string> allTankIds = new List<string>();
                List<string> allStorageIds = new List<string>();
                List<string> siteIds = new List<string>();
                if (result.LocationDetails != null && result.LocationDetails.Any())
                {
                    var tanksDetails = result.LocationDetails.Where(top => top.Tanks != null).SelectMany(x => x.Tanks).ToList();
                    if (tanksDetails.Any())
                    {
                        allTankIds = tanksDetails.Select(t1 => t1.TankId).Distinct().ToList();
                        allStorageIds = tanksDetails.Select(t1 => t1.StorageId).Distinct().ToList();
                    }
                    siteIds = result.LocationDetails.Select(t => t.SiteId).Distinct().ToList();
                }
                //IsRateOfConsumption Enabled
                if (requestModel.IsRateOfConsumption)
                {
                    respData = await GetSalesDaysRemainingAsync(siteIds, allTankIds, allStorageIds);
                }
                else
                {
                    respData = await GetSalesDaysRemainingAsync(siteIds, allTankIds, allStorageIds);
                }
                var scheduleStatus = mdbContext.DeliveryRequests.Find(t => (requestModel.CompanyId == 0 || t.TfxAssignedToCompanyId == requestModel.CompanyId || t.TfxCreatedByCompanyId == requestModel.CompanyId) && filterJobIds.Contains(t.TfxJobId) && (t.ScheduleShiftEndDateTime == null || t.ScheduleShiftEndDateTime.Value > DateTimeOffset.UtcNow.AddDays(-2).DateTime)
                                            && !t.GroupChildDRs.Any()
                                            && t.IsActive && !t.IsDeleted && (t.ParentId == null || t.ScheduleBuilderId != null)
                                            && (t.TfxScheduleStatus == (int)DeliveryScheduleStatus.None || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.New
                                                   || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Acknowledged || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Accepted || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Modified)
                                               && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                            ).Project(t => new { t.Id, t.TfxProductTypeId, t.Status, t.TfxJobId, t.ScheduleShiftEndDateTime, t.JobTimeZoneOffset })
                                            .ToList().GroupBy(t => new { t.TfxJobId, t.TfxProductTypeId }).SelectMany(t => t.ToList()).ToList();
                int missedSchedulePeriod = 4;
                if (scheduleStatus.Any())
                {
                    ExchangeAccess exchange = new ExchangeAccess();
                    int.TryParse(exchange.GetAppSetting("MissedScheduleWaitingPeriod"), out missedSchedulePeriod);
                }
                DateTimeOffset currentTime = DateTimeOffset.UtcNow;
                var query = from item in scheduleStatus
                            let jobTime = currentTime.Add(item.JobTimeZoneOffset)
                            where item.ScheduleShiftEndDateTime == null
                            || item.ScheduleShiftEndDateTime.Value.AddHours(missedSchedulePeriod) >= jobTime.DateTime
                            select item;
                scheduleStatus = query.ToList();

                if (result.LocationDetails.Any())
                {
                    foreach (var jobItem in result.LocationDetails)
                    {
                        if (jobRegions.Any())
                        {
                            var regionId = string.Empty;
                            regionId = jobRegions.Where(reg => reg.TfxCompanyId == requestModel.CompanyId && reg.TfxJobs.Any(r => r.Id != jobItem.JobId)).Select(t1 => t1.Id.ToString()).LastOrDefault();
                            if (String.IsNullOrWhiteSpace(regionId))
                            {
                                regionId = jobRegions.Where(reg => reg.TfxJobs.Any(r => r.Id != jobItem.JobId)).Select(t1 => t1.Id.ToString()).LastOrDefault();
                            }
                            jobItem.RegionId = regionId ?? string.Empty;
                        }

                        var tankInformation = jobItem.Tanks;
                        if (tankInformation.Any())
                        {
                            foreach (var item in tankInformation)
                            {
                                //set the tank schedule status.
                                var tankSchedules = scheduleStatus.Where(t => t.TfxJobId == jobItem.JobId && t.TfxProductTypeId == item.TfxProductTypeId).ToList();
                                var tStatus = Resource.valMessageNoDR;
                                if (tankSchedules != null && tankSchedules.Any())
                                {
                                    if (tankSchedules.All(t => t.Status == DeliveryReqStatus.ScheduleCreated)) { tStatus = Resource.lblScheduled; }
                                    else tStatus = Resource.valMessageDRCreated;
                                }
                                item.Status = tStatus;
                                if (respData.Any())
                                {
                                    //set the tank days remaining.
                                    var tankDaysRemaining = respData.FirstOrDefault(t => t.SiteId == jobItem.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId);
                                    if (tankDaysRemaining != null && !string.IsNullOrEmpty(tankDaysRemaining.DaysRemaining) && !tankDaysRemaining.DaysRemaining.Contains("--"))
                                    {
                                        tankDaysRemaining.DaysRemaining = tankDaysRemaining.DaysRemaining.Replace("--", string.Empty).Trim();
                                        item.DaysRemaining = Convert.ToDouble(tankDaysRemaining.DaysRemaining).GetPreciseValue(1);
                                    }
                                }
                            }
                            //set the jobs schedule status based on all the tank schedule status.
                            //get lowest days remaining along the all the tanks
                            if (!tankInformation.All(x => x.DaysRemaining == null))
                            {
                                var jobDaysRemaining = tankInformation.Where(x => x.DaysRemaining != null).OrderBy(top => top.DaysRemaining).FirstOrDefault();
                                if (jobDaysRemaining != null)
                                {
                                    jobItem.DaysRemaining = jobDaysRemaining.DaysRemaining;
                                }
                            }
                            if (tankInformation.All(t => t.Status == Resource.lblScheduled))
                            {
                                jobItem.Status = Resource.lblScheduled;
                            }
                            else
                            {
                                if (tankInformation.All(t => t.Status == Resource.valMessageNoDR))
                                {
                                    jobItem.Status = Resource.valMessageNoDR;
                                }
                                else
                                {

                                    if (tankInformation.Any(t => t.Status == Resource.valMessageNoDR) && tankInformation.Any(t => t.Status == Resource.lblScheduled))
                                    {
                                        jobItem.Status = Resource.lblScheduled;
                                    }
                                    else
                                    {
                                        jobItem.Status = Resource.valMessageDRCreated;
                                    }
                                }
                            }
                        }
                        else
                        {
                            jobItem.Status = Resource.valMessageNoDR;
                        }
                        //order by tank sequence
                        jobItem.Tanks = jobItem.Tanks.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();
                    }

                }
                //order by location info based on lowest job DaysRemaining.
                if (result.LocationDetails.Any())
                {
                    result.LocationDetails = result.LocationDetails.OrderBy(top => top.DaysRemaining).ToList();
                }
                result.StatusCode = (int)Status.Success;
                result.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)Status.Failed;
                result.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("SalesRepository", "GetLocationTanks", ex.Message, ex);
            }

            return result;
        }

        private List<JobTanksData> GetJobTData(List<int> filterJobIds)
        {
            return mdbContext.JobAdditionalDetails.Find(t => filterJobIds.Contains(t.TfxJobId) && !string.IsNullOrEmpty(t.TfxDisplayJobId) && t.Tanks.Any() && t.IsActive)
                                                           .Project(t =>
                                                               new JobTanksData
                                                               {
                                                                   TfxDisplayJobId = t.TfxDisplayJobId,
                                                                   TfxJobId = t.TfxJobId,
                                                                   tanksData = t.Tanks.Select(t1 =>
                                                                          new TanksData()
                                                                          {
                                                                              StorageId = t1.StorageId,
                                                                              StorageTypeId = t1.StorageTypeId,
                                                                              TfxProductTypeId = t1.TfxProductTypeId,
                                                                              TankName = t1.TankName,
                                                                              RunOutLevel = t1.RunOutLevel,
                                                                              FillType = t1.FillType,
                                                                              MinFill = t1.MinFill,
                                                                              MaxFill = t1.MaxFill,
                                                                              ThresholdDeliveryRequest = t1.ThresholdDeliveryRequest,
                                                                              FuelCapacity = t1.FuelCapacity
                                                                          }),
                                                               }).ToList();
        }

        private List<LocalSalesScheduleStatus> GetSalesScheduleStatuses(List<int> filterJobIds, SalesDataRequestModel requestModel)
        {
           return mdbContext.DeliveryRequests.Find(t =>
                                            (requestModel.CompanyId == 0
                                                || t.TfxAssignedToCompanyId == requestModel.CompanyId
                                                || t.TfxCreatedByCompanyId == requestModel.CompanyId
                                            )
                                            && filterJobIds.Contains(t.TfxJobId)
                                            && (t.ScheduleShiftEndDateTime == null
                                                    || t.ScheduleShiftEndDateTime.Value > DateTimeOffset.UtcNow.AddDays(-2).DateTime
                                                )
                                            && !t.GroupChildDRs.Any()
                                            && t.IsActive && !t.IsDeleted && (t.ParentId == null || t.ScheduleBuilderId != null)
                                            && (t.TfxScheduleStatus == (int)DeliveryScheduleStatus.None
                                                    || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.New
                                                    || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Acknowledged
                                                    || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Accepted
                                                    || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Modified
                                                )
                                            && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                            ).Project(t => new LocalSalesScheduleStatus (){ Id = t.Id, TfxProductTypeId = t.TfxProductTypeId, 
                                                                                            Status = t.Status,
                                                                                            TfxJobId = t.TfxJobId,
                                                                                            ScheduleShiftEndDateTime = t.ScheduleShiftEndDateTime,
                                                                                            JobTimeZoneOffset  = t.JobTimeZoneOffset }
                                                    ).ToList().GroupBy(t => new { t.TfxJobId, t.TfxProductTypeId }
                                                                        ).SelectMany(t => t.ToList()).ToList();
        }

        private int GetMissedSchedulePeriod()
        {
            int missedSchedulePeriod;
            ExchangeAccess exchange = new ExchangeAccess();
            int.TryParse(exchange.GetAppSetting("MissedScheduleWaitingPeriod"), out missedSchedulePeriod);
            return missedSchedulePeriod;
        }

        public async Task<SalesDataResponseModel> GetSalesDataAsync(SalesDataRequestModel requestModel)
        {
            var result = new SalesDataResponseModel();
            try
            {
                var regJobIds = new List<int>();
                //STEP 1: Get Job Id if any filter is applied
                var filterJobIds = requestModel.Jobs.Select(t => t.JobId).ToList();

                //STEP 2: Get All Jobs for the Region if Region is not null
                if (!string.IsNullOrEmpty(requestModel.RegionId))
                {
                    regJobIds = GetRegionJobIds(requestModel.RegionId);
                    filterJobIds = filterJobIds.Where(t => regJobIds.Contains(t)).ToList();
                }

                int missedSchedulePeriod = 4;
                DateTimeOffset currentTime = DateTimeOffset.UtcNow;

                //STEP 3: Get tanks and Additional Details  for the job
                //STEP 4: Get Schedule Status
                IEnumerable<JobTanksData> jobTanks = null;
                IEnumerable<LocalSalesScheduleStatus> scheduleStatus = null;

                Parallel.Invoke(
                    (() => jobTanks = GetJobTData(filterJobIds)),
                    (() => scheduleStatus = GetSalesScheduleStatuses(filterJobIds, requestModel))
                    );

                List<string> siteIds = null, tankIds = null, storageIds = null;

                //STEP 3.1: Get all Display Job Id from Job Addition details
                //STEP 3.2: Get all Tanks from Job Addition details
                //STEP 3.3: Get all Storage details from Job Addition details
                Parallel.Invoke(
                        (() => siteIds = jobTanks.Select(t => t.TfxDisplayJobId).Distinct().ToList()),
                        (() => tankIds = jobTanks.SelectMany(t => t.tanksData.Select(t1 => t1.StorageTypeId)).Distinct().ToList()),
                        (() => storageIds = jobTanks.SelectMany(t => t.tanksData.Select(t1 => t1.StorageId)).Distinct().ToList()),
                        (() => scheduleStatus = (from item in scheduleStatus
                                                 let jobTime = currentTime.Add(item.JobTimeZoneOffset)
                                                 where item.ScheduleShiftEndDateTime == null
                                                 || item.ScheduleShiftEndDateTime.Value.AddHours(missedSchedulePeriod) >= jobTime.DateTime
                                                 select item).ToList()),
                        (() => missedSchedulePeriod = scheduleStatus.Any()? GetMissedSchedulePeriod():4)
                        );

                //STEP 5: Get Sales details, water levels based on SiteIds, TankIds, StorageIds
                SalesDataResponseModel respData = null;
                List<SalesInventoryWaterLevel> waterLevels = null;                

                //Task tRespData = Task.Run(async () => respData = await GetSalesAsync(siteIds, tankIds, storageIds));
                //Task tWaterLevels = Task.Run(async () => waterLevels = await GetWaterLevelForTanks(siteIds, tankIds, storageIds));
                //await Task.WhenAll(tRespData, tWaterLevels); 

                var gtSales = GetSalesAsync(siteIds, tankIds, storageIds);
                var getWaterLevel = GetWaterLevelForTanks(siteIds, tankIds, storageIds);
                await Task.WhenAll(gtSales, getWaterLevel);
                respData = await gtSales;
                waterLevels = await getWaterLevel;

                //STEP 6: Process respData (Sales Data ) and Schedule Status
                if (respData != null)
                {
                    result.StatusCode = respData.StatusCode;
                    result.StatusMessage = respData.StatusMessage;
                    var regJobs = mdbContext.Regions.Find(t => t.TfxJobs.Any(t1 => filterJobIds.Contains(t1.Id)))
                                                    .Project(t => new { t.Id, t.TfxCompanyId, rJobs = t.TfxJobs.Where(t1 => filterJobIds.Contains(t1.Id)).Select(t1 => t1.Id).ToList() })
                                                    .ToList();

                    Parallel.ForEach(jobTanks, new ParallelOptions { MaxDegreeOfParallelism = 20 }, job =>
                    {
                        var selectedJob = requestModel.Jobs.FirstOrDefault(t => t.JobId == job.TfxJobId);

                        var regId = regJobs.Where(t => t.TfxCompanyId == requestModel.CompanyId && t.rJobs.Contains(job.TfxJobId)).Select(t => t.Id.ToString()).FirstOrDefault();
                        if (String.IsNullOrWhiteSpace(regId))
                        {
                            regId = regJobs.Where(t => t.rJobs.Contains(job.TfxJobId)).Select(t => t.Id.ToString()).FirstOrDefault();
                        }

                        Parallel.ForEach(job.tanksData, new ParallelOptions { MaxDegreeOfParallelism = 20 }, tank =>
                        {
                            var tankMaxFill = GetTankCapacity(tank.MaxFill, tank.FuelCapacity, tank.FillType.Value);
                            var tankMinFill = GetTankCapacity(tank.MinFill, tank.FuelCapacity, tank.FillType.Value);

                            var tStatus = Resource.valMessageNoDR;
                            var tankSchedules = scheduleStatus.Where(t => t.TfxJobId == job.TfxJobId && t.TfxProductTypeId == tank.TfxProductTypeId).ToList();

                            if (tankSchedules != null && tankSchedules.Any())
                            {
                                if (tankSchedules.All(t => t.Status == DeliveryReqStatus.ScheduleCreated)) { tStatus = Resource.lblScheduled; }
                                else tStatus = Resource.valMessageDRCreated;
                            }

                            string location = selectedJob.LocationTypeId == JobLocationTypes.Various ? selectedJob.StateCode : $"{selectedJob.JobAddress}, {selectedJob.City}, {selectedJob.StateCode} {selectedJob.ZipCode}";
                            SalesDataModel tankData = respData.SalesData.Where(t => t.SiteId == job.TfxDisplayJobId && t.TankId == tank.StorageTypeId && t.StorageId == tank.StorageId).FirstOrDefault();
                            InventoryDataCaptureType inventoryDataCaptureType = requestModel.Jobs.Where(t => t.JobId == job.TfxJobId).Select(t => t.InventoryDataCaptureType).FirstOrDefault();

                            if (tankData != null)
                            {
                                decimal waterLevel = 0;
                                if (tankData != null)
                                {
                                    var waterLvl = waterLevels.Where(t => t.SiteId == job.TfxDisplayJobId && t.TankId == tank.StorageTypeId && t.StorageId == tank.StorageId)
                                                            .Select(w => w.WaterNetLevel).FirstOrDefault();
                                    waterLevel = Convert.ToDecimal(waterLvl);

                                }
                                //waterLevel = await GetWaterLevelForTank(job.TfxDisplayJobId, tank.StorageTypeId, tank.StorageId);

                                var timeZoneName = requestModel.Jobs.Where(t => t.JobId == job.TfxJobId).Select(t => t.TimeZoneName).FirstOrDefault();
                                //tankData.TankInventoryDiffinHrs = (DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName).DateTime - tankData.EndUllageDate).Hours;
                                TimeSpan ts = (DateTimeOffset.Now.ToTargetDateTimeOffset(timeZoneName).DateTime - tankData.EndUllageDate);

                                UpdateSalesData(tankData, tank.TankName, selectedJob.CustomerId, selectedJob.CustomerName
                                                , location, selectedJob.LocationName, tStatus, job.TfxJobId, tank.TfxProductTypeId
                                                , tankMaxFill, tankMinFill, tank.FuelCapacity, regId
                                                , GetTankPriority(tankData.InventoryData, tank.MinFill, tank.FillType, tank.RunOutLevel, tank.FuelCapacity, tank.ThresholdDeliveryRequest)
                                                , selectedJob.LocationManagedType
                                                , inventoryDataCaptureType
                                                , tankData.InventoryData <= 0 ? Resource.lblHyphen.ToString() : tankData.InventoryData.ToString()
                                                , waterLevel, ts.TotalHours);

                                result.SalesData.Add(tankData);

                            }
                            else if (requestModel.SelectedTab != (int)FreightModels.SelctedSalesTab.Priority)
                            {

                                SalesDataModel salesDataModel = GetSalesData(tank.TankName, selectedJob.CustomerId, selectedJob.CustomerName,
                                                    job.TfxDisplayJobId, location, selectedJob.LocationName, tank.StorageId, tank.StorageTypeId
                                                    , tStatus, job.TfxJobId, tank.TfxProductTypeId, tankMaxFill, regId, DeliveryReqPriority.MustGo
                                                    , selectedJob.LocationManagedType, inventoryDataCaptureType);

                                result.SalesData.Add(salesDataModel);
                            }
                        });
                    });

                    if (requestModel.Priority > 0)
                    {
                        var priority = (DeliveryReqPriority)requestModel.Priority;
                        result.SalesData = result.SalesData.Where(t => t.Priority == priority).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesRepository", "GetSalesDataAsync", ex.Message, ex);
            }
            return result;
        }

        private static void UpdateSalesData(SalesDataModel tankData, string tankName, int companyId, string companyName, string location, string locationName
                                            , string tStatus, int tfxJobId, int productTypeId
                                            , decimal? tankMaxFill, decimal? tankMinFill, decimal? tankCapacity
                                            , string regionId, DeliveryReqPriority reqPriority, int locationManagedType
                                            , InventoryDataCaptureType invDCapture, string inventory, decimal waterLevel
                                            , double inventoryDiffInHours)
        {
            if (tankData != null)
            {

                tankData.TankName = tankName;
                tankData.CompanyId = companyId;
                tankData.CompanyName = companyName;

                tankData.Location = location;
                tankData.LocationName = locationName;

                tankData.Status = tStatus;
                tankData.TfxJobId = tfxJobId;

                tankData.ProductTypeId = productTypeId;

                tankData.MaxFillQuantity = tankMaxFill;
                tankData.MinFillQuantity = tankMinFill;
                tankData.TankCapacity = tankCapacity;

                tankData.RegionId = regionId;
                tankData.Priority = reqPriority;

                tankData.InventoryDataCaptureType = invDCapture;
                tankData.InventoryDataCaptureTypeName = invDCapture.GetDisplayName();

                tankData.LocationManagedType = locationManagedType;

                tankData.WaterLevel = waterLevel;
                tankData.Inventory = inventory;
                tankData.TankInventoryDiffinHrs = inventoryDiffInHours;
            }
        }

        private static SalesDataModel GetSalesData(string tankName, int companyId, string companyName, string siteid, string location, string locationName
                                            , string storageId, string storageTypeId, string tStatus, int tfxJobId, int productTypeId, decimal? tankMaxFill
                                            , string regionId, DeliveryReqPriority deliveryReq, int locationManagedType
                                            , InventoryDataCaptureType invDCapture)
        {
            return new SalesDataModel
            {
                TankName = tankName,
                CompanyId = companyId,
                CompanyName = companyName,
                SiteId = siteid,
                Location = location,
                LocationName = locationName,
                StorageId = storageId,
                TankId = storageTypeId,
                Status = tStatus,
                TfxJobId = tfxJobId,
                ProductTypeId = productTypeId,
                MaxFillQuantity = tankMaxFill,
                RegionId = regionId,
                Priority = deliveryReq,
                AvgSale = Resource.lblHyphen,
                DaysRemaining = Resource.lblHyphen,
                Inventory = Resource.lblHyphen,
                LastDeliveredQuantity = Resource.lblHyphen,
                LastDeliveryDate = Resource.lblHyphen,
                PrevSale = Resource.lblHyphen,
                Ullage = Resource.lblHyphen,
                WeekAgoSale = Resource.lblHyphen,
                LastReadingTime = Resource.lblHyphen,
                InventoryDataCaptureType = invDCapture,
                InventoryDataCaptureTypeName = invDCapture.GetDisplayName(),
                LocationManagedType = locationManagedType,
                UOM = 0
            };
        }

        private decimal? GetTankCapacity(decimal? quantity, decimal? fuelCapacity, int fillType)
        {
            decimal? tankMaxFill = null;
            if (quantity.HasValue && fuelCapacity.HasValue)
            {
                if (fillType == (int)FillType.Percent)
                {
                    tankMaxFill = fuelCapacity.Value * quantity.Value / 100;
                }
                else
                {
                    tankMaxFill = quantity.Value;
                }
            }
            return tankMaxFill;
        }

        private List<int> GetRegionJobIds(string regionIds)
        {
            List<int> regJobIds;
            var oRegIds = regionIds.Split(',').Select(t => ObjectId.Parse(t.Trim())).ToList();

            var regJobs = mdbContext.Regions.Find(t => oRegIds.Contains(t.Id))
                                        .Project(t => t.TfxJobs.Select(t1 => t1.Id)).ToList();
            regJobIds = regJobs.SelectMany(t => t).Distinct().ToList();
            return regJobIds;
        }

        private List<int> GetJobIdsForRegions(List<string> regionIds)
        {
            List<int> regJobIds = new List<int>();
            foreach (var regionIdStr in regionIds)
            {
                var regionId = ObjectId.Parse(regionIdStr.Trim());

                var regJobs = mdbContext.Regions.Find(t => t.Id == regionId)
                                            .Project(t => t.TfxJobs.Select(t1 => t1.Id)).ToList();
                var jobIds = regJobs.SelectMany(t => t).Distinct().ToList();
                regJobIds.AddRange(jobIds);
            }
            return regJobIds;
        }

        private async Task<SalesDataResponseModel> GetSalesAsync(List<string> siteIds, List<string> tankIds, List<string> storageIds, int timeout = 30)
        {
            var result = new SalesDataResponseModel();
            try
            {
                //Create SiteIds Table
                DataTable SiteIds, TankIds, StorageIds;
                IntializeDemandsParameters(siteIds, tankIds, storageIds, out SiteIds, out TankIds, out StorageIds);
                var siteIdParam = new SqlParameter("@SiteList", SqlDbType.Structured);
                siteIdParam.Value = SiteIds;
                siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                var tankIdsParam = new SqlParameter("@TankList", SqlDbType.Structured);
                tankIdsParam.Value = TankIds;
                tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                var storageIdParams = new SqlParameter("@StorageList", SqlDbType.Structured);
                storageIdParams.Value = StorageIds;
                storageIdParams.TypeName = "dbo.DemandsSearchTypes";
                //if (context.Database.CommandTimeout != timeout)
                //    context.Database.CommandTimeout = timeout;
                //Since ADO.NET employs connection pooling and creating a new DbContext is relatively light - weight,
                //you are safe to create a lot of contexts which can be disposed after using them.
                using (var ctx = new DemandCaptureContext())
                {
                    result.SalesData = await ctx.Database.SqlQuery<SalesDataModel>("usp_GetSalesData @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();
                }
                result.StatusCode = (int)Status.Success;
                result.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)Status.Failed;
                result.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("SalesRepository", "GetSalesAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> GetDailySalesAsync()
        {
            var response = false;
            try
            {
                var startDate = DateTime.Now.AddDays(-8).Date;
                var endDate = startDate.AddDays(2);
                var todayDate = DateTime.Now.Date;
                while (endDate <= todayDate)
                {
                    var lastweekdemands = await GetLastWeekDemandsAsync(startDate, endDate.AddDays(1));
                    if (lastweekdemands != null)
                    {
                        var updatedTankInventory = lastweekdemands.GroupBy(t => new { t.SiteId, t.TankId, t.StorageId })
                                                                  .Select(t => new
                                                                  {
                                                                      t.Key.SiteId,
                                                                      t.Key.TankId,
                                                                      t.Key.StorageId,
                                                                      Demands = t.Where(t1 => t1.Ullage.HasValue).ToList(),
                                                                      Sales = context.DailySales.Where(t1 => t1.SiteId == t.Key.SiteId && t1.TankId == t.Key.TankId && t1.StorageId == t.Key.StorageId && t1.CreatedDate >= startDate && t1.CreatedDate <= endDate).ToList(),
                                                                      Drops = context.TankDrops.Where(t1 => t1.SiteId == t.Key.SiteId && t1.TankId == t.Key.TankId && t1.StorageId == t.Key.StorageId && t1.EndTime >= startDate && t1.EndTime <= endDate).ToList()
                                                                  });

                        var tasks = updatedTankInventory.Select(async item =>
                        {
                            var data = await SyncUpdatedDiptest(item.Sales, item.Demands, item.Drops, startDate, endDate);
                        });
                        await Task.WhenAll(tasks);
                        //foreach (var item in updatedTankInventory)
                        //{
                        //    await SyncUpdatedDiptest(item.Sales, item.Demands, item.Drops, startDate, endDate);
                        //}
                        //await context.SaveChangesAsync();

                        startDate = endDate;
                        endDate = endDate.AddDays(2);
                    };
                }
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                LogManager.Logger.WriteException("SalesRepository", "GetDailySalesAsync", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<DemandSalesModel>> GetLastWeekDemandsAsync(DateTime StartDate, DateTime EndDate, int timeout = 60)
        {
            var result = new List<DemandSalesModel>();
            try
            {
                var inputModel = new { StartDate.Date, EndDate };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetLastWeekDemandsData", inputModel);

                context.Database.CommandTimeout = timeout;
                result = await context.Database.SqlQuery<DemandSalesModel>(input.Query, input.Params.ToArray()).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesRepository", "GetLastWeekDemandsAsync", ex.Message, ex);
            }
            return result;
        }

        private async Task<bool> SyncUpdatedDiptest(List<DailySale> tankDailySale, List<DemandSalesModel> tankdemand, List<TankDrop> tankDrops, DateTime sDate, DateTime tDate)
        {
            try
            {
                var isRecordModified = false;
                while (sDate < tDate)
                {
                    var prevDayEndValue = tankdemand.Where(t => t.CaptureTime.Value.Date == sDate).OrderByDescending(t => t.CaptureTime).FirstOrDefault();
                    var nxtDay = sDate.AddDays(1);
                    var nextDayEndValue = tankdemand.Where(t => t.CaptureTime.Value.Date == nxtDay).OrderByDescending(t => t.CaptureTime).FirstOrDefault();

                    var existingNextDaySale = tankDailySale.Where(t => t.CreatedDate.Date == nxtDay).FirstOrDefault();
                    var existingPrevDaySale = tankDailySale.Where(t => t.CreatedDate.Date == sDate).FirstOrDefault();
                    if (existingNextDaySale != null && nextDayEndValue != null)
                    {
                        //update
                        if (prevDayEndValue != null && existingNextDaySale.StartUllageDate != prevDayEndValue.CaptureTime)
                        {
                            existingNextDaySale.StartUllage = prevDayEndValue.Ullage.Value;
                            existingNextDaySale.StartUllageDate = prevDayEndValue.CaptureTime.Value;
                        }
                        else if (existingPrevDaySale != null && existingNextDaySale.StartUllageDate != existingPrevDaySale.EndUllageDate)
                        {
                            existingNextDaySale.StartUllage = existingPrevDaySale.EndUllage;
                            existingNextDaySale.StartUllageDate = existingPrevDaySale.EndUllageDate;
                        }
                        existingNextDaySale.EndUllage = nextDayEndValue.Ullage.Value;
                        existingNextDaySale.EndUllageDate = nextDayEndValue.CaptureTime.Value;
                        existingNextDaySale.DroppedQuantity = tankDrops.Where(t => t.EndTime > existingNextDaySale.StartUllageDate && t.EndTime < existingNextDaySale.EndUllageDate && t.IsActive).Sum(t => t.DroppedQuantity);
                        context.Entry(existingNextDaySale).State = EntityState.Modified;
                        isRecordModified = true;
                    }
                    else if (nextDayEndValue != null)
                    {
                        //insert
                        var newSale = new DailySale
                        {
                            CreatedDate = nxtDay,
                            EndUllage = nextDayEndValue.Ullage.Value,
                            NetVolume = nextDayEndValue.NetVolume.Value,
                            EndUllageDate = nextDayEndValue.CaptureTime.Value,
                            SiteId = nextDayEndValue.SiteId,
                            TankId = nextDayEndValue.TankId,
                            StorageId = nextDayEndValue.StorageId,
                            UoM = nextDayEndValue.DipTestUoM
                        };
                        if (existingPrevDaySale != null)
                        {
                            newSale.StartUllage = existingPrevDaySale.EndUllage;
                            newSale.StartUllageDate = existingPrevDaySale.EndUllageDate;
                        }
                        else if (prevDayEndValue != null)
                        {
                            newSale.StartUllage = prevDayEndValue.Ullage.Value;
                            newSale.StartUllageDate = prevDayEndValue.CaptureTime.Value;
                        }
                        else
                        {
                            var nextDayStartValue = tankdemand.Where(t => t.CaptureTime.Value.Date == nxtDay).OrderBy(t => t.CaptureTime).FirstOrDefault();
                            newSale.StartUllage = nextDayStartValue.Ullage.Value;
                            newSale.StartUllageDate = nextDayStartValue.CaptureTime.Value;
                        }
                        newSale.DroppedQuantity = tankDrops.Where(t => t.EndTime > newSale.StartUllageDate && t.EndTime < newSale.EndUllageDate && t.IsActive).Sum(t => t.DroppedQuantity);
                        tankDailySale.Add(newSale);
                        context.DailySales.Add(newSale);
                        isRecordModified = true;
                    }
                    else if (existingNextDaySale == null && existingPrevDaySale != null)
                    {
                        var newNextDaySale = new DailySale
                        {
                            CreatedDate = nxtDay,
                            EndUllage = existingPrevDaySale.EndUllage,
                            NetVolume = existingPrevDaySale.NetVolume,
                            EndUllageDate = existingPrevDaySale.EndUllageDate,
                            SiteId = existingPrevDaySale.SiteId,
                            TankId = existingPrevDaySale.TankId,
                            StorageId = existingPrevDaySale.StorageId,
                            UoM = existingPrevDaySale.UoM,
                            StartUllage = existingPrevDaySale.EndUllage,
                            StartUllageDate = existingPrevDaySale.EndUllageDate
                        };
                        tankDailySale.Add(newNextDaySale);
                        context.DailySales.Add(newNextDaySale);
                        isRecordModified = true;
                    }
                    sDate = sDate.AddDays(1);
                }
                if (isRecordModified)
                    context.SaveChanges();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesRepository", "SyncUpdatedDiptest", ex.Message, ex);
                return false;
            }
            return true;
        }

        public async Task<DeliveryDetailsRespModel> GetExistingSchedulesAsync(int jobId, int productTypeId, int companyId)
        {
            var result = new DeliveryDetailsRespModel();
            try
            {
                result.DeliveryDetails = await mdbContext.DeliveryRequests.Find(t => (companyId == 0 || t.TfxAssignedToCompanyId == companyId || t.TfxCreatedByCompanyId == companyId) && jobId == t.TfxJobId
                                           && productTypeId == t.TfxProductTypeId
                                           && t.IsActive && !t.IsDeleted && t.GroupChildDRs.Count() == 0
                                           && (t.TfxScheduleStatus == (int)DeliveryScheduleStatus.None || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.New
                                                   || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Acknowledged || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Accepted || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Modified)
                                               && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                           ).Project(t => new DeliveryDetailsModel
                                           {
                                               TrackableScheduleId = t.TfxTrackableScheduleId,
                                               Quantity = t.RequiredQuantity,
                                               ScheduleDate = Resource.lblHyphen,
                                               ScheduleTime = Resource.lblHyphen,
                                               Carrier = Resource.lblHyphen,
                                               Driver = Resource.lblHyphen,
                                               QuantityTypeId = t.ScheduleQuantityType
                                           })
                                           .ToListAsync();

                result.StatusCode = (int)Status.Success;
                result.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)Status.Failed;
                result.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("SalesRepository", "GetExistingSchedulesAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<SalesGraphRespDataModel> GetSalesGraphDataAsync(int jobId, int noOfDays)
        {
            var result = new SalesGraphRespDataModel();
            try
            {
                var jobTanks = await mdbContext.JobAdditionalDetails.Find(t => t.TfxJobId == jobId && t.IsActive)
                                                        .Project(t => new
                                                        {
                                                            t.TfxDisplayJobId,
                                                            t.TfxJobId,
                                                            Tanks = t.Tanks.Select(t1 => new
                                                            {
                                                                t1.TankName,
                                                                t1.StorageId,
                                                                t1.StorageTypeId
                                                            })
                                                        }).FirstOrDefaultAsync();
                if (jobTanks.Tanks != null && jobTanks.Tanks.Any())
                {
                    var siteId = jobTanks.TfxDisplayJobId;
                    var tankIds = jobTanks.Tanks.Select(t => t.StorageTypeId).ToList();
                    var storageIds = jobTanks.Tanks.Select(t => t.StorageId).ToList();

                    var respData = await GetSalesDataForGraph(siteId, tankIds, storageIds, noOfDays);
                    if (respData != null)
                    {
                        result.StatusCode = respData.StatusCode;
                        result.StatusMessage = respData.StatusMessage;
                        foreach (var tank in jobTanks.Tanks)
                        {
                            var tankData = respData.Sales.Where(t => t.SiteId == jobTanks.TfxDisplayJobId && t.TankId == tank.StorageTypeId && t.StorageId == tank.StorageId).ToList();
                            if (tankData != null && tankData.Any())
                            {
                                tankData.ForEach(t => { t.TankName = tank.TankName; t.JobId = jobId; });
                                result.Sales.AddRange(tankData);
                            }
                            else
                            {
                                var tdata = new SalesGraphDataModel
                                {
                                    TankName = tank.TankName,
                                    JobId = jobId,
                                    SiteId = jobTanks.TfxDisplayJobId,
                                    StorageId = tank.StorageId,
                                    TankId = tank.StorageTypeId
                                };
                                result.Sales.Add(tdata);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesRepository", "GetSalesGraphDataAsync", ex.Message, ex);
            }
            return result;
        }

        private async Task<SalesGraphRespDataModel> GetSalesDataForGraph(string siteId, List<string> tankIds, List<string> storageIds, int noOfDays, int timeout = 30)
        {
            var result = new SalesGraphRespDataModel();
            try
            {
                var inputmodel = new
                {
                    SiteId = siteId,
                    TankId = string.Join(",", tankIds),
                    StorageId = string.Join(",", storageIds),
                    NoOfDays = noOfDays
                };
                var input = SqlHelperMethods.GetStoredProcedure("usp_GetTanksSale", inputmodel);

                context.Database.CommandTimeout = timeout;
                result.Sales = await context.Database.SqlQuery<SalesGraphDataModel>(input.Query, input.Params.ToArray()).ToListAsync();

                result.StatusCode = (int)Status.Success;
                result.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)Status.Failed;
                result.StatusMessage = Status.Failed.ToString();
                LogManager.Logger.WriteException("SalesRepository", "GetSalesDataForGraph", ex.Message, ex);
            }
            return result;
        }

        private DeliveryReqPriority GetTankPriority(decimal currentInventory, decimal? minFill, int? fillType, decimal? runOutLevel, decimal? fuelCapacity, decimal? reOrderLevel)
        {
            var response = DeliveryReqPriority.CouldGo;
            try
            {
                decimal minFillPercent = 0;
                decimal currentInventoryPercent = 0;
                if (fuelCapacity.HasValue)
                {
                    currentInventoryPercent = currentInventory * 100 / fuelCapacity.Value;
                }

                if (minFill.HasValue && fuelCapacity.HasValue && fillType == (int)FillType.UoM)
                {
                    minFillPercent = (minFill.Value * 100) / fuelCapacity.Value;
                }
                else if (minFill.HasValue && fillType == (int)FillType.Percent)
                {
                    minFillPercent = minFill.Value;
                }

                if (currentInventoryPercent <= minFillPercent)
                {
                    response = DeliveryReqPriority.MustGo;
                }
                else if (runOutLevel.HasValue && currentInventoryPercent <= runOutLevel)
                {
                    response = DeliveryReqPriority.ShouldGo;
                }
                else
                {
                    response = DeliveryReqPriority.CouldGo;
                }
            }
            catch (Exception ex)
            {
                Exchange.Logger.LogManager.Logger.WriteException("SalesRepository", "GetTankPriority", ex.Message, ex);
            }
            return response;
        }

        private async Task<List<SalesDataDaysRemainingModel>> GetSalesDaysRemainingAsync(List<string> siteIds, List<string> tankIds, List<string> storageIds, int timeout = 30)
        {
            var result = new List<SalesDataDaysRemainingModel>();
            try
            {
                //Create SiteIds Table
                DataTable SiteIds, TankIds, StorageIds;
                IntializeDemandsParameters(siteIds, tankIds, storageIds, out SiteIds, out TankIds, out StorageIds);
                var siteIdParam = new SqlParameter("@SiteList", SqlDbType.Structured);
                siteIdParam.Value = SiteIds;
                siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                var tankIdsParam = new SqlParameter("@TankList", SqlDbType.Structured);
                tankIdsParam.Value = TankIds;
                tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                var storageIdParams = new SqlParameter("@StorageList", SqlDbType.Structured);
                storageIdParams.Value = StorageIds;
                storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                context.Database.CommandTimeout = timeout;
                result = await context.Database.SqlQuery<SalesDataDaysRemainingModel>("usp_GetSalesData @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesRepository", "GetSalesDaysRemainingAsync", ex.Message, ex);
            }
            return result;
        }
                
        private async Task<List<SalesInventoryWaterLevel>> GetWaterLevelForTanks(List<string> siteIds, List<string> tankIds, List<string> storageIds, int timeout = 30)
        {
            var result = new List<SalesInventoryWaterLevel>();
            try
            {
                DataTable SiteIds, TankIds, StorageIds;
                IntializeDemandsParameters(siteIds, tankIds, storageIds, out SiteIds, out TankIds, out StorageIds);
                var siteIdParam = new SqlParameter("@SiteIds", SqlDbType.Structured);
                siteIdParam.Value = SiteIds;
                siteIdParam.TypeName = "dbo.DemandsSearchTypes";

                var tankIdsParam = new SqlParameter("@TankIds", SqlDbType.Structured);
                tankIdsParam.Value = TankIds;
                tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

                var storageIdParams = new SqlParameter("@StorageIds", SqlDbType.Structured);
                storageIdParams.Value = StorageIds;
                storageIdParams.TypeName = "dbo.DemandsSearchTypes";

                //if (context.Database.CommandTimeout != timeout)
                //    context.Database.CommandTimeout = timeout;

                //Since ADO.NET employs connection pooling and creating a new DbContext is relatively light - weight,
                //you are safe to create a lot of contexts which can be disposed after using them.
                using (var ctx = new DemandCaptureContext())
                {
                    result = await ctx.Database.SqlQuery<SalesInventoryWaterLevel>("usp_GetWaterLevelForTanks @SiteIds,@TankIds,@StorageIds", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();
                }
                    
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesRepository", "GetWaterLevelForTanks", ex.Message, ex);
            }
            return result;

        }

        public async Task<InventoryDataResponseModel> GetInventoryDataForDashboard(InventoryDataViewModel requestModel)
        {
            var result = new InventoryDataResponseModel();
            try
            {
                var regJobIds = new List<int>();
                var filterJobIds = requestModel.CustomerJobs.Select(t => t.JobId).ToList();
                if (requestModel.SelectedRegionIds != null && requestModel.SelectedRegionIds.Any())
                {
                    regJobIds = GetJobIdsForRegions(requestModel.SelectedRegionIds);
                    filterJobIds = filterJobIds.Where(jobId => regJobIds.Any(regionJobId => regionJobId == jobId)).ToList();
                }

                var jobTanks = mdbContext.JobAdditionalDetails.Find(t => filterJobIds.Contains(t.TfxJobId) && !string.IsNullOrEmpty(t.TfxDisplayJobId) && t.Tanks.Any() && t.IsActive)
                                                           .Project(t =>
                                                           new
                                                           {
                                                               t.TfxDisplayJobId,
                                                               t.TfxJobId,
                                                               tanksData = t.Tanks.Select(t1 =>
                                                                      new
                                                                      {
                                                                          t1.StorageId,
                                                                          t1.StorageTypeId,
                                                                          t1.TfxProductTypeId,
                                                                          t1.TankName,
                                                                          t1.RunOutLevel,
                                                                          t1.FillType,
                                                                          t1.MinFill,
                                                                          t1.MaxFill,
                                                                          t1.ThresholdDeliveryRequest,
                                                                          t1.FuelCapacity,
                                                                          t.TfxDisplayJobId,
                                                                          MaxFillInQty = (t1.FillType ?? (int)FillType.Percent) == (int)FillType.Percent ? (t1.MaxFill / 100) * t1.FuelCapacity : t1.MaxFill,
                                                                          MinFillInQty = (t1.FillType ?? (int)FillType.Percent) == (int)FillType.Percent ? (t1.MinFill / 100) * t1.FuelCapacity : t1.MinFill
                                                                      }),
                                                           }).ToList();

                var siteIds = jobTanks.Select(t => t.TfxDisplayJobId).Distinct().ToList();
                var tankIds = jobTanks.SelectMany(t => t.tanksData.Select(t1 => t1.StorageTypeId)).Distinct().ToList();
                var storageIds = jobTanks.SelectMany(t => t.tanksData.Select(t1 => t1.StorageId)).Distinct().ToList();
                var respData = await GetSalesAsync(siteIds, tankIds, storageIds);

                var response = new InventoryDataModel();
                if (respData != null && respData.StatusCode == (int)Status.Success && respData.SalesData.Any())
                {
                    response.TotalInventory = respData.SalesData.Sum(t => t.InventoryData).ToString("0.0000");
                    response.TotalUllage = respData.SalesData.Sum(t => t.UllageData).ToString("0.0000");
                    response.PrevDaySale = respData.SalesData.Sum(t => t.PrevSaleData).ToString("0.0000");
                    response.AvgWeekAgoSale = respData.SalesData.Sum(t => t.WeekAgoSaleData).ToString("0.0000");

                    var tanksData = jobTanks.SelectMany(t => t.tanksData).ToList();
                    response.OverfillTanks = respData.SalesData.Count(t => t.InventoryData > tanksData.FirstOrDefault(t1 => t1.TfxDisplayJobId == t.SiteId && t1.StorageId == t.StorageId && t.TankId == t.TankId).MaxFillInQty);
                    response.RunOutTanks = respData.SalesData.Count(t => t.InventoryData < tanksData.FirstOrDefault(t1 => t1.TfxDisplayJobId == t.SiteId && t1.StorageId == t.StorageId && t.TankId == t.TankId).MinFillInQty);
                }
                response.DeliveryRequests = Convert.ToInt32(mdbContext.DeliveryRequests.CountDocuments(t => t.IsActive && !t.IsDeleted && t.TfxSupplierCompanyId == requestModel.CompanyId && (filterJobIds.Count() == 0 || filterJobIds.Contains(t.TfxJobId)) && t.GroupChildDRs.Count() == 0 && (t.ScheduleShiftEndDateTime == null || t.ScheduleShiftEndDateTime.Value >= DateTimeOffset.UtcNow.DateTime) && (t.Status == DeliveryReqStatus.Assigned || t.Status == DeliveryReqStatus.Draft || t.Status == DeliveryReqStatus.Pending || t.Status == DeliveryReqStatus.None)));
                response.ExistingDeliverySchedule = Convert.ToInt32(mdbContext.DeliveryRequests.CountDocuments(t => t.IsActive && !t.IsDeleted && t.TfxSupplierCompanyId == requestModel.CompanyId && (filterJobIds.Count() == 0 || filterJobIds.Contains(t.TfxJobId)) && t.GroupChildDRs.Count() == 0 && t.Status == DeliveryReqStatus.ScheduleCreated && (t.ScheduleShiftEndDateTime != null && t.ScheduleShiftEndDateTime.Value >= DateTimeOffset.UtcNow.DateTime)));

                result.InventoryData = response;
                result.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                result.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("SalesRepository", "GetInventoryDataForDashboard", ex.Message, ex);
            }
            return result;
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

        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SearchVar", typeof(string));
            return dt;
        }

    }


    internal class LocalSalesScheduleStatus
    {
        internal ObjectId Id;
        internal int TfxProductTypeId;
        internal DeliveryReqStatus Status;
        internal int TfxJobId;
        internal DateTime? ScheduleShiftEndDateTime;
        internal TimeSpan JobTimeZoneOffset;

    }

    internal class JobTanksData
    {
        internal string TfxDisplayJobId;
        internal int TfxJobId;
        internal IEnumerable<TanksData> tanksData;
    }

    internal class TanksData
    {
        internal string StorageId;
        internal string StorageTypeId;
        internal int TfxProductTypeId;
        internal string TankName;
        internal int? FillType;
        internal Nullable<decimal> RunOutLevel;
        internal Nullable<decimal> MinFill;
        internal Nullable<decimal> MaxFill;
        internal Nullable<decimal> FuelCapacity;
        internal Nullable<decimal> ThresholdDeliveryRequest;

    }


}
