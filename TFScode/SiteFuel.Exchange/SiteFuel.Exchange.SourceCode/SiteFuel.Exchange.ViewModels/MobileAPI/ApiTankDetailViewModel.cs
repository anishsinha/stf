using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ApiTankDetailViewModel
    {
        public int Id { get; set; }

        public string TankName { get; set; }

        public string ProducType { get; set; }

        public int ProducTypeId { get; set; }

        public string StorageId { get; set; }

        public string TankModelTypeId { get; set; }

        public string TankImage { get; set; }

        public string TankChart { get; set; }

        public string TankChartFileName { get; set; }

        public string TankMake { get; set; }

        public string TankModel { get; set; }

        public int ScaleMeasurement { get; set; }

        public float LastReading { get; set; }

        public DateTime CaptureTime { get; set; }

        public string SiteId { get; set; }

        public Nullable<decimal> FuelCapacity { get; set; }

        public int JobId { get; set; }

        public string TankId { get; set; }

        public string TankNumber { get; set; }

        public string UoM { get; set; }

        public float Ullage { get; set; }

        public int Priority { get; set; }

        public Nullable<TankType> TankType { get; set; }

        public Nullable<decimal> MinFill { get; set; }

        public Nullable<FillType> FillType { get; set; }

        public Nullable<decimal> MinFillPercent { get; set; }

        public Nullable<decimal> MaxFillPercent { get; set; }

        public Nullable<decimal> MaxFill { get; set; }

        public Nullable<decimal> PhysicalPumpStop { get; set; }

        public Nullable<decimal> RunOutLevel { get; set; }

        public Nullable<DipTestMethod> DipTestMethod { get; set; }

        public Nullable<decimal> ThresholdDeliveryRequest { get; set; }

        public List<int> TankAcceptDelivery { get; set; }

        public int JobXAssetId { get; set; }

        public List<int> MappedToProductTypeId { get; set; }
        public List<int> MappedToBlendProductTypeId { get; set; }
        public List<int> TanksConnected { get; set; }
        public bool IsTankConnected { get; set; }
        public Nullable<int> TankSequence { get; set; }
    }
}
