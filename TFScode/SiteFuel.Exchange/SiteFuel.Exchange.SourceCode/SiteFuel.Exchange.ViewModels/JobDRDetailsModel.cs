using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobDRDetailsModel
    {
        public int JobId { get; set; }
        public DeliveryReqPriority Priority { get; set; } = DeliveryReqPriority.MustGo;
    }

    public class JobDRPriorityInputModel
    {
        public List<int> JobIds { get; set; }
        public int CompanyId { get; set; }
    }
}
