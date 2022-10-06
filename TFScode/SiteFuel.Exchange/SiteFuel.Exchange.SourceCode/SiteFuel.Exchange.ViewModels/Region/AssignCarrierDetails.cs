using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class AssignDrToCarrierModel
    {
        public List<TfxCarrierDropdownDisplayItem> CarrierDetails { get; set; } = new List<TfxCarrierDropdownDisplayItem>();
        public List<DispatchOrderDetailsDropdown> OrderDetails { get; set; } = new List<DispatchOrderDetailsDropdown>();
    }
}
