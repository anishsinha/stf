namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelRequestTankRentalFrequency
    {
        public FuelRequestTankRentalFrequency()
        {
            TankDetails = new HashSet<TankDetail>();
        }

        public int Id { get; set; }

        public int FuelRequestId { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        public int FrequencyTypeId { get; set; }

        public DateTimeOffset ScheduleStartDate { get; set; }

        public DateTimeOffset? LastRunDate { get; set; }

        public DateTimeOffset? DeactivationDate { get; set; }

        public int ActivationStatusId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("FuelRequestId")]
        public virtual FuelRequest FuelRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TankDetail> TankDetails { get; set; }
    }
}