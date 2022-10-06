using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryGroupScheduleViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int? DeliveryGroupId { get; set; }
        public bool IsSelected { get; set; }
        public bool IsFutureSchedule { get; set; }
        public string Code { get; set; }
        public DeliveryGroupPickupViewModel PickupLocation { get; set; }
        
        public int? deliverystatus { get; set; }
    }
}
