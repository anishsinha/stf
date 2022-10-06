using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
   public class ForecastingTankViewModel
    {
        public string TankName { get; set; }
        public float TankFill { get; set; }
        public float TankFillRemaining { get; set; }
        public string LastInventoryReading { get; set; }
        public string UllageSinceLastReading { get; set; }
        public string LastReadingTime { get; set; }
        public string EstimatedCurrentInventory { get; set; }
        public string DeliverySinceLastReading { get; set; }
        public float? DaysLeft { get; set; }
        public string ProductType { get; set; }
        public string RegionId { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public double TankInventoryDiffinHrs { get; set; }
        public int TfxProductTypeId { get; set; }
        public string Status { get; set; }
        public string MaxFillQuantity { get; set; }
        public string AvgSale { get; set; }
        public string PrevSale { get; set; }
        public string WeekAgoSale { get; set; }
        public string SiteInstructions { get; set; }
        public int LocationManagedType { get; set; }
    }
}
