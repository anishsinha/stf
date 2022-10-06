using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class CarrierXDeliveryRequestInfo
    {
        public List<DeliveryRequestViewModel> DeliveryRequestDetails { get; set; } = new List<DeliveryRequestViewModel>();
        public List<DeliveryRequestViewModel> AssignedByMeDeliveryRequestDetails { get; set; } = new List<DeliveryRequestViewModel>();
    }
}
