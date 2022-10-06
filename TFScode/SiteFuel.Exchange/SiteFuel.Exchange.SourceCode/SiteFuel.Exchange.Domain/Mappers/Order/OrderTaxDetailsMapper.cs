using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OrderTaxDetailsMapper
    {
        public static OrderTaxDetail ToEntity(this OrderTaxDetailsViewModel viewModel, int fuelTypeId, OrderTaxDetail entity = null)
        {
            if (entity == null)
                entity = new OrderTaxDetail();

            if (viewModel != null)
            {
                entity.TaxPricingTypeId = viewModel.TaxPricingTypeId;
                entity.TaxRate = viewModel.TaxRate;
                entity.TaxDescription = viewModel.TaxDescription;
                entity.IsActive = true;
                entity.OtherFuelTypeId = fuelTypeId;
                entity.AddedBy = viewModel.AddedBy;
                entity.AddedByCompanyId = viewModel.AddedByCompanyId;
                entity.Currency = viewModel.Currency;
                entity.ExchangeRate = viewModel.ExchangeRate;
                entity.BaseTaxRate = MoneyConverter.GetBaseAmount(viewModel.Currency, viewModel.TaxRate, viewModel.ExchangeRate);
            }
            return entity;
        }

        public static List<OrderTaxDetailsViewModel> ToViewModel(this List<OrderTaxDetail> entities, List<OrderTaxDetailsViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<OrderTaxDetailsViewModel>();

            if (entities != null && entities.Count > 0)
            {
                foreach (OrderTaxDetail tax in entities)
                {
                    OrderTaxDetailsViewModel taxRecord = new OrderTaxDetailsViewModel();
                    taxRecord.TaxPricingTypeId = tax.TaxPricingTypeId;
                    taxRecord.TaxRate = tax.TaxRate.GetPreciseValue(6);
                    taxRecord.TaxDescription = tax.TaxDescription;
                    viewModel.Add(taxRecord);
                }
            }
            return viewModel;
        }
    }
}
