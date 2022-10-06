using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ForcastingHelpers;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.FreightRepository
{
    public interface IForecastingRepository
    {
        Task<List<SaleDemandModel>> GetForecastingDemandData(DateTime startDate, DateTime endDate, int _timeout = 60);
        Task<List<SaleTankDropModel>> GetForecastingTankDropData(DateTime startDate, DateTime endDate, int _timeout = 60);
        Task<StatusModel> SaveDailySale(ISale24HourModel sale24Hours);
        Task<StatusModel> SaveMonthlySale(List<ISale24HourModel> sale24HourModels, int _timeout = 60);
        Task<List<ForecastingDeliveryModel>> GetForecastingTankDeliveryDetails(JobWithTanksModel job);
        Task<List<DeliveryRequest>> GetForecastingTankScheduleDetails(JobWithTanksModel job);
        Task<ForecastingTankDataForChartModel> GetForecastingTankDataForChart(int jobId, DateTime startDtTm, string tankId, string storageId, string uOM);
        Task<bool> CalculateMonthlySales(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60);
        Task<bool> CalculateSaleConsumptionRates(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60);
        Task<bool> RebuildReorganizeSalesIndexes(int _timeout = 60);
        Task<SaleTankModel> GetSaleTank(string siteId, string tankId, string storageId);
        Task<List<SaleMonthlyDataModel>> GetSaleTankMonthlyData(DateTime monthStartDate, DateTime monthEndDate, int saleTankId, int _timeout = 60);
        Task<UspTankUllage> GetTankUllage(string siteId, string tankId, string storageId, int _timeout = 60);
        Task<JobWithTanksModel> GetJobWithTanks(int jobId, string tankId, string storageId);
        Task<List<Demand>> GetDemands(string siteId, List<string> allTankIds, List<string> allStorageIds);
        Task<TankDrop> GetTankDrop(string siteId, string tankId, string storageId, DateTime captureTime);
        string GetRegionDetails(int jobId);
        Task<List<UspTankEstimatedUsage>> GetTankEstimatedUsages(JobWithTanksModel job, DateTime startDate, DateTime endDate, int _timeout = 60);
        Task<StatusModel> SaveDsbNotification(DsbNotificationModel dsbNotification);
        Task<List<DsbNotificationModel>> GetDsbNotification(string regionId);
        Task<int> GetDsbNotificationCount(string regionId);
        StatusModel UpdateDsbNotificationStatus(string id);
        List<TankScheduleStatus> GetTankScheduleStatus(string TfxDisplayJobId, List<string> tankIds, List<string> storageIds);
        Task<StatusModel> SaveForecastingCalculation(ForecastingTankInformationModel model);
        Task<List<ForecastingTankInformationModel>> GetForecastingTankInformation(List<string> SiteId, List<string> TankId, List<string> StorageId);
        Task<StatusModel> UpdateForecastingCalculation(ForecastingTankInformationModel model);
        Task<List<SaleTankModel>> GetSaleTank(List<string> siteId, List<string> tankId, List<string> storageId);
    }
}
