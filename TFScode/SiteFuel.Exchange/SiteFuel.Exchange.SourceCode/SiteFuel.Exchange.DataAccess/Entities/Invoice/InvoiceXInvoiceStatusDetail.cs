namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InvoiceXInvoiceStatusDetail
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public int StatusId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual MstInvoiceStatus MstInvoiceStatus { get; set; }

        public virtual User User { get; set; }
    }
}
