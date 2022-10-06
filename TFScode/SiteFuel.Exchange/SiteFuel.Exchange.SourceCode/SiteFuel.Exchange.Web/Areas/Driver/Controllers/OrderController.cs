using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using SiteFuel.Exchange.Core.Logger;
using System.Linq;
using System;

namespace SiteFuel.Exchange.Web.Areas.Driver.Controllers
{
    public class OrderController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Orders(OrderFilterType filter = OrderFilterType.All)
        {
            using (var tracer = new Tracer("OrderController", "Orders"))
            {
                var response = ContextFactory.Current.GetDomain<OrderDomain>().GetOrderFilter(0, filter);
                return View("View", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("OrderController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDriverOrderDetailsAsync(id, CurrentUser.Id, UserContext);
                return View(response);
            }
        }

        [HttpGet]
        public ActionResult OrdersGrid(OrderFilterViewModel orderFilter = null)
        {
            using (var tracer = new Tracer("OrderController", "OrdersGrid"))
            {
                var response = ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetDriverOrders(CurrentUser.CompanyId, CurrentUser.Id, orderFilter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult PartialMapView(OrderFilterViewModel orderFilter = null)
        {
            using (var tracer = new Tracer("OrderController", "PartialMapView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetDriverMapAsync(CurrentUser.CompanyId, CurrentUser.Id, orderFilter)).Result;
                return PartialView("_PartialMapView", response);
            }
        }

        [HttpGet]
        public ActionResult DropHistory()
        {
                return View("_PartialDropHistoryGridView");
        }

        [HttpGet]
        public async Task<ActionResult> DropHistoryGrid(string startDate, string endDate)
        {
            using (var tracer = new Tracer("OrderController", "DropHistoryGrid"))
            {
                List<int> driverIds = new List<int>();
                driverIds.Add(CurrentUser.Id);
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetCompletedDeliveriesForSupplierAsync(CurrentUser.CompanyId, driverIds, startDate, endDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("DeliverySchedules")]
        public ActionResult GetAllDeliverySchedules()
        {
            return View("DeliverySchedules");
        }

        public async Task<ActionResult> GetDeliverySchedules(string startDate, string endDate)
        {
            using (var tracer = new Tracer("OrderController", "GetDeliverySchedules"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetDriverDeliverySchedulesAsync(CurrentUser.CompanyId, CurrentUser.Id, startDate, endDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> TimeCardGrid(string startDate, string endDate, TimeCardView timeCardView = TimeCardView.All)
        {
            using (var tracer = new Tracer("OrderController", "TimeCardGrid"))
            {
                if (timeCardView == TimeCardView.Day)
                {
                    var response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardGridDataAsync(CurrentUser.CompanyId, CurrentUser.Id, startDate, endDate);
                    response = response.OrderBy(t => t.Id).ToList();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<int> driverIds = new List<int>();
                    driverIds.Add(CurrentUser.Id);

                    var response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardActionSummaryForAllDrivers(driverIds, CurrentUser.CompanyId, startDate, endDate);
                    response = response.OrderByDescending(t => Convert.ToDateTime(t.ActionDate)).ToList();
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}