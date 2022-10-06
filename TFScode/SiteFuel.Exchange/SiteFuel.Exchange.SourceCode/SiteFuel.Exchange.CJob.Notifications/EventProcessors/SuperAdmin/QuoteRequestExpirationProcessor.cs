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
    public class QuoteRequestExpirationProcessor : BaseSuperAdminEventProcessor, IEmailProcessor
    {
        private NotificationQuoteViewModel viewModel;

        public EventType EventType => EventType.QuoteRequestExpired;

        public QuoteRequestExpirationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetQuoteNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);
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
                var superAdminEmailList = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingNewIncomingFRMailingList);
                if (viewModel.QuoteId > 0)
                {
                    var callbackUrl = string.Empty;
                    var notification = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Supplier, _serverUrl, callbackUrl);
                    notification.BodyText = string.Format(notification.BodyText, viewModel.BuyerCompany, viewModel.BuyerQuoteNumber, viewModel.EndDate, viewModel.QuotesNeeded, viewModel.QuotesReceived);
                    notification.ShowHelpLineInfo = false;
                    SendNotificationForDefaultEvent(superAdminEmailList, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuoteRequestExpirationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
