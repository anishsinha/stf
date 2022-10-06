using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class TractorMapper
    {
        public static TractorDetail ToEntity(this TractorDetailViewModel model)
        {
            var entity = new TractorDetail();
            if (model != null)
            {
                entity.TractorId = model.TractorId;
                entity.VIN = model.VIN;
                entity.Plate = model.Plate;
                entity.ExpirationDate = model.ExpirationDate;
                entity.TrailerType = new List<TrailerTypeStatus>();
                if (model.TrailerType != null && model.TrailerType.Count() > 0)
                    model.TrailerType.ForEach(x =>
                    {
                        entity.TrailerType.Add(x);
                    });
                entity.Owner = model.Owner;
                entity.Drivers = new List<MdbDataAccess.Collections.DriverDetails>();
                if (model.Drivers != null && model.Drivers.Count() > 0)
                    model.Drivers.ForEach(x =>
                    {
                        entity.Drivers.Add(new MdbDataAccess.Collections.DriverDetails { TfxId = x.Id, TfxName = x.Name });
                    });
                entity.CreatedDate = DateTime.Now;
                entity.IsDeleted = model.IsDeleted;
                entity.TfxCompanyId = model.TfxCompanyId;
                entity.TfxCreatedBy = model.TfxCreatedBy;
                entity.Status = model.Status;

            }
            return entity;
        }
        public static TractorDetailViewModel ToViewModel(this TractorDetail entity)
        {
            var viewModel = new TractorDetailViewModel();
            if (entity != null)
            {
                viewModel.Id = entity.Id.ToString();
                viewModel.TractorId = entity.TractorId;
                viewModel.VIN = entity.VIN;
                viewModel.Plate = entity.Plate;
                viewModel.ExpirationDate = entity.ExpirationDate;
                viewModel.TrailerType = new List<TrailerTypeStatus>();
                if (entity.TrailerType != null && entity.TrailerType.Count() > 0)
                    entity.TrailerType.ForEach(x =>
                    {
                        viewModel.TrailerType.Add(x);
                    });
                viewModel.Owner = entity.Owner;
                viewModel.Drivers = new List<FreightModels.DriverDetails>();
                if (entity.Drivers != null && entity.Drivers.Count() > 0)
                    entity.Drivers.ForEach(x =>
                    {
                        viewModel.Drivers.Add(new FreightModels.DriverDetails { Id=x.TfxId,Name=x.TfxName });
                    });
                viewModel.TfxCreatedBy = entity.TfxCreatedBy;
                viewModel.CreatedDate = entity.CreatedDate;
                viewModel.IsDeleted = entity.IsDeleted;
                viewModel.TfxCompanyId = entity.TfxCompanyId;
                viewModel.Status = entity.Status;
            }
            return viewModel;
        }
    }
}
