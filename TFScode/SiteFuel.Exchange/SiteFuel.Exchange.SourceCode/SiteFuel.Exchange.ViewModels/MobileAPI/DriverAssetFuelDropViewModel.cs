using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverAssetFuelDropViewModel : BaseViewModel
    {
        public DriverAssetFuelDropViewModel()
        {
            InstanceInitialize();
        }

        public DriverAssetFuelDropViewModel(Status status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Driver = new DriverViewModel();
            FuelDrop = new DriverFuelDropViewModel();
        }

        public DriverViewModel Driver { get; set; }

        public DriverFuelDropViewModel FuelDrop { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }
    }
}
