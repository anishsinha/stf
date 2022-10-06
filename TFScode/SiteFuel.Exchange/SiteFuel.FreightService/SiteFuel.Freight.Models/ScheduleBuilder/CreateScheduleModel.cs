using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels.ScheduleBuilder
{

    public class CreateScheduleModel
    {

        public ProcessDSBCreation CreateScheduleInput { get; set; }

        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; }
    }

    public class ProcessDSBCreation
    {
        public int CarrierCompanyId { get; set; }
        public string CarrierCompanyName { get; set; }
        public string CarrierRegionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool IsCommonPickup { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public BulkPlantAddressModel BulkPlant { get; set; }
        public DropdownDisplayExtendedItem Drivers { get; set; } = new DropdownDisplayExtendedItem();
        public List<ScheduleDetails> ScheduleDetails { get; set; } = new List<ScheduleDetails>();
        public List<ScheduleApiResponse> ApiResponseModel { get; set; } = new List<ScheduleApiResponse>();
        public int SlotPeriod { get; set; }
        public List<ShiftModel> Shifts { get; set; } = new List<ShiftModel>();
    }

    public class ScheduleDetails
    {
        public int JobId { get; set; }
        public int JobCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string CustomerCompanyName { get; set; }
        public int OrderId { get; set; }
        public string PoNumber { get; set; }
        public DropdownDisplayItem Terminal { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public BulkPlantAddressModel BulkPlant { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string BadgeNumber { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string DispatcherNote { get; set; }
        public int FuelTypeId { get; set; }
        public string SiteId { get; set; }
        public string SupplierRegionId { get; set; }
    }

    public class ScheduleApiResponse
    {
        public string RequestCarrierOrderId { get; set; }
        public string ResponseCarrierOrderId { get; set; }
        public int TrackableScheduleId { get; set; }
    }
}
