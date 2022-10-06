using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications
{
    public interface IEmailProcessor
    {
        EventType EventType { get; }
        void Initialize(NotificationEventViewModel notificationEventViewModel);
    }
}
