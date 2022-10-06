using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverViewModel : StatusViewModel
    {
        public DriverViewModel()
        {
        }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Comment { get; set; }

        public string FCMAppId { get; set; }

        public int AssetFilled { get; set; }

        public DriverViewModel Clone()
        {
            return (DriverViewModel)this.MemberwiseClone();
        }
    }
}
