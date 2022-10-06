namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SourceRegion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SourceRegion()
        {
            SourceRegionCarrier = new HashSet<SourceRegionCarrier>();
            SourceRegionPickupLocation = new HashSet<SourceRegionPickupLocation>();
        }

        public int Id { get; set; }

        [StringLength(32)]
        public string Name { get; set; }

        public int CompanyId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SourceRegionCarrier> SourceRegionCarrier { get; set; }

        public virtual SourceRegionAddress SourceRegionAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SourceRegionPickupLocation> SourceRegionPickupLocation { get; set; }

    }
}
