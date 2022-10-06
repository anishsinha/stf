namespace SiteFuel.Exchange.ViewModels
{
    public class UspSupplierPerformanceViewModel
    {
        public int SupplierCompanyId { get; set; }
        public string SupplierName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalOrderValue { get; set; }
        public decimal GallonsDelivered { get; set; }
        public int TotalDeliveries { get; set; }
        public int MissedDeliveries { get; set; }
        public int LateDelivries { get; set; }
        public int DeliveryOverages { get; set; }

    }
}
