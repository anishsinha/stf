using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class BillingStatementController : BaseController
    {
        // GET: Supplier/BillingStatement
        [ActionName("View")]
        public ActionResult Index()
        {
            using (var tracer = new Tracer("BillingStatementController", "Index"))
            {
                StatementSummaryFilterViewModel filter = new StatementSummaryFilterViewModel();
                return View("View", filter);
            }
        }

        [HttpPost]
        public async Task<ActionResult> StatementGrid(StatementSummaryDataViewModel stmtModel)
        {
            using (var tracer = new Tracer("BillingStatementController", "StatementGrid"))
            {
                var response = await ContextFactory.Current.GetDomain<BillingStatementDomain>().GetStatementSummary(CurrentUser.Id, CurrentUser.CompanyId, stmtModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;

                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = stmtModel.draw,
                        data = response,
                        recordsTotal = totalCount,
                        recordsFiltered = totalCount
                    },

                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        public async Task<PartialViewResult> DetailsTab(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "DetailsTab"))
            {
                var response = await ContextFactory.Current.GetDomain<BillingStatementDomain>().GetStatementDetailsAsync(id, UserContext);
                return PartialView("_PartialTabStatementDetails", response);
            }
        }

        public async Task<ActionResult> StatementInvoices(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "StatementInvoices"))
            {
                var response = await ContextFactory.Current.GetDomain<BillingStatementDomain>().GetStatementInvoicesAsync(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "Details"))
            {
                BillingStatementDetailsViewModel response = new BillingStatementDetailsViewModel() { Id = id };
                return View(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StatementEditViewModel viewModel)
        {
            using (var tracer = new Tracer("BillingStatementController", "Edit"))
            {
                var response = await ContextFactory.Current.GetDomain<BillingStatementDomain>().EditStatementAsync(CurrentUser.Id,viewModel);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return Json(viewModel.StatementId, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult HistoryTab(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "HistoryTab"))
            {
                return PartialView("_PartialTabStatementHistory", id);
            }
        }

        public async Task<ActionResult> StatementHistory(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "StatementHistory"))
            {
                var response = await ContextFactory.Current.GetDomain<BillingStatementDomain>().GetStatementHistoryAsync(id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetStatementPdfDetails(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "GetStatementPdfDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<BillingStatementDomain>().GetStatementPdfDetailsAsync(id);
                return PartialView("~/Views/Shared/_PartialStatementPdf.cshtml", response);
            }
        }

        public ActionResult DownloadPdf(int id)
        {
            using (var tracer = new Tracer("BillingStatementController", "DownloadPdf"))
            {
                var invoicePdfModel = Task.Run(() => ContextFactory.Current.GetDomain<BillingStatementDomain>().GetStatementPdfDetailsAsync(id)).Result;
                invoicePdfModel.PhoneNumber = invoicePdfModel.PhoneNumber.ToFormattedPhoneNumber();
                invoicePdfModel.ContactPerson.PhoneNumber = invoicePdfModel.ContactPerson.PhoneNumber.ToFormattedPhoneNumber();

                return GetPartialViewAsPdf("~/Views/Shared/_PartialStatementPdf.cshtml", invoicePdfModel.StatementNumber, invoicePdfModel);
            }
        }

        [HttpGet]
        public ActionResult GetStatementSummary(int countryId = (int)Country.USA)
        {
            var viewModel = new StatementSummaryFilterViewModel();
            return PartialView("_PartialBillingStatement", viewModel);
        }
    }
}