using SiteFuel.Exchange.Core;
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
    public class InvoiceCreatedAprovalWorkflowProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceCreatedApprovalWorkflow;

        public InvoiceCreatedAprovalWorkflowProcessor()
        {
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

            SendInvoiceCreatedApprovalWorkflowEmailToBuyer(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
            SendInvoiceCreatedApprovalWorkflowEmailToBuyerCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
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

            SendInvoiceCreatedApprovalWorkflowToCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
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
                        notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceCreated_Supplier_AprovalWorkflow, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreated_Supplier_AprovalWorkflow, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                    $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                    $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                    viewModel.CreatedOn.DateTime.DayOfWeek,
                                                    viewModel.CreatedOn.DateTime.ToShortDateString());

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
                LogManager.Logger.WriteException("InvoiceCreatedAprovalWorkflowProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceCreatedApprovalWorkflowEmailToBuyer(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            NotificationViewModel notification;
            if (viewModel.DropAdditionalDetails.Any(t => t.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest))
            {
                var quantityDelivered = ($"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity).GetPreciseValue().GetCommaSeperatedValue()} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}").ToLower();
                notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryCompletedForTPO_Buyer, _serverUrl, callbackUrl, eventTypeId);
                notification.BodyText = string.Format(notification.BodyText,
                                                        $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                        viewModel.SupplierCompanyName,
                                                        quantityDelivered,
                                                        viewModel.DropDate,
                                                        viewModel.DropStartTime,
                                                        viewModel.DropEndTime, viewModel.DropAdditionalDetails.Select(t => t.PoNumber).FirstOrDefault());
                notification.BodyButtonUrl = string.Empty;
            }
            else
            {
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceCreated_Buyer_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreated_Buyer_AprovalWorkflow, _serverUrl, callbackUrl, eventTypeId);
                }
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                notification.BodyText = string.Format(notification.BodyText,
                                                viewModel.JobName,
                                                $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                viewModel.CreatedOn.DateTime.DayOfWeek,
                                                viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");
            }
            notification.Attachments = viewModel.Attachments;
            if (SendNotification(viewModel.BuyerUser.Email, notification))
            {
                if (viewModel.InvoiceApprover != null && viewModel.BuyerUser.Id != viewModel.InvoiceApprover.Id)
                {
                    SendNotification(viewModel.InvoiceApprover.Email, notification);
                }
            }
        }

        private void SendInvoiceCreatedApprovalWorkflowToCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceCreated_AprovalWorkflow_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreated_AprovalWorkflow_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }

                notification.Attachments = viewModel.Attachments;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (viewModel.InvoiceApprover != null && item.Id != viewModel.SupplierUser.Id && item.Id != viewModel.InvoiceApprover.Id)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                                        $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                        $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                        $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                        viewModel.CreatedOn.DateTime.DayOfWeek,
                                                        viewModel.CreatedOn.DateTime.ToShortDateString());

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendInvoiceCreatedApprovalWorkflowEmailToBuyerCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.DropAdditionalDetails.Any(t => t.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest))
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryCompletedForTPO_Buyer, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceCreated_AprovalWorkflow_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreated_AprovalWorkflow_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                    }
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                }
                var bodyText = notification.BodyText;
                notification.Attachments = viewModel.Attachments;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        if (viewModel.DropAdditionalDetails.Any(t => t.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest))
                        {
                            notification.BodyText = string.Format(bodyText,
                                                           $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                           viewModel.SupplierCompanyName,
                                                           viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity),
                                                           viewModel.DropDate,
                                                           viewModel.DropStartTime,
                                                           viewModel.DropEndTime, viewModel.DropAdditionalDetails.Select(t => t.PoNumber).FirstOrDefault());
                            notification.BodyButtonUrl = string.Empty;
                        }
                        else
                        {
                            notification.BodyText = string.Format(bodyText,
                                                            viewModel.JobName,
                                                            $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                            $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                            viewModel.CreatedOn.DateTime.DayOfWeek,
                                                            viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                            $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");
                        }
                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
