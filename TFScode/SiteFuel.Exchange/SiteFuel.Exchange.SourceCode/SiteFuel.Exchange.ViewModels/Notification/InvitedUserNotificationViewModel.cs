using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvitedUserNotificationViewModel : BaseNotificationViewModel
    {
        public InvitedUserNotificationViewModel()
        {
            Roles = new List<string>();
        }

        public NotificationUserViewModel User { get; set; }

        public List<string> Roles { get; set; }

        public string InvitedByName { get; set; }

        public int InvitedCompanyId { get; set; }

        public string InvitedCompanyName { get; set; }

        public string PersonalMessage { get; set; }

        public string SupplierCode { get; set; }
    }
}
