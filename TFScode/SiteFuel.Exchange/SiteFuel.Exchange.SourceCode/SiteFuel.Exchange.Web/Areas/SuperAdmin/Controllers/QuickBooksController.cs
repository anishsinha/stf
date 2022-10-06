using System.Threading.Tasks;
using System.Web.Mvc;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.ViewModels.Quickbooks;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
    public class QuickBooksController : BaseController
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
    }
}
