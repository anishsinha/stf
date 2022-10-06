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
    public class InvoiceCreatedViaMobileDropProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceCreatedViaMobileDrop;

        public InvoiceCreatedViaMobileDropProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                if (viewModel.DeliveryInstructionsExists && (viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.None || viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.SendOnlyBillingFile))
                {
                    return;
                }
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                GetInvoiceTypeId(viewModel);
                var companyType = viewModel.IsBrokeredInvoice ? CompanyType.Supplier : CompanyType.Buyer;
                var callbackUrl = $"~/{companyType.ToString()}/Invoice/Details?id={viewModel.Id}";
                NotificationViewModel notification = GetBuyerNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, callbackUrl);

                SendInvoiceCreatedEmailToBuyerUsers(notification);

                //Send an email to Buyer Company admins
                SendInvoiceCreatedEmailToBuyerCompanyAdmins(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreatedViaMobileDropProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification = GetSupplierNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, callbackUrl);
                    var bodyText = notification.BodyText;
                    notification.BodyText = GetSupplierNotificationBody(notification.BodyText, viewModel.SupplierUser.FirstName);
                    if (SendNotification(viewModel.SupplierUser.Email, notification))
                    {
                        notification.BodyText = bodyText;
                        SendInvoiceToSupplierUsers(notification);
                        SendInvoiceCreatedEmailToSupplierCompanyAdmins(notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreatedViaMobileDropProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {

        }

        private void SendInvoiceToSupplierUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.SupplierAccountingUsers)
            {
                notification.BodyText = GetSupplierNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
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

        private void SendInvoiceCreatedEmailToSupplierCompanyAdmins(NotificationViewModel notification)
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
                                    $"{viewModel.DriverName}",
                                    viewModel.SupplierCompanyName,
                                    $"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity)} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}",
                                    viewModel.JobName,
                                    $"{viewModel.DayOfWeek} {viewModel.DropDate}",
                                    viewModel.DropStartTime, viewModel.DropEndTime,
                                    string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                    $"{GetInvoiceType()} {viewModel.InvoiceNumber}",
                                    viewModel.DueDate.ToString(Resource.constFormatDate));
            bodyText = IsInvoiceCreated() && !viewModel.IsPartOfStatement ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.DropAdditionalDetails.Any(t => t.IsExceedingQuantity) ? notificationDomain.ReplaceBodyContent(bodyText, 2) : notificationDomain.RemoveBodyContent(bodyText, 2);
            if (viewModel.IsProFormaPo && viewModel.IsInvoice)
            {
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 3);
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 4);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 3);
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 4);
            }
            bodyText = viewModel.ReplaceInvoiceWithDdt ? notificationDomain.RemoveBodyContent(bodyText, 5) : notificationDomain.ReplaceBodyContent(bodyText, 5);
            return bodyText;
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

        private NotificationViewModel GetBuyerNotificationContent(EventType eventType, CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            byte[] pdfContent = null;
            if (viewModel.ReplaceInvoiceWithDdt)
            {
                ReplaceInvoiceContentWithDdt(notification);
                pdfContent = GetDdtPdfFileFromInvoice(viewModel.Id, (int)companyType, viewModel.SendAttachmentToBuyer);
            }
            else
            {
                pdfContent = GetPdfFileContent(viewModel.Id, (int)companyType, viewModel.SendAttachmentToBuyer);
            }
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
                                    firstName, viewModel.DriverName,
                                    $"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity)} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}",
                                    viewModel.JobName,
                                    $"{viewModel.DayOfWeek} {viewModel.DropDate}",
                                    viewModel.DropStartTime, viewModel.DropEndTime,
                                   string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                    $"{GetInvoiceType()} {viewModel.InvoiceNumber}", viewModel.DueDate.ToString(Resource.constFormatDate)
                                    );
            bodyText = IsInvoiceCreated() && !viewModel.IsPartOfStatement ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.DropAdditionalDetails.Any(t => t.IsExceedingQuantity) ? notificationDomain.ReplaceBodyContent(bodyText, 2) : notificationDomain.RemoveBodyContent(bodyText, 2);
            if (viewModel.IsProFormaPo && viewModel.IsInvoice)
            {
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 3);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 3);
            }
            return bodyText;
        }

        private bool IsInvoiceCreated()
        {
            return viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp;
        }

        private string GetInvoiceType()
        {
            return IsInvoiceCreated() ? Resource.lblInvoice : Resource.lblDDT;
        }
    }
}
