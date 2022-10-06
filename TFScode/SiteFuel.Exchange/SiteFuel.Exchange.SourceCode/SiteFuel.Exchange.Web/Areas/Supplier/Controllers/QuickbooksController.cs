using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Quickbooks;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class QuickbooksController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("QuickBooks");
        }

        [HttpPost]
        public async Task<ActionResult> GetQuickBooksGrid(QbDataTableModel qbModel)
        {
            using (var tracer = new Tracer("QuickBooksController", "GetQuickBooksGrid"))
            {
                var dataTableSearchModel = new DataTableSearchModel(qbModel);
                var qbDomain = ContextFactory.Current.GetDomain<QbDomain>();
                qbModel.CompanyId = CurrentUser.CompanyId;
                var response = await qbDomain.GetQuickBooksSummaryAsync(qbModel, UserContext.Roles[0], dataTableSearchModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = qbModel.draw,
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
        public async Task<ActionResult> GetQuickBooksReport(QbDataTableModel qbModel)
        {
            using (var tracer = new Tracer("QuickBooksController", "GetQuickBooksReport"))
            {
                var dataTableSearchModel = new DataTableSearchModel(qbModel);
                var qbDomain = ContextFactory.Current.GetDomain<QbDomain>();
                qbModel.CompanyId = CurrentUser.CompanyId;
                var response = await qbDomain.GetQuickBooksReportAsync(qbModel, dataTableSearchModel);
                var totalCount = 0;

                if (response.Count > 0)
                    totalCount = response[0].TotalCount;
                return new JsonResult
                {
                    Data = new DatatableResponse()
                    {
                        draw = qbModel.draw,
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
        public ActionResult SyncReport()
        {
            return PartialView("SyncReport");
        }

        [HttpPost]
        public async Task<ActionResult> SyncReport(DateTimeOffset reportDate)
        {
            var qbReportDomain = new QbReportDomain();
            var reportViewModel = await qbReportDomain.GetQuickBooksSyncReportViewModel(UserContext.CompanyId, reportDate);
            reportViewModel.ShowLoginNote = false;
            if (reportViewModel.Report == null)
            {
                reportViewModel.Report = new QbSyncReport();
            }
            return PartialView("_PartialQbSyncReport", reportViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> EmailSyncReport(DateTimeOffset reportDate)
        {
            var response = new StatusViewModel();
            var qbReportDomain = new QbReportDomain();
            var reportViewModel = await qbReportDomain.GetQuickBooksSyncReportViewModel(UserContext.CompanyId, reportDate);
            if (reportViewModel.Report != null)
            {
                response = await qbReportDomain.SendQbSyncReportEmailAsync(reportViewModel, new List<string>() { CurrentUser.Email });
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }
    }
}