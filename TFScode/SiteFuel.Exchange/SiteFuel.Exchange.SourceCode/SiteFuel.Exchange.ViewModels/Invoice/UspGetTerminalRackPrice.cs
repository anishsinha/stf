using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetTerminalRackPrice
    {
        public decimal AvgPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal HighPrice { get; set; }
        public int RackTypeId { get; set; }
        public int PricingTypeId { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public Currency Currency { get; set; }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
    }
}
