using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ForcastingHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IForecastingDomain
    {
        Task<StatusModel> ProcessDailySalesCalculation(DateTime startTime, DateTime endTime);
        Task<StatusModel> ProcessMonthlySalesCalculation(DateTime startTime, DateTime endTime);
        Task<List<ForecastingTankDetailsModel>> GetForecastingTankDetails(int jobId, string tankId, string storageId, string uOM,string jobTimeZone);
        Task<List<ForecastingEstimatedUsageModel>> GetForecastingTankEstimatedUsageDetails(int jobId, string startDate, string endDate, string tankId, string storageId, string uOM);
        Task<List<ForecastingInventoryModel>> GetForecastingTankInventoryDetails(int jobId, string tankId, string storageId, string uOM);
        Task<List<ForecastingDeliveryModel>> GetForecastingTankDeliveryDetails(int jobId, string tankId, string storageId, string uOM);
        Task<List<ForecastingExistingScheduleModel>> GetForecastingTankScheduleDetails(int jobId, string tankId, string storageId, string uOM);
        Task<ForecastingTankDataForChartModel> GetForecastingTankDataForChart(int jobId, DateTime startDtTm, string tankId, string storageId, string uOM);
        Task<bool> CalculateMonthlySales(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60);
        Task<bool> CalculateSaleConsumptionRates(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60);
        Task<bool> RebuildReorganizeSalesIndexes(int _timeout = 60);
        Task<TankEstimate> CalculateEstimate(string siteId, string tankId, string storageId);
        Task<TankRetainInfo> CalculateTankRetainWindowInfo(UspRetainWindowInfo uspRetainWindowInfo);
        Task<StatusModel> SaveDsbNotification(DsbNotificationModel dsbNotification);
        Task<List<DsbNotificationModel>> GetDsbNotification(string regionId);
        Task<int> GetDsbNotificationCount(string regionId);
        StatusModel UpdateDsbNotificationStatus(string id);
        Task<StatusModel> ProcessforcastingTankCaculation(List<UspForecastingTankInfomation> uspForecastingTankInfomations);
    }
}
