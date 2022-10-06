using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using SiteFuel.Exchange.Core.Logger;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    public class ReportController : BaseController
    {
        [HttpGet]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public ActionResult InvoiceReport()
        {
            return View("InvoiceReportSummary");
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.Buyer, UserRoles.BuyerAdmin, UserRoles.AccountingPerson, UserRoles.ReportingPerson)]
        public JsonResult GetJobList(List<int> supplierCompanyIds)
        {
            using (var tracer = new Tracer("ReportController", "GetJobList"))
            {
                var response = ContextFactory.Current.GetDomain<ReportDomain>().GetJobsForSelectedSupplier(supplierCompanyIds, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}