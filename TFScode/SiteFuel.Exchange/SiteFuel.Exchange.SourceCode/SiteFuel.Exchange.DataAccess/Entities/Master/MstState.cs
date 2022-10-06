namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstState
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstState()
        {
            AssetAdditionalDetails = new HashSet<AssetAdditionalDetail>();
            CompanyAddresses = new HashSet<CompanyAddress>();
            Jobs = new HashSet<Job>();
            MstExternalTerminals = new HashSet<MstExternalTerminal>();
            TaxExemptLicenses = new HashSet<TaxExemptLicens>();
            CompanyAddresses1 = new HashSet<CompanyAddress>();
            ExternalSupplierAddresses = new HashSet<ExternalSupplierAddress>();
            ExternalSupplierAddresses1 = new HashSet<ExternalSupplierAddress>();
            MstCities = new HashSet<MstCity>();
            CompanyXServingLocations = new HashSet<CompanyXServingLocation>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int CountryId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int QuantityIndicatorTypeId { get; set; }
        public int? CountryGroupId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssetAdditionalDetail> AssetAdditionalDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAddress> CompanyAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        public virtual MstCountry MstCountry { get; set; }
        public virtual MstCountryAsGroup MstCountryAsGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstExternalTerminal> MstExternalTerminals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyAddress> CompanyAddresses1 { get; set; }
		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxExemptLicens> TaxExemptLicenses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalSupplierAddress> ExternalSupplierAddresses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalSupplierAddress> ExternalSupplierAddresses1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstCity> MstCities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyXServingLocation> CompanyXServingLocations { get; set; }
    }
}
