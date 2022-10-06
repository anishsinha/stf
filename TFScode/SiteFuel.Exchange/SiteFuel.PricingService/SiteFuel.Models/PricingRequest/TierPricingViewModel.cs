using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Models
{
    public class TierPricingViewModel
    {
   
        public TierPricingType TierPricingType { get; set; } = TierPricingType.VolumeBased;

        public bool IsResetCumulation { get; set; } = false;

        public PricingViewModel AboveQuantityPricing { get; set; } = new PricingViewModel();

        public List<PricingViewModel> Pricings { get; set; }

        public CumulationSetting ResetCumulationSetting { get; set; } = new CumulationSetting();
        public int RequestPriceDetailId { get; set; }
    }

    public class PricingViewModel
    {
        public int? RequestPriceDetailId { get; set; }
        public decimal FromQuantity { get; set; } = 0;
        public decimal ToQuantity { get; set; }
        public decimal? Quantity { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public int? CityGroupTerminalStateId { get; set; }
        public string DisplayPrice { get; set; }
        public Nullable<int> RackAvgTypeId { get; set; }
        public decimal? PricePerGallon { get; set; }
        public Nullable<int> SupplierCostMarkupTypeId { get; set; }
        public decimal? SupplierCostMarkupValue { get; set; }
        public Nullable<int> MarginTypeId { get; set; }
        public decimal? Margin { get; set; }
        public decimal? BasePrice { get; set; }
        public decimal? RackPrice { get; set; }
        public decimal? BaseSupplierCost { get; set; }
        public Currency Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public UoM UoM { get; set; }
        public int? FuelTypeId { get; set; }
        public int JobId { get; set; }
        public int PricingSourceId { get; set; }
        public int PricingTypeId { get; set; } = (int)PricingType.PricePerGallon;
        public int RackTypeId { get; set; }
        public bool IncludeTaxes { get; set; }
        public bool IsActive { get; set; }
        public bool IsAboveQuantity { get; set; }
        public int? RowIndex { get; set; }
        public int? TotalRows { get; set; }
        public string ParameterJson { get; set; }
        public PricingCodeDetailViewModel PricingCode { get; set; } = new PricingCodeDetailViewModel();

        public bool IsTerminalPrice()
        {
            return PricingTypeId == (int)PricingType.RackAverage
                    || PricingTypeId == (int)PricingType.RackLow
                    || PricingTypeId == (int)PricingType.RackHigh;
        }
        public bool IsSupplierCostPrice()
        {
            return PricingTypeId == (int)PricingType.Suppliercost;
        }
        public bool IsFixedPrice()
        {
            return PricingTypeId == (int)PricingType.PricePerGallon;
        }
    }

    public class CumulationSetting
    {
        public CumulationType CumulationType { get; set; }

        //public int? Day { get; set; }
        public WeekDay? Day { get; set; }
        public DateTimeOffset? Date { get; set; }
    }

    public class PricingCodeDetailViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
