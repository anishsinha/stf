using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.BuyerDipTest
{
    public class BuyerDipTestViewModel
    {
        public int AssetId { get; set; }
        public string TankId { get; set; }
        public Nullable<TankType> TankType { get; set; }
        public string TankNumber { get; set; }
        public Nullable<DipTestMethod> DipTestMethod { get; set; }
        public Nullable<decimal> FuelCapacity { get; set; }
        public string Manufacturer { get; set; }
        public Nullable<decimal> ThresholdDeliveryRequest { get; set; }
        public Nullable<decimal> MinFill { get; set; }
        public Nullable<FillType> FillType { get; set; }
        public Nullable<decimal> MaxFill { get; set; }
        public Nullable<decimal> MaxFillPercent { get; set; }
        public Nullable<decimal> MinFillPercent { get; set; }
        public Nullable<decimal> PhysicalPumpStop { get; set; }
        public Nullable<decimal> RunOutLevel { get; set; }
        public Nullable<decimal> NotificationUponUsageSwing { get; set; }
        public Nullable<decimal> NotificationUponUsageSwingValue { get; set; }
        public Nullable<decimal> NotificationUponInventorySwing { get; set; }
        public Nullable<decimal> NotificationUponInventorySwingValue { get; set; }
        public Nullable<ManiFolded> ManiFolded { get; set; }
        public Nullable<TankConstruction> TankConstruction { get; set; }
        public string TankAcceptDelivery { get; set; }
        public string TankName { get; set; }
        public string CurrentReading { get; set; }
        //
        public DemandModel Demand { get; set; }
        public long Id { get; set; }
        public string SiteId { get; set; }
        public string StorageId { get; set; }
        public decimal Level { get; set; }
        public float Ullage { get; set; }
        public float GrossVolume { get; set; }
        public float NetVolume { get; set; }
        public float WaterNetLevel { get; set; }
        public float WaterGrossLevel { get; set; }
        public DateTime CaptureTime { get; set; }
        public string DisplayCaptureTime { get; set; }
        public string ProductName { get; set; }
        public int DataSourceTypeId { get; set; }
        public bool IsDRExists { get; set; }
    }
}
