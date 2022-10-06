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
    public class InvoiceApprovalReminderProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceApprovalReminder;

        public InvoiceApprovalReminderProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Buyer/Invoice/Details?id={viewModel.Id}";
            SendInvoiceApprovalReminderToBuyerCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
            SendInvoiceApprovalReminderToSupplier(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
            SendInvoiceApprovalReminderToSupplierCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0 && viewModel.InvoiceApprover != null)
                {
                    var callbackUrl = $"~/Buyer/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification;
                    if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApprovalReminder_Buyer, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApprovalReminder_Buyer, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    }
                    var quantityDelivered = ($"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity).GetPreciseValue().GetCommaSeperatedValue()} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}").ToString();
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    quantityDelivered,
                                                    viewModel.JobName,
                                                    $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                    viewModel.CreatedOn.DateTime.DayOfWeek,
                                                    viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                    $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                    viewModel.InvoiceNumber);

                    SendNotification(viewModel.BuyerUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceApprovalReminderProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceApprovalReminderToBuyerCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApprovalReminder_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApprovalReminder_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }
                var quantityDelivered = ($"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity).GetPreciseValue().GetCommaSeperatedValue()} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}").ToString();
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                var bodyText = notification.BodyText;

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.BuyerUser.Id && viewModel.InvoiceApprover != null)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                quantityDelivered,
                                                viewModel.JobName,
                                                $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                viewModel.CreatedOn.DateTime.DayOfWeek,
                                                viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}",
                                                viewModel.InvoiceNumber);
                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendInvoiceApprovalReminderToSupplier(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            if (viewModel.InvoiceApprover != null)
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                NotificationViewModel notification;
                if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApprovalReminder_Supplier, _serverUrl, callbackUrl, eventTypeId);
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApprovalReminder_Supplier, _serverUrl, callbackUrl, eventTypeId);
                }
                notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                notification.BodyText = string.Format(notification.BodyText,
                                                viewModel.InvoiceNumber,
                                                $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                viewModel.CreatedOn.DateTime.DayOfWeek,
                                                viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");

                SendNotification(viewModel.SupplierUser.Email, notification);
            }
        }

        private void SendInvoiceApprovalReminderToSupplierCompanyAdmins(NotificationInvoiceViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            if (viewModel.InvoiceApprover != null)
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
                if (companyAdminList.Count > 0)
                {
                    NotificationViewModel notification;
                    if (viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp)
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceApprovalReminder_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DropTicketApprovalReminder_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                    }
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    viewModel.InvoiceNumber,
                                                    $"{viewModel.CreatedOn.DateTime.Hour}:{viewModel.CreatedOn.DateTime.Minute}",
                                                    viewModel.CreatedOn.DateTime.DayOfWeek,
                                                    viewModel.CreatedOn.DateTime.ToShortDateString(),
                                                    $"{viewModel.InvoiceApprover.FirstName} {viewModel.InvoiceApprover.LastName}");

                    foreach (var item in companyAdminList)
                    {
                        //to avoid duplicate emails in case Admin is creator
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            SendNotification(item.Email, notification, true);
                        }
                    }
                }
            }
        }
    }
}
