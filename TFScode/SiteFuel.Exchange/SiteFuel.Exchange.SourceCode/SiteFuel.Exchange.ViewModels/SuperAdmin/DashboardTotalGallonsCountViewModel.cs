using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardTotalGallonsCountViewModel
    { 
        public decimal? TotalCount { get; set; }

        public decimal? TotalOrderedDeliveredCount { get; set; }
    }
}
