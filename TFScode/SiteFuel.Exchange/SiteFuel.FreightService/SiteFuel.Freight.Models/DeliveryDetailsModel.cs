using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class DeliveryDetailsRespModel : StatusModel
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
    }
    public class PreBOLRetainDeliveryDetailsModel
    {
        public string DeliveryReqId { get; set; }
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public string ProductType { get; set; }
        public int FuelTypeId { get; set; }
        public decimal RetainQuantity { get; set; } = 0;
        public bool IsTrailerRetain { get; set; } = true;
    }
    public class PreBOLRetainModel
    {
        public string DeliveryReqId { get; set; }
        public int FuelTypeId { get; set; }
        public List<CompartmentsInfoViewModel> CompartmentInfo { get; set; } = new List<CompartmentsInfoViewModel>();
    }
}
