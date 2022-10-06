using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.Forecasting
{
    public class TankConsumptionRateModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime BandStartDate { get; set; }
        public TimeSpan BandStartTime { get; set; }
        public DateTime BandEndDate { get; set; }
        public TimeSpan BandEndTime { get; set; }
        public int BandNumber { get; set; }
        public int SaleTankId { get; set; }
        public decimal RetainHours { get; set; }
        public decimal SaftyStockHours { get; set; }
        public decimal RunoutHours { get; set; }
        public decimal RemainingHours { get; set; }
        public int UoM { get; set; }
    }
}
