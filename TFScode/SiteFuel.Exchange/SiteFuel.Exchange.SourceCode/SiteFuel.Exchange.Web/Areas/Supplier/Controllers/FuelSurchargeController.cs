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
    [AuthorizeCompany(CompanyType.Supplier, CompanyType.Carrier)]
    public class FuelSurchargeController : BaseController
    {
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public ActionResult Create()
        {
            var response = ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetCreateFuelSurchargeInput(CurrentUser.CompanyId);
            return View(response);
        }       

        [HttpPost]
        public ActionResult GenerateTable(CreateFuelSurchargeInputViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "SaveDiscount"))
            {
                if (ModelState.IsValid)
                {
                    var surchargeTable = ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GenerateTable(viewModel);
                    return PartialView("_PartialFuelSurchargeTable", surchargeTable);
                }
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson)]
        public async Task<ActionResult> Create(CreateFuelSurchargeInputViewModel viewModel)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "SaveSurchargeTable"))
            {
                if (ModelState.IsValid)
                {

                    StatusViewModel response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().CreateFuelSurchargeTableAsync(viewModel, UserContext.Id, UserContext.CompanyId);
                    DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
                    if (response.StatusCode == Status.Success)
                    {
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        return View("~/Areas/Supplier/Views/FuelSurcharge/Create.cshtml", viewModel);
                    }
                }
                return View("~/Areas/Supplier/Views/FuelSurcharge/Create.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetFuelSurchargeSummary(ViewFuelSurchargeInputViewModel input)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "GetFuelSurchargeSummary"))
            {
                var customerId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.UrlReferrer.Query).GetValues("buyerCompanyId")[0]);
                var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetFuelSurchargeSummary(input, customerId, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetViewFuelSurchargeSummary(ViewFuelSurchargeInputViewModel input)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "GetViewFuelSurchargeSummary"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetFuelSurchargeSummaryNew(input, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> ArchiveFuelSurchargeTable(int fuelSurchargeIndexId)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "ArchiveFuelSurchargeTable"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().ArchiveFuelSurchargeTable(fuelSurchargeIndexId, CurrentUser.Id);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetSurchargeTable(CreateFuelSurchargeInputViewModel input)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "GetSurchargeTable"))
            {
                var customerId = Convert.ToInt32(HttpUtility.ParseQueryString(Request.UrlReferrer.Query).GetValues("buyerCompanyId")[0]);
                var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetSurchargeTable(input, customerId, CurrentUser.CompanyId);
                return PartialView("_PartialEditFuelSurchargeTable", response);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSurchargeTableNew(int fuelSurchargeIndexId)
        {
            var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetSurchargeTableNew(fuelSurchargeIndexId, CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> EditSurchargeTable(List<FuelSurchargeTableViewModel> input)
        {
            using (var tracer = new Tracer("FuelSurchargeController", "EditSurchargeTable"))
            {
                var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().EditSurchargeTable(input, CurrentUser.CompanyId, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ActionName("View")]
        public ActionResult Index()
        {
            var response = ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetViewFuelSurchargeInput(CurrentUser.CompanyId);
            return PartialView("_PartialFuelSurchargeTableView", response);
        }

       
        #region Angular Fuel Surcharge related impplementation 

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public ActionResult CreateNew()
        {
            return View();
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public  JsonResult GetTableTypes()
        {
            var response = CommonHelperMethods.GetSurchargeTableTypes();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
       
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public JsonResult GetSupplierCustomers()
        {
            var response = CommonHelperMethods.GetSupplierCustomers(CurrentUser.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
      
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public JsonResult GetCarriersByCompanyId(int supplierCompanyId)
        {
            var response = CommonHelperMethods.GetCarriersByCompanyId(supplierCompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        public async Task<JsonResult> GetSourceRegionsAsync(SourceRegionInputViewModel sourceRegionInput)
        {
            var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetSourceRegionAsync(sourceRegionInput, UserContext);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public async Task<JsonResult> GetTerminalsAndBulkPlantsAsync(string regionIds)
        {
            List<DropdownDisplayExtended> response = null;
            if (!string.IsNullOrEmpty(regionIds))
            {
                response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetTerminalsAndBulkPlantsAsync(regionIds.Split(',').Select(int.Parse).ToList());
             }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public async Task<ActionResult> GetFuelSurchargeProductAsync(int countryId)
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetFuelSurchargeLookupAsync(countryId, (int)Lookup.FuelSurchargeProduct);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public async Task<ActionResult> GetFuelSurchargePeriodAsync(int countryId)
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetFuelSurchargeLookupAsync(countryId, (int)Lookup.FuelSurchargePeriod);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public async Task<ActionResult> GetFuelSurchargeAreaAsync(int countryId)
        {
            var response = await ContextFactory.Current.GetDomain<MasterDomain>().GetFuelSurchargeLookupAsync(countryId, (int)Lookup.FuelSurchargeArea);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public ActionResult GetEIAIndexPrice(int periodId, int productType, string fetchDate, int areaId)
        {
            var eiaResponse = ContextFactory.Current.GetDomain<EIAPriceUpdateDomain>().GetEIAPrice((FuelSurchagePricingType)periodId, (SurchargeProductTypes)productType, DateTime.Parse(fetchDate).Date, (FuelSurchageArea)areaId);
            return Json(eiaResponse, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpPost]
        public async Task<JsonResult> CreateFuelSurchargeAsync(FuelSurchargeIndexViewModel viewModel)
        {
            StatusViewModel response;
            var entityExist = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetExistingFuelSurchargeId(viewModel, UserContext.CompanyId);
            if (entityExist== null) // pure publish
            {
                response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().CreateFuelSurchargeTableAsync(viewModel, UserContext.Id, UserContext.CompanyId);
                
            }else
            {
                response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().UpdateFuelSurchargeTableAsync(viewModel, UserContext.Id, UserContext.CompanyId, entityExist.Value);
            }
            return Json(response, JsonRequestBehavior.DenyGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        public async Task<JsonResult> GetFuelSurchargeTableAsync(int fuelSurchargeTableId)
        {
            var response = await ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetFuelSurchargeTableAsync(fuelSurchargeTableId, UserContext.Id, UserContext.CompanyId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin, UserRoles.Dispatcher)]
        [HttpGet]
        [AuthorizeRole(UserRoles.SupplierAdmin, UserRoles.Supplier, UserRoles.AccountingPerson, UserRoles.Carrier, UserRoles.CarrierAdmin)]
        public ActionResult GetHistoricalPrice(int fuelSurchargeIndexId, int forPeriod)
        {
            var response = ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GetHistoricalPrice(fuelSurchargeIndexId, forPeriod);
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetGenerateSurchargeTable(string pRSV, string pREV,string pRI, string sI, string fSSP)
        {
            var surchargeTable = ContextFactory.Current.GetDomain<FuelSurchargeDomain>().GenerateTable(decimal.Parse(pRSV),
                decimal.Parse(pREV),
                decimal.Parse(pRI),
                 decimal.Parse(sI),
                decimal.Parse(fSSP));
            return Json(surchargeTable, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}