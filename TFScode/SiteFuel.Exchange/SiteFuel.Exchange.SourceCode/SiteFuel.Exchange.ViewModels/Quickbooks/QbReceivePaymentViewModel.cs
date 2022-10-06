using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class QbReceivePaymentViewModel
    {
        public string TxnId { get; set; }

        public string TransRefNumber { get; set; }

        public string PaymentDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public string QbInvoiceNumber { get; set; }

        public decimal BalanceRemaining { get; set; }

        public decimal AmountPaid { get; set; }

        public int CompanyId { get; set; }
    }
}
