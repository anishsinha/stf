using Ionic.Zip;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace SiteFuel.Exchange.Web.Controllers
{
    public class InvoiceBaseController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Approved(int id)
        {
            using (var tracer = new Tracer("InvoiceBaseController", "Approved"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(string.Format(ApplicationConstants.UrlApproveInvoice, id), UserContext);

                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageApproveInvoiceFailed);
                }
                else
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                }

                return RedirectToAction("Details", "Invoice", new { id = id });
            }
        }

        [HttpGet]
        public async Task<ActionResult> Decline(int id, int statusId)
        {
            using (var tracer = new Tracer("InvoiceBaseController", "Decline"))
            {
                var viewModel = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<DeclineInvoiceViewModel>(string.Format(ApplicationConstants.UrlGetDeclineInvoiceDetail, id, statusId), UserContext);
                return View("~/Areas/Buyer/Views/Invoice/Decline.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Decline(DeclineInvoiceViewModel viewModel)
        {
            using (var tracer = new Tracer("InvoiceBaseController", "Decline(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    var response = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiPostCall<StatusViewModel>(ApplicationConstants.UrlDeclineInvoice, new DeclineInvoiceModel() { UserContext = UserContext, Model = viewModel });
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Details", "Invoice", new { id = viewModel.DeclineReason.Id });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageDeclineInvoiceFailed);
                    }
                }
                return View("~/Areas/Buyer/Views/Invoice/Decline.cshtml", viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.Supplier, UserRoles.SupplierAdmin)]
        public async Task<ActionResult> Paid(InvoiceDetailViewModel viewModel)
        {
            using (var tracer = new Tracer("InvoiceBaseController", "Paid"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().PaidInvoiceAsync(viewModel, CurrentUser.Id);
                if (response.StatusCode == Status.Failed)
                {
                    DisplayCustomMessages((MessageType)response.StatusCode, Resource.errMessageInvoicePaidFailed);
                }
                return RedirectToAction("Details", "Invoice", new { id = viewModel.Invoice.Id });
            }
        }

        [AllowAnonymous]
        public async Task<bool> SendResaleCustomerNotification(int invoiceId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            var response = false;
            try
            {
                var invoicePdfModel = await ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>().ApiGetCall<InvoicePdfViewModel>(string.Format(ApplicationConstants.UrlGetInvoicePdfNewAsync, invoiceId, CompanyType.Supplier));
                if (invoicePdfModel.FuelRequest.Job.IsResaleEnabled &&
                    invoicePdfModel.FuelRequest.FuelRequestResale.ResaleCustomer.Count > 0 &&
                    (invoicePdfModel.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || invoicePdfModel.Invoice.InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp))
                {
                    byte[] content = GetPartialViewPdfContent("_PartialResaleDropTicket", invoicePdfModel);

                    var helperDomain = new HelperDomain();
                    var notificationDomain = new NotificationDomain(helperDomain);

                    var serverUrl = helperDomain.GetServerUrl();
                    var notificationViewModel = notificationDomain.GetInvoiceNotificationDetails(invoiceId);
                    var emailTemplate = helperDomain.GetApplicationEventNotificationTemplate();

                    string fileName = string.Format(Resource.lblDigitalDropTicket + ".pdf", DateTime.Now.ToString("dd_MMM_yy_hh:mm"));
                    System.Net.Mail.Attachment file = new System.Net.Mail.Attachment(new MemoryStream(content), fileName, "application/pdf");

                    var notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreated_Supplier, serverUrl, string.Empty);

                    var attachements = new List<System.Net.Mail.Attachment>();
                    if (file != null)
                    {
                        attachements.Add(file);
                    }

                    string bodyText = notification.BodyText;
                    foreach (var customer in notificationViewModel.ResaleCustomer)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                            customer.Name,
                                                            notificationViewModel.InvoiceNumber,
                                                            string.Concat(notificationViewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                                            notificationViewModel.DueDate);

                        var emailModel = new ApplicationEventNotificationViewModel
                        {
                            To = new List<string> { customer.Email },
                            Subject = notification.Subject,
                            CompanyLogo = notification.CompanyLogo,
                            BodyText = notification.BodyText,
                            Attachments = attachements,
                            ShowFooterContent = false,
                            ShowHelpLineInfo = false
                        };

                        response = await Email.GetClient().SendAsync(emailTemplate, emailModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceBaseController", "SendResaleCustomerNotification", ex.Message, ex);
            }
            return response;
        }

        [AllowAnonymous]
        public JsonResult GetPdfViewModelAsByetArray(int id, int companyType)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();

            var invoiceHeaderId = Task.Run(() => invoiceDomain.ApiGetCall<int>(string.Format(ApplicationConstants.UrlGetInvoiceHeaderIdById, id))).Result;
            var input = new InvoicePdfRequestModel() { InvoiceHeaderId = invoiceHeaderId, CompanyType = (CompanyType)companyType };
            var suppInvoicePdfModel = Task.Run(() => invoiceDomain.ApiPostCall<ConsolidatedInvoicePdfViewModel>(ApplicationConstants.UrlGetConsolidatedInvoicePdf, input)).Result;

            string partialViewName = "_PartialConsolidatedInvoicePdf";
            if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.TankRental || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.TankRental)))
            {
                partialViewName = "_PartialTankRentalInvoicePdf";
            }
            else if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.Balance || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.Balance)))
            {
                partialViewName = "_PartialBalanceInvoicePdf";
            }
            else if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
            {
                partialViewName = "_PartialDryRunInvoicePdf";
            }
            else if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t=>t.IsMarineLocation))
            {
                partialViewName = "_PartialConsolidatedMarineInvoicePdf";
            }

            var supplierPdfBytes = GetPartialViewPdfContent(partialViewName, suppInvoicePdfModel);
            return Json(supplierPdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetAndConvertInvoicePdfToDdtPdf(int id, int companyType)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();

            var invoiceHeaderId = Task.Run(() => invoiceDomain.ApiGetCall<int>(string.Format(ApplicationConstants.UrlGetInvoiceHeaderIdById, id))).Result;
            var input = new InvoicePdfRequestModel() { InvoiceHeaderId = invoiceHeaderId, CompanyType = (CompanyType)companyType };
            var suppInvoicePdfModel = Task.Run(() => invoiceDomain.ApiPostCall<ConsolidatedInvoicePdfViewModel>(ApplicationConstants.UrlGetConsolidatedInvoicePdf, input)).Result;

            suppInvoicePdfModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber = suppInvoicePdfModel.InvoicePdfHeaderDetail.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
            foreach (var invoice in suppInvoicePdfModel.Invoices)
            {
                invoice.DisplayInvoiceNumber = invoice.DisplayInvoiceNumber.Replace(ApplicationConstants.SFIN, ApplicationConstants.SFDD);
                invoice.InvoiceTypeId = invoice.InvoiceTypeId == (int)InvoiceType.Manual ? (int)InvoiceType.DigitalDropTicketManual : invoice.InvoiceTypeId;
                invoice.InvoiceTypeId = invoice.InvoiceTypeId == (int)InvoiceType.MobileApp ? (int)InvoiceType.DigitalDropTicketMobileApp : invoice.InvoiceTypeId;
            }

            var supplierPdfBytes = GetPartialViewPdfContent("_PartialConsolidatedInvoicePdf", suppInvoicePdfModel);
            return Json(supplierPdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetBDRPdfViewModelAsByteArray(int id, int companyType)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
            var input = new InvoicePdfRequestModel() { CompanyType = (CompanyType)companyType, InvoiceHeaderId = id };
            var suppInvoicePdfModel = Task.Run(() => invoiceDomain.ApiPostCall<BDRPdfViewModel>(ApplicationConstants.UrlGetBDRPdf, input)).Result;

            string partialViewName = "_PartialBDRPdf";

            var supplierPdfBytes = GetPartialViewPdfContent(partialViewName, suppInvoicePdfModel);
            return Json(supplierPdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetInvoiceSummaryPdfModelAsByteArray(int id)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();

            var suppInvoicePdfModel = Task.Run(() => invoiceDomain.ApiGetCall<InvoiceDetailViewModel>(string.Format(ApplicationConstants.UrlGetInvoiceDetailSummary, id))).Result;

            string partialViewName = "_PartialInvoiceDetailSummary";

            var supplierPdfBytes = GetPartialViewPdfContent(partialViewName, suppInvoicePdfModel);
            return Json(supplierPdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetStatementPdfViewModelAsByetArray(int statementId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();

            var suppStatementPdfModel = Task.Run(() => invoiceDomain.ApiGetCall<StatementPdfViewModel>(string.Format(ApplicationConstants.UrlGetStatementPdfDetails, statementId))).Result;
            var supplierPdfBytes = GetPartialViewPdfContent("~/Views/Shared/_PartialStatementPdf.cshtml", suppStatementPdfModel);
            return Json(supplierPdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeCompany(CompanyType.Supplier, CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "imageId", Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> DownloadSingleInvoiceImage(int imageId = 0)
        {
            using (var tracer = new Tracer("InvoiceController", "DownloadSingleInvoiceImage"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetImage(imageId);
                if (response != null)
                {
                    return Json(response.Item2, JsonRequestBehavior.AllowGet);
                }
                return new EmptyResult();
            }
        }

        [AuthorizeCompany(CompanyType.Supplier, CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        [OutputCache(Duration = int.MaxValue, VaryByParam = "id;imageId", Location = OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> DownloadInvoiceImages(int id, int imageId = 0)
        {
            using (var tracer = new Tracer("InvoiceController", "DownloadInvoiceImages"))
            {
                var response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetInvoiceImagesAsync(id, imageId);
                if (response.Count > 1)
                {
                    var fileName = string.Format("{0}.zip", response[0].Name);
                    MemoryStream outputStream = new MemoryStream();
                    outputStream.Seek(0, SeekOrigin.Begin);
                    using (ZipFile zip = new ZipFile())
                    {
                        foreach (var image in response)
                        {
                            if (zip.ContainsEntry(image.Name))
                            {
                                zip.AddEntry(string.Format("{0}{1}.png", image.Name, image.Id), image.Data);
                            }
                            else
                            {
                                zip.AddEntry(string.Format("{0}.png", image.Name), image.Data);
                            }
                        }
                        zip.Save(outputStream);
                    }
                    outputStream.WriteTo(Response.OutputStream);
                    Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
                    Response.ContentType = "application/zip";
                    return new FileStreamResult(outputStream, "application/zip");
                }
                else
                {
                    var fileName = string.Format("{0}.png", response[0].Name);
                    return File(response[0].Data, "image/png", fileName);
                }
            }
        }

        [HttpGet]
        [AuthorizeCompany(CompanyType.Supplier, CompanyType.Buyer, CompanyType.BuyerAndSupplier)]
        public async Task<JsonResult> GetCheckoutContent(int headerId, int supplierCompanyId)
        {
            var punchooutDomain = new PunchoutOrderMessageDomain();
            var response = await punchooutDomain.GetPunchoutOrderMessageXmlString(UserContext, headerId, supplierCompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveInvoicePdfEmailNotificationDetails(InvoiceFilterViewModel model)
        {
            using (var tracer = new Tracer("InvoiceController", "SaveInvoicePdfEmailDetails"))
            {
                StatusViewModel response;
                model.InvoiceAttachment.OrderId = model.OrderId;
                model.InvoiceAttachment.PoNumber = model.PoNumber;
                //model.InvoiceAttachment.CompanyType = CompanyType.Supplier;

                response = await ContextFactory.Current.GetDomain<InvoiceDomain>().SaveInvoicePdfEmailNotificationDetails(model.InvoiceAttachment, UserContext);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult GetMobileAppPdfViewModelAsByteArray(int invoiceHeaderId, int companyType)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();

            // var invoiceHeaderId = Task.Run(() => invoiceDomain.GetInvoiceHeaderIdByIdAsync(id)).Result;
            var input = new InvoicePdfRequestModel() { InvoiceHeaderId = invoiceHeaderId, CompanyType = (CompanyType)companyType };
            var suppInvoicePdfModel = Task.Run(() => invoiceDomain.ApiPostCall<ConsolidatedInvoicePdfViewModel>(ApplicationConstants.UrlGetConsolidatedInvoicePdf, input)).Result;

            string partialViewName = "_PartialConsolidatedInvoicePdf";
            if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.TankRental || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.TankRental)))
            {
                partialViewName = "_PartialTankRentalInvoicePdf";
            }
            else if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.Balance || ((t.InvoiceTypeId == (int)InvoiceType.CreditInvoice || t.InvoiceTypeId == (int)InvoiceType.PartialCredit) && t.OriginalInvoiceType == (int)InvoiceType.Balance)))
            {
                partialViewName = "_PartialBalanceInvoicePdf";
            }
            else if (suppInvoicePdfModel.Invoices.Any() && suppInvoicePdfModel.Invoices.Any(t => t.InvoiceTypeId == (int)InvoiceType.DryRun))
            {
                partialViewName = "_PartialDryRunInvoicePdf";
            }

            var supplierPdfBytes = GetPartialViewPdfContent(partialViewName, suppInvoicePdfModel);
            return Json(supplierPdfBytes, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public JsonResult GetMarineTaxAffidavitPdfViewModelAsByetArray(int invoiceHeaderId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
            var PdfModel = Task.Run(() => invoiceDomain.ApiGetCall<MarineTaxAffidavitPdfViewModel>(string.Format(ApplicationConstants.UrlGetMarineTaxAffidavitInfo, invoiceHeaderId))).Result;
            PdfModel.IsFromDownloadPdf = true;

            string partialViewName = "~/Views/Shared/_PartialMarineTaxAffidavitDetails.cshtml";
            var pdfBytes = GetPartialViewPdfContent(partialViewName, PdfModel);
            return Json(pdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetMarineBDNPdfViewModelAsByetArray(int invoiceHeaderId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
            var PdfModel = Task.Run(() => invoiceDomain.ApiGetCall<MarineBDNPdfViewModel>(string.Format(ApplicationConstants.UrlGetMarineBDNInfo, invoiceHeaderId))).Result;
            PdfModel.IsFromDownloadPdf = true;

            string partialViewName = "~/Views/Shared/_PartialMarineBDNDetails.cshtml";
            var pdfBytes = GetPartialViewPdfContent(partialViewName, PdfModel);
            return Json(pdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetMarineCGInspPdfViewModelAsByetArray(int invoiceHeaderId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
            var PdfModel = Task.Run(() => invoiceDomain.ApiGetCall<MarineCGInspectionDocumentPdfViewModel>(string.Format(ApplicationConstants.UrlGetMarineCGInspectionDocumentInfo, invoiceHeaderId))).Result;
            PdfModel.IsFromDownloadPdf = true;
            string partialViewName = "~/Views/Shared/_PartialCGInspectionDocument.cshtml";
            var pdfBytes = GetPartialViewPdfContent(partialViewName, PdfModel);
            return Json(pdfBytes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetMarineInspRequestVoucherViewModelsAsByetArray(int invoiceHeaderId)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InvoiceServiceApiDomain invoiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
            var PdfModel = Task.Run(() => invoiceDomain.ApiGetCall<List<MarineInspectionRequestVoucherViewModel>>(string.Format(ApplicationConstants.UrlGetMarineInspectionVoucher, invoiceHeaderId))).Result;
            if (PdfModel != null && PdfModel.Any())
            {
                PdfModel.ForEach(t => t.IsFromDownloadPdf = true);
                string partialViewName = "~/Views/Shared/_PartialInspRequestVoucherDownload.cshtml";
                var pdfBytes = GetPartialViewPdfContent(partialViewName, PdfModel);
                return Json(pdfBytes, JsonRequestBehavior.AllowGet);
            }
            else { return null; }


        }

        //[HttpGet]
        //[AuthorizeCompany(CompanyType.Supplier, CompanyType.SupplierAndCarrier, CompanyType.BuyerSupplierAndCarrier, CompanyType.BuyerAndSupplier)]
        //public async Task<JsonResult> GetBolListForInvoiceHeader(int headerId)
        //{
        //    List<DropdownDisplayItem> response = await ContextFactory.Current.GetDomain<InvoiceDomain>().GetBOlListForInvoiceHeader(headerId);
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}


        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> GetMarineInvoiceBolListAsync(int headerId)
        {
            var invoiceServiceDomain = ContextFactory.Current.GetDomain<InvoiceServiceApiDomain>();
            var response = await invoiceServiceDomain.ApiGetCall<List<InvoiceBolEditGrid>>(string.Format(ApplicationConstants.UrlGetMarineInvoiceBolList, CurrentUser.CompanyId, headerId));           
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}