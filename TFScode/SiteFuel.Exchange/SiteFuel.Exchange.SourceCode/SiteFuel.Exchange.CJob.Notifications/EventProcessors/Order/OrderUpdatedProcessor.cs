using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class OrderUpdatedProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;

        public EventType EventType => EventType.OrderUpdated;

        public OrderUpdatedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetUpdatedOrderNotificationDetails(notificationEventViewModel.EntityId);
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
                    var callbackUrl = $"~/Supplier/Order/Details/{viewModel.Id}";
                    if (viewModel.IsUpdatedByBuyer)
                    {
                        callbackUrl = !viewModel.IsOpenBrokerOrderExists ? $"~/Buyer/Order/Details?id={viewModel.Id}" : $"~/Supplier/Order/Details/{viewModel.Id}?isBrokeredRequest=true";
                    }
                    EventSubType eventSubType = viewModel.IsUpdatedByBuyer ? EventSubType.OrderUpdated_Buyer : EventSubType.OrderUpdated_Supplier;

                    var notification = notificationDomain.GetNotificationContent(eventSubType, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                    viewModel.PoNumber);

                    SendNotification(viewModel.BuyerUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderUpdatedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
