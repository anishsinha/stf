using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;
using SiteFuel.Exchange.Core.Logger;
using System.Collections.Generic;
using System;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class OrderController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Orders(int jobId = 0, OrderFilterType filter = OrderFilterType.All, int fuelTypeId = 0, string groupIds = "")
        {
            using (var tracer = new Tracer("OrderController", "Orders"))
            {
                var response = ContextFactory.Current.GetDomain<OrderDomain>().GetOrderFilter(jobId, filter, fuelTypeId, 0, groupIds);
                return View("View", response);
            }
        }

        public PartialViewResult OrdersDetails(int jobId = 0, int country = (int)Country.USA, OrderFilterType filter = OrderFilterType.All)
        {
            using (var tracer = new Tracer("OrderController", "OrdersDetails"))
            {
                var response = ContextFactory.Current.GetDomain<OrderDomain>().GetOrderFilter(jobId, filter);
                response.Country.Id = country;
                return PartialView("_PartialOrderDetails", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> OrdersGrid(OrderDataTableViewModel orderModel)
        {
            using (var tracer = new Tracer("OrderController", "OrdersGrid"))
            {
                var dashboardDomain = new DashboardDomain();
                var orderDomain = new OrderDomain(dashboardDomain);
                orderModel.GroupIds = dashboardDomain.DecryptData(orderModel.GroupIds);

                var response = await orderDomain.GetBuyerOrdersAsync(CurrentUser.Id, CurrentUser.CompanyId, orderModel, CurrentUser.BrandedCompanyId);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = orderModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("OrderController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBuyerOrderStatusAsync(id, CurrentUser.Id);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Cancel(int id)
        {
            using (var tracer = new Tracer("OrderController", "Cancel"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().IsOrderHasInvoiceAsync(id);
                if (response.StatusCode == Status.Success)
                {
                    CancelOrderViewModel viewModel = new CancelOrderViewModel() { OrderId = id };
                    return View("Cancel", viewModel);
                }
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Buyer", id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Cancel(CancelOrderViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "Cancel(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.CanceledBy = CurrentUser.Id;
                    var originalOrderId = viewModel.OrderId;

                    var response = await ContextFactory.Current.GetDomain<OrderDomain>().CancelOrderAsync(UserContext, viewModel, false, true);
                    if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return RedirectToAction("Details", "Order", new { area = "Buyer", id = originalOrderId });
                    }
                    else
                    {
                        var drCloseStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { viewModel.OrderId }, UserContext);
                        if (drCloseStatus.StatusCode != (int)Status.Success)
                        {
                            DisplayCustomMessages((MessageType)drCloseStatus.StatusCode, drCloseStatus.StatusMessage);
                        }
                    }

                    if (viewModel.IsFuelRequestReSubmit)
                    {
                        await ContextFactory.Current.GetDomain<FuelRequestDomain>().ReSubmitFuelRequestAsync(originalOrderId, UserContext);
                    }
                    return RedirectToAction("View", "Order", new { area = "Buyer" });
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult PartialMapView(OrderFilterViewModel orderFilter = null)
        {
            using (var tracer = new Tracer("OrderController", "PartialMapView"))
            {
                var dashboardDomain = new DashboardDomain();
                var orderDomain = new OrderDomain(dashboardDomain);

                var decryptedGroupIds = dashboardDomain.DecryptData(orderFilter.GroupIds);
                orderFilter.GroupIds = decryptedGroupIds;
                var response = Task.Run(() => orderDomain.GetBuyerMapAsync(CurrentUser.Id, orderFilter)).Result;
                return PartialView("_PartialMapView", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Close(int id)
        {
            using (var tracer = new Tracer("OrderController", "Close"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().CloseOrderAsync(UserContext, id, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                else
                {
                    var drCloseStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { id }, UserContext);
                    if (drCloseStatus.StatusCode != (int)Status.Success)
                    {
                        DisplayCustomMessages((MessageType)drCloseStatus.StatusCode, drCloseStatus.StatusMessage);
                    }
                }
                return RedirectToAction("Details", "Order", new { area = "Buyer", id = id });
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson)]
        public async Task<JsonResult> EditPoNumber(int id, int fuelRequestId, string poNumber, bool isProFormaPo = false)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("OrderController", "EditPoNumber"))
            {
                if (!isProFormaPo)
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().EditPoNumberAsync(UserContext, id, poNumber);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<OrderDomain>().EditProFormaPoNumberAsync(UserContext, id, poNumber);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult OrderPoView(int id)
        {
            using (var tracer = new Tracer("OrderController", "OrderPoView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetOrderPoAsync(id, 0)).Result;
                return PartialView("_PartialOrderPo", response);
            }
        }

        [HttpGet]
        public ActionResult DeliverySchedule(int scheduleType = (int)DeliveryScheduleType.SpecificDates, int orderId = 0)
        {
            var deliverySchedule = new DeliveryScheduleViewModel() { ScheduleType = scheduleType, CreatedBy = CurrentUser.Id };
            if (orderId > 0)
            {
                deliverySchedule = ContextFactory.Current.GetDomain<OrderDomain>().GetDefaultScheduleTime(orderId, deliverySchedule);
            }
            return PartialView("_PartialDeliveryScheduleOrder", deliverySchedule);
        }

        [HttpGet]
        public ActionResult RescheduleDeliverySchedule(int trackableScheduleId, int scheduleType = (int)DeliveryScheduleType.SpecificDates)
        {
            var deliverySchedule = ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryScheduleByTrackableScheduleId(trackableScheduleId);
            deliverySchedule.ScheduleType = scheduleType;
            deliverySchedule.CreatedBy = CurrentUser.Id;
            deliverySchedule.RescheduledTrackableId = trackableScheduleId;
            deliverySchedule.StatusId = (int)DeliveryScheduleStatus.Rescheduled;
            deliverySchedule.IsRescheduled = true;
            return PartialView("_PartialDeliveryScheduleOrder", deliverySchedule);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.OnsitePerson)]
        public async Task<ActionResult> SaveDeliverySchedules(OrderDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveDeliverySchedules"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveDeliverySchedulesAsync(UserContext, viewModel, true);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Buyer", id = viewModel.Id });
            }
        }

        [HttpGet]
        public ActionResult DeliveryScheduleHistory(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "DeliveryScheduleHistory"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetOrderVersionHistoryAsync(orderId)).Result;
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public ViewResult WhereIsMyDriver()
        {
            return View("WhereIsMyDriver");
        }

        [HttpGet]
        public JsonResult GetDeliveryScheduleForJobAsync(int jobId, int enrouteStatus = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetDeliveryScheduleForJobAsync"))
            {
                long scheduleDate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryScheduleForJobAsync(jobId, string.Empty, 0, 0, scheduleDate, 0, enrouteStatus)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetDriverDistance(int driverId, int orderId, int deliveryRequestId, int enrouteStatus = 0)
        {
            using (var tracer = new Tracer("OrderController", "GetDriverDistance"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetDriverDetailsForBuyerAsync(driverId, orderId, deliveryRequestId, enrouteStatus)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetDriverScheduleGridDetails(int driverId, int orderId, int deliveryRequestId, int enrouteStatus = 0)
        {
            var response = ContextFactory.Current.GetDomain<OrderDomain>().GetDriverScheduleGridForBuyer(driverId, orderId, deliveryRequestId, enrouteStatus);
            return PartialView("_PartialDriverScheduleGrid", response);
        }

        [HttpGet]
        public ActionResult OrderHistory(int id)
        {
            using (var tracer = new Tracer("OrderController", "OrderHistory"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetBuyerOrderHistoryAsync(id, CurrentUser.Id)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTogglePricingDetails(int orderId, bool isHidePricingEnabled)
        {
            using (var tracer = new Tracer("OrderController", "UpdateTogglePricingDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateTogglePricingDetailsAsync(UserContext, orderId, isHidePricingEnabled, CompanyType.Buyer);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return Json(response);
            }
        }

        public async Task<PartialViewResult> DetailsTab(int id)
        {
            using (var tracer = new Tracer("OrderController", "DetailsTab"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBuyerOrderDetailsAsync(id, UserContext);
                return PartialView("_PartialTabOrderDetails", response);
            }
        }

        public async Task<PartialViewResult> DeliveryTab(int id)
        {
            using (var tracer = new Tracer("OrderController", "DeliveryTab"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBuyerDeliveryDetailsAsync(id, CurrentUser.Id);
                return PartialView("_PartialTabOrderDelivery", response);
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> DropInformationTab(int id)
        {
            using (var tracer = new Tracer("OrderController", "DropInformationTab"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetDropInformationDetailsAsync(id);
                return PartialView("_PartialTabDropInformation", response);
            }
        }

        public PartialViewResult HistoryTab(int id)
        {
            return PartialView("_PartialTabOrderHistory");
        }

        [HttpGet]
        public ActionResult GetMissedDeliverySchedules(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetMissedDeliverySchedules"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetMissedDeliverySchedules(orderId)).Result;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ProcessDeliverySchedule(int orderId, int trackableScheduleId, int deliveryScheduleStatusId)
        {
            using (var tracer = new Tracer("OrderController", "ProcessDeliverySchedule"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().ProcessTrackableScheduleAsync(UserContext, trackableScheduleId, deliveryScheduleStatusId, true);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetBuyerOrderStat(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetBuyerOrderStat"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBuyerOrderStatAsync(orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(UserContext, EntityType.Order, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllJobLocationsByUser(int userId)
        {
            using (var tracer = new Tracer("OrderController", "GetAllJobLocationsByUser"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetAllJobLocationsByUser(userId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson)]
        public async Task<JsonResult> UpdateWbsNumber(int fuelRequestId, string wbsNumber)
        {
            StatusViewModel response;
            response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateWbsNumber(UserContext, fuelRequestId, wbsNumber);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}