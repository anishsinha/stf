namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CompanyAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanyAddress()
        {
            SupplierAddressXWorkingHours = new HashSet<SupplierAddressXWorkingHour>();
            Users = new HashSet<User>();
            MstProductTypes = new HashSet<MstProductType>();
            MstSupplierQualifications = new HashSet<MstSupplierQualification>();
            MstStates = new HashSet<MstState>();
            CompanyXServingLocations = new HashSet<CompanyXServingLocation>();
        }

        public int Id { get; set; }

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

        public int CompanyId { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual Company Company { get; set; }

        public virtual MstCountry MstCountry { get; set; }

        public virtual MstPhoneType MstPhoneType { get; set; }

        public virtual MstState MstState { get; set; }

        public virtual SupplierAddressXSetting SupplierAddressXSetting { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierAddressXWorkingHour> SupplierAddressXWorkingHours { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProductType> MstProductTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstSupplierQualification> MstSupplierQualifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstState> MstStates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXServingLocation> CompanyXServingLocations { get; set; }
    }
}
