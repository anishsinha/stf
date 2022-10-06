using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    [Table("ForcastingServiceSetting")]
    public partial class ForcastingServiceSetting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ForcastingServiceSetting()
        {
            ForcastingServiceXCarriers = new HashSet<ForcastingServiceXCarrier>();
        }

        public int Id { get; set; }
        public int BandPeriod { get; set; }
        public TimeSpan StartTiming { get; set; }
        public decimal MinimumLoadQty { get; set; }
        public decimal AverageLoadQty { get; set; }
        public int InventoryPriorityType { get; set; }
        public int InventoryUOM { get; set; }
        public int RetainCouldGo { get; set; }
        public int SafetyStockShouldGo { get; set; }
        public int RunoutLevelMustGo { get; set; }
        public bool IsAutoDRCreation { get; set; } = false;
        public int StartBuffer { get; set; }
        public int StartBufferUOM { get; set; }
        public int EndBuffer { get; set; }
        public int EndBufferUOM { get; set; }
        public int SupplierLead { get; set; }
        public int SupplierLeadUOM { get; set; }
        public int RetainTimeBuffer { get; set; }
        public int RetainTimeBufferUOM { get; set; }
        public int LeadTime { get; set; }
        public int LeadTimeUOM { get; set; }
        public bool IsOttoAutoDRCreation { get; set; }
        public bool IsAllCarrierEnabled { get; set; }
        public bool IsOttoScheduleCreation { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ForcastingServiceXCarrier> ForcastingServiceXCarriers { get; set; }
    }
}
