using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverAddAssetResponseViewModel : BaseViewModel
    {
        public DriverAddAssetResponseViewModel()
        {
        }

        public DriverAddAssetResponseViewModel(Status status) : base(status)
        {
        }

        public int AssetId { get; set; }

        public int ImageId { get; set; }

        public int JobXAssetId { get; set; }
    }
}
