using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class TimeCardMapper
    {
        public static TimeCardEntry ToEntity(this TimeCardInputRequestViewModel viewModel, TimeCardEntry entity = null)
        {
            if (entity == null)
                entity = new TimeCardEntry();

            entity.ActionId = viewModel.ActionId;

            switch (entity.ActionId)
            {
                case ((int)TimeCardAction.ClockIn):
                case ((int)TimeCardAction.BreakStart):
                case ((int)TimeCardAction.LunchStart):
                case ((int)TimeCardAction.FuelDeliveryStart):
                case ((int)TimeCardAction.PickUpFuelStart):
                    entity.ActionStartDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.UserTimeZone); 
                    break;

                case ((int)TimeCardAction.ClockOut):
                    entity.ActionEndDate = DateTimeOffset.Now.ToTargetDateTimeOffset(viewModel.UserTimeZone); 
                    break;
            }

            entity.CreatedDate = DateTimeOffset.Now;
            entity.DriverId = viewModel.UserId;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.TimeZoneName = viewModel.UserTimeZone;
            return entity;
        }


        public static TimeCardOutputRequestViewModel ToViewModel(this TimeCardEntry entity, TimeCardOutputRequestViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TimeCardOutputRequestViewModel();

            if (entity.ActionId == (int)TimeCardAction.ClockOut)
            {
                viewModel.CurrentActionId = 0;
                viewModel.CurrentActionName = string.Empty;
            }
            else
            {
                viewModel.CurrentActionId = entity.ActionId;
                viewModel.CurrentActionName = entity.MstTimeCardAction.Name;
            }

            viewModel.ActionDateTime = entity.CreatedDate.ToClientDateTime();
            viewModel.UtcActionDateTime = entity.CreatedDate.ToUnixTimeMilliseconds();
            return viewModel;
        }

    }
}
