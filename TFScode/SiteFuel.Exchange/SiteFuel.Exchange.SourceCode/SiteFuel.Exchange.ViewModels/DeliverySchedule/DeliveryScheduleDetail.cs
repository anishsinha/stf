using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryScheduleDetail
    {
        public DeliveryScheduleDetail()
        {
            DayNames = new List<string>();
        }

        public string Date { get; set; }

        public IEnumerable<string> DayNames { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public string Gallons { get; set; }

        public int GroupId { get; set; }

        public int StatusId { get; set; }

        public int Type { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<int> RescheduledTrackableId { get; set; }
    }


    public class ScheduleStatusUpdateInput
    {
        public int OrderId { get; set; }
        public string TimeZoneName { get; set; }
        public DateTimeOffset? FuelRequestEndDate { get; set; }
        public DateTimeOffset? JobEndDate { get; set; }
        public decimal FuelRequestMaxQuantity { get; set; }
        public decimal? OrderMaxQuantity { get; set; }
        public int? OrderClosingThreshold { get; set; }
        public decimal DroppedQuantity { get; set; }
        public decimal ExistingScheduleQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public int ActiveOrderScheduleVersion { get; set; }
        public int FuelRequestTypeId { get; set; }
    }

    public class UpdateScheduleStatusInputModel
    {
        public string TimeZoneName { get; set; }
        public int? CompanyId { get; set; }
        public int? JobId { get; set; }
        public int? AcceptedCompanyId { get; set; }
        public int? DeliveryGroupId { get; set; }
        public string PoNumber { get; set; }
        public DateTimeOffset Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime? ShiftEndDateTime { get; set; }
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? StatusId { get; set; }
    }

    public class ResetDeliveryGroupScheduleModel: ResponseViewModel
    {
        public List<int> DeliveryGroupIds { get; set; } = new List<int>();
        public List<int> DeliveryScheduleIds { get; set; } = new List<int>();
        public string ScheduleBuilderId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string UpdatedByName { get; set; }
        public List<string> DeliveryRequestIds { get; set; }
    }
}
