using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class InActiveDriverMessageViewModel
    {
        public int OrderId { get; set; }

        public DateTimeOffset DeliveryDate { get; set; }
    }

    public class InActiveDriverViewModel : BaseNotificationViewModel
    {
        public int? DriverId { get; set; }

        public int OrderId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset DriverInvitedDate { get; set; }

        public string CompanyName { get; set; }

        public int CompanyId { get; set; }

        public string DriverFirstName { get; set; }

        public string DriverLastName { get; set; }

        public string PONumber { get; set; }
    }
}
