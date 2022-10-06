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
    public static class FreightTableMapper
    {
        public static FreightTable ToEntity(this FreightTableModel model)
        {
            var entity = new FreightTable();
            entity.CompanyId = model.CompanyId;
            entity.CreatedOn = model.CreatedOn;
            entity.Description = model.Description;
            entity.EndDate = model.EndDate;
            entity.FuelType = model.FuelType;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.Name = model.Name;
            entity.StartDate = model.StartDate;
            entity.Type = (int)model.Type;
            return entity;
        }

        public static FreightTablePrice ToEntity(this FreightTablePriceModel model)
        {
            var entity = new FreightTablePrice();
            entity.CompanyId = model.CompanyId;
            entity.FreightPrices = model.FreightPrices.Select(t => t.ToEntity()).ToList();
            entity.FreightTableId = new ObjectId(model.FreightTableId);
            return entity;
        }

        public static PointToPointPrice ToEntity(this PointToPointPriceModel model)
        {
            var entity = new PointToPointPrice();
            entity.CompanyId = model.CompanyId;
            entity.CreatedOn = model.CreatedOn;
            entity.Currency = (int)model.Currency;
            entity.EndPoint = model.EndPoint;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.Rate = model.Rate;
            entity.StartPoint = model.StartPoint;
            return entity;
        }

        public static DistanceRangePrice ToEntity(this DistanceRangePriceModel model)
        {
            var entity = new DistanceRangePrice();
            entity.CompanyId = model.CompanyId;
            entity.CreatedOn = model.CreatedOn;
            entity.Currency = (int)model.Currency;
            entity.MaxDistance = model.MaxDistance;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.Rate = model.Rate;
            entity.MinDistance = model.MinDistance;
            return entity;
        }

        public static Price ToEntity(this PriceModel model)
        {
            Price price = null;
            switch (model.Type)
            {
                case PriceType.PointToPoint:
                    price = ((PointToPointPriceModel)model).ToEntity();
                    break;
                case PriceType.DistanceRange:
                    price = ((DistanceRangePriceModel)model).ToEntity();
                    break;
                case PriceType.QuantityRange:
                    break;
                case PriceType.PerGallonOrLitres:
                    break;
                case PriceType.Route:
                    break;
                default:
                    break;
            }
            return price;
        }
    }
}
