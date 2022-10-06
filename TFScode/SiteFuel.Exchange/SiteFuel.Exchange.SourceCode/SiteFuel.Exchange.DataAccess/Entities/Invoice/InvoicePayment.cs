namespace SiteFuel.Exchange.DataAccess.Entities
{
    using SiteFuel.Exchange.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class InvoicePayment
    {
        public int Id { get; set; }

        public int InvoiceNumberId { get; set; }

        [MaxLength(32)]
        public string QbInvoiceNumber { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        public decimal BalanceRemaining { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTimeOffset PaymentDate { get; set; }

        [MaxLength(100)]
        public string TxnId { get; set; }

        [MaxLength(100)]
        public string TransRefNumber { get; set; }

        [Required]
        public PaymentSource PaymentSource { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public int? UpdatedBy { get; set; }

        [Required]
        public DateTimeOffset UpdatedDate { get; set; }
    }
}
