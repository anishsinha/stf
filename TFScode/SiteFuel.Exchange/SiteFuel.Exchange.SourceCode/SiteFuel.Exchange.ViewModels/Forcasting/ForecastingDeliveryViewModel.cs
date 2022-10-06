using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
    public class ForecastingDeliveryViewModel
    {
        public string TankName { get; set; }
        public int NoOfDeliveries { get; set; }
        public string LastDeliveredQty { get; set; }
        public string LastDeliveredDate { get; set; }
    }
}
