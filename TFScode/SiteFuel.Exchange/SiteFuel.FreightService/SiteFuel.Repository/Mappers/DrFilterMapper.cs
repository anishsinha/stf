using MongoDB.Bson;
using Newtonsoft.Json;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ScheduleBuilder;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class DrFilterMapper
    {
        public static DrFilterPreferences ToEntity(this DrFilterPreferencesModel model)
        {
            var entity = new DrFilterPreferences();

            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity.Id = ObjectId.Parse(model.Id);
            }
            entity.UserId = model.UserId;
            entity.CompanyId = model.CompanyId;
            entity.Date = model.Date;
            entity.RegionId = model.RegionId;
            entity.FilterData = BsonDocument.Parse(model.FilterData);

            return entity;
        }

        public static DrFilterPreferencesModel ToViewModel(this DrFilterPreferences entity)
        {
            var model = new DrFilterPreferencesModel();

            model.Id = entity.Id.ToString();
            model.UserId = entity.UserId;
            model.CompanyId = entity.CompanyId;
            model.Date = entity.Date;
            model.FilterData = entity.FilterData.ToString();
            model.RegionId = entity.RegionId;

            return model;
        }
    }
}
