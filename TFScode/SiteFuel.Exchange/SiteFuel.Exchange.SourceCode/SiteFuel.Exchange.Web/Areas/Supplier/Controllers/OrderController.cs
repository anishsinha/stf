using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Forcasting;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier, CompanyType.SupplierAndCarrier)]
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
        public ActionResult ContactPerson(string collectionName)
        {
            return PartialView("_PartialContactPerson", new ContactPersonViewModel() { EntityNumber = collectionName });
        }


        [HttpGet]
        public async Task<ActionResult> Details(int id, bool isBrokeredRequest = false, bool isInvoiceGenerated = false)
        {
            using (var tracer = new Tracer("OrderController", "Details"))
            {

                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetSupplierOrderDetailsAsync(id, CurrentUser.Id, UserContext, isBrokeredRequest);
                if (response.ParentOrderDetails != null && response.IsMultiOrder)
                {
                    response.ParentOrderDetails.IsMultiOrder = true;
                    response.ParentOrderDetails.StatusId = (int)OrderStatus.Open;
                }

                return View(response);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Edit(OrderDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "Edit(viewModel)"))
            {
                var response = new StatusViewModel();                
                if (viewModel.CustomerContacts != null && viewModel.CustomerContacts.Any())
                {
                    var userEmails = viewModel.CustomerContacts.Select(t => t.Email).ToList();
                    userEmails.Add(viewModel.BuyerUserEmail);
                    var duplicate = userEmails
                                            .GroupBy(x => x)
                                            .Where(group => group.Count() > 1)
                                            .Select(group => group.Key).ToList();

                    if (duplicate != null && duplicate.Any())
                    {
                        string duplicateEmails = string.Join(", ", duplicate);
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                            new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });
                        DisplayCustomMessages((MessageType)Status.Failed, response.StatusMessage);
                        return View(viewModel);
                    }
                }
                if (ModelState.IsValid)
                {
                    response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().UpdateThirdPartyOrder(viewModel, UserContext);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                if (response.StatusCode == Status.Success)
                {
                    return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.Id });
                }
                else
                {
                    DisplayCustomMessages((MessageType)Status.Failed, response.StatusMessage);
                    return View(viewModel);
                }
            }
        }

        public async Task<ActionResult> IsTerminalAvailable(int orderId)
        {
            var isTerminalAvailable = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().IsTerminalAvailable(orderId);
            if (isTerminalAvailable)
            {
                return Json(new
                {
                    Message = Resource.errMessageFuelRequestAccepted,
                    StatusCode = (int)Status.Success
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Message = Resource.msgNoTerminalAvailable,
                    StatusCode = (int)Status.Failed
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrderLicenses(List<int> licenses, int orderId)
        {
            using (var tracer = new Tracer("OrderController", "UpdateOrderLicenses"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateOrderLicenses(licenses, orderId, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return Json(response);
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

        [HttpPost]
        public async Task<ActionResult> UpdateOrderAdditionalDetails(OrderAdditionalDetailsViewModel viewmodel, bool bolImg, bool dropImg, bool signImg, int Id = 1, OrderAdditionalUpdateType updateType = OrderAdditionalUpdateType.SupplierAllowance)
        {
            using (var tracer = new Tracer("OrderController", "UpdateOrderAdditionalDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateOrderAdditionalDetailsAsync(viewmodel, bolImg, dropImg, signImg, Id, UserContext, updateType);
                return Json(response);
            }
        }

        [HttpPost]
        public async Task<JsonResult> OrdersGrid(OrderDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("OrderController", "OrdersGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(requestModel);

                var dashboardDomain = new DashboardDomain();
                var orderDomain = new OrderDomain(dashboardDomain);
                var decryptedGroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);

                var orderFilter = new OrderFilterViewModel() { JobId = requestModel.JobId, StartDate = requestModel.StartDate, EndDate = requestModel.EndDate, OrderId = requestModel.OrderId, FuelTypeId = requestModel.FuelTypeId, Filter = requestModel.Filter, Currency = requestModel.Currency, CountryId = requestModel.CountryId, GroupIds = decryptedGroupIds, CustomerIds = requestModel.CustomerIds, VesselIds = requestModel.VesselIds, LocationIds = requestModel.LocationIds, IsMarine = requestModel.IsMarine };
                var response = await orderDomain.GetSupplierOrders(CurrentUser.CompanyId, dataTableSearchModel, orderFilter);
                var totalCount = 0;

                if (response.Count > 0)
                {
                    totalCount = response[0].TotalCount;
                }

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = requestModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<ActionResult> AssignNewTerminalToOrder(int terminalId, int orderId, bool IsTierPricedOrder = false)
        {
            using (var tracer = new Tracer("OrderController", "AssignNewTerminalToOrder"))
            {
                if (!IsTierPricedOrder)
                {
                    var response = await ContextFactory.Current.GetDomain<OrderDomain>().AssignNewTerminalToOrderAsync(terminalId, orderId);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }
                else
                {
                    var response = await ContextFactory.Current.GetDomain<OrderDomain>().AssignNewTerminalForTierPricedOrder(terminalId, orderId);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }


            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> AssignNewPoContact(int poCotactId, int orderId)
        {
            using (var tracer = new Tracer("OrderController", "AssignNewPoContact"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().AssignNewPoContact(poCotactId, orderId);
                if (response.StatusCode == Status.Success)
                {
                    var poContactDetails = await ContextFactory.Current.GetDomain<HelperDomain>().GetPoContactAsync(poCotactId);
                    return Json(new
                    {
                        poContactName = poContactDetails.Name,
                        poContactEmail = poContactDetails.Email,
                        poContactPhoneNumber = poContactDetails.PhoneNumber,
                        Message = response.StatusMessage,
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Message = response.StatusMessage,
                        StatusCode = (int)response.StatusCode
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> AssignNewCityGroupTerminalToOrder(int terminalId, int orderId)
        {
            using (var tracer = new Tracer("OrderController", "AssignNewCityGroupTerminalToOrder"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().AssignNewTerminalToOrderAsync(terminalId, orderId, true);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return PartialView("_DisplayCustomMessage");
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<JsonResult> SetDefaultInvoiceTypeForOrder(int orderId, bool defaultInvoiceTypeManual)
        {
            using (var tracer = new Tracer("OrderController", "SetDefaultInvoiceTypeForOrder"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SetDefaultInvoiceTypeForOrder(orderId, defaultInvoiceTypeManual);
                return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
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
                return RedirectToAction("Details", "Order", new { area = "Supplier", id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Cancel(CancelOrderViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "Cancel(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.CanceledBy = CurrentUser.Id;
                    var originalOrderId = viewModel.OrderId;
                    var response = await ContextFactory.Current.GetDomain<OrderDomain>().CancelOrderAsync(UserContext, viewModel, false, false);

                    if (response.StatusCode == Status.Failed)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return RedirectToAction("Details", "Order", new { area = "Supplier", id = originalOrderId });
                    }
                    else
                    {
                        var drCloseStatus = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().DeleteDeliveryRequestOnOrderClose(new List<int> { viewModel.OrderId }, UserContext);
                        if (drCloseStatus.StatusCode != (int)Status.Success)
                        {
                            DisplayCustomMessages((MessageType)drCloseStatus.StatusCode, drCloseStatus.StatusMessage);
                        }
                    }
                    return RedirectToAction("View", "Order", new { area = "Supplier" });
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
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
        public ActionResult PartialMapView(OrderFilterViewModel orderFilter = null)
        {
            using (var tracer = new Tracer("OrderController", "PartialMapView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetSupplierMapAsync(CurrentUser.Id, orderFilter)).Result;
                var jsonResult = new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                return PartialView("_PartialMapView", jsonResult);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
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
                return RedirectToAction("Details", "Order", new { area = "Supplier", id = id });
            }
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult OrderPoView(int id)
        {
            using (var tracer = new Tracer("OrderController", "OrderPoView"))
            {
                var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetOrderPoAsync(id, 0)).Result;
                return PartialView("_PartialOrderPo", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ChooseOrder(int id, int selectedId)
        {
            using (var tracer = new Tracer("OrderController", "ChooseOrder"))
            {
                OrderSelectionViewModel viewModel = new OrderSelectionViewModel()
                {
                    OrderId = id,
                    UserId = CurrentUser.Id,
                    SelectedOrderId = selectedId
                };
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().ChooseOrderAsync(viewModel, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Supplier", id = response.SelectedOrderId });
            }
        }

        [HttpGet]
        public ActionResult DeliverySchedule(int scheduleType = (int)DeliveryScheduleType.SpecificDates, bool isThirdPartyCall = false, bool isFtl = false, int orderId = 0)
        {
            var deliverySchedule = new DeliveryScheduleViewModel() { ScheduleType = scheduleType, CreatedBy = CurrentUser.Id };
            if (orderId > 0)
            {
                deliverySchedule = ContextFactory.Current.GetDomain<OrderDomain>().GetDefaultScheduleTime(orderId, deliverySchedule);
            }
            if (isThirdPartyCall)
            {
                return PartialView("~/Areas/Buyer/Views/Shared/_PartialDeliveryScheduleFR.cshtml", deliverySchedule);
            }

            deliverySchedule.IsFtlOrder = isFtl;
            return PartialView("_PartialDeliveryScheduleSupplier", deliverySchedule);
        }

        public ActionResult AddLocationForSchedule(int countryId, string countryCode, string prefix = "")
        {
            return PartialView("_PartialSplitLoadDropLocation", new SplitLoadAddressViewModel() { CollectionHtmlPrefix = prefix, CountryId = countryId, CountryCode = countryCode });
        }

        [HttpGet]
        public ActionResult RescheduleDeliverySchedule(int trackableScheduleId, int scheduleType = (int)DeliveryScheduleType.SpecificDates)
        {
            using (var tracer = new Tracer("OrderController", "RescheduleDeliverySchedule"))
            {
                var deliverySchedule = ContextFactory.Current.GetDomain<OrderDomain>().GetDeliveryScheduleByTrackableScheduleId(trackableScheduleId);
                deliverySchedule.ScheduleType = scheduleType;
                deliverySchedule.CreatedBy = CurrentUser.Id;
                deliverySchedule.RescheduledTrackableId = trackableScheduleId;
                deliverySchedule.StatusId = (int)DeliveryScheduleStatus.Rescheduled;
                deliverySchedule.IsRescheduled = true;
                return PartialView("_PartialDeliveryScheduleSupplier", deliverySchedule);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<ActionResult> SaveDeliverySchedules(OrderDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveDeliverySchedules"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveDeliverySchedulesAsync(UserContext, viewModel, false);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.Id });
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
        public ActionResult OrderHistory(int id)
        {
            var response = Task.Run(() => ContextFactory.Current.GetDomain<OrderDomain>().GetSupplierOrderHistoryAsync(id, CurrentUser.Id)).Result;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> AssignDriver(int id, int? driverId)
        {
            using (var tracer = new Tracer("OrderController", "AssignDriver"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().AssignDriverToOrder(UserContext, id, driverId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateTogglePricingDetails(int orderId, bool isHidePricingEnabled, bool IsBrokeredOrder = false)
        {
            using (var tracer = new Tracer("OrderController", "UpdateTogglePricingDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateTogglePricingDetailsAsync(UserContext, orderId, isHidePricingEnabled, IsBrokeredOrder ? CompanyType.Buyer : CompanyType.Supplier);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
                return Json(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateIsPrePostDipRequired(int orderId, bool isPrePostDipRequired)
        {
            using (var tracer = new Tracer("OrderController", "UpdateIsPrePostDipRequired"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateIsPrePostDipRequired(UserContext, orderId, isPrePostDipRequired);
                return Json(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFTLCheckDetails(int orderId, bool isFTL)
        {
            using (var tracer = new Tracer("OrderController", "UpdateFTLCheckDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateFTLCheckDetailsAsync(UserContext, orderId, isFTL);
                //if (response.StatusCode == Status.Failed)
                //{
                //    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                //}
                return Json(response);
            }
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
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().ProcessTrackableScheduleAsync(UserContext, trackableScheduleId, deliveryScheduleStatusId, false);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(UserContext, EntityType.Order, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            using (var tracer = new Tracer("OrderController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().InitializeTpoViewModel(UserContext);
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Clone(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "Clone"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GelCloneOrderDetails(orderId, UserContext);

                if (response.FuelDetails.IsMarineLocation)
                {
                    response.CurrentCompanyId = UserContext.CompanyId;
                    return View("CreateTPONomination", response);
                }
                else
                {
                    return View("Create", response);
                }
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetPickUpLocation(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetPickUpLocation"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderPickUpLocationAsync(orderId);
                return PartialView("_PartialPickUpLocation", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ModifyPickUpLocation(PickUpAddressViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "ModifyPickUpLocation"))
            {

                //if (ModelState.IsValid)
                //{
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().ModifyPickUpLocationAsync(UserContext, viewModel);
                return Json(response, JsonRequestBehavior.AllowGet);
                //}
                //return Json(new StatusViewModel(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InActivePickUpLocation(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "InActivePickUpLocation"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<OrderDomain>().InActivePickUpLocationAsync(orderId);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                return Json(new StatusViewModel(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(ThirdPartyOrderViewModel thirdPartyOrderViewModel)
        {
            using (var tracer = new Tracer("OrderController", "Create(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    if (thirdPartyOrderViewModel.CustomerDetails.ContactPersons != null && thirdPartyOrderViewModel.CustomerDetails.ContactPersons.Any())
                    {
                        var userEmails = thirdPartyOrderViewModel.CustomerDetails.ContactPersons.Select(t => t.Email).ToList();
                        userEmails.Add(thirdPartyOrderViewModel.CustomerDetails.Email);
                        var duplicate = userEmails
                                                .GroupBy(x => x)
                                                .Where(group => group.Count() > 1)
                                                .Select(group => group.Key).ToList();

                        if (duplicate != null && duplicate.Any())
                        {
                            string duplicateEmails = string.Join(", ", duplicate);
                            thirdPartyOrderViewModel.StatusCode = Status.Failed;
                            thirdPartyOrderViewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageDuplicateEntries,
                                new[] { duplicate.Count == 1 ? $"{duplicateEmails} is" : $"{duplicateEmails} are" });

                            DisplayCustomMessages((MessageType)thirdPartyOrderViewModel.StatusCode, thirdPartyOrderViewModel.StatusMessage);
                            return View("Create", thirdPartyOrderViewModel);
                        }
                    }

                    // Save additional image and site map for TPO.
                    var errorList = ValidateFileUpload(thirdPartyOrderViewModel.ImageDetails);
                    if (errorList.Length > 0)
                    {
                        thirdPartyOrderViewModel.StatusCode = Status.Failed;
                        thirdPartyOrderViewModel.StatusMessage = errorList.ToString();
                        DisplayCustomMessages((MessageType)thirdPartyOrderViewModel.StatusCode, thirdPartyOrderViewModel.StatusMessage);
                        return View("Create", thirdPartyOrderViewModel);
                    }
                    if (string.IsNullOrWhiteSpace(thirdPartyOrderViewModel.AddressDetails.CountyName))
                    {
                        thirdPartyOrderViewModel.AddressDetails.CountyName = thirdPartyOrderViewModel.AddressDetails.City;
                    }
                    else
                    {
                        thirdPartyOrderViewModel.ImageDetails.SiteImage = await SetImageDataToBolb(thirdPartyOrderViewModel.ImageDetails.SiteImage, thirdPartyOrderViewModel.ImageDetails.SiteImageFiles, BlobContainerType.JobFilesUpload);
                        thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImage = await SetImageDataToBolb(thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImage, thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImageFiles, BlobContainerType.JobFilesUpload);
                    }

                    var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateThirdPartyOrder(UserContext, thirdPartyOrderViewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        //Save the forcasting setting details.
                        //if (thirdPartyOrderViewModel.AddressDetails.IsRetailJob)
                        //{
                        var IsTpoBuyer = false;
                        if (thirdPartyOrderViewModel.CustomerDetails.CompanyId.HasValue && thirdPartyOrderViewModel.CustomerDetails.CompanyId > 0)
                        {
                            IsTpoBuyer = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().IsValidTpoCompany(thirdPartyOrderViewModel.CustomerDetails.CompanyId.Value);
                        }
                        var forcastingResponse = await Saveforcastingdetails(thirdPartyOrderViewModel, response);
                        if (forcastingResponse.StatusCode == Status.Failed)
                        {
                            DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                            return View("Create", thirdPartyOrderViewModel);
                        }
                        else
                        {
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        }
                        //}
                        //if (thirdPartyOrderViewModel.IsAssetTracked || thirdPartyOrderViewModel.AddressDetails.IsRetailJob)
                        //{
                        if (IsTpoBuyer)
                        {
                            return RedirectToAction("AssignAssetTank", "Order", new { area = "Supplier", oId = thirdPartyOrderViewModel.OrderId, jId = thirdPartyOrderViewModel.AddressDetails.JobId });
                        }
                        else
                        {
                            return RedirectToAction("Details", "Order", new { area = "Supplier", id = thirdPartyOrderViewModel.OrderId });
                        }
                    }
                    else
                    {
                        if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing != null && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Any())
                        {
                            //if (thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Where(w => w.ToQuantity == 0).FirstOrDefault() != null)
                            //{
                            //   thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Where(w => w.ToQuantity == 0).FirstOrDefault().
                            //    FromQuantity= thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.ToList().OrderByDescending(o => o.ToQuantity).FirstOrDefault().ToQuantity;
                            //}
                            thirdPartyOrderViewModel.FuelDetails.TierPricing = thirdPartyOrderViewModel.FuelDetails.FuelPricing.TierPricing;

                        }
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        return View("Create", thirdPartyOrderViewModel);
                    }
                }
                return View("Create", thirdPartyOrderViewModel);
            }
        }

        private StringBuilder ValidateFileUpload(TPOSiteImageViewModel model)
        {
            //const int maxFileSize = 1048576;
            var errorList = new StringBuilder();
            if (model.SiteImageFiles != null)
            {
                foreach (var file in model.SiteImageFiles)
                {
                    if (file?.ContentLength > ApplicationConstants.TFXImageAndPdfAllowedFileUploadSizeInBytes)
                    {
                        errorList.Append(string.Format(Resource.valMsgSiteImageUpload, file.FileName));
                    }
                }
            }

            if (model.AdditionalImage.SiteImageFiles != null)
            {
                foreach (var file in model.AdditionalImage.SiteImageFiles)
                {
                    if (file?.ContentLength > ApplicationConstants.TFXImageAndPdfAllowedFileUploadSizeInBytes)
                    {
                        errorList.Append(string.Format(Resource.valMsgSiteImageUpload, file.FileName));
                    }
                }
            }

            return errorList;
        }

        [HttpPost]
        public async Task<PartialViewResult> SendDeliveryNotification(int orderId, string poNumber, int driverId, int groupId)
        {
            using (var tracer = new Tracer("OrderController", "SendDeliveryNotification"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SendDeliveryNotification(orderId, poNumber, driverId, groupId);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return PartialView("_DisplayCustomMessage");
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public ActionResult AssignAssetTank(int oId, int jId)
        {
            var assetFilterViewModel = new AssetFilterViewModel();
            assetFilterViewModel.JobId = jId;
            assetFilterViewModel.OrderId = oId;
            var response = ContextFactory.Current.GetDomain<HelperDomain>().IsRetailJob(jId);
            assetFilterViewModel.IsRetailJob = response;
            ViewBag.JobId = jId;
            return View(assetFilterViewModel);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> AssignAssets(List<int> assets)
        {
            using (var tracer = new Tracer("OrderController", "AssignAssets"))
            {
                var jobId = Convert.ToInt32(Request.QueryString["jId"]);
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().AssignAssetsToJobAsync(UserContext, jobId, assets, true);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response.StatusCode, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> GetJobDetails(string jobName, string companyName)
        {
            using (var tracer = new Tracer("OrderController", "GetJobDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetJobDetails(jobName, companyName, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult GetTPOCompanyContactPersons(int companyId)
        {
            using (var tracer = new Tracer("OrderController", "GetTPOCompanyContactPersons"))
            {
                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetTPOCompanyContactPersons(companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult GetTPOContactPersonDetails(int userId)
        {
            using (var tracer = new Tracer("OrderController", "GetTPOContactPersonDetails"))
            {
                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetTPOContactPersonDetails(userId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public ActionResult GetJobSpecificBillingAddressDetails(int jobId, int customerId = 0)
        {
            var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetJobSpecificBillingAddressDetails(jobId, customerId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> GetBillingAddress(int companyId)
        {
            using (var tracer = new Tracer("OrderController", "GetBillingAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetBillingAddress(companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public JsonResult GetJobList(string companyName, bool isFtl, bool isFoAsTerminal, int supplierUserId, int supplierCompanyId, bool isPort = false, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("OrderController", "GetJobList"))
            {
                var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetJobList(companyName, isFtl, isFoAsTerminal, supplierUserId, supplierCompanyId, isPort, countryId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetClosedTerminal(int fuelTypeId, decimal latitude, decimal longitude, int countryId, int pricingCodeId, string terminal = "", int orderId = 0, int pricingSourceId = (int)PricingSource.Axxis)
        {
            using (var tracer = new Tracer("OrderController", "GetClosedTerminal"))
            {
                if (orderId > 0)
                {
                    var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(orderId, terminal);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var fueltype = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelTypeId(fuelTypeId, pricingSourceId);
                    var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetClosestTerminals(fueltype, latitude, longitude, countryId, terminal, pricingCodeId, UserContext.CompanyId);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult GetOpisTerminals(int cityRackId = 0, decimal latitude = 0, decimal longitude = 0, int countryId = 1, string terminal = "", PricingSource source = PricingSource.Axxis)
        {
            var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
            var response = pricingDomain.GetOpisTerminals(cityRackId, latitude, longitude, countryId, terminal.Trim(), source, UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ValidateJobName(string jobName, string companyName)
        {
            using (var tracer = new Tracer("OrderController", "ValidateJobName"))
            {
                var isValidJobName = true;
                var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetJobDetails(jobName, companyName, UserContext);
                if (response.Job.Id > 0)
                {
                    isValidJobName = false;
                }
                return Json(isValidJobName, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ValidateJobNameByCompanyId(string jobName, int companyId)
        {
            using (var tracer = new Tracer("OrderController", "ValidateJobNameByCompanyId"))
            {
                var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().ValidateJobNameByCompanyId(jobName, companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<ActionResult> CreateAsset(int oId, int jId, int type = 1, int assetId = 0, bool isCallFromOrderDetails = false)
        {
            using (var tracer = new Tracer("OrderController", "CreateAsset"))
            {
                var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                var response = await assetDomain.GetAssetAsync(assetId, jId);
                if (assetId == 0)
                {
                    response.AssetAdditionalDetail.DipTestMethod = await assetDomain.GetDefaultDiptest(CurrentUser.CompanyId);
                    response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Job, jId);
                }
                else
                {
                    response.ForcastingPreference = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().GetForCastingPreferanceSetting(UserContext, (int)ForcastingSettingLevel.Tank, jId);
                }
                var buyerCompanyId = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrdersBuyerCompanyIdAsync(oId);
                ViewBag.OrderId = oId;
                ViewBag.JobId = jId;
                ViewBag.Type = type;
                ViewBag.isCallFromOrderDetails = isCallFromOrderDetails;
                ViewBag.ShouldJobGetAssigned = true;
                response.CompanyId = buyerCompanyId;
                response.Type = type;
                response.AssetAdditionalDetail.Type = type;
                response.JobId = jId;
                response.MaxAllowedFileSize = AppSettings.MaxAllowedUploadFileSize;
                return PartialView("CreateAsset", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<ActionResult> CreateAsset(AssetViewModel viewModel, HttpPostedFileBase imageFile = null, int oId = 0, int? jId = 0, bool shouldJobGetAssigned = false, bool isCallFromOrderDetails = false)
        {
            using (var tracer = new Tracer("OrderController", "CreateAsset"))
            {
                if (ModelState.IsValid)
                {
                    var response = new StatusViewModel();
                    viewModel.UserId = CurrentUser.Id;
                    viewModel.CreatedDate = DateTimeOffset.Now;
                    if (imageFile != null)
                    {
                        var reader = new BinaryReader(imageFile.InputStream);
                        byte[] imageData = reader.ReadBytes((int)imageFile.InputStream.Length);
                        viewModel.Image = new ImageViewModel() { Data = imageData };
                        await viewModel.Image.UploadImageToAzureBlobService(ApplicationConstants.AssetDropImageFileNamePrefix, BlobContainerType.JobFilesUpload);
                    }
                    if (viewModel.Id > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().UpdateAssetAsync(UserContext, viewModel);
                    }
                    else
                    {
                        response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateAssetsAsync(UserContext, viewModel, jId.Value);
                    }

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        var forcastingResponse = await SaveAssetforcastingdetails(viewModel, response);
                        if (forcastingResponse.StatusCode == Status.Failed)
                        {
                            DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                        }
                        if (isCallFromOrderDetails)
                        {
                            return RedirectToAction("Details", "Order", new { area = "Supplier", id = oId });
                        }

                        return RedirectToAction("AssignAssetTank", "Order", new { area = "Supplier", oId, jId });
                    }
                }

                ViewBag.OrderId = oId;
                ViewBag.JobId = jId;
                ViewBag.Type = viewModel.Type;
                ViewBag.ShouldJobGetAssigned = true;
                ViewBag.isCallFromOrderDetails = isCallFromOrderDetails;
                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<JsonResult> AssignAssetsToJob(int jId)
        {
            using (var tracer = new Tracer("OrderController", "AssignAssetsToJob"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTpoAssetGridAsync(UserContext, jId, (int)AssetType.Asset);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> AssignTanksToJob(int jId)
        {
            using (var tracer = new Tracer("OrderController", "AssignAssetsToJob"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTpoAssetGridAsync(UserContext, jId, (int)AssetType.Tank);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> AssetBulkUpload(AssetViewModel viewModel, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("OrderController", "AssetBulkUpload"))
            {
                var orderId = Convert.ToInt32(Request.QueryString["oId"]);
                var jobId = Convert.ToInt32(Request.QueryString["jId"]);

                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {
                        string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                        var csvFilePath = Server.MapPath(Resource.assetBulkUploadFilePath);

                        var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                        var response = assetDomain.ValidateCsvHeader(csvText, csvFilePath);
                        if (response.StatusCode == Status.Success)
                        {
                            response = await assetDomain.SaveBulkAssetsAsync(csvText.Trim(), CurrentUser.Id, CurrentUser.CompanyId, jobId);
                            if (response.StatusMessage.Equals(Resource.errMessageDuplicateAssetName))
                            {
                                response.StatusMessage = Resource.errMessageDuplicateAssetTpo;
                            }
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            if (response.StatusCode != Status.Failed)
                            {
                                return RedirectToAction("AssignAssetTank", "Order", new { area = "Supplier", oId = orderId, jId = jobId });
                            }
                        }
                        else
                        {
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }

                return RedirectToAction("CreateAsset", "Order", new { area = "Supplier", oId = orderId, jId = jobId, type = (int)AssetType.Asset });
            }
        }

        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TankBulkUpload(AssetViewModel viewModel, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("AssetController", "TankBulkUpload"))
            {
                var orderId = Convert.ToInt32(Request.QueryString["oId"]);
                var jobId = Convert.ToInt32(Request.QueryString["jId"]);
                bool isError = false;
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath("~\\Content\\Tank_BulkUpload_Template.csv");

                            var tankBulkUploadDomain = ContextFactory.Current.GetDomain<TankBulkUploadDomain>();
                            var response = await tankBulkUploadDomain.ValidateTankBulkFile(csvText, csvFilePath, UserContext, CompanyType.Supplier, jobId);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await tankBulkUploadDomain.UploadTankFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Supplier);
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    isError = true;
                                }
                            }
                            else
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                isError = true;
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                            isError = true;
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                        isError = true;
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                    isError = true;
                }

                if (isError)
                {
                    return RedirectToAction("AssignAssetTank", "Order", new { area = "Supplier", oId = orderId, jId = jobId });
                }
                return RedirectToAction("CreateAsset", "Order", new { area = "Supplier", oId = orderId, jId = jobId, type = (int)AssetType.Tank });
            }
        }

        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TPOTankBulkUpload(HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("AssetController", "TPOTankBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath("~\\Content\\TPO_Tank_BulkUpload_Template.csv");

                            var tankBulkUploadDomain = ContextFactory.Current.GetDomain<TankBulkUploadDomain>();
                            var response = tankBulkUploadDomain.ValidateCsvHeader(csvText, csvFilePath);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await tankBulkUploadDomain.UploadTankFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Supplier, true);
                            }
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }

                return RedirectToAction("View", "Job", new { area = "Supplier" });
            }
        }

        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TPOAssetBulkUpload(HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("AssetController", "TPOAssetBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath("~\\Content\\TPO_Asset_BulkUpload_Template.csv");

                            var assetDomain = ContextFactory.Current.GetDomain<AssetDomain>();
                            var response = assetDomain.ValidateTPOAssetCsvHeader(csvText, csvFilePath);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await assetDomain.UploadAssetFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Supplier);
                            }
                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }

                return RedirectToAction("View", "Job", new { area = "Supplier" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.Dispatcher, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> OrderBulkUpload(ThirdPartyOrderViewModel viewModel, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("OrderController", "OrderBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath(Resource.orderBulkUploadFilePath);

                            var tpoDomain = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>();
                            var response = await tpoDomain.ValidateOrderBulkFile(UserContext, csvText, csvFilePath);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await tpoDomain.UploadFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName);

                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    return RedirectToAction("View", "Order", new { area = "Supplier" });
                                }
                            }
                            else
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            }
                        }
                        else
                        {
                            DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                        }
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
                }
                return RedirectToAction("Create", "Order", new { area = "Supplier" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateFuelCurrentCost(UpdateCurrentCostViewModel updateCurrentCost)
        {
            using (var tracer = new Tracer("OrderController", "UpdateFuelCurrentCost"))
            {
                StatusViewModel response;
                if (updateCurrentCost.IsGlobalCost)
                {
                    response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().UpdateGlobalFuelCost(UserContext, updateCurrentCost);
                }
                else
                {
                    response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().UpdateSupplierCostForOrder(UserContext, updateCurrentCost);
                }
                var calculatedPPG = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetCurrenSupplierCost(updateCurrentCost.PriceRequestDetailId, updateCurrentCost.FuelCost);
                return Json(new { CalculatedPpg = calculatedPPG, Message = response.StatusMessage, StatusCode = (int)response.StatusCode }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> GetCalculatedFuelCostPrice(int priceDetailId)
        {
            using (var tracer = new Tracer("OrderController", "GetCalculatedFuelCostPrice"))
            {
                var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetCalculatedFuelCostPriceAsync(priceDetailId);
                return Json(new { CalculatedPpg = response.CalculatedFuelCost, FuelCost = response.FuelCost }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> GetGlobalFuelCost(int fuelTypeId, int jobStateId, UoM uom, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("OrderController", "GetGlobalFuelCost"))
            {
                var response = await ContextFactory.Current.GetDomain<CurrentCostDomain>().GetGlobalCost(UserContext, fuelTypeId, jobStateId, uom, currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> Edit(int id, bool isBrokeredRequest = false)
        {
            using (var tracer = new Tracer("OrderController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetSupplierOrderDetailsAsync(id, CurrentUser.Id, UserContext, isBrokeredRequest);
                return View(response);
            }
        }

        [HttpGet]
        //[OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult DeliveryFeeByQuantity(string prefix, Currency currency, UoM uoM)
        {
            var deliveryFeeByQuantityModel = new DeliveryFeeByQuantityViewModel();
            deliveryFeeByQuantityModel.CollectionHtmlPrefix = prefix;
            deliveryFeeByQuantityModel.Currency = currency;
            deliveryFeeByQuantityModel.UoM = uoM;
            return PartialView("~/Views/Shared/_PartialDeliveryFeeByQuantity.cshtml", deliveryFeeByQuantityModel);
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult AdditionalFee()
        {
            return PartialView("_PartialBrokeredAdditionalFee", new BrokeredOrderFeeViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> GetTpoCompaniesById(string prefix)
        {
            List<CompanyViewModel> companies = await ContextFactory.Current.GetDomain<CompanyDomain>().GetTpoCompaniesById(UserContext.CompanyId);
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                companies = companies.Where(t => t.Name.Trim().ToLower().Contains(prefix.ToLower().Trim())).ToList();
            }
            return Json(companies, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetUsersByCompany(string prefix, int companyId)
        {
            List<TPOCustomerViewModel> users = await ContextFactory.Current.GetDomain<CompanyDomain>().GetUsersByCompany(companyId);
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                users = users.Where(t => t.FullName.Trim().ToLower().StartsWith(prefix.ToLower().Trim())).ToList();
            }
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OtherProductTax()
        {
            return PartialView("_PartialOtherProductTax", new OrderTaxDetailsViewModel());
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> SaveOtherPorductTypeTaxes(OrderDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveOtherPorductTypeTaxes"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveOtherPorductTypeTaxes(UserContext, viewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.Id });
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> SaveBadgeDetails(OrderDetailsViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveBadgeDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveBadgeDetails(UserContext, viewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.Id });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> ViewAssetInformation(int jId)
        {
            using (var tracer = new Tracer("OrderController", "ViewAssetInformation"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetInfoGridAsync(CurrentUser.Id, jId, (int)AssetType.Asset);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> ViewTankInformation(int jId)
        {
            using (var tracer = new Tracer("OrderController", "ViewTankInformation"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssetInfoGridAsync(CurrentUser.Id, jId, (int)AssetType.Tank);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<JsonResult> ViewAssetHistory(int oId)
        {
            using (var tracer = new Tracer("OrderController", "ViewAssetHistory"))
            {
                var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetSupplierAssetHistoryViewAsync(oId);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSupplierOrderStat(int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetSupplierOrderStat"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetSupplierOrderStat(orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpPost]
        public async Task<JsonResult> UpdatePaymentTerms(OrderDetailsViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdatePaymentTerms(UserContext, model.Id, model.PaymentTermId, model.NetDays, model.PaymentMethod);
            if (response.StatusCode == Status.Success)
            {
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpPost]
        public async Task<JsonResult> UpdateFuelSurchargeFreightFee(OrderDetailsViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateFuelSurchargeFreightFee(UserContext, model);
            if (response.StatusCode == Status.Success)
            {
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpPost]
        public async Task<JsonResult> UpdateFreightRateAndFuelSurchargeForAuto(OrderDetailsViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateFreightRate(UserContext, model);
            if (response.StatusCode == Status.Success)
            {
                response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateFuelSurchargeForAuto(UserContext, model);
                if (response.StatusCode == Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);            
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        [HttpPost]
        public async Task<JsonResult> UpdateAccessorialFees(OrderDetailsViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateAccessorialFees(UserContext, model);
            if (response.StatusCode == Status.Success)
            {
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetTPOCompanyBillingAddress(int companyId)
        {
            var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetCompanyBillingAddress(companyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPricingSourceFeeds(int pricingSourceId)
        {
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetPricingSourceFeeds(pricingSourceId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllBuyerCompanyList(string companyName)
        {
            var response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetAllBuyerCompanies(companyName);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<JsonResult> UpdateWbsNumber(int fuelRequestId, string wbsNumber)
        {
            StatusViewModel response;
            response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateWbsNumber(UserContext, fuelRequestId, wbsNumber);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        [ValidateInput(false)]
        public async Task<JsonResult> UpdateInvoiceNotes(int id, string notes)
        {
            StatusViewModel response;
            response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateInvoiceNotesAsync(id, notes, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> SaveTankSchedules(TankRentalFrequencyViewModel viewModel)
        {
            using (var tracer = new Tracer("OrderController", "SaveTankSchedules"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().SaveTankSchedulesAsync(UserContext, viewModel, false);
                return Json(new { response.StatusCode, response.StatusMessage, viewModel.ActivationStatusId }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        [ValidateInput(false)]
        public async Task<JsonResult> RemoveTankSchedule(int frequencyId, int frId)
        {
            StatusViewModel response;
            response = await ContextFactory.Current.GetDomain<OrderDomain>().RemoveTankScheduleAsync(UserContext, frequencyId, frId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public ActionResult GetTankTypesGrid()
        {
            using (var tracer = new Tracer("AssetController", "GetTankTypesGrid"))
            {
                return PartialView("_PartialTankTypesGrid");
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        [HttpGet]
        public async Task<JsonResult> GetTankTypesByCompanyAsync()
        {
            using (var tracer = new Tracer("AssetController", "GetTankTypesByCompanyAsync"))
            {
                var response = new List<TankModalTypeViewModel>();
                try
                {

                    response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTankTypesByCompanyAsync(CurrentUser.CompanyId);

                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "GetTankTypesByCompanyAsync", ex.Message, ex);
                }

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        data = response,
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        [HttpPost]
        public async Task<JsonResult> TankTypeDipChartBulkUpload(int orderId, string tankTypeName, string tankTypeModal, int scaleMeasurement)
        {
            var response = new StatusViewModel();
            try
            {
                using (var tracer = new Tracer("AssetController", "TankTypeDipChartBulkUpload"))
                {
                    var tankTypeViewModel = new TankModalTypeViewModel { Name = tankTypeName, Modal = tankTypeModal, ScaleMeasurement = scaleMeasurement };

                    if (Request.Files.Count > 0)
                    {
                        var csvFile = Request.Files[0];

                        if (csvFile != null && csvFile.ContentLength > 0)
                        {
                            if (csvFile.ContentLength < AppSettings.MaxAllowedUploadFileSize)
                            {
                                if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                                {
                                    string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                                    var csvFilePath = Server.MapPath("~\\Content\\TankMakeModel_Bulkupload_Template.csv");
                                    var dipChartDetails = await ContextFactory.Current.GetDomain<TankBulkUploadDomain>().ValidateTankTypesBulkFile(csvText, csvFilePath);

                                    if (dipChartDetails.Count > 0)
                                    {
                                        //initilize
                                        tankTypeViewModel.DipChartDetails = dipChartDetails;
                                        tankTypeViewModel.CreatedBy = CurrentUser.Id;
                                        tankTypeViewModel.CreatedByCompanyId = CurrentUser.CompanyId;
                                        tankTypeViewModel.SupplierCompanyId = CurrentUser.CompanyId;
                                        tankTypeViewModel.BuyerCompanyId = await ContextFactory.Current.GetDomain<HelperDomain>().GetCompanyIdFromOrderId(orderId);
                                        tankTypeViewModel.IsActive = true;
                                        tankTypeViewModel.CreatedOn = DateTime.Now;

                                        if (scaleMeasurement == (int)TankScaleMeasurement.Cm)
                                        {
                                            tankTypeViewModel.ScaleMeasurementText = Resource.lblCm;
                                        }
                                        else if (scaleMeasurement == (int)TankScaleMeasurement.Inches)
                                        {
                                            tankTypeViewModel.ScaleMeasurementText = Resource.lblInches;
                                        }

                                        //generate pdf and set path
                                        Random rand = new Random();
                                        var fileName = tankTypeViewModel.Name.ToLower() + "-" + tankTypeViewModel.Modal.ToLower() + "-" + CurrentUser.CompanyId + "-" + rand.Next(100).ToString() + ".pdf";
                                        var partialPdfView = GetPartialViewPdfContent("_PartialTankTypeDipChartPdf", tankTypeViewModel);
                                        Stream stream = new MemoryStream(partialPdfView);
                                        var azureBlob = new AzureBlobStorage();
                                        tankTypeViewModel.PdfFilePath = await azureBlob.SaveBlobAsync(stream, fileName, BlobContainerType.TankTypeDipChart.ToString().ToLower());
                                        response = await ContextFactory.Current.GetDomain<AssetDomain>().SaveTankTypes(tankTypeViewModel);
                                    }
                                    else { response.StatusMessage = Resource.errorInvalidDataInFile; }
                                }
                                else { response.StatusMessage = Resource.errMessageSelectCsvFile; }
                            }
                            else { response.StatusMessage = Resource.errFileSizeMessage; }
                        }
                        else { response.StatusMessage = Resource.errMessageNoFileChosen; }
                    }
                    else { response.StatusMessage = Resource.errMessageNoFileChosen; }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssetController", "TankTypeDipChartBulkUpload", ex.Message, ex);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        [HttpPost]
        public async Task<JsonResult> DeleteTankDipChartById(string id)
        {
            using (var tracer = new Tracer("AssetController", "DeleteTankDipChartById"))
            {
                var response = new StatusViewModel();
                try
                {

                    response = await ContextFactory.Current.GetDomain<AssetDomain>().DeleteTankDipChartById(id);

                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AssetController", "DeleteTankDipChartById", ex.Message, ex);
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public JsonResult GetCarrierAndSelectedUserEmails(int assignedCarrierCompanyId, int jobId = 0, bool isNewJob = true)
        {
            using (var tracer = new Tracer("OrderController", "GetCarrierUserEmails"))
            {
                if (isNewJob)
                {
                    jobId = 0;
                }

                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCarrierAndSelectedUserEmails(assignedCarrierCompanyId, jobId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<JsonResult> GetCarriersAssignedToRegion(string regionId)
        {
            using (var tracer = new Tracer("OrderController", "GetCarriersAssignedToRegion"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().GetCarriersAssignedToRegion(regionId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public JsonResult GetAssignedCarriers(bool isJobEdit = false)
        {
            var response = CommonHelperMethods.GetCarriersTPO(CurrentUser.CompanyId, isJobEdit);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        [HttpGet]
        public async Task<JsonResult> GetAllTankTypeNameForDipChart(string searchValue)
        {
            var response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAllTankTypeNameForDipChart(CurrentUser.CompanyId, searchValue);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> EditLocationToRegion(JobToRegionAssignViewModel jobInfo)
        {
            var response = new JobToRegionAssignViewModel();
            try
            {
                if (jobInfo != null && jobInfo.JobId > 0)
                {
                    response.CompanyId = UserContext.CompanyId;
                    response.JobId = jobInfo.JobId;
                    response.JobName = jobInfo.JobName;
                    response.RegionId = jobInfo.RegionId;
                    response.UpdatedBy = UserContext.Id;
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "EditLocationToRegion", ex.Message, ex);
            }
            return PartialView("_PartialJobRegionAssignment", response);
        }

        [HttpPost]
        public async Task<ActionResult> SaveRegionAssignmentForJob(JobToRegionAssignViewModel jobToAssign)
        {
            var response = new StatusViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<FreightServiceDomain>().AssignJobToRegion(jobToAssign);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderController", "EditLocationToRegion", ex.Message, ex);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the name of the assigned product based on the selected terminal.
        /// </summary>
        /// <param name="terminalId">The terminal identifier.</param>
        /// <param name="orderId">The order identifier.</param>
        /// <returns>Assigned Product Name.</returns>
        [HttpPost]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<JsonResult> GetAssignedProductName(int terminalId, int orderId)
        {
            using (var tracer = new Tracer("OrderController", "GetAssignedProductName"))
            {
                var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetAssignedProductName(terminalId, orderId);
                var mapping = new { MyProductId = response?.MyProductId, BackOfficeProductId = response?.BackOfficeProductId, DriverProductId = response?.DriverProductId };
                return Json(mapping, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<JsonResult> UpdateOrderName(int id, string orderName)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("OrderController", "UpdateOrderName"))
            {
                response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateOrderNameAsync(UserContext, id, orderName);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<JsonResult> UpdateIsDispatchRetainedForBrokerOrders(int OrderId, bool IsDispatchRetained)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("OrderController", "UpdateIsDispatchRetainedForBrokerOrders"))
            {
                response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateIsDispatchRetainedForBrokerOrders(OrderId, IsDispatchRetained, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public JsonResult GetCarrierUserEmails(int assignedCarrierCompanyId)
        {
            using (var tracer = new Tracer("OrderController", "GetCarrierUserEmails"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCarrierUserEmails(assignedCarrierCompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        private async Task<StatusViewModel> Saveforcastingdetails(ThirdPartyOrderViewModel viewModel, StatusViewModel response)
        {
            viewModel.ForcastingPreference.ForcastingServiceSetting = viewModel.ForcastingServiceSetting;
            var forcastingResponse = await SaveforcastingSettingTPOLevel(viewModel.ForcastingPreference, response);
            return forcastingResponse;
        }
        private async Task<StatusViewModel> SaveforcastingSettingTPOLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                viewModel.IsEditable = viewModel.ForcastingServiceSetting.IsEditableTpo;
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Job, response.EntityId, response.CustomerCompanyId);
                await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().InactiveJOBPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Tank, response.EntityId, response.CustomerCompanyId);
            }
            return statusViewModel;
        }
        private async Task<StatusViewModel> SaveAssetforcastingdetails(AssetViewModel viewModel, StatusViewModel response)
        {
            var forcastingResponse = await SaveforcastingSettingAssetLevel(viewModel.ForcastingPreference, response);
            return forcastingResponse;
        }
        private async Task<StatusViewModel> SaveforcastingSettingAssetLevel(ForcastingPreferenceViewModel viewModel, StatusViewModel response)
        {
            StatusViewModel statusViewModel = new StatusViewModel();
            statusViewModel.StatusCode = Status.Success;
            if (viewModel != null && viewModel.ForcastingServiceSetting != null && response.StatusCode == (int)Status.Success)
            {
                statusViewModel = await ContextFactory.Current.GetDomain<ForcastingServiceDomain>().SaveForeCastingPreferanceSetting(viewModel, UserContext, (int)ForcastingSettingLevel.Tank, response.EntityId, 0, response.CustomerCompanyId);
            }
            return statusViewModel;
        }

        [HttpGet]
        public async Task<ActionResult> CreateTPONomination()
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().InitializeTpoViewModel(UserContext);
            response.FuelDetails.IsMarineLocation = true;
            response.CurrentCompanyId = UserContext.CompanyId;
            response.AddressDetails.IsMarineLocation = true;
            response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.MarineFuelType;
            response.FuelDeliveryDetails.OrderEnforcementId = OrderEnforcement.NoEnforcement;
            response.OrderAdditionalDetailsViewModel.BOLInvoicePreferenceTypes = InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails;
            response.OrderAdditionalDetailsViewModel.IsIncludePricing = true;
            response.OrderAdditionalDetailsViewModel.IsPdiTaxRequired = true;
            response.OrderAdditionalDetailsViewModel.IsManualInvoiceConfirmationRequired = true;
            response.FuelDeliveryDetails.IsBolImageRequired = true;
            response.IsSupressOrderPricing = false; //for tponomination supress pricing flag should always be disabled
            response.DefaultInvoiceType = (int)InvoiceType.Manual;
            response.OrderAdditionalDetailsViewModel.FreightPricingMethod = FreightPricingMethod.Manual;
            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTPONomination(ThirdPartyOrderViewModel thirdPartyOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                // Save additional image and site map for TPO.
                var errorList = ValidateFileUpload(thirdPartyOrderViewModel.ImageDetails);
                if (errorList.Length > 0)
                {
                    thirdPartyOrderViewModel.StatusCode = Status.Failed;
                    thirdPartyOrderViewModel.StatusMessage = errorList.ToString();
                    DisplayCustomMessages((MessageType)thirdPartyOrderViewModel.StatusCode, thirdPartyOrderViewModel.StatusMessage);
                    return View("CreateTPONomination", thirdPartyOrderViewModel);
                }
                else
                {
                    thirdPartyOrderViewModel.ImageDetails.SiteImage = await SetImageDataToBolb(thirdPartyOrderViewModel.ImageDetails.SiteImage, thirdPartyOrderViewModel.ImageDetails.SiteImageFiles, BlobContainerType.JobFilesUpload);
                    thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImage = await SetImageDataToBolb(thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImage, thirdPartyOrderViewModel.ImageDetails.AdditionalImage.SiteImageFiles, BlobContainerType.JobFilesUpload);
                }

                var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().CreateThirdPartyOrder(UserContext, thirdPartyOrderViewModel);
                if (response.StatusCode == Status.Success)
                {
                    //var forcastingResponse = await Saveforcastingdetails(thirdPartyOrderViewModel, response);
                    //if (forcastingResponse.StatusCode == Status.Failed)
                    //{
                    //    DisplayCustomMessages((MessageType)forcastingResponse.StatusCode, forcastingResponse.StatusMessage);
                    //    return View("CreateTPONomination", thirdPartyOrderViewModel);
                    //}
                    //else
                    //{
                    //    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    //}

                    return RedirectToAction("Details", "Order", new { area = "Supplier", id = thirdPartyOrderViewModel.OrderId });
                    //return RedirectToAction("AssignAssetTank", "Order", new { area = "Supplier", oId = thirdPartyOrderViewModel.OrderId, jId = thirdPartyOrderViewModel.AddressDetails.JobId });
                }
                else
                {
                    if (thirdPartyOrderViewModel.FuelDetails.IsTierPricing && thirdPartyOrderViewModel.FuelDetails.TierPricing != null && thirdPartyOrderViewModel.FuelDetails.TierPricing.Pricings.Any())
                    {
                        thirdPartyOrderViewModel.FuelDetails.TierPricing = thirdPartyOrderViewModel.FuelDetails.FuelPricing.TierPricing;

                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return View("CreateTPONomination", thirdPartyOrderViewModel);
                }
            }
            return View("CreateTPONomination", thirdPartyOrderViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SaveTPOBulkUploadCsv(HttpPostedFileBase csvFile)
        {
            if (csvFile != null && csvFile.ContentLength > 0)
            {
                if (csvFile.ContentLength < AppSettings.MaxAllowedUploadFileSize)
                {
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {

                        var domain = ContextFactory.Current.GetDomain<OrderDomain>();
                        var csvOrdersFilePath = Server.MapPath("~\\Content\\Orders_BulkUpload_Template.csv");
                        var response = await domain.ProcessUploadedOrderCsvFile(csvFile, csvOrdersFilePath, UserContext);
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    }
                    else
                    {
                        DisplayCustomMessages(MessageType.Error, Resource.errMessageSelectCsvFile);
                    }
                }
                else
                {
                    DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                }
            }
            else
            {
                DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
            }
            return RedirectToAction("Create", "Order", new { area = "Supplier" });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateSourceRegionDetails(SourceRegionsViewModel inputModel)
        {
            var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().UpdateOrderSourceRegionsAsync(inputModel, UserContext);

            return Json(response);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateIncludePricingFlagForPDI(int orderId, bool isIncludePricing)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("OrderController", "UpdateIncludePricingFlagForPDI"))
            {
                response = await ContextFactory.Current.GetDomain<OrderDomain>().UpdateIncludePricingFlagForPDI(orderId, isIncludePricing, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetPortDetails(int portId, string portName = "")
        {
            var response = await ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetPortDetails(portId, portName, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}