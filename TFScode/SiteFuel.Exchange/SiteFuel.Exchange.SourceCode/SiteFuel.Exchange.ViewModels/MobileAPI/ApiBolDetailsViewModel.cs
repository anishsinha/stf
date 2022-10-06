using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiBolDetailsViewModel
    {
        public ApiBolDetailsViewModel()
        {

        }

        public decimal GrossQuantity { get; set; }

        public decimal NetQuantity { get; set; }

        public string BolNumber { get; set; }

        public string Carrier { get; set; }

        public int OrderId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public string BolImage { get; set; }

        public bool IsDriverToUpdateBOL { get; set; }

        public int UserId { get; set; }

        public int TimeZoneOffset { get; set; }

        public long LiftDate { get; set; }
    }

    public class ApiBolResponseViewModel : StatusViewModel
    {
        public int BolId { get; set; }

        public int BolImageId { get; set; }
    }
}
