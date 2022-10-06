namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImpersonationActivityLog")]
    public partial class ImpersonationActivityLog
    {
        public int Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public int ImpersonatedUserId { get; set; }

        public int ImpersonatedByUserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public string Data { get; set; }
    }
}
