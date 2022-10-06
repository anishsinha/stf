namespace SiteFuel.Models
{
    public class PricingCodesRequestModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Search { get; set; }

        public int? PricingSourceId { get; set; }

        public int? PricingTypeId { get; set; }

        public int? FeedTypeId { get; set; }

        public int? QuantityIndicatorId { get; set; }

        public int? FuelClassTypeId { get; set; }

        public int? RackTypeId { get; set; }

        public int? WeekendPricingTypeId { get; set; }

        public int? TFxProductId { get; set; }
    }
}
