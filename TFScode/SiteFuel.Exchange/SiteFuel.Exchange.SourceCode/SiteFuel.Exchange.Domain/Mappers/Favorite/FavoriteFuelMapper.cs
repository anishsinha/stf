using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels;
using System.Linq;
using SiteFuel.Exchange.Core;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FavoriteFuelMapper
    {
        public static FavoriteFuelGridViewModel ToGridViewModel(this CompanyFavoriteFuel entity, FavoriteFuelGridViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FavoriteFuelGridViewModel();

            viewModel.Id = entity.Id;
            viewModel.FuelTypeId = entity.FuelTypeId;
            viewModel.FuleName = entity.MstTfxProduct.Name;
            viewModel.AddedBy = $"{entity.User.FirstName} {entity.User.LastName}";
            viewModel.AddedDate = entity.AddedDate.ToString(Resource.constFormatDateTime);
            viewModel.RemovedBy = entity.User1 == null ? null : $"{entity.User1.FirstName} {entity.User1.LastName}";
            viewModel.RemovedDate = entity.RemovedDate == null ? null : entity.RemovedDate.Value.ToString(Resource.constFormatDateTime);

            return viewModel;
        }
    }
}
