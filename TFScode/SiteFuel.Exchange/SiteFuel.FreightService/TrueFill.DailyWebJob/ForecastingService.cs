using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DailyWebJob
{
    public class ForecastingService
    {
        //public async Task<bool> ProcessDailySalesCalculation()
        //{
        //    var response = false;
        //    try
        //    {
        //        using (var tracer = new Tracer("ForecastingService", "ProcessDailySalesCalculation"))
        //        {
        //            CultureInfo myCI = new CultureInfo("en-US", false);
        //            myCI.DateTimeFormat.LongDatePattern = Resource.constFormatDate;
        //            myCI.DateTimeFormat.LongTimePattern = Resource.constFormat24HourTime;

        //            var startTime = DateTime.Now.Date.AddDays(-29);
        //            var endDate = DateTime.Now.Date.AddDays(-1);
        //            var drDomain = new ForecastingDomain(new ForecastingRepository());
        //            await drDomain.ProcessMonthlySalesCalculation(startTime, endDate);
        //            response = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Logger.WriteException("ForecastingService", "ProcessDailySalesCalculation", ex.Message, ex);
        //    }
        //    return response;
        //}

        public async Task<bool> ProcessMonthlySalesCalculation()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("ForecastingService", "ProcessMonthlySalesCalculation"))
                {
                    CultureInfo myCI = new CultureInfo("en-US", false);
                    myCI.DateTimeFormat.LongDatePattern = Resource.constFormatDate;
                    myCI.DateTimeFormat.LongTimePattern = Resource.constFormat24HourTime;

                    var startTime = DateTime.Now.Date.AddDays(-29);
                    var endDate = DateTime.Now.Date.AddDays(-1);
                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                    var drDomain = new ForecastingDomain(new ForecastingRepository());
                    await drDomain.ProcessMonthlySalesCalculation(startTime, endDate);
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "ProcessDailySalesCalculation", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> CalculateMonthlySales()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("ForecastingService", "CalculateMonthlySales"))
                {
                    CultureInfo myCI = new CultureInfo("en-US", false);
                    myCI.DateTimeFormat.LongDatePattern = Resource.constFormatDate;
                    myCI.DateTimeFormat.LongTimePattern = Resource.constFormat24HourTime;

                    var startTime = DateTime.Now.Date.AddDays(-29);
                    var endDate = DateTime.Now.Date.AddDays(-1);
                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                    var drDomain = new ForecastingDomain(new ForecastingRepository());
                    await drDomain.CalculateMonthlySales(startTime, endDate, 360);
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "CalculateMonthlySales", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> CalculateSaleConsumptionRates()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("ForecastingService", "CalculateSaleConsumptionRates"))
                {
                    CultureInfo myCI = new CultureInfo("en-US", false);
                    myCI.DateTimeFormat.LongDatePattern = Resource.constFormatDate;
                    myCI.DateTimeFormat.LongTimePattern = Resource.constFormat24HourTime;

                    var startTime = DateTime.Now.Date.AddDays(-29);
                    var endDate = DateTime.Now.Date.AddDays(-1);
                    endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
                    var drDomain = new ForecastingDomain(new ForecastingRepository());
                    await drDomain.CalculateSaleConsumptionRates(startTime, endDate, 240);
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "CalculateSaleConsumptionRates", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> RebuildReorganizeSalesIndexes()
        {
            var response = false;
            try
            {
                using (var tracer = new Tracer("ForecastingService", "RebuildReorganizeSalesIndexes"))
                {
                    var drDomain = new ForecastingDomain(new ForecastingRepository());
                    await drDomain.RebuildReorganizeSalesIndexes(120);
                    response = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "RebuildReorganizeSalesIndexes", ex.Message, ex);
            }
            return response;
        }
        public async Task<bool> ProcessCalendarDrs()
        {
            var response = false;
            try
            {
                var drDomain = new DeliveryRequestDomain(new DeliveryRequestRepository());
                await drDomain.MoveCalendarDeliveryRequest();
                response = true;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ForecastingService", "ProcessCalendarDrs", ex.Message, ex);
            }
            return response;
        }
    }
}
