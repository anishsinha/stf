using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
    public class ForecastingTankDataForChartViewModel
    {
        public List<string> XAxisTimeSpan { get; set; }
        public List<TankDetailsForChartViewModel> TankDetailsForChart { get; set; }
        public List<TankLevelViewModel> TankLevels { get; set; }
    }


    public class TankDetailsForChartViewModel
    {
        public string TankName { get; set; }
        public List<float> Data { get; set; }
        public decimal Retain { get; set; } //Could Go
        public decimal SafteyStock { get; set; } //Should Go
        public decimal RunOutLevel { get; set; } //Must Go
    }

    public class TankLevelViewModel
    {
        public string TankName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public float Quantity { get; set; }
        public int Type { get; set; }
        public DateTime DateTime { get; set; }
    }
}
