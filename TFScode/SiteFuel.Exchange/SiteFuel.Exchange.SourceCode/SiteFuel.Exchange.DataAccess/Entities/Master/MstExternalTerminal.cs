namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstExternalTerminal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstExternalTerminal()
        {
            FuelRequests = new HashSet<FuelRequest>();
            InvoiceBolDetails = new HashSet<InvoiceFtlDetail>();
            PreLoadBolDetails = new HashSet<PreLoadBolDetail>();
            Jobs = new HashSet<Job>();
            MstProductMappings = new HashSet<MstProductMapping>();
            Orders = new HashSet<Order>();
            RequestPrices = new HashSet<RequestPrice>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [Required]
        [StringLength(64)]
        public string CountyName { get; set; }

        public Currency Currency { get; set; }

        public int PricingSourceId { get; set; }

        [StringLength(256)]
        public string TerminalOwner { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceFtlDetail> InvoiceBolDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreLoadBolDetail> PreLoadBolDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        public virtual MstState MstState { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProductMapping> MstProductMappings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestPrice> RequestPrices { get; set; }

        [ForeignKey("PricingSourceId")]
        public MstPricingSource MstPricingSource { get; set; }
    }
}
