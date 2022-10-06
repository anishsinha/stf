using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueFill.DemandCaptureDataAccess.Entities
{
    public class SaleTank
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(256)]
        public string SiteId { get; set; }
        [Required, StringLength(256)]
        public string TankId { get; set; }
        [Required, StringLength(256)]
        public string StorageId { get; set; }
        public int BandPeriod { get; set; } = 1;
        public TimeSpan DayStartOn { get; set; }
        public decimal? MaxFill { get; set; }
        public int? FillType { get; set; }
        public decimal? FuelCapacity { get; set; }
        public int? Retain { get; set; }
        public int? SaftyStock { get; set; }
        public int? Runout { get; set; }
        public int? InventoryUoM { get; set; }
    }
}
