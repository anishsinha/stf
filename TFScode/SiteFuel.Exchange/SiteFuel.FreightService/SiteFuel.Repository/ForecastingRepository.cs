using MongoDB.Bson;
using MongoDB.Driver;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ForcastingHelpers;
using SiteFuel.FreightModels.Forecasting;
using SiteFuel.FreightRepository.Helpers;
using SiteFuel.FreightRepository.Mappers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess;
using TrueFill.DemandCaptureDataAccess.Entities;
using TrueFill.ExchangeDataAccess.DataAccess;

namespace SiteFuel.FreightRepository
{
    public class ForecastingRepository : IForecastingRepository
    {
        //private int _timeout = 60;
        private readonly DemandCaptureContext context=new DemandCaptureContext();
        private readonly MdbContext mdbContext;
        public ForecastingRepository()
        {
            if (mdbContext == null)
            {
                mdbContext = new MdbContext();
            }
        }
        public ForecastingRepository(MdbContext _mdbcontext)
        {
            mdbContext = _mdbcontext;
        }
        public async Task<List<SaleDemandModel>> GetForecastingDemandData(DateTime startDate, DateTime endDate, int _timeout = 60)
        {
            var inputmodel = new { StartDate = startDate, EndDate = endDate };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetAllForcastingDemandData", inputmodel);

            context.Database.CommandTimeout = _timeout;
            var response = await context.Database.SqlQuery<SaleDemandModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<SaleTankDropModel>> GetForecastingTankDropData(DateTime startDate, DateTime endDate, int _timeout = 60)
        {
            var inputmodel = new { StartDate = startDate, EndDate = endDate };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetAllForcastingTankDropData", inputmodel);

            context.Database.CommandTimeout = _timeout;
            var response = await context.Database.SqlQuery<SaleTankDropModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<StatusModel> SaveDailySale(ISale24HourModel sale24Hours)
        {
            var response = new StatusModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    await SaveSale24Hours(sale24Hours);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        public async Task<StatusModel> SaveMonthlySale(List<ISale24HourModel> sale24HourModels, int _timeout = 60)
        {
            var response = new StatusModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var sale24Hours in sale24HourModels)
                    {
                        await SaveSale24Hours(sale24Hours);
                    }
                    await context.SaveChangesAsync();
                    transaction.Commit();
                    response.StatusCode = (int)Status.Success;
                    response.StatusMessage = Status.Success.ToString();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return response;
        }

        private async Task SaveSale24Hours(ISale24HourModel sale24Hours, int _timeout = 60)
        {
            var inputmodel = new
            {
                sale24Hours.SaleTank.SiteId,
                sale24Hours.SaleTank.TankId,
                sale24Hours.SaleTank.StorageId,
                sale24Hours.SaleTank.BandPeriod,
                sale24Hours.SaleTank.StartTime,
                sale24Hours.Date,
                sale24Hours.SaleTank.MaxFill,
                sale24Hours.SaleTank.FillType,
                sale24Hours.SaleTank.FuelCapacity,
                sale24Hours.SaleTank.Retain,
                sale24Hours.SaleTank.SaftyStock,
                sale24Hours.SaleTank.Runout,
                sale24Hours.SaleTank.InventoryUoM,
                sale24Hours.From00To01,
                sale24Hours.From01To02,
                sale24Hours.From02To03,
                sale24Hours.From03To04,
                sale24Hours.From04To05,
                sale24Hours.From05To06,
                sale24Hours.From06To07,
                sale24Hours.From07To08,
                sale24Hours.From08To09,
                sale24Hours.From09To10,
                sale24Hours.From10To11,
                sale24Hours.From11To12,
                sale24Hours.From12To13,
                sale24Hours.From13To14,
                sale24Hours.From14To15,
                sale24Hours.From15To16,
                sale24Hours.From16To17,
                sale24Hours.From17To18,
                sale24Hours.From18To19,
                sale24Hours.From19To20,
                sale24Hours.From20To21,
                sale24Hours.From21To22,
                sale24Hours.From22To23,
                sale24Hours.From23To00
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_Save24HoursSaleData", inputmodel);

            context.Database.CommandTimeout = _timeout;
            var result = await context.Database.SqlQuery<object>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
        }

        public async Task<JobWithTanksModel> GetJobWithTanks(int jobId, string tankId, string storageId)
        {
            var job = new JobWithTanksModel();
            if (!string.IsNullOrEmpty(tankId) && !string.IsNullOrEmpty(storageId))
            {
                job = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxJobId == jobId && t.Tanks != null && t.Tanks.Any())
                                                .Project(t1 => new JobWithTanksModel()
                                                {
                                                    TfxDisplayJobId = t1.TfxDisplayJobId,
                                                    Tanks = t1.Tanks.Where(t3 => t3.StorageTypeId == tankId && t3.StorageId == storageId).ToList(),
                                                    TfxJobId = t1.TfxJobId
                                                }).FirstOrDefaultAsync();
            }
            else
            {
                job = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxJobId == jobId && t.Tanks != null && t.Tanks.Any())
                                                .Project(t1 => new JobWithTanksModel()
                                                {
                                                    TfxDisplayJobId = t1.TfxDisplayJobId,
                                                    Tanks = t1.Tanks
                                                }).FirstOrDefaultAsync();
            }
            if (job.Tanks == null)
            {
                job.Tanks = new List<TankDetail>();
            }
            return job;
        }

        public async Task<List<Demand>> GetDemands(string siteId, List<string> allTankIds, List<string> allStorageIds)
        {
            DataTable TankIds, StorageIds;
            IntializeDemandsParameters(allTankIds, out TankIds);
            var tankIdsParam = new SqlParameter("@TankIds", SqlDbType.Structured);
            tankIdsParam.Value = TankIds;
            tankIdsParam.TypeName = "dbo.DemandsSearchTypes";

            IntializeDemandsParameters(allStorageIds, out StorageIds);
            var storageIdsParam = new SqlParameter("@StorageIds", SqlDbType.Structured);
            storageIdsParam.Value = StorageIds;
            storageIdsParam.TypeName = "dbo.DemandsSearchTypes";

            var siteIdParam = new SqlParameter("@SiteId", SqlDbType.NVarChar);
            siteIdParam.Value = siteId;

            context.Database.CommandTimeout = 500; 
            var demands = await context.Database.SqlQuery<Demand>("usp_GetDemandsBySiteIdTankIdsStorageIds @TankIds,@StorageIds,@SiteId", tankIdsParam, storageIdsParam, siteIdParam).ToListAsync();

            return demands;
        }

        private void IntializeDemandsParameters(List<string> IdList,  out DataTable Ids)
        {
            Ids = CreateTable();
            foreach (var item in IdList.Distinct().ToList())
            {
                var row = Ids.NewRow();
                row["SearchVar"] = item;
                Ids.Rows.Add(row);
            }
        }
        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SearchVar", typeof(string));
            return dt;
        }

        public async Task<TankDrop> GetTankDrop(string siteId, string tankId, string storageId, DateTime captureTime)
        {
            var tankdrop = await context.TankDrops.Where(t => t.SiteId == siteId && t.TankId == tankId && t.StorageId == storageId
                                                                && t.StartTime > captureTime).FirstOrDefaultAsync();
            return tankdrop;
        }

        public string GetRegionDetails(int jobId)
        {
            var regionFilter = Builders<Region>.Filter.And(
                     Builders<Region>.Filter.Where(t => (t.IsActive && !t.IsDeleted && t.TfxJobs.Any(x => x.Id == jobId))));
            var regionInfoDetails = mdbContext.Regions.Find(regionFilter).Project(x => x.Id).FirstOrDefault();
            return regionInfoDetails.ToString();
        }

        public async Task<List<UspTankEstimatedUsage>> GetTankEstimatedUsages(JobWithTanksModel job, DateTime startDate, DateTime endDate, int _timeout = 60)
        {
            var inputmodel = new
            {
                SiteId = job.TfxDisplayJobId,
                TankId = string.Join(",", job.Tanks.Select(t1 => t1.StorageTypeId).Distinct().ToList()),
                StorageId = string.Join(",", job.Tanks.Select(t1 => t1.StorageId).Distinct().ToList()),
                StartDate = startDate,
                EndDate = endDate
            };

            var input = SqlHelperMethods.GetStoredProcedure("usp_GetForecastingTankEstimatedUsage", inputmodel);
            context.Database.CommandTimeout = _timeout;
            var response = await context.Database.SqlQuery<UspTankEstimatedUsage>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<ForecastingDeliveryModel>> GetForecastingTankDeliveryDetails(JobWithTanksModel job)
        {
            var allTankIds = job.Tanks.Select(t1 => t1.StorageTypeId).Distinct().ToList();
            var allStorageIds = job.Tanks.Select(t1 => t1.StorageId).Distinct().ToList();

            var inputmodel = new
            {
                SiteId = job.TfxDisplayJobId,
                TankId = string.Join(",", allTankIds.Distinct().ToList()),
                StorageId = string.Join(",", allStorageIds.Distinct().ToList())
            };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetForecastingTankDeliveryDetails", inputmodel);
            context.Database.CommandTimeout = 30;
            var tankDrops = await context.Database.SqlQuery<ForecastingDeliveryModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return tankDrops;
        }

        public async Task<List<DeliveryRequest>> GetForecastingTankScheduleDetails(JobWithTanksModel job)
        {
            var siteId = job.TfxDisplayJobId;
            var allTankIds = job.Tanks.Select(t1 => t1.StorageTypeId).Distinct().ToList();
            var allStorageIds = job.Tanks.Select(t1 => t1.StorageId).Distinct().ToList();

            var filter = Builders<DeliveryRequest>.Filter.And(
                Builders<DeliveryRequest>.Filter.Where(t => t.TfxDisplayJobId == siteId),
                Builders<DeliveryRequest>.Filter.Where(t => t.IsActive == true),
                Builders<DeliveryRequest>.Filter.Where(t => t.IsDeleted == false),
                Builders<DeliveryRequest>.Filter.In(t => t.StorageId, allStorageIds),
                Builders<DeliveryRequest>.Filter.In(t => t.StorageTypeId, allTankIds)
            );

            var deliveryRequests = await mdbContext.DeliveryRequests.FindAsync(filter);
            return deliveryRequests.ToList();
        }

        public async Task<ForecastingTankDataForChartModel> GetForecastingTankDataForChart(int jobId, DateTime startDtTm, string tankId, string storageId, string uOM)
        {
            var response = new ForecastingTankDataForChartModel();
            var job = new JobWithTanksModel();
            if (!string.IsNullOrEmpty(tankId) && !string.IsNullOrEmpty(storageId))
            {
                job = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxJobId == jobId && t.Tanks != null && t.Tanks.Any())
                                                .Project(t1 => new JobWithTanksModel()
                                                {
                                                    TfxDisplayJobId = t1.TfxDisplayJobId,
                                                    Tanks = t1.Tanks.Where(t3 => t3.StorageTypeId == tankId && t3.StorageId == storageId).ToList()
                                                }).FirstOrDefaultAsync();
            }
            else
            {
                job = await mdbContext.JobAdditionalDetails
                                                .Find(t => t.TfxJobId == jobId && t.Tanks != null && t.Tanks.Any())
                                                .Project(t1 => new JobWithTanksModel()
                                                {
                                                    TfxDisplayJobId = t1.TfxDisplayJobId,
                                                    Tanks = t1.Tanks
                                                }).FirstOrDefaultAsync();
            }

            var lstForecastingTankDetails = new List<TankDetailsForChartModel>();
            var lstTankLevelModel = new List<TankLevelModel>();

            var xAxisTimeSpan = new List<DateTime>();
            startDtTm = new DateTime(startDtTm.Year, startDtTm.Month, startDtTm.Day, startDtTm.Hour, 0, 0);
            TankLevelHelper tankLevelHelper = new TankLevelHelper();
            var jobTanks = job.Tanks.Select(t => t.ToViewModel()).ToList();
            foreach (var item in jobTanks)
            {
                var tankDetailsForChart = tankLevelHelper.GetTankDetails(item);
                var tankLevelModel = new TankLevelModel();
                tankDetailsForChart.TankName = item.TankName;
                tankDetailsForChart.SiteId = job.TfxDisplayJobId;
                tankDetailsForChart.TankId = item.TankId;
                tankDetailsForChart.StorageId = item.StorageId;

                var saleTank = await GetSaleTank(tankDetailsForChart.SiteId, tankDetailsForChart.TankId, tankDetailsForChart.StorageId);
                if (saleTank != null)
                {
                    AddHours(startDtTm, startDtTm.AddDays(2), saleTank.BandPeriod, xAxisTimeSpan);
                    var monthStartDate = DateTime.Now.AddDays(-29).Date;
                    var monthEndDate = DateTime.Now.AddDays(-1).Date;
                    var monthlyDatas = await GetSaleTankMonthlyData(monthStartDate, monthEndDate, saleTank.Id);

                    var tankUllage = await GetTankUllage(tankDetailsForChart.SiteId, tankDetailsForChart.TankId, tankDetailsForChart.StorageId);
                    if (tankUllage != null)
                    {
                        var currentInventory = (saleTank.FuelCapacity - (decimal)tankUllage.Ullage) ?? 0;
                        tankDetailsForChart.Data = GetChartEstimate(monthlyDatas, xAxisTimeSpan, saleTank.BandPeriod, currentInventory);
                        var thisTankLevels = tankLevelHelper.GetTankLevels(monthlyDatas, startDtTm, currentInventory, saleTank.BandPeriod, ApplicationConstants.VisibleHoursOnChart, tankDetailsForChart.Retain, tankDetailsForChart.SafetyStock, tankDetailsForChart.RunOutLevel);
                        thisTankLevels.ForEach(t => t.TankName = tankDetailsForChart.TankName);
                        lstTankLevelModel.AddRange(thisTankLevels);
                    }
                }
                lstForecastingTankDetails.Add(tankDetailsForChart);
            }

            response.XAxisTimeSpan = xAxisTimeSpan.Select(t => t.ToString(Resource.constFormatDateTime)).ToList();
            response.TankDetailsForChart = lstForecastingTankDetails;
            response.TankLevels = lstTankLevelModel;
            return response;
        }

        private List<decimal> GetChartEstimate(List<SaleMonthlyDataModel> monthlyDatas, List<DateTime> timeList, int bandPeriod, decimal currentInventory)
        {
            var response = new List<decimal>();
            if (monthlyDatas.Count == 0)
                return response;

            // Get the start band info to start calculating estimates
            int startBandIndex = 0; int loopCounter = 0;
            var startTime = timeList.First();
            var startBand = monthlyDatas.FirstOrDefault(t => t.BandStartTime >= startTime.TimeOfDay && t.BandEndTime < startTime.TimeOfDay);
            if (startBand != null)
                startBandIndex = monthlyDatas.IndexOf(startBand);

            decimal bandTotalSale = 0;
            int loopEndCounter = ApplicationConstants.VisibleHoursOnChart / bandPeriod;
            response.Add(currentInventory.GetPreciseValue(0));
            while (bandPeriod > 0 && loopCounter < loopEndCounter)
            {
                // This is for calculating estimated hours
                for (int index = startBandIndex; index < monthlyDatas.Count; index++)
                {
                    loopCounter++;
                    bandTotalSale = monthlyDatas[index].AverageSale;
                    currentInventory -= bandTotalSale;
                    if (currentInventory <= 0)
                        response.Add(0);

                    if (currentInventory <= 0 || loopCounter >= loopEndCounter)
                        break;

                    response.Add(currentInventory.GetPreciseValue(0));
                    if (currentInventory > 0 && index == monthlyDatas.Count - 1)
                        index = 0;
                }
            }
            return response;
        }

        private void AddHours(DateTime startDtTm, DateTime EndDtTm, int bandPeriod, List<DateTime> xAxisTimeSpan)
        {
            if (xAxisTimeSpan.Count == 0)
                xAxisTimeSpan.Add(startDtTm);

            if (startDtTm < EndDtTm)
            {
                startDtTm = startDtTm.AddHours(bandPeriod);
                if (!xAxisTimeSpan.Any(t => t == startDtTm))
                {
                    xAxisTimeSpan.Add(startDtTm);
                }
                AddHours(startDtTm, EndDtTm, bandPeriod, xAxisTimeSpan);
            }
        }

        public async Task<bool> CalculateMonthlySales(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 120)
        {
            var inputmodel = new { MonthStartDate = monthStartDate, MonthEndDate = monthEndDate };
            var input = SqlHelperMethods.GetStoredProcedure("usp_CalculateMonthlySales", inputmodel);

            context.Database.CommandTimeout = _timeout;
            await context.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            return true;
        }

        public async Task<bool> CalculateSaleConsumptionRates(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60)
        {
            var inputmodel = new { MonthStartDate = monthStartDate, MonthEndDate = monthEndDate };
            var input = SqlHelperMethods.GetStoredProcedure("usp_CalculateSaleConsumptionRates", inputmodel);

            context.Database.CommandTimeout = _timeout;
            await context.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            return true;
        }

        public async Task<bool> RebuildReorganizeSalesIndexes(int _timeout = 60)
        {
            var inputmodel = new { };
            var input = SqlHelperMethods.GetStoredProcedure("usp_Rebuild_Reorganize_SalesIndexes", inputmodel);

            context.Database.CommandTimeout = _timeout;
            await context.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            return true;
        }

        public async Task<SaleTankModel> GetSaleTank(string siteId, string tankId, string storageId)
        {
            SaleTankModel response = null;
            var saleTank = await context.SaleTanks.FirstOrDefaultAsync(t => t.SiteId == siteId && t.TankId == tankId && t.StorageId == storageId);
            if (saleTank != null)
            {
                response = saleTank.ToViewModel();
            }
            return response;
        }

        public async Task<StatusModel> SaveDsbNotification(DsbNotificationModel dsbNotification)
        {
            var response = new StatusModel();

            var entity = dsbNotification.ToEntity();
            await mdbContext.DsbNotification.InsertOneAsync(entity);
            response.StatusCode = (int)Status.Success;
            return response;
        }

        public async Task<List<DsbNotificationModel>> GetDsbNotification(string regionId)
        {
            var response = new List<DsbNotificationModel>();

            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(30).Date;
            var dsbNotifications = await mdbContext.DsbNotification.Find(x => x.Status == (int)DsbNotificationStatus.UnRead
                    && x.Type == (int)DsbNotificationType.Otto && x.RegionId == regionId).ToListAsync();
            dsbNotifications = dsbNotifications.Where(x => x.CreatedDate.Date >= startDate && x.CreatedDate <= endDate).ToList();
            dsbNotifications = dsbNotifications.OrderByDescending(t => t.CreatedDate).ToList();
            foreach (var item in dsbNotifications)
            {
                response.Add(item.ToViewModel());
            }

            return response;
        }

        public async Task<int> GetDsbNotificationCount(string regionId)
        {
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(30).Date;
            var dsbNotifications = await mdbContext.DsbNotification.Find(x => x.Status == (int)DsbNotificationStatus.UnRead
                    && x.Type == (int)DsbNotificationType.Otto && x.RegionId == regionId
                    ).ToListAsync();
            var response = dsbNotifications.Where(x => x.CreatedDate.Date >= startDate && x.CreatedDate <= endDate).Count();
            return Convert.ToInt32(response);
        }

        public StatusModel UpdateDsbNotificationStatus(string id)
        {
            ObjectId dsbNotificationId = ObjectId.Empty;
            ObjectId.TryParse(id, out dsbNotificationId);
            var response = new StatusModel(Status.Failed);
            try
            {
                var filter = Builders<DsbNotification>.Filter.Where(x => dsbNotificationId == x.Id);
                var update = Builders<DsbNotification>.Update.Set(x => x.Status, (int)DsbNotificationStatus.Read);
                mdbContext.DsbNotification.UpdateOne(filter, update);
                response.StatusCode = (int)Status.Success;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingRepository", "UpdateDsbNotificationStatus", "UpdateDsbNotificationStatus", ex);
            }

            return response;
        }

        public async Task<List<SaleMonthlyDataModel>> GetSaleTankMonthlyData(DateTime monthStartDate, DateTime monthEndDate, int saleTankId, int _timeout = 60)
        {
            var response = new List<SaleMonthlyDataModel>();
            var inputmodel = new { MonthStartDate = monthStartDate, MonthEndDate = monthEndDate, SaleTankId = saleTankId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetSaleTankMonthlyData", inputmodel);

            context.Database.CommandTimeout = _timeout;
            response = await context.Database.SqlQuery<SaleMonthlyDataModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<UspTankUllage> GetTankUllage(string siteId, string tankId, string storageId, int _timeout = 60)
        {
            UspTankUllage response = new UspTankUllage();
            var inputmodel = new { SiteId = siteId, TankId = tankId, StorageId = storageId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetTankUllage", inputmodel);

            context.Database.CommandTimeout = _timeout;
            response = await context.Database.SqlQuery<UspTankUllage>(input.Query, input.Params.ToArray()).FirstOrDefaultAsync();
            return response;
        }

        public async Task<List<TankConsumptionRateModel>> GetTankConsumptionRates(DateTime monthStartDate, DateTime monthEndDate, int saleTankId, int _timeout = 60)
        {
            var response = new List<TankConsumptionRateModel>();
            var inputmodel = new { MonthStartDate = monthStartDate, MonthEndDate = monthEndDate, SaleTankId = saleTankId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetTankConsumptionRates", inputmodel);

            context.Database.CommandTimeout = _timeout;
            response = await context.Database.SqlQuery<TankConsumptionRateModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }
        public List<TankScheduleStatus> GetTankScheduleStatus(string TfxDisplayJobId, List<string> tankIds, List<string> storageIds)
        {
            List<TankScheduleStatus> tankScheduleStatuses = new List<TankScheduleStatus>();
            var deliveryRequestInfo = mdbContext.DeliveryRequests.Find(t => t.TfxDisplayJobId == TfxDisplayJobId && (t.ScheduleShiftEndDateTime == null || t.ScheduleShiftEndDateTime.Value > DateTimeOffset.UtcNow.AddDays(-2).DateTime)
                                            && !t.GroupChildDRs.Any()
                                            && t.IsActive && !t.IsDeleted && (t.ParentId == null || t.ScheduleBuilderId != null)
                                            && (t.TfxScheduleStatus == (int)DeliveryScheduleStatus.None || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.New
                                                   || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Acknowledged || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Accepted || t.TfxScheduleStatus == (int)DeliveryScheduleStatus.Modified)
                                               && t.TfxScheduleEnrouteStatus != (int)EnrouteDeliveryStatus.CompletedDrop
                                            ).Project(t => new { t.Id, t.TfxProductTypeId, t.Status, t.TfxJobId, t.ScheduleShiftEndDateTime, t.JobTimeZoneOffset })
                                            .ToList().GroupBy(t => new { t.TfxJobId, t.TfxProductTypeId }).SelectMany(t => t.ToList()).ToList();

            int missedSchedulePeriod = 4;
            if (deliveryRequestInfo.Any())
            {
                ExchangeAccess exchange = new ExchangeAccess();
                int.TryParse(exchange.GetAppSetting("MissedScheduleWaitingPeriod"), out missedSchedulePeriod);
            }
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            var query = from item in deliveryRequestInfo
                        let jobTime = currentTime.Add(item.JobTimeZoneOffset)
                        where item.ScheduleShiftEndDateTime == null
                        || item.ScheduleShiftEndDateTime.Value.AddHours(missedSchedulePeriod) >= jobTime.DateTime
                        select item;
            deliveryRequestInfo = query.ToList();
            deliveryRequestInfo.ForEach(t => tankScheduleStatuses.Add(new TankScheduleStatus { Id = t.Id.ToString(), Status = t.Status, TfxJobId = t.TfxJobId, TfxProductTypeId = t.TfxProductTypeId }));

            return tankScheduleStatuses;
        }
        public async Task<List<ForecastingTankInformationModel>> GetForecastingTankInformation(List<string> SiteId, List<string> TankId, List<string> StorageId)
        {
            List<ForecastingTankInformationModel> tankInformation = new List<ForecastingTankInformationModel>();

            var forecastingTankInfo = await mdbContext.ForecastingTankInformations.Find(top => SiteId.Contains(top.SiteId) && TankId.Contains(top.TankId) && StorageId.Contains(top.StorageId)).ToListAsync();
            foreach (var item in forecastingTankInfo)
            {
                tankInformation.Add(item.ToEntity());
            }
            return tankInformation;
        }

        public async Task<StatusModel> SaveForecastingCalculation(ForecastingTankInformationModel model)
        {
            var response = new StatusModel();
            if (model != null)
            {
                var forecastingTank = model.ToEntity();
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    session.StartTransaction();
                    try
                    {
                        await mdbContext.ForecastingTankInformations.InsertOneAsync(forecastingTank);
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                    }
                    catch (Exception)
                    {
                        await session.AbortTransactionAsync();
                        throw;
                    }
                }
            }
            return response;
        }
        public async Task<StatusModel> UpdateForecastingCalculation(ForecastingTankInformationModel model)
        {
            var response = new StatusModel();
            if (model != null)
            {
                var forecastingTank = model.ToEntity();
                using (var session = await mdbContext.Client.StartSessionAsync())
                {
                    session.StartTransaction();
                    try
                    {
                        var tankInfo = model.TankInformation.ToTankInfoEntity();
                        var updateFields = Builders<ForecastingTankInformation>.Update
                                           .Set(t => t.DaysLeft, model.DaysLeft)
                                           .Set(t => t.EstimatedCurrentInventory, model.EstimatedCurrentInventory)
                                           .Set(t => t.TankInformation, tankInfo);
                        var filter = Builders<ForecastingTankInformation>.Filter.And(
                             Builders<ForecastingTankInformation>.Filter.Where(x => x.SiteId == model.SiteId),
                             Builders<ForecastingTankInformation>.Filter.Where(x => x.TankId == model.TankId),
                             Builders<ForecastingTankInformation>.Filter.Where(x => x.StorageId == model.StorageId));

                        await mdbContext.ForecastingTankInformations.UpdateOneAsync(filter, updateFields);
                        await session.CommitTransactionAsync();
                        response.StatusCode = (int)Status.Success;
                    }
                    catch (Exception)
                    {
                        await session.AbortTransactionAsync();
                        throw;
                    }
                }
            }
            return response;
        }
        public async Task<List<SaleTankModel>> GetSaleTank(List<string> siteId, List<string> tankId, List<string> storageId)
        {
            List<SaleTankModel> response = new List<SaleTankModel>();
            var saleTank = await context.SaleTanks.Where(t => siteId.Contains(t.SiteId) && tankId.Contains(t.TankId) && storageId.Contains(t.StorageId)).ToListAsync();
            if (saleTank != null && saleTank.Any())
            {
                saleTank.ForEach(x => response.Add(x.ToViewModel()));

            }
            return response;
        }
    }
}
