namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SourceRegionPickupLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SourceRegionPickupLocation()
        {
        }

        [Key]
        public int Id { get; set; }

        public int SourceRegionId { get; set; }

        public int? TerminalId { get; set; }

        public int? BulkPlantId { get; set; }

        [ForeignKey("SourceRegionId")]
        public virtual SourceRegion SourceRegion { get; set; }

        [ForeignKey("TerminalId")]
        public virtual MstExternalTerminal MstExternalTerminal { get; set; }

        [ForeignKey("BulkPlantId")]
        public virtual BulkPlantLocation BulkPlantLocation { get; set; }
    }
}
