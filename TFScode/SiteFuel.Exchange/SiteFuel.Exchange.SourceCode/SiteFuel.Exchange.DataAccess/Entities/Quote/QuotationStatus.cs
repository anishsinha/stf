namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class QuotationStatus
    {
        public int Id { get; set; }

        public int QuotationId { get; set; }

        public int StatusId { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [ForeignKey("QuotationId")]
        public virtual Quotation Quotation { get; set; }

        [ForeignKey("StatusId")]
        public virtual MstQuoteRequestStatus MstQuoteRequestStatus { get; set; }
    }
}
