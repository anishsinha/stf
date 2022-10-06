namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuoteRequestStatus
    {
        public int Id { get; set; }

        public int QuoteRequestId { get; set; }

        public int StatusId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("QuoteRequestId")]
        public virtual QuoteRequest QuoteRequest { get; set; }

        [ForeignKey("StatusId")]
        public virtual MstQuoteRequestStatus MstQuoteRequestStatus { get; set; }
    }
}
