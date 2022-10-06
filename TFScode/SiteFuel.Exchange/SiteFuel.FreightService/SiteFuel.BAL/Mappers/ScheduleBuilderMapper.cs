using SiteFuel.FreightModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public static class ScheduleBuilderMapper
    {
        public static DSBSaveModel ToDsbSaveModel(this ScheduleBuilderViewModel model)
        {
            var saveModel = new DSBSaveModel();
            saveModel.CompanyId = model.CompanyId;
            saveModel.Date = model.Date;
            saveModel.DateFilter = model.DateFilter;
            saveModel.DeletedDriverScheduleMappingId = model.DeletedDriverScheduleMappingId;
            saveModel.Id = model.Id;
            saveModel.isCreateSchedule = model.isCreateSchedule;
            saveModel.ObjectFilter = model.ObjectFilter;
            saveModel.RegionFilter = model.RegionFilter;
            saveModel.RegionId = model.RegionId;
            saveModel.Shifts = model.Shifts;
            saveModel.Status = model.Status;
            saveModel.TimeStamp = model.TimeStamp;
            //saveModel.Trips = model.Trips;
            saveModel.UserId = model.UserId;
            return saveModel;
        }
    }
}
