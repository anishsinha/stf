using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ScheduleBuilderMapper
    {
        public static DSBSaveModel ToDsbSaveModel(this ScheduleBuilderViewModel viewModel)
        {
            var model = new DSBSaveModel();
            model.CompanyId = viewModel.CompanyId;
            model.Date = viewModel.Date;
            model.DateFilter = viewModel.DateFilter;
            model.DSBFilter = viewModel.DSBFilter;
            model.DeletedDriverScheduleMappingId = viewModel.DeletedDriverScheduleMappingId;
            model.DeletedGroupId = viewModel.DeletedGroupId;
            model.DeletedTripId = viewModel.DeletedTripId;
            model.Id = viewModel.Id;
            model.ObjectFilter = viewModel.ObjectFilter;
            model.RegionFilter = viewModel.RegionFilter;
            model.RegionId = viewModel.RegionId;
            model.Shifts = viewModel.Shifts;
            model.TimeStamp = viewModel.TimeStamp;
            model.UserId = model.UserId;
            model.WindowMode = viewModel.WindowMode;
            model.IsDriverScheduleReset = viewModel.IsDriverScheduleReset;
            return model;
        }

        public static void UpdateModifiedPostLoadedDrValues(this DeliveryRequestViewModel entity, DeliveryRequestViewModel model)
        {
            entity.BadgeNo1 = model.BadgeNo1;
            entity.BadgeNo2 = model.BadgeNo2;
            entity.BadgeNo3 = model.BadgeNo3;
            entity.DispactherNote = model.DispactherNote;
            entity.BulkPlant = model.BulkPlant;
            entity.Terminal = model.Terminal;
            entity.UoM = model.UoM;
            entity.OrderId = model.OrderId;
            entity.PickupLocationType = model.PickupLocationType;
            entity.RequiredQuantity = model.RequiredQuantity;
            entity.ScheduleStatus = (int)Utilities.DeliveryScheduleStatus.Modified;
            entity.Compartments = model.Compartments;
        }
    }
}
