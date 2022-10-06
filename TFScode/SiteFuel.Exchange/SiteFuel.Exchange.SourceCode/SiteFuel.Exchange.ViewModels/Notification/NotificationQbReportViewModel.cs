using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Notification
{
    public class NotificationQbReportViewModel : BaseNotificationViewModel
    {
        public List<string> SubscribedUsers { get; set; }

        public QbSyncReportViewModel QbReport { get; set; }
    }
}
