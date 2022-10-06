using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers.TankRental
{
    public static class TankDetailsMapper
    {
        public static TankDetailsViewModel ToViewModel(this TankDetail entity, TankDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TankDetailsViewModel();

            viewModel.TankDetailId = entity.Id;
            viewModel.BillingFrequencyId = entity.RentalFrequencyId;
            viewModel.RentalFee = entity.RentalFee;
            viewModel.Description = entity.FeeDescription;
            viewModel.FeeTaxDetails = new FeeTaxDetails
            {
                Amount = entity.TaxAmount,
                Description = entity.TaxDescription,
                Percentage = entity.TaxPercentage
            };
            viewModel.StartDate = entity.StartDate.Date;
            viewModel.EndDate = entity.EndDate?.Date;

            return viewModel;
        }
    }
}
