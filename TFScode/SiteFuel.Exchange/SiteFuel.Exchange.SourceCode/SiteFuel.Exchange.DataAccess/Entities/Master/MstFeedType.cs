namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstFeedType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MstFeedType()
        {
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        public int PricingSourceId { get; set; }

        [ForeignKey("PricingSourceId")]
        public virtual MstPricingSource PricingSource { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}