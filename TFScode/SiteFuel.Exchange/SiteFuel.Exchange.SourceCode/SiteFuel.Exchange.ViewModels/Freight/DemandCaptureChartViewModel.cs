using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DemandCaptureChartViewModel
    {
        public float NetVolume { get; set; }
        public float Ullage { get; set; }
        public string TankName { get; set; }
        public int SupplierId { get; set; }
        public string TankId { get; set; }
        public string CaptureTime { get; set; }
        public string StorageId { get; set; }
        public string ProductName { get; set; }
    }
   
}
