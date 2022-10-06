namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceXDeclineReason
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int DeclineReasonId { get; set; }

        public string AdditionalNotes { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual MstInvoiceDeclineReason MstInvoiceDeclineReason { get; set; }
    }
}
