using System;
using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class FilterPricingRequestViewModel
    {
        public int PricingType { get; set; }
        public List<int> PriceDetailIds { get; set; }
    }
}
