using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverScheduleUpdateViewModel : StatusViewModel
    {
        public List<ScheduleBuilderViewModel> DsbScheduleBuilderInfo { get; set; } = new List<ScheduleBuilderViewModel>();
    }
}
