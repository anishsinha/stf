using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DeliveryReqStatusUpdateModel 
    {
        public string DeliveryRequestId { get; set; }

        public int ScheduleStatusId { get; set; }

        public int ScheduleEnrouteStatusId { get; set; }

        public string ScheduleStatusName { get; set; }

        public int UserId { get; set; }
        public int TrackableScheduleId { get; set; }
        public int OrderStatusId { get; set; }
    }
    public class DeliveryReqCancelScheduleUpdateModel
    {
        public string ScheduleBuilderId { get; set; }
        public int DriverId { get; set; }
        public int ShiftIndex { get; set; }
        public string ShiftId { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string DeliveryReqId { get; set; }
        public int TrackableScheduleId { get; set; }
        public int UserId { get; set; }
        public int ScheduleStatusId { get; set; }

        public int ScheduleEnrouteStatusId { get; set; }

        public string ScheduleStatusName { get; set; }

    }
}
