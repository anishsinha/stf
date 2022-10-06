using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess
{
    public class HeldDeliveryRequest
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string HeldDrId { get; set; }
        public string UniqueOrderNo { get; set; }
        public string Sap_OrderNo { get; set; }
        public string Sap_Order_Status { get; set; }
        public string SiteId { get; set; } //from freightservice db
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int JobId { get; set; }
        public decimal QuantityInPercent { get; set; }
        public bool IsAdditive { get; set; }
        public int ProductTypeId { get; set; }
        public int? FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string JobName { get; set; }
        public string JobAddress { get; set; }
        public string JobCity { get; set; }
        public string CustomerCompany { get; set; }
        public string ProductType { get; set; }
        public bool IsDREdited { get; set; }
        public int UoM { get; set; }
        public string CreatedByRegionId { get; set; }
        public string AssignedToRegionId { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public decimal CurrentThreshold { get; set; }
        public decimal TankMaxFill { get; set; }
        public int? OrderId { get; set; }
        public DRSource DelReqSource { get; set; } = DRSource.Manual;
        public bool isRecurringSchedule { get; set; }
        public string PoNumber { get; set; }
        //public List<RecurringSchdule> RecurringSchdule { get; set; }
        public int BuyerCompanyId { get; set; }
        public string DispactherNote { get; set; }
        public string BrokeredDrId { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; } = false;
        public DeliveryRequestFor DeliveryRequestFor { get; set; }
        public string Notes { get; set; }
        public bool isRetainInfo { get; set; }
        public bool isTankExists { get; set; }
        public bool IsRetainButtonClick { get; set; }
        public string RetainTime { get; set; }
        public string RetainDate { get; set; }
        public string WindowStartTime { get; set; }
        public string WindowStartDate { get; set; }
        public string WindowEndTime { get; set; }
        public string WindowEndDate { get; set; }
        public int? SupplierCompanyId { get; set; }
        public int AssignedToCompanyId { get; set; }
        public bool RequestFromBuyerWallyBoard { get; set; }
        public string BadgeNo1 { get; set; }
        public string BadgeNo2 { get; set; }
        public string BadgeNo3 { get; set; }
        public string DispatcherNote { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public MdbDataAccess.Collections.DropdownDisplayItem Terminal { get; set; }
        public BulkPlantAddressModel Bulkplant { get; set; }
        public bool IsAcceptNightDeliveries { get; set; }
        public string HoursToCoverDistance { get; set; }
        public TimeSpan JobTimeZoneOffset { get; set; }
        public bool IsMaxFillAllowed { get; set; }
        public bool IsReAssignToCarrier { get; set; }
        public bool IsTBD { get; set; }
        public string TBDGroupId { get; set; }
        public int UserId { get; set; }
        public string DeliveryDateStartTime { get; set; } = string.Empty;
        public string Vessel { get; set; } = string.Empty;
        public string Berth { get; set; } = string.Empty;
        public bool IsMarine { get; set; } = false;
        public string DeliveryLevelPO { get; set; } = string.Empty;
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
        public string FileName { get; set; }
        public string CreditApprovalFilePath { get; set; }
        //BLENDED REQUEST
        public bool IsBlendedRequest { get; set; }
        //public List<BlendedRequest> BlendedRequests { get; set; }
        public bool IsCommonPickupForBlend { get; set; }
        public string BlendedGroupId { get; set; }
        public int BlendParentProductTypeId { get; set; }
        public string SelectedDate { get; set; }
        public bool IsFutureDR { get; set; }
        public bool IsCalendarView { get; set; }
        public int NumOfSubDrs { get; set; }
        public decimal? IndicativePrice { get; set; }
        public CompanyType CompanyTypeId { get; set; }
        public int CreatedBy { get; set; }
        public int CreatedByCompanyId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public HeldDrStatus Status { get; set; }
        public int UpdatedBy { get; set; }
        public int UpdatedByCompanyId { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public bool IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
