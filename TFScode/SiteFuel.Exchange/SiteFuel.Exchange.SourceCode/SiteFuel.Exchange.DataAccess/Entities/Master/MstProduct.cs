namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstProduct()
        {
            CompanyFavoriteFuels = new HashSet<CompanyFavoriteFuel>();
            FuelRequests = new HashSet<FuelRequest>();
            MstProductMappings = new HashSet<MstProductMapping>();
            RequestPrices = new HashSet<RequestPrice>();
           // Companies = new HashSet<Company>();
            CurrentCosts = new HashSet<CurrentCost>();
            QuoteRequests = new HashSet<QuoteRequest>();
            OfferPricings = new HashSet<OfferPricing>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductDisplayGroupId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public int PricingSourceId { get; set; }

        public int? TfxProductId { get; set; }

        public int? MappedParentId { get; set; }

        [StringLength(256)]
        public string DisplayName { get; set; }

        [StringLength(32)]
        public string ProductCode { get; set; }

        // for products added as additives at company level
        public int? CompanyId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FuelRequest> FuelRequests { get; set; }

        public virtual MstProductDisplayGroup MstProductDisplayGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MstProductMapping> MstProductMappings { get; set; }

        public virtual MstProductType MstProductType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestPrice> RequestPrices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanyFavoriteFuel> CompanyFavoriteFuels { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Company> Companies { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CurrentCost> CurrentCosts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteRequest> QuoteRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPricing> OfferPricings { get; set; }

        [ForeignKey("PricingSourceId")]
        public virtual MstPricingSource MstPricingSource { get; set; }

        [ForeignKey("TfxProductId")]
        public virtual MstTfxProduct MstTFXProduct { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }


    }
}