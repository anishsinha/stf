using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
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
    public class CancelDeliverySchedule
    {
        public string ScheduleBuilderId { get; set; }
        public int DriverId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string DeliveryReqId { get; set; }
        public int TrackableScheduleId { get; set; }
        public int UserId { get; set; }

        public int ScheduleStatus { get; set; }
        public int TrackScheduleStatus { get; set; }
        public string TrackScheduleStatusName { get; set; }
        public int StatusClassId { get; set; }
    }
    public class CancelDSDeliveryScheduleInfo
    {
        public int TfxCompanyId { get; set; }
        public string RegionId { get; set; }
        public List<CancelDSDeliverySchedule> CancelDSDeliverySchedules { get; set; } = new List<CancelDSDeliverySchedule>();
    }
    public class CancelDSDeliverySchedule
    {
        public string DeliveryReqId { get; set; }
        public bool IsSubDR { get; set; } = false;
    }

    public class CancelDSDeliveryScheduleViewModel
    {
        public bool IsChecked { get; set; } = false;
        public string ScheduleBuilderDate { get; set; }
        public string ScheduleBuilderId { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string Quantity { get; set; }
        public string FuelType { get; set; }
        public string CurrentState { get; set; }
        public string ShiftId { get; set; }
        public int ShiftIndex { get; set; }
        public int DriverRowIndex { get; set; }
        public int DriverColIndex { get; set; }
        public string DeliveryReqId { get; set; }
        public int TrackableScheduleId { get; set; }

    }
}
