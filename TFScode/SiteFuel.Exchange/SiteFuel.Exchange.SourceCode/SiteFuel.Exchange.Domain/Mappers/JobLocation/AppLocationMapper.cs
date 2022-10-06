using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AppLocationMapper
    {
        public static AppLocation ToEntity(this AppLocationViewModel viewModel, AppLocation entity = null)
        {
            if (entity == null)
                entity = new AppLocation();

            entity.UserId = viewModel.UserId;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.FCMAppId = viewModel.FCMAppId;
            entity.AppTypeId = (int)viewModel.AppType;
            entity.UpdatedDate = DateTime.Now;
            entity.IsUserLogout = false;
            entity.OrderId = viewModel.OrderId;
            entity.DeliveryScheduleId = viewModel.DeliveryScheduleId;
            entity.TrackableScheduleId = viewModel.TrackableScheduleId;
            entity.ExternalRefID = viewModel.ExternalRefID;
            return entity;
        }
    }
}
