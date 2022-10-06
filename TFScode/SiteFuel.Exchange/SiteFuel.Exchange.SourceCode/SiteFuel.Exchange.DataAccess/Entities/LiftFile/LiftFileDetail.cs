using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LiftFileDetail
    {
        public LiftFileDetail()
        {
            LiftFileValidationRecords = new HashSet<LiftFileValidationRecord>();
        }

        public int Id { get; set; }

        [StringLength(512)]
        public string LFID { get; set; } //Terminal+Bol
        public int AddedBy { get; set; }
        public int CompanyId { get; set; }

        [StringLength(512)]
        public string ExternalRefId { get; set; }
        public DateTimeOffset AddedDate { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<LiftFileValidationRecord> LiftFileValidationRecords { get; set; }
    }
}
