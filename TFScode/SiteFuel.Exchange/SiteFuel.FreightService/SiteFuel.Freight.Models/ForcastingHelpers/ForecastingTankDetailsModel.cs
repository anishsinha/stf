using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ForecastingTankDetailsModel
    {
        public string TankName { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public string SiteId { get; set; }
        public float FuelCapacity { get; set; }
        public float TankFill { get; set; }
        public float TankFillRemaining { get; set; }
        public string LastInventoryReading { get; set; } = Resource.lblHyphen;
        public string UllageSinceLastReading { get; set; } = Resource.lblHyphen;
        public string LastReadingTime { get; set; } = Resource.lblHyphen;
        public string EstimatedCurrentInventory { get; set; } = Resource.lblHyphen;
        public string DeliverySinceLastReading { get; set; } = Resource.lblHyphen;
        public string RegionId { get; set; }
        public float? DaysLeft { get; set; } = null;
        public string ProductType { get; set; }
        public double TankInventoryDiffinHrs { get; set; }
        public int TfxProductTypeId { get; set; }
        public string Status { get; set; }
        public string MaxFillQuantity { get; set; } = Resource.lblHyphen;
        public string AvgSale { get; set; } = Resource.lblHyphen;
        public string PrevSale { get; set; } = Resource.lblHyphen;
        public string WeekAgoSale { get; set; } = Resource.lblHyphen;
        public Nullable<int> TankSequence { get; set; }
    }
}
