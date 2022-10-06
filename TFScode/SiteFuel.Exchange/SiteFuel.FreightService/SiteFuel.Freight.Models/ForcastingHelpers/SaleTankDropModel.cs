using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.ForcastingHelpers
{
    public class SaleTankDropModel : ITankDropModel
    {
        public string SiteId { get; set; }
        public string TankId { get; set; }
        public string StorageId { get; set; }
        public decimal DroppedQuantity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TimeZoneName { get; set; }
    }
}
