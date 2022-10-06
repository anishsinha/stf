using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.DeliveryAlerts.Firebase
{
    public static class MobileCancelScheduleMapper
    {
        public static CancelScheduleModel ToCancelScheduleViewModel(this MobileCanceledScheduleModel model)
        {
            var entity = new CancelScheduleModel();
            entity.TrackableScheduleIds = model.TrackableScheduleIds;
            entity.DeliveryRequestIds = model.DeliveryRequestIds;
            entity.GroupedParentDrIds = model.GroupedParentDrIds;
            entity.DriverId = model.DriverId;
            entity.IsCancelAll = model.IsCancelAll;
            return entity;
        }

    }
}
