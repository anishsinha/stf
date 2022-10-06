using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.OttoSchedule
{
    public class OttoBuilder
    {
        public string Date { get; set; }
        public string RegionId { get; set; }
        public string ShiftId { get; set; }
        public List<OttoTripModel> Loads { get; set; } = new List<OttoTripModel>();
    }
}
