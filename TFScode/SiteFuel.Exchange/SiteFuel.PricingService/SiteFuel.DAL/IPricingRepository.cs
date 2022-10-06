using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{
    public interface IPricingRepository
	{
        Task<PricingResponseModel> GetAxxisPricingDataAsync(int terminalId, int? cityGroupTerminalId, int productId, DateTime priceDate, int pricingCodeId, int timeout = 30);
        Task<PricingResponseModel> GetTerminalPriceForOpisAsync(PriceRequestModel requestModel, int timeout = 30);
        Task<PricingResponseModel> GetTerminalPriceForPlattsAsync(int? terminalId, int productId, DateTime priceDate, int pricingCodeId, int timeout = 30);
        Task<List<PricingData>> GetClosestTerminalPriceAsync(SalesCalculatorRequestModel requestModel, int timeout = 30);
        Task<List<PricingData>> GetOpisTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel, int timeout = 30);
        Task<List<PricingData>> GetPlattsTerminalPricesForCalculatorAsync(SalesCalculatorRequestModel requestModel, int timeout = 30);
        Task<PricingConfigResponse> GetPricingConfigAsync(List<string> keys);
        Task<bool> IsAxxisCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId, DateTime effectiveDate);
        Task<bool> IsOpisCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId);
        Task<bool> IsPlattsCityRackPriceAvailable(int fueltypeId, int cityGroupTerminalId);
        Task<PricingResponseModel> GetPlattsTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel, int timeout = 30);
        Task<IntResponseModel> ExecuteStoredProcedureScalar(string spName, int timeout = 30);
        Task<SyncPricingResponseModel> SyncExternalSourcePricing(int timeout = 30);
        Task<PricingResponseModel> GetTerminalPriceAsync(int productId, int terminalId);
        Task<List<PricingData>> GetCityRackTerminalPricesForCalculator(CityRackPricesRequestModel model, int timeout = 30);
        Task<List<PricingData>> GetTerminalPricesForAuditAsync(TerminalPricesRequestModel model, int timeout = 30);
        Task<List<TerminalDetails>> GetClosestTerminalsAsync(TerminalRequestModel requestModel, int timeout = 30);
        Task<List<DropdownDisplayItem>> GetAxxisProductDetailsAsync(ProductDetailsRequestModel requestModel, int timeout = 30);
        Task<PricingResponseModel> GetAxxisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel, int timeout = 30);
        Task<PricingResponseModel> GetOpisTerminalPriceForCurrentDateAsync(SourceBasedPriceRequestModel requestModel, int timeout = 30);
        Task<IntResponseModel> AddNewProduct(ProductRequestModel product);
        Task<IntResponseModel> AddNewTfxProduct(ProductRequestModel product);
        Task<IntResponseModel> SaveTerminalDetails(PickupLocationDetailViewModel terminal);
        Task<IntResponseModel> UpdateProductDetails(ProductRequestModel product);
        Task<IntResponseModel> UpdateTfxProduct(ProductRequestModel product);
        Task<int> GetPricingSourceIdAsync(int? requestPriceId);
        Task<List<RequestPriceDetailModel>> GetRequestPriceDetailsAsync(List<int> requestPriceDetailIds);
        Task<DateTime> GetLastUpdatedPricingDate(int requestPriceDetailId);
        Task<int> GetSourceFromPriceCodeAsync(int pricingCodeId);
        Task<int> GetPriceCodeId(int requestPriceId);
        Task<List<TierPricingRequestModel>> GetTierPricingReqModel(int requestPriceId, decimal maxQuantity);
        Task<PricingConfigResponseModel> GetPricingConfigDetailsAsync(int id = 0);
        Task<PricingConfigResponseModel> EditPricingConfigAsync(PricingConfigModel model);
        Task<List<TerminalDetails>> GetClosestTerminalsForFueltypesAsync(TerminalForFueltypesRequestModel requestModel, int timeout = 30);
        Task<List<DropdownDisplayItem>> GetAllTerminalsAsync();
        Task<SyncPricingResponseModel> SyncExternalActualOPISPricing(int timeout = 30);

        Task<BooleanResponseModel> AssignNewTerminalForTierPricedOrder(int? terminalId, int requestPricingDetailsId);

        Task<bool> ResetCumulation();
        Task<bool> UpdateCumulationQtyPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> cumulationQtyList);
        Task<List<TierPricingRequestModel>> GetPricingForVolBasedTierWithCumulationReset(int requestPriceId, decimal fromQuantity, decimal toQuantity);
        Task<List<TerminalDetails>> GetClosestTerminalsByDistanceAsync(TerminalRequestViewModel requestModel, int timeout = 30);

        Task<List<DropdownDisplayExtendedId>> GetSourceRegionForCustomers(List<DropdownDisplayExtendedId> requestPriceDetailIds);
    }
}
