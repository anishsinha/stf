using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AppLocationViewModel : StatusViewModel
    {
        public AppLocationViewModel()
        {
        }

        public AppLocationViewModel(Status status)
            : base(status)
        {
        }

        public int UserId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string FCMAppId { get; set; }

        public AppType AppType { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int? OrderId { get; set; }

        public int? DeliveryScheduleId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public string ExternalRefID { get; set; }
    }
}
