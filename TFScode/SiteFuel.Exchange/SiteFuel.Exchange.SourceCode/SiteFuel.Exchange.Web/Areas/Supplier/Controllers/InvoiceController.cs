using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Services;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
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
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class InvoiceController : InvoiceBaseController
    {
        public int carrierUserId = 0;
        public string reportDate = string.Empty;
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "Index"))
            {
                if (Request.QueryString["carrierCompanyId"] != null)
                {
                    int.TryParse(Request.QueryString["carrierCompanyId"].ToString(), out carrierUserId);
                }
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(0, filter, groupIds);
                if (Request.QueryString["ReportDate"] != null)
                {
                    reportDate = Request.QueryString["ReportDate"].ToString();
                    if (!string.IsNullOrEmpty(reportDate))
                    {
                        response.StartDate = reportDate;
                        response.EndDate = reportDate;
                    }
                    response.ReportDate = reportDate;
                    response.CarrierCompanyId = carrierUserId;
                }
                return View("View", response);
            }
        }

        [HttpGet]
        public ActionResult DropTicketsView(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "DropTicketsView"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetDigitalDropTicketFilter(0, filter, groupIds);
                return View("View", response);
            }
        }

        [HttpGet]
        public ActionResult GetInvoiceTab(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "", int CarrierCompanyId = 0, string ReportDate = "")
        {
            using (var tracer = new Tracer("InvoiceController", "GetInvoiceTab"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(0, filter, groupIds);
                if (CarrierCompanyId > 0)
                {
                    if (!string.IsNullOrEmpty(ReportDate))
                    {
                        response.StartDate = ReportDate;
                        response.EndDate = ReportDate;
                    }
                    response.ReportDate = ReportDate;
                    response.CarrierCompanyId = CarrierCompanyId;
                }
                return PartialView("~/Areas/Supplier/Views/Shared/_PartialInvoiceTab.cshtml", response);
            }
        }

        [HttpGet]
        public ActionResult GetDropTicketTab(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "", int CarrierCompanyId = 0, string ReportDate = "")
        {
            using (var tracer = new Tracer("InvoiceController", "GetDropTicketTab"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetDigitalDropTicketFilter(0, filter, groupIds);
                if (CarrierCompanyId > 0)
                {
                    if (!string.IsNullOrEmpty(ReportDate))
                    {
                        response.StartDate = ReportDate;
                        response.EndDate = ReportDate;
                    }
                    response.ReportDate = ReportDate;
                    response.CarrierCompanyId = CarrierCompanyId;
                }
                return PartialView("~/Areas/Supplier/Views/Shared/_PartialDigitalDropTicketTab.cshtml", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InvoiceGrid(InvoiceDataTableViewModel requestModel, ViewInvoices view = ViewInvoices.All)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceGrid"))
            {

                var dashboardDomain = new DashboardDomain();
                var invoiceApiDomain = new InvoiceServiceApiDomain(dashboardDomain);
                requestModel.GroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);

                var response = await invoiceApiDomain.GetSupplierInvoiceGridAsync(CurrentUser.CompanyId, requestModel, (int)InvoiceType.Manual, view);
                if (requestModel.CarrierCompanyId > 0)
                {
                    response = invoiceApiDomain.FilterCarrierInvoiceRecords(response, requestModel.CarrierCompanyId);
                }
                var totalCount = 0;
                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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
        public async Task<ActionResult> DigitalDropTicketGrid(InvoiceDataTableViewModel requestModel, ViewInvoices view = ViewInvoices.All)
        {
            using (var tracer = new Tracer("InvoiceController", "DigitalDropTicketGrid"))
            {

                var dashboardDomain = new DashboardDomain();
                var invoiceServiceApiDomain = new InvoiceServiceApiDomain(dashboardDomain);
                requestModel.GroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);

                var response = await invoiceServiceApiDomain.GetSupplierInvoiceGridAsync(CurrentUser.CompanyId, requestModel, (int)InvoiceType.DigitalDropTicketManual, view);
                if (requestModel.CarrierCompanyId > 0)
                {
                    response = invoiceServiceApiDomain.FilterCarrierInvoiceRecords(response, requestModel.CarrierCompanyId);
                }
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<JsonResult> EditInvoicePoNumber(int invoiceId, string poNumber)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("InvoiceController", "EditInvoicePoNumber"))
            {
                response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().EditInvoicePoNumber(UserContext, invoiceId, poNumber);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DiscountLineItem(int invoiceId)
        {
            var response = new DiscountLineItemViewModel();
            response.InvoiceId = invoiceId;
            return PartialView("_PartialDiscountLineItem", response);
        }

        [HttpGet]
        public PartialViewResult DiscountTab(int invoiceId, int orderId, Currency currency)
        {
            using (var tracer = new Tracer("InvoiceController", "DiscountTab"))
            {
                var response = new DiscountViewModel();
                response.InvoiceId = invoiceId;
                response.OrderId = orderId;

                response.DiscountLineItems.Add(new DiscountLineItemViewModel { InvoiceId = invoiceId, Currency = currency });
                return PartialView("_PartialDiscounts", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<JsonResult> SaveDiscount(DiscountViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceController", "SaveDiscount"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.CreatedCompanyId = CurrentUser.CompanyId;
                    viewModel.CreatedBy = CurrentUser.Id;
                    var input = new SaveDiscountRequestModel { ViewModel = viewModel, UserContext = UserContext };
                    response = await new InvoiceServiceApiDomain().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlSaveDiscount, input);
                }
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public async Task<ActionResult> DiscountGrid(int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "DiscountGrid"))
            {
                var response = await new InvoiceServiceApiDomain().ApiGetCall<List<DiscountSummaryViewModel>>(string.Format(ApplicationConstants.UrlDiscountGrid, invoiceId));
                return new JsonResult
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }


        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DealAgree(int discountId, int invoiceId, int invoiceHeaderId)
        {
            using (var tracer = new Tracer("InvoiceController", "DealAgree"))
            {
                var invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                var response = await invoiceServiceApiDomain.ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlDealAgree, discountId, invoiceId, invoiceHeaderId), UserContext);

                if (response.StatusCode != Status.Success)
                {
                    response.StatusMessage = string.Format(response.StatusMessage, "Supplier", response.EntityId);
                }
                return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DealNotAgree(int discountId, int invoiceId)
        {
            using (var tracer = new Tracer("OrderController", "DealNotAgree"))
            {
                var response = await new InvoiceServiceApiDomain().ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlDeclineDeal, discountId, invoiceId), UserContext);
                response.EntityId = invoiceId;
                return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        public async Task<ActionResult> InvoiceHistoryGrid(int id = 0)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceHistoryGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().InvoiceHistoryGrid(id, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetSupplierInvoiceDetail(id, UserContext);
                if (response.StatusMessage == Resource.errMessageInvoiceDeleted)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "Invoice", new { area = "Supplier" });
                }
                else if (response.StatusMessage == Resource.errMessageDraftConvertedtoDDT)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("Messages", "Mailbox", new { area = "Messages" });
                }
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> DetailsTab(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetSupplierInvoiceDetail(id, UserContext);
                if (response.StatusMessage == Resource.errMessageInvoiceDeleted)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "Invoice", new { area = "Supplier" });
                }
                else if (response.StatusMessage == Resource.errMessageDraftConvertedtoDDT)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("Messages", "Mailbox", new { area = "Messages" });
                }
                return PartialView("_PartialInvoiceDetail", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PartialInvoicePdf(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "PartialInvoicePdf"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().PartialInvoicePdf(id, CompanyType.Supplier);
                return PartialView("_PartialInvoicePdf", response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PartialConsolidatedInvoicePdf(int invoiceHeaderId)
        {
            using (var tracer = new Tracer("InvoiceController", "PartialConsolidatedInvoicePdf"))
            {

                var input = new InvoicePdfRequestModel() { InvoiceHeaderId = invoiceHeaderId, CompanyType = CompanyType.Supplier, UserContext = UserContext };
                var response = await new InvoiceServiceApiDomain().ApiPostCall<ConsolidatedInvoicePdfViewModel>(ApplicationConstants.UrlGetConsolidatedInvoicePdf, input);
                if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.TankRental || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.TankRental)))
                {
                    return PartialView("_PartialTankRentalInvoicePdf", response);
                }
                else if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.Balance || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.Balance)))
                {
                    return PartialView("_PartialBalanceInvoicePdf", response);
                }
                else if (response.Invoices.Any() && response.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
                {
                    return PartialView("_PartialDryRunInvoicePdf", response);
                }
                else
                {
                    if (response.Invoices.Any(inv => inv.IsMarineLocation)) { return PartialView("_PartialConsolidatedMarineInvoicePdf", response); }
                    return PartialView("_PartialConsolidatedInvoicePdf", response);
                }
            }
        }



        //[HttpGet]
        //public async Task<ActionResult> DownloadBDRSummary(int id)
        //{
        //    var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetSupplierInvoiceDetailAsync(id, CurrentUser.CompanyId, UserContext);
        //    var partialPdfView = GetPartialViewAsPdf("~/Views/Shared/_PartialInvoiceDetailSummaryPDF.cshtml", response.Invoice.DisplayInvoiceNumber, response);
        //    return partialPdfView;
        //}

        public PartialViewResult EmailBDRDocumentPopup(int id, string number, CompanyType companyType, DocumentName docName)
        {
            var model = new EmailDocumentViewModel();
            if (docName == DocumentName.BDR)
            {
                model.InvoiceHeaderId = id;
                model.InvoiceNumber = number;
            }

            model.CompanyType = companyType;
            model.DocumentName = docName;

            return PartialView("_PartialBDREmailPopup", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> PayConfirm(InvoiceDetailViewModel viewModel)
        {
            using (var tracer = new Tracer("InvoiceController", "PayConfirm"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlGetBuyerInvoiceDetails, CurrentUser.Id), viewModel);

                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessagePayInvoiceConfirmedFailed);
                }
                return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = viewModel.Invoice.Id });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DryRun(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "DryRun"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetDryRunInvoice(id, CurrentUser.Id);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DryRunEdit(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "DryRunEdit"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetDryRunInvoiceForEdit(id, CurrentUser.Id);
                return View("DryRun", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DryRun(DryRunInvoiceViewModel viewModel)
        {
            using (var tracer = new Tracer("InvoiceController", "DryRun(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.UserId = CurrentUser.Id;
                    var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateDryRunInvoice, viewModel);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode != (int)Status.Success)
                    {
                        return RedirectToAction("DryRun", "Invoice", new { area = "Supplier", id = viewModel.OrderId });
                    }
                    else if (viewModel.InvoiceId > 0)
                    {
                        return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.OrderId });
                    }
                }

                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult AssignToOrder(int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "AssignToOrder"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetOrderforInvoice(invoiceId);
                if (response.OrderId == 0)
                    response = new AssignToOrderViewModel() { OrderId = 0, InvoiceId = invoiceId, InvoiceUoM = response.InvoiceUoM };
                else
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "Invoice", new { area = "Supplier" });
                }
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> AssignToOrderGrid()
        {
            using (var tracer = new Tracer("InvoiceController", "AssignToOrderGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().AssignToOrderGrid(CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> OrderPreView(int orderId, int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "OrderPreView"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().OrderPreview(orderId, invoiceId);
                return PartialView("_PartialAssignToOrderPreviewDetails", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> AssignInvoiceToOrder(int orderId, int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "AssignInvoiceToOrder"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().AssignInvoiceToOrder(orderId, invoiceId, CurrentUser.Id);
                if (response.StatusCode == Status.Success)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageAssignInvoieToOrderSuccess);
                    return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = response.InvoiceId });
                }
                else
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, string.IsNullOrWhiteSpace(response.StatusMessage) ? Resource.errMessageAssignInvoieToOrderFailed : response.StatusMessage);
                    return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = invoiceId });
                }
            }
        }


        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public async Task<ActionResult> GetTerminalDetails(int productId, int pricingCodeId, DateTimeOffset deliveryDate, int cityGroupTerminalId, int trackableScheduleId, Currency currency)
        {
            using (var tracer = new Tracer("InvoiceController", "GetTerminalDetails"))
            {
                var terminal = await ContextFactory.Current.GetDomain<DispatchDomain>().GetTerminalForTrackableSchedule(trackableScheduleId);
                if (terminal != null)
                {
                    var response = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetTerminalRackPriceAsync(terminal.TerminalId.Value, productId, pricingCodeId, deliveryDate, currency, cityGroupTerminalId);
                    return Json(new { Name = terminal.TerminalName, Price = response }, JsonRequestBehavior.DenyGet);
                }
                return Json(new { Name = string.Empty, Price = -1 }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetBulkplantAddress(int trackableScheduleId, int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetBulkplantAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceCommonDomain>().GetBulkplantAddress(trackableScheduleId, orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CreateSplitInvoice(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "CreateSplitInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceAsync(orderId);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> EditSplitDraftInvoice(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "EditSplitDraftInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceForEditAsync(id);
                return View("~/Areas/Supplier/Views/Invoice/CreateSplitInvoice.cshtml", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> AddNewSplitInvoice(int orderId, string splitLoadChainId)
        {
            using (var tracer = new Tracer("InvoiceController", "AddNewSplitInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualSplitInvoiceAsync(orderId, splitLoadChainId);
                return PartialView("_PartialSplitInvoiceDetails", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> EditSplitLoadDraftDdt(int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "EditSplitLoadDraftDdt"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualSplitInvoiceForEditAsync(invoiceId);
                return PartialView("_PartialSplitInvoiceDetails", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> GetSplitLoadInvoiceDetailsToEdit(int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "EditSplitLoadInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceForEditAsync(invoiceId);
                response.TaxType = TaxType.Manual;
                return PartialView("_PartialSplitInvoiceDetails", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> Create(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetManualInvoiceAsync(orderId);
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> Create(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase[] bolImageFiles, HttpPostedFileBase signImageFile, HttpPostedFileBase[] additionalImgFiles, HttpPostedFileBase[] taxAffidavitImgFiles = null, HttpPostedFileBase[] coastGuardInspectionImgFiles = null, HttpPostedFileBase[] inspectionRequestVoucherImgFiles = null, HttpPostedFileBase[] BDNImgFiles = null)
        {
            using (var tracer = new Tracer("InvoiceController", "Create(viewModel)"))
            {
                InvoiceServiceApiDomain invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                if (ModelState.IsValid)
                {
                    if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }

                    HttpPostedFileBase bolImageFile = null;
                    if (bolImageFiles != null && bolImageFiles.Length > 1)
                    {
                        if (viewModel.BolImage == null)
                            viewModel.BolImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(bolImageFiles, UserContext.Id, viewModel.BolImage);
                    }
                    else
                    {
                        bolImageFile = bolImageFiles != null ? bolImageFiles[0] : null;
                    }

                    HttpPostedFileBase additionalImgFile = null;
                    if (additionalImgFiles != null && additionalImgFiles.Length > 1)
                    {
                        if (viewModel.AdditionalImage == null)
                            viewModel.AdditionalImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(additionalImgFiles, UserContext.Id, viewModel.AdditionalImage);
                    }
                    else
                    {
                        additionalImgFile = additionalImgFiles != null ? additionalImgFiles[0] : null;
                    }

                    HttpPostedFileBase taxAffidavitImgFile = null;
                    if (taxAffidavitImgFiles != null && taxAffidavitImgFiles.Length > 1)
                    {
                        if (viewModel.TaxAffidavitImage == null)
                            viewModel.TaxAffidavitImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(taxAffidavitImgFiles, UserContext.Id, viewModel.TaxAffidavitImage);
                    }
                    else
                    {
                        taxAffidavitImgFile = taxAffidavitImgFiles != null ? taxAffidavitImgFiles[0] : null;
                    }

                    HttpPostedFileBase BDNImgFile = null;
                    if (BDNImgFiles != null && BDNImgFiles.Length > 1)
                    {
                        if (viewModel.BDNImage == null)
                            viewModel.BDNImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(BDNImgFiles, UserContext.Id, viewModel.BDNImage);
                    }
                    else
                    {
                        BDNImgFile = BDNImgFiles != null ? BDNImgFiles[0] : null;
                    }

                    HttpPostedFileBase coastGuardInspectionImgFile = null;
                    if (coastGuardInspectionImgFiles != null && coastGuardInspectionImgFiles.Length > 1)
                    {
                        if (viewModel.CoastGuardInspectionImage == null)
                            viewModel.CoastGuardInspectionImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(coastGuardInspectionImgFiles, UserContext.Id, viewModel.CoastGuardInspectionImage);
                    }
                    else
                    {
                        coastGuardInspectionImgFile = coastGuardInspectionImgFiles != null ? coastGuardInspectionImgFiles[0] : null;
                    }

                    HttpPostedFileBase inspectionRequestVoucherImgFile = null;
                    if (inspectionRequestVoucherImgFiles != null && inspectionRequestVoucherImgFiles.Length > 1)
                    {
                        if (viewModel.InspectionRequestVoucherImage == null)
                            viewModel.InspectionRequestVoucherImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(inspectionRequestVoucherImgFiles, UserContext.Id, viewModel.InspectionRequestVoucherImage);
                    }
                    else
                    {
                        inspectionRequestVoucherImgFile = inspectionRequestVoucherImgFiles != null ? inspectionRequestVoucherImgFiles[0] : null;
                    }
                    var mainOrderId = viewModel.OrderId;
                    viewModel.userId = CurrentUser.Id;
                    SetImageDataToViewModel(imageFile, bolImageFile, signImageFile, additionalImgFile, taxAffidavitImgFile, coastGuardInspectionImgFile, inspectionRequestVoucherImgFile, viewModel, BDNImgFile);

                    StatusViewModel response;
                    response = await UploadImagesToAzure(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        if (viewModel.IsFTL)
                        {
                            response = await invoiceServiceApiDomain.CreateManualFtlInvoice(UserContext, viewModel);
                        }
                        else
                        {
                            response = await invoiceServiceApiDomain.CreateManualInvoice(UserContext, viewModel);
                        }
                    }

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                    if (response.StatusCode == Status.Success)
                    {
                        MessageViewModel messageViewModel = new MessageViewModel();
                        //push notify to approval user
                        var notifyUsers = new List<int>();
                        notifyUsers.Add((int)FCMAppUserTypes.ApprovalUser);
                        messageViewModel.Title = Resource.notification_OrderDropped_Title;
                        messageViewModel.Body = string.Format(Resource.notificationToBuyer_OrderDropped_Body, viewModel.PoNumber);
                        await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(messageViewModel, viewModel.OrderId, 0, notifyUsers);

                        if (viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || viewModel.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp)
                            await SendResaleCustomerNotification(viewModel.InvoiceId);
                    }
                    return RedirectToAction("Details", "Order", new { area = "Supplier", id = mainOrderId });
                }
                viewModel = await invoiceServiceApiDomain.GetAssetsForInvoice(viewModel);
                return View(viewModel);
            }
        }

        private async Task<StatusViewModel> UploadImagesToAzure(ManualInvoiceViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Success);
            if (viewModel.InvoiceImage != null && viewModel.InvoiceImage.IsPdf)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.InvoiceImage.InputStream, viewModel.InvoiceImage.Name, BlobContainerType.InvoicePdfFiles);
                if (response.StatusCode == Status.Success)
                    viewModel.InvoiceImage.FilePath = response.StatusMessage;
            }
            if (viewModel.BolImage != null && viewModel.BolImage.IsPdf)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.BolImage.InputStream, viewModel.BolImage.Name, BlobContainerType.InvoicePdfFiles);
                if (response.StatusCode == Status.Success)
                    viewModel.BolImage.FilePath = response.StatusMessage;
            }
            if (viewModel.AdditionalImage != null && viewModel.AdditionalImage.IsPdf)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.AdditionalImage.InputStream, viewModel.AdditionalImage.Name, BlobContainerType.InvoicePdfFiles);
                if (response.StatusCode == Status.Success)
                    viewModel.AdditionalImage.FilePath = response.StatusMessage;
            }
            if (viewModel.TaxAffidavitImage != null && viewModel.TaxAffidavitImage.IsPdf)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.TaxAffidavitImage.InputStream, viewModel.TaxAffidavitImage.Name, BlobContainerType.InvoicePdfFiles);
                if (response.StatusCode == Status.Success)
                    viewModel.TaxAffidavitImage.FilePath = response.StatusMessage;
            }
            if (viewModel.CoastGuardInspectionImage != null && viewModel.CoastGuardInspectionImage.IsPdf)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.CoastGuardInspectionImage.InputStream, viewModel.CoastGuardInspectionImage.Name, BlobContainerType.InvoicePdfFiles);
                if (response.StatusCode == Status.Success)
                    viewModel.CoastGuardInspectionImage.FilePath = response.StatusMessage;
            }
            if (viewModel.InspectionRequestVoucherImage != null && viewModel.InspectionRequestVoucherImage.IsPdf)
            {
                response = await AzureStorageService.UploadImageToBlob(UserContext, viewModel.InspectionRequestVoucherImage.InputStream, viewModel.InspectionRequestVoucherImage.Name, BlobContainerType.InvoicePdfFiles);
                if (response.StatusCode == Status.Success)
                    viewModel.InspectionRequestVoucherImage.FilePath = response.StatusMessage;
            }
            return response;
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> Edit(int id, bool isConvertFromDDT = false)
        {
            using (var tracer = new Tracer("InvoiceController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetManualInvoiceForEdit(id);
                response.IsConvertFromDDT = isConvertFromDDT;
                if (response.ConvertionFactor.HasValue && response.ConvertionFactor.Value > 0)
                {
                    response.IsConversionFactor = true;
                    response.FuelDropped = response.ConvertedQuantity;
                }
                response.TaxType = TaxType.Manual;

                if (isConvertFromDDT)
                {
                    response.InvoiceTypeId = (int)InvoiceType.Manual;
                }
                var invoiceCommonDomain = new InvoiceCommonDomain();
                response.IsBadgeMandatory = invoiceCommonDomain.IsBadgeNumberMandatory(response.OrderId, UserContext.CompanyId);
                if (response.BolDetails != null && response.BolDetails.NetQuantity > 0)
                {
                    response.IsBolRequired = true;
                }
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> Edit(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase[] bolImageFiles, HttpPostedFileBase signImageFile, HttpPostedFileBase[] additionalImgFiles = null, HttpPostedFileBase[] taxAffidavitImgFiles = null, HttpPostedFileBase[] coastGuardInspectionImgFiles = null, HttpPostedFileBase[] inspectionRequestVoucherImgFiles = null, HttpPostedFileBase[] BDNImgFiles = null)
        {
            using (var tracer = new Tracer("InvoiceController", "Edit(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                    if (viewModel.FuelRequestFee.DeliveryFeeSubTypeId == (int)FeeSubType.ByQuantity && viewModel.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageFeeByQuantityRequired);
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }
                    viewModel.userId = CurrentUser.Id;
                    HttpPostedFileBase[] imageFiles = { imageFile };
                    viewModel.InvoiceImage = await SetImageDataToBolb(viewModel.InvoiceImage, imageFiles, BlobContainerType.InvoicePdfFiles);
                    HttpPostedFileBase[] signimageFiles = { signImageFile };
                    viewModel.SignatureImage = await SetImageDataToBolb(viewModel.SignatureImage, signimageFiles, BlobContainerType.InvoicePdfFiles);
                    viewModel.BolImage = await SetImageDataToBolb(viewModel.BolImage, bolImageFiles, BlobContainerType.InvoicePdfFiles);
                    viewModel.AdditionalImage = await SetImageDataToBolb(viewModel.AdditionalImage, additionalImgFiles, BlobContainerType.InvoicePdfFiles);
                    viewModel.TaxAffidavitImage = await SetImageDataToBolb(viewModel.TaxAffidavitImage, taxAffidavitImgFiles, BlobContainerType.InvoicePdfFiles, BlobContainerType.BDN);
                    viewModel.BDNImage = await SetImageDataToBolb(viewModel.BDNImage, BDNImgFiles, BlobContainerType.InvoicePdfFiles, BlobContainerType.BDN);
                    viewModel.CoastGuardInspectionImage = await SetImageDataToBolb(viewModel.CoastGuardInspectionImage, coastGuardInspectionImgFiles, BlobContainerType.InvoicePdfFiles, BlobContainerType.BDN);
                    viewModel.InspectionRequestVoucherImage = await SetImageDataToBolb(viewModel.InspectionRequestVoucherImage, inspectionRequestVoucherImgFiles, BlobContainerType.InvoicePdfFiles, BlobContainerType.BDN);
                    StatusViewModel response = new StatusViewModel();
                    if (viewModel.StatusId == (int)InvoiceStatus.Draft)
                    {
                        response = await invoiceServiceApiDomain.EditDraftDDTAsync(UserContext, viewModel);
                    }
                    else if (viewModel.TypeofFuel == (int)ProductDisplayGroups.OtherFuelType && viewModel.IsConvertFromDDT)
                    {
                        response = await invoiceServiceApiDomain.CreateInvoiceFromDropTicketForNonStandardFuel(UserContext, viewModel);
                    }
                    else if (viewModel.IsConvertFromDDT && viewModel.IsFTL && viewModel.WaitingForAction == (int)WaitingAction.BolDetails)
                    {
                        bool isValidInput = true;
                        response = new StatusViewModel();
                        if (viewModel.PickUpAddress != null && viewModel.PickUpAddress.IsAddressAvailable)
                        {
                            if (!viewModel.BolDetails.IsLiftInfoAvailable())
                            {
                                response.StatusMessage = ResourceMessages.GetMessage(Resource.valMessageLiftTicketInfoRequired);
                                isValidInput = false;
                            }
                            else if ((viewModel.IsBadgeMandatory ? !string.IsNullOrEmpty(viewModel.BolDetails.BadgeNumber) : true))
                            {
                                response.StatusMessage = ResourceMessages.GetMessage(Resource.errBadgeNumberMandatory);
                                isValidInput = false;
                            }
                        }
                        else if (!viewModel.BolDetails.IsBolAvailable())
                        {
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.valMessageBolDetailsRequired);
                            isValidInput = false;
                        }
                        else if ((viewModel.IsBadgeMandatory ? !string.IsNullOrEmpty(viewModel.BolDetails.BadgeNumber) : true))
                        {
                            response.StatusMessage = ResourceMessages.GetMessage(Resource.errBadgeNumberMandatory);
                            isValidInput = false;
                        }
                        if (isValidInput)
                        {
                            response = await invoiceServiceApiDomain.CreateInvoiceFromDropTicketWithBol(viewModel, UserContext);
                            if (response.StatusCode == Status.Failed)
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                return RedirectToAction("Edit", "Invoice", new { area = "Supplier", id = viewModel.ConversionDDTId, isConvertFromDDT = viewModel.IsConvertFromDDT });
                            }
                        }
                        else
                        {
                            response.StatusCode = Status.Failed;
                        }
                    }
                    else if (viewModel.IsConvertFromDDT && viewModel.WaitingForAction == (int)WaitingAction.Images)
                    {
                        var invoiceDomain = new InvoiceDomain(invoiceServiceApiDomain);
                        if (!invoiceDomain.HasRequiredInvoiceImages(viewModel))
                        {
                            response = await invoiceServiceApiDomain.InvoiceEdit(UserContext, viewModel);
                        }
                        else
                        {
                            response = await invoiceServiceApiDomain.CreateInvoiceFromDropTicketWithBol(viewModel, UserContext);
                            if (response.StatusCode == Status.Failed)
                            {
                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                return RedirectToAction("Edit", "Invoice", new { area = "Supplier", id = viewModel.ConversionDDTId, isConvertFromDDT = viewModel.IsConvertFromDDT });
                            }
                        }
                    }
                    else
                    {
                        if (viewModel.IsMarineLocation && viewModel.UoM == UoM.MetricTons && viewModel.Gravity.HasValue && viewModel.Gravity.Value > 0)
                        {

                            var GravityFactor = await ContextFactory.Current.GetDomain<OrderDomain>().GetGallonsPerMetricTonAsync(Math.Round(viewModel.Gravity.Value, 1));
                            if (GravityFactor.Result == 0)
                            {
                                viewModel.StatusCode = Status.Failed;
                                viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMsgInvalidGravity);
                                DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                                // return View(viewModel);
                                return RedirectToAction("Edit", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId, isConvertFromDDT = viewModel.IsConvertFromDDT });
                            }
                            else
                            {
                                response = await invoiceServiceApiDomain.InvoiceEdit(UserContext, viewModel);
                            }
                        }
                        else
                        {
                            response = await invoiceServiceApiDomain.InvoiceEdit(UserContext, viewModel);
                        }
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        if (viewModel.DeliveryLevelTrackableScheduleId > 0)
                        {
                            response = await invoiceServiceApiDomain.UpdateDeliveryLevelPO(viewModel);
                        }
                    }
                    if (response.StatusCode == Status.Failed)
                    {
                        return RedirectToAction("Edit", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId, isConvertFromDDT = viewModel.IsConvertFromDDT });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = response.EntityId });
                    }
                }
                if (!ModelState.IsValid)
                {
                    var response = new StatusViewModel();
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("Edit", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId, isConvertFromDDT = viewModel.IsConvertFromDDT });
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CreditAndRebillSplitInvoice(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "CreditAndRebill"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetManualInvoiceForEdit(id);
                response.IsConvertFromDDT = false;
                response.TaxType = TaxType.Standard;
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreditAndRebill(int id, int orderId, int? invoiceType = null)
        {
            using (var tracer = new Tracer("InvoiceController", "CreditAndRebill"))
            {
                var response = new ManualInvoiceViewModel();
                if (invoiceType == (int)InvoiceType.TankRental)
                {
                    response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetManualInvoiceForEdit(id);
                    response.IsConvertFromDDT = false;
                    response.TaxType = TaxType.Standard;

                    response.FuelDeliveryDetails.FuelFees.FuelRequestFees = response.FuelDeliveryDetails.FuelFees.FuelRequestFees.Where(t => t.FeeTypeId != ((int)FeeType.TankRental).ToString()).ToList();
                    SetBalanceInvoiceDetails(response);
                    return View("CreateTankRentalInvoice", response);
                }
                else if (invoiceType == (int)InvoiceType.Balance)
                {
                    response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetManualInvoiceForEdit(id);
                    response.IsConvertFromDDT = false;
                    response.TaxType = TaxType.Standard;

                    SetBalanceInvoiceDetails(response);
                    return View("~/Areas/Supplier/Views/Invoice/CreateBalanceInvoice.cshtml", response);
                }
                else
                {
                    ViewBag.Title = "Credit and Rebill Invoice";
                    return View("CreateNew");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreditAndRebill(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase[] bolImageFiles, HttpPostedFileBase signImageFile, HttpPostedFileBase[] additionalImgFiles = null, HttpPostedFileBase[] taxAffidavitImgFiles = null, HttpPostedFileBase[] coastGuardInspectionImgFiles = null, HttpPostedFileBase[] inspectionRequestVoucherImgFiles = null, HttpPostedFileBase[] BDNImgFiles = null)
        {
            using (var tracer = new Tracer("InvoiceController", "CreditAndRebill(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.FuelRequestFee.DeliveryFeeSubTypeId == (int)FeeSubType.ByQuantity && viewModel.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageFeeByQuantityRequired);
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }
                    viewModel.userId = CurrentUser.Id;

                    HttpPostedFileBase bolImageFile = null;
                    if (bolImageFiles != null && bolImageFiles.Length > 1)
                    {
                        if (viewModel.BolImage == null)
                            viewModel.BolImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(bolImageFiles, UserContext.Id, viewModel.BolImage);
                    }
                    else
                    {
                        bolImageFile = bolImageFiles != null ? bolImageFiles[0] : null;
                    }

                    HttpPostedFileBase additionalImgFile = null;
                    if (additionalImgFiles != null && additionalImgFiles.Length > 1)
                    {
                        if (viewModel.AdditionalImage == null)
                            viewModel.AdditionalImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(additionalImgFiles, UserContext.Id, viewModel.AdditionalImage);
                    }
                    else
                    {
                        additionalImgFile = additionalImgFiles != null ? additionalImgFiles[0] : null;
                    }
                    HttpPostedFileBase taxAffidavitImgFile = null;
                    if (taxAffidavitImgFiles != null && taxAffidavitImgFiles.Length > 1)
                    {
                        if (viewModel.TaxAffidavitImage == null)
                            viewModel.TaxAffidavitImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(taxAffidavitImgFiles, UserContext.Id, viewModel.TaxAffidavitImage);
                    }
                    else
                    {
                        taxAffidavitImgFile = taxAffidavitImgFiles != null ? taxAffidavitImgFiles[0] : null;
                    }

                    HttpPostedFileBase BDNmgFile = null;
                    if (BDNImgFiles != null && BDNImgFiles.Length > 1)
                    {
                        if (viewModel.BDNImage == null)
                            viewModel.BDNImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(BDNImgFiles, UserContext.Id, viewModel.BDNImage);
                    }
                    else
                    {
                        BDNmgFile = BDNImgFiles != null ? BDNImgFiles[0] : null;
                    }

                    HttpPostedFileBase coastGuardInspectionImgFile = null;
                    if (coastGuardInspectionImgFiles != null && coastGuardInspectionImgFiles.Length > 1)
                    {
                        if (viewModel.CoastGuardInspectionImage == null)
                            viewModel.CoastGuardInspectionImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(coastGuardInspectionImgFiles, UserContext.Id, viewModel.CoastGuardInspectionImage);
                    }
                    else
                    {
                        coastGuardInspectionImgFile = coastGuardInspectionImgFiles != null ? coastGuardInspectionImgFiles[0] : null;
                    }

                    HttpPostedFileBase inspectionRequestVoucherImgFile = null;
                    if (inspectionRequestVoucherImgFiles != null && inspectionRequestVoucherImgFiles.Length > 1)
                    {
                        if (viewModel.InspectionRequestVoucherImage == null)
                            viewModel.InspectionRequestVoucherImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(inspectionRequestVoucherImgFiles, UserContext.Id, viewModel.InspectionRequestVoucherImage);
                    }
                    else
                    {
                        inspectionRequestVoucherImgFile = inspectionRequestVoucherImgFiles != null ? inspectionRequestVoucherImgFiles[0] : null;
                    }
                    SetImageDataToViewModel(imageFile, bolImageFile, signImageFile, additionalImgFile, taxAffidavitImgFile, coastGuardInspectionImgFile, inspectionRequestVoucherImgFile, viewModel, BDNmgFile);

                    StatusViewModel response;
                    response = await UploadImagesToAzure(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        var input = new CreateInvoiceViewModel() { UserContext = UserContext, ViewModel = viewModel };
                        response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlRebillInvoiceAsync, input);
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed && response.StatusMessage != Resource.errMessageAlreadyCreditedRebilled)
                    {
                        return RedirectToAction("CreditAndRebill", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId, isConvertFromDDT = viewModel.IsConvertFromDDT });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId });
                    }
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CreateSplitLoadDraftDDT(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase[] bolImageFiles, HttpPostedFileBase signImageFile, HttpPostedFileBase[] additionalImgFiles = null, HttpPostedFileBase[] taxAffidavitImgFiles = null, HttpPostedFileBase[] coastGuardInspectionImgFiles = null, HttpPostedFileBase[] inspectionRequestVoucherImgFiles = null, HttpPostedFileBase[] BDNImgFiles = null)
        {
            var response = new InvoiceCreateResponseViewModel();
            using (var tracer = new Tracer("InvoiceController", "CreateSplitLoadDraftDDT"))
            {
                InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain();
                if (ModelState.IsValid)
                {
                    if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }

                    viewModel.userId = CurrentUser.Id;

                    HttpPostedFileBase bolImageFile = null;
                    if (bolImageFiles != null && bolImageFiles.Length > 1)
                    {
                        if (viewModel.BolImage == null)
                            viewModel.BolImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(bolImageFiles, UserContext.Id, viewModel.BolImage);
                    }
                    else
                    {
                        bolImageFile = bolImageFiles != null ? bolImageFiles[0] : null;
                    }

                    HttpPostedFileBase additionalImgFile = null;
                    if (additionalImgFiles != null && additionalImgFiles.Length > 1)
                    {
                        if (viewModel.AdditionalImage == null)
                            viewModel.AdditionalImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(additionalImgFiles, UserContext.Id, viewModel.AdditionalImage);
                    }
                    else
                    {
                        additionalImgFile = additionalImgFiles != null ? additionalImgFiles[0] : null;
                    }
                    HttpPostedFileBase taxAffidavitImgFile = null;
                    if (taxAffidavitImgFiles != null && taxAffidavitImgFiles.Length > 1)
                    {
                        if (viewModel.TaxAffidavitImage == null)
                            viewModel.TaxAffidavitImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(taxAffidavitImgFiles, UserContext.Id, viewModel.TaxAffidavitImage);
                    }
                    else
                    {
                        taxAffidavitImgFile = taxAffidavitImgFiles != null ? taxAffidavitImgFiles[0] : null;
                    }

                    HttpPostedFileBase BDNImgFile = null;
                    if (BDNImgFiles != null && BDNImgFiles.Length > 1)
                    {
                        if (viewModel.BDNImage == null)
                            viewModel.BDNImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(BDNImgFiles, UserContext.Id, viewModel.BDNImage);
                    }
                    else
                    {
                        BDNImgFile = BDNImgFiles != null ? BDNImgFiles[0] : null;
                    }



                    HttpPostedFileBase coastGuardInspectionImgFile = null;
                    if (coastGuardInspectionImgFiles != null && coastGuardInspectionImgFiles.Length > 1)
                    {
                        if (viewModel.CoastGuardInspectionImage == null)
                            viewModel.CoastGuardInspectionImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(coastGuardInspectionImgFiles, UserContext.Id, viewModel.CoastGuardInspectionImage);
                    }
                    else
                    {
                        coastGuardInspectionImgFile = coastGuardInspectionImgFiles != null ? coastGuardInspectionImgFiles[0] : null;
                    }

                    HttpPostedFileBase inspectionRequestVoucherImgFile = null;
                    if (inspectionRequestVoucherImgFiles != null && inspectionRequestVoucherImgFiles.Length > 1)
                    {
                        if (viewModel.InspectionRequestVoucherImage == null)
                            viewModel.InspectionRequestVoucherImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(inspectionRequestVoucherImgFiles, UserContext.Id, viewModel.InspectionRequestVoucherImage);
                    }
                    else
                    {
                        inspectionRequestVoucherImgFile = inspectionRequestVoucherImgFiles != null ? inspectionRequestVoucherImgFiles[0] : null;
                    }
                    SetImageDataToViewModel(imageFile, bolImageFile, signImageFile, additionalImgFile, taxAffidavitImgFile, coastGuardInspectionImgFile, inspectionRequestVoucherImgFile, viewModel, BDNImgFile);
                    StatusViewModel fileResponse;
                    fileResponse = await UploadImagesToAzure(viewModel);

                    if (fileResponse.StatusCode == Status.Success)
                    {
                        // Set status as Draft DDT
                        viewModel.StatusId = (int)InvoiceStatus.Draft;
                        viewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                        if (viewModel.InvoiceId > 0)
                        {
                            var splitLoadInvoiceDomain = new SplitLoadInvoiceDomain();
                            var editStatus = await splitLoadInvoiceDomain.EditSplitDraftDDTAsync(UserContext, viewModel);
                            return Json(editStatus, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            response = await invoiceCreateDomain.CreateSplitLoadDraftDdtAsync(UserContext, viewModel);
                            response.DisplayMode = PageDisplayMode.Create;
                        }
                    }
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CancelDraft(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "CancelDraft"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().CancelDraftAsync(id, UserContext);

                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageCancelDraftFailed);
                }
                return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreateInvoiceFromDropTicket(InvoiceDetailViewModel viewModel)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceController", "CreateInvoiceFromDropTicket"))
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(viewModel.SplitLoadChainId))
                    {
                        response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ConvertDdtToInvoiceManually(UserContext, viewModel.Invoice.Id, true);
                    }
                    else
                    {
                        response = await ContextFactory.Current.GetDomain<InvoiceDomain>().CreateInvoiceFromSplitLoadDropTicket(UserContext, viewModel);
                    }
                }
                response.EntityId = viewModel.Invoice.Id;
                return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public async Task<JsonResult> GetExistingSplitLoadAddresses(int orderId, int? trackableScheduleId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderDomain>().GetSplitDropAddressesAsync(orderId, trackableScheduleId, null);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DownloadPdf(int id)
        {
            var input = new InvoicePdfRequestModel() { InvoiceHeaderId = id, CompanyType = CompanyType.Supplier };
            var invoicePdfModel = await new InvoiceServiceApiDomain().ApiPostCall<ConsolidatedInvoicePdfViewModel>(ApplicationConstants.UrlGetConsolidatedInvoicePdf, input);

            invoicePdfModel.InvoicePdfHeaderDetail.SupplierPhoneNumber = invoicePdfModel.InvoicePdfHeaderDetail.SupplierPhoneNumber.ToFormattedPhoneNumber();
            invoicePdfModel.InvoicePdfHeaderDetail.PoContact.PhoneNumber = invoicePdfModel.InvoicePdfHeaderDetail.PoContact.PhoneNumber.ToFormattedPhoneNumber();

            string partialViewName = "_PartialConsolidatedInvoicePdf";
            if (invoicePdfModel.Invoices.Any() && invoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.TankRental || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.TankRental)))
            {
                partialViewName = "_PartialTankRentalInvoicePdf";
            }
            else if (invoicePdfModel.Invoices.Any() && invoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.Balance || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.Balance)))
            {
                partialViewName = "_PartialBalanceInvoicePdf";
            }
            else if (invoicePdfModel.Invoices.Any() && invoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
            {
                partialViewName = "_PartialDryRunInvoicePdf";
            }
            else if (invoicePdfModel.Invoices.Any() && invoicePdfModel.Invoices.Any(t => t.IsMarineLocation))
            {
                partialViewName = "_PartialConsolidatedMarineInvoicePdf";
            }

            return GetPartialViewAsPdf(partialViewName, invoicePdfModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber, invoicePdfModel);

        }

        //[HttpGet]
        //public async Task<FileResult> DownloadDtnFile(int invoiceHeaderId, string refId, string password, string siteNumber, string invoiceNumber)
        //{
        //    var input = new GenerateDtnRequestModel() { InvoiceHeaderId = invoiceHeaderId, Password = password, RefId = refId, SiteNumber = siteNumber };
        //    var fileData = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<string>(ApplicationConstants.UrlGenerateDtnFile, input);
        //    return File(new UTF8Encoding().GetBytes(fileData.ToString()), "text/csv", invoiceNumber + ".csv");
        //}

        [HttpGet]
        public async Task<FileResult> DownloadDtnFile(int invoiceHeaderId, string refId, string password, string siteNumber, string invoiceNumber)
        {
            var fileData = await ContextFactory.Current.GetDomain<InvoiceReadDomain>().GenerateDtnFileAsync(invoiceHeaderId, refId, password, siteNumber);
            return File(new UTF8Encoding().GetBytes(fileData.ToString()), "text/csv", invoiceNumber + ".csv");
        }

        public async Task<JsonResult> UploadDtnFileToFtp(int invoiceHeaderId, int buyerCompanyId, string invoiceNumber)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceReadDomain>().UploadDtnFileToFtp(invoiceHeaderId, invoiceNumber, CurrentUser.CompanyId, buyerCompanyId, UserContext.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public async Task<JsonResult> UploadDtnFileToFtp(int invoiceHeaderId, int buyerCompanyId, string invoiceNumber)
        //{
        //    var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<StatusViewModel>(string.Format(ApplicationConstants.UrlUploadDtnFileToFtp, invoiceHeaderId, invoiceNumber, CurrentUser.CompanyId, buyerCompanyId, UserContext.Id));
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult AddNewAsset(int orderId, UoM uoM = UoM.Gallons)
        {
            return PartialView("_PartialAssetDrop", new AssetDropViewModel() { OrderId = orderId, UoM = uoM });
        }

        public ActionResult AddExistingAsset(AssetDropViewModel viewModel)
        {
            return PartialView("_PartialAssetDrop", viewModel);
        }

        [HttpGet]
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
            return PartialView("_PartialInvoiceAdditionalFee", new AdditionalFeeViewModel());
        }

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult Tax()
        {
            return PartialView("_PartialTax", new TaxViewModel());
        }

        [HttpGet]
        public async Task<ActionResult> GetDriverByDeliverySchedule(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "GetDriverByDeliverySchedule"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDriverByDeliveryScheduleAsync(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0, EntityType entityTypeId = EntityType.Invoice)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetNewsfeed(UserContext, entityId, currentPage, latestId, entityTypeId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public FileContentResult DownloadCsv(string fileName)
        {
            var azureBlob = new AzureBlobStorage();
            var fileStream = azureBlob.DownloadBlob(fileName, BlobContainerType.MansfieldAMPInvoiceCsv.ToString().ToLower());
            var memoryStream = fileStream as MemoryStream;
            string mimeType = "text/csv";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(memoryStream.ToArray(), mimeType);
        }

        public async Task<decimal> GetDryRunAmount(int orderId, DateTimeOffset dateTime)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetDryRunFee(orderId, dateTime);
            return response;
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.SuperAdmin, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> EditInvoiceNumber(int id, string displayInvoiceNumber)
        {
            using (var tracer = new Tracer("InvoiceController", "EditInvoiceNumber"))
            {
                var invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                var response = await invoiceServiceApiDomain.EditInvoiceNumber(UserContext, id, displayInvoiceNumber);
                response.StatusMessage = string.Format(response.StatusMessage, "Supplier", response.EntityId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> DraftDDT(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase bolImageFile)
        {
            using (var tracer = new Tracer("InvoiceController", "DraftDDT(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    InvoiceServiceApiDomain invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                    if (viewModel.FuelRequestFee.DeliveryFeeSubTypeId == (int)FeeSubType.ByQuantity && viewModel.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageFeeByQuantityRequired);
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }
                    viewModel.userId = CurrentUser.Id;
                    if (imageFile != null)
                    {
                        var reader = new BinaryReader(imageFile.InputStream);
                        byte[] imageData = reader.ReadBytes((int)imageFile.InputStream.Length);
                        viewModel.InvoiceImage = new ImageViewModel() { Data = imageData };
                    }
                    if (bolImageFile != null)
                    {
                        var reader = new BinaryReader(bolImageFile.InputStream);
                        byte[] imageData = reader.ReadBytes((int)bolImageFile.InputStream.Length);
                        viewModel.BolImage = new ImageViewModel() { Data = imageData };
                    }

                    StatusViewModel response = new StatusViewModel();
                    if (viewModel.StatusId == (int)InvoiceStatus.Draft) // submit draft ddt and create ddt in received state
                    {
                        // update invoice status to Received
                        viewModel.StatusId = (int)InvoiceStatus.Received;
                        response = await invoiceServiceApiDomain.CreateInvoiceFromDraftDdt(UserContext, viewModel);
                    }

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Failed)
                    {
                        return RedirectToAction("Edit", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId });
                    }
                    else
                    {
                        return RedirectToAction("View", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId });
                    }
                }
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CreateSplitLoadInvoice(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase[] bolImageFiles, HttpPostedFileBase signImageFile, bool isSave = true, HttpPostedFileBase[] additionalImgFiles = null, HttpPostedFileBase[] taxAffidavitImgFiles = null, HttpPostedFileBase[] coastGuardInspectionImgFiles = null, HttpPostedFileBase[] inspectionRequestVoucherImgFiles = null, HttpPostedFileBase[] BDNImgFiles = null)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceController", "CreateSplitLoadInvoice"))
            {
                InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain();
                if (ModelState.IsValid)
                {
                    if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }

                    viewModel.userId = CurrentUser.Id;
                    HttpPostedFileBase bolImageFile = null;
                    if (bolImageFiles != null && bolImageFiles.Length > 1)
                    {
                        if (viewModel.BolImage == null)
                            viewModel.BolImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(bolImageFiles, UserContext.Id, viewModel.BolImage);
                    }
                    else
                    {
                        bolImageFile = bolImageFiles != null ? bolImageFiles[0] : null;
                    }

                    HttpPostedFileBase additionalImgFile = null;
                    if (additionalImgFiles != null && additionalImgFiles.Length > 1)
                    {
                        if (viewModel.AdditionalImage == null)
                            viewModel.AdditionalImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(additionalImgFiles, UserContext.Id, viewModel.AdditionalImage);
                    }
                    else
                    {
                        additionalImgFile = additionalImgFiles != null ? additionalImgFiles[0] : null;
                    }
                    HttpPostedFileBase taxAffidavitImgFile = null;
                    if (taxAffidavitImgFiles != null && taxAffidavitImgFiles.Length > 1)
                    {
                        if (viewModel.TaxAffidavitImage == null)
                            viewModel.TaxAffidavitImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(taxAffidavitImgFiles, UserContext.Id, viewModel.TaxAffidavitImage);
                    }
                    else
                    {
                        taxAffidavitImgFile = taxAffidavitImgFiles != null ? taxAffidavitImgFiles[0] : null;
                    }

                    HttpPostedFileBase BDNImgFile = null;
                    if (BDNImgFiles != null && BDNImgFiles.Length > 1)
                    {
                        if (viewModel.BDNImage == null)
                            viewModel.BDNImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(BDNImgFiles, UserContext.Id, viewModel.BDNImage);
                    }
                    else
                    {
                        BDNImgFile = BDNImgFiles != null ? BDNImgFiles[0] : null;
                    }

                    HttpPostedFileBase coastGuardInspectionImgFile = null;
                    if (coastGuardInspectionImgFiles != null && coastGuardInspectionImgFiles.Length > 1)
                    {
                        if (viewModel.CoastGuardInspectionImage == null)
                            viewModel.CoastGuardInspectionImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(coastGuardInspectionImgFiles, UserContext.Id, viewModel.CoastGuardInspectionImage);
                    }
                    else
                    {
                        coastGuardInspectionImgFile = coastGuardInspectionImgFiles != null ? coastGuardInspectionImgFiles[0] : null;
                    }

                    HttpPostedFileBase inspectionRequestVoucherImgFile = null;
                    if (inspectionRequestVoucherImgFiles != null && inspectionRequestVoucherImgFiles.Length > 1)
                    {
                        if (viewModel.InspectionRequestVoucherImage == null)
                            viewModel.InspectionRequestVoucherImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(inspectionRequestVoucherImgFiles, UserContext.Id, viewModel.InspectionRequestVoucherImage);
                    }
                    else
                    {
                        inspectionRequestVoucherImgFile = inspectionRequestVoucherImgFiles != null ? inspectionRequestVoucherImgFiles[0] : null;
                    }
                    SetImageDataToViewModel(imageFile, bolImageFile, signImageFile, additionalImgFile, taxAffidavitImgFile, coastGuardInspectionImgFile, inspectionRequestVoucherImgFile, viewModel, BDNImgFile);

                    response = await UploadImagesToAzure(viewModel);
                    if (response.StatusCode == Status.Success)
                    {
                        // Set status as Draft DDT
                        viewModel.StatusId = (int)InvoiceStatus.Draft;
                        viewModel.InvoiceTypeId = (int)InvoiceType.DigitalDropTicketManual;
                        string splitLoadChainId = viewModel.SplitLoadChainId;
                        if (isSave)
                        {
                            if (viewModel.InvoiceId > 0)
                            {
                                var editStatus = await ContextFactory.Current.GetDomain<SplitLoadInvoiceDomain>().EditSplitDraftDDTAsync(UserContext, viewModel);
                                if (editStatus.StatusCode == Status.Failed)
                                {
                                    return Json(editStatus, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                var draftDdtSaveOutPut = await invoiceCreateDomain.CreateSplitLoadDraftDdtAsync(UserContext, viewModel);
                                if (draftDdtSaveOutPut.StatusCode == Status.Failed)
                                {
                                    return Json(draftDdtSaveOutPut, JsonRequestBehavior.AllowGet);
                                }
                                splitLoadChainId = draftDdtSaveOutPut.SplitLoadChainId;
                            }
                        }
                        response = await ContextFactory.Current.GetDomain<SplitLoadInvoiceDomain>().CreateInvoicesFromSplitLoadDraftDdts(UserContext, splitLoadChainId);
                    }
                    if (response.StatusCode == Status.Success)
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    }
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CancelAllSplitLoadDraftDdts(string splitLoadChainId)
        {
            using (var tracer = new Tracer("InvoiceController", "CancelAllSplitLoadDraftDdts"))
            {
                var response = await ContextFactory.Current.GetDomain<SplitLoadInvoiceDomain>().CancelAllSplitLoadDraftDdtsAsync(splitLoadChainId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> CancelSplitLoadDraftDdt(int invoiceId, string splitLoadChainId)
        {
            using (var tracer = new Tracer("InvoiceController", "CancelSplitLoadDraftDdt"))
            {
                var response = await ContextFactory.Current.GetDomain<SplitLoadInvoiceDomain>().CancelSplitLoadDraftDdtAsync(invoiceId, splitLoadChainId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> EditSplitInvoice(int id, bool isConvertFromDDT = false)
        {
            using (var tracer = new Tracer("InvoiceController", "EditSplitInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceForEditAsync(id);
                response.IsConvertFromDDT = isConvertFromDDT;
                response.TaxType = TaxType.Manual;
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> EditSplitLoadInvoice(ManualInvoiceViewModel viewModel, HttpPostedFileBase imageFile, HttpPostedFileBase[] bolImageFiles, HttpPostedFileBase signImageFile, HttpPostedFileBase[] additionalImgFiles = null, HttpPostedFileBase[] taxAffidavitImgFiles = null, HttpPostedFileBase[] coastGuardInspectionImgFiles = null, HttpPostedFileBase[] inspectionRequestVoucherImgFiles = null, HttpPostedFileBase[] BDNImgFiles = null)
        {
            using (var tracer = new Tracer("InvoiceController", "Edit(viewModel)"))
            {
                InvoiceEditResponseViewModel response = new InvoiceEditResponseViewModel();
                if (ModelState.IsValid)
                {
                    if (viewModel.FuelRequestFee.DeliveryFeeSubTypeId == (int)FeeSubType.ByQuantity && viewModel.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageFeeByQuantityRequired);
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }
                    viewModel.userId = CurrentUser.Id;

                    HttpPostedFileBase bolImageFile = null;
                    if (bolImageFiles != null && bolImageFiles.Length > 1)
                    {
                        if (viewModel.BolImage == null)
                            viewModel.BolImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(bolImageFiles, UserContext.Id, viewModel.BolImage);
                    }
                    else
                    {
                        bolImageFile = bolImageFiles != null ? bolImageFiles[0] : null;
                    }

                    HttpPostedFileBase additionalImgFile = null;
                    if (additionalImgFiles != null && additionalImgFiles.Length > 1)
                    {
                        if (viewModel.AdditionalImage == null)
                            viewModel.AdditionalImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(additionalImgFiles, UserContext.Id, viewModel.AdditionalImage);
                    }
                    else
                    {
                        additionalImgFile = additionalImgFiles != null ? additionalImgFiles[0] : null;
                    }
                    HttpPostedFileBase taxAffidavitImgFile = null;
                    if (taxAffidavitImgFiles != null && taxAffidavitImgFiles.Length > 1)
                    {
                        if (viewModel.TaxAffidavitImage == null)
                            viewModel.TaxAffidavitImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(taxAffidavitImgFiles, UserContext.Id, viewModel.TaxAffidavitImage);
                    }
                    else
                    {
                        taxAffidavitImgFile = taxAffidavitImgFiles != null ? taxAffidavitImgFiles[0] : null;
                    }

                    HttpPostedFileBase BDNImgFile = null;
                    if (BDNImgFiles != null && BDNImgFiles.Length > 1)
                    {
                        if (viewModel.BDNImage == null)
                            viewModel.BDNImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(BDNImgFiles, UserContext.Id, viewModel.BDNImage);
                    }
                    else
                    {
                        BDNImgFile = BDNImgFiles != null ? BDNImgFiles[0] : null;
                    }


                    HttpPostedFileBase coastGuardInspectionImgFile = null;
                    if (coastGuardInspectionImgFiles != null && coastGuardInspectionImgFiles.Length > 1)
                    {
                        if (viewModel.CoastGuardInspectionImage == null)
                            viewModel.CoastGuardInspectionImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(coastGuardInspectionImgFiles, UserContext.Id, viewModel.CoastGuardInspectionImage);
                    }
                    else
                    {
                        coastGuardInspectionImgFile = coastGuardInspectionImgFiles != null ? coastGuardInspectionImgFiles[0] : null;
                    }

                    HttpPostedFileBase inspectionRequestVoucherImgFile = null;
                    if (inspectionRequestVoucherImgFiles != null && inspectionRequestVoucherImgFiles.Length > 1)
                    {
                        if (viewModel.InspectionRequestVoucherImage == null)
                            viewModel.InspectionRequestVoucherImage = new ImageViewModel();
                        PdfHelper.Instance.SetStreamInImageModel(inspectionRequestVoucherImgFiles, UserContext.Id, viewModel.InspectionRequestVoucherImage);
                    }
                    else
                    {
                        inspectionRequestVoucherImgFile = inspectionRequestVoucherImgFiles != null ? inspectionRequestVoucherImgFiles[0] : null;
                    }
                    SetImageDataToViewModel(imageFile, bolImageFile, signImageFile, additionalImgFile, taxAffidavitImgFile, coastGuardInspectionImgFile, inspectionRequestVoucherImgFile, viewModel, BDNImgFile);
                    StatusViewModel fileResponse = await UploadImagesToAzure(viewModel);
                    if (fileResponse.StatusCode == Status.Success)
                    {
                        InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain();
                        if (viewModel.InvoiceId == 0)
                        {
                            viewModel.SplitLoadSequence += 1;
                            var newInvoiceStatus = await invoiceCreateDomain.CreateSplitManualFtlInvoiceAsync(UserContext, viewModel);
                            newInvoiceStatus.DisplayMode = PageDisplayMode.Create;
                            return Json(newInvoiceStatus, JsonRequestBehavior.AllowGet);
                        }
                        if (viewModel.IsConvertFromDDT && viewModel.WaitingForAction == (int)WaitingAction.BolDetails)
                        {
                            InvoiceDomain invoiceDomain = new InvoiceDomain(invoiceCreateDomain);
                            var convertResponse = await invoiceDomain.CreateInvoiceFromDropTicketWithBol(viewModel, UserContext);
                            if (convertResponse.StatusCode == Status.Success)
                            {
                                convertResponse = await invoiceDomain.CreateInvoicesFromSplitLoadDropTicketsWFB(UserContext, viewModel);
                            }
                            response.InvoiceId = convertResponse.EntityId;
                            response.StatusCode = convertResponse.StatusCode;
                            response.StatusMessage = convertResponse.StatusMessage;
                        }
                        else if (viewModel.IsConvertFromDDT && viewModel.WaitingForAction == (int)WaitingAction.Images)
                        {
                            InvoiceDomain invoiceDomain = new InvoiceDomain(invoiceCreateDomain);
                            if (!invoiceDomain.HasRequiredInvoiceImages(viewModel))
                            {
                                InvoiceEditDomain invoiceEditDomain = new InvoiceEditDomain(invoiceCreateDomain);
                                response = await invoiceEditDomain.EditSplitInvoiceAsync(UserContext, viewModel);
                            }
                            else
                            {
                                var convertResponse = await invoiceDomain.CreateInvoiceFromDropTicketWithBol(viewModel, UserContext);

                                response.StatusCode = convertResponse.StatusCode;
                                response.StatusMessage = convertResponse.StatusMessage;
                            }
                        }
                        else
                        {
                            InvoiceEditDomain invoiceEditDomain = new InvoiceEditDomain(invoiceCreateDomain);
                            response = await invoiceEditDomain.EditSplitInvoiceAsync(UserContext, viewModel);
                        }
                    }
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetImageDataToViewModel(HttpPostedFileBase imageFile, HttpPostedFileBase bolImageFile, HttpPostedFileBase signImageFile, HttpPostedFileBase additionalImageFile, HttpPostedFileBase taxAffidavitImgFile, HttpPostedFileBase coastGuardInspectionImgFile, HttpPostedFileBase inspectionRequestVoucherImgFile, ManualInvoiceViewModel viewModel, HttpPostedFileBase BDNImgFile)
        {
            if (imageFile != null)
            {
                var reader = new BinaryReader(imageFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)imageFile.InputStream.Length);
                viewModel.InvoiceImage = new ImageViewModel() { Data = imageData, IsPdf = imageFile.ContentType == ApplicationConstants.ContentTypePdf, Name = imageFile.FileName.Replace('&', '-'), InputStream = imageFile.InputStream };
            }
            if (bolImageFile != null)
            {
                var reader = new BinaryReader(bolImageFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)bolImageFile.InputStream.Length);
                viewModel.BolImage = new ImageViewModel() { Data = imageData, IsPdf = bolImageFile.ContentType == ApplicationConstants.ContentTypePdf, Name = bolImageFile.FileName.Replace('&', '-'), InputStream = bolImageFile.InputStream };
            }
            if (signImageFile != null)
            {
                var reader = new BinaryReader(signImageFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)signImageFile.InputStream.Length);
                viewModel.SignatureImage = new ImageViewModel() { Data = imageData, IsPdf = signImageFile.ContentType == ApplicationConstants.ContentTypePdf, Name = signImageFile.FileName.Replace('&', '-'), InputStream = signImageFile.InputStream };
            }
            if (additionalImageFile != null)
            {
                var reader = new BinaryReader(additionalImageFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)additionalImageFile.InputStream.Length);
                viewModel.AdditionalImage = new ImageViewModel() { Data = imageData, IsPdf = additionalImageFile.ContentType == ApplicationConstants.ContentTypePdf, Name = additionalImageFile.FileName.Replace('&', '-'), InputStream = additionalImageFile.InputStream };
            }
            if (taxAffidavitImgFile != null)
            {
                var reader = new BinaryReader(taxAffidavitImgFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)taxAffidavitImgFile.InputStream.Length);
                viewModel.TaxAffidavitImage = new ImageViewModel() { Data = imageData, IsPdf = taxAffidavitImgFile.ContentType == ApplicationConstants.ContentTypePdf, Name = taxAffidavitImgFile.FileName.Replace('&', '-'), InputStream = taxAffidavitImgFile.InputStream };
            }

            if (BDNImgFile != null)
            {
                var reader = new BinaryReader(BDNImgFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)BDNImgFile.InputStream.Length);
                viewModel.TaxAffidavitImage = new ImageViewModel() { Data = imageData, IsPdf = BDNImgFile.ContentType == ApplicationConstants.ContentTypePdf, Name = BDNImgFile.FileName.Replace('&', '-'), InputStream = BDNImgFile.InputStream };
            }
            if (coastGuardInspectionImgFile != null)
            {
                var reader = new BinaryReader(coastGuardInspectionImgFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)coastGuardInspectionImgFile.InputStream.Length);
                viewModel.CoastGuardInspectionImage = new ImageViewModel() { Data = imageData, IsPdf = coastGuardInspectionImgFile.ContentType == ApplicationConstants.ContentTypePdf, Name = coastGuardInspectionImgFile.FileName.Replace('&', '-'), InputStream = coastGuardInspectionImgFile.InputStream };
            }

            if (inspectionRequestVoucherImgFile != null)
            {
                var reader = new BinaryReader(inspectionRequestVoucherImgFile.InputStream);
                byte[] imageData = reader.ReadBytes((int)inspectionRequestVoucherImgFile.InputStream.Length);
                viewModel.InspectionRequestVoucherImage = new ImageViewModel() { Data = imageData, IsPdf = inspectionRequestVoucherImgFile.ContentType == ApplicationConstants.ContentTypePdf, Name = inspectionRequestVoucherImgFile.FileName.Replace('&', '-'), InputStream = inspectionRequestVoucherImgFile.InputStream };
            }
        }

        [HttpGet]
        public ActionResult BolSummary(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "BolSummary"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(0, filter, groupIds);

                int invoiceAttachmentsPerEmail;
                int maxInvoiceCountPerSession;
                var masterDomain = ContextFactory.Current.GetDomain<MasterDomain>();

                int.TryParse(masterDomain.GetApplicationSettingValue(Constants.MaxInvoiceAttachmentsPerEmailKey), out invoiceAttachmentsPerEmail);
                int.TryParse(masterDomain.GetApplicationSettingValue(Constants.MaxInvoiceCountPerSessionKey), out maxInvoiceCountPerSession);
                response.MaxInvoiceAttachmentsPerEmail = invoiceAttachmentsPerEmail;
                response.MaxInvoiceCountPerSession = maxInvoiceCountPerSession;

                return View("~/Views/Shared/_PartialBolView.cshtml", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> BolInvoiceGrid(InvoiceDataTableViewModel requestModel, ViewInvoices view = ViewInvoices.All)
        {
            using (var tracer = new Tracer("InvoiceController", "BolInvoiceGrid"))
            {
                var dashboardDomain = new DashboardDomain();
                var invoiceDomain = new InvoiceServiceApiDomain(dashboardDomain);
                requestModel.GroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);
                var input = new InvoiceGridRequestModel() { ViewModel = requestModel, View = view, CompanyId = CurrentUser.CompanyId };
                var response = await invoiceDomain.ApiPostCall<List<InvoiceBolGridViewModel>>(ApplicationConstants.UrlGetSupplierBolInfo, input);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

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

        [HttpGet]
        public async Task<JsonResult> GetTerminals(int orderId, string terminal = "")
        {
            using (var tracer = new Tracer("InvoiceController", "GetTerminals"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<List<DropdownDisplayItem>>(ApplicationConstants.UrlGetTerminals, new GetTerminalsModel() { OrderId = orderId, Terminal = terminal });
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.CarrierAdmin)]
        public JsonResult GetAllDrivers()
        {
            using (var tracer = new Tracer("InvoiceController", "GetAllDrivers"))
            {
                var response = ContextFactory.Current.GetDomain<HelperDomain>().GetAllDrivers(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetTerminalPriceById(int terminalId, int orderId, DateTimeOffset deliveryDate)
        {
            using (var tracer = new Tracer("InvoiceController", "GetTerminalPriceById"))
            {
                var invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                var response = await invoiceServiceApiDomain.ApiPostCall<decimal>(ApplicationConstants.UrlGetTerminalPrice, new GetTerminalPriceModel() { OrderId = orderId, DeliveryDate = deliveryDate, TerminalId = terminalId });

                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }


        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetTerminalPrice(int orderId, DateTimeOffset deliveryDate)
        {
            using (var tracer = new Tracer("InvoiceController", "GetTerminalPrice"))
            {
                var invoiceServiceApiDomain = new InvoiceServiceApiDomain();
                var response = await invoiceServiceApiDomain.ApiPostCall<decimal>(ApplicationConstants.UrlGetTerminalPrice, new GetTerminalPriceModel() { OrderId = orderId, DeliveryDate = deliveryDate });
                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }

        #region Balance Invoice

        private ManualInvoiceViewModel SetBalanceInvoiceDetails(ManualInvoiceViewModel viewModel)
        {
            viewModel.FuelDropped = 1M; // required as its min decimal - but we zero it when posting
            viewModel.StartTime = DateTime.Now.ToLongTimeString();
            viewModel.EndTime = DateTime.Now.ToLongTimeString();

            //Remove processing fee for both tank rental and Balance
            var processingFee = viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.FirstOrDefault(t => t.FeeTypeId == ((int)FeeType.ProcessingFee).ToString());
            if (processingFee != null)
            {
                viewModel.FuelDeliveryDetails.FuelFees.ProcessingFee = processingFee;
                viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Remove(processingFee);
            }
            return viewModel;
        }

        [HttpGet]
        [ActionName("Balance")]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreateBalanceInvoice(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "CreateBalanceInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<ManualInvoiceViewModel>(string.Format(ApplicationConstants.UrlGetOrderDetailsForBalanceInvoice, orderId));
                SetBalanceInvoiceDetails(response);
                return View("~/Areas/Supplier/Views/Invoice/CreateBalanceInvoice.cshtml", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreateBalanceInvoice(ManualInvoiceViewModel viewModel)
        {
            viewModel.FuelDropped = 0;
            using (var tracer = new Tracer("InvoiceController", "CreateBalanceInvoice"))
            {
                BalanceInvoiceDomain invoiceCreateDomain = new BalanceInvoiceDomain();
                MessageViewModel messageViewModel = new MessageViewModel();
                if (ModelState.IsValid)
                {
                    if (viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.Any(t =>
                        t.FeeSubTypeId == (int)FeeSubType.ByQuantity && !t.DeliveryFeeByQuantity.Any()))
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = Resource.errMessageFeeByQuantityRequired;
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }

                    var mainOrderId = viewModel.OrderId;
                    viewModel.userId = CurrentUser.Id;
                    StatusViewModel response;
                    var isDuplicate = invoiceCreateDomain.IsDuplicateInvoiceNumber(viewModel.SupplierInvoiceNumber);
                    if (!isDuplicate)
                    {
                        var input = new CreateInvoiceViewModel() { UserContext = UserContext, ViewModel = viewModel };

                        if (viewModel.InvoiceId > 0)
                            response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<InvoiceCreateResponseViewModel>(ApplicationConstants.UrlCreditRebillBalanceInvoice, input);
                        else
                            response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<InvoiceCreateResponseViewModel>(ApplicationConstants.UrlCreateBalanceInvoice, input);
                    }
                    else
                        response = new StatusViewModel() { StatusMessage = ResourceMessages.GetMessage(Resource.valMessageAlreadyExist, new object[] { Resource.lblInvoiceNumber }) };

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                    if (response.StatusCode == Status.Success)
                    {
                        //push notify to approval user
                        var notifyUsers = new List<int>();
                        notifyUsers.Add((int)FCMAppUserTypes.ApprovalUser);
                        messageViewModel.Title = Resource.notification_OrderDropped_Title;
                        messageViewModel.Body = string.Format(Resource.notificationToBuyer_OrderDropped_Body, viewModel.PoNumber);
                        await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(messageViewModel, viewModel.OrderId, 0, notifyUsers);

                        return RedirectToAction("Details", "Order", new { area = "Supplier", id = mainOrderId });
                    }
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        public ActionResult AddFee(Currency currency, UoM uoM, bool isConstraintFee = false, bool isCommonFee = false, int truckLoadType = (int)TruckLoadTypes.LessTruckLoad)
        {
            var model = isConstraintFee ? new FeesViewModel { FeeConstraintTypeId = (int)FeeConstraintType.Weekend } : new FeesViewModel();
            model.CommonFee = isCommonFee;
            model.TruckLoadType = truckLoadType;
            model.CommonFee = isCommonFee;
            model.Currency = currency;
            model.UoM = uoM;
            model.FeeTaxDetails = new FeeTaxDetails();
            return PartialView("~/Views/Shared/BalanceInvoice/_PartialBalanceFeeDetails.cshtml", model);
        }

        #endregion

        #region Tank Rental Invoice

        [HttpGet]
        public ActionResult AddTankRentalFee(Currency currency, UoM uoM, int feeCategoryId = (int)FeeCategory.TankRental)
        {
            var model = new FeesViewModel
            {
                CommonFee = feeCategoryId != (int)FeeCategory.OtherFee,
                FeeSubTypeId = (int)FeeSubType.FlatFee,
                Currency = currency,
                UoM = uoM
            };

            return PartialView("~/Views/Shared/TankRentalInvoice/_PartialTankRentalFeeDetails.cshtml", model);
        }

        [HttpGet]
        [ActionName("TankRental")]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreateTankRentalInvoice(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "CreateTankRentalInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<ManualInvoiceViewModel>(string.Format(ApplicationConstants.UrlGetOrderDetailsForTankRentalInvoice, orderId));
                SetBalanceInvoiceDetails(response);
                return View("~/Areas/Supplier/Views/Invoice/CreateTankRentalInvoice.cshtml", response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreateTankRentalInvoice(ManualInvoiceViewModel viewModel)
        {
            viewModel.FuelDropped = 0;
            using (var tracer = new Tracer("InvoiceController", "CreateTankRentalInvoice"))
            {
                
                MessageViewModel messageViewModel = new MessageViewModel();
                if (ModelState.IsValid)
                {
                    var mainOrderId = viewModel.OrderId;
                    viewModel.userId = CurrentUser.Id;
                    StatusViewModel response;
                    var input = new CreateInvoiceViewModel() { UserContext = UserContext, ViewModel = viewModel };
                    if (viewModel.InvoiceId > 0)
                        response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<InvoiceCreateResponseViewModel>(ApplicationConstants.UrlRebillTankRentalInvoice, input);
                    else
                        response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<InvoiceCreateResponseViewModel>(ApplicationConstants.UrlCreateTankRentalInvoice, input);

                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                    if (response.StatusCode == Status.Success)
                    {
                        //push notify to approval user
                        var notifyUsers = new List<int>();
                        notifyUsers.Add((int)FCMAppUserTypes.ApprovalUser);
                        messageViewModel.Title = Resource.notification_OrderDropped_Title;
                        messageViewModel.Body = string.Format(Resource.notificationToBuyer_OrderDropped_Body, viewModel.PoNumber);
                        await ContextFactory.Current.GetDomain<PushNotificationDomain>().NotificationToBuyer(messageViewModel, viewModel.OrderId, 0, notifyUsers);
                    }
                    return RedirectToAction("Details", "Order", new { area = "Supplier", id = mainOrderId });
                }
                return View(viewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> GetBillingFrequencyTanks(int billingFrequencyTypeId, int fuelRequestId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetBillingFrequencyTanks"))
            {
                var response = await ContextFactory.Current.GetDomain<TankRentalInvoiceDomain>().GetRentalTanks(billingFrequencyTypeId, fuelRequestId);
                return PartialView("~/Views/Shared/TankRentalInvoice/_PartialInvoiceTanks.cshtml", response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetEIAPrice(int buyerCompanyId, int pricingType, int productType, DateTimeOffset deliveryDate)
        {
            using (var tracer = new Tracer("InvoiceController", "GetEIAPrice"))
            {
                var eiaResponse = ContextFactory.Current.GetDomain<EIAPriceUpdateDomain>().GetEIAPrice((FuelSurchagePricingType)pricingType, (SurchargeProductTypes)productType, deliveryDate.Date);
                if (eiaResponse > 0)
                {
                    var fsc = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPrice(eiaResponse, CurrentUser.CompanyId, buyerCompanyId, deliveryDate, (SurchargeProductTypes)productType);
                    if (fsc != null)
                    {
                        return Json(new { eiaResponse, percent = fsc.FuelSurchargeStartPercentage, start = fsc.PriceRangeStartValue, end = fsc.PriceRangeEndValue }, JsonRequestBehavior.DenyGet);
                    }
                }
                return Json(new { eiaResponse, percent = 0, start = 0, end = 0 }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetEIAPriceForAutoFreightMethod(int fuelSurchargeIndexId, DateTimeOffset deliveryDate)
        {
            using (var tracer = new Tracer("InvoiceController", "GetEIAPriceForAutoFreightMethod"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetFuelSurchargeTableForAutoFreightMethod(fuelSurchargeIndexId);
                var eiaResponse = ContextFactory.Current.GetDomain<EIAPriceUpdateDomain>().GetEIAPrice(response.SurchargePricingType, response.SurchargeProductType, deliveryDate.Date, response.FuelSurchageArea);
                if (eiaResponse > 0)
                {
                    var fsc = new FuelSurchargeDomain().GetFuelSurchargeRecordForEaiPriceForAutoFreightMethod(eiaResponse, fuelSurchargeIndexId);
                    if (fsc != null)
                    {
                        return Json(new { eiaResponse, percent = fsc.FuelSurchargeStartPercentage, start = fsc.PriceRangeStartValue, end = fsc.PriceRangeEndValue }, JsonRequestBehavior.DenyGet);
                    }
                }
                return Json(new { eiaResponse, percent = 0, start = 0, end = 0 }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> GetAccessorialFeeByAccessorialFeeId(string accessorialFeeId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetAccessorialFeeByAccessorialFeeId"))
            {
                var response = await ContextFactory.Current.GetDomain<AccessorialFeeDomain>().GetAccessorialFeeByAccessorialFeeId(accessorialFeeId, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> Credit(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "Credit"))
            {
                var response = await ContextFactory.Current.GetDomain<CreditRebillInvoiceDomain>().CreateCreditInvoiceAsync(UserContext.Id, id);
                return new JsonResult { Data = response, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreditPartial(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "CreditPartial"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetManualInvoiceForEditAsync(id);
                response.IsConvertFromDDT = false;
                response.TaxType = TaxType.Manual;
                response.TotalInvoiceAmount = response.TotalInvoiceAmount.GetPreciseValue(2);
                response.TotalCreditAmount = 0;
                if (response.FuelDropped == 0)
                {
                    response.FuelDropped = 1;
                }
                response.ZeroGallonAllowedFuelDropped = 0;
                return View(response);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreditPartial(ManualInvoiceViewModel viewModel)
        {
            using (var tracer = new Tracer("InvoiceController", "CreditPartial(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.FuelDropped = viewModel.ZeroGallonAllowedFuelDropped;
                    InvoiceCreateDomain invoiceCreateDomain = new InvoiceCreateDomain();
                    if (viewModel.FuelRequestFee.DeliveryFeeSubTypeId == (int)FeeSubType.ByQuantity && viewModel.FuelRequestFee.DeliveryFeeByQuantity.Count == 0)
                    {
                        viewModel.StatusCode = Status.Failed;
                        viewModel.StatusMessage = ResourceMessages.GetMessage(Resource.errMessageFeeByQuantityRequired);
                        DisplayCustomMessages((MessageType)viewModel.StatusCode, viewModel.StatusMessage);
                        return View(viewModel);
                    }
                    viewModel.userId = CurrentUser.Id;
                    StatusViewModel response;
                    response = await invoiceCreateDomain.CreatePartialCreditManualInvoiceAsync(UserContext, viewModel);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);

                    if (response.StatusCode == Status.Failed)
                    {
                        return RedirectToAction("Details", "Invoice", new { area = "Supplier", id = viewModel.InvoiceId });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Order", new { area = "Supplier", id = viewModel.OrderId });
                    }
                }
                return View(viewModel);
            }
        }

        #endregion

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PoNumberBulkUpload(PoNumberBulkCsvViewModel viewModel, HttpPostedFileBase csvFile)
        {
            using (var tracer = new Tracer("InvoiceController", "PoNumberBulkUpload"))
            {
                if (csvFile != null && csvFile.ContentLength > 0)
                {
                    if (csvFile.ContentLength < SiteFuel.Exchange.Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                    {
                        if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                        {
                            string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                            var csvFilePath = Server.MapPath("~\\Content\\PoNumber_Bulkupload_Template.csv");

                            var invoiceDomain = ContextFactory.Current.GetDomain<PoNumberBulkUploadDomain>();
                            var response = await invoiceDomain.ValidatePoNumberBulkFile(UserContext, csvText, csvFilePath, CompanyType.Supplier);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await invoiceDomain.UploadFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Supplier);

                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    return RedirectToAction("View", "Invoice", new { area = "Supplier" });
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

                return RedirectToAction("View", "Invoice", new { area = "Supplier" });
            }
        }

        #region Invoice Bulk Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<ActionResult> InvoiceBulkUpload(ManualInvoiceViewModel viewModel, HttpPostedFileBase csvFile)
        {
            if (csvFile != null && csvFile.ContentLength > 0)
            {
                if (csvFile.ContentLength < Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                {
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {
                        string csvText = new StreamReader(csvFile.InputStream).ReadToEnd();
                        var csvFilePath = Server.MapPath(Resource.invoiceBulkUploadFilePath);

                        var bulkDomain = ContextFactory.Current.GetDomain<InvoiceBulkUploadDomain>();
                        var input = new BulkUploadRequestModel() { UserContext = UserContext, CsvFilePath = csvFilePath, CsvText = csvText };

                        var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlValidateInvoiceBulkFile, input);
                        if (response.StatusCode == Status.Success)
                        {
                            response = await bulkDomain.UploadFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName);

                            DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                            if (response.StatusCode != Status.Failed)
                            {
                                return RedirectToAction("View", "Invoice", new { area = "Supplier" });
                            }
                        }
                        else
                        {
                            //add validation errors to queuemessage
                            bulkDomain.AddErrorListToQueue(response.StatusMessage, csvFile.FileName, UserContext);
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
            return RedirectToAction("View", "Invoice", new { area = "Supplier" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.Supplier, UserRoles.SupplierAdmin, UserRoles.AccountingPerson, UserRoles.CarrierAdmin, UserRoles.Carrier)]
        public async Task<ActionResult> ImageBulkUpload(List<HttpPostedFileBase> postedFiles)
        {
            var uploadedFiles = postedFiles;
            if (uploadedFiles.Any())
            {
                StringBuilder uploadFailedResult = new StringBuilder();
                StringBuilder uploadSuccessResult = new StringBuilder();

                foreach (var fileBase in uploadedFiles)
                {
                    if (fileBase != null)
                    {
                        if (fileBase.ContentLength < Core.Utilities.AppSettings.MaxAllowedUploadFileSize)
                        {
                            var fileFormat = Path.GetExtension(fileBase.FileName).ToLower();
                            if (fileFormat == ".jpg" || fileFormat == ".png" || fileFormat == ".pdf")
                            {
                                var bulkDomain = ContextFactory.Current.GetDomain<InvoiceBulkUploadDomain>();
                                var fileName = bulkDomain.RemoveSpecialCharacters(fileBase.FileName);
                                var response = bulkDomain.ValidateUploadedImageFiles(UserContext, fileBase.FileName);
                                if (response.StatusCode == Status.Success)
                                {
                                    response = await bulkDomain.UploadImageToBlob(UserContext, fileBase.InputStream, fileName, response.EntityNumber.ToEnum<InvoiceImageType>(), response.EntityId);

                                    if (response.StatusCode == Status.Failed)
                                    {
                                        uploadFailedResult.AppendLine("</br>");
                                        uploadFailedResult.AppendLine(response.StatusMessage);
                                    }
                                    else if (response.StatusCode == Status.Success)
                                    {
                                        uploadSuccessResult.AppendLine("</br>");
                                        uploadSuccessResult.AppendLine(Resource.successMessageTPDInvoiceImageUpload);
                                    }
                                }
                                else
                                {
                                    uploadFailedResult.AppendLine("</br>");
                                    uploadFailedResult.AppendLine(response.StatusMessage);
                                }
                            }
                            else
                            {
                                uploadFailedResult.AppendLine("</br>");
                                uploadFailedResult.AppendLine(Resource.errorMessageInvoiceImageFileType);
                            }
                        }
                        else
                        {
                            //DisplayCustomMessages(MessageType.Error, Resource.errFileSizeMessage);
                            uploadFailedResult.AppendLine("</br>");
                            uploadFailedResult.AppendLine(Resource.errFileSizeMessage);
                        }
                    }
                    else
                    {
                        uploadFailedResult.AppendLine("</br>");
                        uploadFailedResult.AppendLine(Resource.errMessageNoFileChosen);
                    }
                }

                if (uploadFailedResult.Length > 0)
                    DisplayCustomMessages(MessageType.Error, uploadFailedResult.ToString());
                else if (uploadSuccessResult.Length > 0)
                    DisplayCustomMessages(MessageType.Success, Resource.successMessageTPDInvoiceImageUpload);
            }
            else
            {
                DisplayCustomMessages(MessageType.Error, Resource.errMessageNoFileChosen);
            }
            return RedirectToAction("View", "Invoice", new { area = "Supplier" });
        }
        #endregion

        #region New Invoice Creation using Angular

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<JsonResult> GetInvoiceViewModel(int orderId, int trackableScheduleId = 0)
        {
            var invoiceCreate = new InvoiceServiceApiDomain();
            InvoiceViewModelNew viewModel = await invoiceCreate.ApiPostCall<InvoiceViewModelNew>(string.Format(ApplicationConstants.UrlGetPoDetailsToCreateInvoice, orderId, trackableScheduleId), new InvoiceServiceRequestModel() { UserContext = UserContext });
            return new JsonResult
            {
                Data = viewModel,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue

            };
            //return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get drop details for selected order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetInvoiceDropModel(int orderId)
        {
            var invoiceCreate = new InvoiceServiceApiDomain();
            var response = await invoiceCreate.ApiGetCall<InvoiceDropViewModel>(string.Format(ApplicationConstants.UrlGetInvoiceDropModel, orderId));
            return new JsonResult
            {
                Data = response,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue

            };
            // return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get fees for selected order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetInvoiceDropFees(int orderId)
        {
            var invoiceCreate = new InvoiceCreateDomain();
            var response = await invoiceCreate.GetInvoiceDropFeesAsync(orderId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get other PO numbers for this buyer
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> GetCustomerPoList(int orderId)
        {
            var invoiceCreate = new InvoiceCreateDomain();
            var response = await invoiceCreate.GetCustomerPoList(UserContext, orderId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public ActionResult CreateNew(int orderId, int trackableScheduleId = 0, int existingHeaderId = 0)
        {
            ViewBag.Title = "Create Invoice";
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoiceViewModelNew"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> CreateNew(InvoiceViewModelNew invoiceViewModelNew)
        {
            using (var tracer = new Tracer("InvoiceController", "CreateNew"))
            {
                if (invoiceViewModelNew.Driver != null && invoiceViewModelNew.Driver.Id > 0)
                {
                    if ((invoiceViewModelNew.Driver.Name.Contains(Resource.lblDriverEmailVerfied)))
                    {
                        var driverName = invoiceViewModelNew.Driver.Name.Replace(Resource.lblDriverEmailVerfied, "");
                        invoiceViewModelNew.Driver.Name = driverName;
                    }
                    else if ((invoiceViewModelNew.Driver.Name.Contains(Resource.lblDriverInvited)))
                    {
                        var driverName = invoiceViewModelNew.Driver.Name.Replace(Resource.lblDriverInvited, "");
                        invoiceViewModelNew.Driver.Name = driverName;
                    }
                }
                
                invoiceViewModelNew.CreationMethod = CreationMethod.SFX;
                //var domain = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>();
                //var response = await domain.CreateManualInvoice(UserContext, invoiceViewModelNew);

                var domain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
                var response = await domain.ApiPostCall<StatusViewModel>(ApplicationConstants.UrlCreateConsolidatedInvoice, new CreateConsolidateInvoiceModel() { ViewModel = invoiceViewModelNew, UserContext = UserContext });
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAssignedAssets(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetAssignedAssetsAsync"))
            {
                var invoiceCommonDomain = new InvoiceServiceApiDomain();
                var response = await invoiceCommonDomain.ApiGetCall<List<AssetDropViewModel>>(string.Format(ApplicationConstants.UrlGetAssignedAssets, orderId));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetFeeTypes(Currency currency = Currency.None, int orderId = 0)
        {
            using (var tracer = new Tracer("InvoiceController", "GetFeeTypes"))
            {
                var masterDomain = new MasterDomain();
                var response = masterDomain.GetFeeTypesAsync(UserContext.CompanyId, orderId, currency);
                response.RemoveAll(t => t.Code.Equals(((int)FeeType.DryRunFee).ToString()));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetFeeSubTypes(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetFeeSubTypes"))
            {
                var masterDomain = new MasterDomain();
                var response = masterDomain.GetAllFeeSubTypes(orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetFeeConstraintTypes()
        {
            using (var tracer = new Tracer("InvoiceController", "GetFeeConstraintTypes"))
            {
                var masterDomain = new MasterDomain();
                var response = masterDomain.GetAllFeeConstraintTypes();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetTaxePricingTypes(int OrderId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetTaxePricingTypes"))
            {
                var masterDomain = new MasterDomain();
                var response = masterDomain.GetTaxesForOtherProductFuelType(OrderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDeliverySchedules(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetDeliverySchedules"))
            {
                var helperDomain = new HelperDomain();
                var response = helperDomain.GetCurrentDeliverySchedules(orderId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetOriginalInvoiceDetails(int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetOriginalInvoiceDetails"))
            {
                var invoiceDomain = new InvoiceServiceApiDomain();
                var response = await invoiceDomain.ApiGetCall<InvoiceViewModelNew>(string.Format(ApplicationConstants.UrlGetOriginalInvoiceDetails, invoiceId, UserContext.CompanyId));
                //var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetOriginalInvoiceDetails(invoiceId, UserContext.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetBlendedProducts(string blendGroupId)
        {
            using (var tracer = new Tracer("InvoiceController", "GetBlendedProducts"))
            { 
                var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().GetBlendedSchedules(blendGroupId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpGet]
        public JsonResult GetSasToken()
        {
            var azureblob = new AzureBlobStorage();
            var response = azureblob.GetContainerSasUri(BlobContainerType.InvoicePdfFiles.ToString());
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public ActionResult ConvertToInvoice(int id, int orderId, int waitingAction = 0)
        {
            ViewBag.Title = "Create Invoice";
            return View("CreateNew");
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> ConvertToInvoice(InvoiceViewModelNew invoiceViewModel, int ddtId)
        {
            var input = new CreateConsolidateInvoiceModel() { UserContext = UserContext, ViewModel = invoiceViewModel };
            var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlConvertToInvoice, ddtId), input);
            return Json(response);
        }

        [HttpGet]
        public async Task<JsonResult> GetAssignedDriver(int scheduleId, int orderId)
        {
            var response = await ContextFactory.Current.GetDomain<HelperDomain>().GetAssignedDriverForSchedule(scheduleId, orderId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> CalculateDropQuantitiesFromPrePostForCreateInvoice(List<AssetDropViewModel> assetInfoList)
        {
            var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<List<DropQuantityByPrePostDipResponseModel>>(ApplicationConstants.UrlCalculateDropQuantitiesFromPrePostForCreateInvoice, assetInfoList);
            return Json(response);
        }

        [HttpGet]
        public async Task<JsonResult> GetBulkPlantsForAutoFreightMethod(int orderId, string bulkPlant = "")
        {
            using (var tracer = new Tracer("InvoiceController", "GetBulkPlantsForAutoFreightMethod"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<List<DropdownDisplayItem>>(ApplicationConstants.UrlGetBulkPlantsForAutoFreightMethod, new GetBulkPlantsModel() { OrderId = orderId, BulkPlant = bulkPlant });
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public async Task<DecimalResponseModel> GetGallonsPerMetricTonAsync(decimal gravity)
        {

            DecimalResponseModel response = new DecimalResponseModel();
            if (gravity > 0)
            {
                response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<DecimalResponseModel>(string.Format(ApplicationConstants.UrlGetGallonsPerMetricTonAsync, Math.Round(gravity, 1)));
            }
            return response;
        }

        [HttpPost]
        public async Task<JsonResult> ValidateGravityAndConvertForMFN(MFNConversionRequestViewModel conversionRequest)
        {
            using (var tracer = new Tracer("InvoiceController", "ValidateGravityAndConvertForMFN"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<MFNConversionResponseViewModel>(ApplicationConstants.UrlValidateGravityAndConvertForMFN, conversionRequest);
                return Json(response);
            }
        }

        [HttpGet]
        public async Task<JsonResult> ValidateGravityByInvoiceId(int invoiceId, decimal gravity)
        {
            using (var tracer = new Tracer("InvoiceController", "ValidateGravityByInvoiceId"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<StatusViewModel>(string.Format(ApplicationConstants.UrlValidateGravityByInvoiceId, invoiceId, gravity));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> IsValidApiGravity(int invoiceId, decimal gravity)
        {
            using (var tracer = new Tracer("InvoiceController", "IsValidApiGravity"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<MFNConversionResponseViewModel>(string.Format(ApplicationConstants.UrlIsValidApiGravity, invoiceId, gravity));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        #region Update Images
        [HttpPost]
        public async Task<JsonResult> UpdateImagesToInvoice(UploadImageModel viewModel)
        {
            StatusViewModel response = new StatusViewModel();
            if (viewModel.InvoiceHeaderId > 0)
            {
                var uploadModel = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>().ValidateUploadImagesModel(viewModel);
                if (uploadModel.StatusCode == Status.Failed)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = uploadModel.StatusMessage;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                foreach (var item in uploadModel.ImagesModels)
                {
                    if (item.ImageFile != null)
                    {
                        string fileName = FileHelper.GetValidFileName(item.ImageFile.FileName);
                        if (item.ImageType == InvoiceImageType.Bol)
                        {
                            var fileNameArr = fileName.Split('|');
                            if (fileNameArr.Length == 2)
                                fileName = fileNameArr[1];
                        }
                        fileName = BlobContainerType.BDN.GetDisplayName() + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "/" + fileName;

                        response = await AzureStorageService.UploadImageToBlob(UserContext, item.ImageFile.InputStream, fileName, BlobContainerType.InvoicePdfFiles);
                        if (response.StatusCode == Status.Success)
                            item.ImageViewModel = new ImageViewModel() { FilePath = response.StatusMessage };
                        else
                            break;
                    }
                }
                if (response.StatusCode == Status.Success)
                    response = await ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>().UpdateImagesToMarineInvoice(uploadModel);
            }
            else
                response.StatusMessage = Resource.ErrInUpdateImagesToInvoice;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUploadImagesCollectionView(int invoiceHeaderId, int countryId, bool isMarineLocation = false)
        {
            var model = new UpdateImagesModel() { InvoiceHeaderId = invoiceHeaderId, IsMarineLocation = isMarineLocation, countryId = countryId };
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialUploadImagesCollection.cshtml", model);
        }

        [HttpGet]
        public ActionResult GetUploadInvoiceImagesView(int invoiceHeaderId, bool isMarineLocation, int countryId)
        {
            var model = new UpdateImagesViewModel();
            model.InvoiceHeaderId = invoiceHeaderId;
            model.IsMarineLocation = isMarineLocation;
            model.countryId = countryId;
            return PartialView("~/Areas/Supplier/Views/Shared/_PartialUploadInvoiceImagesPopup.cshtml", model);
        }
        #endregion

        #region Edit from grid
        [HttpPost]
        public async Task<JsonResult> SaveBDNInvoiceDetails(List<InvoiceBolEditGrid> gridData)
        {
            StatusViewModel response = new StatusViewModel();
            var conversionResponse = await ContextFactory.Current.GetDomain<InvoiceDomain>().ValidateGravityForDDTInlineEdit(gridData);
            if (!conversionResponse.IsValidGravity)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = string.Format(Resource.lblInvalidAPIGravity, conversionResponse.ConvertedQty);
                return Json(response);
            }
            response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlSaveBDNInvoiceDetails, new UpdateBDNInvoiceDetailModel() { Model = gridData, UserContext = UserContext });
            return Json(response);
        }
        #endregion

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> ConfirmBdnDetails(int InvoiceId, int OrderId, int InvoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<ScheduleBuilderDomain>().CancelDriverScheduleAfterBDNConfirmation(UserContext, OrderId, InvoiceId, InvoiceHeaderId);
            return Json(response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public async Task<ActionResult> ConfirmInvoiceDetails(int InvoiceId, int OrderId, int InvoiceHeaderId)
        {
            var response = await ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>().ConfirmInvoiceDetails(UserContext, InvoiceId, OrderId, InvoiceHeaderId);
            return Json(response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public ActionResult ConvertDdtToInvoiceWithoutTax(int InvoiceId, int OrderId, int InvoiceHeaderId)
        {
            var response = ContextFactory.Current.GetDomain<ConsolidatedInvoiceDomain>().ConvertToInvoiceWithoutTax(UserContext, InvoiceId, OrderId, InvoiceHeaderId);
            return Json(response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        public ActionResult RetryPDI(int headerId = 0, string displayNumber = "")
        {
            using (var tracer = new Tracer("InvoiceController", "RetryPDI"))
            {
                if (headerId > 0 && !string.IsNullOrWhiteSpace(displayNumber))
                {
                    var response = ContextFactory.Current.GetDomain<InvoiceCommonDomain>().RetrySendingDetailsToPdi(headerId, displayNumber, UserContext.Id);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return PartialView("_DisplayCustomMessage");
                }
                else
                    LogManager.Logger.WriteException("InvoiceController", "RetryPDI", $"headerId={headerId} - displayNumber={displayNumber} ", new Exception());

                DisplayCustomMessages(MessageType.Error, "Could not get parameter values required to Retry request");
                return PartialView("_DisplayCustomMessage");
            }
        }


        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.CarrierAdmin, UserRoles.Admin,UserRoles.SuperAdmin)]
        public JsonResult RetryAllPDI(bool IsDdt)
        {
            StatusViewModel response = new StatusViewModel(Status.Failed);
            using (var tracer = new Tracer("InvoiceController", "RetryAllPDI"))
            {
                var lst = ContextFactory.Current.GetDomain<ExceptionDomain>().GetPDIExceptions(CurrentUser.CompanyId, IsDdt);
                
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        if (item.Id > 0 && !string.IsNullOrWhiteSpace(item.Name))
                        {
                           ContextFactory.Current.GetDomain<InvoiceCommonDomain>().RetrySendingDetailsToPdi(item.Id, item.Name, UserContext.Id);
                            
                        }
                    }
                    response.StatusCode = Status.Success;
                    DisplayCustomMessages(MessageType.Success, Resource.msgPdiRetryRequestSuccess);
                }
                else if (lst==null)
                {
                    response.StatusCode = Status.Warning;
                    DisplayCustomMessages(MessageType.Info, Resource.msgPdiRetryRequestRecordAlreadyEnqueued);
                }
                else
                {
                    response.StatusCode = Status.Warning;
                    DisplayCustomMessages(MessageType.Info, Resource.msgPdiRetryRequestRecordNotFound);
                }

            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<JsonResult> DeleteBolForMarineInvoice(int invoiceHeaderId,int invoiceFtlDetailsId,int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "DeleteBolForMarineInvoice"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<StatusViewModel>(string.Format(ApplicationConstants.UrlDeleteBolForMarineInvoice, invoiceHeaderId, invoiceFtlDetailsId, invoiceId));
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> InvoiceSummary(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceSummary"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().GetSupplierInvoiceDetail(id, UserContext);
                if (response.StatusMessage == Resource.errMessageInvoiceDeleted)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "Invoice", new { area = "Supplier" });
                }
                else if (response.StatusMessage == Resource.errMessageDraftConvertedtoDDT)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("Messages", "Mailbox", new { area = "Messages" });
                }
                return PartialView("_PartialInvoiceDetailSummary", response);
            }
        }
    }
}