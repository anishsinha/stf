using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductDetailsApiResponseModel
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public List<DropdownDisplayItem> ProductDetails { get; set; } = new List<DropdownDisplayItem>();
    }
}
