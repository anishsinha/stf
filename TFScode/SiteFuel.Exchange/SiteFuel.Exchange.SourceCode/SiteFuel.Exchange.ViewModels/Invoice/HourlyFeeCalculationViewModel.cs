using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class HourlyFeeCalculationViewModel : StatusViewModel
    {
        public HourlyFeeCalculationViewModel()
        {
            
        }

        public HourlyFeeCalculationViewModel(Status status)
            : base(status)
        {
            
        }

        public decimal Fee { get; set; }

        public decimal HourlyFee { get; set; }

        public string TotalHours { get; set; }
    }
}

