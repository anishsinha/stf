using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class StateMapper
    {
        public static StateViewModel ToViewModel(this MstState entity, StateViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new StateViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Code = entity.Code;
            viewModel.Name = entity.Name;

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            return viewModel;
        }

        public static MstState ToEntity(this StateViewModel viewModel, MstState entity = null)
        {
            if (entity == null)
                entity = new MstState();

            entity.Id = viewModel.Id;
            entity.Code = viewModel.Code;
            entity.Name = viewModel.Name;

            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }
    }
}
