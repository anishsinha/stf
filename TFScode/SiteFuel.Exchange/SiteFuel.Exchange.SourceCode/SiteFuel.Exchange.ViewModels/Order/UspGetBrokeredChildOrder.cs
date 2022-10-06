using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Order
{
    public class UspGetBrokeredChildOrder
    {
        public int FuelRequestId { get; set; }
        public int OrderId { get; set; }
        public int BrokeredToCompanyId { get; set; }
        public string BrokeredToCompanyName { get; set; }
        public int BrokeredToUserId { get; set; }
        public string BrokeredToUserName { get; set; }
        public int StatusId { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; }
    }
}
