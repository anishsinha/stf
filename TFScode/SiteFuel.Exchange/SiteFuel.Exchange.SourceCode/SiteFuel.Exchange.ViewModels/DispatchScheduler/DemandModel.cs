using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DemandModel
    {
        public long Id { get; set; }
        public int AssetId { get; set; }
        public long JobId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string TankName { get; set; }
        public string StorageId { get; set; }
        public decimal Level { get; set; }
        public float Ullage { get; set; }
        public float GrossVolume { get; set; }
        public float NetVolume { get; set; }
        public float CalculatedUllage { get; set; }
        public float CalculatedGrossVolume { get; set; }
        public float CalculatedNetVolume { get; set; }
        public float WaterNetLevel { get; set; }
        public float WaterGrossLevel { get; set; }
        public string DisplayCaptureTime { get; set; }
        public string ProductName { get; set; }
        public int DataSourceTypeId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string JobName { get; set; }
        public bool IsDRExists { get; set; }
        public bool IsDRMissed { get; set; }
        public List<PartialDRDetails> ExistingDR { get; set; } = new List<PartialDRDetails>();
        public string UoM { get; set; }
        public DateTime CaptureTime { get; set; }
        public decimal TankCapacity { get; set; }
        public decimal TankMinFill { get; set; }
        public decimal TankMaxFill { get; set; }
        public decimal CurrentThreshold { get; set; }
        public int? FillType { get; set; }
        public decimal ReorderPercent { get; set; }
        public decimal ReorderQuantity { get; set; }
        public int ScaleMeasurement { get; set; }
        public int? OrderId { get; set; }
        public float DipTestValue { get; set; }
        public TankScaleMeasurement DipTestUoM { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public int FuelTypeId { get; set; }
        public string ProductType { get; set; }
        public List<RecurringDRSchedule> RecurringDRScheduleDetails { get; set; } = new List<RecurringDRSchedule>();
        [DisplayName("Recurring Schedule")]
        public bool isRecurringSchedule { get; set; } = false;
        public string PoNumber { get; set; }
        public string IndexRecurring { get; set; }
        public int ProductTypeId { get; set; }
        public int CarrierStatus { get; set; }
        public int ProductSequence { get; set; }
        public DeliveryRequestFor DeliveryRequestFor { get; set; } = DeliveryRequestFor.UnKnown;
        public bool IsEndSupplier { get; set; }

        public bool IsDispatchRetained { get; set; }
        public int? SupplierCompanyId { get; set; }
        public List<DropdownDisplayItem> SupplierCompanies { get; set; } = new List<DropdownDisplayItem>();
        public List<OrderPickupDetailModel> OrderPickupDetails { get; set; } = new List<OrderPickupDetailModel>();
        public bool IsAcceptNightDeliveries { get; set; }
        public List<DropdownDisplayItem> TrailerTypes { get; set; } = new List<DropdownDisplayItem>();
    }

    public class CreateDRTankModel
    {
        public List<ProductModelToCreateDR> Tanks { get; set; } = new List<ProductModelToCreateDR>();
        public RegionFavProductModel FavoriteProducts { get; set; }
    }

    public class ProductModelToCreateDR
    {
        public long Id { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string TankName { get; set; }
        public bool IsTankAndAssetAvailableForJob { get; set; }
        public string StorageId { get; set; }

        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string JobName { get; set; }
        public float Ullage { get; set; }
        public float NetVolume { get; set; }
        public string DisplayCaptureTime { get; set; }
        public string ProductName { get; set; }
        public bool IsDRExists { get; set; }
        public List<PartialDRDetails> ExistingDR { get; set; } = new List<PartialDRDetails>();
        public decimal TankCapacity { get; set; }
        public decimal TankMaxFill { get; set; }
        public decimal CurrentThreshold { get; set; }
        public decimal ReorderPercent { get; set; }
        public decimal ReorderQuantity { get; set; }
        public long JobId { get; set; }
        public int LocationManagedType { get; set; }
        public string UoM { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public List<RecurringDRSchedule> RecurringDRScheduleDetails { get; set; } = new List<RecurringDRSchedule>();
        public int FuelTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public List<TankDipTestModel> Tanks { get; set; }

        public int? SupplierCompanyId { get; set; }
        public List<DropdownDisplayItem> SupplierCompanies { get; set; } = new List<DropdownDisplayItem>();
        public List<OrderPickupDetailModel> OrderPickupDetails { get; set; } = new List<OrderPickupDetailModel>();
        public List<OrderPickupDetailModel> BlendOrderPickupDetails { get; set; } = new List<OrderPickupDetailModel>();
        public bool IsDispatchRetainedByCustomerDisplay { get; set; } = true;
    }

    public class TankDipTestModel
    {
        public string TankId { get; set; }
        public string SiteId { get; set; }
        public string TankName { get; set; }
        public string StorageId { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public float Ullage { get; set; }
        public float NetVolume { get; set; }
        public string DisplayCaptureTime { get; set; }
        public decimal TankCapacity { get; set; }
        public decimal TankMaxFill { get; set; }
        public decimal CurrentThreshold { get; set; }
        public decimal ReorderPercent { get; set; }
        public decimal ReorderQuantity { get; set; }
        public int AssetId { get; set; }
        public float WaterLevel { get; set; }
    }


    public class PartialDRDetails
    {
        public string Id { get; set; }
        public DeliveryReqPriority Priority { get; set; }
        public string ScheduleStatusName { get; set; }
        public int ScheduleStatusId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string CreatedOn { get; set; }
        public bool IsMissedDr { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public bool IsRecurringSchedule { get; set; }
        public int ScheduleQuantityType { get; set; }
        public string ScheduleQuantityTypeName { get; set; }
    }
    public class RecurringDRSchedule
    {
        public string Id { get; set; }
        public int ScheduleType { get; set; }
        public List<string> WeekDayId { get; set; }
        public int MonthDayId { get; set; }
        public string Date { get; set; }
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int? OrderId { get; set; }
        public string SiteId { get; set; }
        public int JobId { get; set; }
        public int? TfxSupplierCompanyId { get; set; }
        public string TfxCompanyName { get; set; }
        public int TfxUserId { get; set; }
        public int AssignedToCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string PoNumber { get; set; }
        public string TankName { get; set; }
        public int Index { get; set; }
        public int MaxIndex { get; set; }
        public int AssetId { get; set; }
        public int? ProductTypeId { get; set; }
        public bool IsBlendedRequest { get; set; } = false;
        public string BlendedGroupId { get; set; } = string.Empty;
        public string DeliveryLevelPO { get; set; }
    }
}
