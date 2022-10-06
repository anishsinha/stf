namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationFuelRequestStatusViewModel : NotificationFuelRequestViewModel
    {
        public NotificationUserViewModel Supplier { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string OrderNumber { get; set; }
        public int OrderId { get; set; }
        public bool SendOrderAttachmentToBuyer { get; set; }
        public bool SendOrderAttachmentToSupplier { get; set; }
    }
}
