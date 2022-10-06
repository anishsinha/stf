namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightRatePtoPFuelGroup
    {
        [Key]
        public int Id { get; set; }
        public int FreightRatePtoPLocationId { get; set; }
        public int FuelGroupId { get; set; }
        public decimal RateValue { get; set; }

        [ForeignKey("FuelGroupId")]
        public virtual FuelGroup FuelGroup { get; set; }

        [ForeignKey("FreightRatePtoPLocationId")]
        public virtual FreightRatePtoPLocation FreightRatePtoPLocation { get; set; }
    }
}
