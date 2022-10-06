using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels.WebNotification
{
    public class NotificationDispatchLocationViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string JobName { get; set; }
        public int? DeliveryScheduleId { get; set; }
        public int? TrackableScheduleId { get; set; }
        public string PreviousTerminalName { get; set; }
        public string CurrentTerminalName { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public string CreatedByCompanyName { get; set; }
        public DispatchNotificationType DispatchNotificationType { get; set; }
        public EnrouteDeliveryStatus Status { get; set; }
    }
}
