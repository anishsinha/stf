using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class SalesCalculatorDaily : ISalesCalculatorDaily
    {
        private DateTime _date;
        public SalesCalculatorDaily(DateTime date)
        {
            _date = date;
            Demands = new List<IDemandModel>();
            TankDrops = new List<ITankDropModel>();
        }
        public DateTime Date { get { return _date.Date; } set { _date = value; } }
        public ForecastingSettings ForecastingSettings { get; set; }
        public List<IDemandModel> Demands { get; set; }
        public List<ITankDropModel> TankDrops { get; set; }

        public ISale24HourModel Calculate(SaleTankModel tank)
        {
            Sale24HourModel response = null;
            try
            {
                var startTime = _date;
                var hourlySales = new List<Sale1Hour>();
                for (int index = 0; index < 24; index++)
                {
                    var endTime = startTime.AddHours(1);
                    var oneHourSale = new Sale1Hour(startTime, endTime, Demands, TankDrops);
                    if (oneHourSale.SaleQuantity != 0)
                    {
                        hourlySales.Add(oneHourSale);
                    }
                    startTime = startTime.AddHours(1);
                }
                response = new Sale24HourModel(_date, hourlySales, tank);
            }
            catch
            {
                throw;
            }
            return response;
        }
    }
}
