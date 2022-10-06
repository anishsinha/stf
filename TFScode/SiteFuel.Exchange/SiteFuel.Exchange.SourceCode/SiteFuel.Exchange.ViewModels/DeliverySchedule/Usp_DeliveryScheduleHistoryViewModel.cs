using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class Usp_DeliveryScheduleHistoryViewModel
    {
        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public int? GroupId { get; set; }
        public int? Id { get; set; }
        public DateTimeOffset? Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public decimal? Quantity { get; set; }
        public string ScheduleType { get; set; }
        public ScheduleQuantityType? QuantityTypeId { get; set; }
        public int? UoM { get; set; }
        public int Version { get; set; }
        public int? Type { get; set; }
        public string WeekDayCode { get; set; }
    }
}
