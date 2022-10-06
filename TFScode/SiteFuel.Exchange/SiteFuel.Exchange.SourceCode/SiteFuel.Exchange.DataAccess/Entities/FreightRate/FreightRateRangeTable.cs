namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightRateRangeTable
    {
        [Key]
        public int Id { get; set; }
        public decimal UptoQuantity { get; set; }
        public decimal RateValue { get; set; }
        public bool IsActive { get; set; }
        public int FreightRateRuleId { get; set; }
        public int FuelGroupId { get; set; }

        [ForeignKey("FuelGroupId")]
        public virtual FuelGroup FuelGroup { get; set; }

        [ForeignKey("FreightRateRuleId")]
        public virtual FreightRateRule FreightRateRule { get; set; }
    }
}
