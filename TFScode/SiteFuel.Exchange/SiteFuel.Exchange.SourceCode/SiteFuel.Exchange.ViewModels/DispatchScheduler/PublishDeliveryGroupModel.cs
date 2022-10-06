using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderDetailModel
    {
        public int Id { get; set; }
        public Currency Currency { get; set; }
        public string TimeZoneName { get; set; }
    }

    public class CreateDeliveryGroupModel
    {
        public int LatestSchGroupNumber { get; set; }
        public List<TripViewModel> PublishedGroups { get; set; } = new List<TripViewModel>();
        public List<OrderDetailModel> Orders { get; set; } = new List<OrderDetailModel>();
        public List<int> OrderIds { get; set; } = new List<int>();
        public List<int> TrackableScheduleIds { get; set; }
        public List<DropAddressViewModel> Terminals { get; set; }
        public DateTimeOffset ScheduleDate { get; set; }
        public List<ScheduleNotificationModel> GroupChanges { get; set; } = new List<ScheduleNotificationModel>();
        public DSBSaveModel ScheduleBuilder { get; set; }
    }
}
