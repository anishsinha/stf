namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightRatePtoPLocation
    {
        public FreightRatePtoPLocation()
        {
            FreightRatePtoPFuelGroups = new HashSet<FreightRatePtoPFuelGroup>();
        }

        [Key]
        public int Id { get; set; }
        public int FreightRatePtoPTableId { get; set; }
        public int JobId { get; set; }
        public string LaneID { get; set; }
        public decimal AssumedMiles { get; set; }
        public bool IsLaneRequired { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }

        [ForeignKey("FreightRatePtoPTableId")]
        public virtual FreightRatePtoPTable FreightRatePtoPTable { get; set; }

        public virtual ICollection<FreightRatePtoPFuelGroup> FreightRatePtoPFuelGroups { get; set; }
    }
}
