using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class NextDeliveryScheduleViewModel
    {
        public string ScheduleDate { get; set; }

        public string ScheduleTime { get; set; }

        public DateTimeOffset Date { get; set; }

        public string Quantity { get; set; }
    }
}
