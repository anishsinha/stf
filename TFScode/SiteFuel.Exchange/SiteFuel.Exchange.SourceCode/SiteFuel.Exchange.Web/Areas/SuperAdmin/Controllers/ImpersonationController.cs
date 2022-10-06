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
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Web.Areas.SuperAdmin.Controllers
{
    public class ImpersonationController : BaseController
    {
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult ImpersonationHistory()
        {
            return View();
        }

        [AuthorizeRole(UserRoles.SuperAdmin,UserRoles.AccountSpecialist)]
        public ActionResult ActiveImpersonations()
        {
            return View();
        }


        [AuthorizeRole(UserRoles.SuperAdmin)]
        public ActionResult ImpersonationActivityLog()
        {
            return View();
        }

        [AuthorizeRole(UserRoles.SuperAdmin,UserRoles.AccountSpecialist)]
        public ActionResult GetImpersonations()
        {
            using (var tracer = new Tracer("ImpersonationController", "GetImpersonations"))
            {
                var response = ContextFactory.Current.GetDomain<SuperAdminDomain>().GetImpersonations();
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin, UserRoles.AccountSpecialist)]
        public async Task<ActionResult> RemoveImpersonation(int id)
        {
            using (var tracer = new Tracer("ImpersonationController", "RemoveImpersonation"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().RemoveImpersonation(id, CurrentUser.Id);
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                return RedirectToAction("ActiveImpersonations", "Impersonation", new { area = "SuperAdmin" });
            }
        }

        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> GetImpersonationHistory(string fromDate, string toDate, int ImpersonatedBy = 0)
        {
            using (var tracer = new Tracer("ImpersonationController", "GetImpersonationHistory"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetImpersonationHistoryAsync(ImpersonatedBy, fromDate, toDate);
                return new JsonResult
                {
                    Data = response,
                    MaxJsonLength = int.MaxValue,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.SuperAdmin)]
        public async Task<ActionResult> GetImpersonationActivityLog(ImpersonateLogDataTableViewModel requestModel)
        {
            using (var tracer = new Tracer("ImpersonationController", "GetImpersonationActivityLog"))
            {
                var response = await ContextFactory.Current.GetDomain<SuperAdminDomain>().GetImpersonationActivityLogAsync(requestModel);
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