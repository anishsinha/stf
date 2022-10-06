using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.Notification
{
	public class BaseNotificationViewModel
	{
		public List<NotificationUserViewModel> DefaultSupplierEmailRecievers { get; set; } = new List<NotificationUserViewModel>();
		public List<NotificationUserViewModel> DefaultBuyerEmailRecievers { get; set; } = new List<NotificationUserViewModel>();
	}
}
