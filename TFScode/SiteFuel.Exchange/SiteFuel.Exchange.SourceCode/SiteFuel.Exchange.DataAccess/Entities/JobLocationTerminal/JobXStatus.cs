namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobXStatuses")]
    public partial class JobXStatus
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int StatusId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual Job Job { get; set; }

        public virtual MstJobStatus MstJobStatus { get; set; }
    }
}
