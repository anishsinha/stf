using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Models
{
    public class PricingResponseModel : BaseResponseModel
    {
        public PricingResponseModel()
        {
        }
        public PricingResponseModel(Status status) : base(status)
        {
            Status = status;
        }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
        public int Currency { get; set; }
        public decimal Price { get; set; }
        public decimal AvgPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }
        public int PricingTypeId { get; set; }
    }
}
