namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class OrderDetailVersion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderDetailVersion()
        {
        }

        public int Id { get; set; }

        public int OrderId { get; set; }

        public int Version { get; set; }

        [StringLength(256)]
        public string PoNumber { get; set; }

        public int PaymentTermId { get; set; }

        public int NetDays { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public PaymentMethods PaymentMethod { get; set; }
        public EditPropertyType? EditPropertyType { get; set; }
        public string JsonOrderHistory { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("PaymentTermId")]
        public virtual MstPaymentTerm PaymentTerm { get; set; }
    }
}
