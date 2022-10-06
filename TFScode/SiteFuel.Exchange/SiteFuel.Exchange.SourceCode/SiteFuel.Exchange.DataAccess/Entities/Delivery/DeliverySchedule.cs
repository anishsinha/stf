namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DeliverySchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliverySchedule()
        {
            DeliveryScheduleXDrivers = new HashSet<DeliveryScheduleXDriver>();
            DeliveryScheduleXTrackableSchedules = new HashSet<DeliveryScheduleXTrackableSchedule>();
            EnrouteDeliveryHistories = new HashSet<EnrouteDeliveryHistory>();
            OrderVersionXDeliverySchedules = new HashSet<OrderVersionXDeliverySchedule>();
            FuelRequests = new HashSet<FuelRequest>();
            UoM = UoM.Gallons;
            FuelDispatchLocations = new HashSet<FuelDispatchLocation>();
        }

        public int Id { get; set; }

        public int Type { get; set; }

        public int WeekDayId { get; set; }

        public DateTimeOffset Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public decimal Quantity { get; set; }

        public int GroupId { get; set; }

        public int CreatedBy { get; set; }

        public int StatusId { get; set; }

        public bool IsRescheduled { get; set; }

        public Nullable<int> RescheduledTrackableId { get; set; }

        public UoM UoM { get; set; }


        public int? CarrierId { get; set; }

        public int? SupplierSourceId { get; set; }

        [StringLength(100)]
        public string SupplierContract { get; set; }

        [StringLength(100)]
        public string LoadCode { get; set; }

        public int? DeliveryGroupId { get; set; }

        public int? QuantityTypeId { get; set; }

        [ForeignKey("SupplierSourceId")]
        public virtual SupplierSource SupplierSource { get; set; }

        public virtual Carrier Carrier { get; set; }

        public virtual MstDeliveryScheduleType MstDeliveryScheduleType { get; set; }
		
        public virtual MstWeekDay MstWeekDay { get; set; }
		
        public virtual User User { get; set; }

        [ForeignKey("DeliveryGroupId")]
        public virtual DeliveryGroup RouteGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryScheduleXDriver> DeliveryScheduleXDrivers { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderVersionXDeliverySchedule> OrderVersionXDeliverySchedules { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrouteDeliveryHistory> EnrouteDeliveryHistories { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryScheduleXTrackableSchedule> DeliveryScheduleXTrackableSchedules { get; set; }
		
        public virtual DeliveryScheduleXTrackableSchedule DeliveryScheduleXTrackableSchedule { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelDispatchLocation> FuelDispatchLocations { get; set; }
    }
}
