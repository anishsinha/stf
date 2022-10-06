using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class OrderGroupController : BaseController
    {
        [ActionName("View")]
        public ActionResult Index()
        {
            ViewBag.Title = "View Groups";
            return View();
        }

        public async Task<JsonResult> GetSuppliersForCustomer()
        {
            var response = await ContextFactory.Current.GetDomain<OrderGroupDomain>().GetSuppliersForCustomer(UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public ActionResult TermPricingContract()
        {
            return View("CreateTermPriceContract");
        }
    }
}