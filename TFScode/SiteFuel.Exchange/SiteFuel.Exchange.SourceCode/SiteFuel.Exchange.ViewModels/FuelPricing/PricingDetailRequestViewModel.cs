namespace SiteFuel.Exchange.ViewModels
{
    public class PricingDetailRequestViewModel
    {
        public int Id { get; set; }

        public int Currency { get; set; }

        public int? AcceptedCompanyId { get; set; }

        public int? PricingCodeId { get; set; }

        public int? FuelTypeId { get; set; }

        public int? StateId { get; set; }

        public int? PricingSourceId { get; set; }

        public int? PricingTypeId { get; set; }

        public int? RackTypeId { get; set; }

        public int? FeedTypeId { get; set; }

        public int? QuantityIndicatorId { get; set; }

        public int? FuelClassTypeId { get; set; }

        public int? WeekendPricingTypeId { get; set; }
    }
}
