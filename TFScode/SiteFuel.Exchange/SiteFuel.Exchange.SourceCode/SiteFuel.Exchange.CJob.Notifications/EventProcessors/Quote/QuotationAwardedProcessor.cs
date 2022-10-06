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
	public class QuotationAwardedProcessor : BaseQuoteEventProcessor, IEmailProcessor
    {
		private NotificationQuoteViewModel viewModel;

        public EventType EventType => EventType.QuotationAwarded;

        public QuotationAwardedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetQuoteNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
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
                var callbackUrl = $"~/Supplier/Order/Details?id={viewModel.OrderId}";
                var notification = notificationDomain.GetNotificationContent(EventSubType.QuotationAwarded_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                notification.BodyText = string.Format(notification.BodyText,
                                                    $"{ viewModel.SupplierFirstName }{ viewModel.SupplierLastName }",
                                                    viewModel.BuyerCompany,
                                                    viewModel.SupplierQuoteNumber,
                                                    viewModel.BuyerQuoteNumber);
                notification.To = viewModel.SupplierEmail;
                SendNotification(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuotationAwardedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
