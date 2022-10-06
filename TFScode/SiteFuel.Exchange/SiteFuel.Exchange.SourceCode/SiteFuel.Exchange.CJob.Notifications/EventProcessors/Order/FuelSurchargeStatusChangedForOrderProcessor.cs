using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class FuelSurchargeStatusChangedForOrderProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;

        public EventType EventType => EventType.FuelSurchargeStatusChangedForOrder;

        public FuelSurchargeStatusChangedForOrderProcessor()
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

            //Send an email to all admins of the company
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                var bodyText = notification.BodyText;
                notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
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
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    var bodyText = notification.BodyText;
                    notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                            SendNotification(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeStatusChangedForOrderProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName, notificationEventViewModel.JsonMessage);
                    notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    SendNotification(viewModel.SupplierUser.Email, notification);

                    if (!viewModel.IsTpoOrder)
                    {
                        notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.BuyerUser.FirstName, notificationEventViewModel.JsonMessage);
                        notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                        SendNotification(viewModel.BuyerUser.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelSurchargeStatusChangedForOrderProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;

            if (companyType == CompanyType.Buyer)
                callbackUrl = $"~/Buyer/Order/Details/{viewModel.Id}";
            else if (companyType == CompanyType.Supplier)
                callbackUrl = $"~/Supplier/Order/Details/{viewModel.Id}";

            var notification = notificationDomain.GetEmailNotificationContent(eventType, CompanyType.Supplier, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName, string jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<OrderFuelSurchargeMessageViewModel>(jsonMessage);

            if (message.FuelSurchargeStatus == Resource.lblEnabled)
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 1);
            else
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 1);

            bodyText = string.Format(bodyText,
                                    firstName,
                                    $"{viewModel.UpdatedByUser.FirstName} {viewModel.UpdatedByUser.LastName}",
                                    message.FuelSurchargeStatus.ToLower(),
                                    viewModel.PoNumber, viewModel.PricingType.GetDisplayName());
            return bodyText;
        }

        private string GetNotificationSubjectText(string subjectText, string jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<OrderFuelSurchargeMessageViewModel>(jsonMessage);

            subjectText = string.Format(subjectText,
                                     message.FuelSurchargeStatus.ToLower());
            return subjectText;
        }
    }
}

