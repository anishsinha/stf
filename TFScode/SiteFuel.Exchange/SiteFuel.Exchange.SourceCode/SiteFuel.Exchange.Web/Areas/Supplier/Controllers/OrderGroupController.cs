using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    public class OrderGroupController : OrderBaseController
    {
        [ActionName("View")]
        public ActionResult Index()
        {
            ViewBag.Title = "View Groups";
            return View();
        }


        public async Task<JsonResult> GetCustomersForSupplier()
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetCustomersForSupplier(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetJobsForCustomer(int buyerCompanyId)
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetJobsForCustomer(UserContext.CompanyId, buyerCompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult TermPricingContract()
        {
            return View("CreateTermPriceContract");
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier)]
        public ActionResult CreateSameDestGroup()
        {
            return View("CreateBlendGroup");
        }
    }
}