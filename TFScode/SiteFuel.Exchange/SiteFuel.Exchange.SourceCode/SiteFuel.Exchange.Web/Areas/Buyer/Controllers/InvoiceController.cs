using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class InvoiceController : InvoiceBaseController
    {
        [HttpGet]
        [ActionName("View")]
        public ActionResult Index(int jobId = 0, InvoiceFilterType filter = InvoiceFilterType.All, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "Index"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(jobId, filter, groupIds);
                return View("View", response);
            }
        }

        [HttpGet]
        public ActionResult DropTicketsView(int jobId = 0, InvoiceFilterType filter = InvoiceFilterType.All, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "DropTicketsView"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetDigitalDropTicketFilter(jobId, filter, groupIds);
                return View("View", response);
            }
        }

        public PartialViewResult InvoiceDetails(int jobId = 0, InvoiceFilterType filter = InvoiceFilterType.All)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceDetails"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(jobId, filter);
                return PartialView("~/Areas/Buyer/Views/Shared/_PartialInvoiceGrid.cshtml", response);
            }
        }

        public PartialViewResult DropTicketDetails(int jobId = 0, InvoiceFilterType filter = InvoiceFilterType.All)
        {
            using (var tracer = new Tracer("InvoiceController", "DropTicketDetails"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetDigitalDropTicketFilter(jobId, filter);
                return PartialView("~/Areas/Buyer/Views/Shared/_PartialDigitalDropTicketGrid.cshtml", response);
            }
        }

        [HttpGet]
        public ActionResult GetInvoiceTab(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "GetInvoiceTab"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(0, filter, groupIds);
                return PartialView("~/Areas/Buyer/Views/Shared/_PartialInvoiceTab.cshtml", response);
            }
        }

        [HttpGet]
        public ActionResult GetDropTicketTab(InvoiceFilterType filter = InvoiceFilterType.SupplierInvoices, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "GetDropTicketTab"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetDigitalDropTicketFilter(0, filter, groupIds);
                return PartialView("~/Areas/Buyer/Views/Shared/_PartialDigitalDropTicketTab.cshtml", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> InvoiceGrid(InvoiceDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceGrid"))
            {
                var dashboardDomain = new DashboardDomain();
                var invoiceDomain = new InvoiceServiceApiDomain(dashboardDomain);
                requestModel.GroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);

                var input = new InvoiceGridRequestModel { ViewModel = requestModel, UserContext = UserContext, InvoiceType = 0 };
                var response = await invoiceDomain.ApiPostCall<List<InvoiceGridViewModel>>(ApplicationConstants.UrlGetBuyerInvoiceGrid, input);

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
        public async Task<ActionResult> DigitalDropTicketGrid(InvoiceDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("InvoiceController", "DigitalDropTicketGrid"))
            {
                var dashboardDomain = new DashboardDomain();
                var invoiceDomain = new InvoiceServiceApiDomain(dashboardDomain);
                requestModel.GroupIds = dashboardDomain.DecryptData(requestModel.GroupIds);
                var input = new InvoiceGridRequestModel() { ViewModel = requestModel, UserContext = UserContext, InvoiceType = (int)InvoiceType.DigitalDropTicketManual };
                var response = await invoiceDomain.ApiPostCall<List<InvoiceGridViewModel>>(ApplicationConstants.UrlGetBuyerInvoiceGrid, input);
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

        public async Task<ActionResult> InvoiceGridByOrder(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceGridByOrder"))
            {
                var url = string.Format(ApplicationConstants.UrlBuyerInvoiceGridByOrder, orderId, CurrentUser.Id, (int)InvoiceType.Manual);
                var response = await new InvoiceServiceApiDomain().ApiGetCall<List<InvoiceGridViewModel>>(url);
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

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> DealAgree(int discountId, int invoiceId, int invoiceHeaderId)
        {
            using (var tracer = new Tracer("InvoiceController", "DealAgree"))
            {
                var invoiceDomain = new InvoiceServiceApiDomain();
                var response = await invoiceDomain.ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlDealAgree, discountId, invoiceId, invoiceHeaderId), UserContext);
                if (response.StatusCode == Status.Failed)
                {
                    response.StatusMessage = string.Format(response.StatusMessage, "Buyer", response.EntityId);
                }
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Invoice", new { area = "Buyer", id = response.EntityId });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> DealNotAgree(int discountId, int invoiceId)
        {
            using (var tracer = new Tracer("InvoiceController", "DealNotAgree"))
            {
                var response = await new InvoiceServiceApiDomain().ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlDeclineDeal, discountId, invoiceId), UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("Details", "Invoice", new { area = "Buyer", id = invoiceId });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> SaveDiscount(DiscountViewModel viewModel)
        {
            using (var tracer = new Tracer("InvoiceController", "SaveDiscount"))
            {
                if (ModelState.IsValid)
                {
                    viewModel.CreatedCompanyId = CurrentUser.CompanyId;
                    viewModel.CreatedBy = CurrentUser.Id;
                    var input = new SaveDiscountRequestModel { ViewModel = viewModel, UserContext = UserContext };
                    var response = await new InvoiceServiceApiDomain().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlSaveDiscount, input);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }
            }

            return RedirectToAction("Details", "Invoice", new { area = "Buyer", id = viewModel.InvoiceId });
        }

        public async Task<ActionResult> DigitalDropTicketGridByOrder(int orderId)
        {
            using (var tracer = new Tracer("InvoiceController", "DigitalDropTicketGridByOrder"))
            {
                var url = string.Format(ApplicationConstants.UrlBuyerInvoiceGridByOrder, orderId, CurrentUser.Id, (int)InvoiceType.DigitalDropTicketManual);
                var response = await new InvoiceServiceApiDomain().ApiGetCall<List<InvoiceGridViewModel>>(url);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson)]
        public async Task<JsonResult> EditInvoicePoNumber(int invoiceId, string poNumber)
        {
            StatusViewModel response;
            using (var tracer = new Tracer("InvoiceController", "EditInvoicePoNumber"))
            {
                var inputObj = new EditInvoicePoNumberReqModel { InvoiceId = invoiceId, PoNumber = poNumber, UserContext = UserContext };
                response = await new InvoiceServiceApiDomain().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlEditInvoicePoNumber, inputObj);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> InvoiceHistoryGrid(int id = 0)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceHistoryGrid"))
            {
                var url = string.Format(ApplicationConstants.UrlGetInvoiceHistoryGridBuyerAsync, CurrentUser.Id, id);
                var response = await new InvoiceServiceApiDomain().ApiGetCall<List<InvoiceHistoryGridViewModel>>(url);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> InvoiceApprovalHistoryGrid(int id = 0)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceApprovalHistoryGrid"))
            {
                var url = string.Format(ApplicationConstants.UrlInvoiceApprovalHistoryGrid, id);
                var response = await new InvoiceServiceApiDomain().ApiGetCall<List<InvoiceApprovalHistoryGridViewModel>>(url);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "Details"))
            {
                var url = string.Format(ApplicationConstants.UrlGetBuyerInvoiceStatus, id);
                var response = await new InvoiceServiceApiDomain().ApiGetCall<InvoiceDetailViewModel>(url);
                if (response.StatusMessage == Resource.errMessageInvoiceDeleted)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View", "Invoice", new { area = "Buyer" });
                }
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetStatementPdfDetails(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "GetStatementPdfDetails"))
            {
                var url = string.Format(ApplicationConstants.UrlGetStatementPdfDetails, id);
                var response = await new InvoiceServiceApiDomain().ApiGetCall<StatementPdfViewModel>(url);
                return PartialView("~/Views/Shared/_PartialStatementPdf.cshtml", response);
            }
        }

        [HttpGet]
        public ActionResult Approvals(int jobId = 0, InvoiceFilterType filter = InvoiceFilterType.WaitingForApproval, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "Approvals"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(jobId, filter, groupIds);
                return View(response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> WaitingForApprovalList(InvoiceFilterViewModel invoiceFilter = null)
        {
            using (var tracer = new Tracer("InvoiceController", "WaitingForApprovalList"))
            {
                var response = await new InvoiceServiceApiDomain().ApiPostCall<List<InvoiceApprovalHistoryGridViewModel>>(string.Format(ApplicationConstants.UrlGetBuyerWaitingForApprovalList, CurrentUser.Id), invoiceFilter);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> PartialMapView(InvoiceFilterViewModel invoiceFilter = null)
        {
            using (var tracer = new Tracer("InvoiceController", "PartialMapView"))
            {
                var response = await new InvoiceServiceApiDomain().ApiPostCall<List<MapViewModel>>(string.Format(ApplicationConstants.UrlGetMapDataAsync, CurrentUser.Id), invoiceFilter);
                return PartialView("_PartialMapView", response);
            }
        }

        //[AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> DownloadPdf(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "DownloadPdf"))
            {
                var input = new InvoicePdfRequestModel() { InvoiceHeaderId = id, CompanyType = CompanyType.Buyer };
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
        }

        [HttpGet]
        public async Task<PartialViewResult> DetailsTab(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "DetailsTab"))
            {
                var response = await new InvoiceServiceApiDomain().ApiPostCall<InvoiceDetailViewModel>(string.Format(ApplicationConstants.UrlGetBuyerInvoiceDetails, id), UserContext);
                return PartialView("_PartialTabInvoiceDetail", response);
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> InvoicePdfTab(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoicePdfTab"))
            {
                var response = await new InvoiceServiceApiDomain().ApiGetCall<InvoicePdfViewModel>(string.Format(ApplicationConstants.UrlGetInvoicePdfNewAsync, id, CompanyType.Buyer));
                return PartialView("_PartialInvoicePdf", response);
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> ConsolidatedInvoicePdfTab(int invoiceHeaderId)
        {
            using (var tracer = new Tracer("InvoiceController", "ConsolidatedInvoicePdfTab"))
            {
                var input = new InvoicePdfRequestModel() { InvoiceHeaderId = invoiceHeaderId, CompanyType = CompanyType.Buyer };
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
                else if (response.Invoices.Any() && response.Invoices.Any(t=>t.IsMarineLocation))
                {
                    return PartialView("_PartialConsolidatedMarineInvoicePdf", response);
                }
                else
                {
                    return PartialView("_PartialConsolidatedInvoicePdf", response);
                }
            }
        }

        [HttpGet]
        public async Task<PartialViewResult> InvoiceHistoryTab(int id)
        {
            using (var tracer = new Tracer("InvoiceController", "InvoiceHistoryTab"))
            {
                var response = await new InvoiceServiceApiDomain().ApiGetCall<InvoiceDetailViewModel>(string.Format(ApplicationConstants.UrlGetBuyerInvoiceStatus, id));
                return PartialView("_PartialTabInvoiceHistory", response);
            }
        }

        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0, EntityType entityTypeId = EntityType.Invoice)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(UserContext, entityTypeId, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get job approvals by job id. By default shows WaitingForApproval jobs
        /// </summary>
        /// <param name="jobId">int</param>
        /// <param name="filter">InvoiceFilterType</param>
        /// <returns>PartialViewResult</returns>
        [HttpGet]
        public PartialViewResult GetApprovals(int jobId, InvoiceFilterType filter = InvoiceFilterType.WaitingForApproval)
        {
            using (var tracer = new Tracer("InvoiceController", "GetApprovals"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(jobId, filter);
                return PartialView("Approvals", response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCxmlCheckoutFlag(int headerId)
        {
            var punchoutOrderMessageDomain = new PunchoutOrderMessageDomain();
            await punchoutOrderMessageDomain.UpdateCxmlCheckoutFlag(headerId);
            return RedirectToAction("SignOut", "Account", new { area = string.Empty });
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
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
                            var csvFilePath = Server.MapPath("~\\Content\\PoNumber_BulkUpload_Template.csv");

                            var invoiceDomain = ContextFactory.Current.GetDomain<PoNumberBulkUploadDomain>();
                            var input = new BulkUploadRequestModel() { CompanyType = CompanyType.Buyer, UserContext = UserContext, CsvFilePath = csvFilePath, CsvText = csvText };
                            var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlValidatePoNumberBulkFile, input);
                            if (response.StatusCode == Status.Success)
                            {
                                response = await invoiceDomain.UploadFileToBlob(UserContext, csvFile.InputStream, csvFile.FileName, CompanyType.Buyer);

                                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                                if (response.StatusCode != Status.Failed)
                                {
                                    return RedirectToAction("View", "Invoice", new { area = "Buyer" });
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

                return RedirectToAction("View", "Invoice", new { area = "Buyer" });
            }
        }

        [HttpGet]
        public ActionResult BolSummary(InvoiceFilterType filter = InvoiceFilterType.All, string groupIds = "")
        {
            using (var tracer = new Tracer("InvoiceController", "BolSummary"))
            {
                var response = ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceFilter(0, filter, groupIds);

                int invoiceAttachmentsPerEmail = 5;
                int maxInvoiceCountPerSession = 30;
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
                var input = new BolGridRequestModel { UserContext = UserContext, RequestModel = requestModel, View = view };
                var response = await invoiceDomain.ApiPostCall<List<InvoiceBolGridViewModel>>(ApplicationConstants.UrlBuyerBolInvoiceGrid, input);
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
    }
}