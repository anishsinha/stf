using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Tank
{
    public class TankDetailsModel
    {
        public int AssetId { get; set; }

        public string TankId { get; set; }

        public string StorageId { get; set; }

        public string TankName { get; set; }

        public string TankNumber { get; set; }

        public string Manufacturer { get; set; }

        public decimal? FuelCapacity { get; set; }

        public decimal? ThresholdDeliveryRequest { get; set; }

        public decimal? MinFill { get; set; }

        public FillType? FillType { get; set; }

        public decimal? MaxFill { get; set; }

        public decimal? MaxFillPercent { get; set; }

        public decimal? MinFillPercent { get; set; }

        public decimal? PhysicalPumpStop { get; set; }

        public decimal? RunOutLevel { get; set; }

        public decimal? NotificationUponUsageSwing { get; set; }

        public decimal? NotificationUponUsageSwingValue { get; set; }

        public decimal? NotificationUponInventorySwing { get; set; }

        public decimal? NotificationUponInventorySwingValue { get; set; }

        public TankType? TankType { get; set; }

        public string TankModelTypeId { get; set; }

        public DipTestMethod? DipTestMethod { get; set; }

        public ManiFolded? ManiFolded { get; set; }

        public TankConstruction? TankConstruction { get; set; }

        public string TankAcceptDelivery { get; set; }

        public int FuelTypeId { get; set; }

        public string ProductTypeName { get; set; }

        public int JobId { get; set; }

        public string JobDisplayId { get; set; }

        public string JobName { get; set; }

        public float LastReading { get; set; }
        public DateTime CaptureTime { get; set; }

        public List<JobDipChartDetails> dipChartDetails { get; set; } = new List<JobDipChartDetails>();
        public string TankChartPath { get; set; }
    }

    public class JobDipChartDetails
    {
        public string TankId { get; set; }
        public string SiteId { get; set; }
        public float Ullage { get; set; }
        public float GrossVolume { get; set; }
        public float NetVolume { get; set; }
        public DateTime CaptureTime { get; set; }
        public string CaptureTimeString { get; set; }
    }
}
