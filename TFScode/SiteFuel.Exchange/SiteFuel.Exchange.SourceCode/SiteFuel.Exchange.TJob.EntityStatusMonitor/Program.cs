using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.EntityStatusMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.EntityStatusMonitor.Program", "Main", "Start");

            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            TriggeredEntityStatusMonitor entityStatusMonitor = new TriggeredEntityStatusMonitor();
            var result = Task.Run(() => entityStatusMonitor.ProecssEntityStatusMonitor()).Result;
            LogManager.Logger.WriteInfo($"TJob.EntityStatusMonitor.Program- {result}", "Main", "End");
        }
    }
}
