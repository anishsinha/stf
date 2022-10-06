
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiDeliveryRequestViewModel
    {
        public int UserId { get; set; }
        public int JobId { get; set; }
        public int AssetId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string SiteId { get; set; } //from freightservice db
        public string TankId { get; set; }
        public int ProductTypeId { get; set; }
        public string StorageId { get; set; }
        public decimal MaxFill { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
    }

    public class ApiRaiseDeliveryRequestInput
    {
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int JobId { get; set; }
        public int FuelTypeId { get; set; }
        public string JobName { get; set; }
        public string ProductType { get; set; }
        public int UoM { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public bool isRecurringSchedule { get; set; }
        public List<RecurringSchedule> RecurringSchdules { get; set; } = new List<RecurringSchedule>();
        public int BuyerCompanyId { get; set; }
        public int SupplierCompanyId { get; set; }
        public int UserId { get; set; }
        public string Notes { get; set; }
        public DeliveryWindowInfoModel DeliveryWindowInfo { get; set; }
        public int OrderId { get; set; }
        public string DeliveryLevelPO { get; set; }
    }

    public class RecurringSchedule
    {
        public string Id { get; set; }
        public int ScheduleType { get; set; }
        public string[] WeekDayId { get; set; }
        public string Date { get; set; }
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int? ProductTypeId { get; set; }
        public int MonthDayId { get; set; }
        public int Index { get; set; }
        public int MaxIndex { get; set; }
        public string Prefix { get; set; }
        public int UoM { get; set; }
        public string DeliveryLevelPO { get; set; }
    }

    public class ApiDeleteRecurringSchedule
    {
        public string Id { get; set; }
        public int UserId { get; set; }
    }
}

