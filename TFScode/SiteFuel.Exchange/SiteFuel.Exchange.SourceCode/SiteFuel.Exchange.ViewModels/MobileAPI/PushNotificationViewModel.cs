using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{

    public class PushNotificationViewModel
    {
        public int Id { get; set; }
        public List<string> registration_ids { get; set; }
        public PushNotificationDetails notification { get; set; }
        public NotificationDataModel data { get; set; }
        public bool IsRead { get; set; }
    }

    public class PushNotificationDetails
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Sound { get; set; }
        public bool ContentAvailable { get; set; }
        public bool ShowInForeground { get; set; }
        public string Priority { get; set; }

    }

    public class NotificationDataModel
    {
        public int OrderId { get; set; }
        public int DeliveryScheduleId { get; set; }
        public int Code { get; set; }
        public string ChannelId { get; set; }
    }
    public class ClearNotificationInputModel
    {
        public int UserId { get; set; }
        public List<int> NotificationIds { get; set; }
    }
}


