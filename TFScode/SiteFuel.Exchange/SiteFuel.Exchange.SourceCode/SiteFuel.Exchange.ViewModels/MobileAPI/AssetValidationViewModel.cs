using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetValidationViewModel : ResponseViewModel
    {
        public AssetValidationViewModel()
        {
            AssetFuelRequestsList = new List<AssetFuelRequestInputViewModel>();
            InvalidAssetList = new List<string>();
        }
        public List<AssetFuelRequestInputViewModel> AssetFuelRequestsList { get; set; }
        public List<string> InvalidAssetList { get; set; }
    }
}
