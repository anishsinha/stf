namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OfferPricingItem
    {
        public int Id { get; set; }

        public int OfferPricingId { get; set; }

        public int? TierId { get; set; }

        public int? CustomerId { get; set; }

        public int? StateId { get; set; }

        public int? CityId { get; set; }

        [MaxLength(16)]
        public string ZipCode { get; set; }

        public int? ParentId { get; set; }

        public int? UpdateCommandId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        
        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        [ForeignKey("UpdateCommandId")]
        public virtual OfferUpdateCommand OfferUpdateCommand { get; set; }

        [ForeignKey("ParentId")]
        public virtual OfferPricingItem ParentOfferPricingItem { get; set; }

        [ForeignKey("OfferPricingId")]
        public virtual OfferPricing OfferPricing { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        [ForeignKey("CityId")]
        public virtual MstCity MstCity { get; set; }

        [ForeignKey("TierId")]
        public virtual MstTierType MstTierType { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferPricingExclusion> OfferPricingExclusions { get; set; } = new HashSet<OfferPricingExclusion>();
    }
}
