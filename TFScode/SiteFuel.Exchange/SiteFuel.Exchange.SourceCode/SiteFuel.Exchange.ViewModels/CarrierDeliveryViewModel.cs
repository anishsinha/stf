using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CarrierDeliveryViewModel
    {
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string ReportGenerateDate { get; set; }
        public string Culture { get; set; }
        public List<CarrierDelXUserViewModel> CarrierDelXUserViewModel { get; set; } = new List<CarrierDelXUserViewModel>();
        public List<string> EmailAddress { get; set; } = new List<string>();
        public List<int> CarrierCompanyIds { get; set; } = new List<int>();
        public UoM UoM { get; set; }
    }
    public class CarrierDelXUserViewModel
    {
        public string Name { get; set; }
        public decimal TotalDSCount { get; set; }
        public decimal TotalQty { get; set; }
        public UoM UoM { get; set; }
        public string URL { get; set; }
        public List<DeliveryUploadFailure> DeliveryUploadFailure { get; set; } = new List<DeliveryUploadFailure>();
        public CarrierDeliveryRequestInfo CarrierDeliveryRequestInfo = new CarrierDeliveryRequestInfo();
        public List<CarrierTankRunOutInfo> CarrierTankRunOutInfo { get; set; } = new List<CarrierTankRunOutInfo>();
        public CarrierOverUnderDeliveryRequestInfo CarrierOverUnderDeliveryRequestInfo { get; set; } = new CarrierOverUnderDeliveryRequestInfo();
    }
    public class DeliveryUploadFailure
    {
        public string Carriername { get; set; }
        public string APIName { get; set; }
        public string ExternalRefID { get; set; }
        public string DateTime { get; set; }
        public string Status { get; set; }
        public string LocationID { get; set; }
        public string DropDate { get; set; }
        public string LiftDate { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string BOL { get; set; }
        public string ProductType { get; set; }
        public string ProductQty { get; set; }
        public string PickupLocationName { get; set; }
        public string DeliveryLocationName { get; set; }
        public string ReasonForFailure { get; set; }
    }

    public class CarrierDeliveryRequestInfo
    {
        public int MustGo { get; set; }
        public int ShouldGo { get; set; }
        public int CouldGo { get; set; }
        public int AssignedToMe { get; set; }
        public List<CarrierDeliveryRequestDetails> CarrierDeliveryRequestDetails { get; set; } = new List<CarrierDeliveryRequestDetails>();
    }
    public class CarrierDeliveryRequestDetails
    {
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string ProductType { get; set; }
        public string Quantity { get; set; }
        public int Priority { get; set; }
        public bool AssignedToMe { get; set; } = false;
    }
    public class CarrierTankRunOutInfo
    {
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string ProductType { get; set; }
        public string TankName { get; set; }
        public string StorageId { get; set; }
        public string StorageTypeId { get; set; }
    }
    public class CarrierOverUnderDeliveryRequestInfo
    {
        public int TotalCount { get; set; }
        public int UnderDeliveries { get; set; }
        public int OverDeliveries { get; set; }
        public int MissedDeliveries { get; set; }
        public List<CarrierOverUnderDeliveryRequestDetails> CarrierOverUnderDeliveryRequestDetails { get; set; } = new List<CarrierOverUnderDeliveryRequestDetails>();
    }
    public class CarrierOverUnderDeliveryRequestDetails
    {
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string ProductType { get; set; }
        public string RequiredQuantity { get; set; }
        public string ActualDeliveredQuantity { get; set; }
        public string DaysRemaining { get; set; }
        public int DeliveryType { get; set; }
    }
}
