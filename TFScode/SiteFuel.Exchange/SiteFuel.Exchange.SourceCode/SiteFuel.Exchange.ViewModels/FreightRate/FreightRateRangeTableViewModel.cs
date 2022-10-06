using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class FreightRateRangeTableViewModel
    {
        public decimal UptoQuantity { get; set; }

        public decimal RateValue { get; set; }

        public int FuelGroupId { get; set; }

        public string FuelGroupName { get; set; }
    }
}
