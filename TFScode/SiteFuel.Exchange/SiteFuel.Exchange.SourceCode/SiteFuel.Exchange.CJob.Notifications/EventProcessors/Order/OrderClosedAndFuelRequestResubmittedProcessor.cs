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
    public class OrderClosedAndFuelRequestResubmittedProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;

        public EventType EventType => EventType.OrderClosedAndFuelRequestResubmitted;

        public OrderClosedAndFuelRequestResubmittedProcessor()
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
                var callbackUrl = $"~/Buyer/Order/Details?id={viewModel.Id}";
                SendOrderClosedAndFrResubmittedEmailToBuyer(viewModel, callbackUrl, (int)EventType);

                //Send an email to company admins
                SendOrderClosedAndFrResubmittedEmailToBuyerCompanyAdmins(viewModel, callbackUrl, (int)EventType);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Supplier/Order/Details?id={viewModel.Id}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            //Send an email to company admins
            SendOrderClosedEmailToCompanyAdmins(viewModel, callbackUrl, (int)EventType);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    //Send an email to order creator
                    var closedBy = viewModel.IsUpdatedByBuyer ? $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}" : "you";

                    var callbackUrl = $"~/Supplier/Order/Details?id={viewModel.Id}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.OrderClosed_Owner, _serverUrl, callbackUrl, (int)EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    viewModel.PoNumber,
                                                    closedBy);

                    SendNotification(viewModel.SupplierUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderClosedAndFuelRequestResubmittedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendOrderClosedAndFrResubmittedEmailToBuyer(NotificationOrderViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var notification = notificationDomain.GetNotificationContent(EventSubType.OrderClosedAndFuelRequestResubmitted_Buyer, _serverUrl, callbackUrl, eventTypeId);
            var closedBy = viewModel.IsUpdatedByBuyer ? "you" : $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}";
            notification.BodyText = string.Format(notification.BodyText,
                                            $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                            viewModel.PoNumber,
                                            closedBy);
            SendNotification(viewModel.BuyerUser.Email, notification);
        }

        private void SendOrderClosedAndFrResubmittedEmailToBuyerCompanyAdmins(NotificationOrderViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.OrderClosedAndFuelRequestResubmitted_Buyer, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id)
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
    }
}
