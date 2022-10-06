using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Helpers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class SalesController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetSalesData(string jobId = "", int priority = 0, int SelectedTab = 0, bool isShowCarrierManaged = false, string carriers = "", string supplierIds = "", bool isRetailJob = true, string inventoryCaptureTypeIds = "")
        {
            using (var tracer = new Tracer("SalesController", "GetSalesData:Buyer"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetBuyerSalesDataAsync(CurrentUser.CompanyId, jobId, priority, SelectedTab, 0, isShowCarrierManaged, carriers, supplierIds, isRetailJob, inventoryCaptureTypeIds);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetFilteredLocations(string jobId = "", bool isShowCarrierManaged = false, string carriers = "", string inventoryCaptureType = "")
        {
            using (var tracer = new Tracer("SalesController", "GetFilteredLocations"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetBuyerLocationsAsync(CurrentUser.CompanyId, jobId, isShowCarrierManaged, carriers, inventoryCaptureType);
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
                var response = await ContextFactory.Current.GetDomain<ExceptionDomain>().GetRaisedExceptions(exceptionTypes, UserContext.CompanyId, true);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetBuyerScheduleQtyType()
        {
            var response = CommonHelperMethods.GetScheduleQtyType();
            return Json(response, JsonRequestBehavior.AllowGet);
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
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankDetails(jobId, tankId, storageId, UserContext );
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
        public async Task<ActionResult> GetForecastingTankEstimatedUsageDetails(int jobId, string tankId, string storageId, string startDate, string endDate)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankEstimatedUsageDetails"))
            {
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
        public async Task<ActionResult> GetForecastingTankDataForChart(int jobId, string tankId, string storageId, string currentDate)
        {
            using (var tracer = new Tracer("SalesController", "GetForecastingTankDataForChart"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetForecastingTankDataForChart(jobId, currentDate, tankId, storageId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> GetLocationTanksInfo(ForecastingLocationFilter forecastingFilter)
        {
            using (var tracer = new Tracer("SalesController", "GetLocationTanksInfo"))
            {
                var response = await ContextFactory.Current.GetDomain<SalesDomain>().GetBuyerLocationTanksInfo(CurrentUser.CompanyId, forecastingFilter.RegionId, forecastingFilter.CustomerIds, forecastingFilter.IsShowCarrierManaged, forecastingFilter.Carriers, forecastingFilter.InventoryCaptureType, forecastingFilter.IsRateOfConsumption);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}