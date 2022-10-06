namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class AccessorialFee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccessorialFee()
        {
            FuelFees = new HashSet<FuelFee>();
            FreightTableCompanies = new HashSet<FreightTableCompany>();
            FreightTablePickupLocations = new HashSet<FreightTablePickupLocation>();
            FreightTableSourceRegions = new HashSet<FreightTableSourceRegion>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public TableTypes TableType { get; set; }
        public int Version { get; set; }
        public int? SupplierCompanyId { get; set; }
        public FreightTableStatus StatusId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        
        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        public virtual ICollection<FreightTableCompany> FreightTableCompanies { get; set; }
        public virtual ICollection<FreightTablePickupLocation> FreightTablePickupLocations { get; set; }
        public virtual ICollection<FreightTableSourceRegion> FreightTableSourceRegions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelFee> FuelFees { get; set; }
    }
}