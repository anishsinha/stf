using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class TankHitRunoutLevelProcessor : BaseOttoEventProcessor, IEmailProcessor
    {
        private NotificationTankHitRunoutLevelViewModel viewModel;

        public EventType EventType => EventType.TankHitRunOutLevel;

        public TankHitRunoutLevelProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = JsonConvert.DeserializeObject<NotificationTankHitRunoutLevelViewModel>(notificationEventViewModel.JsonMessage);
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
                var callbackUrl = string.Empty;
                var notification = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Supplier, _serverUrl, callbackUrl);
                var subjectText = notification.Subject;
                var bodyText = notification.BodyText;
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
                if (companyAdminList.Count > 0)
                {
                    foreach (var item in companyAdminList)
                    {
                        notification.Subject = string.Format(subjectText, viewModel.Tank, viewModel.Location);
                        notification.BodyText = string.Format(bodyText,
                                        item.FirstName, viewModel.Tank, viewModel.Location, viewModel.TankStorageId, viewModel.CurrentInventory,
                                        viewModel.Ullage);//, viewModel.DaysRemaining);

                        SendNotification(item.Email, notification);
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankHitRunoutLevelProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
