namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SourceRegionCarrier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SourceRegionCarrier()
        {
        }

        [Key]
        public int Id { get; set; }

        public int SourceRegionId { get; set; }

        [ForeignKey("SourceRegionId")]
        public virtual SourceRegion SourceRegion { get; set; }

        public int CarrierId { get; set; }

        [ForeignKey("CarrierId")]
        public virtual Company Company { get; set; }
    }
}
