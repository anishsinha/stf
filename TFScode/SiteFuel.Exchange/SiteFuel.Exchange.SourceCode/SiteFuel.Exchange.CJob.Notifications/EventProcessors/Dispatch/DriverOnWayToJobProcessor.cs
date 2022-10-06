using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class DriverOnWayToJobProcessor : BaseDispatchEventProcessor, IEmailProcessor
    {
        private NotificationDispatchViewModel viewModel;
        public EventType EventType => EventType.DriverOnWayToJob;
        private readonly List<string> buyerUsersEmailSent = new List<string>();
        public DriverOnWayToJobProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetDispatchNotificationDetails(notificationEventViewModel.EntityId);
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
                LogManager.Logger.WriteException("DriverOnWayToJobProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                LogManager.Logger.WriteException("DriverOnWayToJobProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
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
                LogManager.Logger.WriteException("DriverOnWayToJobProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationSmsText(string smsText)
        {
            smsText = string.Format(smsText,
                                    $"{viewModel.UserFirstName} {viewModel.UserLastName}",
                                    viewModel.SupplierCompanyName,
                                    viewModel.PoNumber,
                                    viewModel.JobName);

            return smsText;
        }
    }
}