using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CarrierXDeliveryRequestInfo
    {
        public List<DeliveryRequestViewModel> DeliveryRequestDetails { get; set; } = new List<DeliveryRequestViewModel>();
        public List<DeliveryRequestViewModel> AssignedByMeDeliveryRequestDetails { get; set; } = new List<DeliveryRequestViewModel>();

    }
    public class JobTankAdditionalDetailModel
    {
        public int AssetId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string TankName { get; set; }
        public string TankNumber { get; set; }
        public decimal MaxFill { get; set; }
        public int FillType { get; set; }
        public decimal MinFill { get; set; }
        public decimal RunOut { get; set; }
        public int ProductTypeId { get; set; }
        public string TfxProductTypeName { get; set; }
        public string JobName { get; set; }
        public int JobId { get; set; }
        public bool ISRunOut { get; set; } = false;
        public decimal FuelCapacity { get; set; }
        public string DaysRemaining { get; set; }
    }
}
