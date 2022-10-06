using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    [AuthorizeRole(UserRoles.SuperAdmin,UserRoles.AccountSpecialist)]
    public class DashboardController : BaseController
    {
        // GET: SuperAdmin/Dashboard
        public ActionResult Index()
        {
             return View();
        }

        public async Task<JsonResult> GetSuperAdminCount()
        {
            using (var tracer = new Tracer("DashboardController", "GetSuperAdminCount"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSuperAdminCountAsync();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetSuperAdminCompanyCount()
        {
            using (var tracer = new Tracer("DashboardController", "GetSuperAdminCompanyCount"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSuperAdminCompanyCountAsync();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetCompanyUsersCount(int SelectedCompanyId = 0)
        {
            using (var tracer = new Tracer("DashboardController", "GetCompanyUsersCount"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetCompanyUsersCountAsync(SelectedCompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetSuperAdminAdWidgetCount()
        {
            using (var tracer = new Tracer("DashboardController", "GetSuperAdminAdWidgetCount"))
            {
                var response = await ContextFactory.Current.GetDomain<DashboardDomain>().GetSuperAdminAdWidgetCountAsync();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTotalGallonsOrdered(string startDate, string endDate, int selectedCompany = 0)
        {
            using (var tracer = new Tracer("DashboardController", "GetTotalGallonsOrdered"))
            {
                var response = ContextFactory.Current.GetDomain<DashboardDomain>().GetGallonsOrderedCount(startDate, endDate, selectedCompany);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTotalGallonsDelivered(string startDate, string endDate, int selectedCompany = 0)
        {
            using (var tracer = new Tracer("DashboardController", "GetTotalGallonsDelivered"))
            {
                var response = ContextFactory.Current.GetDomain<DashboardDomain>().GetGallonsDeliveredCount(startDate, endDate, selectedCompany);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetCompanies()
        {
            using (var tracer = new Tracer("DashboardController", "GetCompanies"))
            {
                var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompanies();
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}