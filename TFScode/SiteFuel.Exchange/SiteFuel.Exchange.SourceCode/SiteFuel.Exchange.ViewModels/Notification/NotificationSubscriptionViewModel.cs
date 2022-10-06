using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationSubscriptionViewModel
    {
        public bool IsEmailSubscribed { get; set; } = false;

        public bool IsSmsSubscribed { get; set; } = false;

        public string ToPhoneNumber { get; set; }

        public string EventName { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }
    }
}
