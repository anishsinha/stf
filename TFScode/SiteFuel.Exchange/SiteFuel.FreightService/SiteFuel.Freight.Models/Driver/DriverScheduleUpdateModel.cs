using SiteFuel.FreightModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.Driver
{
    public class DriverScheduleUpdateModel:StatusModel
    {
        public List<ScheduleBuilderViewModel> DsbScheduleBuilderInfo { get; set; } = new List<ScheduleBuilderViewModel>();
    }
}
