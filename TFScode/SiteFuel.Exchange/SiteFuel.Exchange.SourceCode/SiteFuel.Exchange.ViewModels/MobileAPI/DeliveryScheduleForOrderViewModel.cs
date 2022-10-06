using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleForOrderViewModel
    {
        public int DeliveryScheduleId { get; set; }

        public decimal GallonsOrdered { get; set; }

        public DateTimeOffset ScheduleDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int TrackableScheduleId { get; set; }

        public DateTimeOffset Date { get; set; }
    }
}
