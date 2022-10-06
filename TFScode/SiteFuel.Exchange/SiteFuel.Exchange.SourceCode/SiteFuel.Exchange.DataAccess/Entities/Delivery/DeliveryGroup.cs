namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DeliveryGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryGroup()
        {
            DeliverySchedules = new HashSet<DeliverySchedule>();
            DeliveryScheduleXTrackableSchedules = new HashSet<DeliveryScheduleXTrackableSchedule>();
            FuelDispatchLocations = new HashSet<FuelDispatchLocation>();
        }
        public int Id { get; set; }

        public int DriverId { get; set; }

        public string RouteNote { get; set; }

        public string LoadCode { get; set; }

        public int CompanyId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("DriverId")]
        public virtual User Driver { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliverySchedule> DeliverySchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryScheduleXTrackableSchedule> DeliveryScheduleXTrackableSchedules { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelDispatchLocation> FuelDispatchLocations { get; set; }
    }
}
