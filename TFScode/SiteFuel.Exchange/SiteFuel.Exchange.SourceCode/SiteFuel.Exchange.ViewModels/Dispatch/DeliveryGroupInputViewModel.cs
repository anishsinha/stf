using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryGroupInputViewModel
    { 
        public int? DriverId { get; set; }

        public string LoadCode { get; set; }

        public string RouteNote { get; set; }

        public List<GroupTrackableScheduleDetails> GroupTrackableSchedules { get; set; }

        public DeliveryGroupPickupViewModel PickupLocation { get; set; }

        public bool IsCommonForGroup { get; set; }
    }

    public class GroupTrackableScheduleDetails
    {
        public int Id { get; set; }
        public DeliveryGroupPickupViewModel PickupLocation { get; set; }
    }
}
