using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceNumberMapper
    {
        public static InvoiceNumberViewModel ToViewModel(this InvoiceNumber entity, InvoiceNumberViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceNumberViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.Number = entity.Number;

            return viewModel;
        }

        public static InvoiceNumber ToEntity(this InvoiceNumberViewModel viewModel, InvoiceNumber entity = null)
        {
            if (entity == null)
                entity = new InvoiceNumber();

            entity.Id = viewModel.Id;
            entity.Number = viewModel.Number;

            return entity;
        }
    }
}
