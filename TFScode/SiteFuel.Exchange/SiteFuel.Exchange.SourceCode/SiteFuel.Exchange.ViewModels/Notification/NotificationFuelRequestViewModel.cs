using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationFuelRequestViewModel : BaseNotificationViewModel
	{
        public int Id { get; set; }
        public string FuelRequestNumber { get; set; }
        public int TypeId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public NotificationUserViewModel Creator { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string ExpirationDate { get; set; }
        public string DeliveryStartDate { get; set; }
        public string DeliveryStartTime { get; set; }
        public bool IsMarineLocation { get; set; }
        public string ExternalPoNumber { get; set; }
    }
}
