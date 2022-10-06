using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverAssetDropHistoryViewModel : StatusViewModel
    {
        public DriverAssetDropHistoryViewModel()
        {
            InstanceInitialize();
        }

        public DriverAssetDropHistoryViewModel(Status status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DropHistory = new List<AssetDropHistoryViewModel>();
        }

        public AssetViewModel Asset { get; set; }

        public List<AssetDropHistoryViewModel> DropHistory { get; set; }
    }
}
