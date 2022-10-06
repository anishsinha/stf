using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetDuplicateGridViewModel
    {
        public AssetDuplicateGridViewModel()
        {
            AssetDuplicates = new List<AssetDuplicateViewModel>();
        }

        public int JobId { get; set; }

        public List<AssetDuplicateViewModel> AssetDuplicates { get; set; }
    }
}
