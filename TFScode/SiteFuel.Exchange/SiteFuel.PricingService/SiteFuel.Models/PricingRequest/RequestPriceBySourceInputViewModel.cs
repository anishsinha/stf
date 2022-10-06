using System.Collections.Generic;

namespace SiteFuel.Models
{
    public class RequestPriceBySourceInputViewModel
    {
        public List<int> RequestPriceDetailIds { get; set; }
    
        public bool IsAxxis { get; set; }
    }
}