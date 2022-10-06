namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public partial class AssetSubcontractor
    {
        public int Id { get; set; }

        public int AssetId { get; set; }

        public int SubcontractorId { get; set; }

        public int AssignedBy { get; set; }

        public DateTimeOffset AssignedDate { get; set; }

        public Nullable<int> RemovedBy { get; set; }

        public Nullable<System.DateTimeOffset> RemovedDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("AssetId")]
        public virtual Asset Asset { get; set; }

        [ForeignKey("SubcontractorId")]
        public virtual Subcontractor Subcontractor { get; set; }
    }
}
