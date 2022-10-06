using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DriverAssetFuelDropMapper
    {
        public static AssetDrop ToAssetDropEntity(this DriverAssetFuelDropViewModel viewModel, AssetDrop entity = null)
        {
            if (entity == null)
                entity = new AssetDrop();

            entity.Id = viewModel.FuelDrop.AssetDropId;
            entity.OrderId = viewModel.FuelDrop.OrderId;
            entity.JobXAssetId = viewModel.FuelDrop.JobXAssetId;
            entity.DropStartDate = viewModel.DropStartDate;
            entity.DropEndDate = viewModel.DropEndDate;
            entity.DroppedBy = viewModel.Driver.UserId;
            entity.DropStatus = viewModel.FuelDrop.DropStatus;
            entity.IsNewAsset = viewModel.FuelDrop.IsNewAsset;

            if (viewModel.FuelDrop.InvoiceId > 0)
                entity.InvoiceId = viewModel.FuelDrop.InvoiceId;

            if (viewModel.FuelDrop.Image != null)
                entity.Image = viewModel.FuelDrop.Image.ToEntity(entity.Image);

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.Driver.UserId;
            entity.UpdatedDate = viewModel.DropEndDate;

            return entity;
        }

    }
}
