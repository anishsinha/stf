using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class FuelPricingResponseModel : BaseResponseModel
    {
        public FuelPricingResponseModel()
        {
        }
        public FuelPricingResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public decimal PricePerGallon { get; set; }
        public decimal TerminalPrice { get; set; }
        public decimal? FuelCost { get; set; }
        public int? FuelCostTypeId { get; set; }
        public DateTimeOffset PricingDate { get; set; }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
        public Guid Guid { get; set; }
        public WaitingAction WaitingFor { get; set; }
        public int PricingTypeId { get; set; }

        public TierPricingResponseModel TierPricingDetails { get; set; } = new TierPricingResponseModel();
    }

    public class TierPricingResponseModel
    {
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
    }

    public class TierPricingRequestModel
    {
        public decimal? MinQuantity { get; set; }
        public decimal? MaxQuantity { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal? SupplierCost { get; set; }
        public int? SupplierCostTypeId { get; set; }
        public int? RackTypeId { get; set; }
        public int? RackAvgTypeId { get; set; }
        public int PricingTypeId { get; set; }
        public int Currency { get; set; }
        public int PricingSourceId { get; set; }
        public int? FeedTypeId { get; set; }
        public int PricingCodeId { get; set; }
        public int? ProductId { get; set; }
        public int? TierTypeId { get; set; }
        public decimal CumulatedQuantity { get; set; }
    }
}
