namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OfferBuyerStatuses")]
    public partial class OfferBuyerStatus
    {
        public int Id { get; set; }

        public int OfferPricingId { get; set; }

        public int BuyerCompanyId { get; set; }

        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public virtual MstBuyerOfferStatus MstBuyerOfferStatus { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company Company { get; set; }

        [ForeignKey("OfferPricingId")]
        public virtual OfferPricing OfferPricing { get; set; }

        [StringLength(1024)]
        public string Reason { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
