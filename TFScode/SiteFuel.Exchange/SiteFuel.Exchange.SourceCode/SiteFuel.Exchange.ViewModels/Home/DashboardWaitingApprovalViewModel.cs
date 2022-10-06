using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardWaitingApprovalViewModel : StatusViewModel
    {
        public DashboardWaitingApprovalViewModel()
        {
            
        }

        public DashboardWaitingApprovalViewModel(Status status)
            : base(status)
        {
           
        }

        public int TotalCount { get; set; }

        public int InvoiceCount { get; set; }

        public int DropTicketCount { get; set; }

        public int SelectedJobId { get; set; }

        public int RejectedInvoiceCount { get; set; }

        public int RejectedDropTicketCount { get; set; }

        public string GroupIds { get; set; }
    }
}
