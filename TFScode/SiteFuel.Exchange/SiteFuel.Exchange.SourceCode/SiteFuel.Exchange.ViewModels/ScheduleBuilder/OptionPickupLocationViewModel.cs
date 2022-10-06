using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OptionPickupLocationViewModel
    {
    }
    public class OrderFuelInfo
    {
        public Status StatusCode { get; set; } = Status.Success;
        public List<OrderFuelDetails> OrderFuelDetails = new List<OrderFuelDetails>();
    }
    public class OrderFuelDetails
    {
       
        public int OrderId { get; set; }
        public List<DropdownDisplayItem> FuelTypeDetails = new List<DropdownDisplayItem>();
    }
}
