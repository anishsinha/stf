namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightRateFuelGroup
    {
        [Key]
        public int Id { get; set; }
        public int MinQuantity { get; set; }
        public int FuelGroupId { get; set; }
        public int FreightRateRuleId { get; set; }

        [ForeignKey("FuelGroupId")]
        public virtual FuelGroup FuelGroup { get; set; }

        [ForeignKey("FreightRateRuleId")]
        public virtual FreightRateRule FreightRateRule { get; set; }

    }
}
