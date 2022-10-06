using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
    public class ForecastingEstimatedUsageViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TankName { get; set; }
        public string UsagePeriod { get; set; }
        public string AverageBusinessDayUsage { get; set; }
        public string TotalExceptedUsage { get; set; }
        public string MaximumBusinessdayUsage { get; set; }
    }
}
