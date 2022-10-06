using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class ScheduleOutputModel
    {
        public List<TankDetailListModel> TankDetailList { get; set; }

        public List<ScheduleTankModel> ScheduleTank { get; set; }

        public List<JobAdditionalDetailsModel> JobDetails { get; set; }
    }
}
