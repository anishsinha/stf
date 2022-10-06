using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryDetailsRespModel : StatusViewModel
    {
        public List<DeliveryDetailsModel> DeliveryDetails { get; set; }
    }
    public class DeliveryDetailsModel
    {
        public int? TrackableScheduleId { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleTime { get; set; }
        public string Driver { get; set; }
        public string Carrier { get; set; }
        public decimal Quantity { get; set; }
        public int QuantityTypeId { get; set; }
        public string QuantityTypeName { get; set; }
    }
}
