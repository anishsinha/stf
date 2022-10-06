using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceXAccessorialFeeMapper
    {
        public static AccessorialFeeTableDetailViewModel ToViewModel(this InvoiceXAccessorialFee entity, AccessorialFeeTableDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AccessorialFeeTableDetailViewModel();

            viewModel.AccessorialFeeId = entity.AccessorialFeeId;
            return viewModel;
        }

        public static InvoiceXAccessorialFee ToEntity(this AccessorialFeeTableDetailViewModel viewModel, InvoiceXAccessorialFee entity = null)
        {
            if (entity == null)
                entity = new InvoiceXAccessorialFee();

            entity.AccessorialFeeId = viewModel.AccessorialFeeId.Value;

            return entity;
        }
    }
}
