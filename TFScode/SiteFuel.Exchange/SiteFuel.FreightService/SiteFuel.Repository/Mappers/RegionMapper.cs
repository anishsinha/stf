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
    public static class RegionMapper
    {
        public static Region ToEntity(this RegionViewModel model)
        {
            var entity = new Region();
            entity.Id = string.IsNullOrWhiteSpace(model.Id) ? entity.Id : ObjectId.Parse(model.Id);
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.SlotPeriod = model.SlotPeriod;
            entity.TfxCompanyId = model.CompanyId;

            if(model.Jobs != null)
                entity.TfxJobs = model.Jobs.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            if(model.Drivers != null)
                entity.TfxDrivers = model.Drivers.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            if(model.Dispatchers != null)
                entity.TfxDispatchers = model.Dispatchers.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            if(model.Trailers != null)
                entity.TfxTrailers = model.Trailers.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            if(model.States != null)
                entity.TfxStates = model.States.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem { Code = t.Code, Name = t.Name }).ToList();
            if (model.Carriers != null)
                entity.TfxCarriers = model.Carriers.Select(t => new TfxCarrierDropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name ,SequenceNo=t.SequenceNo,RegionId= t.RegionId }).ToList();
            if (model.ProductTypeIds != null && model.ProductTypeIds.Any())
                entity.TfxProductTypeIds = model.ProductTypeIds;
            if (model.FuelTypeIds != null && model.FuelTypeIds.Any())
                entity.TfxFuelTypeIds = model.FuelTypeIds.Select(t => new MdbDataAccess.Collections.DropdownDisplayItem {Id = t.Id, Code = t.Code, Name = t.Name}).ToList();
            entity.CreatedBy = model.CreatedBy;
            entity.CreatedOn = model.CreatedOn;
            entity.IsActive = model.IsActive;
            entity.IsDeleted = model.IsDeleted;
            entity.TfxFavProductTypeId = model.FavProductTypeId;
            return entity;
        }

        public static RegionViewModel ToModel(this Region entity)
        {
            var model = new RegionViewModel();
            model.Id = entity.Id.ToString();
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.SlotPeriod = entity.SlotPeriod;
            model.CompanyId = entity.TfxCompanyId;
            model.Jobs = entity.TfxJobs.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            model.Drivers = entity.TfxDrivers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            model.Dispatchers = entity.TfxDispatchers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            model.Trailers = entity.TfxTrailers.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code, Name = t.Name }).ToList();
            model.States = entity.TfxStates.Select(t => new FreightModels.DropdownDisplayItem { Code = t.Code, Name = t.Name }).ToList();
            model.ProductTypeIds = entity.TfxProductTypeIds;
            model.FuelTypeIds = entity.TfxFuelTypeIds.Select(t => new FreightModels.DropdownDisplayItem { Id = t.Id, Code = t.Code}).ToList();
            model.CreatedBy = entity.CreatedBy;
            model.CreatedOn = entity.CreatedOn;
            model.IsActive = entity.IsActive;
            model.IsDeleted = entity.IsDeleted;
            model.FavProductTypeId = entity.TfxFavProductTypeId;
            return model;
        }
    }
}
