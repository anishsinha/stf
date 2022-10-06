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
	public class BrokerFuelRequestCreationProcessor : BaseFuelRequestEventProcessor, IEmailProcessor
    {
		private NotificationFuelRequestCreatedViewModel viewModel;

        public EventType EventType => EventType.BrokerFuelRequestCreated;

        public BrokerFuelRequestCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetFuelRequestNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
		}

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
		}

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

			var callbackUrl = $"~/Supplier/FuelRequest/Details?id={viewModel.Id}";
			//send email to eligible suppliers
			SendFuelRequestEmailToSuppliers(viewModel, callbackUrl, (int)notificationEventViewModel.EventType, EventSubType.FuelRequestCreated_Supplier);

            //Send an email to company admins
            SendBrokerFuelRequestEmailToCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.Id > 0)
                {
                    //Send an email to FR creator
                    var callbackUrl = $"~/Supplier/FuelRequest/Details?id={viewModel.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.BrokerFuelRequestCreated_Owner, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Creator.FirstName} {viewModel.Creator.LastName}",
                                                    viewModel.FuelRequestNumber);

                    SendNotification(viewModel.Creator.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BrokerFuelRequestCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendBrokerFuelRequestEmailToCompanyAdmins(NotificationFuelRequestCreatedViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.CompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.BrokerFuelRequestCreated_CompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Creator.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                $"{item.FirstName} {item.LastName}",
                                                $"{viewModel.Creator.FirstName} {viewModel.Creator.LastName}",
                                                viewModel.FuelRequestNumber);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
