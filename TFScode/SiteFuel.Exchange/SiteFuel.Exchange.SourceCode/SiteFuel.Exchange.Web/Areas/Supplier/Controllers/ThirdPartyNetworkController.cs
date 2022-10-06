using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.BillingStatement;
using SiteFuel.Exchange.ViewModels.FuelSurcharge;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier)]
    public class ThirdPartyNetworkController : BaseController
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        public async Task<ActionResult> GetInvitationTokenByCompany()
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetInvitationTokenByCompany(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GenerateInvitationToken()
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GenerateInvitationToken(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetNonRegisteredInvitedCompanies(ThirdPartyCompanyFilter filter = null)
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetNonRegisteredInvitedCompanies(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetRegisteredInvitedCompanies(ThirdPartyCompanyFilter filter = null)
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetRegisteredInvitedCompanies(UserContext.CompanyId, filter);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //
        [HttpGet]
        public async Task<ActionResult> GetNonRegisteredInvitedCompany(int entityId)
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetNonRegisteredInvitedCompany(entityId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetRegisteredInvitedCompany(int companyId)
        {
            var response = await ContextFactory.Current.GetDomain<InvitationDomain>().GetRegisteredInvitedCompany(companyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}