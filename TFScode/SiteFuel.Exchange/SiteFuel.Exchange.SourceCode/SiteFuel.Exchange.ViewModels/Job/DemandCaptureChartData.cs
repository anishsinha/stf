using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Job
{
   public class DemandCaptureChartData
    {
        List<string> siteID { get; set; }
        List<string> TankId { get; set; }
        public int noOfDays { get; set; }
    }
}
