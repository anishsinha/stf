using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDSOutsideDeliveryWindowModel
    {
        public NotificationDSOutsideDeliveryWindowModel()
        {
        }

        public string Location { get; set; }

        public int OrderId { get; set; }
        
        public int CarrierCompanyId { get; set; }

        public string Product { get; set; }

        public string ScheduledQuantity { get; set; }

        public string EstimatedDeliveryWindow { get; set; }

        public string ScheduledTiming { get; set; }
    }
}
