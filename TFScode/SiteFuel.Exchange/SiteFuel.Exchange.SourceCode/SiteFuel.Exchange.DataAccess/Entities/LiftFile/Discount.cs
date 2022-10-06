namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Discount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discount()
        {
            DiscountLineItems = new HashSet<DiscountLineItem>();
        }

        public int Id { get; set; }

        public Nullable<int> InvoiceId { get; set; }

        [Required, StringLength(256)]
        public string DealName { get; set; }

        public int DealStatus { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int CreatedCompanyId { get; set; }

        public Nullable<int> StatusChangedBy { get; set; }

        public Nullable<DateTimeOffset> StatusChangedDate { get; set; }

        public Nullable<int> StatusChangedCompanyId { get; set; }

        public int OrderId { get; set; }

        [Required, StringLength(500)]
        public string Notes { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiscountLineItem> DiscountLineItems { get; set; }
    }
}
