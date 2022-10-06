using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
   public  class SaleTankModel
    {
        public int Id { get; set; }
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
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
