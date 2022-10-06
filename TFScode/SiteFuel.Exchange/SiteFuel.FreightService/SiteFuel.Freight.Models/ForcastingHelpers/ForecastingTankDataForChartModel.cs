using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ForecastingTankDataForChartModel
    {
        public List<string> XAxisTimeSpan { get; set; }
        public List<TankDetailsForChartModel> TankDetailsForChart { get; set; }
        public List<TankLevelModel> TankLevels { get; set; }
    }

    public class TankDetailsForChartModel
    {
        public string TankName { get; set; }
        public List<decimal> Data { get; set; } = new List<decimal>();
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public decimal Retain { get; set; } //Could Go
        public decimal SafetyStock { get; set; } //Should Go
        public decimal RunOutLevel { get; set; } //Must Go
    }

    public class TankLevelModel
    {
        public string TankName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public decimal Quantity { get; set; }
        public int Type { get; set; }
        public DateTime DateTime { get; set; }
        public double Hours { get; set; }
    }
}
