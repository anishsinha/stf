namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OfferTierMapping
    {
        public int Id { get; set; }

        public int SupplierCompanyId { get; set; }

        public int BuyerCompanyId { get; set; }

        public int TierId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        [ForeignKey("SupplierCompanyId")]
        public virtual Company SupplierCompany { get; set; }

        [ForeignKey("BuyerCompanyId")]
        public virtual Company BuyerCompany { get; set; }

        [ForeignKey("TierId")]
        public virtual MstTierType MstTierType { get; set; }
    }
}
