namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceApprovalHistoryGridViewModel : StatusViewModel
    {
        public InvoiceApprovalHistoryGridViewModel()
        {
        }

        public InvoiceApprovalHistoryGridViewModel(Utilities.Status status) 
            : base(status)
        {
        }

        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public string AssignedTo { get; set; }

        public string ApprovedBy { get; set; }

        public string ApprovedDate { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedTime { get; set; }

        public string Status { get; set; }

        public bool IsApprovalUser { get; set; }

        public string JobName { get; set; }

        public string PoNumber { get; set; }

        public decimal Quantity { get; set; }

        public int StatusId { get; set; }

        public int UnitOfMeasurement { get; set; }

        public int Currency { get; set; }
    }
}
