using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AssetAdditionalDetailMapper
    {
        public static AssetAdditionalDetailViewModel ToViewModel(this AssetAdditionalDetail entity, AssetAdditionalDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetAdditionalDetailViewModel(Status.Success);

            viewModel.AssetId = entity.AssetId;
            viewModel.Make = entity.Make;
            viewModel.Model = entity.Model;
            viewModel.Year = entity.Year;
            var assetContractNumber = entity.Asset.AssetContractNumbers.FirstOrDefault(t => t.IsActive);
            if (assetContractNumber != null)
            {
                viewModel.Color = assetContractNumber.ContractNumber;
            }
            viewModel.TelematicsProvider = entity.TelematicsProvider;
            viewModel.LicensePlateStateId = entity.LicensePlateStateId;
            viewModel.LicensePlateState = entity.MstState == null ? null : entity.MstState.Code;
            viewModel.LicensePlate = entity.LicensePlate;
            viewModel.Class = entity.AssetClass;
            viewModel.Vendor = entity.Vendor;
            viewModel.VehicleId = entity.VehicleId;
            viewModel.Description = entity.Description;
            viewModel.TankId = entity.VehicleId;
            viewModel.StorageId = entity.Vendor;
            if (entity.FuelCapacity.HasValue)
                viewModel.FuelCapacity = entity.FuelCapacity.Value.GetPreciseValue(2);

            var assetSubcontractor = entity.Asset.AssetSubcontractors.FirstOrDefault(t => t.IsActive);
            if (assetSubcontractor != null)
            {
                viewModel.SubContractorId = assetSubcontractor.SubcontractorId;
            }

            if (entity.DipTestMethod != null && entity.DipTestMethod > 0)
                viewModel.DipTestMethod = (DipTestMethod)entity.DipTestMethod;

            if (entity.TankType != null)
                viewModel.TankType = (TankType)entity.TankType;

            viewModel.WaterLevel = entity.WaterLevel;
            viewModel.ThresholdDeliveryRequest = entity.ThresholdDeliveryRequest;
            viewModel.MinFill = entity.MinFill;
            viewModel.MaxFill = entity.MaxFill;            
            viewModel.IsActive = entity.IsActive;

            viewModel.PedigreeAssetDBID = entity.PedigreeAssetDBId;
            viewModel.SkyBitzRTUID = entity.SkyBitzRTUID;
            if (viewModel.DipTestMethod != null && viewModel.DipTestMethod == DipTestMethod.VeederRoot)
            {
                viewModel.VeederRootTankID = entity.ExternalTankId;
                viewModel.VeederRootIPAddress = entity.VeederRootIPAddress;
                viewModel.Port = entity.Port;
            }
            else if (viewModel.DipTestMethod != null && viewModel.DipTestMethod.Value == DipTestMethod.Insight360)
            {
                viewModel.Insight360TankId = entity.ExternalTankId;
            }
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            return viewModel;
        }

        public static AssetAdditionalDetail ToEntity(this AssetAdditionalDetailViewModel viewModel, AssetAdditionalDetail entity = null, string assetName = "")
        {
            if (entity == null)
                entity = new AssetAdditionalDetail();

            entity.AssetId = viewModel.AssetId;
            entity.Make = viewModel.Make;
            entity.Model = viewModel.Model;
            entity.Year = viewModel.Year;

            entity.TelematicsProvider = viewModel.TelematicsProvider;
            entity.LicensePlate = viewModel.LicensePlate;
            entity.LicensePlateStateId = viewModel.LicensePlateStateId;
            entity.AssetClass = viewModel.Class;
            entity.Description = viewModel.Description;
            entity.FuelCapacity = viewModel.FuelCapacity;

            if (viewModel.DipTestMethod != null && viewModel.DipTestMethod > 0)
                entity.DipTestMethod = (int)viewModel.DipTestMethod;

            if (viewModel.TankType != null)
                entity.TankType = (int)viewModel.TankType;
               
            entity.VehicleId = viewModel.VehicleId;
            entity.Vendor = viewModel.Vendor;

            if (viewModel.Type == (int)AssetType.Tank)
            {
                entity.VehicleId = string.IsNullOrEmpty(viewModel.TankId) ? ApplicationConstants.TankId : viewModel.TankId;
                entity.Vendor = string.IsNullOrEmpty(viewModel.StorageId) ? assetName : viewModel.StorageId;
                entity.PedigreeAssetDBId = viewModel.PedigreeAssetDBID;
                entity.SkyBitzRTUID = viewModel.SkyBitzRTUID;
                if (viewModel.DipTestMethod != null && viewModel.DipTestMethod.Value == DipTestMethod.Insight360)
                {
                    entity.ExternalTankId = viewModel.Insight360TankId;
                }
                else if (viewModel.DipTestMethod != null && viewModel.DipTestMethod.Value == DipTestMethod.VeederRoot)
                {
                    entity.ExternalTankId = viewModel.VeederRootTankID;
                    entity.VeederRootIPAddress = viewModel.VeederRootIPAddress;
                    entity.Port = viewModel.Port;
                }
            }

            entity.ThresholdDeliveryRequest = viewModel.ThresholdDeliveryRequest;
            entity.MinFill = viewModel.MinFill;
            entity.MaxFill = viewModel.MaxFill;
            entity.FillType = (int?)viewModel.FillType;
            entity.WaterLevel = viewModel.WaterLevel;

            entity.Flag = viewModel.Flag;
            entity.IMONumber = viewModel.IMONumber;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            

            return entity;
        }
    }
}
