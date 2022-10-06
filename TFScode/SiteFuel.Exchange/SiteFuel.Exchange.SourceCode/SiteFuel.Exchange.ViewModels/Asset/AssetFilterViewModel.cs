using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class AssetFilterViewModel : BaseInputViewModel
    {
        public AssetFilterViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = AssetFilterType.All;
            IsJobAssetBulkUploadEnabled = false;
            IsJobDetails = false;
            IsAssignAssets = false;
        }

        public bool DuplicateExists { get; set; }

        public int JobId { get; set; }

        public int OrderId { get; set; }

        public int BuyerId { get; set; }

        public bool IsJobAssetBulkUploadEnabled { get; set; }

        public AssetFilterType Filter { get; set; }

        public bool IsJobDetails { get; set; }

        public bool IsRetailJob { get; set; }

        public bool IsAssignAssets { get; set; }

        public AssetType Type { get; set; }
    }
}
