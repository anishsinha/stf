using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SubcontractorMapper
    {
        public static SubcontractorViewModel ToViewModel(this Subcontractor entity, SubcontractorViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new SubcontractorViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            return viewModel;
        }

        public static Subcontractor ToEntity(this SubcontractorViewModel viewModel, Subcontractor entity = null)
        {
            if (entity == null)
                entity = new Subcontractor();

            entity.Id = viewModel.Id;
            entity.Name = viewModel.Name;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            return entity;
        }
    }
}
