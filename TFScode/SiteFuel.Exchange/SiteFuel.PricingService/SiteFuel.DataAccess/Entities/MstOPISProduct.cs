namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstOPISProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstOPISProduct()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public int PricingSourceId { get; set; }

        public int? MstProductId { get; set; }

        [StringLength(32)]
        public string ProductCode { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
        
        public int? TfxProductId { get; set; }

        [ForeignKey("TfxProductId")]
        public virtual MstTfxProduct MstTfxProduct { get; set; }

        [ForeignKey("PricingSourceId")]
        public virtual MstPricingSource MstPricingSource { get; set; }

        [ForeignKey("MstProductId")]
        public virtual MstProduct MstProduct { get; set; }
    }
}
