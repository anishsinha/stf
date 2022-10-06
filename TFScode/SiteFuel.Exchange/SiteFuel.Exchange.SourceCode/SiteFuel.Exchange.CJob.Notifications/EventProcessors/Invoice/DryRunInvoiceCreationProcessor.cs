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
    public class DryRunInvoiceCreationProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.DryRunInvoiceCreated;

        public DryRunInvoiceCreationProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetDryrunInvoiceNotificationDetails(notificationEventViewModel.EntityId);
            _doNotSendInvoiceAttachment = viewModel.IsPartOfStatement || viewModel.IsProFormaPo || (!viewModel.SendAttachmentToBuyer && !viewModel.SendAttachmentToSupplier);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                if (viewModel.DeliveryInstructionsExists && viewModel.IsBillingFileRequired())
                {
                    return;
                }
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                NotificationViewModel notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                SendInvoiceCreatedEmailToBuyerUsers(notification);

                //Send an email to Buyer Company admins
                SendInvoiceCreatedEmailToBuyerCompanyAdmins(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DryRunInvoiceCreationProcessor", "SendDefaultBuyerEmailForEvent", "Exception Details : ", ex);
            }
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
                    NotificationViewModel notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    var bodyText = notification.BodyText;
                    notification.BodyText = GetSupplierNotificationBody(notification.BodyText, viewModel.SupplierUser.FirstName);
                    if (SendNotification(viewModel.SupplierUser.Email, notification))
                    {
                        notification.BodyText = bodyText;
                        SendInvoiceCreatedEmailToSupplierUsers(notification);
                        //send email to company admins
                        SendInvoiceCreatedEmailToCompanyAdmins(notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DryRunInvoiceCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceCreatedEmailToSupplierUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.SupplierAccountingUsers)
            {
                notification.BodyText = GetSupplierNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }


        private void SendInvoiceCreatedEmailToCompanyAdmins(NotificationViewModel notification)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            var bodyText = notification.BodyText;
            foreach (var item in companyAdminList)
            {
                if (item.Id != viewModel.SupplierUser.Id)
                {
                    notification.BodyText = bodyText;
                    notification.BodyText = GetSupplierNotificationBody(notification.BodyText, item.FirstName);
                    SendNotification(item.Email, notification);
                }
            }
        }

        private void SendInvoiceCreatedEmailToBuyerUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.UsersAssignedToJob)
            {
                notification.BodyText = GetBuyerNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }

        private void SendInvoiceCreatedEmailToBuyerCompanyAdmins(NotificationViewModel notification)
        {
            List<NotificationUserViewModel> companyAdminList;
            if (viewModel.DeliveryInstructionsExists)
            {
                companyAdminList = notificationDomain.GetEmailSubscribedTpoBuyerAdmins(viewModel.BuyerCompanyId, EventType);
            }
            else
            {
                companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            }
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    notification.BodyText = GetBuyerNotificationBody(notification.BodyText, item.FirstName);
                    SendNotification(item.Email, notification);
                    notification.BodyText = bodyText;
                }
            }
        }

        private string GetBuyerNotificationBody(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.UpdatedByUserName,
                                    viewModel.SupplierCompanyName,
                                    viewModel.InvoiceNumber);
            if (viewModel.IsProFormaPo && viewModel.IsInvoice)
            {
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 1);
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 2);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 1);
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 2);
            }

            return bodyText;
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = $"~/{companyType.ToString()}/Invoice/Details?id={viewModel.Id}";
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            if (companyType == CompanyType.Supplier || (companyType == CompanyType.Buyer && !viewModel.IsProFormaPo))
            {
                var pdfContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Supplier, (companyType == CompanyType.Supplier && viewModel.SendAttachmentToSupplier) || (companyType == CompanyType.Buyer && viewModel.SendAttachmentToBuyer));
                if (pdfContent != null)
                {
                    notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
                }
            }
            return notification;
        }

        private string GetSupplierNotificationBody(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.UpdatedByUserName,
                                    viewModel.InvoiceNumber);
            if (viewModel.IsProFormaPo && viewModel.IsInvoice)
            {
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 1);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 1);
            }
            bodyText = notificationDomain.RemoveBodyContent(bodyText, 2);

            return bodyText;
        }
    }
}
