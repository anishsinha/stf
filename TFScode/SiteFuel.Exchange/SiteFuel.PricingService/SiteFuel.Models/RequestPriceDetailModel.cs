namespace SiteFuel.Models
{
    public class RequestPriceDetailModel
    {
        public decimal PricePerGallon { get; set; }
        public decimal? SupplierCost { get; set; }
        public int? SupplierCostTypeId { get; set; }
        public int? RackTypeId { get; set; }
        public int? RackAvgTypeId { get; set; }
        public int PricingTypeId { get; set; }
        public int Currency { get; set; }
        public int PricingSourceId { get; set; }
        public int? FeedTypeId { get; set; }
        public int RequestPriceDetailId { get; set; }
    }
}
