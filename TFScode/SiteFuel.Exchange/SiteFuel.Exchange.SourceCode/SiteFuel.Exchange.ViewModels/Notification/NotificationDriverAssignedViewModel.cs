using SiteFuel.Exchange.ViewModels.Notification;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationDriverAssignedViewModel : BaseNotificationViewModel
    {
        public NotificationDriverAssignedViewModel()
        {
            Driver = new NotificationUserViewModel();
            SupplierUser = new NotificationUserViewModel();
        }

        public int OrderId { get; set; }

        public string PoNumber { get; set; }

        public NotificationUserViewModel Driver { get; set; }

        public NotificationUserViewModel SupplierUser { get; set; }

        public int SupplierCompanyId { get; set; }

        public string SupplierCompanyName { get; set; }
    }
}
