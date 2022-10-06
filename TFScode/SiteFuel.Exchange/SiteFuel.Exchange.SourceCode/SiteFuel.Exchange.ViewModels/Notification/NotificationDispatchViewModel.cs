using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDispatchViewModel : BaseNotificationViewModel
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }
        public string JobName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public NotificationUserViewModel BuyerUser { get; set; }
        public NotificationUserViewModel SupplierUser { get; set; }
        public List<NotificationUserViewModel> OnsitePersons { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public bool IsTpoOrder { get; set; }
    }
}
