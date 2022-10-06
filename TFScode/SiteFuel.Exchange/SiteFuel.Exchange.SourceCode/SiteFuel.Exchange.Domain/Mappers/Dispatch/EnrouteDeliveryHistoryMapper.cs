using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class EnrouteDeliveryHistoryMapper
    {
        public static EnrouteDeliveryHistory ToEntity(this EnrouteDeliveryViewModel viewModel, EnrouteDeliveryHistory entity = null)
        {
            if (entity == null)
                entity = new EnrouteDeliveryHistory();

            entity.UserId = viewModel.UserId;
            entity.OrderId = viewModel.OrderId;
            entity.EnrouteDate = DateTime.Now;
            if (viewModel.DeliveryScheduleId > 0)
                entity.DeliveryScheduleId = viewModel.DeliveryScheduleId;
            if (viewModel.TrackableScheduleId > 0)
                entity.TrackableScheduleId = viewModel.TrackableScheduleId;
            entity.StatusId = viewModel.StatusId;

            return entity;
        }
    }
}
