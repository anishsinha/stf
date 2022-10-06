using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using SiteFuel.Exchange.Core.Logger;
using System.Web.Mvc;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class YourCustomerController : BaseController
    {
        [HttpGet]
        public async Task<JsonResult> GetBillingAddressSummary(int companyId)
        {
            using (var tracer = new Tracer("YourCustomerController", "GetBillingAddressSummary"))
            {
                var response = await ContextFactory.Current.GetDomain<YourCustomerDomain>().GetBillingAddressSummary(companyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetBillingAddressDetails(int billingAddressId)
        {
            using (var tracer = new Tracer("YourCustomerController", "GetBillingAddressDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<YourCustomerDomain>().GetBillingAddressDetails(billingAddressId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateBillingAddress(BillingAddressViewModel model)
        {
            using (var tracer = new Tracer("YourCustomerController", "CreateBillingAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<YourCustomerDomain>().CreateBillingAddress(model, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateBillingAddress(BillingAddressViewModel model)
        {
            using (var tracer = new Tracer("YourCustomerController", "UpdateBillingAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<YourCustomerDomain>().UpdateBillingAddress(model, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteBillingAddress(int billingAddressId)
        {
            using (var tracer = new Tracer("YourCustomerController", "DeleteBillingAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<YourCustomerDomain>().DeleteBillingAddress(billingAddressId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SetDefaultBillingAddress(int billingAddressId,int companyId)
        {
            using (var tracer = new Tracer("YourCustomerController", "SetDefaultBillingAddress"))
            {
                var response = await ContextFactory.Current.GetDomain<YourCustomerDomain>().SetDefaultBillingAddress(billingAddressId, companyId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
    }
}