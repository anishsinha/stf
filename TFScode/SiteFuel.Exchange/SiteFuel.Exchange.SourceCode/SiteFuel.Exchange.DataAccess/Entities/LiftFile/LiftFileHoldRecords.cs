using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class LiftFileHoldRecords
    {
        public int Id { get; set; }
        public int LiftFileRecordId { get; set; }
        public string JsonString { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("LiftFileRecordId")]
        public virtual LiftFileValidationRecord LiftFileValidationRecord { get; set; }
    }
}
