using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FuelPricing
{
    public class UspSourceBasedTerminalPrice
    {
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset PriceLastUpdatedDate { get; set; }
    }
}
