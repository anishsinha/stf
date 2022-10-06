namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuoteRequestDocument
    {
        public int Id { get; set; }

        [Required]
        [StringLength(252)]
        public string FileName { get; set; }

        [StringLength(1024)]
        public string ModifiedFileName { get; set; }

        public int? QuoteRequestId { get; set; }

        public int? QuotationId { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }

        public Nullable<DateTimeOffset> UpdatedDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("QuoteRequestId")]
        public virtual QuoteRequest QuoteRequest { get; set; }

        [ForeignKey("QuotationId")]
        public virtual Quotation Quotation { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
    }
}
