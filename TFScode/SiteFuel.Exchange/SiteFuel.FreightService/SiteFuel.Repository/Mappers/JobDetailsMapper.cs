using MongoDB.Bson;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using SiteFuel.MdbDataAccess.PriceFactories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class JobDetailsMapper
    {
        public static JobAdditionalDetail ToEntity(this JobAdditionalDetailsModel model)
        {
            var entity = new JobAdditionalDetail
            {
                AdditionalImageDescription = model.AdditionalImageDescription,
                AdditionalImageFilePath = model.AdditionalImageFilePath,
                DeliveryDays = model.DeliveryDays,
                DeliveryDaysList = model.DeliveryDaysList,
                FromDeliveryTime = model.FromDeliveryTime,
                IsActive = model.IsActive,
                TfxJobId = model.JobId,
                SiteImageFilePath = model.SiteImageFilePath,
                ToDeliveryTime = model.ToDeliveryTime,
                TfxDisplayJobId = model.SiteId,
                TfxJobName = model.JobName,
                IsAutoCreateDREnable = model.IsAutoCreateDREnable,
                TrailerType = model.TrailerType,
                DistanceCovered= model.DistanceCovered
            };
            return entity;
        }
        public static JobAdditionalDetailsModel ToViewModel(this JobAdditionalDetail entity)
        {
            var model = new JobAdditionalDetailsModel
            {
                AdditionalImageDescription = entity.AdditionalImageDescription,
                AdditionalImageFilePath = entity.AdditionalImageFilePath,
                DeliveryDays = entity.DeliveryDays,
                DeliveryDaysList = entity.DeliveryDaysList,
                FromDeliveryTime = entity.FromDeliveryTime,
                IsActive = entity.IsActive,
                JobId = entity.TfxJobId,
                SiteImageFilePath = entity.SiteImageFilePath,
                ToDeliveryTime = entity.ToDeliveryTime,
                Id = entity.Id.ToString(),
                TfxDisplayJobId = entity.TfxDisplayJobId,
                IsAutoCreateDREnable = entity.IsAutoCreateDREnable,
                TrailerType = entity.TrailerType,
                DistanceCovered = entity.DistanceCovered
               
            };
            if (entity.Tanks.Count > 0)
            {
                foreach (var item in entity.Tanks)
                {
                    model.TankDetails.Add(item.ToViewModel());
                }
            }
            return model;
        }
    }
}
