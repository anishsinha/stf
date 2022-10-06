using SiteFuel.FreightModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class RegionResponseModel : StatusModel
    {
        public string RegionId { get; set; }
        //used for reset dsb schedule builder when driver removed from the region.
        public List<ScheduleBuilderViewModel> ScheduleBuilderDetails { get; set; } = new List<ScheduleBuilderViewModel>();
    }
    public class InvitedDriverResponseModel
    {
        public List<string> ScheduleBuilderIds { get; set; } = new List<string>();
        public int DriverId { get; set; }
        public int UserId { get; set; }
    }
}
