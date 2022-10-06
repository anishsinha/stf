using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ScheduleEditRequestViewModel
    {
        public int OrderId { get; set; }
        public bool IsFtlOrder { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public int EnrouteStatus { get; set; } = (int)EnrouteDeliveryStatus.Unknown;
        public int CountryId { get; set; }
        public Currency Currency { get; set; }
        public string CountryCode { get; set; }
    }
}
