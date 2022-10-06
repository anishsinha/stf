using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.DispatchScheduler
{
    public class ChildDeliveryRequestInfoViewModel: StatusViewModel
    {
        public string DrId { get; set; }
        public int OrderId { get; set; }
        public string BrokeredParentId { get; set; }
    }
}
