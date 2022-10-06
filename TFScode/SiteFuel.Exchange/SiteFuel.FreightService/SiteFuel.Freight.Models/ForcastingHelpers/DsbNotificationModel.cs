using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DsbNotificationModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public int TfxJobId { get; set; }
        public string RegionId { get; set; }
        public string ScheduleBuilderId { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
