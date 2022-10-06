using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AssetMapper
    {
        public static AssetViewModel ToViewModel(this Asset entity, AssetViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetViewModel();

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.FuelType = entity.MstProductType == null ? new AssetFuelTypeViewModel() : new AssetFuelTypeViewModel() { Id = entity.MstProductType.Id, Name = entity.MstProductType.Name };
            viewModel.Image = entity.Image == null ? new ImageViewModel() : entity.Image.ToViewModel();
            viewModel.Image.FilePath = viewModel.Image == null ? string.Empty : viewModel.Image.GetAzureFilePath(BlobContainerType.JobFilesUpload);
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.AssetAdditionalDetail = entity.AssetAdditionalDetail.ToViewModel();
            var jobXAsset = entity.JobXAssets.OrderByDescending(t => t.Id).FirstOrDefault(t => t.RemovedBy == null && t.RemovedDate == null);
            if (jobXAsset != null)
            {
                viewModel.JobId = jobXAsset.JobId;
            }
            viewModel.CompanyId = entity.Company.Id;
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.IsMarine = entity.IsMarine;
            viewModel.AssetTankFuelTypeId = entity.FuelTypeId;
            return viewModel;
        }

        public static AssetDropModel ToViewModel(this AssetDrop entity)
        {
            var assetDropModel = new AssetDropModel();
            assetDropModel.AssetName = entity.JobXAsset.Asset.Name;
            assetDropModel.JobXAssetId = entity.JobXAssetId;
            assetDropModel.OrderId = entity.OrderId;
            //assetDropModel.InvoiceId
            assetDropModel.MeterStartReading = entity.MeterStartReading;
            assetDropModel.MeterEndReading = entity.MeterEndReading;
            assetDropModel.DropGallons = entity.DroppedGallons;
            assetDropModel.DropStartDate = entity.DropStartDate;
            assetDropModel.DropEndDate = entity.DropEndDate;
            assetDropModel.DroppedBy = entity.DroppedBy;
            assetDropModel.ImageId = entity.ImageId;
            assetDropModel.SubcontractorName = entity.SubcontractorName;
            assetDropModel.SubcontractorId = entity.SubcontractorId;
            assetDropModel.ContractNumber = entity.ContractNumber;
            assetDropModel.DropStatus = entity.DropStatus;
            assetDropModel.IsNewAsset = entity.IsNewAsset;
            assetDropModel.PreDip = entity.PreDip;
            assetDropModel.PostDip = entity.PostDip;
            assetDropModel.IsActive = entity.IsActive;
            assetDropModel.UpdatedBy = entity.UpdatedBy;
            assetDropModel.UpdatedDate = entity.UpdatedDate;
            assetDropModel.Gravity = entity.Gravity;
            
            return assetDropModel;
        }

        public static ApiTankDetailViewModel ToApiTankViewModel(this TankDetailViewModel viewModel, string UoM)
        {
            var apiTankViewModel = new ApiTankDetailViewModel();
            apiTankViewModel.Id = viewModel.AssetId;
            apiTankViewModel.TankId = viewModel.TankId;
            apiTankViewModel.StorageId = viewModel.StorageId;
            apiTankViewModel.TankName = viewModel.TankName;
            apiTankViewModel.TankNumber = viewModel.TankNumber;
            apiTankViewModel.LastReading = viewModel.LastReading;
            apiTankViewModel.CaptureTime = viewModel.CaptureTime;
            apiTankViewModel.JobId = viewModel.JobId;
            apiTankViewModel.SiteId = viewModel.JobDisplayId;
            apiTankViewModel.ProducTypeId = viewModel.FuelTypeId;
            apiTankViewModel.TankType = viewModel.TankType;
            apiTankViewModel.DipTestMethod = viewModel.DipTestMethod;
            apiTankViewModel.FuelCapacity = viewModel.FuelCapacity;
            apiTankViewModel.ThresholdDeliveryRequest = viewModel.ThresholdDeliveryRequest;
            apiTankViewModel.FillType = viewModel.FillType;
            apiTankViewModel.UoM = UoM;
            apiTankViewModel.Ullage = viewModel.Ullage;
            apiTankViewModel.Priority = viewModel.Priority;

            if (viewModel.FillType == FillType.UoM)
            {
                apiTankViewModel.MinFill = viewModel.MinFill;
                apiTankViewModel.MaxFill = viewModel.MaxFill;
                apiTankViewModel.MinFillPercent = (viewModel.MinFill * 100) / apiTankViewModel.FuelCapacity;
                apiTankViewModel.MaxFillPercent = (viewModel.MaxFill * 100) / apiTankViewModel.FuelCapacity;
            }
            else if (viewModel.FillType == FillType.Percent)
            {
                apiTankViewModel.MinFill = (viewModel.MinFillPercent * apiTankViewModel.FuelCapacity) / 100;
                apiTankViewModel.MaxFill = (viewModel.MaxFillPercent * apiTankViewModel.FuelCapacity) / 100;
                apiTankViewModel.MinFillPercent = viewModel.MinFillPercent;
                apiTankViewModel.MaxFillPercent = viewModel.MaxFillPercent;
            }

            apiTankViewModel.TankModelTypeId = viewModel.TankModelTypeId;
            apiTankViewModel.TankChart = viewModel.TankChart;
            apiTankViewModel.TankChartFileName = viewModel.TankChartFileName;
            apiTankViewModel.TankMake = viewModel.TankMake;
            apiTankViewModel.TankModel = viewModel.TankModel;
            apiTankViewModel.ScaleMeasurement = viewModel.ScaleMeasurement;
            apiTankViewModel.PhysicalPumpStop = viewModel.PhysicalPumpStop;
            apiTankViewModel.RunOutLevel = viewModel.RunOutLevel;
            apiTankViewModel.ProducType = viewModel.ProductTypeName;
            if (!string.IsNullOrEmpty(viewModel.TankAcceptDelivery))
                apiTankViewModel.TankAcceptDelivery = viewModel.TankAcceptDelivery.Split(',').Select(x => Convert.ToInt32(x)).Distinct().ToList();

            apiTankViewModel.TanksConnected = viewModel.TanksConnected;
            if (viewModel.TanksConnected != null && viewModel.TanksConnected.Any())
            {
                apiTankViewModel.IsTankConnected = true;
            }
            return apiTankViewModel;
        }

        public static AssetDrop ToEntity(this AssetDropModel viewModel)
        {
            var entity = new AssetDrop
            {
                JobXAssetId = viewModel.JobXAssetId,
                OrderId = viewModel.OrderId,
                //entity.InvoiceId
                MeterStartReading = viewModel.MeterStartReading,
                MeterEndReading = viewModel.MeterEndReading,
                DroppedGallons = viewModel.DropGallons ?? 0,
                DropStartDate = viewModel.DropStartDate,
                DropEndDate = viewModel.DropEndDate,
                DroppedBy = viewModel.DroppedBy,
                ImageId = viewModel.ImageId,
                SubcontractorName = viewModel.SubcontractorName,
                SubcontractorId = viewModel.SubcontractorId,
                ContractNumber = viewModel.ContractNumber,
                DropStatus = viewModel.DropStatus,
                IsActive = viewModel.IsActive,
                UpdatedBy = viewModel.UpdatedBy,
                UpdatedDate = viewModel.UpdatedDate,
                IsNewAsset = viewModel.IsNewAsset,
                PreDip = viewModel.PreDip.HasValue && viewModel.PreDip.Value > 0 ? viewModel.PreDip : null,
                PostDip = viewModel.PostDip.HasValue && viewModel.PostDip.Value > 0 ? viewModel.PostDip : null,
                TankScaleMeasurement = viewModel.TankScaleMeasurement,
                Image = viewModel.Image?.ToEntity(),
                Gravity = viewModel.Gravity
            };
            return entity;
        }

        public static Asset ToEntity(this AssetViewModel viewModel, Asset entity = null)
        {
            if (entity == null)
                entity = new Asset();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name.Trim();
            entity.FuelType = viewModel.FuelType == null ? (int?)null : viewModel.FuelType.Id;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.Type = viewModel.Type;
            entity.Image = viewModel.Image == null || string.IsNullOrWhiteSpace(viewModel.Image?.FilePath) || viewModel.Image.IsRemoved ? null : viewModel.Image.ToEntity();
            viewModel.AssetAdditionalDetail.UpdatedBy = viewModel.UserId;
            viewModel.AssetAdditionalDetail.Type = viewModel.Type;
            entity.AssetAdditionalDetail = viewModel.AssetAdditionalDetail.ToEntity(entity.AssetAdditionalDetail, viewModel.Name);
            if (!string.IsNullOrWhiteSpace(viewModel.AssetAdditionalDetail.Color))
            {
                entity.AssetContractNumbers.Where(t => t.IsActive).ToList().ForEach(t => { t.RemovedBy = viewModel.UserId; t.RemovedDate = DateTimeOffset.Now; t.IsActive = false; });
                AssetContractNumber assetContractNumber = new AssetContractNumber() { ContractNumber = viewModel.AssetAdditionalDetail.Color, AddedBy = viewModel.UserId, AddedDate = DateTimeOffset.Now, IsActive = true };
                entity.AssetContractNumbers.Add(assetContractNumber);
            }

            if (viewModel.Type == (int)AssetType.Asset)
            {
                if (viewModel.AssetAdditionalDetail.SubContractorId != null && viewModel.AssetAdditionalDetail.SubContractorId > 0)
                {
                    entity.AssetSubcontractors.Where(t => !(t.SubcontractorId == viewModel.AssetAdditionalDetail.SubContractorId && t.IsActive) && t.IsActive).ToList().ForEach(t => { t.RemovedBy = viewModel.UserId; t.RemovedDate = DateTimeOffset.Now; t.IsActive = false; });
                    if (!entity.AssetSubcontractors.Any(t => t.SubcontractorId == viewModel.AssetAdditionalDetail.SubContractorId && t.IsActive))
                    {
                        AssetSubcontractor assetSubcontractor = new AssetSubcontractor() { SubcontractorId = viewModel.AssetAdditionalDetail.SubContractorId.Value, AssignedBy = viewModel.UserId, AssignedDate = DateTimeOffset.Now, IsActive = true };
                        entity.AssetSubcontractors.Add(assetSubcontractor);
                    }
                }
                else if (entity.AssetSubcontractors.Any(t => t.IsActive))
                {
                    entity.AssetSubcontractors.Where(t => t.IsActive).ToList().ForEach(t => { t.RemovedBy = viewModel.UserId; t.RemovedDate = DateTimeOffset.Now; t.IsActive = false; });
                }
            }

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UserId;
            entity.IsMarine = viewModel.IsMarine;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.FuelTypeId = viewModel.AssetTankFuelTypeId;
            return entity;
        }

        public static AssetDropHistoryViewModel ToAssetDropViewModel(this AssetDrop entity, AssetDropHistoryViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetDropHistoryViewModel();

            viewModel.DropDate = entity.DropEndDate.ToString(Resource.constFormatDate);
            viewModel.DropAmount = entity.DroppedGallons.GetPreciseValue(6);
            viewModel.DropTime = entity.DropEndDate.DateTime.ToShortTimeString();
            viewModel.SubcontractorName = entity.SubcontractorName ?? Resource.lblHyphen;
            viewModel.TimeZoneName = entity.JobXAsset.Job.TimeZoneName;
            return viewModel;
        }

        public static AssetDropModel ToAssetDropModel(this AssetDropViewModel entity, TimeSpan offset)
        {
            var assetDropModel = new AssetDropModel();
            assetDropModel.AssetName = entity.AssetName;
            assetDropModel.JobXAssetId = entity.JobXAssetId;
            assetDropModel.OrderId = entity.OrderId;
            //assetDropModel.InvoiceId
            assetDropModel.MeterStartReading = entity.MeterStartReading;
            assetDropModel.MeterEndReading = entity.MeterEndReading;
            assetDropModel.DropGallons = entity.DropGallons;
            assetDropModel.DropStartDate = entity.DropDate.Date.Add(Convert.ToDateTime(entity.StartTime).TimeOfDay);
            var dropEndDate = entity.DropEndDate.HasValue ? entity.DropEndDate.Value.Date : entity.DropEndDate;
            if ((Convert.ToDateTime(entity.EndTime).TimeOfDay < Convert.ToDateTime(entity.StartTime).TimeOfDay)
                && assetDropModel.DropStartDate.Date <= dropEndDate)
                assetDropModel.DropEndDate = entity.DropDate.Date.AddDays(1).Add(Convert.ToDateTime(entity.EndTime).TimeOfDay);
            else
                assetDropModel.DropEndDate = entity.DropDate.Date.Add(Convert.ToDateTime(entity.EndTime).TimeOfDay);
            assetDropModel.DropStartDate = assetDropModel.DropStartDate.AttachOffset(offset);
            assetDropModel.DropEndDate = assetDropModel.DropEndDate.AttachOffset(offset);
            assetDropModel.DroppedBy = entity.DroppedBy;
            assetDropModel.ImageId = entity.Image == null || entity.Image.Id == 0 ? (int?)null : entity.Image.Id;
            assetDropModel.Image = entity.Image == null || entity.Image.Data == null || entity.Image.Id > 0 ? null : entity.Image;
            assetDropModel.SubcontractorName = entity.SubcontractorName;
            assetDropModel.SubcontractorId = entity.SubcontractorId;
            assetDropModel.ContractNumber = entity.ContractNumber;
            assetDropModel.DropStatus = entity.DropStatusId;
            assetDropModel.IsNewAsset = entity.IsNewAsset;
            assetDropModel.IsActive = entity.IsActive;
            assetDropModel.UpdatedBy = entity.UpdatedBy;
            assetDropModel.UpdatedDate = entity.UpdatedDate;

            assetDropModel.PreDip = entity.PreDip;
            assetDropModel.PostDip = entity.PostDip;
            assetDropModel.TankScaleMeasurement = entity.TankScaleMeasurement;
            assetDropModel.Gravity = entity.Gravity;
            return assetDropModel;
        }

        public static TankInventoryDataCaptureResponseModel ToSalesViewModel(this SalesDataModel entity, TankInventoryDataCaptureResponseModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TankInventoryDataCaptureResponseModel();

            viewModel.TankName = entity.TankName;
            viewModel.SiteId = entity.SiteId;
            viewModel.TankId = entity.TankId;
            viewModel.StorageId = entity.StorageId;
            viewModel.Customer = entity.CompanyName;
            viewModel.TankCapacity = entity.TankCapacity == null ? "0.00" : entity.TankCapacity.Value.ToString("0.00");
            viewModel.MinFill = entity.MinFillQuantity == null ? "0.00" : entity.MinFillQuantity.Value.ToString("0.00");
            viewModel.MaxFill = entity.MaxFillQuantity == null ? "0.00" : entity.MaxFillQuantity.Value.ToString("0.00");
            viewModel.PrevSale = entity.PrevSale?.Replace(",", "");
            viewModel.Inventory = entity.Inventory?.Replace(",", "");
            viewModel.Ullage = entity.Ullage?.Replace(",", "");
            viewModel.AvgSale = entity.AvgSale?.Replace(",", "");
            viewModel.WeekAgoSale = entity.WeekAgoSale?.Replace(",", "");
            viewModel.DaysRemaining = entity.DaysRemaining;
            viewModel.LastDeliveredQuantity = entity.LastDeliveredQuantity;
            viewModel.LastReadingTime = entity.LastReadingTime;
            viewModel.LastDeliveryDate = entity.LastDeliveryDate;
            viewModel.JobId = entity.TfxJobId;
            viewModel.ProductTypeId = entity.ProductTypeId;
            viewModel.WaterLevel = entity.WaterLevel.ToString("0.00");

            return viewModel;
        }
    }
}
