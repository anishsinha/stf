namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MstNewsfeedEvent
    {
        public int Id { get; set; }

        public int EntityTypeId { get; set; }

        [Required]
        [StringLength(64)]
        public string EventType { get; set; }
    }
}
