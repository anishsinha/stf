namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OfferPricingExclusion
    {
        public int Id { get; set; }

        public int OfferPricingItemId { get; set; }

        public int? TierId { get; set; }

        public int? CustomerId { get; set; }

        public int? StateId { get; set; }

        public int? CityId { get; set; }

        [MaxLength(16)]
        public string ZipCode { get; set; }


        [ForeignKey("OfferPricingItemId")]
        public virtual OfferPricingItem OfferPricingItem { get; set; }

        [ForeignKey("StateId")]
        public virtual MstState MstState { get; set; }

        [ForeignKey("CityId")]
        public virtual MstCity MstCity { get; set; }

        [ForeignKey("TierId")]
        public virtual MstTierType MstTierType { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Company Company { get; set; }
    }
}
