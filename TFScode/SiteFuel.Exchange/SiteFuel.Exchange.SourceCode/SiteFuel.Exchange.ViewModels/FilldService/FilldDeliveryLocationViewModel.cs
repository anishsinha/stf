using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.FilldService
{
    public class FilldDeliveryLocationViewModel
    {
        public string address { get; set; }
        public Decimal lat { get; set; }
        public Decimal lng { get; set; }
        public int truck_id { get; set; }
        public long driver_id { get; set; }
        public int external_stop_id { get; set; }
        public string date { get; set; }
        public string user_id { get; set; }
        public string territory_id { get; set; }
    }
    public class DeliveryLocationDataModel
    {
        public long waypoint_id { get; set; }
    }
    public class FilldDeliveryResponseViewModel : FilldStatusViewModel
    {
        public DeliveryLocationDataModel Data { get; set; }
    }
}
