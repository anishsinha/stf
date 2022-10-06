using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationPendingDdtToInvoiceConversionModel
    {
        public NotificationPendingDdtToInvoiceConversionModel()
        {
        }

        public int DdtId { get; set; }
        public int InvoiceHeaderId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string TimeZoneName { get; set; }
        public int CreatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int DefaultNotificationPeriod { get; set; } = ApplicationConstants.DefaultNotificationPeriod;
    }    
}
