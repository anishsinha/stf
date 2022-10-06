using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Order;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier.Controllers
{
    public class OrderController : BaseController
    {
        
        [HttpGet]
        [ActionName("View")]
        public ActionResult Orders(OrderFilterType filter = OrderFilterType.All, int orderId = 0, string groupIds = "")
        {
            using (var tracer = new Tracer("OrderController", "Orders"))
            {
                var response = ContextFactory.Current.GetDomain<OrderDomain>().GetOrderFilter(0, filter, 0, orderId, groupIds);
                return View("View", response);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAllCustomerData()
        {
            using (var tracer = new Tracer("OrderController", "GetAllCustomerData"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetAllCarrierCustomerData(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<JsonResult> SaveCarrierCustomerMapping(UspCarrierCustomerMapping customerMapping)
        {
            using (var tracer = new Tracer("OrderController", "GetAllCustomerData"))
            {
                var duplicateResponse = new StatusViewModel(string.IsNullOrEmpty(customerMapping.CarrierAssignedCustomerId) ? Status.Success : Status.Failed);
                var response = new StatusViewModel();

                if (!string.IsNullOrEmpty(customerMapping.CarrierAssignedCustomerId))
                {
                    duplicateResponse = ContextFactory.Current.GetDomain<OrderDomain>().CheckDuplicateCustomerId(customerMapping, UserContext);
                }

                if (duplicateResponse.StatusCode == Status.Success)
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveCarrierCustomerMapping(customerMapping, UserContext);
                }
                else
                {
                    response = duplicateResponse;
                }

                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckDuplicateCustomerId(UspCarrierCustomerMapping customerDetail)
        {
            using (var tracer = new Tracer("OrderController", "CheckDuplicateCustomerId"))
            {
                var response = ContextFactory.Current.GetDomain<OrderDomain>().CheckDuplicateCustomerId(customerDetail, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult PartialMapView(OrderFilterViewModel orderFilter = null)
        {
            using (var tracer = new Tracer("OrderController", "PartialMapView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetSupplierMapAsync(CurrentUser.Id, orderFilter)).Result;
                return PartialView("_PartialMapView", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id, bool isBrokeredRequest = false, bool isInvoiceGenerated = false)
        {
            using (var tracer = new Tracer("OrderController", "Details"))
            {

                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderDetails(id, CurrentUser.Id, UserContext, isBrokeredRequest);
                if (response.ParentOrderDetails != null && response.IsMultiOrder)
                {
                    response.ParentOrderDetails.IsMultiOrder = true;
                    response.ParentOrderDetails.StatusId = (int)OrderStatus.Open;
                }

                return View(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveBadgeDetails(OrderDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveBadgeDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveBadgeDetails(UserContext, viewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Carrier", id = viewModel.Id });
            }
        }

        [HttpPost]
        public async Task<JsonResult> createFreightOrdersForAssignedCarrier(List<SupplierCarrierViewModel> carriers)
        {
            using (var tracer = new Tracer("OrderController", "createFreightOrdersForAssignedCarrier"))
            {
                var response = await ContextFactory.Current.GetDomain<CarrierDomain>().createFreightOrdersForAssignedCarrier(carriers, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> EditFreightOnlyOrders(EditFreightOnlyOrderViewModel JobIdsToEdit)
        {
            using (var tracer = new Tracer("OrderController", "EditFreightOnlyOrders"))
            {
                var response = await ContextFactory.Current.GetDomain<CarrierDomain>().EditFreightOnlyOrders(JobIdsToEdit, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> closeAssignedOrdersforCarrier(EditFreightOnlyOrderViewModel OrdersToClose)
        {
            var response = await ContextFactory.Current.GetDomain<CarrierDomain>().closeAssignedOrdersforCarrier(OrdersToClose, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCarrierData(int countryId=(int)Country.USA)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetCarrierData(UserContext, countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult CheckDuplicateCarrierId(UspCarrierMapping carrierMapping)
        //{
        //    var response = ContextFactory.Current.GetDomain<OrderDomain>().CheckDuplicateCarrierId(carrierMapping, UserContext);
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public async Task<JsonResult> SaveAndUpdateCarrierMapping(UspCarrierMapping carrierMapping)
        {
            //var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveAndUpdateCarrierMapping(carrierMapping, UserContext);
            //return Json(response, JsonRequestBehavior.AllowGet);
            return null;
        }

        [HttpGet]
        public async Task<JsonResult> GetFilterDataForCalenderView()
        {
            using (var tracer = new Tracer("OrderController", "GetFilterDataForCalenderView"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetFilterDataForCalenderView(UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
