namespace SiteFuel.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MstPricingCode
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public int PricingSourceId { get; set; }

        [Required]
        public int PricingTypeId { get; set; }

        [Required]
        public int RackTypeId { get; set; }

        [Required]
        public int FeedTypeId { get; set; }

        [Required]
        public int QuantityIndicatorId { get; set; }

        [Required]
        public int FuelClassTypeId { get; set; }

        [Required]
        public int WeekendPricingTypeId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("PricingSourceId")]
        public virtual MstPricingSource MstPricingSource { get; set; }
    }
}
