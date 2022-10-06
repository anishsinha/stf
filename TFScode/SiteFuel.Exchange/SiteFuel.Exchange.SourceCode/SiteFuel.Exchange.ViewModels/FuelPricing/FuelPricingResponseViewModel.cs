using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelPricingResponseViewModel : StatusViewModel
    {
        public decimal PricePerGallon { get; set; }
        public decimal TerminalPrice { get; set; }
        public DateTimeOffset PricingDate { get; set; }
        public WaitingAction WaitingFor { get; set; }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
        public decimal? FuelCost { get; set; }
        public int? FuelCostTypeId { get; set; }
        public int PricingTypeId { get; set; }
        public Guid Guid { get; set; }

        public TierPricingResponseModel TierPricingDetails { get; set; } = new TierPricingResponseModel();
    }

    public class TierPricingResponseModel
    {
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
    }

    public class BooleanResponseModel
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public bool Result { get; set; }
    }

    public class IntResponseModel
    {
        public Status Status { get; set; }
        public int Result { get; set; }
        public List<int> ListResult { get; set; }
    }

    public class CustomResponseModel
    {
        public Status Status { get; set; }
        public int Result { get; set; }
        public string CustomString1 { get; set; }
        public string CustomString2 { get; set; }
    }

    public class PriceUpdatedDateResponseModel
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
    }


    public class PricingConfigResponse
    {
        public Status Status { get; set; }
        public List<PricingConfig> Configs { get; set; }
    }

    public class PricingConfig
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SyncPricingResponseModel
    {
        public Status Status { get; set; }

        public List<SyncPricingResponse> PricingResponse { get; set; }
    }

    public class SyncPricingResponse
    {
        public int RecordInserted { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SourceId { get; set; }
        public string ProductCode { get; set; }
        public int? TfxProductId { get; set; }
        public int? MstProductId { get; set; }
    }

    public class CurrentCostRequestModel
    {
        public decimal Cost { get; set; }
        public int SupplierCostType { get; set; }
        public List<int> RequestPriceDetailIds { get; set; }
    }

    public class CurrentCostResponseModel
    {
        public Status Status { get; set; }
        public List<CostResponseModel> Cost { get; set; } = new List<CostResponseModel>();
    }

    public class CostResponseModel
    {
        public int PriceDetailId { get; set; }
        public decimal previousCost { get; set; }
        public int previousCostType { get; set; }
    }
    public class FilterPricingRequestViewModel
    {
        public int PricingType { get; set; }
        public List<int> PriceDetailIds { get; set; }
    }

    public class JobAddtionalDetailsViewModel
    {
        public string SiteImageFilePath { get; set; }
        public string AdditionalImageFilePath { get; set; }
        public string AdditionalImageDescription { get; set; }
        public List<int> DeliveryDays { get; set; }
        public DateTimeOffset FromDeliveryTime { get; set; }
        public DateTimeOffset ToDeliveryTime { get; set; }
        public int JobId { get; set; }
        public bool IsActive { get; set; }

    }
}
