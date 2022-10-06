using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public interface IDemandModel
    {
        string SiteId { get; set; }
        string TankId { get; set; }
        string StorageId { get; set; }
        decimal Level { get; set; }
        float Ullage { get; set; }
        float GrossVolume { get; set; }
        float NetVolume { get; set; }
        DateTime CaptureTime { get; set; }
        float DipTestValue { get; set; }
        DipTestUoM DipTestUoM { get; set; }
        int BandPeriod { get; set; }
        TimeSpan StartTime { get; set; }
        decimal? MaxFill { get; set; }
        int? FillType { get; set; }
        decimal? FuelCapacity { get; set; }
        int? Retain { get; set; }
        int? SaftyStock { get; set; }
        int? Runout { get; set; }
        int? InventoryUoM { get; set; }
    }
}
