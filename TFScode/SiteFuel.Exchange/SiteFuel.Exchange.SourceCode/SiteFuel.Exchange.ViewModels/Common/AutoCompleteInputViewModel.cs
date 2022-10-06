using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class AutoCompleteInputViewModel
    {
        public List<int> OrderList { get; set; }
        public string Terminal { get; set; }
        public List<int> FuelTypeId { get; set; }
    }
}
