using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    public class DeliveryGroupController : SupplierBaseController
    {
        [ActionName("View")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetCreateDeliveryGroup()
        {
            return PartialView("_PartailCreateDeliveryGroup");
        }

        [HttpGet]
        public async Task<JsonResult> GetOrdersForRouteGroup()
        {
            using (var tracer = new Tracer("DeliveryGroupController", "GetOrdersForRouteGroup"))
            {
                var dispatchDomain = ContextFactory.Current.GetDomain<DispatchDomain>();
                var response = await dispatchDomain.GetOrdersForRouteGroupAsync(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetSchedulesForRouteGroup(int orderId, int includeSchedules)
        {
            using (var tracer = new Tracer("DispatchCoDeliveryGroupControllerntroller", "GetSchedulesForRouteGroup"))
            {
                var dispatchDomain = ContextFactory.Current.GetDomain<DispatchDomain>();
                var response = await dispatchDomain.GetSchedulesForRouteGroupAsync(orderId, includeSchedules);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetSchedulesForRouteGroupEdit(List<int> orderIds, bool? includeFutureSchedules, int deliveryGroupId)
        {
            using (var tracer = new Tracer("DeliveryGroupController", "GetSchedulesForRouteGroupEdit"))
            {
                var dispatchDomain = ContextFactory.Current.GetDomain<DispatchDomain>();
                var response = await dispatchDomain.GetSchedulesForRouteGroupEditAsync(orderIds, includeFutureSchedules ?? false, deliveryGroupId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTabSchedulesForRouteGroupEdit(int orderId, bool? includeFutureSchedules, int deliveryGroupId)
        {
            using (var tracer = new Tracer("DeliveryGroupController", "GetSchedulesForRouteGroupEdit"))
            {
                var dispatchDomain = ContextFactory.Current.GetDomain<DispatchDomain>();
                var response = await dispatchDomain.GetSchedulesForRouteGroupEditAsync(orderId, includeFutureSchedules ?? false, deliveryGroupId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateDeliveryGroup(DeliveryGroupInputViewModel viewModel)
        {
            using (var tracer = new Tracer("DeliveryGroupController", "CreateDeliveryGroup"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().CreateDeliveryGroupAsync(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditDeliveryGroup(DeliveryGroupGridViewModel viewModel)
        {
            using (var tracer = new Tracer("DeliveryGroupController", "EditDeliveryGroup"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().EditDeliveryGroupAsync(viewModel, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDeliveryGroupsAsync(string searchText = "", int pageSize = 10, int pageNumber = 1, string startDate = "", string endDate = "")
        {
            using (var tracer = new Tracer("DeliveryGroupController", "GetDeliveryGroupsAsync"))
            {
                var response = await ContextFactory.Current.GetDomain<DispatchDomain>().GetDeliveryGroupsAsync(CurrentUser.CompanyId, searchText, pageSize, pageNumber, startDate, endDate);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}