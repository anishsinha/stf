namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class FreightRatePtoPTable
    {
        public FreightRatePtoPTable()
        {
            FreightRatePtoPLocations = new HashSet<FreightRatePtoPLocation>();
        }

        [Key]
        public int Id { get; set; }
        public int? TerminalId { get; set; }
        public int? BulkPlantId { get; set; }
        public bool IsActive { get; set; }
        public int FreightRateRuleId { get; set; }

        [ForeignKey("FreightRateRuleId")]
        public virtual FreightRateRule FreightRateRule { get; set; }

        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("BulkPlantId")]
        public virtual BulkPlantLocation BulkPlantLocation { get; set; }
        public virtual ICollection<FreightRatePtoPLocation> FreightRatePtoPLocations { get; set; }
    }
}
