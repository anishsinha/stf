using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Sales.Controllers
{
    [AuthorizeCalculator]
    public class InternalSalesController : BaseController
    {

        [HttpGet]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult Calculator()
        {
            var response = new SalesCalculatorViewModel();
            return PartialView("SalesCalculator", response);
        }

        [HttpPost]
        public async Task<ActionResult> Calculator(SalesCalculatorViewModel viewModel)
        {
            using (var tracer = new Tracer("InternalSalesController", "Calculator(viewModel)"))
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.IsCityRackTerminal)
                    {
                        if (viewModel.CityTerminalPricingType == (int)SalesCalculatorRegionType.City && viewModel.CityTerminalIds.Count == 0)
                        {
                            DisplayCustomMessages((MessageType)Status.Failed, ResourceMessages.GetMessage(Resource.valMessageRequired, new object[] { Resource.lblCity }));
                            return View("SalesCalculator", viewModel);
                        }
                        else if (viewModel.CityTerminalPricingType == (int)SalesCalculatorRegionType.State && viewModel.StateTerminalIds.Count == 0)
                        {
                            DisplayCustomMessages((MessageType)Status.Failed, ResourceMessages.GetMessage(Resource.valMessageRequired, new object[] { Resource.lblState }));
                            return View("SalesCalculator", viewModel);
                        }
                    }

                    if (viewModel.FuelTypeInYourAreaId == null && viewModel.CommonFuelTypeId == null && viewModel.LessCommonFuelTypeId == null)
                    {
                        DisplayCustomMessages((MessageType)Status.Failed, ResourceMessages.GetMessage(Resource.valMessageRequired, new object[] { Resource.lblFuelType }));
                        return PartialView("SalesCalculator", viewModel);
                    }
                    var fuelrequestDomain = new FuelRequestDomain();
                    if (viewModel.SelectedFuelType == (int)ProductDisplayGroups.FuelTypesInYourArea)
                    {
                        viewModel.FuelTypeInYourAreaId = fuelrequestDomain.GetFuelTypeId(viewModel.FuelTypeInYourAreaId ?? 0, viewModel.FuelPricingDetails.PricingSourceId);
                    }
                    else if (viewModel.SelectedFuelType == (int)ProductDisplayGroups.LessCommonFuelType)
                    {
                        viewModel.LessCommonFuelTypeId = fuelrequestDomain.GetFuelTypeId(viewModel.LessCommonFuelTypeId ?? 0, viewModel.FuelPricingDetails.PricingSourceId);
                    }
                    else
                    {
                        viewModel.CommonFuelTypeId = fuelrequestDomain.GetFuelTypeId(viewModel.CommonFuelTypeId ?? 0, viewModel.FuelPricingDetails.PricingSourceId);
                    }
                    var salesCalculatorDomain = new SalesCalculatorDomain();
                    if (viewModel.FuelPricingDetails.PricingSourceId == (int)PricingSource.OPIS)
                    {
                        return PartialView("SalesCalculatorOpisGrid", viewModel);
                    }
                    else if (viewModel.FuelPricingDetails.PricingSourceId == (int)PricingSource.PLATTS)
                    {
                        return PartialView("SalesCalculatorPlattsGrid", viewModel);
                    }
                    else
                    {
                        var response = await salesCalculatorDomain.GetTerminalPricesForCalculator(viewModel, CurrentUser.CompanyId);
                        return PartialView("SalesCalculatorGrid", response);
                    }
                }
                return PartialView("SalesCalculator", viewModel);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProductList(string zipCode, decimal radius = 100, PricingSource source = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("InternalSalesController", "GetProductList"))
            {
                var response = await ContextFactory.Current.GetDomain<HelperDomain>().GetProductsByZip(zipCode, radius, source, CurrentUser.CompanyId);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCityGroupCities(PricingSource source = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("InternalSalesController", "GetCityGroupCities"))
            {
                var response = ContextFactory.Current.GetDomain<MasterDomain>().GetCityGroupCities(source);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetProductListByGroup(ProductDisplayGroups displayGroupId, int jobId = 0, decimal radius = 100, string zipCode = "", PricingSource source = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("FuelRequestController", "GetProductList"))
            {
                var response = new List<DropdownDisplayItem>();
                var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
                //if (source == PricingSource.OPIS || source == PricingSource.PLATTS)
                //{
                //    response = pricingDomain.GetSourceBasedFuelProducts(source);
                //}
                //else
                //{
                    response = await pricingDomain.GetAxxisFuelProducts(displayGroupId, CurrentUser.CompanyId, jobId, radius, zipCode);
                //}
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetCityRackPricingForCalculator(CityRackCalculatorInputViewModel inputData)
        {
            using (var tracer = new Tracer("InternalSalesController", "GetCityRackPricingForCalculator"))
            {
                var response = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetCityRackTerminalPricesForCalculator(inputData);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetSalesCalculatorPlattsGrid(SalesCalculatorViewModel requestModel)
        {
            using (var tracer = new Tracer("OrderController", "GetSalesCalculatorPlattsGrid"))
            {
                var salesCalculatorDomain = new SalesCalculatorDomain();
                var dataTableSearchModel = new DataTableSearchModel(requestModel);

                var response = await salesCalculatorDomain.GetPlattsTerminalPricesForCalculator(requestModel, dataTableSearchModel);
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

        [HttpPost]
        public async Task<JsonResult> GetSalesCalculatorOpisGrid(SalesCalculatorDatatableViewModel requestModel)
        {
            using (var tracer = new Tracer("OrderController", "GetSalesCalculatorOpisGrid"))
            {
                var salesCalculatorDomain = new SalesCalculatorDomain();
                var dataTableSearchModel = new DataTableSearchModel(requestModel);

                var response = await salesCalculatorDomain.GetOpisTerminalPricesForCalculator(requestModel, dataTableSearchModel);
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