using S22.Imap;
using SiteFuel.BAL;
using SiteFuel.FreightModels;
using SiteFuel.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;

namespace TrueFill.HourlyWebJob
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = false;
            try
            {
                LogManager.Logger.WriteDebug("Program", "Main", "Start");
                var demandService = new DemandCaptureService();
                LogManager.Logger.WriteDebug("Program", "Main", "Process Data start");
                result = Task.Run(() => demandService.ProcessData()).Result;
                LogManager.Logger.WriteDebug("Program", "Main", "Process ProcessDeliveryRequest start");
                var deliveryRequestService = new DeliveryRequestService();
                result = Task.Run(() => deliveryRequestService.ProcessDeliveryRequest()).Result;

                LogManager.Logger.WriteDebug("Program", "Main", "Process ProcessOttoDeliveryRequest start");
                var Ottoresult = Task.Run(() => deliveryRequestService.ProcessOttoDeliveryRequest()).Result;
                LogManager.Logger.WriteDebug("Program", "Main", "Ottoresult => " + Ottoresult);

                LogManager.Logger.WriteDebug("Program", "Main", "Process ProcessVedorRootData start");
                var ProcessVedorRootData = Task.Run(() => demandService.ProcessVedorRootData()).Result;
                LogManager.Logger.WriteDebug("Program", "Main", "ProcessVedorRootData ");

                LogManager.Logger.WriteDebug("Program", "Main", "Process ProcessDailySalesCalculation start");
                var forecastingService = new ForecastingService();
                var response = Task.Run(() => forecastingService.ProcessDailySalesCalculation()).Result;
                LogManager.Logger.WriteDebug("Program", "Main", "Process ProcessDailySalesCalculation end");                

                LogManager.Logger.WriteDebug("Program", "Main", "Process ProcessForecastingTankCaculation start");
                var forecastingTankCaculation = Task.Run(() => forecastingService.ProcessForecastingTankCaculation()).Result;
                LogManager.Logger.WriteDebug("Program", "Main", "ProcessForecastingTankCaculation => " + forecastingTankCaculation);

                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteDebug("Program", "Main", "Status => " + result);
                LogManager.Logger.WriteException("Program", "Main", ex.Message, ex);
            }
        }
    }
}
