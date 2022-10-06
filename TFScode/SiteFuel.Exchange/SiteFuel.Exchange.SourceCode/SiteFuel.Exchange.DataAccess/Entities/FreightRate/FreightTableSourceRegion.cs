namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightTableSourceRegion
    {
        [Key]
        public int Id { get; set; }

        public int? FuelSurchargeIndexId { get; set; }

        public int? AccessorialFeeId { get; set; }

        public int? FreightRateRuleId { get; set; }

        public int SourceRegionId { get; set; }

        [ForeignKey("SourceRegionId")]
        public virtual SourceRegion SourceRegion { get; set; }

        [ForeignKey("FuelSurchargeIndexId")]
        public virtual FuelSurchargeIndex FuelSurchargeIndex { get; set; }

        [ForeignKey("AccessorialFeeId")]
        public virtual AccessorialFee AccessorialFee { get; set; }

        [ForeignKey("FreightRateRuleId")]
        public virtual FreightRateRule FreightRateRule { get; set; }
    }
}
