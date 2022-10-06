namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExternalSupplierAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExternalSupplierAddress()
        {
            MstProductTypes = new HashSet<MstProductType>();
            MstSupplierQualifications = new HashSet<MstSupplierQualification>();
            MstStates = new HashSet<MstState>();
        }

        public int Id { get; set; }

        public int ExternalSupplierId { get; set; }

        [Required]
        public string Address { get; set; }

        [StringLength(512)]
        public string AddressLine2 { get; set; }

        [StringLength(512)]
        public string AddressLine3 { get; set; }

        [Required]
        [StringLength(128)]
        public string City { get; set; }

        public int StateId { get; set; }

        [Required]
        [StringLength(32)]
        public string ZipCode { get; set; }

        public int CountryId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int PhoneTypeId { get; set; }

        [Required]
        [StringLength(16)]
        public string PhoneNumber { get; set; }

        public bool IsStateWideService { get; set; }

        public int Radius { get; set; }

        public bool IsLocationOwned { get; set; }

        public bool IsHedgeOrderAllowed { get; set; }

        public bool IsOverWaterRefuelingAllowed { get; set; }

        public int? NumberOfTrucks { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("ExternalSupplierId")]
        public virtual ExternalSupplier ExternalSupplier { get; set; }       

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProductType> MstProductTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstSupplierQualification> MstSupplierQualifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstState> MstStates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalSupplierAddressTruckType> ExternalSupplierAddressTruckTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual MstState MstState { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual MstCountry MstCountry { get; set; }
    }
}
