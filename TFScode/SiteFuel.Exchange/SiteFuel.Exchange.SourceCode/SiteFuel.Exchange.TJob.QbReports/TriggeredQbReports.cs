using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.QbReports
{
    public class TriggeredQbReports
    {
        public TriggeredQbReports()
        {
            //Register Context
            ContextFactory.Register(new ApplicationContext());
        }

        public async Task<bool> ProcessQbReports()
        {
            var response = false;
            var currentDateTime = DateTimeOffset.Now;
            var notificationDomain = new NotificationDomain();
            var companies = Task.Run(() => notificationDomain.GetQbConfiguredCompanies()).Result;
            foreach (var company in companies)
            {
                var isTimeMatch = notificationDomain.IsQbReportTimeMatchToTimeZone(currentDateTime, company.SyncReportTime, company.ReportTimeZone);
                if (isTimeMatch)
                {
                    await notificationDomain.AddNotificationEventAsync(EventType.QuickBooksSyncReport, company.Id, (int)SystemUser.System);
                    response = true;
                }
            }
            return response;
        }
    }
}
