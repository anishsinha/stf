using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoicePaymentViewModel
    {
        public string DisplayInvoiceNumber { get; set; }

        public int InvoiceNumberId { get; set; }

        public string TransRefNumber { get; set; }

        public DateTimeOffset PaymentDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string QbInvoiceNumber { get; set; }

        public decimal BalanceRemaining { get; set; }

        public decimal AmountPaid { get; set; }

        public int CompanyId { get; set; }
    }
}
