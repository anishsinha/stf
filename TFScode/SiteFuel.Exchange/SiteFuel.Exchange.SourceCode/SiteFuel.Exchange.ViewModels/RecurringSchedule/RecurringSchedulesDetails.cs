using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class RecurringSchedulesDetails
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
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; }
        public RecurringShiftInfo ShiftInfo { get; set; } = null;
        public string ScheduleBuilderId { get; set; }
        public int OrderId { get; set; }
        public string DeliveryLevelPO { get; set; }
    }
    public class RecurringShiftInfo
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
    public class RecurringDeliveryRequestModel
    {
        public string TfxJobAddress { get; set; }
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
        public DropAddressViewModel TfxBulkPlant { get; set; }
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
        public RouteInfoDetails RouteInfo { get; set; } = null;
        public string RecurringScheduleId { get; set; }
    }
    public class RecurringShiftDetails
    {
        public string ShiftId { get; set; }
        public int DriverRowIndex { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverColIndex { get; set; }
    }
    public class RecurringScheduleBuilder
    {
        public string RegionId { get; set; }
        public string Date { get; set; }
        public string ScheduleBuilderId { get; set; }
        public string ScheduleBuilderViewId { get; set; }
        public int View { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public List<RecurringShiftDetails> ShiftInformation { get; set; }
        public bool IsBackgroundJobScheduleCreation { get; set; } = true;
        public bool IsDsbDriverSchedule { get; set; } = false;
    }
    public class BuyerRecurringSchedule
    {
        public int JobId { get; set; }
        public string PoNumber { get; set; }
        public string JobSiteId { get; set; }

        public int AssetId { get; set; }
    }
}
