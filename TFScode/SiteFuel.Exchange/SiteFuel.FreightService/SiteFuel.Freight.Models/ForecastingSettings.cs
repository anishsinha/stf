using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ForecastingSettings
    {
        public bool IsEnabled { get; set; } = false;
        public int Id { get; set; }
        public int BandPeriod { get; set; }
        public string StartTime { get; set; }
        public decimal MinimumLoad { get; set; }
        public decimal AverageLoad { get; set; }
        public int ForcastingType { get; set; }
        public int InventoryUOM { get; set; }
        public int Retain { get; set; }
        public int SafetyStock { get; set; }
        public int RunoutLevel { get; set; }
        public bool IsAutoDRCreation { get; set; }
        public int StartBuffer { get; set; }
        public int EndBuffer { get; set; }
        public int RetainTimeBuffer { get; set; }
        public int LeadTime { get; set; }
    }
}
