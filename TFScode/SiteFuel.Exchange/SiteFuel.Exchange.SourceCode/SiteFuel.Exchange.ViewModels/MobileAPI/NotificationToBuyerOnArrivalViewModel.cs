using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationToBuyerOnArrivalViewModel
    {
        public NotificationToBuyerOnArrivalViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Message = new MessageViewModel();
            AppLocation = new AppLocationViewModel();
            UncanceledDeliveryScheduleId = new List<int>();
        }

        public MessageViewModel Message { get; set; }

        public AppLocationViewModel AppLocation { get; set; }

        public int OrderId { get; set; }

        public int DeliveryScheduleId { get; set; }

        public List<int> UncanceledDeliveryScheduleId { get; set; }
    }
}
