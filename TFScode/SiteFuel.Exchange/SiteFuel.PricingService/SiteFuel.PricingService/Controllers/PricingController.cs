using SiteFuel.BAL;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using SiteFuel.PricingService.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.PricingService.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class PricingController : ApiController
    {
        private readonly IPricingDomain _pricingDomain;

        public PricingController(IPricingDomain pricingDomain)
        {
            _pricingDomain = pricingDomain;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<PricingResponseModel> GetTerminalPriceAsync(PriceRequestModel requestModel)
        {
            PricingResponseModel response  = await _pricingDomain.GetTerminalPriceAsync(requestModel);            
            return response;
        }

        [HttpGet]
        public async Task<PricingResponseModel> GetTerminalPriceAsync(int productId, int terminalId)
        {
            PricingResponseModel response = await _pricingDomain.GetTerminalPriceAsync(productId, terminalId);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<List<FuelPricingResponseModel>> GetFuelPriceAsync(List<FuelPriceRequestModel> requestModel)
        {
            List<FuelPricingResponseModel> response = await _pricingDomain.GetFuelPriceAsync(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<SalesCalculatorResponseModel> GetTerminalPricesForSalesCalculatorAsync(SalesCalculatorRequestModel inputModel)
        {
            SalesCalculatorResponseModel response;

            switch (inputModel.PricingSourceId)
            {
                case (int)PricingSource.Axxis:
                    response = await _pricingDomain.GetClosestTerminalPriceAsync(inputModel);
                    break;
                case (int)PricingSource.OPIS:
                    response = await _pricingDomain.GetOpisTerminalPricesForCalculatorAsync(inputModel);
                    break;
                case (int)PricingSource.PLATTS:
                    response = await _pricingDomain.GetPlattsTerminalPricesForCalculatorAsync(inputModel);
                    break;
                default:
                    response = new SalesCalculatorResponseModel(Status.Failed) { Message = Resource.valMessagePricingSourceIdNotSupported };
                    break;
            }
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<SalesCalculatorResponseModel> GetCityRackTerminalPricesAsync(CityRackPricesRequestModel requestModel)
        {
            SalesCalculatorResponseModel response = await _pricingDomain.GetCityRackTerminalPricesForCalculator(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<SalesCalculatorResponseModel> GetTerminalPricesForAuditAsync(TerminalPricesRequestModel requestModel)
        {
            SalesCalculatorResponseModel response = await _pricingDomain.GetTerminalPricesForAuditAsync(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<TerminalResponseModel> GetClosestTerminalsAsync(TerminalRequestModel inputModel)
        {
            TerminalResponseModel response = await _pricingDomain.GetClosestTerminalsAsync(inputModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<ProductDetailsResponseModel> GetAxxisProductDetailsAsync(ProductDetailsRequestModel requestModel)
        {
            ProductDetailsResponseModel response = await _pricingDomain.GetAxxisProductDetailsAsync(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<PricingResponseModel> GetLatestTerminalPriceAsync(SourceBasedPriceRequestModel inputModel)
        {
            PricingResponseModel response;

            switch (inputModel.PricingSourceId)
            {
                case (int)PricingSource.Axxis:
                    response = await _pricingDomain.GetAxxisTerminalPriceForCurrentDateAsync(inputModel);
                    break;
                case (int)PricingSource.OPIS:
                    response = await _pricingDomain.GetOpisTerminalPriceForCurrentDateAsync(inputModel);
                    break;
                case (int)PricingSource.PLATTS:
                    response = await _pricingDomain.GetPlattsTerminalPriceForCurrentDateAsync(inputModel);
                    break;
                default:
                    response = new PricingResponseModel(Status.Failed) { Message = Resource.valMessagePricingSourceIdNotSupported };
                    break;
            }
            return response;
        }

        [HttpGet]
        public async Task<DateTime> GetLastUpdatedPricingDate(int requestPriceDetailId)
        {
            DateTime response = await _pricingDomain.GetLastUpdatedPricingDate(requestPriceDetailId);
            return response;
        }

        [HttpGet]
        public async Task<BooleanResponseModel> IsCityRackPriceAvailable(int productId, int cityGroupTerminalId, int pricingSourceId, DateTime? effectiveDate)
        {
            BooleanResponseModel response;

            switch (pricingSourceId)
            {
                case (int)PricingSource.Axxis:
                    if (effectiveDate == null)
                    {
                        effectiveDate = DateTime.Now;
                    }
                    response = await _pricingDomain.IsAxxisCityRackPriceAvailable(productId, cityGroupTerminalId, effectiveDate.Value);
                    break;
                case (int)PricingSource.OPIS:
                    response = await _pricingDomain.IsOpisCityRackPriceAvailable(productId, cityGroupTerminalId);
                    break;
                case (int)PricingSource.PLATTS:
                    response = await _pricingDomain.IsPlattsCityRackPriceAvailable(productId, cityGroupTerminalId);
                    break;
                default:
                    response = new BooleanResponseModel(Status.Failed) { Message = Resource.valMessagePricingSourceIdNotSupported };
                    break;
            }
            return response;
        }

        [HttpGet]
        public async Task<IntResponseModel> SyncAxxisPricingData()
        {
            var response = await _pricingDomain.SyncAxxisPricingData();
            return response;
        }

        [HttpGet]
        public async Task<SyncPricingResponseModel> SyncOpisPlattsPricingData()
        {
            var response = await _pricingDomain.SyncOpisPlattsPricingData();
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<PricingConfigResponse> GetPricingConfig(List<string> keys)
        {
            var response = await _pricingDomain.GetPricingConfigAsync(keys);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<IntResponseModel> AddNewProduct(ProductRequestModel product)
        {
            var response = await _pricingDomain.AddNewProduct(product);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<IntResponseModel> AddNewTfxProduct(ProductRequestModel product)
        {
            var response = await _pricingDomain.AddNewTfxProduct(product);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<IntResponseModel> UpdateTfxProduct(ProductRequestModel product)
        {
            var response = await _pricingDomain.UpdateTfxProduct(product);
            return response;
        }

        [HttpGet]
        public async Task<PricingConfigResponseModel> GetPricingConfigDetails(int id = 0)
        {
            var response = await _pricingDomain.GetPricingConfigDetailsAsync(id);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<PricingConfigResponseModel> EditPricingConfig(PricingConfigModel model)
        {
            var response = await _pricingDomain.EditPricingConfigAsync(model);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<TerminalResponseModel> GetClosestTerminalsForFueltypesAsync(TerminalForFueltypesRequestModel inputModel)
        {
            TerminalResponseModel response = await _pricingDomain.GetClosestTerminalsForFueltypesAsync(inputModel);
            return response;
        }

        [ValidateToken]
        [HttpGet]
        public async Task<List<DropdownDisplayItem>> GetAllTerminalsAsync()
        {
            var response = await _pricingDomain.GetAllTerminalsAsync();
            return response;
        }

        [HttpGet]
        public async Task<SyncPricingResponseModel> SyncActualOpisPricingData()
        {
            var response = await _pricingDomain.SyncActualOpisPricingData();
            return response;
        }

        [HttpGet]
        public async Task<IntResponseModel> SyncDyedProductPricingFromClearProducts()
        {
            var response = await _pricingDomain.SyncDyedProductPricingFromClearProducts();
            return response;
        }

        [HttpGet]
        public async Task<BooleanResponseModel> AssignNewTerminalForTierPricedOrder(int? terminalId , int requestPriceDetailsId)
        {
           var response =  await _pricingDomain.AssignNewTerminalForTierPricedOrder(terminalId, requestPriceDetailsId);
           return response;
        }

        [HttpGet]
        public async Task<bool> ResetCumulation()
        {
            var response = await _pricingDomain.ResetCumulation();
            return response;
        }

        [HttpPost]
        public async Task<bool> UpdateCumulationQtyPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> cumulationQtyList)
        {
            var response = await _pricingDomain.UpdateCumulationQtyPostInvoiceCreate(cumulationQtyList);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<TerminalResponseModel> GetClosestTerminalsByDistanceAsync(TerminalRequestViewModel inputModel)
        {
            TerminalResponseModel response = await _pricingDomain.GetClosestTerminalsByDistanceAsync(inputModel);
            return response;
        }

        [HttpPost]
        public async Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds)
        {
            var response = await _pricingDomain.GetSourceRegionForCustomers(requestPriceDetailIds);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<IntResponseModel> SaveTerminalDetails(PickupLocationDetailViewModel terminal)
        {
            var response = await _pricingDomain.SaveTerminalDetails(terminal);
            return response;
        }
    }
}
