using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Supplier.Controllers
{
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class SupplierBaseController : BaseController
    {
        [HttpGet]
        public JsonResult GetCarriers(string Prefix)
        {
            var Countries = ContextFactory.Current.GetDomain<DispatchDomain>().GetCarriers(Prefix);
            return Json(Countries, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSupplierSource(string Prefix)
        {
            var Countries = ContextFactory.Current.GetDomain<DispatchDomain>().GetSupplierSource(Prefix);
            return Json(Countries, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetBulkPlants(string Prefix, int orderId = 0)
        {
            var bulkPlants = Task.Run(() => ContextFactory.Current.GetDomain<DispatchDomain>().GetBulkPlants(Prefix, CurrentUser.CompanyId, orderId)).Result;
            return Json(bulkPlants, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBulkPlantDetails(string name)
        {
            var bulkPlantdetails = ContextFactory.Current.GetDomain<DispatchDomain>().GetBulkPlantDetailsByName(name, CurrentUser.CompanyId);
            return Json(bulkPlantdetails, JsonRequestBehavior.AllowGet);
        }
    }
}