using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDSOutsideDeliveryWindowViewModel : BaseNotificationViewModel
    {
        public NotificationDSOutsideDeliveryWindowViewModel()
        {
        }

        public string Location { get; set; }

        public int OrderId { get; set; }

        public int SupplierCompanyId { get; set; }

        public string Carrier { get; set; }

        public int CarrierCompanyId { get; set; }

        public string Product { get; set; }

        public string ScheduledQuantity { get; set; }

        public string EstimatedDeliveryWindow { get; set; }

        public string ScheduledTiming { get; set; }
    }
}
