using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ScheduleMapDataViewModel : BaseViewModel
    {
        public ScheduleMapDataViewModel()
        {
           
        }

        public ScheduleMapDataViewModel(Status status)
            : base(status)
        {
           
        }

        public string Name { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public bool IsDriver { get; set; }
    }
}
