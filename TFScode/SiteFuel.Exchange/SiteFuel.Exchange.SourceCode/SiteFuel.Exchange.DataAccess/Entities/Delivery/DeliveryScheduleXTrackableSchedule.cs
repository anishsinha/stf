namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DeliveryScheduleXTrackableSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryScheduleXTrackableSchedule()
        {
            DeliverySchedules = new HashSet<DeliverySchedule>();
            ExternalDropDetails = new HashSet<ExternalDropDetail>();
            Invoices = new HashSet<Invoice>();
            UoM = UoM.Gallons;
            ScheduleXBrokerOrderDetails = new HashSet<ScheduleXBrokerOrderDetail>();
            FuelDispatchLocations = new HashSet<FuelDispatchLocation>();
        }

        public int Id { get; set; }

        public DateTimeOffset Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal Quantity { get; set; }

        public int DeliveryScheduleId { get; set; }

        public bool IsActive { get; set; }

        public int? OrderId { get; set; }

        public int DeliveryScheduleStatusId { get; set; }

        public Nullable<int> DriverId { get; set; }

        public UoM UoM { get; set; }

        public int? CarrierId { get; set; }

        public int? SupplierSourceId { get; set; }

        [StringLength(512)]
        public string SupplierContract { get; set; }

        [StringLength(512)]
        public string LoadCode { get; set; }

        public int? DeliveryGroupId { get; set; }

        public int? QuantityTypeId { get; set; }

        public int? DeliveryStatus { get; set; }

        public DateTimeOffset? DeliveryStatusUpdatedDate { get; set; }

        [StringLength(24)]
        public string FrDeliveryRequestId { get; set; }

        [StringLength(24)]
        public string BlendGroupId { get; set; }

        public string AdditionalInfo { get; set; }

        public DateTimeOffset ShiftStartDate { get; set; }

        public DateTime? ShiftEndDateTime { get; set; }

        public int DeliveryScheduleType { get; set; }

        public int? PostLoadedForId { get; set; }

        [StringLength(512)]
        public string BadgeNo1 { get; set; }
        [StringLength(512)]
        public string BadgeNo2 { get; set; }
        [StringLength(512)]
        public string BadgeNo3 { get; set; }

        public string DisPatcherNote { get; set; }
        public bool IsCommonBadge { get; set; }

        [StringLength(256)]
        public string CarrierOrderId { get; set; }

        [StringLength(256)]
        public string ExternalRefId { get; set; }

        public string RouteAdditionalInfo { get; set; }
        public string RecurringAdditionalInfo { get; set; }

        public string CompartmentInfo { get; set; }
        public bool IsFilldInvoke { get; set; }
        public string Notes { get; set; }

        [ForeignKey("SupplierSourceId")]
        public virtual SupplierSource SupplierSource { get; set; }

        public virtual Carrier Carrier { get; set; }

        public virtual DeliverySchedule DeliverySchedule { get; set; }

        public virtual MstDeliveryScheduleStatus MstDeliveryScheduleStatus { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("DeliveryGroupId")]
        public virtual DeliveryGroup RouteGroup { get; set; }

        [StringLength(250)]
        public string GroupParentDRId { get; set; }

        public bool IsDispatcherDragDrop { get; set; } = false;
        public int DispatcherDragDropSequence { get; set; } = 0;

        [StringLength(250)]
        public string DeliveryLevelPO { get; set; } = string.Empty;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliverySchedule> DeliverySchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalDropDetail> ExternalDropDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduleXBrokerOrderDetail> ScheduleXBrokerOrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelDispatchLocation> FuelDispatchLocations { get; set; }
    }
}
