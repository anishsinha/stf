using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class ShiftMapper
    {
        public static Shift ToEntity(this ShiftViewModel model)
        {
            var entity = new Shift();
            if (!string.IsNullOrWhiteSpace(model.Id))
                entity.Id = ObjectId.Parse(model.Id);
            entity.Name = model.Name;
            entity.CompanyId = model.CompanyId;
            if (!string.IsNullOrWhiteSpace(model.RegionId))
                entity.RegionId = ObjectId.Parse(model.RegionId);
            entity.StartTime = model.StartTime;
            entity.EndTime = model.EndTime;
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            return entity;
        }

        public static ShiftViewModel ToModel(this Shift entity)
        {
            var model = new ShiftViewModel();
            model.Id = entity.Id.ToString();
            model.Name = entity.Name;
            model.CompanyId = entity.CompanyId;
            model.StartTime = entity.StartTime;
            model.EndTime = entity.EndTime;
            model.CreatedBy = entity.CreatedBy;
            model.CreatedOn = entity.CreatedOn;
            model.IsActive = entity.IsActive;
            model.IsDeleted = entity.IsDeleted;
            return model;
        }
    }
}
