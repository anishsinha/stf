using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class SalesCalculatorMonthly : ISalesCalculatorMonthly
    {
        private DateTime _date;
        public SalesCalculatorMonthly(DateTime date)
        {
            _date = date;
            Demands = new List<IDemandModel>();
            TankDrops = new List<ITankDropModel>();
        }
        public DateTime Date { get { return _date.Date; } set { _date = value; } }
        public ForecastingSettings ForecastingSettings { get; set; }
        public List<IDemandModel> Demands { get; set; }
        public List<ITankDropModel> TankDrops { get; set; }

        public List<ISale24HourModel> Calculate(SaleTankModel tank)
        {
            var response = new List<ISale24HourModel>();
            DateTime startDate = _date.Date, endDate = startDate.AddDays(1);
            var salesCalculatorDaily = new SalesCalculatorDaily(startDate);
            salesCalculatorDaily.ForecastingSettings = ForecastingSettings;
            try
            {
                for (int index = 0; index < 28; index++)
                {
                    salesCalculatorDaily.Date = startDate;
                    salesCalculatorDaily.Demands = Demands.Where(t => t.CaptureTime >= startDate && t.CaptureTime < endDate).OrderBy(t => t.CaptureTime).ToList();
                    salesCalculatorDaily.TankDrops = TankDrops.Where(t => t.EndTime >= startDate && t.EndTime < endDate).OrderBy(t => t.EndTime).ToList();
                    var prevDayLastDemand = Demands.Where(t => t.CaptureTime >= startDate.AddHours(-1) && t.CaptureTime < endDate).OrderBy(t => t.CaptureTime).FirstOrDefault();
                    if (prevDayLastDemand != null)
                    {
                        salesCalculatorDaily.Demands.Insert(0, prevDayLastDemand);
                    }
                    var sale24Hours = salesCalculatorDaily.Calculate(tank);
                    if (sale24Hours != null && salesCalculatorDaily.Demands.Any())
                    {
                        response.Add(sale24Hours);
                    }
                    startDate = startDate.AddDays(1);
                    endDate = endDate.AddDays(1);
                }
            }
            catch
            {
                throw;
            }
            return response;
        }
    }
}
