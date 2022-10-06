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
    public static class TruckMapper
    {
        public static TruckDetail ToEntity(this TruckDetailViewModel model)
        {
            var entity = new TruckDetail();
            if (model != null)
            {
                entity.Name = model.Name;
                entity.Owner = model.Owner;
                entity.ContractNumber = model.ContractNumber;
                entity.TrailerType = model.TrailerType;
                entity.TfxCreatedBy = model.TfxCreatedBy;
                entity.CreatedDate = model.CreatedDate;
                entity.FuelCapacity = model.FuelCapacity;
                entity.OptimizedCapacity = model.OptimizedCapacity;
                entity.IsDeleted = model.IsDeleted;
                entity.TruckId = model.TruckId;
                entity.TfxCompanyId = model.TfxCompanyId;
                entity.Status = model.Status;
                entity.LicenceRequirement = model.LicenceRequirement;
                entity.LicencePlate = model.LicencePlate;
                entity.ExpirationDate = model.ExpirationDate;
                entity.IsPump = model.IsPump;
                entity.Compartments = new List<TruckCompartment>();
                if (model.Compartments != null)
                    model.Compartments.ForEach(x => {
                        entity.Compartments.Add(new TruckCompartment { Capacity = x.Capacity, CompartmentId = x.CompartmentId,FuelType=x.FuelType, PumpId = x.PumpId});
                    });

                entity.TfxUpdatedBy = model.TfxCreatedBy;
                entity.IsFilldCompatible = model.IsFilldCompatible;
                entity.SmartDeviceId = model.SmartDeviceId;
            }
            return entity;
        }

        public static TruckDetailViewModel ToViewModel(this TruckDetail entity)
        {
            var viewModel = new TruckDetailViewModel();
            if(entity != null)
            {
                viewModel.Id = entity.Id.ToString();
                viewModel.Name = entity.Name;
                viewModel.Owner = entity.Owner;
                viewModel.ContractNumber = entity.ContractNumber;
                viewModel.TrailerType = entity.TrailerType;
                viewModel.TfxCreatedBy = entity.TfxCreatedBy;
                viewModel.CreatedDate = entity.CreatedDate;
                viewModel.FuelCapacity = entity.FuelCapacity;
                viewModel.OptimizedCapacity = entity.OptimizedCapacity;
                viewModel.IsDeleted = entity.IsDeleted;
                viewModel.TruckId = entity.TruckId;
                viewModel.TfxCompanyId = entity.TfxCompanyId;
                viewModel.Status = entity.Status;
                viewModel.LicenceRequirement = entity.LicenceRequirement;
                viewModel.LicencePlate = entity.LicencePlate;
                viewModel.ExpirationDate = entity.ExpirationDate;
                viewModel.IsPump = entity.IsPump;
                viewModel.Compartments = new List<Compartment>();
                if (entity.Compartments != null)
                    entity.Compartments.ForEach(x => {
                        viewModel.Compartments.Add(new Compartment { Capacity = x.Capacity, CompartmentId = x.CompartmentId,FuelType=x.FuelType, PumpId = x.PumpId });
                    });
                viewModel.TrailerFuelRetains = new List<TrailerFuelRetainViewModel>();
                viewModel.IsFilldCompatible = entity.IsFilldCompatible;
                viewModel.SmartDeviceId = entity.SmartDeviceId;
            }
            return viewModel;
        }

        public static ExternalVehicleMappings ToEntity(this ExternalVehicleMappingViewModel model)
        {
            var entity = new ExternalVehicleMappings();
            if (model != null)
            {
                entity.TruckId = model.TruckId;
                entity.ThirdPartyId = model.ThirdPartyId;
                entity.TargetVehicleValue = model.TargetVehicleValue;
                entity.IsActive = true;            
            }
            return entity;
        }

    }
}
