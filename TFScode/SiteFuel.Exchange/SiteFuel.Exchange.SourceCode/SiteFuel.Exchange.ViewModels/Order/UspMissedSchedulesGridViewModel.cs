using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspMissedSchedulesGridViewModel
    {
        public int  Id { get; set; }

        public string DriverName { get; set; }

        public string Status { get; set; }

        public string ScheduleDate { get; set; }

        public string DeliveryWindow { get; set; }

        public int OrderId { get; set; }
    }
}
