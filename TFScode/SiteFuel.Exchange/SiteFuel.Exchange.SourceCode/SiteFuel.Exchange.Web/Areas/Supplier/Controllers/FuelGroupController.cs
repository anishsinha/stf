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
    public class FuelGroupController : BaseController
    {
        #region Fuel Group related implementation 

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetFuelGroupSummary()
        {
            using (var tracer = new Tracer("FuelGroupController", "GetFuelGroupSummary"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().GetFuelGroupSummary(CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetFuelGroupDetails(int fuelGroupId)
        {
            using (var tracer = new Tracer("FuelGroupController", "GetFuelGroupDetails"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().GetFuelGroupDetails(fuelGroupId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ArchiveFuelGroup(string fuelGroupId)
        {
            using (var tracer = new Tracer("FuelGroupController", "ArchiveAccessorialFee"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().ArchiveFuelGroup(int.Parse(fuelGroupId), CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateFuelGroup(FuelGroupViewModel model)
        {
            StatusViewModel response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().GetExistingFuelGroup(model, UserContext.CompanyId);
            if (response == null)
            {
                 response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().CreateFuelGroup(model, CurrentUser.CompanyId, CurrentUser.Id);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateFuelGroup(FuelGroupViewModel model)
        {
            var response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().UpdateFuelGroup(model, CurrentUser.CompanyId, CurrentUser.Id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetFuelTypes(string productTypeIds,string fuelGroupType, int editingGroupId , int editingcompanyId)
        {
            var response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().GetFuelTypes(productTypeIds, fuelGroupType, editingGroupId, editingcompanyId, CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetProductTypes()
        {
            List<int> exlcudedproductTypeIds = new List<int>() { (int)ProductTypes.PremiumGas };
            exlcudedproductTypeIds.Add((int)ProductTypes.RegularGas);
            exlcudedproductTypeIds.Add((int)ProductTypes.MidgradeGas);
            var response = ContextFactory.Current.GetDomain<MasterDomain>().GetProductTypes(exlcudedproductTypeIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetFuelGroups(int fuelGroupType, string companyIds)
        {
            var response = await ContextFactory.Current.GetDomain<FuelGroupDomain>().GetFuelGroups((FuelGroupType)fuelGroupType,companyIds, UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}