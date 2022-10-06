using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class QuickBooksSyncReportProcessor : BaseQuickBooksEventProcessor, IEmailProcessor
    {
        private NotificationQbReportViewModel viewModel;

        public EventType EventType => EventType.QuickBooksSyncReport;

        public QuickBooksSyncReportProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            viewModel = Task.Run(() => notificationDomain.GetQuickBooksSyncReportNotificationDetail(notificationEventViewModel.EntityId, notificationEventViewModel.CreatedDate.Value)).Result;
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            // There are no default email to send. Only subscribed user will get this email. 
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            // There are no default email to send. Only subscribed user will get this email. 
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.QbReport != null && viewModel.SubscribedUsers.Any())
                {
                    var qbReportDomain = new QbReportDomain(notificationDomain);
                    var res = qbReportDomain.SendQbSyncReportEmailAsync(viewModel.QbReport, viewModel.SubscribedUsers).Result;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("QuickBooksSyncReportProcessor", "SendEmail", ex.Message, ex);
            }
        }
    }
}
