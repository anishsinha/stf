namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JobXApprovalUser
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int UserId { get; set; }

        public DateTimeOffset AssignedDate { get; set; }

        public Nullable<System.DateTimeOffset> RemovedDate { get; set; }

        public bool IsActive { get; set; }

        public virtual Job Job { get; set; }

        public virtual User User { get; set; }
    }
}
