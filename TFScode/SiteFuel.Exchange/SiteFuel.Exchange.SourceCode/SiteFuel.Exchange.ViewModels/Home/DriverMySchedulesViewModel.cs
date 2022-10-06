using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverMySchedulesViewModel : StatusViewModel
    {
        public DriverMySchedulesViewModel()
        {
            InstanceInitialize();
        }

        public DriverMySchedulesViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            deliveryScheduleViewModel = new List<DeliveryScheduleViewModel>();
            driverDetails = new DriverLocationViewModel();
            latLongDetails = new List<LatLongDetailsViewModel>();
        }
        
        public List<DeliveryScheduleViewModel> deliveryScheduleViewModel { get; set; }

        public DriverLocationViewModel driverDetails { get; set; }

        public List<LatLongDetailsViewModel> latLongDetails { get; set; }
    }
}
