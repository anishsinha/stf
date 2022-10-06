using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ForecastingEstimatedUsageModel
    {
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TankName { get; set; }
        public string UsagePeriod { get; set; }
        public string AverageBusinessDayUsage { get; set; }
        public string TotalExceptedUsage { get; set; }
        public string MaximumBusinessdayUsage { get; set; }
        public decimal AverageDayUsage { get; set; }
        public decimal TotalUsage { get; set; }
        public decimal MaximumDayUsage { get; set; }
    }

    public class UspTankEstimatedUsage
    {
        public int DayId { get; set; }
        public string DayName { get; set; }
        public decimal AverageSale { get; set; }
        public int SaleTankId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
    }

    public class TankEstimate
    {
        public decimal Hours { get; set; }
        public decimal Usages { get; set; }
        public decimal CurrentInventory { get; set; }
    }

    public class UspTankUllage
    {
        public float Ullage { get; set; }
        public DateTime CaptureTime { get; set; }
    }
    public class UspRetainWindowInfo
    {
        public string siteId { get; set; }
        public string tankId { get; set; }
        public string storageId { get; set; }
        public int startBuffer { get; set; }
        public int startBufferUOM { get; set; } = 1;
        public int endBuffer { get; set; }
        public int endBufferUOM { get; set; } = 1;
        public int maxBuffer { get; set; } 
        public int maxBufferUOM { get; set; } = 1;
        public decimal Quantity { get; set; }
    }
}
