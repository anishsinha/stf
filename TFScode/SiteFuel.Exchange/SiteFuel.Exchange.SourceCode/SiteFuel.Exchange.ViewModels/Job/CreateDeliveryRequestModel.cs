using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class CreateDeliveryRequestModel
    {
        public int JobId { get; set; }
        public string JobName { get; set; }        
        public List<JobProductTypeDetails> ProductTypes { get; set; } = new List<JobProductTypeDetails>();
    }

    public class JobProductTypeDetails
    {
        public int ScheduleQuantityType { get; set; }
        public decimal RequiredQuantity { get; set; }
        public int FuelTypeId { get; set; }
        public string ProductType { get; set; }
        public int JobId { get; set; }
        public int UoM { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
        public bool isRecurringSchedule { get; set; }
        public List<RecurringSchedule> RecurringSchdules { get; set; } = new List<RecurringSchedule>();
        public int SupplierCompanyId { get; set; }
        public List<DropdownDisplayItem> SupplierCompanies = new List<DropdownDisplayItem>();
        public string Notes { get; set; }
        public bool IsRetainJob { get; set; } = false;
        public int AssetId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string JobDisplayId { get; set; }
        public string RetainTime { get; set; }
        public string RetainDate { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public bool IsRetainButtonClick { get; set; }
        public string OtherProductsNames { get; set; }
        public string DeliveryLevelPO { get; set; }

        public int OrderId { get; set; }

        public List<DropdownDisplayExtendedItem> Orders = new List<DropdownDisplayExtendedItem>();
    }
}
