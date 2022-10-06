using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AccessorialFuelFeeViewModel : BaseCultureViewModel
    {
        public AccessorialFuelFeeViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            FuelFees = new List<FeesViewModel>();
        }

        public List<FeesViewModel> FuelFees { get; set; }
    }
}
