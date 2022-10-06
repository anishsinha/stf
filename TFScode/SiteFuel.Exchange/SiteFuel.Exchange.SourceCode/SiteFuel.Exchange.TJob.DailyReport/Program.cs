using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DailyReports
{
    class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.DailyReports.Program", "Main", "Start");

            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            TriggeredDailyReports triggeredDailyReport = new TriggeredDailyReports();
            var result = Task.Run(() => triggeredDailyReport.ProcessDailyReports()).Result;

            TankRentalInvoices tankRentalInvoices = new TankRentalInvoices();
            var invoiceResult = Task.Run(() => tankRentalInvoices.ProcessTankRentalScheduleInvoices()).Result;

            ExceptionAutoApproval exceptionAutoApproval = new ExceptionAutoApproval();
            var exceptionResult = Task.Run(() => exceptionAutoApproval.ProcessExceptionsForAutoApproval()).Result;
            var apiExceptionResult = Task.Run(() => exceptionAutoApproval.ProcessExceptionsForAutoReject()).Result;


            Domain.LFVDomain lFVDomain = new Domain.LFVDomain();
            var badgeResult = Task.Run(() => lFVDomain.GetBadgelistFromParkLandBadgeManagermentApi()).Result;

            Domain.FuelRequestDomain frDomain = new Domain.FuelRequestDomain();
            var isProcessed = Task.Run(() => frDomain.ResetCommulation()).Result;
            LogManager.Logger.WriteInfo($"TJob.DailyReports.Program- {result}", "Main", "End");

            //below lines of code is for daily carrier Delivery report
            LogManager.Logger.WriteInfo($"TJob.DailyReports.AddNotificationEventAsync(CarrierDeliveries)", "Main", "Start");
            ContextFactory.Register(new ApplicationContext());
            NotificationDomain notificationDomain = new NotificationDomain();
            var cReportResult = Task.Run(() => notificationDomain.AddNotificationEventAsync(EventType.CarrierDeliveries, 0, 1, null, null, 4)).Result;
            LogManager.Logger.WriteInfo($"TJob.DailyReports.AddNotificationEventAsync(CarrierDeliveries)", "Main", "End");

        }
    }
}
