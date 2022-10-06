using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class BillingStatementCreationProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        private new NotificationBillingStatementViewModel viewModel;

        public EventType EventType => EventType.BillingStatementGenerated;

        public BillingStatementCreationProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetBillingStatementDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                NotificationViewModel notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, string.Empty);

                SendInvoiceCreatedEmailToBuyerUsers(notification);

                //Send an email to Buyer Company admins
                SendInvoiceCreatedEmailToBuyerCompanyAdmins(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingStatementCreationProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
                    var callbackUrl = $"~/Supplier/BillingStatement/Details?id={viewModel.Id}";
                    NotificationViewModel notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, callbackUrl);

                    SendInvoiceCreatedEmailToSupplierUsers(notification);
                    //send email to company admins
                    SendInvoiceCreatedEmailToSupplierCompanyAdmins(notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BillingStatementCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            
        }

        private void SendInvoiceCreatedEmailToSupplierUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.SupplierUsers)
            {
                notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }


        private void SendInvoiceCreatedEmailToSupplierCompanyAdmins(NotificationViewModel notification)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName);
                    SendNotification(item.Email, notification);
                    notification.BodyText = bodyText;
                }
            }
        }

        private void SendInvoiceCreatedEmailToBuyerUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.UsersAssignedToJob)
            {
                notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }
        
        private void SendInvoiceCreatedEmailToBuyerCompanyAdmins(NotificationViewModel notification)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if(companyAdminList.Count == 0 && viewModel.IsTpoOrder)
            {
                companyAdminList = notificationDomain.GetEmailSubscribedTpoBuyerAdmins(viewModel.BuyerCompanyId, EventType);
            }
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName);
                    SendNotification(item.Email, notification);
                    notification.BodyText = bodyText;
                }
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType , CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            var pdfContent = GetStatementPdfFileContent(viewModel.Id);
            if (pdfContent != null)
            {
                notification.Attachments = GetAttachments(pdfContent, viewModel.StatementName);
            }
            notification.Subject = string.Format(notification.Subject, viewModel.Frequency);
            return notification;
        }

        private string GetNotificationBody(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.Frequency,
                                    viewModel.StatementName,
                                    viewModel.BuyerCompanyName,
                                    viewModel.SupplierCompanyName,
                                    viewModel.StartDate,
                                    viewModel.EndDate,
                                    viewModel.DueDate.ToString(Resource.constFormatDate)
                                    );
            return bodyText;
        }
    }
}
