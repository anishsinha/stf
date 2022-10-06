using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankCapacityViewModel
    {
        public DeliveryReqPriority Priority { get; set; }
        public decimal MaxPercent { get; set; }
        public decimal MinPercent { get; set; }
    }
}
