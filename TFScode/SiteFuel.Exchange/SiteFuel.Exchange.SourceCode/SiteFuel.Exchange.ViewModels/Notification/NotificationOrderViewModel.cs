using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationOrderViewModel : BaseNotificationViewModel
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }
        public NotificationUserViewModel BuyerUser { get; set; }
        public NotificationUserViewModel SupplierUser { get; set; }
        public NotificationUserViewModel UpdatedByUser { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public bool IsUpdatedByBuyer { get; set; }
        public string CancellationReason { get; set; }
        public bool IsOpenBrokerOrderExists { get; set; }
        public bool IsProFormaPo { get; set; }
        public bool IsTpoOrder { get; set; }
        public string NewOrderVersionNumber { get; set; }
        public bool IsBrokeredOrder { get; set; }
        public FuelSurchagePricingType PricingType { get; set; }
        public string OrderType { get; set; }
        public string FuelType { get; set; }
        public string OrderName { get; set; }
        public int AssignedCarrierCompanyId { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string TerminalName { get; set; }
        public string DeliveryStartDate { get; set; }
        public string Quantity { get; set; }
        public List<string> SpecialInstructions { get; set; }
        public bool SendInvoiceAttachmentToBuyer { get; set; }
        public bool SendInvoiceAttachmentToSupplier { get; set; }
        public bool SendOrderAttachmentToBuyer { get; set; }
        public bool SendOrderAttachmentToSupplier { get; set; }
    }
}
