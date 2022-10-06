using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FuelPricing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class PricingServiceDomain : PricingApiDomain
    {

        public PricingServiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public PricingServiceDomain(BaseDomain domain) : base(domain)
        {
        }
        public async Task<CustomResponseModel> UpdateSourceRegion(SourceRegionPricingRequestModel model)
        {
            var result = new CustomResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateSourceRegion;
                result = await ApiPostCall<CustomResponseModel>(apiUrl, model);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "UpdateSourceRegion", ex.Message, ex);
                throw ex;
            }
            return result;
        }

        #region FuelRequestMethods
        public async Task<CustomResponseModel> SavePricingDetails(FuelPricingViewModel fuelPricing, UoM Uom, BrokerFuelRequestDetailsViewModel brokerFuelPricing = null)
        {
            var result = new CustomResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveRequestDetails;

                if (brokerFuelPricing == null)
                {
                    var requestModel = GetPricingRequestObject(fuelPricing, Uom);
                    result = await ApiPostCall<CustomResponseModel>(apiUrl, requestModel);
                }
                else
                {
                    var requestModel = GetBrokerPricingRequestObject(brokerFuelPricing, Uom);
                    result = await ApiPostCall<CustomResponseModel>(apiUrl, requestModel);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SavePricingDetails", ex.Message, ex);
                throw ex;
            }
            return result;
        }

        public async Task<CustomResponseModel> UpdatePricingDetails(FuelPricingViewModel fuelPricing, UoM Uom)
        {
            var result = new CustomResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateRequestDetails;

                var requestModel = GetPricingRequestObject(fuelPricing, Uom);
                result = await ApiPostCall<CustomResponseModel>(apiUrl, requestModel);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SavePricingDetails", ex.Message, ex);
                throw ex;
            }
            return result;
        }


        private object GetBrokerPricingRequestObject(BrokerFuelRequestDetailsViewModel brokerPricingVm, UoM uom)
        {
            SetTierPricingBrokerRequestDtls(brokerPricingVm);
            var fuelPricing = brokerPricingVm.FuelPricing;
            if (fuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                var requestModel = new
                {
                    PricePerGallon = HelperDomain.GetPriceWithMargin(brokerPricingVm.FuelPriceMargin.Margin, fuelPricing.PricePerGallon),
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    Currency = (int)fuelPricing.Currency,
                    UoM = uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    FuelTypeId = brokerPricingVm.FuelTypeId,
                    TerminalId = brokerPricingVm.TerminalId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    brokerPricingVm.FuelPriceMargin.MarginTypeId,
                    brokerPricingVm.FuelPriceMargin.Margin,

                };
                return requestModel;
            }
            else if (fuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                var requestModel = new
                {
                    PricePerGallon = fuelPricing.SupplierCostMarkupValue,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    Currency = (int)fuelPricing.Currency,
                    UoM = uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    RackAvgTypeId = fuelPricing.SupplierCostMarkupTypeId,
                    SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost,
                    SupplierCost = fuelPricing.SupplierCost,
                    FuelTypeId = brokerPricingVm.FuelTypeId,
                    TerminalId = brokerPricingVm.TerminalId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    BaseSupplierCost = fuelPricing.SupplierCost.HasValue ? MoneyConverter.GetBaseAmount(fuelPricing.Currency, fuelPricing.SupplierCost.Value, fuelPricing.ExchangeRate) : 0,
                    brokerPricingVm.FuelPriceMargin.MarginTypeId,
                    brokerPricingVm.FuelPriceMargin.Margin
                };
                return requestModel;
            }
            else if (fuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                var requestModel = new
                {
                    Currency = (int)fuelPricing.Currency,
                    UoM = uom,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    TierPricing = fuelPricing.TierPricing,

                };
                return requestModel;
            }
            else
            {
                var requestModel = new
                {
                    PricePerGallon = HelperDomain.GetPriceWithMargin(brokerPricingVm.FuelPriceMargin.Margin, fuelPricing.RackPrice),
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    RackAvgTypeId = fuelPricing.RackAvgTypeId,
                    FuelTypeId = brokerPricingVm.FuelTypeId,
                    TerminalId = brokerPricingVm.TerminalId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    Currency = (int)fuelPricing.Currency,
                    UoM = uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    brokerPricingVm.FuelPriceMargin.MarginTypeId,
                    brokerPricingVm.FuelPriceMargin.Margin
                };

                return requestModel;
            }
        }

        public async Task<bool> FuelCostCompare(FuelRequest fuelRequest, FuelRequestViewModel viewModel)
        {
            bool sucess = false;
            double fuelCost = Convert.ToDouble(fuelRequest.FuelRequestPricingDetail.DisplayPrice.Substring(fuelRequest.FuelRequestPricingDetail.DisplayPrice.LastIndexOf("$") + 1));
            var objFuelCostDetail = GetPricingRequestObject(viewModel.FuelDetails.FuelPricing, viewModel.FuelDetails.FuelQuantity.UoM);
            double pricePerGallon = Convert.ToDouble(objFuelCostDetail.GetType().GetProperty("PricePerGallon").GetValue(objFuelCostDetail));

            fuelCost = Math.Round(fuelCost, 2, MidpointRounding.ToEven);
            pricePerGallon = Math.Round(pricePerGallon, 2, MidpointRounding.ToEven);

            if (fuelCost == pricePerGallon && viewModel.FuelDetails.FuelPricing.PricingTypeId == fuelRequest.PricingTypeId && viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Id == fuelRequest.FuelRequestPricingDetail.PricingCodeId)
            {
                sucess = true;
            }
            return sucess;
        }


        private object GetPricingRequestObject(FuelPricingViewModel fuelPricing, UoM Uom)
        {
            SetTierPricingRequestDtls(fuelPricing);
            if (fuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                var requestModel = new
                {
                    Id = fuelPricing.FuelPricingDetails.RequestPriceDetailId,
                    PricePerGallon = fuelPricing.PricePerGallon,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    Currency = (int)fuelPricing.Currency,
                    UoM = Uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    FuelTypeId = fuelPricing.FuelTypeId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    TerminalId = fuelPricing.TerminalId,
                    parameterJSON = fuelPricing.ParameterJSON,
                };
                return requestModel;
            }
            else if (fuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                var requestModel = new
                {
                    Id = fuelPricing.FuelPricingDetails.RequestPriceDetailId,
                    PricePerGallon = fuelPricing.SupplierCostMarkupValue,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    Currency = (int)fuelPricing.Currency,
                    UoM = Uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    RackAvgTypeId = fuelPricing.SupplierCostMarkupTypeId,
                    SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost,
                    SupplierCost = fuelPricing.SupplierCost,
                    BaseSupplierCost = fuelPricing.SupplierCost.HasValue ? MoneyConverter.GetBaseAmount(fuelPricing.Currency, fuelPricing.SupplierCost.Value, fuelPricing.ExchangeRate) : 0,
                    FuelTypeId = fuelPricing.FuelTypeId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    TerminalId = fuelPricing.TerminalId,
                    parameterJSON = fuelPricing.ParameterJSON,
                };
                return requestModel;
            }
            else if (fuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                var requestModel = new
                {
                    //PricePerGallon = fuelPricing.RackPrice,
                    // PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    //RackAvgTypeId = fuelPricing.RackAvgTypeId,
                    Id = fuelPricing.FuelPricingDetails.RequestPriceDetailId,
                    Currency = (int)fuelPricing.Currency,
                    UoM = Uom,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    TierPricing = fuelPricing.TierPricing,

                };
                return requestModel;
            }
            else
            {
                var requestModel = new
                {
                    Id = fuelPricing.FuelPricingDetails.RequestPriceDetailId,
                    PricePerGallon = fuelPricing.RackPrice,
                    PricingCodeId = fuelPricing.FuelPricingDetails.PricingCode.Id,
                    RackAvgTypeId = fuelPricing.RackAvgTypeId,
                    Currency = (int)fuelPricing.Currency,
                    UoM = Uom,
                    ExchangeRate = fuelPricing.ExchangeRate,
                    FuelTypeId = fuelPricing.FuelTypeId,
                    PricingTypeId = fuelPricing.PricingTypeId,
                    TerminalId = fuelPricing.TerminalId,
                    parameterJSON = fuelPricing.ParameterJSON,
                };

                return requestModel;
            }
        }

        #endregion

        internal async Task<IntResponseModel> SaveNonStandardProduct(string nonStandardFuelName, int createdBy, int pricingSourceId, int? companyId = null, bool isDeleted = false)
        {
            IntResponseModel result = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAddNewProduct;
                var requestModel = GetProductRequestObject(nonStandardFuelName, createdBy, pricingSourceId, companyId, isDeleted);
                result = await ApiPostCall<IntResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SaveNonStandardProduct", ex.Message, ex);
            }
            return result;
        }

        internal async Task<IntResponseModel> SaveTerminalDetails(PickupLocationDetailViewModel viewModel)
        {
            IntResponseModel result = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSaveTerminalDetails;
                result = await ApiPostCall<IntResponseModel>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SaveTerminalDetails", ex.Message, ex);
            }
            return result;
        }

        internal async Task<IntResponseModel> UpdateNonStandardProduct(AdditiveProductDetailsViewModel product)
        {
            IntResponseModel result = new IntResponseModel();
            try
            {
                //used existing API for edit/delete as well
                var apiUrl = ApplicationConstants.UrlAddNewProduct;
                var requestModel = new
                {
                    Id = product.Id,
                    Name = product.AdditiveProductName,
                    isDeleted = product.IsDeleted
                };
                result = await ApiPostCall<IntResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "UpdateNonStandardProduct", ex.Message, ex);
            }
            return result;
        }

        private object GetProductRequestObject(string nonStandardFuelName, int createdBy, int pricingSourceId, int? companyId = null, bool isDeleted = false)
        {
            var requestModel = new
            {
                Name = nonStandardFuelName,
                PricingSourceId = pricingSourceId,
                ProductTypeId = companyId == null ? (int)ProductTypes.NonStandardFuel : (int)ProductTypes.Additives,
                ProductDisplayGroupId = companyId == null ? (int)ProductDisplayGroups.OtherFuelType : (int)ProductDisplayGroups.AdditiveFuelType,
                IsActive = true,
                UpdatedBy = createdBy,
                UpdatedDate = DateTimeOffset.Now,
                CompanyId = companyId,
                isDeleted = isDeleted
            };
            return requestModel;
        }

        internal async Task<IntResponseModel> SaveTfxProduct(ProductViewModel productViewModel)
        {
            IntResponseModel result = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAddNewTfxProduct;
                var requestModel = GetTfxProductRequestObject(productViewModel);
                result = await ApiPostCall<IntResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SaveTfxProduct", ex.Message, ex);
            }
            return result;
        }

        internal async Task<IntResponseModel> UpdateTfxProduct(ProductViewModel productViewModel)
        {
            IntResponseModel result = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateTfxProduct;
                var requestModel = GetTfxProductRequestObject(productViewModel);
                result = await ApiPostCall<IntResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "UpdateTfxProduct", ex.Message, ex);
            }
            return result;
        }

        private object GetTfxProductRequestObject(ProductViewModel productViewModel)
        {
            var requestModel = new
            {
                Name = productViewModel.DisplayName,
                Id = productViewModel.Id,
                ProductTypeId = productViewModel.ProductTypeId,
                ProductDisplayGroupId = productViewModel.ProductDisplayGroupId,
                ProductCode = productViewModel.ProductCode,
                IsActive = true,
                UpdatedBy = productViewModel.UpdatedBy,
                UpdatedDate = DateTimeOffset.Now,
                AxxisProductId = productViewModel.AxxisProductId,
                //ParklandProductId = productViewModel.ParklandProductId,
                PlattsProductId = productViewModel.PlattsProductId,
                OpisProductId = productViewModel.OpisProductId,
            };
            return requestModel;
        }

        public async Task<UspSourceBasedTerminalPrice> GetTerminalPrice(int? terminalId, int? cityGroupTerminalId, int productId, DateTimeOffset priceDate, int pricingCodeId)
        {
            UspSourceBasedTerminalPrice result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalPriceAsync;
                var inputmodel = new
                {
                    TerminalId = terminalId,
                    ProductId = productId,
                    PriceDate = priceDate.DateTime,
                    PricingCodeId = pricingCodeId,
                    CityGroupTerminalId = cityGroupTerminalId
                };
                result = await ApiPostCall<UspSourceBasedTerminalPrice>(apiUrl, inputmodel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetTerminalPrice", ex.Message, ex);
            }
            return result;
        }
        public async Task<IntResponseModel> GetFilterPriceDetailsByPricingType(FilterPricingRequestViewModel requestModel)
        {
            IntResponseModel result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetFilterPriceDetailsByPricingType;
                result = await ApiPostCall<IntResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetFilterPriceDetailsByPricingType", ex.Message, ex);
            }
            return result;
        }

        public async Task<UspGetTerminalRackPrice> GetTerminalPrice(int terminalId, int productId)
        {
            UspGetTerminalRackPrice result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalPriceAsync;
                var apiMethodUrl = string.Format(apiUrl, productId, terminalId);
                result = await ApiGetCall<UspGetTerminalRackPrice>(apiMethodUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetTerminalPrice", ex.Message, ex);
            }
            return result;
        }

        public async Task<FuelPricingResponseViewModel> GetFuelPrice(FuelPricingRequestViewModel fuelPricingRequestViewModel)
        {
            FuelPricingResponseViewModel response = null;
            try
            {
                var requestModel = GetPriceRequestObject(fuelPricingRequestViewModel);
                List<FuelPriceRequestModel> priceRequestModels = new List<FuelPriceRequestModel>() { requestModel };
                var result = await GetFuelPrice(priceRequestModels);
                response = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetFuelPrice", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<FuelPricingResponseViewModel>> GetFuelPrice(List<FuelPriceRequestModel> priceRequestModels)
        {
            List<FuelPricingResponseViewModel> result = null;
            try
            {
                var apiUrl = ApplicationConstants.UrlGetFuelPriceAsync;
                result = await ApiPostCall<List<FuelPricingResponseViewModel>>(apiUrl, priceRequestModels);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetFuelPrice", ex.Message, ex);
            }
            return result;
        }

        public async Task<DateTime> GetLastUpdatedPricingDate(int requestPriceDetailId)
        {
            DateTime result = new DateTime();
            try
            {
                var apiMethodUrl = string.Format(ApplicationConstants.UrlGetLastUpdatedPricingDate, requestPriceDetailId);
                result = await ApiGetCall<DateTime>(apiMethodUrl);
                result = result.Date;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetLastUpdatedPricingDate", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<int>> GetPriceDetailIdsBySource(List<int> requestPriceDetailIds, bool isAxxis = false)
        {
            var result = new List<int>();
            try
            {
                var requestModel = new { RequestPriceDetailIds = requestPriceDetailIds, IsAxxis = isAxxis };
                result = await ApiPostCall<List<int>>(ApplicationConstants.UrlGetPriceDetailIdsBySourceAsync, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetAxxisRequestPriceDetailIds", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DateTime>> GetPublicHolidayList()
        {
            var response = new List<DateTime>();
            try
            {
                var keys = new List<string>();
                keys.Add(ApplicationConstants.PublicHolidayList);
                PricingConfigResponse apiResponse = await GetPricingConfigs(keys);
                if (apiResponse.Status == Status.Success && apiResponse.Configs != null)
                {
                    var holidayList = apiResponse.Configs.FirstOrDefault()?.Value;
                    response = GetDatelistFromString(response, holidayList);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPublicHolidayList", ex.Message, ex);
            }
            return response;
        }

        public async Task<PricingConfigResponse> GetPricingConfigs(List<string> keys)
        {
            PricingConfigResponse result = new PricingConfigResponse();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetPricingConfig;
                result = await ApiPostCall<PricingConfigResponse>(apiUrl, keys);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPricingConfigs", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> IsCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId, PricingSource pricingSource, DateTime? effectiveDate)
        {
            bool result = false;
            try
            {
                var apiUrl = ApplicationConstants.UrlIsCityRackPriceAvailable;
                var apiMethodUrl = string.Format(apiUrl, fueltypeId, cityGroupTerminalId, (int)pricingSource, effectiveDate);
                var apiResult = await ApiGetCall<BooleanResponseModel>(apiMethodUrl);
                result = apiResult.Result;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "IsCityRackPriceAvailable", ex.Message, ex);
            }
            return result;
        }

        public async Task<IntResponseModel> SyncAxxisPricing()
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSyncAxxisPricing;
                response = await ApiGetCall<IntResponseModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SyncAxxisPricing", ex.Message, ex);
            }
            return response;
        }

        public async Task<SyncPricingResponseModel> SyncOPISPlattsPricing()
        {
            SyncPricingResponseModel response = new SyncPricingResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSyncOpisPlattsPricing;
                response = await ApiGetCall<SyncPricingResponseModel>(apiUrl, 600);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SyncOPISPlattsPricing", ex.Message, ex);
            }
            return response;
        }


        public async Task<CurrentCostResponseModel> UpdateFuelCostForFuelRequestAsync(CurrentCostRequestModel requestModel)
        {
            var response = new CurrentCostResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateSupplierCostToPriceDetail;
                response = await ApiPostCall<CurrentCostResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "UpdateFuelCostForFuelRequestAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<SalesCalculatorGridViewModel>> GetTerminalPricesForCalculator(SalesCalculatorInputViewModel viewModel)
        {
            List<SalesCalculatorGridViewModel> result = new List<SalesCalculatorGridViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalPricesForSalesCalculatorAsync;
                var reqMododel = GetCalculatorRequest(viewModel, PricingSource.Axxis);
                var apiResult = await ApiPostCall<SalesCalculatorApiResponse>(apiUrl, reqMododel);
                result = apiResult.ToGridViewModel(result);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetTerminalPricesForCalculator", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DropdownDisplayItem>> GetProductsInYourArea(decimal radius, decimal latitude, decimal longitude, string countryCode, int companyId = 0)
        {
            List<DropdownDisplayItem> result = new List<DropdownDisplayItem>();
            try
            {
                int companyCountryId = 0;
                if (companyId > 0)
                {
                    companyCountryId = await Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault && t.IsActive)
                                                                                 .Select(t => t.CountryId).FirstOrDefaultAsync();
                }
                var apiUrl = ApplicationConstants.UrlGetProductsInYourAreaAsync;
                var inputmodel = new
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Radius = radius,
                    CountryCode = countryCode,
                    CompanyCountryId = companyCountryId
                };
                var apiResult = await ApiPostCall<ProductDetailsApiResponseModel>(apiUrl, inputmodel);
                result = apiResult.ProductDetails;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetProductsInYourArea", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetCityRackTerminalPricesForCalculator(CityRackCalculatorInputViewModel viewModel)
        {
            List<SalesCalculatorGridViewModel> result = new List<SalesCalculatorGridViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetCityRackTerminalPricesForSalesCalculatorAsync;
                var reqModel = GetCityRackPriceRequest(viewModel);
                var apiResult = await ApiPostCall<SalesCalculatorApiResponse>(apiUrl, reqModel);
                result = apiResult.ToGridViewModel(result);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetCityRackTerminalPricesForCalculator", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetTerminalPricesForAuditAsync(SalesCalculatorInputViewModel viewModel)
        {
            List<SalesCalculatorGridViewModel> result = new List<SalesCalculatorGridViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalPricesForAuditAsync;
                var reqModel = GetTerminalAuditRequest(viewModel);
                var apiResult = await ApiPostCall<SalesCalculatorApiResponse>(apiUrl, reqModel);
                result = apiResult.ToGridViewModel(result);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetTerminalPricesForAuditAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TerminalDetails>> GetClosestTerminalsAsync(int countryId, int pricingCodeId, int companyCountryId = 0, int fuelTypeId = 0, decimal latitude = 0, decimal longitude = 0, string terminal = "", bool isSuppressPricing = false)
        {
            List<TerminalDetails> result = new List<TerminalDetails>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetClosestTerminalsAsync;
                var reqModel = GetTerminalRequest(countryId, fuelTypeId, latitude, longitude, terminal, pricingCodeId, companyCountryId, isSuppressPricing);
                var apiResult = await ApiPostCall<GetTerminalApiResponse>(apiUrl, reqModel);
                result = apiResult.Terminals;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetClosestTerminalsAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TerminalDetails>> GetClosestTerminalsForSouceRegionsAsync(int countryId, int pricingCodeId, int companyCountryId = 0, int fuelTypeId = 0, decimal latitude = 0, decimal longitude = 0, string terminalIds = "", int pricingTypeId = 0, bool isSuppressPricing = false)
        {
            List<TerminalDetails> result = new List<TerminalDetails>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetClosestTerminalsByDistanceAsync;
                var reqModel = GetTerminalRequestModel(countryId, fuelTypeId, latitude, longitude, terminalIds, pricingTypeId, pricingCodeId, companyCountryId, isSuppressPricing);
                var apiResult = await ApiPostCall<GetTerminalApiResponse>(apiUrl, reqModel);
                result = apiResult.Terminals;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetClosestTerminalsForSouceRegionsAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<UspGetTerminalRackPrice> GetLatestTerminalPriceAsync(int? cityGroupTerminalId, int pricingSource, string fuelTypes, int pricingCodeId = 0, decimal latitude = 0, decimal longitude = 0)
        {
            UspGetTerminalRackPrice result = new UspGetTerminalRackPrice();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetLatestTerminalPriceAsync;
                var inputmodel = new
                {
                    PricingSourceId = pricingSource,
                    CountryCode = Country.USA,
                    TerminalId = cityGroupTerminalId,
                    ProductId = fuelTypes,
                    Latitude = latitude,
                    Longitude = longitude,
                    PricingCodeId = pricingCodeId
                };
                result = await ApiPostCall<UspGetTerminalRackPrice>(apiUrl, inputmodel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetLatestTerminalPriceAsync", ex.Message, ex);
            }
            return result;
        }

        private FuelPriceRequestModel GetPriceRequestObject(FuelPricingRequestViewModel fuelPricingRequestViewModel)
        {
            var inputmodel = new FuelPriceRequestModel()
            {
                TerminalId = fuelPricingRequestViewModel.TerminalId,
                ProductId = fuelPricingRequestViewModel.FuelTypeId,
                PriceDate = fuelPricingRequestViewModel.DropEndDate.Date,
                RequestPriceDetailId = fuelPricingRequestViewModel.FuelRequestPricingDetails.RequestPriceDetailId,
                CityGroupTerminalId = fuelPricingRequestViewModel.CityGroupTerminalId,
                SupplierCost = fuelPricingRequestViewModel.SupplierCost,
                DroppedQuantity = fuelPricingRequestViewModel.DroppedQuantity ?? 0,
                TierMinQuantity = fuelPricingRequestViewModel.TierMinQuantity,
                TierMaxQuantity = fuelPricingRequestViewModel.TierMaxQuantity
            };
            return inputmodel;
        }

        private object GetTerminalAuditRequest(SalesCalculatorInputViewModel viewModel)
        {
            var inputmodel = new
            {
                PricingDate = viewModel.PricingDate,
                ExternalProductId = viewModel.ExternalProductId,
                ProductId = viewModel.ExternalProductId,
                SrcLatitude = viewModel.SrcLatitude,
                SrcLongitude = viewModel.SrcLongitude,
                RecordCount = viewModel.RecordCount
            };
            return inputmodel;
        }

        private object GetCityRackPriceRequest(CityRackCalculatorInputViewModel viewModel)
        {
            var inputmodel = new
            {
                PriceDate = viewModel.PriceDate,
                ExternalProductId = viewModel.ExternalProductId,
                StateOrTerminalIds = viewModel.StateOrTerminalIds,
                CityTerminalPricingType = viewModel.CityTerminalPricingType
            };
            return inputmodel;
        }

        private object GetTerminalRequest(int countryId, int fuelTypeId, decimal latitude, decimal longitude, string terminal, int pricingCodeId, int companyCountryId, bool isSuppressPricing)
        {
            var inputmodel = new
            {
                ProductId = fuelTypeId,
                CountryId = countryId,
                SearchStringTeminal = terminal,
                SrcLatitude = latitude,
                SrcLongitude = longitude,
                pricingCodeId = pricingCodeId,
                CompanyCountryId = companyCountryId > 0 ? companyCountryId : countryId,
                IsSuppressPricing = isSuppressPricing
            };
            return inputmodel;
        }

        private object GetTerminalRequestModel(int countryId, int fuelTypeId, decimal latitude, decimal longitude, string terminalIds, int pricingTypeId, int pricingCodeId, int companyCountryId, bool isSuppressPricing)
        {
            var inputmodel = new
            {
                ProductId = fuelTypeId,
                CountryId = countryId,
                TerminalIds = terminalIds,
                PricingTypeId = pricingTypeId,
                SrcLatitude = latitude,
                SrcLongitude = longitude,
                pricingCodeId = pricingCodeId,
                CompanyCountryId = companyCountryId > 0 ? companyCountryId : countryId,
                IsSuppressPricing = isSuppressPricing
            };
            return inputmodel;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetPlattsTerminalPricesForCalculator(SalesCalculatorViewModel viewModel, DataTableSearchModel requestModel, int timeout = 30)
        {
            List<SalesCalculatorGridViewModel> result = new List<SalesCalculatorGridViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalPricesForSalesCalculatorAsync;
                var reqMododel = GetCalculatorRequest(viewModel, requestModel, PricingSource.PLATTS);
                var apiResult = await ApiPostCall<SalesCalculatorApiResponse>(apiUrl, reqMododel);
                result = apiResult.ToGridViewModel(result);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPlattsTerminalPricesForCalculator", ex.Message, ex);
            }
            return result;
        }

        private object GetCalculatorRequest(SalesCalculatorInputViewModel viewModel, PricingSource pricingSource)
        {
            var inputmodel = new
            {
                PricingSourceId = (int)pricingSource,
                PriceDate = viewModel.PricingDate,
                CityGroupTerminalIds = viewModel.CityGroupTerminalId,
                ProductId = viewModel.ExternalProductId,
                SrcLatitude = viewModel.SrcLatitude,
                SrcLongitude = viewModel.SrcLongitude,
                RecordCount = viewModel.RecordCount == 0 ? 5 : viewModel.RecordCount,
                CountryCode = viewModel.CountryCode,
                PricingCodeId = viewModel.PricingCodeId,
                CompanyCountryId = viewModel.CompanyCountryId
            };
            return inputmodel;
        }

        private object GetCalculatorRequest(SalesCalculatorViewModel viewModel, DataTableSearchModel requestModel, PricingSource pricingSource)
        {
            var inputmodel = new
            {
                PricingSourceId = (int)pricingSource,
                //SourceId = viewModel.FuelPricingDetails.PricingSourceId,
                PriceDate = viewModel.PriceDate,
                CityGroupTerminalIds = viewModel.CityTerminalIds,
                ProductId = viewModel.CommonFuelTypeId,
                CountryCode = Country.USA,
                PricingCodeId = viewModel.PricingCodeId,
                requestModel
            };
            return inputmodel;
        }

        private object GetCalculatorRequest(SalesCalculatorDatatableViewModel viewModel, DataTableSearchModel requestModel, PricingSource pricingSource)
        {
            var inputmodel = new
            {
                PricingSourceId = (int)pricingSource,
                CountryCode = Country.USA,
                PriceDate = viewModel.PriceDate,
                CityGroupTerminalIds = viewModel.CityTerminalIds,
                ProductId = viewModel.ProductId,
                PricingCodeId = viewModel.PricingCodeId,
                requestModel
            };
            return inputmodel;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetOpisTerminalPricesForCalculator(SalesCalculatorDatatableViewModel viewModel, DataTableSearchModel requestModel, int timeout = 30)
        {
            List<SalesCalculatorGridViewModel> result = new List<SalesCalculatorGridViewModel>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalPricesForSalesCalculatorAsync;
                var reqMododel = GetCalculatorRequest(viewModel, requestModel, PricingSource.OPIS);
                var apiResult = await ApiPostCall<SalesCalculatorApiResponse>(apiUrl, reqMododel);
                result = apiResult.ToGridViewModel(result);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetOpisTerminalPricesForCalculator", ex.Message, ex);
            }
            return result;
        }

        public List<DateTime> GetDatelistFromString(List<DateTime> holidayDates, string publicHolidayList)
        {
            List<string> holidays = new List<string>();
            if (publicHolidayList != null)
            {
                holidays = publicHolidayList.TrimEnd(';').Split(';').ToList();
                holidayDates = holidays.Select(date => DateTime.Parse(date)).ToList();
            }

            return holidayDates;
        }

        public async Task<PricingCodesApiResponse> GetPricingCodesAsync(PricingCodesRequestViewModel viewModel, int timeout = 30)
        {
            PricingCodesApiResponse result = new PricingCodesApiResponse();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetPricingCodes;
                result = await ApiPostCall<PricingCodesApiResponse>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPricingCodesAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<PricingRequestDetailApiResponse> GetPricingRequestDetailByIdAsync(PricingDetailRequestViewModel viewModel, int timeout = 30)
        {
            PricingRequestDetailApiResponse result = new PricingRequestDetailApiResponse();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetPricingRequestDetailById;
                result = await ApiPostCall<PricingRequestDetailApiResponse>(apiUrl, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPricingRequestDetailByIdAsync", ex.Message, ex);
            }
            return result;
        }

        public async Task<PricingConfigApiResponse> GetPricingConfigSettingsAsync(int id = 0)
        {
            PricingConfigApiResponse result = new PricingConfigApiResponse();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetPricingConfigDetails;
                var apiMethodUrl = string.Format(apiUrl, id);
                result = await ApiGetCall<PricingConfigApiResponse>(apiMethodUrl);

                if (result != null && result.Status == Status.Success)
                {
                    UpdateConfigSettings(result);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPricingConfigSettingsAsync", ex.Message, ex);
            }
            return result;
        }

        private void UpdateConfigSettings(PricingConfigApiResponse result)
        {
            try
            {
                if (result.ConfigList != null && result.ConfigList.Any())
                {
                    foreach (var item in result.ConfigList)
                    {
                        var updatedBy = Convert.ToInt32(item.UpdatedBy);
                        var updatedDate = Convert.ToDateTime(item.UpdatedDate);

                        item.UpdatedBy = Context.DataContext.Users.Where(t1 => t1.Id == updatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault();
                        item.UpdatedDate = updatedDate.ToString(Resource.constFormatDate);
                    }
                }

                if (result.Config != null)
                {
                    var updatedBy = Convert.ToInt32(result.Config.UpdatedBy);
                    var updatedDate = Convert.ToDateTime(result.Config.UpdatedDate);

                    result.Config.UpdatedBy = Context.DataContext.Users.Where(t1 => t1.Id == updatedBy).Select(t1 => (t1.FirstName + " " + t1.LastName)).FirstOrDefault();
                    result.Config.UpdatedDate = updatedDate.ToString(Resource.constFormatDate);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "UpdateConfigSettings", ex.Message, ex);
            }
        }

        public async Task<PricingConfigApiResponse> EditPricingConfigSettingsAsync(PricingConfigViewModel model)
        {
            var result = new PricingConfigApiResponse();
            try
            {
                var apiUrl = ApplicationConstants.UrlEditPricingConfig;
                var requestModel = GetPricingConfigRequestObject(model);
                result = await ApiPostCall<PricingConfigApiResponse>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "EditPricingConfigSettingsAsync", ex.Message, ex);
                throw ex;
            }
            return result;
        }

        private object GetPricingConfigRequestObject(PricingConfigViewModel model)
        {
            var requestModel = new
            {
                Id = model.Id,
                Key = model.Key,
                Value = model.Value,
                Description = model.Description,
                UpdatedBy = model.UpdatedBy,
            };
            return requestModel;

        }

        public async Task<List<TerminalDetails>> GetClosestTerminalsForFueltypesAsync(int countryId, int pricingCodeId, List<int> fuelTypeList, int companyCountryId, bool issuppressPricing, decimal latitude = 0, decimal longitude = 0, string terminal = "")
        {
            List<TerminalDetails> result = new List<TerminalDetails>();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetTerminalsForMultiProductsAsync;
                var reqModel = GetTerminalForFueltypesRequest(countryId, fuelTypeList, latitude, longitude, terminal, pricingCodeId, companyCountryId, issuppressPricing);
                var apiResult = await ApiPostCall<GetTerminalApiResponse>(apiUrl, reqModel);
                result = apiResult.Terminals;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetClosestTerminalsForFueltypesAsync", ex.Message, ex);
            }
            return result;
        }

        private object GetTerminalForFueltypesRequest(int countryId, List<int> fuelTypeIds, decimal latitude, decimal longitude, string terminal, int pricingCodeId, int companyCountryId, bool issuppressPricing)
        {
            var inputmodel = new
            {
                ProductId = string.Join(",", fuelTypeIds),
                CountryId = countryId,
                SearchStringTeminal = terminal,
                SrcLatitude = latitude,
                SrcLongitude = longitude,
                pricingCodeId = pricingCodeId,
                CompanyCountryId = companyCountryId,
                IsSuppressPricing = issuppressPricing
            };
            return inputmodel;
        }

        public async Task<List<DropdownDisplayItem>> GetAllTerminals()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var cacheKey = $"GetAllTerminals";
                response = CacheManager.Get<List<DropdownDisplayItem>>(cacheKey);
                if (response == null)
                {
                    var apiUrl = ApplicationConstants.UrlGetAllTerminalsAsync;
                    response = await ApiGetCall<List<DropdownDisplayItem>>(apiUrl);
                    CacheManager.Set(cacheKey, response, 3600);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetAllTerminals", ex.Message, ex);
            }
            return response;
        }


        public async Task<SyncPricingResponseModel> SyncActualOPISPricing()
        {
            var response = new SyncPricingResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSyncActualOpisPricing;
                response = await ApiGetCall<SyncPricingResponseModel>(apiUrl, 600);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SyncActualOPISPricing", ex.Message, ex);
            }
            return response;
        }

        public async Task<IntResponseModel> SyncDyedProductPricingFromClearProducts()
        {
            IntResponseModel response = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlSyncDyedProductPricingFromClearProducts;
                response = await ApiGetCall<IntResponseModel>(apiUrl);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SyncDyedProductPricingFromClearProducts", ex.Message, ex);
            }
            return response;
        }

        private void SetTierPricingRequestDtls(FuelPricingViewModel fuelPricing)
        {
            if (fuelPricing != null && fuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                foreach (var item in fuelPricing.TierPricing.Pricings)
                {
                    if (item.PricingTypeId == (int)PricingType.PricePerGallon)
                    {
                        item.PricePerGallon = item.PricePerGallon;
                    }
                    else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        item.PricePerGallon = item.SupplierCostMarkupValue;
                        item.RackAvgTypeId = fuelPricing.SupplierCostMarkupTypeId;
                        item.SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost;
                        item.SupplierCost = fuelPricing.SupplierCost;
                        item.BaseSupplierCost = fuelPricing.SupplierCost.HasValue ? MoneyConverter.GetBaseAmount(fuelPricing.Currency, fuelPricing.SupplierCost.Value, fuelPricing.ExchangeRate) : 0;
                    }
                    else
                    {
                        item.PricePerGallon = item.RackPrice;
                    }
                }
            }
        }

        private void SetTierPricingBrokerRequestDtls(BrokerFuelRequestDetailsViewModel viewModel)
        {
            if (viewModel.FuelPricing != null && viewModel.FuelPricing.IsTierPricing)
            {
                viewModel.FuelPricing.PricingTypeId = (int)PricingType.Tier;
                foreach (var item in viewModel.FuelPricing.TierPricing.Pricings)
                {
                    if (item.PricingTypeId == (int)PricingType.PricePerGallon)
                    {
                        //item.PricePerGallon = item.PricePerGallon;
                        item.PricePerGallon = HelperDomain.GetPriceWithMargin(viewModel.FuelPriceMargin.Margin, item.PricePerGallon.Value);

                    }
                    else if (item.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        item.PricePerGallon = item.SupplierCostMarkupValue;
                        item.RackAvgTypeId = viewModel.FuelPricing.SupplierCostMarkupTypeId;
                        item.SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost;
                        item.SupplierCost = viewModel.FuelPricing.SupplierCost;
                        item.BaseSupplierCost = viewModel.FuelPricing.SupplierCost.HasValue ? MoneyConverter.GetBaseAmount(viewModel.FuelPricing.Currency, viewModel.FuelPricing.SupplierCost.Value, viewModel.FuelPricing.ExchangeRate) : 0;
                    }
                    else
                    {
                        //item.PricePerGallon = item.RackPrice;
                        item.PricePerGallon = HelperDomain.GetPriceWithMargin(viewModel.FuelPriceMargin.Margin, item.RackPrice.Value);
                    }
                    item.MarginTypeId = viewModel.FuelPriceMargin.MarginTypeId;
                    item.Margin = viewModel.FuelPriceMargin.Margin;


                }
            }
        }

        public async Task<bool> AssignNewTerminalForTierPricedOrder(int? terminalId, int requestPriceDetailsId)
        {
            bool result = false;
            try
            {
                var apiUrl = ApplicationConstants.UrlAssignNewTerminalForOrder;
                var apiMethodUrl = string.Format(apiUrl, terminalId, requestPriceDetailsId);
                var apiResult = await ApiGetCall<BooleanResponseModel>(apiMethodUrl);
                result = apiResult.Result;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "AssignNewTerminalForTierPricedOrder", ex.Message, ex);
            }
            return result;

        }

        public async Task<bool> ResetCommulation()
        {
            bool result = false;
            try
            {

                var apiUrl = ApplicationConstants.UrlResetCumulation;
                result = await ApiGetCall<bool>(apiUrl);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "ResetCommulation", ex.Message, ex);

            }
            return result;
        }

        public async Task<StatusViewModel> UpdateCumulationQuantitiesPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> requestModel)
        {
            var result = new StatusViewModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlUpdateCumulationQuantity;
                var apiResult = await ApiPostCall<bool>(apiUrl, requestModel);
                result.StatusCode = apiResult ? result.StatusCode = Status.Success : result.StatusCode = Status.Failed;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "UpdateCumulationQuantitiesPostInvoiceCreate", ex.Message, ex);
            }
            return result;
        }

        public async Task<PricingDetailResponseModelForExchangeAPI> GetPricingDetailsByIdList(List<int> requestPriceDetailIds, int timeout = 30)
        {
            PricingDetailResponseModelForExchangeAPI result = new PricingDetailResponseModelForExchangeAPI();
            try
            {
                var apiUrl = ApplicationConstants.UrlGetPricingDetailsByIdList;
                result = await ApiPostCall<PricingDetailResponseModelForExchangeAPI>(apiUrl, requestPriceDetailIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetPricingDetailsByIdList", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds)
        {

            try
            {
                var apiUrl = ApplicationConstants.UrlGetSourceRegionForCustomers;

                var jsonList = await ApiPostCall<List<DropdownDisplayExtendedId>>(apiUrl, requestPriceDetailIds);
                if (jsonList != null && jsonList.Any())
                {
                    List<DropdownDisplayExtendedId> soureRegionLst = new List<DropdownDisplayExtendedId>();
                    foreach (var item in jsonList)
                    {
                        var srNames = JsonConvert.DeserializeObject<SourceRegionJSONParameter>(item.Name);
                        soureRegionLst.Add(new DropdownDisplayExtendedId
                        {
                            CodeId = item.CodeId, // buyer company id
                            Name = srNames.SourceRegion 
                        });
                    }
                    return soureRegionLst;                   
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "GetSourceRegionForCustomers", ex.Message, ex);
            }
            return null;
        }

        internal async Task<IntResponseModel> SaveNewMstProduct(ProductViewModel productViewModel, int? tfxProductId,int pricingSourceId = (int)PricingSource.Axxis)
        {
            IntResponseModel result = new IntResponseModel();
            try
            {
                var apiUrl = ApplicationConstants.UrlAddNewProduct;
                var requestModel = GetMstProductRequestObject(productViewModel, tfxProductId, pricingSourceId);
                result = await ApiPostCall<IntResponseModel>(apiUrl, requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PricingServiceDomain", "SaveNewMstProduct", ex.Message, ex);
            }
            return result;
        }

        private object GetMstProductRequestObject(ProductViewModel productViewModel,int?tfxProductId,int pricingSourceId)
        {
            var requestModel = new
            {
                Name = productViewModel.DisplayName,
                Id = productViewModel.Id,
                ProductTypeId = productViewModel.ProductTypeId,
                ProductDisplayGroupId = productViewModel.ProductDisplayGroupId,
                ProductCode = productViewModel.ProductCode,
                IsActive = true,
                UpdatedBy = productViewModel.UpdatedBy,
                UpdatedDate = DateTimeOffset.Now,           
                TfxProductId = tfxProductId,
                PricingSourceId = pricingSourceId,
                DisplayName = productViewModel.ProductName
            };
            return requestModel;
        }
    }
}
