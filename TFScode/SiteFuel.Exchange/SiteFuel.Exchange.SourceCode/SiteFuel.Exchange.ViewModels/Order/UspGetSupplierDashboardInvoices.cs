namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetSupplierDashboardInvoices
    {
        public int Total { get; set; }
        public int Unconfirmed { get; set; }
        public int Approved { get; set; }
        public int Received { get; set; }
        public int NotApproved { get; set; }
        public int Created { get; set; }
        public int InvoicesFromDDT { get; set; }
        public int DropTicketCount { get; set; }
        public int RejectedDropTicketCount { get; set; }

        public int WaitingForPrice { get; set; }

        public int TotalDDT { get; set; }
        public int UnconfirmedDDT { get; set; }
        public int ApprovedDDT { get; set; }
        public int ReceivedDDT { get; set; }
        public int NotApprovedDDT { get; set; }
        public int CreatedDDT { get; set; }
        public int WaitingForPriceDDT{ get; set; }

    }
}
