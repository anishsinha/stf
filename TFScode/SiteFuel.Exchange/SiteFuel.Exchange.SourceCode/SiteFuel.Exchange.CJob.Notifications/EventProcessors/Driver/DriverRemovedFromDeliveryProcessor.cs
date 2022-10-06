using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class DriverRemovedFromDeliveryProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationDriverAssignedViewModel viewModel;

        public EventType EventType => EventType.DriverRemovedFromDelivery;

        public DriverRemovedFromDeliveryProcessor()
        {
        }
        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetDriverDeliveryScheduleNotificationDetails(notificationEventViewModel.EntityId, false);
        }

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            //Send an email to supplier
            SendDriverRemovedFromDeliveryEmailToSupplier(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);

            //Send an email to supplier company admins
            SendDriverRemovedFromDeliveryEmailToSupplierCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.OrderId > 0)
                {
                    var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DriverRemovedFromDelivery_Driver, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Driver.FirstName} {viewModel.Driver.LastName}",
                                                    viewModel.PoNumber,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    viewModel.SupplierCompanyName);

                    SendNotification(viewModel.Driver.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverRemovedFromDeliveryProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendDriverRemovedFromDeliveryEmailToSupplierCompanyAdmins(NotificationDriverAssignedViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DriverRemovedFromDelivery_SupplierAdmin, _serverUrl, callbackUrl, eventTypeId);

                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                             viewModel.PoNumber,
                                                             $"{viewModel.Driver.FirstName} {viewModel.Driver.LastName}");

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendDriverRemovedFromDeliveryEmailToSupplier(NotificationDriverAssignedViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            if (viewModel.SupplierUser.Id > 0 && viewModel.SupplierUser.Id != viewModel.Driver.Id)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DriverRemovedFromDelivery_Supplier, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                //to avoid duplicate emails in case Admin is creator
                notification.BodyText = string.Format(bodyText,
                                                        $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                        $"{viewModel.Driver.FirstName} {viewModel.Driver.LastName}",
                                                        viewModel.PoNumber);

                SendNotification(viewModel.SupplierUser.Email, notification);
            }
        }
    }
}
