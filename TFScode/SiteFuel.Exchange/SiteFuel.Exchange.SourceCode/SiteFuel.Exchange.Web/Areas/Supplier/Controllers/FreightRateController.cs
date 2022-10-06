using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Web.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class FreightRateController : BaseController
    {

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public JsonResult GetFreightRateRuleTypes()
        {
            var response = CommonHelperMethods.GetFreightRateRuleTypes();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetFreightRateSummary(ViewFreightRateInputViewModel input)
        {
            using (var tracer = new Tracer("FreightRateController", "GetFreightRateSummary"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightRateDomain>().GetFreightRateSummary(input, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFreightRateDetails(int freightRateId)
        {
            using (var tracer = new Tracer("FreightRateController", "GetFreightRateDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightRateDomain>().GetFreightRateDetails(freightRateId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFreightRateTableForView(int freightRateId)
        {
            using (var tracer = new Tracer("FreightRateController", "GetFreightRateTableForView"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightRateDomain>().GetFreightRateTableForView(freightRateId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ArchiveFreightRate(string freightRateId)
        {
            using (var tracer = new Tracer("FreightRateController", "ArchiveFreightRate"))
            {
                var response = await ContextFactory.Current.GetDomain<FreightRateDomain>().ArchiveFreightRate(int.Parse(freightRateId), CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateFreightRate(FreightRateViewModel model)
        {
            StatusViewModel response = await ContextFactory.Current.GetDomain<FreightRateDomain>().GetExistingFreightRate(model, UserContext.CompanyId);
            if (response == null)
            {
                response = await ContextFactory.Current.GetDomain<FreightRateDomain>().CreateFreightRate(model, CurrentUser.CompanyId, CurrentUser.Id);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateFreightRate(FreightRateViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<FreightRateDomain>().UpdateFreightRate(model, CurrentUser.CompanyId, CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetCustomerJobs(int customerId)
        {
            var response = await ContextFactory.Current.GetDomain<HelperDomain>().GetCustomerJobs(customerId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}