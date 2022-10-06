using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AssetDuplicateMapper
    {
        public static AssetDuplicateViewModel ToViewModel(this AssetDuplicate entity, AssetDuplicateViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetDuplicateViewModel();

            viewModel.Id = entity.Id;
            viewModel.CompanyId = entity.Company.Id;
            viewModel.Name = entity.Name;
            viewModel.Make = entity.Make;
            viewModel.Model = entity.Model;
            viewModel.Year = entity.Year;
            viewModel.Color = entity.Color;
            viewModel.Subcontractor = entity.Subcontractor;
            viewModel.FuelType = entity.FuelType;
            viewModel.FuelCapacity = entity.FuelCapacity;
            viewModel.VehicleId = entity.VehicleId;
            viewModel.TelematicsProvider = entity.TelematicsProvider;
            viewModel.LicensePlateState = entity.LicensePlateState;
            viewModel.LicensePlate = entity.LicensePlate;
            viewModel.AssetClass = entity.AssetClass;
            viewModel.Vendor = entity.Vendor;
            viewModel.Description = entity.Description;
            viewModel.DateAdded = entity.DateAdded;

            return viewModel;
        }

        public static AssetDuplicate ToEntity(this AssetDuplicateViewModel viewModel, AssetDuplicate entity = null)
        {
            if (entity == null)
                entity = new AssetDuplicate();

            entity.Id = viewModel.Id;
            entity.CompanyId = viewModel.CompanyId;
            entity.Name = viewModel.Name;
            entity.Make = viewModel.Make;
            entity.Model = viewModel.Model;
            entity.Year = viewModel.Year;
            entity.Color = viewModel.Color;
            entity.Subcontractor = viewModel.Subcontractor;
            entity.FuelType = viewModel.FuelType;
            entity.FuelCapacity = viewModel.FuelCapacity;
            entity.VehicleId = viewModel.VehicleId;
            entity.TelematicsProvider = viewModel.TelematicsProvider;
            entity.LicensePlateState = viewModel.LicensePlateState;
            entity.LicensePlate = viewModel.LicensePlate;
            entity.AssetClass = viewModel.AssetClass;
            entity.Vendor = viewModel.Vendor;
            entity.Description = viewModel.Description;
            entity.DateAdded = viewModel.DateAdded;

            return entity;
        }

        public static AssetDuplicate ToAssetDuplicate(this Asset entity, AssetDuplicate viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new AssetDuplicate();
                viewModel.Id = entity.Id;
                viewModel.CompanyId = entity.Company == null ? 0 : entity.Company.Id;
            }

            viewModel.Name = entity.Name;
            viewModel.Make = entity.AssetAdditionalDetail.Make;
            viewModel.Model = entity.AssetAdditionalDetail.Model;
            viewModel.Year = entity.AssetAdditionalDetail.Year;
            var assetContractNumber = entity.AssetContractNumbers.FirstOrDefault(t => t.IsActive);
            if (assetContractNumber != null)
            {
                viewModel.Color = assetContractNumber.ContractNumber;
            }

            var subcontractor = entity.AssetSubcontractors.FirstOrDefault(t => t.IsActive);
            if (subcontractor != null)
            {
                viewModel.Subcontractor = subcontractor.Subcontractor.Name;
            }
            if (entity.MstProductType != null)
                viewModel.FuelType = entity.MstProductType.Name;
            viewModel.FuelCapacity = entity.AssetAdditionalDetail.FuelCapacity;
            viewModel.VehicleId = entity.AssetAdditionalDetail.VehicleId;
            viewModel.TelematicsProvider = entity.AssetAdditionalDetail.TelematicsProvider;
            if (entity.AssetAdditionalDetail.MstState != null)
                viewModel.LicensePlateState = entity.AssetAdditionalDetail.MstState.Code;
            viewModel.LicensePlate = entity.AssetAdditionalDetail.LicensePlate;
            viewModel.AssetClass = entity.AssetAdditionalDetail.AssetClass;
            viewModel.Vendor = entity.AssetAdditionalDetail.Vendor;
            viewModel.Description = entity.AssetAdditionalDetail.Description;
            viewModel.DateAdded = entity.CreatedDate;

            return viewModel;
        }

        public static Asset ToAssetEntity(this AssetDuplicate viewModel, Asset entity = null)
        {
            if (entity == null)
            {
                entity = new Asset();
                entity.AssetAdditionalDetail = new AssetAdditionalDetail();
            }

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.AssetAdditionalDetail.Make = viewModel.Make;
            entity.AssetAdditionalDetail.Model = viewModel.Model;
            entity.AssetAdditionalDetail.Year = viewModel.Year;
            entity.AssetAdditionalDetail.FuelCapacity = viewModel.FuelCapacity;
            entity.AssetAdditionalDetail.VehicleId = viewModel.VehicleId;
            entity.AssetAdditionalDetail.TelematicsProvider = viewModel.TelematicsProvider;
            entity.AssetAdditionalDetail.LicensePlate = viewModel.LicensePlate;
            entity.AssetAdditionalDetail.AssetClass = viewModel.AssetClass;
            entity.AssetAdditionalDetail.Vendor = viewModel.Vendor;
            entity.AssetAdditionalDetail.Description = viewModel.Description;
            entity.CreatedDate = viewModel.DateAdded;
            return entity;
        }
    }
}
