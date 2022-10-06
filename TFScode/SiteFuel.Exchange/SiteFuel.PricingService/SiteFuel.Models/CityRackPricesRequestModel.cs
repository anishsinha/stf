using SiteFuel.Exchange.Core;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class CityRackPricesRequestModel
    {
        public DateTime PriceDate { get; set; }

        public int ExternalProductId { get; set; }

        public string StateOrTerminalIds { get; set; }

        public int CityTerminalPricingType { get; set; }
    }
}
