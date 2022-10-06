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
using TrueFill.DemandCaptureDataAccess.Entities;

namespace SiteFuel.FreightRepository.Mappers
{
    public static class TankMapper
    {
        public static TankDetail ToEntity(this TankDetailsModel model)
        {
            var entity = new TankDetail();
            entity.TfxAssetId = model.AssetId;
            entity.StorageTypeId = string.IsNullOrEmpty(model.TankId) ? ApplicationConstants.TankId : model.TankId;
            entity.StorageId = string.IsNullOrEmpty(model.StorageId) ? model.TankName : model.StorageId;
            entity.TankName = model.TankName;
            entity.TankNumber = model.TankNumber;
            entity.Manufacturer = model.Manufacturer;
            entity.FuelCapacity = model.FuelCapacity;
            entity.ThresholdDeliveryRequest = model.ThresholdDeliveryRequest;
            entity.MinFill = model.MinFill;
            entity.IsStopATGPolling = model.IsStopATGPolling;
            if (model.FillType != null && model.FillType > 0)
                entity.FillType = (int)model.FillType;

            if (model.FillType.HasValue && model.FillType.Value == FillType.Percent)
            {
                entity.MaxFill = model.MaxFillPercent;
                entity.MinFill = model.MinFillPercent;
            }
            else
            {
                entity.MaxFill = model.MaxFill;
                entity.MinFill = model.MinFill;
            }

            entity.PhysicalPumpStop = model.PhysicalPumpStop;
            entity.WaterLevel = model.WaterLevel;
            entity.RunOutLevel = model.RunOutLevel;
            entity.NotificationUponUsageSwing = model.NotificationUponUsageSwing;
            entity.NotificationUponUsageSwingValue = model.NotificationUponUsageSwingValue;
            entity.NotificationUponInventorySwing = model.NotificationUponInventorySwing;
            entity.NotificationUponInventorySwingValue = model.NotificationUponInventorySwingValue;

            if (model.TankType != null)
                entity.TankType = (int)model.TankType;

            if (model.DipTestMethod != null && model.DipTestMethod > 0)
                entity.DipTestMethod = (int)model.DipTestMethod;

            if (model.ManiFolded != null && model.ManiFolded > 0)
                entity.ManiFolded = (int)model.ManiFolded;

            if (model.TankConstruction != null && model.TankConstruction > 0)
                entity.TankConstruction = (int)model.TankConstruction;

            entity.TankAcceptDelivery = model.TankAcceptDelivery;
            entity.TfxProductTypeId = model.FuelTypeId;
            entity.TfxProductTypeName = model.ProductTypeName;
            entity.TankModelTypeId = model.TankModelTypeId;
            entity.TanksConnected = model.TanksConnected;
            entity.TankSequence = model.TankSequence;
            entity.PedigreeAssetDBID = model.PedigreeAssetDBID;
            entity.TfxFuelTypeId = model.TFXFuelTypeId;
            entity.SkyBitzRTUID = model.SkyBitzRTUID;
            entity.ExternalTankId = model.ExternalTankId;
            entity.VeederRootIPAddress = model.VeederRootIPAddress;
            entity.Port = model.Port;
            return entity;
        }

        public static TankDetailsModel ToViewModel(this TankDetail entity)
        {
            var model = new TankDetailsModel();
            model.AssetId = entity.TfxAssetId;
            model.TankId = entity.StorageTypeId;
            model.StorageId = entity.StorageId;
            model.TankName = entity.TankName;
            model.TankNumber = entity.TankNumber;
            model.Manufacturer = entity.Manufacturer;
            model.FuelCapacity = entity.FuelCapacity;
            model.IsStopATGPolling = entity.IsStopATGPolling;

            if (entity.ThresholdDeliveryRequest.HasValue)
                model.ThresholdDeliveryRequest = entity.ThresholdDeliveryRequest.Value.GetPreciseValue(2);
            
            if (entity.FillType.HasValue)
                model.FillType = (FillType)entity.FillType;

            if (entity.FillType.HasValue && entity.MaxFill.HasValue && entity.FillType.Value == (int)FillType.UoM)
            {
                model.MaxFill = entity.MaxFill.Value.GetPreciseValue(2);
                model.MaxFillPercent = (entity.MaxFill.Value * 100 / entity.FuelCapacity.Value).GetPreciseValue(2);
            }
            else if (entity.FillType.HasValue && entity.MaxFill.HasValue && entity.FillType.Value == (int)FillType.Percent)
            {
                model.MaxFillPercent = entity.MaxFill.Value.GetPreciseValue(2);
                model.MaxFill = (entity.FuelCapacity.Value * entity.MaxFill.Value / 100 ).GetPreciseValue(2);
            }

            if (entity.FillType.HasValue && entity.MinFill.HasValue && entity.FillType.Value == (int)FillType.UoM)
            {
                model.MinFill = entity.MinFill.Value.GetPreciseValue(2);
                model.MinFillPercent = (entity.MinFill.Value * 100 / entity.FuelCapacity.Value).GetPreciseValue(2);
            }
            else if (entity.FillType.HasValue && entity.MinFill.HasValue && entity.FillType.Value == (int)FillType.Percent)
            {
                model.MinFillPercent = entity.MinFill.Value.GetPreciseValue(2);
                model.MinFill = (entity.FuelCapacity.Value * entity.MinFill.Value / 100 ).GetPreciseValue(2);
            }

            if (entity.DipTestMethod != null && entity.DipTestMethod > 0)
                model.DipTestMethod = (DipTestMethod)entity.DipTestMethod;

            if (entity.TankType != null)
                model.TankType = (TankType)entity.TankType;

            if (entity.ManiFolded != null && entity.ManiFolded > 0)
                model.ManiFolded = (ManiFolded)entity.ManiFolded;

            if (entity.PhysicalPumpStop.HasValue)
                model.PhysicalPumpStop = entity.PhysicalPumpStop.Value.GetPreciseValue(2);

            if (entity.WaterLevel.HasValue)
                model.WaterLevel = entity.WaterLevel.Value.GetPreciseValue(2);

            if (entity.TankConstruction != null && entity.TankConstruction > 0)
                model.TankConstruction = (TankConstruction)entity.TankConstruction;

            if (entity.RunOutLevel.HasValue)
                model.RunOutLevel = entity.RunOutLevel.Value.GetPreciseValue(2);

            if (entity.NotificationUponUsageSwing.HasValue)
                model.NotificationUponUsageSwing = entity.NotificationUponUsageSwing.Value.GetPreciseValue(2);

            if (entity.NotificationUponUsageSwingValue.HasValue)
                model.NotificationUponUsageSwingValue = entity.NotificationUponUsageSwingValue.Value.GetPreciseValue(2);

            if (entity.NotificationUponInventorySwing.HasValue)
                model.NotificationUponInventorySwing = entity.NotificationUponInventorySwing.Value.GetPreciseValue(2);

            if (entity.NotificationUponInventorySwingValue.HasValue)
                model.NotificationUponInventorySwingValue = entity.NotificationUponInventorySwingValue.Value.GetPreciseValue(2);

            model.TankAcceptDelivery = entity.TankAcceptDelivery;
            model.FuelTypeId = entity.TfxProductTypeId;
            model.ProductTypeName = entity.TfxProductTypeName;
            model.TankModelTypeId = entity.TankModelTypeId;
            model.TanksConnected = entity.TanksConnected;
            model.TankSequence = entity.TankSequence;
            model.PedigreeAssetDBID = entity.PedigreeAssetDBID;
            model.SkyBitzRTUID = entity.SkyBitzRTUID;
            model.ExternalTankId = entity.ExternalTankId;
            model.WaterLevel = entity.WaterLevel;
            model.VeederRootIPAddress = entity.VeederRootIPAddress;
            model.Port = entity.Port;
            return model;
        }

        public static TankDetailListModel ToDetailsViewModel(this TankDetail entity,int jobId, string displayJobId)
        {
            var model = new TankDetailListModel();
            model.AssetId = entity.TfxAssetId;
            model.TankId = entity.StorageTypeId;
            model.StorageId = entity.StorageId;
            model.TankName = entity.TankName;
            model.TankNumber = entity.TankNumber;
            model.Manufacturer = entity.Manufacturer;
            model.FuelCapacity = entity.FuelCapacity;
            model.IsStopATGPolling = entity.IsStopATGPolling;
            if (entity.ThresholdDeliveryRequest.HasValue)
                model.ThresholdDeliveryRequest = entity.ThresholdDeliveryRequest.Value.GetPreciseValue(2);

            if (entity.MinFill.HasValue)
                model.MinFill = entity.MinFill.Value.GetPreciseValue(2);
            if (entity.FillType.HasValue)
                model.FillType = (FillType)entity.FillType;

            if (entity.FillType.HasValue && entity.MaxFill.HasValue && entity.FillType.Value == (int)FillType.UoM)
            {
                model.MaxFill = entity.MaxFill.Value.GetPreciseValue(2);
            }
            else if (entity.FillType.HasValue && entity.MaxFill.HasValue && entity.FillType.Value == (int)FillType.Percent)
            {
                model.MaxFillPercent = entity.MaxFill.Value.GetPreciseValue(2);
            }

            if (entity.FillType.HasValue && entity.MinFill.HasValue && entity.FillType.Value == (int)FillType.UoM)
            {
                model.MinFill = entity.MinFill.Value.GetPreciseValue(2);
            }
            else if (entity.FillType.HasValue && entity.MinFill.HasValue && entity.FillType.Value == (int)FillType.Percent)
            {
                model.MinFillPercent = entity.MinFill.Value.GetPreciseValue(2);
            }

            if (entity.FillType.HasValue && entity.WaterLevel.HasValue && entity.FillType.Value == (int)FillType.UoM)
            {
                model.WaterLevel = entity.WaterLevel.Value.GetPreciseValue(2);
            }
            else if (entity.FillType.HasValue && entity.WaterLevel.HasValue && entity.FillType.Value == (int)FillType.Percent)
            {
                model.WaterLevelPercent = entity.WaterLevel.Value.GetPreciseValue(2);
            }

            if (entity.DipTestMethod != null && entity.DipTestMethod > 0)
                model.DipTestMethod = (DipTestMethod)entity.DipTestMethod;

            if (entity.TankType != null)
                model.TankType = (TankType)entity.TankType;

            if (entity.ManiFolded != null && entity.ManiFolded > 0)
                model.ManiFolded = (ManiFolded)entity.ManiFolded;

            if (entity.PhysicalPumpStop.HasValue)
                model.PhysicalPumpStop = entity.PhysicalPumpStop.Value.GetPreciseValue(2);

            if (entity.TankConstruction != null && entity.TankConstruction > 0)
                model.TankConstruction = (TankConstruction)entity.TankConstruction;

            if (entity.RunOutLevel.HasValue)
                model.RunOutLevel = entity.RunOutLevel.Value.GetPreciseValue(2);

            if (entity.NotificationUponUsageSwing.HasValue)
                model.NotificationUponUsageSwing = entity.NotificationUponUsageSwing.Value.GetPreciseValue(2);

            if (entity.NotificationUponUsageSwingValue.HasValue)
                model.NotificationUponUsageSwingValue = entity.NotificationUponUsageSwingValue.Value.GetPreciseValue(2);

            if (entity.NotificationUponInventorySwing.HasValue)
                model.NotificationUponInventorySwing = entity.NotificationUponInventorySwing.Value.GetPreciseValue(2);

            if (entity.NotificationUponInventorySwingValue.HasValue)
                model.NotificationUponInventorySwingValue = entity.NotificationUponInventorySwingValue.Value.GetPreciseValue(2);

            model.TankAcceptDelivery = entity.TankAcceptDelivery;
            model.FuelTypeId = entity.TfxProductTypeId;
            model.ProductTypeName = entity.TfxProductTypeName;
            model.JobId = jobId;
            model.JobDisplayId = displayJobId;
            model.TankModelTypeId = entity.TankModelTypeId;
            model.TanksConnected = entity.TanksConnected;
            model.TankSequence = entity.TankSequence;
            model.PedigreeAssetDBID = entity.PedigreeAssetDBID;
            model.TfxProductTypeId = entity.TfxProductTypeId;
            model.IsStopATGPolling = entity.IsStopATGPolling;
            return model;
        }

        public static JobTankDetailModel ToJobTankDetailsViewModel(this TankDetail entity, int jobId, string displayJobId)
        {
            var model = new JobTankDetailModel();
            model.AssetId = entity.TfxAssetId;
            model.TankId = entity.StorageTypeId;
            model.StorageId = entity.StorageId;
            model.TankName = entity.TankName;
            model.TankNumber = entity.TankNumber;
            model.ProductTypeId = entity.TfxProductTypeId;
            model.JobId = jobId;
            model.SiteId = displayJobId;
            return model;
        }

        public static List<OrderTankMapping> ToEntity(this List<OrderTankMappingViewModel> model)
        {
            List<OrderTankMapping> entity = new List<OrderTankMapping>();
            foreach (var item in model)
            {
                var mapping = new OrderTankMapping();
                mapping.CreatedBy = item.CreatedBy;
                mapping.FuelTypeId = item.FuelTypeId;
                mapping.IsActive = item.IsActive;
                mapping.JobId = item.JobId;
                mapping.OrderId = item.OrderId;
                mapping.ProductTypeId = item.ProductTypeId;
                mapping.SupplierCompanyId = item.SupplierCompanyId;
                mapping.TankId = item.TankId;
                entity.Add(mapping);
            }
            
            return entity;
        }

        public static JobTankAdditionalDetailModel ToJobTankAdditionalDetailsViewModel(this TankDetail entity, int jobId, string displayJobId,string TfxJobName)
        {
            var model = new JobTankAdditionalDetailModel();
            model.AssetId = entity.TfxAssetId;
            model.TankId = entity.StorageTypeId;
            model.StorageId = entity.StorageId;
            model.TankName = entity.TankName;
            model.TankNumber = entity.TankNumber;
            model.MinFill = entity.MinFill.GetValueOrDefault();
            model.MaxFill = entity.MaxFill.GetValueOrDefault();
            model.RunOut = entity.RunOutLevel.GetValueOrDefault();
            model.ProductTypeId = entity.TfxProductTypeId;
            model.TfxProductTypeName = entity.TfxProductTypeName;
            model.JobId = jobId;
            model.JobName = TfxJobName;
            model.SiteId = displayJobId;
            model.FillType = entity.FillType.GetValueOrDefault();
            model.FuelCapacity = entity.FuelCapacity.GetValueOrDefault();
            return model;
        }
    }
}
