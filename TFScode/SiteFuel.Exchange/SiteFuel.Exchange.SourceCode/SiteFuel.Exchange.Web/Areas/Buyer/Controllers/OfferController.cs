using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Attributes;
using SiteFuel.Exchange.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using SiteFuel.Exchange.Core.StringResources;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Web.Areas.Buyer.Controllers
{
    [AuthorizeCompany(CompanyType.Buyer)]
    public class OfferController : BaseController
    {
        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public new async Task<ActionResult> View(Currency currency = Currency.USD, Country country = Country.USA)
        {
            var offerViewModel = await ContextFactory.Current.GetDomain<OfferDomain>().GetBuyerOfferViewModel(UserContext, currency, country);
            return View(offerViewModel);
        }

        [HttpGet]
        public ActionResult PartialAcceptOffer(int jobId, int fuelTypeId, int offerPricingId, int quantity, int truckLoadType, int pricingSource)
        {
            var offerOrderViewModel = ContextFactory.Current.GetDomain<OfferDomain>().GetOfferAcceptViewModel(jobId, fuelTypeId, offerPricingId, quantity, truckLoadType, pricingSource);
            return PartialView("_PartialAcceptOffer", offerOrderViewModel);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [OutputCache(CacheProfile = "OutputCacheStaticPage")]
        public ActionResult DeliverySchedule(int scheduleType = (int)DeliveryScheduleType.SpecificDates)
        {
            return PartialView("_PartialDeliveryScheduleFR", new DeliveryScheduleViewModel() { ScheduleType = scheduleType, CreatedBy = CurrentUser.Id });
        }

        [HttpPost]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> BuyerOffersGrid(OfferFilterViewModel filterViewModel)
        {
            var dataTableSearchModel = new DataTableSearchModel(filterViewModel);
            var response = await ContextFactory.Current.GetDomain<OfferDomain>().GetWebBuyerOfferGridAsync(CurrentUser.CompanyId, dataTableSearchModel, filterViewModel);
            var totalCount = 0;
            //if user login with branded supplier company URL then user will only see only branded supplier Offer.
            //we exclude the other offer
            if (CurrentUser.BrandedCompanyId > 0)
            {
                response = response.Where(top => top.SupplierCompanyId == CurrentUser.BrandedCompanyId).ToList();
                if (response.Count > 0)
                    response[0].TotalCount = response.Count;
            }
            if (response.Count > 0)
                totalCount = response[0].TotalCount;
            return new JsonResult
            {
                Data = new DatatableResponse()
                {
                    draw = filterViewModel.draw,
                    data = response,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount
                },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        [HttpPost]
        public async Task<JsonResult> Accept(OfferOrderViewModel viewModel)
        {
            var fuelrequestDomain = ContextFactory.Current.GetDomain<FuelRequestDomain>();
            viewModel.FuelDetails.FuelTypeId = fuelrequestDomain.GetFuelTypeId(viewModel.FuelDetails.FuelTypeId ?? 0, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId, viewModel.FuelDetails.FuelPricing.PricingTypeId);

            var response = await ContextFactory.Current.GetDomain<OfferDomain>().AcceptOfferPricing(viewModel, UserContext);
            if (response.StatusCode == Status.Success)
                DisplayCustomMessages((MessageType)response.StatusCode, response.StatusMessage);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AuthorizeRole(UserRoles.BuyerAdmin, UserRoles.Buyer)]
        public async Task<ActionResult> Details(string request)
        {
            var response = await ContextFactory.Current.GetDomain<OfferDomain>().GetWebOfferPricingDetailsAsync(request, UserContext);
            if (response.StatusCode == Status.Failed || response.OfferOrderViewModel.StatusCode == Status.Failed)
            {
                DisplayCustomMessages((MessageType)response.StatusCode, Resource.errLoadOfferDetailsFailed);
            }
            return View("Details", response);
        }

        [HttpGet]
        public async Task<ActionResult> CalculateLoadedPrice(int jobId, int offerPricingId, int productId, int quantity, string zipcode, int pricingType, int rackType, decimal price, bool includeTaxes, int marketBasedType, decimal supplierCost, int pricingSourceId = 0, int pricingCodeId = 0)
        {
            var fuelrequestDomain = ContextFactory.Current.GetDomain<FuelRequestDomain>();
            productId = fuelrequestDomain.GetFuelTypeId(productId, pricingSourceId, pricingType);

            OfferLoadedPriceViewModel response = await ContextFactory.Current.GetDomain<OfferDomain>().GetLoadedPrice(jobId, offerPricingId, productId, quantity, pricingType, rackType, price, includeTaxes, marketBasedType, zipcode, supplierCost, pricingSourceId, pricingCodeId);
            return PartialView("_PartialLoadedPriceBreakup", response);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllOfferProducts(int companyId, int countryId, int sourceId)
        {
            var response = new List<DropdownDisplayItem>();
            var storedProcedure = ContextFactory.Current.GetDomain<StoredProcedureDomain>();
            response = await storedProcedure.GetAllOfferProductsForCountry(companyId, countryId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBuyerOfferCityGroupTerminals(bool fromJobSearch, int jobId, int stateId, bool allStates = false, PricingSource sourceId = PricingSource.Axxis)
        {
            var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
            var response = pricingDomain.GetBuyerOfferCityGroupTerminals(fromJobSearch, jobId, stateId, allStates, sourceId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}