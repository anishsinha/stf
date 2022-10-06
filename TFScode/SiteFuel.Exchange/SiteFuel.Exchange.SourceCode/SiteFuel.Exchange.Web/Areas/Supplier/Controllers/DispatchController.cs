using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
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
using SiteFuel.Exchange.Logger;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier)]
    public class DispatchController : SupplierBaseController
    {
        // GET: Supplier/Dispatch
        public ActionResult Index(string selectedDrivers = null, bool isScheduleTab = false)
        {
            using (var tracer = new Tracer("DispatchController", "Index"))
            {
                var response = ContextFactory.Current.GetDomain<DispatchDomain>().GetDispatchDetails(CurrentUser.CompanyId);
                if (selectedDrivers != null && selectedDrivers.Length > 0)
                {
                    response.SelectedDrivers = selectedDrivers.Split(',').Select(int.Parse).ToList();
                }
                response.IsScheduleTab = isScheduleTab;
                return View(response);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetDeliverySchedulesforDrivers(List<int> driverIds, string currentDate, string startDate, string endDate, bool isAllData, Currency currency = Currency.USD, int countryId = (int)Country.USA, string searchText = "")
        {
            using (var tracer = new Tracer("DispatchController", "GetDeliverySchedulesforDrivers"))
            {
                var today = Convert.ToDateTime(currentDate);
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetDeliverySchedulesforDriversAsync(CurrentUser.CompanyId, driverIds, today, startDate, endDate, isAllData, currency, countryId, searchText);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeliveriesGrid(List<int> driverIds, string startDate, string endDate, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DispatchController", "DeliveriesGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCompletedDeliveriesForSupplierAsync(CurrentUser.CompanyId, driverIds, startDate, endDate, currency, countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCurrentDriverDropDetails(List<int> driverId, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DispatchController", "GetCurrentDriverDropDetails"))
            {
                //var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCurrentDriverDropDetails(driverId, currency, countryId);
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCurrentDriverDropDetails(driverId, currency, countryId, UserContext.CompanyId);
                return PartialView("_PartialCurrentDriverDropGrid", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetNextDriverDropDetails(List<int> driverId, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DispatchController", "GetNextDriverDropDetails"))
            {
                //var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetNextDriverDropDetails(driverId, currency, countryId);
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetNextDriverDropDetails(driverId, currency, countryId, UserContext.CompanyId);
                return PartialView("_PartialNextDriverDropGrid", response);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetNextDriverDropDetailsForMap(List<int> driverId, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DispatchController", "GetNextDriverDropDetailsForMap"))
            {
                //var response = Task.Run(() => ContextFactory.Current.GetDomain<DispatchDomain>().GetNextDriverDropDetails(driverId, currency, countryId)).Result;
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetNextDriverDropDetails(driverId, currency, countryId, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetCurrentDriverDropDetailsForMap(List<int> driverId, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DispatchController", "GetCurrentDriverDropDetailsForMap"))
            {
                //var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCurrentDriverDropDetails(driverId, currency, countryId);
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCurrentDriverDropDetails(driverId, currency, countryId, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        
        public JsonResult GetDriverLocation(List<int> driverId, int enrouteStatus = 0)
        {
            using (var tracer = new Tracer("DispatchController", "GetDriverLocation"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<DispatchDomain>().GetDriverLocationAsync(driverId, enrouteStatus)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetDriverDistance(int driverId)
        {
            using (var tracer = new Tracer("DispatchController", "GetDriverDistance"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<DispatchDomain>().GetDriverDetailsAsync(driverId)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> TimeCardGrid(string startDate, string endDate, List<int> driverIds, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("DispatchController", "TimeCardGrid"))
            {
                if (driverIds.Count == 1)
                {
                    var response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardGridDataAsync(CurrentUser.CompanyId, driverIds[0], startDate, endDate, currency, countryId);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardActionSummaryForAllDrivers(driverIds, CurrentUser.CompanyId, startDate, endDate, currency, countryId);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReassignDriver(int orderId, Nullable<int> scheduleId, Nullable<int> tscheduleId, Nullable<int> driverId, int previousDriver)
        {
            using (var tracer = new Tracer("DispatchController", "ReassignDriver"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().ReassignDriver(UserContext, orderId, scheduleId, tscheduleId, driverId, previousDriver);
                if (response.StatusCode != Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult RescheduleDeliverySchedule(int trackableScheduleId, bool isScheduleTab)
        {
            using (var tracer = new Tracer("DispatchController", "RescheduleDeliverySchedule"))
            {
                var deliverySchedule = ContextFactory.Current.GetDomain<DispatchDomain>().GetDeliveryScheduleByTrackableScheduleId(trackableScheduleId);
                deliverySchedule.IsScheduleTab = isScheduleTab;
                return PartialView("_PartialRescheduleDelivery", deliverySchedule);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetModifyDeliverySchedule(ScheduleEditRequestViewModel viewModel)
        {
            var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetScheduleDetailsToEditAsync(viewModel);
            response.EnrouteStatus = viewModel.EnrouteStatus;
            response.CountryCode = viewModel.CountryCode;
            response.CountryId = viewModel.CountryId;
            response.Currency = viewModel.Currency;
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialTrackableScheduleEdit.cshtml", response);
        }

        [HttpGet]
        public async Task<ActionResult> AddDeliverySchedule(int orderId)
        {
            var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetOrderDetailsToAddScheduleAsync(orderId);
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialAddSchedule.cshtml", response);
        }

        [HttpPost]
        public async Task<ActionResult> AddDeliverySchedule(ScheduleEditInputViewModel viewModel)
        {
            using (var tracer = new Tracer("DispatchController", "AddDeliverySchedule"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().AddDeliverySchedule(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ModifyDelivery(ScheduleEditInputViewModel viewModel)
        {
            using (var tracer = new Tracer("DispatchController", "ModifyDelivery"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().ModifyDeliverySchedule(viewModel, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                return RedirectToAction("Index", "Dispatch", new { area = "Supplier", isScheduleTab = false });
            }
        }

        [HttpPost]
        public async Task<ActionResult> NotificationToDriver(List<int> driverIds)
        {
            try
            {
                var driverNotificationViewModel = new DriverNotificationViewModel();
                driverNotificationViewModel.Message = await ContextFactory.Current.GetDomain<DispatchDomain>().GetDriverNotificationDetails(CurrentUser.Id);
                driverNotificationViewModel.DriverIds = driverIds;
                var response = await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToDriver(driverNotificationViewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("NotificationController", "NotificationToDriver", ex.Message, ex);
            }
            return PartialView("_DisplayCustomMessage");
        }

        [HttpPost]
        public async Task<JsonResult> GetClosestTerminalWithPrice(int orderId, string terminal)
        {
            using (var tracer = new Tracer("DispatchController", "GetClosestTerminalWithPrice"))
            {
                var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(orderId, terminal);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddTrailerSchedule(TrailerScheduleViewModel viewModel)
        {
            viewModel.CreatedBy = UserContext.Id;
            var fsDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            var response = await fsDomain.AddTrailerSchedule(viewModel);
            return Json(response, JsonRequestBehavior.DenyGet);
        }
    }
}