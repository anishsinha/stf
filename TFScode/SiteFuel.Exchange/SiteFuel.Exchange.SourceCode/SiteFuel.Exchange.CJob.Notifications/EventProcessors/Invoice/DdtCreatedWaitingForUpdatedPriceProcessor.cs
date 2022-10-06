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
    public class DdtCreatedWaitingForUpdatedPriceProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.DdtCreatedAsInvoiceIsWaitingForUpdatedPrice;

        public DdtCreatedWaitingForUpdatedPriceProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            if (viewModel.DeliveryInstructionsExists && (viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.None || viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.SendOnlyBillingFile))
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

            SendDDTCreatedInsteadOfInvoiceEmailToBuyer(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
            SendDDTCreatedInsteadOfInvoiceEmailToBuyerCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
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
                    NotificationViewModel notification;
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    var driverName = string.IsNullOrWhiteSpace(viewModel.DriverName) ? $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}" : viewModel.DriverName;
                    notification.BodyText = string.Format(notification.BodyText,
                                                    driverName,
                                                    viewModel.DropEndTime,
                                                    viewModel.DropDate,
                                                    viewModel.DropDate);
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
                LogManager.Logger.WriteException("DdtCreatedWaitingForUpdatedPriceProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendDDTCreatedInsteadOfInvoiceEmailToBuyer(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            NotificationViewModel notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Buyer, _serverUrl, callbackUrl, eventTypeId);
            notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
            var driverName = string.IsNullOrWhiteSpace(viewModel.DriverName) ?
                $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}" : viewModel.DriverName;
            notification.BodyText = string.Format(notification.BodyText,
                                                    driverName,
                                                    viewModel.DropEndTime,
                                                    viewModel.DropDate,
                                                    viewModel.DropDate);
            notification.Attachments = viewModel.Attachments;
            SendNotification(viewModel.BuyerUser.Email, notification);
        }

        private void SendDDTCreatedInsteadOfInvoiceEmailToBuyerCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
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
                NotificationViewModel notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Buyer, _serverUrl, callbackUrl, eventTypeId);
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                var bodyText = notification.BodyText;
                notification.Attachments = viewModel.Attachments;
                var driverName = string.IsNullOrWhiteSpace(viewModel.DriverName) ?
                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}" : viewModel.DriverName;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                            driverName,
                                                            viewModel.DropEndTime,
                                                            viewModel.DropDate,
                                                            viewModel.DropDate);

                        SendNotification(item.Email, notification);
                    }
                }
            }
        }
    }
}
