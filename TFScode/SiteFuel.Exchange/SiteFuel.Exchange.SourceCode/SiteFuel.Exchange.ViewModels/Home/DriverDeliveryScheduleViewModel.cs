using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverDeliveryScheduleViewModel : StatusViewModel
    {
        public DriverDeliveryScheduleViewModel()
        {
            InstanceInitialize();
        }

        public DriverDeliveryScheduleViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            deliveryScheduleViewModel = new DeliveryScheduleViewModel();
            calendarViewModel = new CalenderViewModel();
        }
        
        public DeliveryScheduleViewModel deliveryScheduleViewModel { get; set; }

        public CalenderViewModel calendarViewModel { get; set; }
    }
}
