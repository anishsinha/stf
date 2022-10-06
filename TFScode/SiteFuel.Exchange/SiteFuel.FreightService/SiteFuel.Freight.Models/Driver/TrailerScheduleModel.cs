using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TrailerScheduleModel : CommonFieldsModel
    {
        public string Id { get; set; }
        public string RegionId { get; set; }
        public string TrailerId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public List<DateTimeOffset> RepeatDayList { get; set; }
        public List<TrailerShiftDetailModel> TrailerShiftDetail { get; set; }
    }

    public class TrailerShiftDetailModel
    {
        public string ShiftId { get; set; }
        public int ColumnId { get; set; }
    }
}
