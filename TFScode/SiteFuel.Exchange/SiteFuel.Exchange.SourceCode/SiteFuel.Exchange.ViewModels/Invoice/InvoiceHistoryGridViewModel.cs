namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceHistoryGridViewModel : StatusViewModel
    {
        public InvoiceHistoryGridViewModel()
        {
        }

        public InvoiceHistoryGridViewModel(Utilities.Status status) 
            : base(status)
        {
        }

        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public int InvoiceHeaderId { get; set; }

        public string Version { get; set; }

        public string Quantity { get; set; }

        public string PricePerGallon { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string DropDate { get; set; }

        public string DropTime { get; set; }

        public string InvoiceDate { get; set; }

        public string ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string Status { get; set; }
    }
}
