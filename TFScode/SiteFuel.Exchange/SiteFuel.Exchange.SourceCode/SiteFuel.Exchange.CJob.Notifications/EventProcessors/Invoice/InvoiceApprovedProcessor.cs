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
    public class InvoiceApprovedProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceApproved;

        public InvoiceApprovedProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceNotificationDetailsForApprovedInvoice(notificationEventViewModel.EntityId, notificationEventViewModel.TriggeredByUserId);
            _doNotSendInvoiceAttachment = viewModel.IsPartOfStatement || viewModel.IsProFormaPo || (!viewModel.SendAttachmentToBuyer && !viewModel.SendAttachmentToSupplier);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            if (viewModel.DeliveryInstructionsExists && viewModel.IsBillingFileRequired())
            {
                return;
            }
            var callbackUrl = $"~/Buyer/Invoice/Details?id={viewModel.Id}";
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            var buyerContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Buyer, viewModel.SendAttachmentToBuyer);
            if (buyerContent != null)
            {
                viewModel.Attachments = GetAttachments(buyerContent, viewModel.InvoiceNumber);
            }

            SendInvoiceApprovedEmailToBuyer(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
            SendInvoiceApprovedEmailToApprover(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
            SendInvoiceApprovedEmailToBuyerCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
            NotificationViewModel supplierNotification = new NotificationViewModel();

            var supplierContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Supplier, viewModel.SendAttachmentToSupplier);
            if (supplierContent != null)
            {
                supplierNotification.Attachments = GetAttachments(supplierContent, viewModel.InvoiceNumber);
            }
            viewModel.Attachments = supplierNotification.Attachments;

            SendInvoiceApprovedEmailToSupplierAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel supplierNotification;
                    if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        supplierNotification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    else
                    {
                        supplierNotification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    supplierNotification.Subject = string.Format(supplierNotification.Subject, viewModel.InvoiceNumber);
                    supplierNotification.BodyText = string.Format(supplierNotification.BodyText,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    viewModel.InvoiceNumber,
                                                    $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");

                    var supplierContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Supplier, viewModel.SendAttachmentToSupplier);
                    if (supplierContent != null)
                    {
                        supplierNotification.Attachments = GetAttachments(supplierContent, viewModel.InvoiceNumber);
                    }

                    SendNotification(viewModel.SupplierUser.Email, supplierNotification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceApprovedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceApprovedEmailToSupplierAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                var bodyText = notification.BodyText;
                notification.Attachments = viewModel.Attachments;

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                $"{item.FirstName} {item.LastName}",
                                                viewModel.InvoiceNumber,
                                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendInvoiceApprovedEmailToBuyer(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            NotificationViewModel notification;
            if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_Buyer, _serverUrl, callbackUrl, eventTypeId);
            }
            else
            {
                notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_Buyer, _serverUrl, callbackUrl, eventTypeId);
            }
            notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
            notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                     $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                    viewModel.InvoiceNumber);
            notification.Attachments = viewModel.Attachments;
            SendNotification(viewModel.BuyerUser.Email, notification);
        }

        private void SendInvoiceApprovedEmailToApprover(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            NotificationViewModel notification;
            if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_Approver, _serverUrl, callbackUrl, eventTypeId);
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
            }
            else
            {
                notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_Approver, _serverUrl, callbackUrl, eventTypeId);
            }
            notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                    viewModel.InvoiceNumber);
            notification.Attachments = viewModel.Attachments;
            SendNotification(viewModel.InvoiceApprover.Email, notification);
        }

        private void SendInvoiceApprovedEmailToBuyerCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
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
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                var bodyText = notification.BodyText;
                notification.Attachments = viewModel.Attachments;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                $"{item.FirstName} {item.LastName}",
                                                viewModel.InvoiceNumber,
                                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
