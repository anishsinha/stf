using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CountryMapper
    {
        public static CountryViewModel ToViewModel(this MstCountry entity, CountryViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CountryViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Code = entity.Code;
            viewModel.Name = entity.Name;
            viewModel.UoM = entity.DefaultUoM;
            viewModel.Currency = entity.Currency;

            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedBy = entity.UpdatedBy;
            viewModel.UpdatedDate = entity.UpdatedDate;

            return viewModel;
        }

        public static MstCountry ToEntity(this CountryViewModel viewModel, MstCountry entity = null)
        {
            if (entity == null)
                entity = new MstCountry();

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
