using MongoDB.Bson;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class TrailerFuelRetainMapper
    {
        public static TrailerFuelRetain ToEntity(this TrailerFuelRetainViewModel model)
        {
            var entity = new TrailerFuelRetain();
            if (model != null)
            {
                ObjectId TrailerId = ObjectId.Empty;
                if (!string.IsNullOrEmpty(model.TrailerId))
                    ObjectId.TryParse(model.TrailerId, out TrailerId);
                entity.TrailerId = TrailerId;
                entity.CompartmentId = model.CompartmentId;
                entity.Quantity = model.Quantity;
                entity.UOM = model.UOM;
                entity.ProductType = model.ProductType;
                entity.ProductId = model.ProductId;
                entity.OrderId = model.OrderId;
                entity.DeliveryRequestId = string.IsNullOrEmpty(model.DeliveryRequestId) ? ObjectId.Empty : ObjectId.Parse(model.DeliveryRequestId);
                entity.TfxDriverId = model.TfxDriverId;
                entity.CreatedOn = DateTime.Now;
                entity.IsActive = true;
                entity.IsDeleted = false;

            }
            return entity;
        }
        public static List<TrailerFuelRetainViewModel> ToEntity(this List<TrailerFuelRetain> model)
        {
            var entity = new List<TrailerFuelRetainViewModel>();
            if (model != null && model.Count > 0)
            {
                foreach (var item in model)
                {
                    var trailerFuelRetain = new TrailerFuelRetainViewModel();
                    trailerFuelRetain.Id = item.Id.ToString();
                    trailerFuelRetain.TrailerId = item.TrailerId.ToString();
                    trailerFuelRetain.CompartmentId = item.CompartmentId;
                    trailerFuelRetain.Quantity = item.Quantity;
                    trailerFuelRetain.UOM = item.UOM;
                    trailerFuelRetain.ProductType = item.ProductType;
                    trailerFuelRetain.ProductId = item.ProductId;
                    trailerFuelRetain.OrderId = item.OrderId;
                    trailerFuelRetain.DeliveryRequestId = item.DeliveryRequestId.ToString();
                    trailerFuelRetain.CreatedDate = item.CreatedOn;
                    trailerFuelRetain.UpdatedOn = item.UpdatedOn;
                    trailerFuelRetain.TfxDriverId = item.TfxDriverId;
                }
            }
            return entity;
        }
    }
}
