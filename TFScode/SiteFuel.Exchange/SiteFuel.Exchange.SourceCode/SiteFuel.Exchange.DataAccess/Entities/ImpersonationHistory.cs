namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ImpersonationHistory")]
    public partial class ImpersonationHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ImpersonatedBy { get; set; }

        public DateTimeOffset ImpersonatedStartTime { get; set; }

        public Nullable<System.DateTimeOffset> ImpersonatedEndTime { get; set; }

        public int? TerminatedBy { get; set; }
    }
}
