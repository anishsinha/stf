using Newtonsoft.Json;
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
    public class DealAcceptedForInvoiceProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationDealViewModel viewModel;
        public EventType EventType => EventType.DealAcceptedForInvoice;

        private readonly List<string> supplierUsersEmailSent = new List<string>();

        public DealAcceptedForInvoiceProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetDealNotificationDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            SendEmailToBuyerUsers(notificationEventViewModel);
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                var bodyText = notification.BodyText;

                //Supplier User
                if (!supplierUsersEmailSent.Contains(viewModel.SupplierUser.Email))
                {
                    notification.BodyText = GetNotificationBodyText(bodyText, viewModel.SupplierUser.FirstName);
                    SendNotification(viewModel.SupplierUser.Email, notification);
                    supplierUsersEmailSent.Add(viewModel.SupplierUser.Email);
                }

                //Supplier Accounting Users
                foreach (var item in viewModel.SupplierAccountingUsers)
                {
                    if (!supplierUsersEmailSent.Contains(item.Email))
                    {
                        notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                        SendNotification(item.Email, notification);
                        supplierUsersEmailSent.Add(item.Email);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DealAcceptedForInvoiceProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.DealId > 0)
                {
                    //Send an email to all supplier admins of the company
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    var bodyText = notification.BodyText;

                    var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, notificationEventViewModel.EventType);
                    foreach (var item in companyAdminList)
                    {
                        notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                        SendNotificationForDefaultEvent(item.Email, notification);
                        supplierUsersEmailSent.Add(item.Email);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DealAcceptedForInvoiceProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendEmailToBuyerUsers(NotificationEventViewModel notificationEventViewModel)
        {
            List<string> emailSent = new List<string>();
            try
            {
                //Send an email to all buyer admins of the company
                var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                var bodyText = notification.BodyText;

                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
                foreach (var item in companyAdminList)
                {
                    notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                    SendNotificationForDefaultEvent(item.Email, notification);
                    emailSent.Add(item.Email);
                }

                //Buyer User
                if (!emailSent.Contains(viewModel.BuyerUser.Email))
                {
                    notification.BodyText = GetNotificationBodyText(bodyText, viewModel.BuyerUser.FirstName);
                    SendNotification(viewModel.BuyerUser.Email, notification);
                    emailSent.Add(viewModel.BuyerUser.Email);
                }

                //User Assigned to Job
                foreach (var item in viewModel.UsersAssignedToJob)
                {
                    if (!emailSent.Contains(item.Email))
                    {
                        notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                        SendNotification(item.Email, notification);
                        emailSent.Add(item.Email);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DealAcceptedForInvoiceProcessor", "SendEmailToBuyerUsers", ex.Message, ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;

            if (companyType == CompanyType.Buyer)
                callbackUrl = $"~/Buyer/Invoice/Details/{viewModel.InvoiceId}";
            else if (companyType == CompanyType.Supplier)
                callbackUrl = $"~/Supplier/Invoice/Details/{viewModel.InvoiceId}";

            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.DealStatusChangedBy,
                                    viewModel.DealName,
                                    viewModel.InvoiceNumber);
            return bodyText;
        }
    }
}