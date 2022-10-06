using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationToBuyerViewModel
    {
        public NotificationToBuyerViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            UncanceledDeliveryScheduleId = new List<int>();
        }

        public int DriverId { get; set; }

        public int OrderId { get; set; }

        public string FCMAppId { get; set; }

        public AppType AppType { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }

        public string Sound { get; set; }

        public bool Notify { get; set; }

        public int DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public List<int> UncanceledDeliveryScheduleId { get; set; }
    }
}
