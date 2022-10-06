namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstCountry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstCountry()
        {
            CompanyAddresses = new HashSet<CompanyAddress>();
            Jobs = new HashSet<Job>();
            MstStates = new HashSet<MstState>();
            TaxExemptLicenses = new HashSet<TaxExemptLicens>();
            ExternalSupplierAddresses = new HashSet<ExternalSupplierAddress>();
            MstCountryAsGroups = new HashSet<MstCountryAsGroup>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public Currency Currency { get; set; }

        [Required]
        [StringLength(8)]
        public string CurrencySymbol { get; set; }

        public UoM DefaultUoM { get; set; }

        [StringLength(100)]
        public string UoD { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [StringLength(8)]
        public string IsoCode { get; set; }
        [StringLength(8)]
        public string PhoneCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstState> MstStates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstCountryAsGroup> MstCountryAsGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExemptLicens> TaxExemptLicenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalSupplierAddress> ExternalSupplierAddresses { get; set; }
    }
}
