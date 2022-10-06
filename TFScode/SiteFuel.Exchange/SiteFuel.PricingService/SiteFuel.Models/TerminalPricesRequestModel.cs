using SiteFuel.Exchange.Core;
using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class TerminalPricesRequestModel
    {
        public DateTime PricingDate { get; set; }

        public int ExternalProductId { get; set; }

        public decimal SrcLatitude { get; set; }

        public decimal SrcLongitude { get; set; }

        public int RecordCount { get; set; }
    }
}
