using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class CancelScheduleModel
    {
        public List<int> TrackableScheduleIds { get; set; }
        public List<string> DeliveryRequestIds { get; set; }
        public List<string> GroupedParentDrIds { get; set; }
        public int DriverId { get; set; }
        public bool IsCancelAll { get; set; }
    }
}
