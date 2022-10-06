using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Tank;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobAdditionalDetailsViewModel
    {
        public string Id { get; set; }
        public string SiteImageFilePath { get; set; }
        public string AdditionalImageFilePath { get; set; }
        public string AdditionalImageDescription { get; set; }
        public List<int> DeliveryDays { get; set; }
        public List<DeliveryDaysViewModel> DeliveryDaysList { get; set; } = new List<DeliveryDaysViewModel>();
        public DateTimeOffset FromDeliveryTime { get; set; }
        public DateTimeOffset ToDeliveryTime { get; set; }
        public int JobId { get; set; }
        public bool IsActive { get; set; }
        public string SiteId { get; set; }
        public string JobName { get; set; }
        public string TfxDisplayJobId { get; set; }
        public bool IsAutoCreateDREnable { get; set; }

        public List<TankDetailsModel> TankDetails { get; set; } = new List<TankDetailsModel>();
        public string RegionId { get; set; }
        public List<TrailerTypeStatus> TrailerType { get; set; }

        public string DistanceCovered { get; set; }
    }
}
