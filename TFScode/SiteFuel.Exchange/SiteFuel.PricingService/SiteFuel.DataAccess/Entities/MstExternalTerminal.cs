namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstExternalTerminal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstExternalTerminal()
        {
            ExternalPricingData = new HashSet<ExternalPricingAxxis>();
            MstProductMappings = new HashSet<MstProductMapping>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(16)]
        public string ControlNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        [Required]
        [StringLength(16)]
        public string Abbreviation { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(128)]
        public string City { get; set; }

        [Required]
        [StringLength(32)]
        public string StateCode { get; set; }

        public int StateId { get; set; }

        [Required]
        [StringLength(32)]
        public string ZipCode { get; set; }
		
        [Required]
        [StringLength(32)]
        public string CountryCode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        [Required]
        [StringLength(64)]
        public string CountyName { get; set; }

        public int Currency { get; set; }

        public int PricingSourceId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [StringLength(256)]
        public string TerminalOwner { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalPricingAxxis> ExternalPricingData { get; set; }

        public virtual MstState MstState { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProductMapping> MstProductMappings { get; set; }

        [ForeignKey("PricingSourceId")]
        public MstPricingSource MstPricingSource { get; set; }
    }
}
