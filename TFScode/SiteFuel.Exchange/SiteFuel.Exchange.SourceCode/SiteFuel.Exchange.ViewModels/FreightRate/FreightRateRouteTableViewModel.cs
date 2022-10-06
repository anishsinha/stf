using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightRateRouteTableViewModel
    {
        public decimal StartQuantity { get; set; }

        public decimal? EndQuantity { get; set; }

        public decimal RateValue { get; set; }

        public int FuelGroupId { get; set; }

        public string FuelGroupName { get; set; }
    }
}
