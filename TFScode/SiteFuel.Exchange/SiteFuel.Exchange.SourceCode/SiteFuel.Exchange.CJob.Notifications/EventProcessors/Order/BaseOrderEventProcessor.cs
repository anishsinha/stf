using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public abstract class BaseOrderEventProcessor : BaseEventProcessor
	{
		protected NotificationDomain notificationDomain;

        public override List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			return notificationDomain.GetBuyerDefaultRecievers(notificationEventViewModel.TriggeredByCompanyId, notificationEventViewModel.EventType);
		}

		public override List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
		{
			//throw new NotImplementedException();
			return new List<NotificationUserViewModel>();
		}

        protected virtual void SendOrderClosedEmailToCompanyAdmins(NotificationOrderViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType.OrderClosed);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.OrderClosed_OwnerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.PoNumber,
                                                                $"{viewModel.UpdatedByUser.FirstName} {viewModel.UpdatedByUser.LastName}");

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
        protected virtual void SendOrderCancelledEmailToCompanyAdmins(NotificationOrderViewModel viewModel, string callbackUrl, string canceledByUserName, int eventTypeId)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType.OrderCancelled);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.OrderCancelled_OwnerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.PoNumber,
                                                                canceledByUserName,
                                                                viewModel.CancellationReason);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
