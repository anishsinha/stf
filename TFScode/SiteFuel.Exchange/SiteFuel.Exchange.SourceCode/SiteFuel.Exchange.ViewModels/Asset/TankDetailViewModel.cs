using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class TankDetailViewModel : BaseViewModel
    {
        public int AssetId { get; set; }

        public int JobId { get; set; }

        public string TankId { get; set; }

        public string StorageId { get; set; }

        public float LastReading { get; set; }

        public DateTime CaptureTime { get; set; }

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

        public int FuelTypeId { get; set; }

        public string JobDisplayId { get; set; }

        public string TankModelTypeId { get; set; }
        
        public string TankMakeModel { get; set; }

        public string TankChart { get; set; }

        public string TankChartFileName { get; set; }

        public string TankMake { get; set; }

        public string TankModel { get; set; }

        public int ScaleMeasurement { get; set; }
        public string ProductTypeName { get; set; }
        public UoM UoM { get; set; }
        public float Ullage { get; set; }
        public int Priority { get; set; }
        public float GrossVolume { get; set; }
        public float NetVolume { get; set; }
        public float DipTestValue { get; set; }
        public TankScaleMeasurement DipTestUoM { get; set; }
        public List<int> TanksConnected { get; set;}
        public Nullable<int> TankSequence { get; set; }
        public string PedigreeAssetDBID { get; set; }
        public int TfxProductTypeId { get; set; }
        public Nullable<decimal> WaterLevel { get; set; }
        public Nullable<decimal> WaterLevelPercent { get; set; }
        public bool IsStopATGPolling { get; set; }
    }
}
