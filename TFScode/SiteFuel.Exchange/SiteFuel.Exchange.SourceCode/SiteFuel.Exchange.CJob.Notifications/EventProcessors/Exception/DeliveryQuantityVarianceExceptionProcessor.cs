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
    public class DeliveryQuantityVarianceExceptionProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationInvoiceViewModel viewModel;

        public EventType EventType => EventType.DeliveryQuantityVarianceExceptionRaised;

        public DeliveryQuantityVarianceExceptionProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceNotificationDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            //Send an email to all admins of the company
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
            if (companyAdminList.Count > 0)
            {
                var companyType = viewModel.IsBrokeredInvoice ? CompanyType.Supplier : CompanyType.Buyer;
                var callbackUrl = $"~/{companyType.ToString()}/Exception/Manage";
                var notification = GetNotificationContent(notificationEventViewModel.EventType, callbackUrl, companyType);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                        SendNotification(item.Email, notification);
                    }
                }
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                //Send an email to all admins of the company
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, notificationEventViewModel.EventType);
                if (companyAdminList.Count > 0)
                {
                    var callbackUrl = $"~/Supplier/Exception/Manage";
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, callbackUrl, CompanyType.Supplier);
                    var bodyText = notification.BodyText;
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                            SendNotification(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryQuantityVarianceExceptionProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Exception/Manage";
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, callbackUrl, CompanyType.Supplier);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName);
                    SendNotification(viewModel.SupplierUser.Email, notification);

                    callbackUrl = $"~/Buyer/Exception/Manage";
                    notification = GetNotificationContent(notificationEventViewModel.EventType, callbackUrl, CompanyType.Buyer);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.BuyerUser.FirstName);
                    SendNotification(viewModel.BuyerUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryQuantityVarianceExceptionProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, string callbackUrl, CompanyType companyType)
        {
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.InvoiceNumber);
            return bodyText;
        }
    }
}

