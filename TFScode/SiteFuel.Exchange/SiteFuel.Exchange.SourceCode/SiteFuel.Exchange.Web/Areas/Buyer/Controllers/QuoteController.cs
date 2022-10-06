using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Infrastructure;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class QuoteController : BaseController
    {
        [HttpGet]
        [ActionName("View")]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public ActionResult Index(int jobId = 0, QuoteRequestFilterType filter = QuoteRequestFilterType.All)
        {
            using (var tracer = new Tracer("OrderController", "Orders"))
            {
                var response = ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetQuoteRequestFilter(jobId, filter);
                return View("QuoteRequests", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> QuoteRequestGrid(QuoteRequestDataTableModel requestModel)
        {
            using (var tracer = new Tracer("QuoteController", "QuoteRequestGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetAllQuoteRequestsBuyerAsync(UserContext, requestModel.StartDate, requestModel.EndDate, requestModel.JobId, requestModel.Filter, requestModel.CountryId, requestModel.Currency);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public ActionResult QuoteRequestsByJob(int jobId = 0)
        {
            return PartialView("_PartialQuoteRequestsGrid", new QuoteRequestDataTableModel { JobId = jobId, Filter = QuoteRequestFilterType.All });
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Create(int jobId = 0)
        {
            if (jobId == 0)
            {
                RemoveReturnUrl();
            }
            var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetQuoteRequestAsync(jobId);
            return View(response);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Edit(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetQuoteRequestDetailsAsync(id, UserContext);
                response.IsEdit = true;
                return View("details", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<JsonResult> Award(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Award"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().AwardQuote(id, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<JsonResult> Exclude(int quotationId)
        {
            using (var tracer = new Tracer("QuoteController", "Exclude"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().ExcludeQuote(quotationId, UserContext);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Create(QuoteRequestViewModel quoteRequestViewModel, HttpPostedFileBase[] files = null)
        {
            using (var tracer = new Tracer("QuoteController", "Create"))
            {
                if (ModelState.IsValid)
                {
                    var quoteRequestDomain = new QuoteRequestDomain();
                    quoteRequestViewModel.CreatedBy = UserContext.Id;
                    var quoteRequest = await quoteRequestDomain.SaveQuoteRequestAsync(UserContext, quoteRequestViewModel);
                    if (quoteRequest.StatusCode == Status.Success)
                    {
                        if (files != null && files.Any())
                        {
                            foreach (HttpPostedFileBase item in files.Where(t => t != null))
                            {
                                await quoteRequestDomain.UploadQuoteDocumentsToBlob(UserContext, BlobContainerType.QuoteRequest, quoteRequest.Id, item.InputStream, item.FileName);
                            }
                        }
                        DisplayCustomMessages((MessageType)quoteRequest.StatusCode, quoteRequest.StatusMessage);
                        return RedirectToAction("Details", "Quote", new { area = "Buyer", id = quoteRequest.Id });
                    }
                    else
                    {
                        DisplayCustomMessages((MessageType)quoteRequest.StatusCode, quoteRequest.StatusMessage);
                    }
                }
            }
            return View(quoteRequestViewModel);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Edit(QuoteRequestDetailsViewModel quoteRequestViewModel, HttpPostedFileBase[] files = null)
        {
            using (var tracer = new Tracer("QuoteController", "Edit"))
            {
                if (ModelState.IsValid)
                {
                    var quoteRequestDomain = new QuoteRequestDomain();

                    quoteRequestViewModel.UpdatedBy = UserContext.Id;
                    var quoteRequests = await quoteRequestDomain.UpdateQuoteRequestAsync(UserContext, quoteRequestViewModel);
                    if (quoteRequests.StatusCode == Status.Success)
                    {
                        if (files != null)
                        {
                            foreach (HttpPostedFileBase item in files.Where(t => t != null))
                            {
                                await quoteRequestDomain.UploadQuoteDocumentsToBlob(UserContext, BlobContainerType.QuoteRequest, quoteRequestViewModel.Id, item.InputStream, item.FileName);
                            }
                        }
                        DisplayCustomMessages((MessageType)quoteRequests.StatusCode, quoteRequests.StatusMessage);
                    }
                    return RedirectToAction("Details", new { id = quoteRequestViewModel.Id });
                }
                return View(quoteRequestViewModel);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> Details(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Details"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetQuoteRequestDetailsAsync(id, UserContext);
                return View(response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> SupplierQuoteDetails(int id)
        {
            using (var tracer = new Tracer("QuoteController", "SupplierQuoteDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetSupplierQuoteDetailsAsync(id);
                return View(response);
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<JsonResult> GetNewsfeed(int entityId, int currentPage, int latestId = 0)
        {
            var response = await ContextFactory.Current.GetDomain<NewsfeedDomain>().GetNewsfeed(UserContext, EntityType.QuoteRequest, entityId, currentPage, latestId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Cancel(int id)
        {
            using (var tracer = new Tracer("QuoteController", "Cancel"))
            {
                await ContextFactory.Current.GetDomain<QuoteRequestDomain>().UpdateQuoteRequestStatus(UserContext.Id, id, QuoteRequestStatuses.Canceled);
                return RedirectToAction("Details", new { id = id });
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<ActionResult> QuotationsGrid(int id)
        {
            using (var tracer = new Tracer("QuoteController", "QuotationsGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetAllQuotationsAsync(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<PartialViewResult> QuotesTileView(int id)
        {
            using (var tracer = new Tracer("QuoteController", "QuotesTileView"))
            {
                var response = new QuoteRequestTileViewModel();
                var quotations = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetAllQuotationsAsync(id);
                if (quotations.Any())
                {
                    response.QuoteRequestId = quotations.FirstOrDefault().CustomerQuoteRequestId;
                    response.Quotations = quotations;
                }
                return PartialView("QuotesTileView", response);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public PartialViewResult QuoteListView(int id)
        {
            return PartialView("_PartialQuotationsGrid", id);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<PartialViewResult> SupplierQuoteDetail(int id)
        {
            var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetSupplierQuoteDetailsAsync(id);
            return PartialView("SupplierQuoteDetails", response);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<PartialViewResult> SupplierQuoteDetailView(int id)
        {
            var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetSupplierQuoteDetailsAsync(id);
            return PartialView("_PartialQuoteDetails", response);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public async Task<PartialViewResult> SupplierQuotePDFView(int id)
        {
            var response = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetSupplierQuoteDetailsAsync(id);
            return PartialView("_PartialQuotePDFDetail", response);
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task UpdateQuotesPriority(int quoteReguestId, int[] ids)
        {
            using (var tracer = new Tracer("QuoteController", "UpdateQuotesPriority"))
            {
                await ContextFactory.Current.GetDomain<QuoteRequestDomain>().UpdateQuotesPriorityAsync(quoteReguestId, ids);
            }
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> RemoveDocument(int id, int quoteRequestId)
        {
            using (var tracer = new Tracer("QuoteController", "RemoveDocument"))
            {
                await ContextFactory.Current.GetDomain<QuoteRequestDomain>().RemoveDocument(UserContext, id);
                return RedirectToAction("Edit", new { id = quoteRequestId });
            }
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
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