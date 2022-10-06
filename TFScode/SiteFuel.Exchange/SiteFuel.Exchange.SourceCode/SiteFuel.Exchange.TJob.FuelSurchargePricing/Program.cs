using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Logger;
using System;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.FuelSurchargePricing
{
    class Program
    {
        public static void Main(string[] args)
        {
            LogManager.Logger.WriteInfo("TJob.FuelSurchargePricing.Program", "Main", "Start");

            ConfigurationPrinter printer = new ConfigurationPrinter();
            printer.PrintConnectionStrings();
            printer.PrintAppSettings();
            Console.WriteLine("Completed Printing appsettings");

            EIAPriceUpdates eaiPriceUpdates = new EIAPriceUpdates();
            var result = Task.Run(() => eaiPriceUpdates.GetEaiPriceUpdates()).Result;

            LogManager.Logger.WriteInfo($"TJob.FuelSurchargePricing.Program- {result}", "Main", "End");
        }
    }
}
