using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;


namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class DeliveryCompletedProcessor : BaseDispatchEventProcessor, IEmailProcessor
    {
        private NotificationInvoiceViewModel viewModel;
        public EventType EventType => EventType.DeliveryCompleted;
        private readonly List<string> buyerUsersEmailSent = new List<string>();
        public DeliveryCompletedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceNotificationDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
                var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                var smsText = notification.SmsText;
                foreach (var item in companyAdminList)
                {
                    if (!buyerUsersEmailSent.Contains(item.Email))
                    {
                        notification.SmsText = GetNotificationSmsText(smsText);
                        SendNotification(item.Email, notification);
                        buyerUsersEmailSent.Add(item.Email);
                    }
                }

                if (viewModel.OnsitePersons != null)
                {
                    foreach (var item in viewModel.OnsitePersons)
                    {
                        if (!buyerUsersEmailSent.Contains(item.Email))
                        {
                            notification.SmsText = GetNotificationSmsText(smsText);
                            SendNotification(item.Email, notification);
                            buyerUsersEmailSent.Add(item.Email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryCompletedProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryCompletedProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                    notification.SmsText = GetNotificationSmsText(notification.SmsText);
                    SendNotification(viewModel.BuyerUser.Email, notification);
                    buyerUsersEmailSent.Add(viewModel.BuyerUser.Email);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryCompletedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        //Commented SMS
        private string GetNotificationSmsText(string smsText)
        {
            smsText = string.Format(smsText,
                                    viewModel.DriverName,
                                    viewModel.SupplierCompanyName,
                                    $"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity)} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}",
                                   string.Concat(viewModel.DropAdditionalDetails.Select(t => t.FuelType).ToList(), ", "),
                                    string.Concat(viewModel.DropAdditionalDetails.Select(t => t.PoNumber).ToList(), ", "),
                                    viewModel.JobName);

            return smsText;
        }
    }
}