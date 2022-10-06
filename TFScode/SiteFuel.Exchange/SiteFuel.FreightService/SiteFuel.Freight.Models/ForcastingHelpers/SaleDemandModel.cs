using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class SaleDemandModel : IDemandModel
    {
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public decimal Level { get; set; }
        public float Ullage { get; set; }
        public float GrossVolume { get; set; }
        public float NetVolume { get; set; }
        public DateTime CaptureTime { get; set; }
        public float DipTestValue { get; set; }
        public DipTestUoM DipTestUoM { get; set; }
        public string TimeZoneName { get; set; }
        public int BandPeriod { get; set; }
        public TimeSpan StartTime { get; set; }
        public decimal? MaxFill { get; set; }
        public int? FillType { get; set; }
        public decimal? FuelCapacity { get; set; }
        public int? Retain { get; set; }
        public int? SaftyStock { get; set; }
        public int? Runout { get; set; }
        public int? InventoryUoM { get; set; }
    }
}
