using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class JobXAssetMapper
    {
        public static AssetJobAssignmentViewModel ToViewModel(this JobXAsset entity, AssetJobAssignmentViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AssetJobAssignmentViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.AssetId = entity.AssetId;
            viewModel.JobId = entity.JobId;
            viewModel.AssignedBy = entity.AssignedBy;
            viewModel.AssignedDate = entity.AssignedDate;
            viewModel.RemovedBy = entity.RemovedBy;
            viewModel.RemovedDate = entity.RemovedDate;

            return viewModel;
        }

        public static JobXAsset ToEntity(this AssetJobAssignmentViewModel viewModel, JobXAsset entity = null)
        {
            if (entity == null)
                entity = new JobXAsset();

            entity.Id = viewModel.Id;
            entity.AssetId = viewModel.AssetId;
            entity.JobId = viewModel.JobId;
            entity.AssignedBy = viewModel.AssignedBy;
            entity.AssignedDate = viewModel.AssignedDate;
            entity.RemovedBy = viewModel.RemovedBy;
            entity.RemovedDate = viewModel.RemovedDate;

            return entity;
        }
    }
}
