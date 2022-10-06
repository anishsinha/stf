using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class DifferentFuelPriceMapper
    {
        public static DifferentFuelPrice ToEntity(this DifferentFuelPriceViewModel viewModel, DifferentFuelPrice entity = null, BrokerMarginViewModel margin = null)
        {
            if (entity == null)
                entity = new DifferentFuelPrice();

            if (margin != null)
            {
                entity.MarginTypeId = margin.MarginTypeId;
                entity.Margin = margin.Margin;
            }
            entity.Id = viewModel.Id;
            entity.MinQuantity = viewModel.MinQuantity;
            entity.MaxQuantity = viewModel.MaxQuantity;
            entity.PricingTypeId = viewModel.PricingTypeId;
            entity.RackAvgTypeId = viewModel.RackAvgTypeId;
            entity.PricePerGallon = viewModel.PricePerGallon.HasValue ? viewModel.PricePerGallon.Value : 0;

            return entity;
        }

        public static DifferentFuelPriceViewModel ToViewModel(this DifferentFuelPrice entity, DifferentFuelPriceViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DifferentFuelPriceViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.MinQuantity = entity.MinQuantity.GetPreciseValue(6);
            viewModel.MaxQuantity = entity.MaxQuantity.HasValue ? entity.MaxQuantity.Value.GetPreciseValue(6) : entity.MaxQuantity;
            viewModel.PricingTypeId = entity.PricingTypeId;
            viewModel.RackAvgTypeId = entity.RackAvgTypeId;
            viewModel.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            viewModel.OfferPricingId = entity.OfferPricingId;

            return viewModel;
        }
    }
}
