using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DailyReports
{
    public class TriggeredDailyReports
    {
        public TriggeredDailyReports()
        {
            using (var tracer = new Tracer("TriggeredDailyReports", "TriggeredDailyReports"))
            {
                //Register Context
                ContextFactory.Register(new ApplicationContext());
            }
        }

        public async Task<bool> ProcessDailyReports()
        {
            using (var tracer = new Tracer("TriggeredDailyReports", "ProcessDailyReports"))
            {
                try
                {
                    //Start your sending progress report logic here
                    await StartSendingProgressReport();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDailyReports", "ProcessDailyReports", "Exception Details : ", ex);
                }
                return true;
            }
        }

        public async Task<bool> ProcessDailyCarrierDeliveryReports()
        {
            using (var tracer = new Tracer("TriggeredDailyReports", "ProcessDailyCarrierDeliveryReports"))
            {
                try
                {
                    //Start your Adding Delivery report logic here
                    await AddCarrierDeliveiesDetails();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDailyReports", "ProcessDailyCarrierDeliveryReports", "Exception Details : ", ex);
                }
                return true;
            }
        }

        private async Task StartSendingProgressReport()
        {
            using (var tracer = new Tracer("TriggeredDailyReports", "StartSendingProgressReport"))
            {
                try
                {
                    ProgressReportFilter filter = new ProgressReportFilter();
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                    DateTime previousDate = DateTime.Now.AddDays(-1);
                    DateTime startDate = new DateTime(previousDate.Year, previousDate.Month, previousDate.Day, 17, 0, 0);
                    DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 17, 0, 0);

                    DateTime monthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime monthEndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month), 23, 59, 59);

                    filter.StartDate = new DateTimeOffset(startDate, timeZoneInfo.GetUtcOffset(startDate));
                    filter.EndDate = new DateTimeOffset(endDate, timeZoneInfo.GetUtcOffset(endDate));
                    filter.MonthStartDate = new DateTimeOffset(monthStartDate, timeZoneInfo.GetUtcOffset(monthStartDate));
                    filter.MonthEndDate = new DateTimeOffset(monthEndDate, timeZoneInfo.GetUtcOffset(monthEndDate));
                    filter.AccountOwnerId = 0;

                    bool response = false;
                    var json = ContextFactory.Current.GetDomain<ApplicationDomain>().GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingProgressReportMailingList);

                    var mailingList = JsonConvert.DeserializeObject<MailingList>(json);

                    if (mailingList != null && mailingList.ProgressReportMailingList != null)
                    {
                        foreach (var mialings in mailingList.ProgressReportMailingList)
                        {
                            filter.CountryId = (int)(Country)Enum.Parse(typeof(Country), mialings.Country);
                            response = await new SuperAdminDomain().GetProgressReport(filter, false, mialings.Emails);
                        }
                    }

                    LogManager.Logger.WriteDebug("TriggeredDailyReports", "StartSendingProgressReport", $"Daily Report from {filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz")} to {filter.EndDate.ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz")}");
                    if (!response)
                    {
                        LogManager.Logger.WriteTrace("TriggeredDailyReports", "StartSendingProgressReport", Resource.msgProgressReportFailed);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TriggeredDailyReports", "StartSendingProgressReport", "Exception Details : ", ex);
                }
            }
        }

        private async Task AddCarrierDeliveiesDetails()
        {
            using (var tracer = new Tracer("TriggeredDailyReports", "AddCarrierDeliveiesDetails"))
            {
                try
                {

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
}

