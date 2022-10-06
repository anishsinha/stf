using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Web;

namespace SiteFuel.FreightModels
{
    public class DeliveryDaysViewModel
    {

        public int? DeliveryDays { get; set; }

        public string FromDeliveryTime { get; set; }
        
        public string ToDeliveryTime { get; set; }
        
        public bool IsAcceptNightDeliveries { get; set; }
        
    }
}
