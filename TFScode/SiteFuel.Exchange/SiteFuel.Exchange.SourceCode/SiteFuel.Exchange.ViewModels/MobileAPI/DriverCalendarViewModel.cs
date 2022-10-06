using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class DriverCalendarViewModel
    {
        public DriverCalendarViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            
        }

        public DateTime Date { get; set; }

        public int CalendarDropType { get; set; }

        public long CalendarDate { get; set; }
    }
}
