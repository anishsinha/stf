using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDealViewModel : BaseNotificationViewModel
    {
        public NotificationDealViewModel()
        {
            BuyerUser = new NotificationUserViewModel();
            SupplierUser = new NotificationUserViewModel();
            UsersAssignedToJob = new List<NotificationUserViewModel>();
            SupplierAccountingUsers = new List<NotificationUserViewModel>();
        }

        public int DealId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string DealName { get; set; }
        public string DealCreatedBy { get; set; }
        public string DealStatusChangedBy { get; set; }
        public int SupplierCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public NotificationUserViewModel BuyerUser { get; set; }
        public NotificationUserViewModel SupplierUser { get; set; }
        public List<NotificationUserViewModel> UsersAssignedToJob { get; set; }
        public List<NotificationUserViewModel> SupplierAccountingUsers { get; set; }
    }
}
