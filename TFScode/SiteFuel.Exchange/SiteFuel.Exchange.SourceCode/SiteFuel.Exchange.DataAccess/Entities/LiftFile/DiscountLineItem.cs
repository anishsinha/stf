namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DiscountLineItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiscountLineItem()
        {
            
        }

        public int Id { get; set; }

        public int DiscountId { get; set; }

        public int FeeTypeId { get; set; }

        public int FeeSubTypeId { get; set; }

        public decimal Amount { get; set; }

        [StringLength(256)]
        public string FeeDetails { get; set; }

        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }

        [ForeignKey("FeeSubTypeId")]
        public virtual MstFeeSubType MstFeeSubType { get; set; }

        [ForeignKey("FeeTypeId")]
        public virtual MstFeeType MstFeeType { get; set; }
    }
}
