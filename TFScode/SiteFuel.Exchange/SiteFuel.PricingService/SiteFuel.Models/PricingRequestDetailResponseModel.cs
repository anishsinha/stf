using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Net;

namespace SiteFuel.Models
{
    public class PricingRequestDetailResponseModel : BaseResponseModel
    {
        public PricingRequestDetailModel PricingRequestDetail { get; set; }
    }

    public class PricingRequestDetailModel
    {
        public int RequestPriceDetailId { get; set; }

        public int PricingCodeId { get; set; }

        public Nullable<int> RackAvgTypeId { get; set; }

        public decimal PricePerGallon { get; set; }

        public decimal? SupplierCost { get; set; }

        public int? SupplierCostTypeId { get; set; }

        public Nullable<int> MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        public decimal BasePrice { get; set; }

        public decimal? BaseSupplierCost { get; set; }

        public int Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public int UoM { get; set; }

        public string PricingCode { get; set; }

        public int PricingSourceId { get; set; }

        public int PricingTypeId { get; set; }

        public int RackTypeId { get; set; }

        public int? CityRackTerminalId { get; set; }

        public int? FuelTypeId { get; set; }

        public int FeedTypeId { get; set; }

        public int QuantityIndicatorId { get; set; }

        public int FuelClassTypeId { get; set; }

        public int WeekendPricingTypeId { get; set; }

        public bool IsActive { get; set; }
   

        public List<TierPricing> TierPricings { get; set; } = new List<TierPricing>();

        public string SourceRegionJsonParameters { get; set; }


    }
    public class TierPricing: PricingRequestDetailModel
    {
     
        public int? TerminalId { get; set; }
        public decimal MinQuantity { get; set; }
        public decimal MaxQuantity { get; set; }
        public int TierTypeId { get; set; }
        public int? CumulationTypeId { get; set; }
        public int? CumulationResetDay { get; set; }
        public DateTimeOffset? CumulationResetDate { get; set; }
        
    }

    public class CumulationQuantityUpdateViewModel
    {
        public decimal DroppedGallons { get; set; }
        public int RequestPriceDetailsId { get; set; }

        public InvoiceType InvoiceType { get; set; }
    }
}