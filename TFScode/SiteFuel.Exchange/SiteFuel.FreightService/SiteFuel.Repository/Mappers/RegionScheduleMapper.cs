using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class RegionScheduleMapper
    {
        public static RegionScheduleMapping ToEntity(this RegionScheduleModel model)
        {
            var entity = new RegionScheduleMapping();
            ObjectId objRegionId = ObjectId.Empty;
            ObjectId.TryParse(model.RegionId, out objRegionId);
            entity.RegionId = objRegionId;
            ObjectId objRouteId = ObjectId.Empty;
            ObjectId.TryParse(model.RouteId, out objRouteId);
            entity.RouteId = objRouteId;
            if (model.RegionShiftDetail != null)
                entity.RegionShiftDetail = model.RegionShiftDetail.Select(t => t.ToRouteShiftDetailEntity()).ToList();
            DateTime dateStartDate = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(model.StartDate))
            {
                dateStartDate = Convert.ToDateTime(model.StartDate).Date;
            }
            entity.StartDate = dateStartDate;
            DateTime dateEndDate = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(model.EndDate))
            {
                dateEndDate = Convert.ToDateTime(model.EndDate).Date;
            }
            entity.EndDate = dateEndDate;
            entity.RepeatDayList = model.RepeatDayList;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            return entity;
        }

        public static RegionShiftDetail ToRouteShiftDetailEntity(this RegionShiftDetailViewModel model)
        {
            var entity = new RegionShiftDetail();
            ObjectId objShiftId = ObjectId.Empty;
            ObjectId.TryParse(model.ShiftId, out objShiftId);
            entity.ShiftId = objShiftId;
            entity.ColumnIndex = model.ColumnIndex;
            return entity;
        }
    }
}
