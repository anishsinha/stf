using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverOrderAssetViewModel : StatusViewModel
    {
        public DriverOrderAssetViewModel()
        {
            InstanceInitialize();
        }

        public DriverOrderAssetViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            Asset = new AssetViewModel(status);
            FuelDrop = new DriverFuelDropViewModel();
        }

        public AssetViewModel Asset { get; set; }

        public DriverFuelDropViewModel FuelDrop { get; set; }
    }
}
