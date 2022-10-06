using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverLocationViewModel
    {
        public DriverLocationViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int DriverId { get; set; }

        public string DriverName { get; set; }

        public string PhoneNumber { get; set; }

        public int? StatusId { get; set; }
    }
}
