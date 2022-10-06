using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DsbNotificationMapper
    {
        public static DsbNotification ToEntity(this DsbNotificationModel model)
        {
            var entity = new DsbNotification();

            if (!string.IsNullOrEmpty(model.Id)) { entity.Id = ObjectId.Parse(model.Id); }
            entity.TfxJobId = model.TfxJobId;
            entity.RegionId = model.RegionId;
            entity.Message = model.Message;
            entity.ScheduleBuilderId = model.ScheduleBuilderId;
            entity.Status = model.Status;
            entity.Type = model.Type;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = model.CreatedBy;

            return entity;
        }

        public static DsbNotificationModel ToViewModel(this DsbNotification entity)
        {
            var model = new DsbNotificationModel();
            model.Id = entity.Id.ToString();
            model.TfxJobId = entity.TfxJobId;
            model.RegionId = entity.RegionId;
            model.Message = entity.Message;
            model.ScheduleBuilderId = entity.ScheduleBuilderId;
            model.Status = entity.Status;
            model.Type = entity.Type;
            model.CreatedDate = entity.CreatedDate;
            model.CreatedBy = entity.CreatedBy;

            return model;
        }
    }
}