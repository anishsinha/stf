namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightTablePickupLocation
    {
        [Key]
        public int Id { get; set; }

        public int? FuelSurchargeIndexId { get; set; }

        public int? AccessorialFeeId { get; set; }

        public int? FreightRateRuleId { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("FuelSurchargeIndexId")]
        public virtual FuelSurchargeIndex FuelSurchargeIndex { get; set; }

        [ForeignKey("AccessorialFeeId")]
        public virtual AccessorialFee AccessorialFee { get; set; }

        [ForeignKey("FreightRateRuleId")]
        public virtual FreightRateRule FreightRateRule { get; set; }

        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("BulkPlantId")]
        public virtual BulkPlantLocation BulkPlantLocation { get; set; }
    }
}
