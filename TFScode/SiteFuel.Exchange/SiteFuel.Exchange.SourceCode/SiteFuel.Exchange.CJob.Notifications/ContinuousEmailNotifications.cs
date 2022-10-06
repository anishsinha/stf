using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications
{
    public class ContinuousEmailNotifications
    {
        public ContinuousEmailNotifications()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> ProcessEmailNotifications()
        {
            var watch = Stopwatch.StartNew();
            try
            {
                //Start your email notfication logic here
                StartProcessingNotificationEvents();
                //await StartProcessingTimeCardEntries();
                await StartProcessingFuelRequestStatus();
                await StartProcessingQuoteRequestStatus();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "ProcessEmailNotifications", "Exception Details : ", ex);
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.Notifications", "ProcessEmailNotifications", "End:TotalTime:" + watch.ElapsedMilliseconds);
            return true;
        }

        private void StartProcessingNotificationEvents()
        {
            try
            {
                var pendingNotifications = ContextFactory.Current.GetDomain<NotificationDomain>().GetPendingNotificationEvents();
                if (pendingNotifications.Any())
                {
                    var eventProcessor = new ContinuousEventProcessor();
                    foreach (var notificationEvent in pendingNotifications)
                    {
                        eventProcessor.ProcessEvent(notificationEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "StartProcessingNotificationEvents", "Exception Details : ", ex);
            }
        }

        private async Task StartProcessingFuelRequestStatus()
        {
            var watch = Stopwatch.StartNew();
            try
            {
                var FRExpirationReminderTime = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingFRExpirationReminderTime);
                var fuelRequests = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetOpenFuelRequestsWithExpirationDateAsync();
                foreach (var fr in fuelRequests)
                {
                    await ProcessFuelRequestStatus(fr, FRExpirationReminderTime);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "StartProcessingFuelRequestStatus", "Exception Details : ", ex);
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.Notifications", "StartProcessingFuelRequestStatus", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }

        private async Task StartProcessingQuoteRequestStatus()
        {
            try
            {
                var qrExpirationReminderTime = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingFRExpirationReminderTime);
                var quoteRequests = await ContextFactory.Current.GetDomain<QuoteRequestDomain>().GetOpenQuoteRequestsAsync();
                foreach (var qr in quoteRequests)
                {
                    await ProcessQuoteRequestStatus(qr, qrExpirationReminderTime);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "StartProcessingQuoteRequestStatus", "Exception Details : ", ex);
            }
        }

        private async Task StartProcessingTimeCardEntries()
        {
            try
            {
                var timeCardEntrieIds = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardEntriesAsync();
                foreach (var timecardEntry in timeCardEntrieIds)
                {
                    await ProcessTimeCardEntries(timecardEntry);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "StartProcessingTimeCardEntries", "Exception Details : ", ex);
            }
        }

        private async Task ProcessTimeCardEntries(int id)
        {
            try
            {
                await ContextFactory.Current.GetDomain<TimeCardDomain>().UpdateUserLocationAsync(id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "ProcessTimeCardEntries", "Exception Details : ", ex);
            }
        }

        private async Task ProcessFuelRequestStatus(int id, int FRExpirationReminderTime)
        {
            try
            {
                await ContextFactory.Current.GetDomain<FuelRequestDomain>().ProcessFuelRequestAsync(id, FRExpirationReminderTime);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "ProcessFuelRequestStatus", "Exception Details : ", ex);
            }
        }

        private async Task ProcessQuoteRequestStatus(int id, int qrExpirationReminderTime)
        {
            try
            {
                await ContextFactory.Current.GetDomain<QuoteRequestDomain>().ProcessQuoteRequestAsync(id, qrExpirationReminderTime);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ContinuousEmailNotifications", "ProcessQuoteRequestStatus", "Exception Details : ", ex);
            }
        }
    }
}
