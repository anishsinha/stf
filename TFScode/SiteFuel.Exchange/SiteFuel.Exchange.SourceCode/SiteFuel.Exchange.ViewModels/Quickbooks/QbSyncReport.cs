using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Quickbooks
{
    public class QbSyncReport
    {
        public QbReport QbInvoices { get; set; } = new QbReport();
        public QbReport QbSalesOders { get; set; } = new QbReport();
        public QbReport QbPurchaseOrders { get; set; } = new QbReport();
        public QbReport QbBills { get; set; } = new QbReport();
        public QbReport QbCreditMemos { get; set; } = new QbReport();
        public QbReport QbVendorCredits { get; set; } = new QbReport();
    }

    public class QbReport
    {
        public int TotalSynced { get; set; }
        public List<string> Created { get; set; } = new List<string>();
        public List<string> Modified { get; set; } = new List<string>();
        public List<string> Skipped { get; set; } = new List<string>();
        public List<string> Failed { get; set; } = new List<string>();
    }
}
