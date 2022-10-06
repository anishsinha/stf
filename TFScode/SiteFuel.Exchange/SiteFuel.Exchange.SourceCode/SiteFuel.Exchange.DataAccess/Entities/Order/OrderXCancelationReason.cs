namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderXCancelationReason
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        public int ReasonId { get; set; }

        public bool IsAlreadyResubmittedFuel { get; set; }

        public int CanceledBy { get; set; }

        public string AdditionalNotes { get; set; }

        public virtual MstOrderCancelationReason MstOrderCancelationReason { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
