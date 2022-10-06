using MongoDB.Bson;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess;
using System;
using System.Collections.Generic;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DSBLoadQueueMapper
    {
        public static List<DSBLoadQueue> ToEntity(this List<DSBLoadQueueModel> requests)
        {
            var entityList = new List<DSBLoadQueue>();
            foreach (var model in requests)
            {
                var entity = model.ToEntity();
                entityList.Add(entity);
            }
            return entityList;
        }
        public static List<DSBLoadQueueModel> ToEntity(this List<DSBLoadQueue> requests)
        {
            var entityList = new List<DSBLoadQueueModel>();
            foreach (var model in requests)
            {
                var entity = model.ToEntity();
                entityList.Add(entity);
            }
            return entityList;
        }
        public static DSBLoadQueue ToEntity(this DSBLoadQueueModel model)
        {
            var entity = new DSBLoadQueue();
            ObjectId objScheduleBuilderId = ObjectId.Empty;
            ObjectId.TryParse(model.ScheduleBuilderId, out objScheduleBuilderId);
            entity.ScheduleBuilderId = objScheduleBuilderId;
            DateTime dsbScheduleDate = DateTimeOffset.Now.Date;
            if (!string.IsNullOrWhiteSpace(model.Date))
            {
                dsbScheduleDate = Convert.ToDateTime(model.Date).Date;
            }
            entity.Date = dsbScheduleDate;
            ObjectId objRegionId = ObjectId.Empty;
            ObjectId.TryParse(model.RegionId, out objRegionId);
            entity.RegionId = objRegionId;
            ObjectId objShiftId = ObjectId.Empty;
            ObjectId.TryParse(model.ShiftId, out objShiftId);
            entity.ShiftId = objShiftId;
            entity.ShiftIndex = model.ShiftIndex;
            entity.DriverRowIndex = model.DriverRowIndex;
            entity.TfxUserId = model.TfxUserId;
            entity.TfxCompanyId = model.TfxCompanyId;
            entity.CreateDate = DateTimeOffset.Now.Date;
            entity.TfxCreatedBy = model.TfxUserId;
            return entity;
        }
        public static DSBLoadQueueModel ToEntity(this DSBLoadQueue model)
        {
            var entity = new DSBLoadQueueModel();
            entity.Id = model.Id.ToString();
            if (model.ScheduleBuilderId == ObjectId.Empty)
                entity.ScheduleBuilderId = string.Empty;
            else
                entity.ScheduleBuilderId = model.ScheduleBuilderId.ToString();
            
            entity.Date = model.Date.ToString(Resource.constFormatDate);
            entity.RegionId = model.RegionId.ToString();
            entity.ShiftId = model.ShiftId.ToString();
            entity.ShiftIndex = model.ShiftIndex;
            entity.DriverRowIndex = model.DriverRowIndex;
            entity.TfxUserId = model.TfxUserId;
            entity.TfxCompanyId = model.TfxCompanyId;
            return entity;
        }
    }
}
