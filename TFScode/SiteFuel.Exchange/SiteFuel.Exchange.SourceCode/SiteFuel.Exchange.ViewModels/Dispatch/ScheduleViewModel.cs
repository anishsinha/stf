using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ScheduleViewModel : BaseViewModel
    {
        public ScheduleViewModel()
        {
            InstanceInitialize();
        }

        public ScheduleViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            GridData = new List<Usp_SchedulesforDriversGridViewModel>();
            MapData = new List<ScheduleMapDataViewModel>();
        }

        public bool IsTimeCardEnabled { get; set; }

        public IList<Usp_SchedulesforDriversGridViewModel> GridData { get; set; }

        public IList<ScheduleMapDataViewModel> MapData { get; set; }
    }
}
