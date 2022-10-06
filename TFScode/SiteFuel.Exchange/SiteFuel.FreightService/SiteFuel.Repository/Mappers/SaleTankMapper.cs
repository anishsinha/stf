using SiteFuel.FreightModels.ForcastingHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class SaleTankMapper
    {
        public static SaleTank ToEntity(this SaleTankModel model)
        {
            var entity = new SaleTank();
            entity.Id = model.Id;
            entity.SiteId = model.SiteId;
            entity.TankId = model.TankId;
            entity.StorageId = model.StorageId;
            entity.BandPeriod = model.BandPeriod;
            entity.DayStartOn = model.StartTime;
            entity.MaxFill = model.MaxFill;
            entity.FillType = model.FillType;
            entity.FuelCapacity = model.FuelCapacity;
            entity.Retain = model.Retain;
            entity.SaftyStock = model.SaftyStock;
            entity.Runout = model.Runout;
            entity.InventoryUoM = model.InventoryUoM;
            return entity;
        }

        public static SaleTankModel ToViewModel(this SaleTank entity)
        {
            var model = new SaleTankModel();
            model.Id = entity.Id;
            model.SiteId = entity.SiteId;
            model.TankId = entity.TankId;
            model.StorageId = entity.StorageId;
            model.BandPeriod = entity.BandPeriod;
            model.StartTime = entity.DayStartOn;
            model.MaxFill = entity.MaxFill;
            model.FillType = entity.FillType;
            model.FuelCapacity = entity.FuelCapacity;
            model.Retain = entity.Retain;
            model.SaftyStock = entity.SaftyStock;
            model.Runout = entity.Runout;
            model.InventoryUoM = entity.InventoryUoM;
            return model;
        }
    }
}
