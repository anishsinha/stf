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
	public class CounterOfferCreationProcessor : BaseCounterOfferEventProcessor,  IEmailProcessor
    {
		private NotificationCounterOfferCreatedViewModel viewModel;

        public EventType EventType => EventType.CounterOfferCreated;

        public CounterOfferCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetCounterOfferNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
		}

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
		}

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.Id > 0)
                {
                    if (viewModel.CreatorRole == UserRoles.Buyer)
                    {
                        // send email to supplier that CO is created
                        var callbackUrl = $"~/Supplier/FuelRequest/Details?id={viewModel.Id}";
                        var notification = notificationDomain.GetNotificationContent(EventSubType.CounterOfferCreated_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                        notification.BodyText = string.Format(notification.BodyText,
                                                        $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                        viewModel.FuelRequestNumber);

                        SendNotification(viewModel.Supplier.Email, notification);
                    }
                    else
                    {
                        // send email to buyer that supplier has created CO
                        var callbackUrl = $"~/Buyer/CounterOffer/Details?fuelRequestId={viewModel.Id}&supplierId={viewModel.Supplier.Id}";
                        var notification = notificationDomain.GetNotificationContent(EventSubType.CounterOfferCreated_Buyer, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                        notification.BodyText = string.Format(notification.BodyText,
                                                        $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                        viewModel.FuelRequestNumber);

                        SendNotification(viewModel.Buyer.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CounterOfferCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
