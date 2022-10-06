using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class BuyerLoadFilterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DropdownDisplayItem> States { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> Suppliers { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> Carriers { get; set; } = new List<DropdownDisplayItem>();
    }
}
