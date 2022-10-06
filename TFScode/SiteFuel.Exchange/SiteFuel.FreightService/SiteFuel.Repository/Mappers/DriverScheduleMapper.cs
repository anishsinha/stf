using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Driver;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DriverScheduleMapper
    {
        public static DriverScheduleShiftMapping ToEntity(this DriverScheduleMappingViewModel model)
        {
            var entity = new DriverScheduleShiftMapping();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity.Id = ObjectId.Parse(model.Id);
            }
            entity.DriverId = model.DriverId;

            entity.DriverScheduleList = model.ScheduleList.Select(t => t.ToDriverScheduleEntity()).ToList();
            entity.IsUnplanedSchedule = model.IsUnplanedSchedule;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            return entity;
        }

        public static DriverSchedule ToDriverScheduleEntity(this DriverScheduleViewModel model)
        {
            var entity = new DriverSchedule();
            //if (!string.IsNullOrWhiteSpace(model.Id))
            //{
            //    entity.Id = ObjectId.Parse(model.Id);
            //}
            entity.Id = model.Id;
            entity.ShiftId = ObjectId.Parse(model.ShiftId);
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.IsActive = model.IsActive;
            entity.RepeatDayList = model.RepeatDayList.Select(t => new DateTimeOffset(t)).ToList();
            entity.Description = model.Description;
            entity.TypeId = model.TypeId;
            entity.RepeatEveryDay = model.RepeatEveryDay;
            return entity;
        }


        public static DriverScheduleViewModel ToDriverScheduleModel(this DriverSchedule entity)
        {
            var model = new DriverScheduleViewModel();
            model.Id = entity.Id.ToString();
            model.ShiftId = entity.ShiftId.ToString();
            model.StartDate = entity.StartDate.DateTime;
            model.EndDate = entity.EndDate.DateTime;
            model.IsActive = entity.IsActive;
            model.RepeatDayList = entity.RepeatDayList.Select(t => t.DateTime).ToList();
            model.Description = entity.Description;
            return model;
        }
        public static DriverScheduleMappingViewModel ToModel(this DriverScheduleShiftMapping entity)
        {
            var model = new DriverScheduleMappingViewModel();
            model.Id = entity.Id.ToString(); ;
            model.IsActive = entity.IsActive;
            model.IsDeleted = entity.IsDeleted;
            model.CreatedBy = entity.CreatedBy;
            model.CreatedOn = entity.CreatedOn;
            model.UpdatedBy = entity.UpdatedBy;
            model.UpdatedOn = entity.UpdatedOn;
            model.ScheduleList = entity.DriverScheduleList.Select(t => t.ToDriverScheduleModel()).ToList();
            return model;
        }
    }
}
