using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors.Invoice
{
    public class InvoiceGeneratedWithAvalaraTaxesProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceGeneratedEstablishConnectionWithAvalara;

        public InvoiceGeneratedWithAvalaraTaxesProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            if (viewModel.DeliveryInstructionsExists && viewModel.IsBillingFileRequired())
            {
                return;
            }
            var callbackUrl = $"~/Buyer/Invoice/Details?id={viewModel.Id}";
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            NotificationViewModel notification = new NotificationViewModel();
            var buyerContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Buyer, viewModel.SendAttachmentToBuyer);
            if (buyerContent != null)
            {
                notification.Attachments = GetAttachments(buyerContent, viewModel.InvoiceNumber);
            }

            SendInvoiceCreatedEmailToBuyer(viewModel, callbackUrl, (int)notificationEventViewModel.EventType, notification.Attachments);

            //Send an email to Buyer Company admins
            SendInvoiceCreatedEmailToBuyerCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType, notification.Attachments);
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            NotificationViewModel notification = new NotificationViewModel();
            var supplierContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Supplier, viewModel.SendAttachmentToSupplier);
            if (supplierContent != null)
            {
                notification.Attachments = GetAttachments(supplierContent, viewModel.InvoiceNumber);
            }

            //send email to company admins
            SendInvoiceCreatedEmailToCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType, notification.Attachments);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification;
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceGeneratedEstablishConnectionWithAvalara, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    viewModel.InvoiceNumber,
                                                    string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                                    viewModel.DueDate.Date);
                    var supplierContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Supplier, viewModel.SendAttachmentToSupplier);
                    if (supplierContent != null)
                    {
                        notification.Attachments = GetAttachments(supplierContent, viewModel.InvoiceNumber);
                    }

                    SendNotification(viewModel.SupplierUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceCreatedEmailToCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId, List<Attachment> attachments)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceGeneratedEstablishConnectionWithAvalara, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;

                notification.Attachments = attachments;
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.InvoiceNumber,
                                                                string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                                                viewModel.DueDate.Date);
                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendInvoiceCreatedEmailToBuyer(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId, List<Attachment> attachments)
        {
            NotificationViewModel notification;
            notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceGeneratedEstablishConnectionWithAvalara, _serverUrl, callbackUrl, eventTypeId);
            notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
            notification.BodyText = string.Format(notification.BodyText,
                                                        $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                                viewModel.InvoiceNumber,
                                                                string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                                                viewModel.DueDate.Date);
            notification.BodyButtonUrl = string.Empty;
            notification.Attachments = attachments;
            SendNotification(viewModel.BuyerUser.Email, notification);
        }

        private void SendInvoiceCreatedEmailToBuyerCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId, List<Attachment> attachments)
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
                NotificationViewModel notification;
                notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceGeneratedEstablishConnectionWithAvalara, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                notification.Attachments = attachments;

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                        notification.BodyText = string.Format(bodyText,
                                                         $"{item.FirstName} {item.LastName}",
                                                                viewModel.InvoiceNumber,
                                                                string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                                                viewModel.DueDate.Date);
                        notification.BodyButtonUrl = string.Empty;

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
