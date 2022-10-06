using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightModels.ScheduleBuilder
{

    public class CalendarLoadDataModel : StatusModel
    {
        public List<DropdownDisplayItem> Shifts { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> Columns { get; set; } = new List<DropdownDisplayItem>();
        public List<DropdownDisplayItem> Loads { get; set; } = new List<DropdownDisplayItem>();
    }

}
