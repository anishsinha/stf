namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaxExemptLicenses")]
    public partial class TaxExemptLicens
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaxExemptLicens()
        {
            TaxDetails = new HashSet<TaxDetail>();
            Jobs = new HashSet<Job>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        public DateTimeOffset BillOfLadingDate { get; set; }

        public DateTimeOffset EffectiveDate { get; set; }

        public DateTimeOffset? ObsoleteDate { get; set; }

        [Required]
        [StringLength(32)]
        public string EntityCustomId { get; set; }

        [StringLength(32)]
        public string IDType { get; set; }

        [Required]
        [StringLength(32)]
        public string IDCode { get; set; }

        public int BusinessType { get; set; }

        public int BusinessSubType { get; set; }

        [StringLength(128)]
        public string TradeName { get; set; }

        [Required]
        [StringLength(128)]
        public string LegalName { get; set; }

        [Required]
        [StringLength(128)]
        public string Address { get; set; }

        [Required]
        [StringLength(128)]
        public string City { get; set; }

        [Required]
        [StringLength(16)]
        public string Jurisdiction { get; set; }

        [StringLength(128)]
        public string County { get; set; }

        [Required]
        [StringLength(16)]
        public string PostalCode { get; set; }

        public int StateId { get; set; }

        public int CountryId { get; set; }

        [StringLength(128)]
        public string LicenseNumber { get; set; }

        public Nullable<decimal> LicensePercentage { get; set; }

        public bool IsDefault { get; set; }

        public int Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public bool IsSameCompanyAddress { get; set; }

        [StringLength(32)]
        public string AccountCustomId { get; set; }

        public virtual Company Company { get; set; }

        public virtual MstBusinessSubType MstBusinessSubType { get; set; }

        public virtual MstCountry MstCountry { get; set; }

        public virtual MstRole MstRole { get; set; }

        public virtual MstState MstState { get; set; }

        public virtual MstTaxExemptLicenseStatus MstTaxExemptLicenseStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaxDetail> TaxDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
