using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.DataAccess.Entities
{
    public partial class DeliveryScheduleXTrackableSchedule
    {
        public bool IsDropCompleted
        {
            get
            {
                return DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Completed || DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.CompletedLate
                    || DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledCompleted || DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.RescheduledLate
                    || DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.PreLoadBolCompleted || DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.UnplannedDropCompleted;
            }
        }
        public bool IsDropped
        {
            get
            {
                return Invoices.Any(t => t.IsActiveInvoice);
            }
        }
        public bool IsScheduleCancelled
        {
            get
            {
                return DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Canceled || DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndCanceled;
            }
        }
        public bool IsScheduleMissed
        {
            get
            {
                return DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Missed || DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.RescheduledMissed;
            }
        }
    }
}
