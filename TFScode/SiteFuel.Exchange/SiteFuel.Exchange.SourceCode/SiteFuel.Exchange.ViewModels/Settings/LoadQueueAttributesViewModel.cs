using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    // using this class for setting boolean flags for Load queue show-hide properties. Code comment published on 15th feb 2021
    public class LoadQueueAttributesViewModel
    {
        public bool LocationName { get; set; } 

        public bool CustomerName { get; set; }

        public bool Driver { get; set; }

        public bool TrailerName { get; set; }
    }
}
