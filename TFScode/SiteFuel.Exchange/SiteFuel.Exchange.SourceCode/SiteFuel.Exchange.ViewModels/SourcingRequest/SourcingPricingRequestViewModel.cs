using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourcingPricingRequestViewModel
    {
        public int Id { get; set; }

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

        public int? AcceptedCompanyId { get; set; }

        public int? FuelTypeId { get; set; }

        public int? StateId { get; set; }

        public int? PricingSourceId { get; set; }

        public int PricingTypeId { get; set; }

        public int? RackTypeId { get; set; }

        public int? FeedTypeId { get; set; }

        public int? QuantityIndicatorId { get; set; }

        public int? FuelClassTypeId { get; set; }

        public int? WeekendPricingTypeId { get; set; }

        public SourcingTierPricingViewModel TierPricing { get; set; } 
        public int RequestPriceDetailId { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public string ParameterJson { get; set; }
        public bool EnableCityRack { get; set; }
    }
}
