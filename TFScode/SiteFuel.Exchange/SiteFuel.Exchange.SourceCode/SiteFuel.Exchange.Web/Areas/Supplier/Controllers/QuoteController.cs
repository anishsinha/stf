using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class QuoteController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public ActionResult Index(QuoteRequestFilterType filter = QuoteRequestFilterType.All)
        {
            using (var tracer = new Tracer("QuoteController", "View"))
            {
                var model = new QuoteRequestFilterViewModel();
                model.filter = filter;
                return View("QuoteRequests", model);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> QuoteRequestGrid(QuoteRequestDataTableModel requestModel)
        {
            using (var tracer = new Tracer("QuoteController", "QuoteRequestGrid"))
            {
                var quoteRequestStat = new USP_SupplierRequestsViewModel()
                {
                    CompanyId = UserContext.CompanyId,
                    UserId = CurrentUser.Id,
                    CountryId = requestModel.CountryId,
                    CurrencyType = (int)requestModel.Currency,
                    dataTableSearchValues = null,
                    StatusFilter = (int)requestModel.Filter
                };
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetAllQuoteRequestsSupplierAsync(quoteRequestStat, requestModel.StartDate, requestModel.EndDate);
                return Json(response.RecentQuoteRequests, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> QuotationsGrid(QuoteRequestDataTableModel responseModel)
        {
            using (var tracer = new Tracer("QuoteController", "QuotationsGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetAllQuotationsSupplierAsync(CurrentUser.Id, responseModel);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetSupplierQuoteDetailsAsync(id);
                return View(response);
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> BuyerQuoteDetails(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetQuoteRequestDetailsAsync(id, UserContext);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Create(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Create"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetQuoteResponseAsync(id, UserContext);
                return View(response);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> Create(QuoteResponseViewModel quoteResponseViewModel, HttpPostedFileBase[] files = null)
        {
            using (var tracer = new Tracer("QuoteController", "Create"))
            {
                if (ModelState.IsValid)
                {
                    var quoteRequestDomain = new QuoteRequestDomain();
                    quoteResponseViewModel.CreatedBy = UserContext.Id;
                    var response = await quoteRequestDomain.SaveQuotationAsync(UserContext, quoteResponseViewModel);
                    if (response.StatusCode == Status.Success && files != null && files.Any())
                    {
                        foreach (HttpPostedFileBase item in files.Where(t => t != null))
                        {
                            await quoteRequestDomain.UploadQuoteDocumentsToBlob(UserContext, BlobContainerType.Quotation, response.QuotationId, item.InputStream, item.FileName);
                        }
                    }
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    return RedirectToAction("View");
                }
                return View(quoteResponseViewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult DeliveryFeeByQuantity(string prefix, Currency currency, UoM uoM)
        {
            var deliveryFeeByQuantityModel = new DeliveryFeeByQuantityViewModel();
            deliveryFeeByQuantityModel.CollectionHtmlPrefix = prefix;
            deliveryFeeByQuantityModel.Currency = currency;
            deliveryFeeByQuantityModel.UoM = uoM;

            return PartialView("~/Views/Shared/_PartialDeliveryFeeByQuantity.cshtml", deliveryFeeByQuantityModel);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> DeclineQuoteRequest(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Decline"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().DeclineQuoteRequest(id, CurrentUser.Id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("View", "Quote", new { area = "Supplier" });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public async Task<ActionResult> RemoveDocument(int id, int quoteRequestId)
        {
            using (var tracer = new Tracer("QuoteController", "RemoveDocument"))
            {
                await ContextFactory.Current.GetDomain<QuoteRequestDomain>().RemoveDocument(UserContext, id);
                return RedirectToAction("Edit", new { id = quoteRequestId });
            }
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public FileContentResult DownloadDocument(string fileName, string container)
        {
            var azureBlob = new AzureBlobStorage();
            var fileStream = azureBlob.DownloadBlob(fileName, container);
            var memoryStream = fileStream as MemoryStream;
            var fileExtension = fileName.Split('.').LastOrDefault();
            string mimeType = MediaType.Pdf;

            switch (fileExtension)
            {
                case "csv": mimeType = MediaType.Text; break;
                case "pdf": mimeType = MediaType.Pdf; break;
                case "doc": mimeType = MediaType.Doc; break;
                case "docx": mimeType = MediaType.Docx; break;
                case "bmp": mimeType = MediaType.Bmp; break;
                case "xls": mimeType = MediaType.Xls; break;
                case "xlsx": mimeType = MediaType.Xlsx; break;
                case "png": mimeType = MediaType.Png; break;
                case "jpeg": mimeType = MediaType.Jpeg; break;
                case "jpg": mimeType = MediaType.Jpg; break;
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName);
            return File(memoryStream.ToArray(), mimeType);
        }
    }
}