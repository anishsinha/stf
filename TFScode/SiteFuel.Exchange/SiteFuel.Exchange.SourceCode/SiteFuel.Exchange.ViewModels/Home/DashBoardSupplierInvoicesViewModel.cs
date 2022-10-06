using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardSupplierInvoicesViewModel : StatusViewModel
    {
        public int TotalInvoiceCount { get; set; }

        public int ApprovedInvoiceCount { get; set; }

        public int UnconfirmedInvoiceCount { get; set; }

        public int NotApprovedInvoiceCount { get; set; }

        public int ReceivedInvoiceCount { get; set; }

        public int CreatedInvoiceCount { get; set; }

        public int InvoicesFromDropTicketCount { get; set; }

        public int WaitingForPriceCount { get; set; }
        
        public int DropTicketCount { get; set; }

        public int DropTicketRejectedCount { get; set; }

        public string GroupIds { get; set; }

        public int TotalDDTCount { get; set; }
        public int UnconfirmedDDTCount { get; set; }
        public int ApprovedDDTCount { get; set; }
        public int ReceivedDDTCount { get; set; }
        public int NotApprovedDDTCount { get; set; }
        public int CreatedDDTCount { get; set; }
        public int WaitingForPriceDDTCount { get; set; }

    }
}
