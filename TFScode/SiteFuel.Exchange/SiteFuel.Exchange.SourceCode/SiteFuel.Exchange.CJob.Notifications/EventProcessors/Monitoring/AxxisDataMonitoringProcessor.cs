using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors.Monitoring
{
    public class AxxisDataMonitoringProcessor : BaseEventProcessor, IEmailProcessor
    {
        private NotificationViewModel viewModel;

        public EventType EventType => EventType.EmailToMonitorAxxisDataUpdates;

        public AxxisDataMonitoringProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            viewModel = new NotificationViewModel();
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
        }

        public override List<NotificationUserViewModel> LoadSupplierDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
        {
            return new List<NotificationUserViewModel>();
        }

        public override List<NotificationUserViewModel> LoadBuyerDefaultEmailReceivers(NotificationEventViewModel notificationEventViewModel)
        {
            return new List<NotificationUserViewModel>();
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                var emailList = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAxxisMonitoringEmailList);
                viewModel.Subject = "Axxis price updates failed";
                viewModel.BodyText = "Axxis price sync not done on " + DateTime.Now.Date.ToShortDateString();
                viewModel.ShowHelpLineInfo = false;
                SendNotificationForDefaultEvent(emailList, viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AxxisDataMonitoringProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
