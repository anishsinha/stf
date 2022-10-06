using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationEventViewModel
    {
        public int Id { get; set; }

        public EventType EventType { get; set; }

        public int EntityId { get; set; }

        public int TriggeredByUserId { get; set; }

        public int TriggeredByCompanyId { get; set; }

        public bool IsNotificationSent { get; set; }

        public Nullable<DateTimeOffset> CreatedDate { get; set; }

        public string JsonMessage { get; set; }

        public int NotificationType { get; set; }

        public string BrandedCompanyName { get; set; }

        public int ApplicationTemplateId { get; set; }
        public bool IsManualTrigger { get; set; }
    }
}
