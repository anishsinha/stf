using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class CreditAppDocumentMapper
    {
        public static DocumentViewModel ToViewModel(this CreditAppDocument entity)
        {
            DocumentViewModel viewModel = new DocumentViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.FileName = entity.FileName;
            viewModel.ModifiedFileName = entity.ModifiedFileName;
            viewModel.FilePath = entity.FilePath;
            if (entity.User != null)
            {
                viewModel.AddedByName = $"{entity.User.FirstName} {entity.User.LastName}";
            }

            return viewModel;
        }

        public static CreditAppDocument ToEntity(this DocumentViewModel viewModel, CreditAppDocument entity = null)
        {
            if (entity == null)
                entity = new CreditAppDocument();

            entity.Id = viewModel.Id;
            entity.FileName = viewModel.FileName;
            entity.ModifiedFileName = viewModel.ModifiedFileName;
            entity.FilePath = viewModel.FilePath;
            entity.AddedBy = viewModel.AddedBy;
            entity.CompanyId = viewModel.CompanyId;

            return entity;
        }
    }
}
