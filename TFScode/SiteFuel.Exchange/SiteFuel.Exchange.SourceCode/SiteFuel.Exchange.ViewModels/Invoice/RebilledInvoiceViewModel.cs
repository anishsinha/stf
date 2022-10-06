namespace SiteFuel.Exchange.ViewModels
{
    public class RebilledInvoiceViewModel
    {
        public int OriginalInvoiceId { get; set; }
        public string OriginalInvoiceDisplayNumber { get; set; }
        public int CreditedInvoiceId { get; set; }
        public string CreditedInvoiceDisplayNumber { get; set; }
    }
}