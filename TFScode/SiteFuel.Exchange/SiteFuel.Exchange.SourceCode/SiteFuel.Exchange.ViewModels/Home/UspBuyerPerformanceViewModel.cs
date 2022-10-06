namespace SiteFuel.Exchange.ViewModels
{
    public class UspBuyerPerformanceViewModel
    {
        public int BuyerCompanyId { get; set; }
        public string BuyerName { get; set; }
        public string AccountingCompanyId { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpend { get; set; }
        public decimal TotalGallonsOrdered { get; set; }
        public decimal TotalGallonsDelivered { get; set; }
        public decimal AveragePPG { get; set; }
        public int TotalDDTCount { get; set; }
        public int TotalInvoiceCount { get; set; }
        public int AssignedTierId { get; set; }
        public string CustomerId { get; set; }
    }
}
