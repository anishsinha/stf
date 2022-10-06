using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationQuoteViewModel : BaseNotificationViewModel
    {
        public NotificationQuoteViewModel()
        {
            SupplierEmail = new List<string>();
        }

        public string SupplierFirstName { get; set; }

        public string SupplierLastName { get; set; }

        public List<string> SupplierEmail { get; set; }

        public string SupplierQuoteNumber { get; set; }

        public string BuyerCompany { get; set; }

        public string BuyerQuoteNumber { get; set; }

        public string EndDate { get; set; }

        public int OrderId { get; set; }

        public List<NotificationUserViewModel> Suppliers { get; set; }

        public int QuoteId { get; set; }

        public int QuotesNeeded { get; set; }

        public int QuotesReceived { get; set; }
        
    }
}
