using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.Otto
{
    public class OttoTripModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<DeliveryRequestViewModel> DeliveryRequests { get; set; } = new List<DeliveryRequestViewModel>();
    }
}
