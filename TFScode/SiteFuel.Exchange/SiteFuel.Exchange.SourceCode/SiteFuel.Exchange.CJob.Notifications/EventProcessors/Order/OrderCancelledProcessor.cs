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
    public class OrderCancelledProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;

        public EventType EventType => EventType.OrderCancelled;

        public OrderCancelledProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetOrderNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            if (!viewModel.IsOpenBrokerOrderExists)
            {
                var cancelledBy = viewModel.IsUpdatedByBuyer ?
                                       $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}" :
                                       $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}";
                var callbackUrl = $"~/Buyer/Order/Details?id={viewModel.Id}";
                SendOrderCancelledEmailToBuyer(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
                SendOrderCancelledEmailToBuyerCompanyAdmins(viewModel, callbackUrl, cancelledBy, (int)notificationEventViewModel.EventType);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Supplier/Order/Details?id={viewModel.Id}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            var cancelledBy = viewModel.IsUpdatedByBuyer ?
                                        $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}" :
                                        $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}";

            //Send an email to company admins
            SendOrderCancelledEmailToCompanyAdmins(viewModel, callbackUrl, cancelledBy, (int)notificationEventViewModel.EventType);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    //Send an email to order creator
                    var callbackUrl = $"~/Supplier/Order/Details?id={viewModel.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.OrderCancelled_Owner, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    viewModel.PoNumber,
                                                    viewModel.CancellationReason);

                    SendNotification(viewModel.SupplierUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderCancelledProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendOrderCancelledEmailToBuyerCompanyAdmins(NotificationOrderViewModel viewModel, string callbackUrl, string cancelledByUserName, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.OrderCancelled_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.PoNumber,
                                                                cancelledByUserName,
                                                                viewModel.CancellationReason);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendOrderCancelledEmailToBuyer(NotificationOrderViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var notification = notificationDomain.GetNotificationContent(EventSubType.OrderCancelled_Buyer, _serverUrl, callbackUrl, eventTypeId);
            notification.BodyText = string.Format(notification.BodyText,
                                            $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                            viewModel.PoNumber,
                                            viewModel.CancellationReason);
            SendNotification(viewModel.BuyerUser.Email, notification);
        }

    }
}
