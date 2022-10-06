using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]
    public class OfferController : ApiBaseController
    {
        #region Get Location Details
        [HttpGet]
        public List<DropdownDisplayItem> GetCitiesByStateCode(int stateId)
        {
            using (var tracer = new Tracer("OfferController", "GetCitiesByStateCode"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    response = ContextFactory.Current.GetDomain<MasterDomain>().GetCities(stateId, false);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "GetCitiesByStateCode", ex.Message, ex);
                }

                return response;
            }
        }

        [HttpGet]
        public async Task<List<DropdownDisplayExtended>> GetZipcodesByStateCity(string state, string city)
        {
            using (var tracer = new Tracer("OfferController", "GetZipcodesByStateCity"))
            {
                var response = new List<DropdownDisplayExtended>();
                try
                {
                    response = await ContextFactory.Current.GetDomain<ZipCodeServiceDomain>().GetZipCodeList(state, city);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "GetCitiesByStateCode", ex.Message, ex);
                }
                return response;
            }
        }
        #endregion

        [HttpPost]
        public async Task<List<UspBuyerOfferGridViewModel>> GetOffers(GetOfferViewModel offerViewModel)
        {
            var response = new List<UspBuyerOfferGridViewModel>();
            using (var tracer = new Tracer("OfferController", "GetOffer"))
            {
                try
                {
                    var requestUrl = Request.RequestUri;
                    var fueltypes = new List<int>();
                    var states = new List<int>();
                    fueltypes.Add(offerViewModel.FuelTypeId);
                    if (offerViewModel.StateId.HasValue)
                    {
                        states.Add(offerViewModel.StateId.Value);
                    }

                    OfferFilterViewModel filter = new OfferFilterViewModel
                    {
                        Cities = offerViewModel.City ?? string.Empty,
                        FuelTypes = fueltypes,
                        IsJobSearch = offerViewModel.JobId.HasValue,
                        OfferType = OfferType.All,
                        JobId = offerViewModel.JobId ?? 0,
                        Quantity = offerViewModel.Quantity,
                        States = states,
                        ZipCodes = offerViewModel.ZipCode ?? string.Empty,
                        TruckLoadType = offerViewModel.TruckLoadType,
                        //PricingTypeId = offerViewModel.PricingTypeId,
                        PricingSource = offerViewModel.PricingSource,
                        //FeedType = offerViewModel.FeedType,
                        //QuantityIndicator = offerViewModel.QuantityIndicator,
                        //FuelClass = offerViewModel.FuelClass,
                        CityGroupTerminalId = offerViewModel.CityGroupTerminalId
                    };

                    // get pricing code from pricing service
                    var pricingModel = new PricingCodesRequestViewModel() { PricingSourceId = (int)offerViewModel.PricingSource, PricingTypeId = offerViewModel.PricingTypeId, FeedTypeId = (int)offerViewModel.FeedType, FuelClassTypeId = (int)offerViewModel.FuelClass, QuantityIndicatorId = (int)offerViewModel.QuantityIndicator};
                    var pricing = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingCodesAsync(pricingModel);
                    if(pricing != null && pricing.PricingCodes != null && pricing.PricingCodes.Any())
                    {
                        var pricingCode = pricing.PricingCodes.OrderByDescending(t => t.Id).FirstOrDefault();
                        filter.PricingCodeId = pricingCode.Id;
                        filter.PricingTypeId = pricingCode.PricingTypeId;
                    }

                    response = await ContextFactory.Current.GetDomain<OfferDomain>().GetBuyerOfferGridAsync(offerViewModel.BuyerCompanyId, null, filter);
                    foreach (var offer in response)
                    {
                        if (offer.SupplierLogoId.HasValue)
                        {
                            offer.SupplierLogoURL = $"{requestUrl.Scheme}://{requestUrl.Authority}/api/Master/GetImage?imageid={offer.SupplierLogoId}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "GetOffer", ex.Message, ex);
                }
            }
            return response;
        }

        [HttpGet]
        public async Task<ApiOfferDetailsViewModel> GetOfferDetails(string encryptedRq, int buyerId)
        {
            var response = new ApiOfferDetailsViewModel();
            using (var tracer = new Tracer("OfferController", "GetOfferDetails"))
            {
                try
                {
                    var userContext = await GetUserContext(buyerId, CompanyType.Buyer);
                    var offerDetails = await ContextFactory.Current.GetDomain<OfferDomain>().GetOfferPricingDetailsAsync(encryptedRq, userContext);
                    response = ToBuyerAppOfferDetailsViewModel(offerDetails);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "GetOfferDetails", ex.Message, ex);
                }
                return response;
            }
        }

        private ApiOfferDetailsViewModel ToBuyerAppOfferDetailsViewModel(OfferPricingDetailsViewModel offerDetails)
        {
            var viewModel = new ApiOfferDetailsViewModel();

            viewModel.OfferNumber = offerDetails.OfferNumber;
            viewModel.OfferType = offerDetails.OfferType;
            viewModel.FuelType = offerDetails.FuelType.FirstOrDefault().ToString();
            viewModel.FuelTypeId = offerDetails.OfferOrderViewModel.FuelDetails.FuelTypeId;

            if (offerDetails.OfferViewModel.OfferLocationTypeId == (int)OfferLocationType.State)
            {
                var stateId = offerDetails.OfferViewModel.States.FirstOrDefault();

                if (offerDetails.OfferViewModel.States.Count == 1 && stateId == 0)
                {
                    var location = new OfferLocationViewModel()
                    {
                        State = Resource.lblAll,
                        City = Resource.lblAll,
                    };
                    location.ZipStringList.Add(Resource.lblAll);
                    viewModel.Locations.Add(location);
                }
                else
                {
                    foreach (var item in offerDetails.DisplayStates)
                    {
                        var location = new OfferLocationViewModel()
                        {
                            State = item,
                            City = Resource.lblAll,
                        };
                        location.ZipStringList.Add(Resource.lblAll);
                        viewModel.Locations.Add(location);
                    }
                }
            }
            else
            {
                viewModel.Locations = offerDetails.OfferViewModel.LocationViewModel;
            }

            viewModel.OfferPricingId = offerDetails.OfferPricingId;
            viewModel.SupplierCompanyId = offerDetails.SupplierCompanyId;
            viewModel.JobId = offerDetails.JobId;
            viewModel.Quantity = offerDetails.Quantity;
            viewModel.FuelFees = offerDetails.OfferViewModel.FuelDeliveryDetails.FuelFees;
            viewModel.FuelPricing = offerDetails.OfferOrderViewModel.FuelDetails.FuelPricing;
            viewModel.FuelPricing.DifferentFuelPrices = offerDetails.OfferViewModel.FuelPricing.DifferentFuelPrices;
            return viewModel;
        }

        [HttpGet]
        public async Task<OfferLoadedPriceViewModel> CalculateLoadedPrice(int jobId, int offerPricingId, int productId, int quantity, string zipcode, int pricingType, int rackType, decimal price, bool includeTaxes, int marketBasedType, decimal suppliercost, int sourceId = 1)
        {
            using (var tracer = new Tracer("OfferController", "CalculateLoadedPrice"))
            {
                var response = new OfferLoadedPriceViewModel();
                try
                {
                    int fuelTypeId = ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelTypeId(productId, sourceId, (int)PricingType.RackHigh);
                    response = await ContextFactory.Current.GetDomain<OfferDomain>().GetLoadedPrice(jobId, offerPricingId, fuelTypeId, quantity, pricingType, rackType, price, includeTaxes, marketBasedType, zipcode, suppliercost);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "CalculateLoadedPrice", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusViewModel> AcceptOffer(AcceptOfferMobileViewModel viewModel)
        {
            using (var tracer = new Tracer("OfferController", "AcceptOffer"))
            {
                var response = new StatusViewModel();
                try
                {
                    var userContext = await GetUserContext(viewModel.BuyerId, CompanyType.Buyer);
                    response = await ContextFactory.Current.GetDomain<OfferDomain>().AcceptOfferFromMobile(viewModel, userContext);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "AcceptOffer", ex.Message, ex);
                }
                return response;
            }
        }

        [HttpGet]
        public async Task<OfferSupplierProfileViewModel> BaseballCardDetails(int buyerCompanyId, int supplierCompanyId)
        {
            var response = new OfferSupplierProfileViewModel();
            try
            {
                response = await ContextFactory.Current.GetDomain<OfferDomain>().BaseballCardDetails(buyerCompanyId, supplierCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferController", "BaseballCardDetails", ex.Message, ex);
            }
            return response;
        }

        [HttpGet]
        public List<DropdownDisplayItem> GetJobsForOffer(int offerPricingId,int companyId,int userId)
        {
            using (var tracer = new Tracer("JobController", "GetJobsForOffer"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    response = ContextFactory.Current.GetDomain<JobDomain>().GetJobsForOfferPricing(companyId, offerPricingId, userId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("JobController", "GetJobsForOffer", ex.Message, ex);
                }

                return response;
            }
        }

        [HttpGet]
        public async Task<List<DropdownDisplayItem>> GetAllOfferProducts(int companyId, int countryId, int sourceId)
        {
            using (var tracer = new Tracer("OfferController", "GetAllOfferProducts"))
            {
                var response = new List<DropdownDisplayItem>();
                try
                {
                    var storedProcedure = ContextFactory.Current.GetDomain<StoredProcedureDomain>();
                    response =  await storedProcedure.GetAllOfferProductsForCountry(companyId, countryId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "GetAllOfferProducts", ex.Message, ex);
                }

                return response;
            }
        }

        [HttpGet]
        public  List<DropdownDisplayStateItem> GetBuyerOfferCityGroupTerminals(bool fromJobSearch, int jobId, int stateId, bool allStates = false, PricingSource sourceId = PricingSource.Axxis)
        {
            using (var tracer = new Tracer("OfferController", "GetBuyerOfferCityGroupTerminals"))
            {
                var response = new List<DropdownDisplayStateItem>();
                try
                {
                    var pricingDomain = ContextFactory.Current.GetDomain<ExternalPricingDomain>();
                    response =  pricingDomain.GetBuyerOfferCityGroupTerminals(fromJobSearch, jobId, stateId, allStates, sourceId);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("OfferController", "GetBuyerOfferCityGroupTerminals", ex.Message, ex);
                }

                return response;
            }
        }
    }
}