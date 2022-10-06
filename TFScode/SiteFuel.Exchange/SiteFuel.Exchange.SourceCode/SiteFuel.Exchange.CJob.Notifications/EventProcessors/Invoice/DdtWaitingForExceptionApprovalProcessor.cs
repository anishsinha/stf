using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class DdtWaitingForExceptionApprovalProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.DdtCreatedAsInvoiceIsWaitingForExceptionApproval;

        public DdtWaitingForExceptionApprovalProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                GetInvoiceTypeId(viewModel);
                var companyType = viewModel.IsBrokeredInvoice ? CompanyType.Supplier : CompanyType.Buyer;
                var callbackUrl = $"~/{companyType.ToString()}/Invoice/Details?id={viewModel.Id}";
                NotificationViewModel notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, callbackUrl);
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                SendInvoiceCreatedEmailToBuyerUsers(notification);

                //Send an email to Buyer Company admins
                SendInvoiceCreatedEmailToBuyerCompanyAdmins(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RebillInvoiceCreationProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, callbackUrl);
                    var bodyText = notification.BodyText;
                    var driverName = string.IsNullOrWhiteSpace(viewModel.DriverName) ? viewModel.UpdatedByUserName : viewModel.DriverName;
                    notification.BodyText = GetNotificationBody(notification.BodyText, viewModel.SupplierUser.FirstName, driverName);
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
                LogManager.Logger.WriteException("RebillInvoiceCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceCreatedEmailToSupplierUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.SupplierAccountingUsers)
            {
                var driverName = string.IsNullOrWhiteSpace(viewModel.DriverName) ? viewModel.UpdatedByUserName : viewModel.DriverName;
                notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName, driverName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }


        private void SendInvoiceCreatedEmailToCompanyAdmins(NotificationViewModel notification)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        var driverName = string.IsNullOrWhiteSpace(viewModel.DriverName) ? viewModel.UpdatedByUserName : viewModel.DriverName;
                        notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName, driverName);
                        SendNotification(item.Email, notification);
                        notification.BodyText = bodyText;
                    }
                }
            }
        }

        private void SendInvoiceCreatedEmailToBuyerUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.UsersAssignedToJob.Where(t => t.Id != viewModel.BuyerUser.Id))
            {
                notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName, viewModel.UpdatedByUserName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }

        private void SendInvoiceCreatedEmailToBuyerCompanyAdmins(NotificationViewModel notification)
        {
            List<NotificationUserViewModel> companyAdminList;
            companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    notification.BodyText = GetNotificationBody(notification.BodyText, item.FirstName, viewModel.UpdatedByUserName);
                    SendNotification(item.Email, notification);
                    notification.BodyText = bodyText;
                }
            }
        }

        private string GetNotificationBody(string bodyText, string firstName, string droppedBy)
        {
            bodyText = string.Format(bodyText, firstName, droppedBy, viewModel.DropEndTime, viewModel.DropDate, $"{viewModel.InvoiceNumber}");
            return bodyText;
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            var pdfContent = GetPdfFileContent(viewModel.Id, (int)companyType, (companyType == CompanyType.Supplier && viewModel.SendAttachmentToSupplier) || (companyType == CompanyType.Buyer && viewModel.SendAttachmentToBuyer));
            if (pdfContent != null)
            {
                notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
            }

            notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
            notification.BodyButtonText = string.Format(notification.BodyButtonText);
            return notification;
        }
    }
}
