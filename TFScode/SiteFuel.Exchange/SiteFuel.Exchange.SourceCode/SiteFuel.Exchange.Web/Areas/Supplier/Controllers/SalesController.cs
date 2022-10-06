using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier)]
    public class SalesController : SupplierBaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetSalesData(string regionId = "", string customerId = "", string jobId = "", int priority = 0, int SelectedTab = 0, bool isShowCarrierManaged = false, string carriers = "", bool isRetailJob = true, string inventoryCaptureType = "")
        {
            using (var tracer = new Tracer("SalesController", "GetSalesData"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetSalesDataAsync(CurrentUser.CompanyId, regionId, customerId, jobId, priority, SelectedTab, isShowCarrierManaged, carriers, isRetailJob, inventoryCaptureType);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFilteredLocations(string regionId = "", string customerIds = "", bool isShowCarrierManaged = false, string carriers = "", string inventoryCaptureType = "")
        {
            using (var tracer = new Tracer("SalesController", "GetFilteredLocations"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetLocationTanksAsync(CurrentUser.CompanyId, regionId, customerIds, isShowCarrierManaged, carriers, inventoryCaptureType);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetSalesDataForGraph(int jobId, int noOfDays)
        {
            using (var tracer = new Tracer("SalesController", "GetSalesDataForGraph"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetSalesDataForGraphAsync(jobId, noOfDays);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetExistingSchedules(int jobId, int productTypeId)
        {
            using (var tracer = new Tracer("SalesController", "GetExistingSchedules"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetExistingSchedulesAsync(jobId, productTypeId, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public async Task<ActionResult> RaiseDeliveryRequest(RaiseDeliveryRequestInput raiseDelivery)
        {
            using (var tracer = new Tracer("SalesController", "RaiseDeliveryRequest"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().RaiseDeliveryRequest(raiseDelivery, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRaisedExceptions(string exceptionTypes)
        {
            using (var tracer = new Tracer("SalesController", "GetRaisedExceptions"))
            {
                var response = await ContextFactory.Current.GetDomain<ExceptionDomain>().GetRaisedExceptions(exceptionTypes, UserContext.CompanyId, false);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetForecastingSetting()
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingSetting"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingSetting(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetForecastingTankDetails(int jobId, string tankId, string storageId)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankDetails(jobId, tankId, storageId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetForecastingTankInventoryDetails(int jobId, string tankId, string storageId)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankInventoryDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankInventoryDetails(jobId, tankId, storageId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetForecastingTankEstimatedUsageDetails(int jobId, string startDate, string endDate, string tankId, string storageId)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankEstimatedUsageDetails"))
            {
                tankId = tankId.ToLower() == "none" ? string.Empty : tankId;
                storageId = storageId.ToLower() == "none" ? string.Empty : storageId;
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankEstimatedUsageDetails(jobId, startDate, endDate, tankId, storageId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetForecastingTankDeliveryDetails(int jobId, string tankId, string storageId)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankDeliveryDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankDeliveryDetails(jobId, tankId, storageId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetForecastingTankScheduleDetails(int jobId, string tankId, string storageId)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankScheduleDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankScheduleDetails(jobId, tankId, storageId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetForecastingTankDataForChart(int jobId, string tankId = null, string storageId = null, string currentDate = null)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankDataForChart"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankDataForChart(jobId, currentDate, tankId, storageId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> CalculateTankRetainWindowInfo(TankRetainWindowInfo tankRetainWindowInfo)
        {
            using (var tracer = new Tracer("DashboardController", "CalculateTankRetainWindowInfo"))
            {
                var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, tankRetainWindowInfo.Id);
                if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
                {
                    tankRetainWindowInfo.startBufferUOM = forcastinPreferance.ForcastingServiceSetting.StartBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.StartBufferUOM;
                    tankRetainWindowInfo.startBuffer = forcastinPreferance.ForcastingServiceSetting.StartBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.StartBuffer;

                    tankRetainWindowInfo.endBufferUOM = forcastinPreferance.ForcastingServiceSetting.EndBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.EndBufferUOM;
                    tankRetainWindowInfo.endBuffer = forcastinPreferance.ForcastingServiceSetting.EndBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.EndBuffer;

                    int retainBufferUOM = forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM;
                    int retainBuffer = forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer;

                    if (tankRetainWindowInfo.startBufferUOM == retainBufferUOM)
                    {
                        tankRetainWindowInfo.maxBuffer = tankRetainWindowInfo.startBuffer > retainBuffer ? tankRetainWindowInfo.startBuffer : retainBuffer;
                        tankRetainWindowInfo.maxBufferUOM = tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours ? (int)RateOfConsumsionUOM.Hours : (int)RateOfConsumsionUOM.Days;
                    }
                    else
                    {
                        if (tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours && retainBufferUOM == (int)RateOfConsumsionUOM.Days)
                        {
                            tankRetainWindowInfo.maxBuffer = retainBuffer;
                            tankRetainWindowInfo.maxBuffer = (int)RateOfConsumsionUOM.Days;
                        }
                        else if (tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Days && retainBufferUOM == (int)RateOfConsumsionUOM.Hours)
                        {
                            tankRetainWindowInfo.maxBuffer = tankRetainWindowInfo.startBuffer;
                            tankRetainWindowInfo.maxBuffer = (int)RateOfConsumsionUOM.Days;
                        }
                    }
                    var response = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(tankRetainWindowInfo);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> CalculateTankDetailsRetainWindowInfo(List<TankRetainWindowInfo> tankRetainWindowInfo)
        {
            var response = new List<TankRetainInfo>();
            using (var tracer = new Tracer("DashboardController", "CalculateTankDetailsRetainWindowInfo"))
            {
                foreach (var item in tankRetainWindowInfo)
                {
                    var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, item.Id);
                    if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
                    {
                        IntializeRetainParametersVals(item, forcastinPreferance);
                        var result = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(item);
                        if (result != null && !string.IsNullOrEmpty(result.RetainTime))
                        {
                            TankRetainInfo tankRetainInfo = new TankRetainInfo();
                            tankRetainInfo.Id = item.Id;
                            tankRetainInfo.TankName = item.TankName;
                            tankRetainInfo.storageId = item.storageId;
                            tankRetainInfo.siteId = item.siteId;
                            tankRetainInfo.tankId = item.tankId;
                            tankRetainInfo.RetainDate = result.RetainDate;
                            tankRetainInfo.RetainTime = result.RetainTime;
                            tankRetainInfo.WindowStartDate = result.WindowStartDate;
                            tankRetainInfo.WindowStartTime = result.WindowStartTime;
                            tankRetainInfo.WindowEndDate = result.WindowEndDate;
                            tankRetainInfo.WindowEndTime = result.WindowEndTime;
                            response.Add(tankRetainInfo);
                        }
                    }
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CalculateProductRetainWindowInfo(List<TankRetainWindowInfo> tankRetainWindowInfos)
        {
            using (var tracer = new Tracer("DashboardController", "CalculateProductRetainWindowInfo"))
            {
                foreach (var tankRetainWindowInfo in tankRetainWindowInfos)
                {
               
                    var forcastinPreferance = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, tankRetainWindowInfo.Id);

                    if (forcastinPreferance.ForcastingServiceSetting.IsEnabled)
                    {
                        tankRetainWindowInfo.startBufferUOM = forcastinPreferance.ForcastingServiceSetting.StartBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.StartBufferUOM;
                        tankRetainWindowInfo.startBuffer = forcastinPreferance.ForcastingServiceSetting.StartBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.StartBuffer;

                        tankRetainWindowInfo.endBufferUOM = forcastinPreferance.ForcastingServiceSetting.EndBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.EndBufferUOM;
                        tankRetainWindowInfo.endBuffer = forcastinPreferance.ForcastingServiceSetting.EndBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.EndBuffer;

                        int retainBufferUOM = forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM;
                        int retainBuffer = forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer;

                        if (tankRetainWindowInfo.startBufferUOM == retainBufferUOM)
                        {
                            tankRetainWindowInfo.maxBuffer = tankRetainWindowInfo.startBuffer > retainBuffer ? tankRetainWindowInfo.startBuffer : retainBuffer;
                            tankRetainWindowInfo.maxBufferUOM = tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours ? (int)RateOfConsumsionUOM.Hours : (int)RateOfConsumsionUOM.Days;
                        }
                        else
                        {
                            if (tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Hours && retainBufferUOM == (int)RateOfConsumsionUOM.Days)
                            {
                                tankRetainWindowInfo.maxBuffer = retainBuffer;
                                tankRetainWindowInfo.maxBuffer = (int)RateOfConsumsionUOM.Days;
                            }
                            else if (tankRetainWindowInfo.startBufferUOM == (int)RateOfConsumsionUOM.Days && retainBufferUOM == (int)RateOfConsumsionUOM.Hours)
                            {
                                tankRetainWindowInfo.maxBuffer = tankRetainWindowInfo.startBuffer;
                                tankRetainWindowInfo.maxBuffer = (int)RateOfConsumsionUOM.Days;
                            }
                        }
                        var response = await ContextFactory.Current.GetDomain<SalesDomain>().CalculateTankRetainWindowInfo(tankRetainWindowInfo);
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetLocationTanksInfo(ForecastingLocationFilter forecastingFilter)
        {
            using (var tracer = new Tracer("SalesController", "GetLocationTanksInfo"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetLocationTanksInfo(CurrentUser.CompanyId, forecastingFilter.RegionId, forecastingFilter.CustomerIds, forecastingFilter.IsShowCarrierManaged, forecastingFilter.Carriers, forecastingFilter.InventoryCaptureType, forecastingFilter.IsRateOfConsumption,UserContext);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
        #region PrivateMethod
        private static void IntializeRetainParametersVals(TankRetainWindowInfo item, ViewModels.Forcasting.ForcastingPreferenceViewModel forcastinPreferance)
        {
            item.startBufferUOM = forcastinPreferance.ForcastingServiceSetting.StartBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.StartBufferUOM;
            item.startBuffer = forcastinPreferance.ForcastingServiceSetting.StartBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.StartBuffer;

            item.endBufferUOM = forcastinPreferance.ForcastingServiceSetting.EndBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.EndBufferUOM;
            item.endBuffer = forcastinPreferance.ForcastingServiceSetting.EndBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.EndBuffer;

            int retainBufferUOM = forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM == null ? 1 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBufferUOM;
            int retainBuffer = forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer == null ? 0 : (int)forcastinPreferance.ForcastingServiceSetting.RetainTimeBuffer;

            if (item.startBufferUOM == retainBufferUOM)
            {
                item.maxBuffer = item.startBuffer > retainBuffer ? item.startBuffer : retainBuffer;
                item.maxBufferUOM = item.startBufferUOM == (int)RateOfConsumsionUOM.Hours ? (int)RateOfConsumsionUOM.Hours : (int)RateOfConsumsionUOM.Days;
            }
            else
            {
                if (item.startBufferUOM == (int)RateOfConsumsionUOM.Hours && retainBufferUOM == (int)RateOfConsumsionUOM.Days)
                {
                    item.maxBuffer = retainBuffer;
                    item.maxBuffer = (int)RateOfConsumsionUOM.Days;
                }
                else if (item.startBufferUOM == (int)RateOfConsumsionUOM.Days && retainBufferUOM == (int)RateOfConsumsionUOM.Hours)
                {
                    item.maxBuffer = item.startBuffer;
                    item.maxBuffer = (int)RateOfConsumsionUOM.Days;
                }
            }
        }
        #endregion PrivateMethod
    }
}