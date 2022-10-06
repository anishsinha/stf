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
    public class InvoiceUpdatedProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceUpdated;

        public InvoiceUpdatedProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceUpdatedNotificationDetails(notificationEventViewModel.EntityId);
            _doNotSendInvoiceAttachment = viewModel.IsPartOfStatement || viewModel.IsProFormaPo || (!viewModel.SendAttachmentToBuyer && !viewModel.SendAttachmentToSupplier);
        }


        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                if (viewModel.DeliveryInstructionsExists && (viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.None || viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.SendOnlyBillingFile))
                {
                    return;
                }
                GetInvoiceTypeId(viewModel);
                var companyType = viewModel.IsBrokeredInvoice ? CompanyType.Supplier : CompanyType.Buyer;
                var callbackUrl = $"~/{companyType.ToString()}/Invoice/Details?id={viewModel.Id}";
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                NotificationViewModel notification = GetBuyerNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, callbackUrl);
                SendInvoiceCreatedEmailToBuyerUsers(notification);

                //Send an email to Buyer Company admins
                SendInvoiceCreatedEmailToBuyerCompanyAdmins(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceUpdatedProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                    NotificationViewModel notification = GetSupplierNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, callbackUrl);
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
                LogManager.Logger.WriteException("InvoiceUpdatedProcessor", "SendEmail", ex.Message, ex);
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
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = GetSupplierNotificationBody(notification.BodyText, item.FirstName);
                        SendNotification(item.Email, notification);
                        notification.BodyText = bodyText;
                    }
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
                                    $"{viewModel.UpdatedByUserName}",
                                    viewModel.SupplierCompanyName,
                                    $"{GetInvoiceType()} {viewModel.InvoiceNumber}",
                                    $"{viewModel.DayOfWeek} {viewModel.UpdatedDate}",
                                    viewModel.UpdatedTime,
                                    string.Concat(viewModel.DropAdditionalDetails.Where(t => t.IsExceedingQuantity).Select(t => t.PoNumber).ToList(), ", "));
            bodyText = viewModel.DropAdditionalDetails.Any(t => t.IsExceedingQuantity) ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.ReplaceInvoiceWithDdt ? notificationDomain.RemoveBodyContent(bodyText, 2) : notificationDomain.ReplaceBodyContent(bodyText, 2);
            return bodyText;
        }

        private NotificationViewModel GetBuyerNotificationContent(EventType eventType, CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            byte[] fileContent = null;
            if (viewModel.ReplaceInvoiceWithDdt)
            {
                ReplaceInvoiceContentWithDdt(notification);
                fileContent = GetDdtPdfFileFromInvoice(viewModel.Id, (int)companyType, viewModel.SendAttachmentToBuyer);
            }
            else
            {
                fileContent = GetPdfFileContent(viewModel.Id, (int)companyType, viewModel.SendAttachmentToBuyer);
            }
            if (fileContent != null)
            {
                notification.Attachments = GetAttachments(fileContent, viewModel.InvoiceNumber);
            }
            notification.Subject = string.Format(notification.Subject, $"{GetInvoiceType()} {viewModel.InvoiceNumber}");
            notification.BodyButtonText = string.Format(notification.BodyButtonText, GetInvoiceType());
            return notification;
        }

        private NotificationViewModel GetSupplierNotificationContent(EventType eventType, CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            var pdfContent = GetPdfFileContent(viewModel.Id, (int)companyType, viewModel.SendAttachmentToSupplier);
            if (pdfContent != null)
            {
                notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
            }
            notification.Subject = string.Format(notification.Subject, $"{GetInvoiceType()} {viewModel.InvoiceNumber}");
            notification.BodyButtonText = string.Format(notification.BodyButtonText, GetInvoiceType());
            return notification;
        }

        private string GetSupplierNotificationBody(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    $"{viewModel.UpdatedByUserName}",
                                    $"{GetInvoiceType()} {viewModel.InvoiceNumber}",
                                    $"{viewModel.DayOfWeek} {viewModel.UpdatedDate}",
                                    viewModel.UpdatedTime,
                                    string.Concat(viewModel.DropAdditionalDetails.Where(t => t.IsExceedingQuantity).Select(t => t.PoNumber).ToList(), ", "));
            bodyText = viewModel.DropAdditionalDetails.Any(t => t.IsExceedingQuantity) ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            return bodyText;
        }

        private string GetInvoiceType()
        {
            return viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp
                    ? Resource.lblInvoice : Resource.lblDDT;
        }
    }
}
