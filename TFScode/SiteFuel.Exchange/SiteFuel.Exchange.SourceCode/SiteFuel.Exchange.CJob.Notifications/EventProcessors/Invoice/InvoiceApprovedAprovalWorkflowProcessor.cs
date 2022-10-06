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
    public class InvoiceApprovedAprovalWorkflowProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceApprovedApprovalWorkflow;

        public InvoiceApprovedAprovalWorkflowProcessor()
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
            var callbackUrl = $"~/Buyer/Invoice/Details?id={viewModel.Id}";
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            var buyerContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Buyer, viewModel.SendAttachmentToBuyer);
            if (buyerContent != null)
            {
                viewModel.Attachments = GetAttachments(buyerContent, viewModel.InvoiceNumber);
            }

            SendInvoiceApprovedApprovalWorkflowEmailToBuyer(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
            SendInvoiceApprovedApprovalWorkflowEmailToBuyerCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
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

            viewModel.Attachments = notification.Attachments;

            SendInvoiceApprovedApprovalWorkflowToCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification;
                    if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_Supplier_AprovalWorkflow, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_Supplier_AprovalWorkflow, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    viewModel.InvoiceNumber,
                                                    $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                    viewModel.CreatedOn.DateTime.DayOfWeek,
                                                    viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                    $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                    $"{viewModel.ApprovedOn.DateTime.Hour}:{viewModel.ApprovedOn.DateTime.Minute}",
                                                    viewModel.ApprovedOn.DateTime.DayOfWeek,
                                                    viewModel.ApprovedOn.DateTime.ToShortDateString(),
                                                    viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Hours,
                                                    viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Minutes,
                                                    viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Seconds);
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
                LogManager.Logger.WriteException("InvoiceApprovedAprovalWorkflowProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceApprovedApprovalWorkflowToCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_SupplierCompanyAdmin_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_SupplierCompanyAdmin_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
                }
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                notification.Attachments = viewModel.Attachments;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                                        viewModel.InvoiceNumber,
                                                        $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                        viewModel.CreatedOn.DateTime.DayOfWeek,
                                                        viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                        $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                        $"{viewModel.ApprovedOn.DateTime.Hour}:{viewModel.ApprovedOn.DateTime.Minute}",
                                                        viewModel.ApprovedOn.DateTime.DayOfWeek,
                                                        viewModel.ApprovedOn.DateTime.ToShortDateString(),
                                                        viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Hours,
                                                        viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Minutes,
                                                        viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Seconds);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendInvoiceApprovedApprovalWorkflowEmailToBuyer(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            NotificationViewModel notification;
            if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
            {
                notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_Buyer_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
            }
            else
            {
                notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_Buyer_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
            }
            notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
            notification.BodyText = string.Format(notification.BodyText,
                                viewModel.InvoiceNumber,
                                $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                viewModel.CreatedOn.DateTime.DayOfWeek,
                                viewModel.CreatedOn.DateTime.ToShortDateString(),
                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                $"{viewModel.ApprovedOn.DateTime.Hour}:{viewModel.ApprovedOn.DateTime.Minute}",
                                viewModel.ApprovedOn.DateTime.DayOfWeek,
                                viewModel.ApprovedOn.DateTime.ToShortDateString(),
                                viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Hours,
                                viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Minutes,
                                viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Seconds);
            notification.Attachments = viewModel.Attachments;

            if (SendNotification(viewModel.BuyerUser.Email, notification))
            {
                if (viewModel.InvoiceApprover != null && viewModel.BuyerUser.Id != viewModel.InvoiceApprover.Id)
                {
                    SendNotification(viewModel.InvoiceApprover.Email, notification);
                }
            }
        }

        private void SendInvoiceApprovedApprovalWorkflowEmailToBuyerCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApproved_BuyerCompanyAdmin_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApproved_BuyerCompanyAdmin_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
                }
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                notification.Attachments = viewModel.Attachments;

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id && item.Id != viewModel.InvoiceApprover.Id)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                viewModel.InvoiceNumber,
                                $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                viewModel.CreatedOn.DateTime.DayOfWeek,
                                viewModel.CreatedOn.DateTime.ToShortDateString(),
                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                $"{viewModel.ApprovedOn.DateTime.Hour}:{viewModel.ApprovedOn.DateTime.Minute}",
                                viewModel.ApprovedOn.DateTime.DayOfWeek,
                                viewModel.ApprovedOn.DateTime.ToShortDateString(),
                                viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Hours,
                                viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Minutes,
                                viewModel.ApprovedOn.Subtract(viewModel.CreatedOn).Seconds);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
