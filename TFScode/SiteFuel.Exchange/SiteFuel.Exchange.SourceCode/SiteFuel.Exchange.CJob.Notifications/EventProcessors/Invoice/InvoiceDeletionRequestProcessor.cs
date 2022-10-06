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
    public class InvoiceDeletionRequestProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.RequestForInvoiceDeletion;

        public InvoiceDeletionRequestProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetSuperAdminDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
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
                    var superAdmin = viewModel.BuyerUser;
                    var callbackUrl = $"~/SuperAdmin/SuperAdmin/DeleteRequests";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceDeleteRequested_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);
                    notification.BodyText = string.Format(notification.BodyText,
                                                              $"{superAdmin.FirstName} {superAdmin.LastName}",
                                                              $"{viewModel.SupplierUser.FirstName} {viewModel.SupplierUser.LastName}",
                                                              viewModel.InvoiceNumber
                                                            );

                    SendNotification(superAdmin.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceDeletionRequestProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
