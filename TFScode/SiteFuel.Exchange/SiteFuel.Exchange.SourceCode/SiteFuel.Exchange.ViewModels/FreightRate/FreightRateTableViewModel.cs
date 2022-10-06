using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightRateTableViewModel
    {
        public FreightRateRuleType FreightRateRuleType { get; set; }

        public List<FreightRateFuelGroupViewModel> FreightRateFuelGroups { get; set; } = new List<FreightRateFuelGroupViewModel>();

        public List<FreightRateRouteTableViewModel> FreightRateRouteTables { get; set; } = new List<FreightRateRouteTableViewModel>();

        public List<FreightRateRangeTableViewModel> FreightRateRangeTables { get; set; } = new List<FreightRateRangeTableViewModel>();
    }
}
