using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DemandCaptureChartData
    {
        public List<string> siteID { get; set; }
        public List<string> TankId { get; set; }
        public int noOfDays { get; set; }
    }
}
