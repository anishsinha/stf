using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DeliveryAlerts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.DeliveryAlerts.Program", "Main", "Start");

            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            Console.WriteLine("Start ProcessrecurringscheduleAsync Function");
            TriggeredRecurringSchedule triggeredRecurringSchedule = new TriggeredRecurringSchedule();
            var SCresult = Task.Run(() => triggeredRecurringSchedule.ProcessrecurringscheduleAsync()).Result;
            Console.WriteLine("Completed ProcessrecurringscheduleAsync Function");
            LogManager.Logger.WriteInfo($"TJob.DeliveryAlerts-ProcessrecurringscheduleAsync.Program- {SCresult}", "Main", "End");

            Console.WriteLine("Start ProcessDeliveryAlters Function");
            TriggeredDeliveryAlerts triggeredDelivery = new TriggeredDeliveryAlerts();
            var result = Task.Run(() => triggeredDelivery.ProcessDeliveryAlters()).Result;
            Console.WriteLine("Completed ProcessDeliveryAlters Function");
            LogManager.Logger.WriteInfo($"TJob.DeliveryAlerts.Program- {result}", "Main", "End");

            
        }
    }
}
