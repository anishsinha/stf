using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.FtpReports
{
    static class Program
    {
        static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.DailyReports.Program", "Main", "Start");

            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            TriggeredFtpReports triggeredDailyReport = new TriggeredFtpReports();
            var result = Task.Run(() => triggeredDailyReport.ProcessFtpReports()).Result;

            LogManager.Logger.WriteInfo($"TJob.DailyReports.Program- {result}", "Main", "End");
        }
    }
}
