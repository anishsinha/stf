using MongoDB.Bson;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.MdbDataAccess.Collections;
using System.Linq;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class TrailerScheduleMapper
    {
        public static TrailerScheduleMapping ToEntity(this TrailerScheduleModel model)
        {
            var entity = new TrailerScheduleMapping();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity.Id = ObjectId.Parse(model.Id);
            }

            entity.RegionId = ObjectId.Parse(model.RegionId);
            entity.TrailerId = ObjectId.Parse(model.TrailerId);
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            entity.RepeatDayList = model.RepeatDayList;
            entity.TrailerShiftDetail = model.TrailerShiftDetail.Select(t => t.ToTrailerShiftDetailEntity()).ToList();
            
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            return entity;
        }

        public static TrailerShiftDetail ToTrailerShiftDetailEntity(this TrailerShiftDetailModel model)
        {
            var entity = new TrailerShiftDetail();
            entity.ShiftId = ObjectId.Parse(model.ShiftId);
            entity.ColumnId = model.ColumnId;
            return entity;
        }
    }
}
