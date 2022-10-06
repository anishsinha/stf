using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ResaleCustomerMapper
    {
        public static ResaleCustomer ToEntity(this FuelRequestResaleCustomerViewModel viewModel, ResaleCustomer entity = null)
        {
            if (entity == null)
                entity = new ResaleCustomer();

            entity.Email = viewModel.Email;
            entity.Name = viewModel.Name;

            return entity;
        }

        public static FuelRequestResaleCustomerViewModel ToViewModel(this ResaleCustomer entity, FuelRequestResaleCustomerViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestResaleCustomerViewModel(Status.Success);

            viewModel.Email = entity.Email;
            viewModel.Name = entity.Name;

            return viewModel;
        }

        public static FuelRequestResaleCustomerViewModel ToDetailsViewModel(this ResaleCustomer entity, FuelRequestResaleCustomerViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestResaleCustomerViewModel(Status.Success);

            viewModel.Email = entity.Name != null ? entity.Email : Resource.lblHyphen;
            viewModel.Name = entity.Name != null ? entity.Name : Resource.lblHyphen;

            return viewModel;
        }
    }
}
