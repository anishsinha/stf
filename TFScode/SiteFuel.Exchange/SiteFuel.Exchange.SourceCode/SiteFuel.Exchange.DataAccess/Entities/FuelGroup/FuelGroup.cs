namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FuelGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FuelGroup()
        {
            FuelGroupProductTypes = new HashSet<FuelGroupProductType>();
            FuelGroupFuelTypes = new HashSet<FuelGroupFuelType>();
        }

        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public FuelGroupType FuelGroupType { get; set; }
        public TableTypes TableType { get; set; }
        public int? AssignedCompanyId { get; set; }
        public int CreatedByCompanyId { get; set; }
        public FreightTableStatus StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("AssignedCompanyId")]
        public virtual Company AssignedCompany { get; set; }

        [ForeignKey("CreatedByCompanyId")]
        public virtual Company CreatedByCompany { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelGroupProductType> FuelGroupProductTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelGroupFuelType> FuelGroupFuelTypes { get; set; }
    }
}