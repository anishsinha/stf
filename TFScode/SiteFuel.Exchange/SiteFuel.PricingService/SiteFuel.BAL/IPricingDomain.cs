using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IPricingDomain
    {
        Task<PricingResponseModel> GetTerminalPriceAsync(PriceRequestModel requestModel, int? pricingSourceId = null);
        Task<PricingResponseModel> GetTerminalPriceAsync(int productId, int terminalId);
        Task<List<FuelPricingResponseModel>> GetFuelPriceAsync(List<FuelPriceRequestModel> requestModels);
        Task<PricingConfigResponse> GetPricingConfigAsync(List<string> keys);
        Task<SalesCalculatorResponseModel> GetClosestTerminalPriceAsync(SalesCalculatorRequestModel requestModel);
        Task<SalesCalculatorResponseModel> GetOpisTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel);
        Task<SalesCalculatorResponseModel> GetPlattsTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel);
        Task<BooleanResponseModel> IsAxxisCityRackPriceAvailable(int productId, int cityGroupTerminalId, DateTime effectiveDate);
        Task<BooleanResponseModel> IsOpisCityRackPriceAvailable(int productId, int cityGroupTerminalId);
        Task<BooleanResponseModel> IsPlattsCityRackPriceAvailable(int productId, int cityGroupTerminalId);
        Task<IntResponseModel> SyncAxxisPricingData();
        Task<SyncPricingResponseModel> SyncOpisPlattsPricingData();
        Task<SalesCalculatorResponseModel> GetCityRackTerminalPricesForCalculator(CityRackPricesRequestModel requestModel);
        Task<SalesCalculatorResponseModel> GetTerminalPricesForAuditAsync(TerminalPricesRequestModel requestModel);
        Task<TerminalResponseModel> GetClosestTerminalsAsync(TerminalRequestModel requestModel);
        Task<ProductDetailsResponseModel> GetAxxisProductDetailsAsync(ProductDetailsRequestModel requestModel);
        Task<PricingResponseModel> GetAxxisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel);
        Task<PricingResponseModel> GetOpisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel);
        Task<PricingResponseModel> GetPlattsTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel);
        Task<IntResponseModel> SaveTerminalDetails(PickupLocationDetailViewModel terminal);
        Task<IntResponseModel> AddNewProduct(ProductRequestModel product);
        Task<IntResponseModel> AddNewTfxProduct(ProductRequestModel product);
        Task<IntResponseModel> UpdateTfxProduct(ProductRequestModel product);
        Task<DateTime> GetLastUpdatedPricingDate(int requestPriceDetailId);
        Task<PricingConfigResponseModel> GetPricingConfigDetailsAsync(int id = 0);
        Task<PricingConfigResponseModel> EditPricingConfigAsync(PricingConfigModel model);
        Task<TerminalResponseModel> GetClosestTerminalsForFueltypesAsync(TerminalForFueltypesRequestModel requestModel);
        Task <List<DropdownDisplayItem>> GetAllTerminalsAsync();
        Task<SyncPricingResponseModel> SyncActualOpisPricingData();
        Task<IntResponseModel> SyncDyedProductPricingFromClearProducts();
        Task<BooleanResponseModel> AssignNewTerminalForTierPricedOrder(int? terminalId, int requestPricingDetailsId);
        Task<bool> ResetCumulation();
        Task<bool> UpdateCumulationQtyPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> cumulationQtyUpdateList);
        Task<TerminalResponseModel> GetClosestTerminalsByDistanceAsync(TerminalRequestViewModel requestModel);
        Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds);
    }
}