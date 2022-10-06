using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryGroupGridViewModel
    { 
        public int DeliveryGroupId { get; set; }

        public int? DriverId { get; set; }

        public string DriverName { get; set; }

        public string LoadCode { get; set; }

        public string RouteNote { get; set; }

        public List<DeliveryGroupScheduleViewModel> TrackableSchedules { get; set; } = new List<DeliveryGroupScheduleViewModel>();

        public DeliveryGroupPickupViewModel PickupLocation { get; set; }

        public bool IsCommonForGroup { get; set; }

        public int TotalCount { get; set; }
    }
}
