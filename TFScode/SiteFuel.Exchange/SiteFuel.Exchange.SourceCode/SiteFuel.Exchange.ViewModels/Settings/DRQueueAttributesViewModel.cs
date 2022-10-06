using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    // using this class for setting boolean flags for Dr queue show-hide properties. Code comment published on 15th feb 2021
    public class DRQueueAttributesViewModel
    {
        public bool CustomerName { get; set; }

        public bool DeliveryShift { get; set; }

        public bool TrailerCompatibility { get; set; }

        public bool HoursToCoverDistance { get; set; } = true;
    }
}
