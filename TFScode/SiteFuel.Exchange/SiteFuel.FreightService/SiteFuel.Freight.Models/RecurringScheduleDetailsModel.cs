using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class RecurringScheduleDetailsModel
    {
        public string Id { get; set; }
        public int ScheduleType { get; set; }
        public string RegionId { get; set; }
        public List<string> WeekDayId { get; set; }
        public int MonthDayId { get; set; }
        public string Date { get; set; }
        public int JobId { get; set; }
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }
        public List<RecurringDeliveryRequestDetailsViewModel> DeliveryRequests { get; set; } = new List<RecurringDeliveryRequestDetailsViewModel>();
        public RecurringShiftInfoViewModel ShiftInfo { get; set; } = null;
    }
    public class CreateRecurringDRViewModel
    {
        public string Id { get; set; }
        public int ScheduleType { get; set; }
        public string RegionId { get; set; }
        public List<string> WeekDayId { get; set; }
        public int MonthDayId { get; set; }
        public string Date { get; set; }
        public int JobId { get; set; }
        public int TfxUserId { get; set; }
        public int TfxCompanyId { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<DeliveryRequestViewModel>();
        public RecurringShiftInfoViewModel ShiftInfo { get; set; } = null;
        public string ScheduleBuilderId { get; set; }
        public int OrderId { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
    public class RecurringDeliveryRequestDetailsViewModel
    {
        public string TfxJobAddress { get; set; }
        public string TfxJobCity { get; set; }
        public string TfxJobName { get; set; }
        public string TfxProductType { get; set; }
        public int TfxUoM { get; set; }
        public int TfxCreatedByCompanyId { get; set; }
        public string TfxDisplayJobId { get; set; } //from freightservice db
        public string StorageTypeId { get; set; } // old TankID
        public string StorageId { get; set; }
        public int TfxProductTypeId { get; set; }
        public decimal DRCreationLevel { get; set; }
        public decimal RequiredQuantity { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public decimal? CurrentThreshold { get; set; }
        public DeliveryReqStatus Status { get; set; } = DeliveryReqStatus.Pending;
        public int TfxScheduleStatus { get; set; }
        public int TfxScheduleEnrouteStatus { get; set; }
        public int TfxAssignedToUserId { get; set; }
        public int TfxAssignedToCompanyId { get; set; }
        public string TfxAssignedToRegionId { get; set; }
        public bool IsReCreated { get; set; }
        public int? TfxSupplierCompanyId { get; set; }
        public string CreatedRegionId { get; set; }
        public string ParentId { get; set; }
        public int TfxJobId { get; set; }
        public string TfxCustomerCompany { get; set; }
        public int? TfxDeliveryGroupId { get; set; }
        public int? TfxDeliveryScheduleId { get; set; }
        public int? TfxTrackableScheduleId { get; set; }
        public int? TfxOrderId { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; }
        public BulkPlantAddressModel TfxBulkPlant { get; set; }
        public AutoDrStatus AutoDRStatus { get; set; }
        public DateTimeOffset? AutoCreatedOn { get; set; }
        public DateTimeOffset? AutoUpdatedOn { get; set; }
        public string TfxScheduleStatusName { get; set; }
        public string ScheduleBuilderId { get; set; }
        public string TripId { get; set; }
        public decimal TankMaxFill { get; set; }
        public int DeliveryRequestType { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public bool IsCommonBadge { get; set; }
        public string DispactherNote { get; set; }
        public string CarrierOrderId { get; set; }
        public RouteInfo RouteInfo { get; set; } = null;
    }
    public class RecurringShiftInfoViewModel
    {
        public int CompanyId { get; set; }
        public string RegionId { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class JobRecurringDRInput
    {
        public List<int?> ProductTypeIds { get; set; } = new List<int?>();
        public List<int> JobIds { get; set; } = new List<int>();
    }
}
