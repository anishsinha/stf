using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.DeliveryRequest
{
    public class DeliveryWindowInfoModel
    {
        public DateTime RetainDate { get; set; }
        public string RetainTime { get; set; }
        public DateTime StartDate { get; set; }
        public string StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public string EndTime { get; set; }
    }
}
