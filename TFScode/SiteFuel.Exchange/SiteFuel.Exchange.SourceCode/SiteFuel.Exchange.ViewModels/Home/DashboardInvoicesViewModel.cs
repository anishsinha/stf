using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardInvoicesViewModel : StatusViewModel
    {
        public DashboardInvoicesViewModel()
        {
            
        }

        public DashboardInvoicesViewModel(Status status)
            : base(status)
        {
           
        }

        public int TotalInvoiceCount { get; set; }
        public int ReceivedInvoiceCount { get; set; }
        public int NotApprovedInvoiceCount { get; set; }
        public int ApprovedInvoiceCount { get; set; }
        public int UnconfirmedInvoiceCount { get; set; }
        public int WaitingForPriceCount { get; set; }
        public int InvoicesFromDropTicketCount { get; set; }
        public int SelectedJobId { get; set; }
        public int DropTicketCount { get; set; }
        public int RejectedDropTicketCount { get; set; }
        public string GroupIds { get; set; }
    }
}
