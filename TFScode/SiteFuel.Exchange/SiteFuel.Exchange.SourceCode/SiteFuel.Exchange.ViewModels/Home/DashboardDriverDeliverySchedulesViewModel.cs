using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DashboardDriverDeliverySchedulesViewModel : StatusViewModel
    {
        public DashboardDriverDeliverySchedulesViewModel()
        {
            InstanceInitialize();
        }

        public DashboardDriverDeliverySchedulesViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            RecenDeliverySchedules = new List<DeliveryScheduleViewModel>();
        }

        public int TotalDeliverySchedulesCount { get; set; }

        public List<DeliveryScheduleViewModel> RecenDeliverySchedules { get; set; }

    }
}
