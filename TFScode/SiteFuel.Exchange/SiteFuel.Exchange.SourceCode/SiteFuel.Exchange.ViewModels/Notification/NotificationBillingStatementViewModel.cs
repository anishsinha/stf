using System;
using System.Collections.Generic;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationBillingStatementViewModel : BaseNotificationViewModel
    {
        public int Id { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public DateTime DueDate { get; set; }
        public string Frequency { get; set; }
        public string StatementName { get; set; }
        public string PreviousStatementName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsTpoOrder { get; set; }
        public List<System.Net.Mail.Attachment> Attachments { get; set; } = new List<System.Net.Mail.Attachment>();
        public List<NotificationUserViewModel> UsersAssignedToJob { get; set; } = new List<NotificationUserViewModel>();
        public List<NotificationUserViewModel> SupplierUsers { get; set; } = new List<NotificationUserViewModel>();
    }
}
