using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Notification
{
    public class InvitedUserNotifyViewModel : BaseNotificationViewModel
    {
        public NotificationUserViewModel User { get; set; }

        public string InvitedByName { get; set; }

        public int InvitedCompanyId { get; set; }

        public string InvitedCompanyName { get; set; }

        public string PersonalMessage { get; set; }

        public string SupplierCode { get; set; }
    }
}
