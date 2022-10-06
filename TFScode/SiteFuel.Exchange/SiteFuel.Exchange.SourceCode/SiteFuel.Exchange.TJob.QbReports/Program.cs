using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.QbReports
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.QbReports.Program", "Main", "Start");

            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            TriggeredQbReports triggeredQbReports = new TriggeredQbReports();
            var result = Task.Run(() => triggeredQbReports.ProcessQbReports()).Result;

            Domain.QbDomain qbDomain = new Domain.QbDomain();
            var isProcessed = Task.Run(() => qbDomain.ProcessQbReceivePaymentRequestWorkflow()).Result;

            LogManager.Logger.WriteInfo($"TJob.QbReports.Program- {result}", "Main", "End");
        }
    }
}
