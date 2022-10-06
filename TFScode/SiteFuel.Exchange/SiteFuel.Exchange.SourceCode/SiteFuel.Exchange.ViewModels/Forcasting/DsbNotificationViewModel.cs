using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Forcasting
{
   public class DsbNotificationViewModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public int TfxJobId { get; set; }
        public string RegionId { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string ScheduleBuilderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
