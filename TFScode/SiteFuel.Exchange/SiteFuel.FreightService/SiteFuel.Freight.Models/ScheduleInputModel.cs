using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ScheduleInputModel
    {
        public int JobId { get; set; }

        public int OrderId { get; set; }

        public int TrackableScheduleId { get; set; }

        public int DeliveryScheduleId { get; set; }
    }
}
