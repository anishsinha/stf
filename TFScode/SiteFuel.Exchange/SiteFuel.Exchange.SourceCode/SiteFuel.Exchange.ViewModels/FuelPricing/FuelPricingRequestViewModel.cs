using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelPricingRequestViewModel
    {
        public int FuelTypeId { get; set; }
        public int? TerminalId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public int PricingTypeId { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public Currency Currency { get; set; }
        public int? RackAvgTypeId { get; set; }
        public decimal? SupplierCost { get; set; }
        public decimal PricePerGallon { get; set; }
        public WaitingAction WaitingFor { get; set; }
        public decimal? DroppedQuantity { get; set; }
        public decimal TierMinQuantity { get; set; }
        public decimal TierMaxQuantity { get; set; }

        public FuelRequestPricingDetailsViewModel FuelRequestPricingDetails { get; set; } = new FuelRequestPricingDetailsViewModel();

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

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"FuelTypeId:{FuelTypeId},TerminalId:{TerminalId},CityGroupTerminalId:{CityGroupTerminalId},");
            stringBuilder.Append($"PricingTypeId:{PricingTypeId},DropEndDate:{DropEndDate},RackAvgTypeId:{RackAvgTypeId},");
            stringBuilder.Append($"FeedTypeId:{FuelRequestPricingDetails?.FeedTypeId},FuelClassTypeId:{FuelRequestPricingDetails?.FuelClassTypeId},");
            stringBuilder.Append($"PricingSourceId:{FuelRequestPricingDetails?.PricingSourceId},PricingSourceQuantityIndicatorTypeId:{FuelRequestPricingDetails?.PricingSourceQuantityIndicatorTypeId},");
            return stringBuilder.ToString();
        }

        public string ToPricingServiceQueryString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"PriceDate={DropEndDate.Date.ToShortDateString()}&");
            stringBuilder.Append($"CityRackTerminalId={CityGroupTerminalId}&");
            stringBuilder.Append($"ProductId={FuelTypeId}&");
            stringBuilder.Append($"FeedTypeId={FuelRequestPricingDetails?.FeedTypeId}&");
            stringBuilder.Append($"RackTypeId={PricingTypeId}&");
            stringBuilder.Append($"SupplierBrandId={FuelRequestPricingDetails?.FuelClassTypeId}&");
            stringBuilder.Append($"QuantityIndicatorTypeId={FuelRequestPricingDetails?.PricingSourceQuantityIndicatorTypeId}&");
            stringBuilder.Append($"WeekendDropPricingDay={(int)FuelRequestPricingDetails.WeekendDropPricingDay}&");
            stringBuilder.Append($"PricingSourceId={FuelRequestPricingDetails?.PricingSourceId}&");
            stringBuilder.Append($"TerminalId={CityGroupTerminalId}");
            return stringBuilder.ToString();
        }
    }
}
