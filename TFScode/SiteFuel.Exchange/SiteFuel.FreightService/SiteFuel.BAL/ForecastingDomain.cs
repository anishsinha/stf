using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ForcastingHelpers;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess;

namespace SiteFuel.BAL
{
    public class ForecastingDomain : IForecastingDomain
    {
        private DemandCaptureContext context;
        private IForecastingRepository _forecastingRepository;
        public ForecastingDomain(IForecastingRepository forecastingRepository)
        {
            _forecastingRepository = forecastingRepository;
            context = new DemandCaptureContext();
        }

        public async Task<StatusModel> ProcessDailySalesCalculation(DateTime startTime, DateTime endTime)
        {
            var response = new StatusModel();
            try
            {
                var demands = await _forecastingRepository.GetForecastingDemandData(startTime, endTime);
                var tankDrops = await _forecastingRepository.GetForecastingTankDropData(startTime, endTime);
                var allTanks = demands.Select(t => new { t.SiteId, t.TankId, t.StorageId, t.BandPeriod, t.StartTime }).Distinct().ToList();
                var saleTanks = allTanks.Select(t => new SaleTankModel { SiteId = t.SiteId, TankId = t.TankId, StorageId = t.StorageId, BandPeriod = t.BandPeriod, StartTime = t.StartTime }).ToList();
                foreach (var thisTank in saleTanks)
                {
                    var thisTankDemands = demands.Where(t => t.SiteId == thisTank.SiteId && t.TankId == thisTank.TankId && t.StorageId == thisTank.StorageId).ToList();
                    var thisTankDrops = tankDrops.Where(t => t.SiteId == thisTank.SiteId && t.TankId == thisTank.TankId && t.StorageId == thisTank.StorageId).ToList();
                    var salesCalculator = new SalesCalculatorDaily(startTime.Date);
                    salesCalculator.Demands.AddRange(thisTankDemands);
                    salesCalculator.TankDrops.AddRange(thisTankDrops);
                    var sales24Hours = salesCalculator.Calculate(thisTank);
                    response = await _forecastingRepository.SaveDailySale(sales24Hours);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "ProcessDailySalesCalculation", ex.Message + $" : startTime:{startTime}, endTime:{endTime}", ex);
            }
            return response;
        }

        public async Task<StatusModel> ProcessMonthlySalesCalculation(DateTime startTime, DateTime endTime)
        {
            var response = new StatusModel();
            try
            {
                var demands = await _forecastingRepository.GetForecastingDemandData(startTime, endTime);
                var tankDrops = await _forecastingRepository.GetForecastingTankDropData(startTime, endTime);
                var allTanks = demands.Select(t => new { t.SiteId, t.TankId, t.StorageId, t.BandPeriod, t.StartTime, t.MaxFill, t.FillType, t.FuelCapacity, t.Retain, t.SaftyStock, t.Runout, t.InventoryUoM }).Distinct().ToList();
                var saleTanks = allTanks.Select(t => new SaleTankModel { SiteId = t.SiteId, TankId = t.TankId, StorageId = t.StorageId, BandPeriod = t.BandPeriod, StartTime = t.StartTime, MaxFill = t.MaxFill, FillType = t.FillType, FuelCapacity = t.FuelCapacity, Retain = t.Retain, SaftyStock = t.SaftyStock, Runout = t.Runout, InventoryUoM = t.InventoryUoM }).ToList();
                for (int index = 0; index < saleTanks.Count; index++)
                {
                    var thisTank = saleTanks[index];
                    var thisTankDemands = demands.Where(t => t.SiteId == thisTank.SiteId && t.TankId == thisTank.TankId && t.StorageId == thisTank.StorageId).ToList();
                    var thisTankDrops = tankDrops.Where(t => t.SiteId == thisTank.SiteId && t.TankId == thisTank.TankId && t.StorageId == thisTank.StorageId).ToList();
                    var salesCalculator = new SalesCalculatorMonthly(startTime.Date);
                    salesCalculator.Demands.AddRange(thisTankDemands);
                    salesCalculator.TankDrops.AddRange(thisTankDrops);
                    var sales24Hours = salesCalculator.Calculate(thisTank);
                    response = await _forecastingRepository.SaveMonthlySale(sales24Hours);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "ProcessMonthlySalesCalculation", ex.Message + $" : startTime:{startTime}, endTime:{endTime}", ex);
            }
            return response;
        }

        public async Task<List<ForecastingTankDetailsModel>> GetForecastingTankDetails(int jobId, string tankId, string storageId, string uOM, string jobTimeZone)
        {
            List<ForecastingTankDetailsModel> response = new List<ForecastingTankDetailsModel>();
            try
            {
                var regionInfoDetails = _forecastingRepository.GetRegionDetails(jobId);
                var job = await _forecastingRepository.GetJobWithTanks(jobId, tankId, storageId);
                foreach (var item in job.Tanks)
                {
                    var forecastingTankDetail = new ForecastingTankDetailsModel();
                    forecastingTankDetail.TankName = item.TankName;
                    forecastingTankDetail.SiteId = job.TfxDisplayJobId;
                    forecastingTankDetail.TankId = item.StorageTypeId;
                    forecastingTankDetail.StorageId = item.StorageId;
                    forecastingTankDetail.FuelCapacity = item.FuelCapacity.HasValue ? (float)item.FuelCapacity.Value : 0;
                    forecastingTankDetail.ProductType = item.TfxProductTypeName;
                    forecastingTankDetail.TfxProductTypeId = item.TfxProductTypeId;
                    forecastingTankDetail.MaxFillQuantity = GetTankCapacity(item.MaxFill, item.FuelCapacity, item.FillType.Value) == null ? string.Empty : GetTankCapacity(item.MaxFill, item.FuelCapacity, item.FillType.Value).Value.GetCommaSeperatedValue().ToString() + " " + uOM;
                    forecastingTankDetail.RegionId = regionInfoDetails != null ? regionInfoDetails : string.Empty;
                    forecastingTankDetail.TankSequence = item.TankSequence;
                    response.Add(forecastingTankDetail);
                }

                var siteId = job.TfxDisplayJobId;
                var allTankIds = job.Tanks.Select(t1 => t1.StorageTypeId).Distinct().ToList();
                var allStorageIds = job.Tanks.Select(t1 => t1.StorageId).Distinct().ToList();
                var demands = await _forecastingRepository.GetDemands(siteId, allTankIds, allStorageIds);

                var tankScheduleDetails = _forecastingRepository.GetTankScheduleStatus(siteId, allTankIds, allStorageIds);
                var tankForecastingCaculationInfo = await _forecastingRepository.GetForecastingTankInformation(new List<string> { siteId }, allTankIds, allStorageIds);
                var respData = await GetSalesAsync(new List<string> { siteId }, allTankIds, allStorageIds);
                var tStatus = Resource.valMessageNoDR;
                foreach (var item in response)
                {
                    tStatus = Resource.valMessageNoDR;
                    if (tankScheduleDetails != null && tankScheduleDetails.Any())
                    {
                        var tankSchedules = tankScheduleDetails.Where(t => t.TfxJobId == jobId && t.TfxProductTypeId == item.TfxProductTypeId).ToList();
                        if (tankSchedules != null && tankSchedules.Any())
                        {
                            if (tankSchedules.All(t => t.Status == DeliveryReqStatus.ScheduleCreated)) { tStatus = Resource.lblScheduled; }
                            else tStatus = Resource.valMessageDRCreated;
                        }
                        item.Status = tStatus;
                    }
                    else
                    {
                        item.Status = Resource.valMessageNoDR;
                    }
                    var demandDetails = demands.FirstOrDefault(t => t.SiteId == item.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId);
                    if (demandDetails != null)
                    {

                        item.LastInventoryReading = ((decimal)demandDetails.NetVolume).GetPreciseValue(1).GetCommaSeperatedValue() + " " + uOM;
                        item.LastReadingTime = demandDetails.CaptureTime.ToString(Resource.constFormatDateTime);
                        item.UllageSinceLastReading = ((decimal)demandDetails.Ullage).GetPreciseValue(1).GetCommaSeperatedValue() + " " + uOM;
                        item.TankFill = (float)(Math.Round((demandDetails.NetVolume / item.FuelCapacity) * 100, 1));
                        item.TankFillRemaining = (float)(Math.Round((demandDetails.Ullage / item.FuelCapacity) * 100, 1));
                        TimeSpan ts = (DateTimeOffset.Now.ToTargetDateTimeOffset(jobTimeZone).DateTime - demandDetails.CaptureTime);
                        item.TankInventoryDiffinHrs = ts.TotalHours;
                    }
                    IntializeSalesTankInformation(uOM, job, respData, item);
                    if (respData != null && respData.SalesData.Any())
                    {
                        //set the tank days remaining.
                        var tankDaysRemaining = respData.SalesData.FirstOrDefault(t => t.SiteId == item.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId);
                        if (tankDaysRemaining != null && !string.IsNullOrEmpty(tankDaysRemaining.DaysRemaining) && !tankDaysRemaining.DaysRemaining.Contains("--"))
                        {
                            tankDaysRemaining.DaysRemaining = tankDaysRemaining.DaysRemaining.Replace("--", string.Empty).Trim();
                            item.DaysLeft = (float)Convert.ToDouble(tankDaysRemaining.DaysRemaining).GetPreciseValue(1);
                        }
                    }
                    var tankForecastingInfo = tankForecastingCaculationInfo.FirstOrDefault(top => top.SiteId == item.SiteId && top.TankId == item.TankId && top.StorageId == item.StorageId);
                    if (tankForecastingInfo != null)
                    {
                        item.EstimatedCurrentInventory = tankForecastingInfo.EstimatedCurrentInventory.GetPreciseValue(1).GetCommaSeperatedValue() + " " + uOM;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "GetForecastingTankDetails", ex.Message, ex);
            }
            response = response.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();
            return response;
        }





        public async Task<List<ForecastingEstimatedUsageModel>> GetForecastingTankEstimatedUsageDetails(int jobId, string startDate, string endDate, string tankId, string storageId, string uOM)
        {
            List<ForecastingEstimatedUsageModel> response = new List<ForecastingEstimatedUsageModel>();
            try
            {
                DateTime startdateTime = DateTime.Now;
                DateTime.TryParse(startDate, out startdateTime);
                DateTime endDatedateTime = DateTime.Now;
                DateTime.TryParse(endDate, out endDatedateTime);
                string usagePeriod = (endDatedateTime - startdateTime).TotalDays + " Days";
                var job = await _forecastingRepository.GetJobWithTanks(jobId, tankId, storageId);
                foreach (var item in job.Tanks)
                {
                    var forecastingEstimatedUsage = new ForecastingEstimatedUsageModel();
                    forecastingEstimatedUsage.TankName = item.TankName;
                    forecastingEstimatedUsage.SiteId = job.TfxDisplayJobId;
                    forecastingEstimatedUsage.TankId = item.StorageTypeId;
                    forecastingEstimatedUsage.StorageId = item.StorageId;
                    response.Add(forecastingEstimatedUsage);
                }

                var estimatedUsage = await _forecastingRepository.GetTankEstimatedUsages(job, startdateTime.Date, endDatedateTime.Date);
                var dayCount = endDatedateTime.Day - startdateTime.Day;
                dayCount = Math.Max(dayCount, 1);

                foreach (var item in response)
                {
                    item.UsagePeriod = usagePeriod;
                    var weekdaysData = estimatedUsage.Where(t => t.SiteId == item.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId).ToList();
                    UpdateCalculatedEsitmates(weekdaysData, item, dayCount, uOM);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "GetForecastingTankEstimatedUsageDetails", ex.Message, ex);
            }
            return response;
        }

        private void UpdateCalculatedEsitmates(List<UspTankEstimatedUsage> uspTankEstimatedUsages, ForecastingEstimatedUsageModel objectToUpdate, int dayCount, string uOM)
        {
            int loopCounter = 0; decimal totalUsages = 0;
            uspTankEstimatedUsages = uspTankEstimatedUsages.OrderBy(t => t.DayId).ToList();
            for (int index = 0; index < uspTankEstimatedUsages.Count; index++)
            {
                loopCounter++;
                totalUsages += uspTankEstimatedUsages[index].AverageSale;
                if (dayCount == loopCounter)
                    break;

                if (index == uspTankEstimatedUsages.Count - 1)
                    index = 0;
            }
            objectToUpdate.TotalExceptedUsage = totalUsages.GetPreciseValue(2).GetCommaSeperatedValue() + " " + uOM;
            if (uspTankEstimatedUsages != null && uspTankEstimatedUsages.Count > 0)
                objectToUpdate.MaximumBusinessdayUsage = uspTankEstimatedUsages.Max(t => t.AverageSale).GetPreciseValue(2).GetCommaSeperatedValue() + " " + uOM;
            else
                objectToUpdate.MaximumBusinessdayUsage = Resource.lblHyphen;

            objectToUpdate.AverageBusinessDayUsage = (totalUsages / dayCount).GetPreciseValue(2).GetCommaSeperatedValue() + " " + uOM;
        }

        public async Task<List<ForecastingInventoryModel>> GetForecastingTankInventoryDetails(int jobId, string tankId, string storageId, string uOM)
        {
            List<ForecastingInventoryModel> response = new List<ForecastingInventoryModel>();
            try
            {
                var job = await _forecastingRepository.GetJobWithTanks(jobId, tankId, storageId);
                foreach (var item in job.Tanks)
                {
                    var forecastingInventoryDetail = new ForecastingInventoryModel();
                    forecastingInventoryDetail.TankName = item.TankName;
                    forecastingInventoryDetail.SiteId = job.TfxDisplayJobId;
                    forecastingInventoryDetail.TankId = item.StorageTypeId;
                    forecastingInventoryDetail.StorageId = item.StorageId;
                    forecastingInventoryDetail.FuelCapacity = item.FuelCapacity.HasValue ? (float)item.FuelCapacity.Value.GetPreciseValue(1) : 0;
                    if (item.PhysicalPumpStop.HasValue)
                    {
                        forecastingInventoryDetail.PhysicalPumpStop = (float)item.PhysicalPumpStop.Value;
                        forecastingInventoryDetail.PhysicalPumpStopQty = ((decimal)(item.PhysicalPumpStop * item.FuelCapacity) / 100).GetPreciseValue(1).GetCommaSeperatedValue() + " " + uOM;
                    }

                    //Here RunOutLevel and RunOutLevelQty is Must Go
                    if (item.FillType.Value == (int)FillType.Percent)
                    {
                        forecastingInventoryDetail.RunOutLevel = (float)item.MinFill;
                        forecastingInventoryDetail.RunOutLevelQty = ((decimal)(item.MinFill * item.FuelCapacity) / 100).GetPreciseValue(1).GetCommaSeperatedValue() + " " + uOM;
                    }
                    else if (item.FillType.Value == (int)FillType.UoM)
                    {
                        forecastingInventoryDetail.RunOutLevelQty = item.MinFill.Value.GetPreciseValue(2).GetCommaSeperatedValue() + " " + uOM;
                        forecastingInventoryDetail.RunOutLevel = (((float)item.MinFill * 100) / (float)item.FuelCapacity).GetPreciseValue(1);
                    }

                    //SafetyStock Means Should Go
                    //Should Go = RunOutLevel //In Mongo DB
                    if (item.RunOutLevel.HasValue)
                    {
                        forecastingInventoryDetail.SafetyStock = (float)item.RunOutLevel.Value;
                        forecastingInventoryDetail.SafetyStockQty = ((decimal)(item.RunOutLevel.Value * item.FuelCapacity) / 100).GetPreciseValue(1).GetCommaSeperatedValue() + " " + uOM;
                    }
                    forecastingInventoryDetail.TankSequence = item.TankSequence;
                    response.Add(forecastingInventoryDetail);
                }

                var siteId = job.TfxDisplayJobId;
                var allTankIds = job.Tanks.Select(t1 => t1.StorageTypeId).Distinct().ToList();
                var allStorageIds = job.Tanks.Select(t1 => t1.StorageId).Distinct().ToList();

                var demands = await _forecastingRepository.GetDemands(siteId, allTankIds, allStorageIds);
                foreach (var item in response)
                {
                    var demandDetails = demands.FirstOrDefault(t => t.SiteId == item.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId);
                    if (demandDetails != null)
                    {
                        item.InventoryLevelQty = ((decimal)demandDetails.NetVolume.GetPreciseValue(1)).GetCommaSeperatedValue() + " " + uOM;
                        item.InventoryLevel = ((demandDetails.NetVolume / item.FuelCapacity) * 100).GetPreciseValue(1);

                        item.Ullage = ((decimal)demandDetails.Ullage.GetPreciseValue(1)).GetCommaSeperatedValue() + " " + uOM;
                        item.PrevInventoryReading = demandDetails.CaptureTime.ToString(Resource.constFormatDateTime);
                    }
                }

                response = response.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "GetForecastingTankInventoryDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingDeliveryModel>> GetForecastingTankDeliveryDetails(int jobId, string tankId, string storageId, string uOM)
        {
            List<ForecastingDeliveryModel> response = new List<ForecastingDeliveryModel>();
            try
            {
                var job = await _forecastingRepository.GetJobWithTanks(jobId, tankId, storageId);
                foreach (var item in job.Tanks)
                {
                    var forecastingDeliveryDetail = new ForecastingDeliveryModel();
                    forecastingDeliveryDetail.TankName = item.TankName;
                    forecastingDeliveryDetail.SiteId = job.TfxDisplayJobId;
                    forecastingDeliveryDetail.TankId = item.StorageTypeId;
                    forecastingDeliveryDetail.StorageId = item.StorageId;
                    forecastingDeliveryDetail.TankSequence = item.TankSequence;
                    response.Add(forecastingDeliveryDetail);
                }

                var tankDrops = await _forecastingRepository.GetForecastingTankDeliveryDetails(job);
                foreach (var item in response)
                {
                    var tankDropDetails = tankDrops.FirstOrDefault(t => t.SiteId == item.SiteId && t.TankId == item.TankId && t.StorageId == item.StorageId);
                    if (tankDropDetails != null)
                    {
                        item.LastDeliveredDate = tankDropDetails.LastDeliveredDateTime.ToString(Resource.constFormatDateTime);
                        item.LastDeliveredQty = tankDropDetails.LastDeliveredQtyValue.GetPreciseValue(2).GetCommaSeperatedValue() + " " + uOM;
                        item.NoOfDeliveries = tankDropDetails.NoOfDeliveries;
                    }
                    else
                    {
                        item.LastDeliveredDate = Resource.lblHyphen;
                        item.LastDeliveredQty = Resource.lblHyphen;
                    }
                }
                response = response.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();

                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "GetForecastingTankDeliveryDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ForecastingExistingScheduleModel>> GetForecastingTankScheduleDetails(int jobId, string tankId, string storageId, string uOM)
        {
            List<ForecastingExistingScheduleModel> response = new List<ForecastingExistingScheduleModel>(); ;
            try
            {
                var job = await _forecastingRepository.GetJobWithTanks(jobId, tankId, storageId);
                foreach (var item in job.Tanks)
                {
                    var forecastingDeliveryDetail = new ForecastingExistingScheduleModel();
                    forecastingDeliveryDetail.TankName = item.TankName;
                    forecastingDeliveryDetail.SiteId = job.TfxDisplayJobId;
                    forecastingDeliveryDetail.TankId = item.StorageTypeId;
                    forecastingDeliveryDetail.StorageId = item.StorageId;
                    forecastingDeliveryDetail.TankSequence = item.TankSequence;
                    response.Add(forecastingDeliveryDetail);
                }

                var deliveryRequests = await _forecastingRepository.GetForecastingTankScheduleDetails(job);
                foreach (var item in response)
                {
                    item.ExistingDeliverySchedule = deliveryRequests.Count(t => t.TfxDisplayJobId == item.SiteId && t.StorageTypeId == item.TankId
                                            && t.StorageId == item.StorageId && (t.TfxScheduleStatus != (int)DeliveryScheduleStatus.Completed && t.TfxScheduleStatus != (int)DeliveryScheduleStatus.CompletedLate &&
                                                t.TfxScheduleStatus != (int)DeliveryScheduleStatus.RescheduledCompleted && t.TfxScheduleStatus != 0));

                    item.DeliveryRequest = deliveryRequests.Count(t => t.TfxDisplayJobId == item.SiteId && t.StorageTypeId == item.TankId && t.StorageId == item.StorageId
                                           && string.IsNullOrEmpty(t.TfxScheduleStatusName));

                }
                response = response.OrderBy(t1 => t1.TankSequence == null || t1.TankSequence == 0 ? 99999 : t1.TankSequence).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "GetForecastingTankScheduleDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<ForecastingTankDataForChartModel> GetForecastingTankDataForChart(int jobId, DateTime startDtTm, string tankId, string storageId, string uOM)
        {
            ForecastingTankDataForChartModel response = null;
            try
            {
                response = await _forecastingRepository.GetForecastingTankDataForChart(jobId, startDtTm, tankId, storageId, uOM);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "GetForecastingTankDataForChart", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> CalculateMonthlySales(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60)
        {
            bool result = false;
            try
            {
                result = await _forecastingRepository.CalculateMonthlySales(monthStartDate, monthEndDate, _timeout);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "CalculateMonthlySales", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> CalculateSaleConsumptionRates(DateTime monthStartDate, DateTime monthEndDate, int _timeout = 60)
        {
            bool result = false;
            try
            {
                result = await _forecastingRepository.CalculateSaleConsumptionRates(monthStartDate, monthEndDate, _timeout);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "CalculateSaleConsumptionRates", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> RebuildReorganizeSalesIndexes(int _timeout = 60)
        {
            bool result = false;
            try
            {
                result = await _forecastingRepository.RebuildReorganizeSalesIndexes(_timeout);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "RebuildReorganizeSalesIndexes", ex.Message, ex);
            }
            return result;
        }

        public async Task<TankEstimate> CalculateEstimate(string siteId, string tankId, string storageId)
        {
            TankEstimate response = new TankEstimate();
            try
            {
                var saleTank = await _forecastingRepository.GetSaleTank(siteId, tankId, storageId);
                if (saleTank != null)
                {
                    var monthStartDate = DateTime.Now.AddDays(-29).Date;
                    var monthEndDate = DateTime.Now.AddDays(-1).Date;
                    var monthlyDatas = await _forecastingRepository.GetSaleTankMonthlyData(monthStartDate, monthEndDate, saleTank.Id);

                    var tankUllage = await _forecastingRepository.GetTankUllage(siteId, tankId, storageId);
                    if (tankUllage != null)
                    {
                        var currentInventory = (saleTank.FuelCapacity - (decimal)tankUllage.Ullage) ?? 0;
                        LogManager.Logger.WriteDebug("GetEstimate", "GetEstimate", DateTime.Now.Ticks.ToString());
                        response = await Task.Run(() => GetEstimate(monthlyDatas, saleTank.BandPeriod, currentInventory));
                        LogManager.Logger.WriteDebug("GetEstimate", "GetEstimate", DateTime.Now.Ticks.ToString());
                        var bandTime = tankUllage.CaptureTime.TimeOfDay;
                        var estimatedTime = (DateTime.Now - tankUllage.CaptureTime).TotalMinutes;
                        var estimatedBand = monthlyDatas.FirstOrDefault(t => t.BandStartTime >= bandTime && t.BandEndTime <= bandTime);
                        if (estimatedBand != null)
                        {
                            var bandAvgInMintes = estimatedBand.AverageSale / 60;
                            response.CurrentInventory = currentInventory - (bandAvgInMintes * (decimal)estimatedTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "CalculateHours", ex.Message, ex);
            }
            return response;
        }

        private TankEstimate GetEstimate(List<SaleMonthlyDataModel> monthlyDatas, int bandPeriod, decimal currentInventory)
        {
            decimal totalHours = 0;
            decimal totalSale = 0;
            decimal bandTotalSale = 0;

            // This is for calculating estimated hours
            if (monthlyDatas.Any(x => x.TotalSale > 0))
            {
                for (int index = 0; index < monthlyDatas.Count; index++)
                {
                    bandTotalSale = monthlyDatas[index].TotalSale;
                    if (totalSale + bandTotalSale > currentInventory)
                        break;

                    totalHours += bandPeriod;
                    if (bandTotalSale < 0)
                        continue;

                    totalSale += bandTotalSale;
                    if (totalSale < currentInventory && index == monthlyDatas.Count - 1)
                        index = 0;
                }

                // This is for calculating estimated fraction hours
                if (totalSale < currentInventory)
                {
                    for (int index = 0; index < monthlyDatas.Count; index++)
                    {
                        bandTotalSale = monthlyDatas[index].AverageSale;
                        for (int index2 = 0; index2 < bandPeriod; index2++)
                        {
                            if (totalSale + bandTotalSale > currentInventory)
                            {
                                totalHours += (currentInventory - totalSale) / bandTotalSale;
                                totalSale += (currentInventory - totalSale);
                                break;
                            }

                            totalHours += 1;
                            totalSale += bandTotalSale;
                        }
                        if (totalSale + bandTotalSale > currentInventory)
                            break;
                    }
                }
            }
            TankEstimate response = new TankEstimate() { Hours = totalHours, Usages = totalSale };
            return response;
        }
        public async Task<TankRetainInfo> CalculateTankRetainWindowInfo(UspRetainWindowInfo uspRetainWindowInfo)
        {
            TankEstimate response = new TankEstimate();
            TankRetainInfo tankRetainInfo = new TankRetainInfo();
            try
            {
                var saleTank = await _forecastingRepository.GetSaleTank(uspRetainWindowInfo.siteId, uspRetainWindowInfo.tankId, uspRetainWindowInfo.storageId);
                if (saleTank != null)
                {
                    var monthStartDate = DateTime.Now.AddDays(-29).Date;
                    var monthEndDate = DateTime.Now.AddDays(-1).Date;
                    var monthlyDatas = await _forecastingRepository.GetSaleTankMonthlyData(monthStartDate, monthEndDate, saleTank.Id);

                    var tankUllage = await _forecastingRepository.GetTankUllage(uspRetainWindowInfo.siteId, uspRetainWindowInfo.tankId, uspRetainWindowInfo.storageId);
                    var currentInventory = (uspRetainWindowInfo.Quantity - (decimal)tankUllage.Ullage);
                    response = GetEstimate(monthlyDatas, saleTank.BandPeriod, currentInventory);
                    if (response != null)
                    {
                        CaculateRetainWindowInfo(uspRetainWindowInfo, response, tankRetainInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "CalculateTankRetainWindowInfo", ex.Message, ex);
            }
            return tankRetainInfo;
        }

        public async Task<StatusModel> SaveDsbNotification(DsbNotificationModel dsbNotification)
        {
            var result = await _forecastingRepository.SaveDsbNotification(dsbNotification);
            return result;
        }

        public async Task<List<DsbNotificationModel>> GetDsbNotification(string regionId)
        {
            var result = await _forecastingRepository.GetDsbNotification(regionId);
            return result;
        }

        public async Task<int> GetDsbNotificationCount(string regionId)
        {
            var result = await _forecastingRepository.GetDsbNotificationCount(regionId);
            return result;
        }

        public StatusModel UpdateDsbNotificationStatus(string id)
        {
            var result = _forecastingRepository.UpdateDsbNotificationStatus(id);
            return result;
        }

        private static void CaculateRetainWindowInfo(UspRetainWindowInfo uspRetainWindowInfo, TankEstimate response, TankRetainInfo tankRetainInfo)
        {
            var retainDateTime = DateTime.Now.AddHours((double)response.Hours);
            if (uspRetainWindowInfo.maxBufferUOM == (int)RateOfConsumsionUOM.Hours)
            {
                retainDateTime = retainDateTime.AddHours(-uspRetainWindowInfo.maxBuffer);
            }
            else
            {
                retainDateTime = retainDateTime.AddDays(-uspRetainWindowInfo.maxBuffer);
            }
            tankRetainInfo.RetainTime = retainDateTime.ToString("hh:mm tt");
            tankRetainInfo.RetainDate = retainDateTime.ToString("MM/dd/yyyy");
            if (uspRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours)
            {
                tankRetainInfo.WindowStartTime = retainDateTime.AddHours(-uspRetainWindowInfo.startBuffer).ToString("hh:mm tt");
                tankRetainInfo.WindowStartDate = retainDateTime.AddHours(-uspRetainWindowInfo.startBuffer).ToString("MM/dd/yyyy");
            }
            else
            {
                tankRetainInfo.WindowStartTime = retainDateTime.AddDays(-uspRetainWindowInfo.startBuffer).ToString("hh:mm tt");
                tankRetainInfo.WindowStartDate = retainDateTime.AddDays(-uspRetainWindowInfo.startBuffer).ToString("MM/dd/yyyy");
            }
            if (uspRetainWindowInfo.endBufferUOM == (int)RateOfConsumsionUOM.Hours)
            {
                tankRetainInfo.WindowEndTime = retainDateTime.AddHours(uspRetainWindowInfo.endBuffer).ToString("hh:mm tt");
                tankRetainInfo.WindowEndDate = retainDateTime.AddHours(uspRetainWindowInfo.endBuffer).ToString("MM/dd/yyyy");
            }
            else
            {
                tankRetainInfo.WindowEndTime = retainDateTime.AddDays(uspRetainWindowInfo.endBuffer).ToString("hh:mm tt");
                tankRetainInfo.WindowEndDate = retainDateTime.AddDays(uspRetainWindowInfo.endBuffer).ToString("MM/dd/yyyy");
            }
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

                context.Database.CommandTimeout = timeout;
                result.SalesData = await context.Database.SqlQuery<SalesDataModel>("usp_GetSalesData @SiteList,@TankList,@StorageList", siteIdParam, tankIdsParam, storageIdParams).ToListAsync();

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
        private static void IntializeSalesTankInformation(string uOM, JobWithTanksModel job, SalesDataResponseModel respData, ForecastingTankDetailsModel item)
        {
            if (respData != null && respData.SalesData.Any())
            {
                var tankData = respData.SalesData.Where(t => t.SiteId == job.TfxDisplayJobId && t.TankId == item.TankId && t.StorageId == item.StorageId).FirstOrDefault();
                if (tankData != null)
                {
                    item.DeliverySinceLastReading = tankData.LastDeliveredQuantity + " " + uOM;
                    item.AvgSale = tankData.AvgSale + " " + uOM;
                    item.WeekAgoSale = tankData.WeekAgoSale + " " + uOM;
                    item.PrevSale = tankData.PrevSale + " " + uOM;
                }
                else
                {
                    IntializeSalesTankEmptyData(uOM, item);
                }
            }
            else
            {
                IntializeSalesTankEmptyData(uOM, item);

            }
        }
        private static void IntializeSalesTankEmptyData(string uOM, ForecastingTankDetailsModel item)
        {
            item.DeliverySinceLastReading = 0 + " " + uOM;
            item.AvgSale = Resource.lblHyphen;
            item.WeekAgoSale = Resource.lblHyphen;
            item.PrevSale = Resource.lblHyphen;
        }
        public async Task<StatusModel> ProcessforcastingTankCaculation(List<UspForecastingTankInfomation> uspForecastingTankInfomations)
        {
            var response = new StatusModel();
            try
            {
                if (uspForecastingTankInfomations.Any())
                {
                    uspForecastingTankInfomations = uspForecastingTankInfomations.GroupBy(x => new { x.SiteID, x.TankId, x.StorageId }).Select(o => o.FirstOrDefault()).ToList();
                    var siteIds = uspForecastingTankInfomations.Select(top => top.SiteID).ToList();
                    var tankIds = uspForecastingTankInfomations.Select(top => top.TankId).ToList();
                    var storageIds = uspForecastingTankInfomations.Select(top => top.StorageId).ToList();
                    var forecastingTankCaculationInfo = await _forecastingRepository.GetForecastingTankInformation(siteIds, tankIds, storageIds);
                    LogManager.Logger.WriteDebug("ForecastingDomain", "ProcessforcastingTankCaculation", "GetSaleTank");
                    var saleTankInfo = await _forecastingRepository.GetSaleTank(siteIds, tankIds, storageIds);
                    LogManager.Logger.WriteDebug("ForecastingDomain", "ProcessforcastingTankCaculation", "GetSaleTank-Completed");
                    foreach (var item in uspForecastingTankInfomations)
                    {
                        var forecastinTankInfo = forecastingTankCaculationInfo.Where(top => top.TankId == item.TankId && top.SiteId == item.SiteID && top.StorageId == item.StorageId).FirstOrDefault();
                        await CalculateTankDataEstimate(item.SiteID, item.TankId, item.StorageId, forecastinTankInfo, saleTankInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "ProcessForcastingTankCaculation", "ProcessForcastingTankCaculation", ex);
            }
            return response;
        }
        public async Task<TankEstimate> CalculateTankDataEstimate(string siteId, string tankId, string storageId, ForecastingTankInformationModel forecastingTankInformation, List<SaleTankModel> saleTankInfo)
        {
            TankEstimate response = new TankEstimate();
            try
            {
                var saleTank = saleTankInfo.FirstOrDefault(x => x.SiteId == siteId && x.StorageId == storageId && x.TankId == tankId);
                if (saleTank != null)
                {
                    var monthStartDate = DateTime.Now.AddDays(-29).Date;
                    var monthEndDate = DateTime.Now.AddDays(-1).Date;
                    LogManager.Logger.WriteDebug("ForecastingDomain", "CalculateTankDataEstimate", "GetSaleTankMonthlyData-SaleTankId" + saleTank.Id);
                    var monthlyDatas = await _forecastingRepository.GetSaleTankMonthlyData(monthStartDate, monthEndDate, saleTank.Id);
                    if (forecastingTankInformation != null)
                    {
                        if (monthlyDatas.Any())
                        {
                            LogManager.Logger.WriteDebug("ForecastingDomain", "CalculateTankDataEstimate-Completed", "GetSaleTankMonthlyData-SaleTankId" + saleTank.Id);
                            var lastMonthlyRecords = monthlyDatas.OrderByDescending(top => top.Date).FirstOrDefault();
                            var tankInfo = forecastingTankInformation.TankInformation;
                            if (lastMonthlyRecords != null)
                            {
                                if (lastMonthlyRecords.TotalSale != tankInfo.TotalSale || lastMonthlyRecords.AverageSale != tankInfo.AverageSale)
                                {
                                    LogManager.Logger.WriteDebug("ForecastingDomain", "CaculateTankForecastingData-Started", "CaculateTankForecastingData-SiteId:" + siteId + "TankId:" + tankId + "StorageId:" + storageId);
                                    response = await CaculateTankForecastingData(siteId, tankId, storageId, response, saleTank, monthlyDatas, lastMonthlyRecords, 1);
                                    LogManager.Logger.WriteDebug("ForecastingDomain", "CaculateTankForecastingData-Completed", "CaculateTankForecastingData-SiteId:" + siteId + "TankId:" + tankId + "StorageId:" + storageId);

                                }
                            }
                        }
                        else
                        {
                            LogManager.Logger.WriteDebug("ForecastingDomain", "CalculateTankDataEstimate", "GetSaleTankMonthlyData-SaleTankId-Completed" + saleTank.Id);
                        }
                    }
                    else
                    {
                        if (monthlyDatas.Any())
                        {
                            var lastMonthlyRecords = monthlyDatas.OrderByDescending(top => top.Date).FirstOrDefault();
                            response = await CaculateTankForecastingData(siteId, tankId, storageId, response, saleTank, monthlyDatas, lastMonthlyRecords, 0);
                        }
                        else
                        {
                            LogManager.Logger.WriteDebug("ForecastingDomain", "CalculateTankDataEstimate", "GetSaleTankMonthlyData-SaleTankId-Completed" + saleTank.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingDomain", "CalculateTankDataEstimate", ex.Message, ex);
            }
            return response;
        }

        private async Task<TankEstimate> CaculateTankForecastingData(string siteId, string tankId, string storageId, TankEstimate response, SaleTankModel saleTank, List<SaleMonthlyDataModel> monthlyDatas, SaleMonthlyDataModel lastMonthlyRecords, int inserUpdateStatus)
        {
            int timeout = 1000;
            var tankUllage = await _forecastingRepository.GetTankUllage(siteId, tankId, storageId);
            if (tankUllage != null)
            {
                var currentInventory = (saleTank.FuelCapacity - (decimal)tankUllage.Ullage) ?? 0;
                LogManager.Logger.WriteDebug("ForecastingDomain", "CaculateTankForecastingData", "GetEstimate-SiteId:" + siteId + "TankId:" + tankId + "StorageId:" + storageId);
                var task = Task.Run(() => GetEstimate(monthlyDatas, saleTank.BandPeriod, currentInventory));
                if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
                {
                    // task completed within timeout
                    response = task.Result;
                    LogManager.Logger.WriteDebug("ForecastingDomain", "CaculateTankForecastingData-Completed", "GetEstimate-SiteId:" + siteId + "TankId:" + tankId + "StorageId:" + storageId);
                    var bandTime = tankUllage.CaptureTime.TimeOfDay;
                    var estimatedTime = (DateTime.Now - tankUllage.CaptureTime).TotalMinutes;
                    var estimatedBand = monthlyDatas.FirstOrDefault(t => t.BandStartTime >= bandTime && t.BandEndTime <= bandTime);
                    if (estimatedBand != null)
                    {
                        var bandAvgInMintes = estimatedBand.AverageSale / 60;
                        response.CurrentInventory = currentInventory - (bandAvgInMintes * (decimal)estimatedTime);
                        //insert/update information in TankForecasting
                        if (inserUpdateStatus == 0)
                            await SaveForecastingTankCaculation(siteId, tankId, storageId, response, lastMonthlyRecords);
                        else
                            await UpdateForecastingCalculation(siteId, tankId, storageId, response, lastMonthlyRecords);
                    }
                }
                else
                {
                    LogManager.Logger.WriteDebug("ForecastingDomain", "CaculateTankForecastingData(GetEstimate)-Timeout", "GetEstimate-SiteId:" + siteId + "TankId:" + tankId + "StorageId:" + storageId);
                }

            }

            return response;
        }

        private async Task UpdateForecastingCalculation(string siteId, string tankId, string storageId, TankEstimate response, SaleMonthlyDataModel lastMonthlyRecords)
        {
            ForecastingTankInformationModel tankInformationModel = new ForecastingTankInformationModel();
            tankInformationModel.SiteId = siteId;
            tankInformationModel.TankId = tankId;
            tankInformationModel.StorageId = storageId;
            tankInformationModel.DaysLeft = ((float)response.Hours / 24).GetPreciseValue(1);
            tankInformationModel.EstimatedCurrentInventory = response.CurrentInventory.GetPreciseValue(1);
            var tankInfoDetails = new TankInformationModel();
            tankInfoDetails.Date = lastMonthlyRecords.Date.ToString(Resource.constFormatDateTime);
            tankInfoDetails.BandNumber = lastMonthlyRecords.BandNumber;
            tankInfoDetails.SaleTankId = lastMonthlyRecords.SaleTankId;
            tankInfoDetails.TotalSale = lastMonthlyRecords.TotalSale;
            tankInfoDetails.AverageSale = lastMonthlyRecords.AverageSale;
            tankInformationModel.TankInformation = tankInfoDetails;
            await _forecastingRepository.UpdateForecastingCalculation(tankInformationModel);
        }

        private async Task SaveForecastingTankCaculation(string siteId, string tankId, string storageId, TankEstimate response, SaleMonthlyDataModel lastMonthlyRecords)
        {
            ForecastingTankInformationModel tankInformationModel = new ForecastingTankInformationModel();
            tankInformationModel.SiteId = siteId;
            tankInformationModel.TankId = tankId;
            tankInformationModel.StorageId = storageId;
            tankInformationModel.DaysLeft = ((float)response.Hours / 24).GetPreciseValue(1);
            tankInformationModel.EstimatedCurrentInventory = response.CurrentInventory.GetPreciseValue(1);
            var tankInfo = new TankInformationModel();
            tankInfo.Date = lastMonthlyRecords.Date.ToString(Resource.constFormatDateTime);
            tankInfo.BandNumber = lastMonthlyRecords.BandNumber;
            tankInfo.SaleTankId = lastMonthlyRecords.SaleTankId;
            tankInfo.TotalSale = lastMonthlyRecords.TotalSale;
            tankInfo.AverageSale = lastMonthlyRecords.AverageSale;
            tankInformationModel.TankInformation = tankInfo;
            await _forecastingRepository.SaveForecastingCalculation(tankInformationModel);
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
}
