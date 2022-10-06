using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Invoice.Pdf;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AssetDropMapper
    {
        public static AssetDrop ToEntity(this AssetDropViewModel viewModel, AssetDrop entity = null)
        {
            if (entity == null)
            {
                entity = new AssetDrop();
            }

            entity.Id = viewModel.Id;
            entity.JobXAssetId = viewModel.JobXAssetId;
            entity.OrderId = viewModel.OrderId;
            entity.InvoiceId = viewModel.InvoiceId;
            entity.DroppedGallons = viewModel.DropGallons ?? 0;
            entity.DropEndDate = viewModel.DropDate.Date.Add(Convert.ToDateTime(viewModel.EndTime).TimeOfDay);
            entity.DropStartDate = viewModel.DropDate.Date.Add(Convert.ToDateTime(viewModel.StartTime).TimeOfDay);
            entity.MeterStartReading = viewModel.MeterStartReading;
            entity.MeterEndReading = viewModel.MeterEndReading;
            entity.DroppedBy = viewModel.DroppedBy;
            entity.DropStatus = viewModel.DropStatusId;
            entity.IsNewAsset = viewModel.IsNewAsset;
            if (viewModel.Image != null && viewModel.Image.Id > 0)
            {
                entity.ImageId = viewModel.Image.Id;
            }
            else if (viewModel.Image != null && !string.IsNullOrWhiteSpace(viewModel.Image?.FilePath))
            {
                entity.Image = viewModel.Image.ToEntity(entity.Image);
            }

            entity.SubcontractorId = viewModel.SubcontractorId;
            entity.SubcontractorName = viewModel.SubcontractorName;
            entity.ContractNumber = viewModel.ContractNumber;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.PreDip = (viewModel.PreDip != null && viewModel.PreDip > 0) ? viewModel.PreDip : null;
            entity.PostDip = (viewModel.PostDip != null && viewModel.PostDip > 0) ? viewModel.PostDip : null;
            return entity;
        }

        public static AssetDropViewModel ToViewModel(this UspInvoicePdfAssetDrop entity, AssetDropViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetDropViewModel();

            viewModel.AssetName = entity.AssetName;
            viewModel.JobXAssetId = entity.JobXAssetId;
            viewModel.DropDate = entity.DropStartDate;
            viewModel.DropEndDate = entity.DropEndDate;
            viewModel.EndTime = entity.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2);
            viewModel.DropGallons = entity.DroppedGallons;
            viewModel.PricePerGallon = entity.PricePerGallon;
            viewModel.SubcontractorName = entity.SubcontractorName;
            viewModel.DropStatusId = entity.DropStatus;
            viewModel.IsNewAsset = entity.IsNewAsset;
            viewModel.OrderId = entity.OrderId;
            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.SpillNotes = entity.SpillNotes;
            viewModel.PreDip = entity.PreDip;
            viewModel.PostDip = entity.PostDip;
            viewModel.TankScaleMeasurement = (Utilities.TankScaleMeasurement)entity.TankScaleMeasurement;
            viewModel.IMONumber = entity.IMONumber;
            viewModel.DeliveryLevelPO = entity.DeliveryLevelPO;
            return viewModel;
        }

        public static AssetDropViewModel ToDropViewModel(this AssetDrop entity)
        {
            var assetDropModel = new AssetDropViewModel();
            assetDropModel.JobXAssetId = entity.JobXAssetId;
            assetDropModel.OrderId = entity.OrderId;
            //assetDropModel.InvoiceId
            assetDropModel.MeterStartReading = entity.MeterStartReading;
            assetDropModel.MeterEndReading = entity.MeterEndReading;
            assetDropModel.DropGallons = entity.DroppedGallons;
            assetDropModel.DropDate = entity.DropStartDate.Date;
            assetDropModel.StartTime = entity.DropStartDate.DateTime.ToString(Resource.constFormat12HourTime2);
            assetDropModel.EndTime = entity.DropEndDate.DateTime.ToString(Resource.constFormat12HourTime2);
            assetDropModel.DroppedBy = entity.DroppedBy;
            if (entity.Image != null)
            {
                assetDropModel.Image = new ImageViewModel() { Id = entity.ImageId.Value };
            }
            assetDropModel.SubcontractorName = entity.SubcontractorName;
            assetDropModel.SubcontractorId = entity.SubcontractorId;
            assetDropModel.ContractNumber = entity.ContractNumber;
            assetDropModel.DropStatusId = entity.DropStatus;
            assetDropModel.IsNewAsset = entity.IsNewAsset;
            assetDropModel.PreDip = entity.PreDip;
            assetDropModel.PostDip = entity.PostDip;
            assetDropModel.IsActive = entity.IsActive;
            assetDropModel.UpdatedBy = entity.UpdatedBy;
            assetDropModel.UpdatedDate = entity.UpdatedDate;
            return assetDropModel;
        }

    }
}
