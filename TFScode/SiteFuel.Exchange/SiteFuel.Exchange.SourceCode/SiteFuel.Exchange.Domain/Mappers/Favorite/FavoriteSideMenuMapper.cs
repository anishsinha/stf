using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.ViewModels;
using System.Linq;
using SiteFuel.Exchange.Core;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FavoriteSideMenuMapper
    {

        public static FavoriteSideMenuViewModel ToViewModel(this FavoriteSideMenu entity, FavoriteSideMenuViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new FavoriteSideMenuViewModel();
            }

            viewModel.Id = entity.Id;
            viewModel.CompanyId = entity.CompanyId;
            viewModel.UserId = entity.UserId;
            viewModel.LinkId = entity.LinkId;
            viewModel.AddedOn = entity.AddedOn;

            return viewModel;
        }

        public static FavoriteSideMenu ToEntity(this FavoriteSideMenuViewModel viewModel, FavoriteSideMenu entity = null)
        {
            if (entity == null)
            {
                entity = new FavoriteSideMenu();
            }

            entity.Id = viewModel.Id;
            entity.CompanyId = viewModel.CompanyId;
            entity.UserId = viewModel.UserId;
            entity.LinkId = viewModel.LinkId;
            entity.AddedOn = viewModel.AddedOn;

            return entity;
        }

    }
}
