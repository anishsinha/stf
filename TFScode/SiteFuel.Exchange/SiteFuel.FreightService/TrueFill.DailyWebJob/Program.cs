using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DailyWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = false;
            try
            {
                Console.WriteLine("TrueFill.DailyWebJob-started");
                var forecastingService = new ForecastingService();

                Console.WriteLine("TrueFill.DailyWebJob-ProcessDsbCalendarMissedDrs-started");
                result = Task.Run(() => forecastingService.ProcessCalendarDrs()).Result;
                Console.WriteLine("TrueFill.DailyWebJob-ProcessDsbCalendarMissedDrs-ended");

                Console.WriteLine("TrueFill.DailyWebJob-ProcessMonthlySalesCalculation-started");
                result = Task.Run(() => forecastingService.ProcessMonthlySalesCalculation()).Result;
                Console.WriteLine("TrueFill.DailyWebJob-ProcessMonthlySalesCalculation-ended");

                Console.WriteLine("TrueFill.DailyWebJob-CalculateMonthlySales-started");
                result = Task.Run(() => forecastingService.CalculateMonthlySales()).Result;
                Console.WriteLine("TrueFill.DailyWebJob-CalculateMonthlySales-ended");

                Console.WriteLine("TrueFill.DailyWebJob-CalculateSaleConsumptionRates-started");
                result = Task.Run(() => forecastingService.CalculateSaleConsumptionRates()).Result;
                Console.WriteLine("TrueFill.DailyWebJob-CalculateSaleConsumptionRates-ended");

                Console.WriteLine("TrueFill.DailyWebJob-RebuildReorganizeSalesIndexes-started");
                result = Task.Run(() => forecastingService.RebuildReorganizeSalesIndexes()).Result;
                Console.WriteLine("TrueFill.DailyWebJob-RebuildReorganizeSalesIndexes-ended");

                LogManager.Logger.WriteDebug("Program", "Main", "Status => " + result);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteDebug("Program", "Main", "Status => " + result);
                LogManager.Logger.WriteException("Program", "Main", ex.Message, ex);
            }
        }
    }
}
