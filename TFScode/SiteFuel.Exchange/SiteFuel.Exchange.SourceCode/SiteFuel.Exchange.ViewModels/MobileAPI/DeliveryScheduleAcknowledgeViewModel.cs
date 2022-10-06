using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels.MobileAPI
{
    public class DeliveryScheduleAcknowledgeViewModel
    {
        public int TrackableScheduleId { get; set; }
        public DriverAcknowledgementStatus Status { get; set; }
        public int UserTimeOffset { get; set; } = -400;
        public int? GroupId { get; set; }
    }
}
