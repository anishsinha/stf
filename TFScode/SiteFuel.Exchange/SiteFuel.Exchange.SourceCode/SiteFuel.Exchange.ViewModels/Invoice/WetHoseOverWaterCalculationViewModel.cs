using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class WetHoseOverWaterCalculationViewModel : StatusViewModel
    {
        public WetHoseOverWaterCalculationViewModel()
        {
           
        }

        public WetHoseOverWaterCalculationViewModel(Status status)
            : base(status)
        {
           
        }


        public decimal Fee { get; set; }

        public decimal WetHoseFee { get; set; }

        public decimal OverWaterFee { get; set; }

        public decimal WetHoseAssetQuantity { get; set; }

        public decimal OverWaterAssetQuantity { get; set; }

        public string WetHoseHours { get; set; }

        public string OverWaterHours { get; set; }
    }
}

