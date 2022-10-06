namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class InvoiceHeaderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceHeaderDetail()
        {
            Invoices = new HashSet<Invoice>();
            InvoiceXBolDetails = new HashSet<InvoiceXBolDetail>();
        }

        public int Id { get; set; }        

        public int InvoiceNumberId { get; set; }

        public decimal TotalDroppedGallons { get; set; }

        public decimal TotalBasicAmount { get; set; }

        public decimal TotalFeeAmount { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public int Version { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceXBolDetail> InvoiceXBolDetails { get; set; }

        [ForeignKey("InvoiceNumberId")]
        public virtual InvoiceNumber InvoiceNumber { get; set; }
    }
}
