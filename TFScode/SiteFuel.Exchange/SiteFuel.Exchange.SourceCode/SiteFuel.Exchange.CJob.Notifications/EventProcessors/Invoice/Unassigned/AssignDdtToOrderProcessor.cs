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
    public class AssignDdtToOrderProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.LinkUnassignedDdtToOrder;

        public AssignDdtToOrderProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            if (viewModel.DeliveryInstructionsExists && (viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.None || viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.SendOnlyBillingFile))
            {
                return;
            }
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
            ProcessAssignDdtToOrderNotificationToBuyer(notificationEventViewModel, viewModel);
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
            ProcessAssignDdtToOrderNotificationToSupplier(notificationEventViewModel, viewModel);
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
        }

        private void ProcessAssignDdtToOrderNotificationToBuyer(NotificationEventViewModel notificationEvent, NotificationInvoiceViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Buyer/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification;
                    notification = notificationDomain.GetNotificationContent(EventSubType.LinkUnassignedDdtToOrder_Buyer, _serverUrl, callbackUrl, (int)notificationEvent.EventType);

                    notification.Subject = string.Format(notification.Subject,
                                                    viewModel.InvoiceNumber,
                                                    viewModel.DropAdditionalDetails.Select(t => t.PoNumber).FirstOrDefault());

                    var quantityDelivered = ($"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity).GetPreciseValue().GetCommaSeperatedValue()} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}").ToLower();
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.BuyerUser.FirstName} {viewModel.BuyerUser.LastName}",
                                                    viewModel.DriverName,
                                                    viewModel.SupplierCompanyName,
                                                    quantityDelivered,
                                                    viewModel.DropDate,
                                                    viewModel.DropStartTime,
                                                    viewModel.DropEndTime,
                                                    string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(),", "),
                                                    viewModel.InvoiceNumber);

                    SendNotification(viewModel.BuyerUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssignDdtToOrderProcessor", "ProcessAssignDdtToOrderNotificationToBuyer", "Exception Details : ", ex);
            }
        }

        private void ProcessAssignDdtToOrderNotificationToSupplier(NotificationEventViewModel notificationEvent, NotificationInvoiceViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification;
                    notification = notificationDomain.GetNotificationContent(EventSubType.LinkUnassignedDdtToOrder_Supplier, _serverUrl, callbackUrl, (int)notificationEvent.EventType);

                    notification.Subject = string.Format(notification.Subject,
                                                    viewModel.InvoiceNumber,
                                                    string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(),", "));

                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                    viewModel.UpdatedByUserName,
                                                    viewModel.InvoiceNumber,
                                                    string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "));

                    SendNotification(viewModel.SupplierUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AssignDdtToOrderProcessor", "ProcessAssignDdtToOrderNotificationToSupplier", "Exception Details : ", ex);
            }
        }
    }
}
