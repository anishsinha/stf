using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.ExternalPricingData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int sleepTime = 200;
            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            SyncExternalPricingData dbSync = new SyncExternalPricingData();
            var insertedRecords = Task.Run(() => dbSync.SyncExternalPriceData()).Result;
            Console.WriteLine($"Pricing data records inserted: {insertedRecords}");

            Thread.Sleep(sleepTime);
            ExchangeService exchangeService = new ExchangeService();
            var result = Task.Run(() => exchangeService.UpdateDailyExchangeRate()).Result;
            Console.WriteLine($"Update completed for CurrencyLayer ExchangeService with {result}");

            Thread.Sleep(sleepTime);
            // process notifications for ddt which are pending to convert to invoices  
            var notificationResponse = Task.Run(() => exchangeService.ProcessDdtPendingToInvoiceNotifications()).Result;
            Console.WriteLine($"Process ddt pending to invoice notifications completed with result {notificationResponse}");

            Thread.Sleep(sleepTime);
            // process notifications for ddt which are pending to convert to invoices  
            var filldResponse = Task.Run(() => exchangeService.ProcessDdtPendingForFilldResponse()).Result;
            Console.WriteLine($"Process ddt pending to filld response completed with result {filldResponse}");

            Thread.Sleep(sleepTime);
            // process unknown delivery exceptions 
            var unknownDeliveryResponse = Task.Run(() => exchangeService.ProcessUnknownDeliveryExceptionManagement()).Result;
            Console.WriteLine($"Process unknown delivery exception completed with result {unknownDeliveryResponse}");

            Thread.Sleep(sleepTime);
            // process missing delivery exceptions 
            var missingDeliveryResponse = Task.Run(() => exchangeService.ProcessMissingDeliveryExceptionManagement()).Result;
            Console.WriteLine($"Process missing delivery exception completed with result {missingDeliveryResponse}");

            Thread.Sleep(sleepTime);
            
            // lift file report generation
            var liftFilereportResponse = Task.Run(() => exchangeService.TriggerLiftFileRecordsDailyReportCreation()).Result;
            Console.WriteLine($"Trigger LiftFileRecordReportGeneration success : {liftFilereportResponse}");


            Thread.Sleep(sleepTime);
            // DR consolidation
            var groupDrConsolidation = Task.Run(() => exchangeService.TriggerGroupDrConsolidationProcess()).Result;
            Console.WriteLine($"Trigger GroupDrConsolidationProcess success : {groupDrConsolidation}");

            //Thread.Sleep(sleepTime);
            //var mfnConsolidation = Task.Run(() => exchangeService.ProcessConsolidatedDdtCreation()).Result;
            //Console.WriteLine($"Trigger ProcessConsolidatedDdtCreation success : {mfnConsolidation}");
            
            Thread.Sleep(sleepTime);
            // Process Pedigree 
            var pedigreeResponse = Task.Run(() => exchangeService.ProcessPedigreeData()).Result;
            Console.WriteLine($"Process Process Pedigree Data : {pedigreeResponse}");

            Thread.Sleep(sleepTime);
            // Process Skybitz 
            var SkybitzResponse = Task.Run(() => exchangeService.ProcessSkybitzData()).Result;
            Console.WriteLine($"Process Process Pedigree Data : {SkybitzResponse}");
            Thread.Sleep(sleepTime);


            // delivery details daily dump  report generation
            var response = Task.Run(() => exchangeService.TriggerDailyDeliveryDataDumpReportCreation()).Result;
            Console.WriteLine($"Trigger DailyDeliveryDataDumpReportCreation success : {response}");
            Thread.Sleep(sleepTime);

        }
    }
}
