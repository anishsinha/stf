using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverScheduleGridViewModel
    {
        public DriverScheduleGridViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
           
        }

        public string DriverName { get; set; }

        public string PhoneNumber { get; set; }

        public string PONumber { get; set; }

        public string ScheduleDate { get; set; }

        public string ScheduleStartTime { get; set; }

        public string ScheduleEndTime { get; set; }

        public string IsDeliverySchedule { get; set; }

        public int? StatusId { get; set; }
    }
}
