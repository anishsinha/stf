using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public static class TrailerCompartmentMapper
    {
        public static List<CompartmentViewModel> ToEntity(this List<TruckCompartment> requests)
        {
            var entityList = new List<CompartmentViewModel>();
            foreach (var model in requests)
            {
                var entity = new CompartmentViewModel();
                entity.CompartmentId = model.CompartmentId;
                entity.Capacity = model.Capacity;
                entity.FuelType = model.FuelType;
                entityList.Add(entity);
            }
            return entityList;
        }

        public static CompartmentViewModel ToEntity(this TruckCompartment model)
        {
            var entity = new CompartmentViewModel();
            entity.CompartmentId = model.CompartmentId;
            entity.Capacity = model.Capacity;
            entity.FuelType = model.FuelType;
            return entity;
        }

        public static List<CompartmentsInfo> ToCloneEntity(this List<CompartmentsInfoViewModel> requests)
        {
            var entityList = new List<CompartmentsInfo>();
            foreach (var model in requests)
            {
                var entity = new CompartmentsInfo();
                entity.TrailerId = string.IsNullOrEmpty(model.TrailerId) ? ObjectId.Empty : ObjectId.Parse(model.TrailerId);
                entity.CompartmentId = model.CompartmentId;
                entity.Quantity = model.Quantity;
                entityList.Add(entity);
            }
            return entityList;
        }

        public static List<CompartmentsInfoViewModel> ToCloneEntity(this List<CompartmentsInfo> requests)
        {
            var entityList = new List<CompartmentsInfoViewModel>();
            foreach (var model in requests)
            {
                var entity = new CompartmentsInfoViewModel();
                entity.TrailerId = model.TrailerId.ToString();
                entity.CompartmentId = model.CompartmentId;
                entity.Quantity = model.Quantity;
                entityList.Add(entity);
            }
            return entityList;
        }
    }
}
