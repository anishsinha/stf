using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderMissedSchedulesViewModel
    {     
        public int OrderId { get; set; }

        public bool IsEndSupplier { get; set; }

        public bool IsBrokeredOrder { get; set; }

        public bool IsOpenOrder { get; set; }
    }
}
