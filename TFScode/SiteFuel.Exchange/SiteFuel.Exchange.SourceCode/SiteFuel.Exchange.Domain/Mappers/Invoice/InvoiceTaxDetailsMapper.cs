using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceTaxDetailsMapper
    {
        public static List<TaxDetail> ToEntity(this InvoiceTaxDetailsViewModel viewModel, List<TaxDetail> entity = null)
        {
            if (entity == null)
                entity = new List<TaxDetail>();

            if (viewModel != null && viewModel.AvaTaxDetails != null && viewModel.AvaTaxDetails.Count > 0)
            {
                foreach (var item in viewModel.AvaTaxDetails)
                {
                    if (item.TaxAmount != 0)
                    {
                        TaxDetail taxDetail = new TaxDetail();
                        taxDetail.CalculationTypeInd = item.CalculationTypeInd;
                        taxDetail.Currency = item.Currency;
                        taxDetail.ProductCategory = item.ProductCategory;
                        taxDetail.RateDescription = item.RateDescription;
                        taxDetail.RateSubType = item.RateSubtype;
                        taxDetail.RateType = item.RateType;
                        taxDetail.SalesTaxBaseAmount = item.SalesTaxBaseAmount;
                        taxDetail.TaxAmount = item.TaxAmount;
                        taxDetail.TaxExemptionInd = item.TaxExemptionInd;
                        taxDetail.TaxRate = item.TaxRate;
                        taxDetail.TaxType = item.TaxType;
                        taxDetail.TaxingLevel = item.TaxingLevel;
                        taxDetail.UnitOfMeasure = item.UnitOfMeasure;
                        taxDetail.LicenseNumber = item.LicenseNumber;
                        taxDetail.TaxPricingTypeId = item.TaxPricingTypeId;
                        taxDetail.TradingTaxAmount = item.TradingTaxAmount;
                        taxDetail.TradingCurrency = item.TradingCurrency;
                        taxDetail.ExchangeRate = item.ExchangeRate;
                        taxDetail.IsModified = item.IsModified;
                        taxDetail.RelatedLineItem = item.RelatedLineItem;
                        entity.Add(taxDetail);
                    }
                }
            }

            return entity;
        }

        public static InvoiceTaxDetailsViewModel ToViewModel(this List<TaxDetail> entity, InvoiceTaxDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceTaxDetailsViewModel(Status.Success);

            if (entity != null && entity.Count > 0)
            {
                foreach (var item in entity)
                {
                    viewModel.AvaTaxDetails.Add(new TaxDetailsViewModel()
                    {
                        Id = item.Id,
                        CalculationTypeInd = item.CalculationTypeInd,
                        Currency = item.Currency,
                        ProductCategory = item.ProductCategory,
                        RateDescription = item.RateDescription,
                        RateSubtype = item.RateSubType,
                        RateType = item.RateType,
                        SalesTaxBaseAmount = item.SalesTaxBaseAmount,
                        TaxAmount = item.TaxAmount.GetPreciseValue(2),
                        TaxExemptionInd = item.TaxExemptionInd,
                        TaxingLevel = item.TaxingLevel,
                        TaxRate = item.TaxRate,
                        TaxType = item.TaxType,
                        UnitOfMeasure = item.UnitOfMeasure,
                        IsModified = item.IsModified,
                        LicenseNumber = item.LicenseNumber,
                        TaxPricingTypeId = item.TaxPricingTypeId ?? (int)OtherProductTaxPricingType.DollarOnTotalAmount,
                        TradingTaxAmount = item.TradingTaxAmount.GetPreciseValue(2),
                        TradingCurrency = item.TradingCurrency,
                        RelatedLineItem = item.RelatedLineItem,
                        ExchangeRate = item.ExchangeRate
                    });
                }
            }

            return viewModel;
        }

        public static List<TaxViewModel> ToTaxViewModel(this IEnumerable<OrderTaxDetail> entities, List<TaxViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<TaxViewModel>();

            foreach (OrderTaxDetail entity in entities)
            {
                TaxViewModel tax = new TaxViewModel();
                tax.TaxAmount = entity.TaxRate.GetPreciseValue(6);
                tax.TaxDescription = entity.TaxDescription;
                tax.TaxPricingTypeId = entity.TaxPricingTypeId;
                tax.OrderId = entity.OrderId;
                
                viewModel.Add(tax);
            }

            return viewModel;
        }

        public static List<TaxViewModel> ToTaxViewModel(this ICollection<TaxDetail> entities, bool isOtherProductType = false, List<TaxViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<TaxViewModel>();

            foreach (TaxDetail entity in entities)
            {
                TaxViewModel tax = new TaxViewModel();
                tax.TaxAmount = isOtherProductType ? entity.SalesTaxBaseAmount.GetPreciseValue(6) : entity.TaxAmount.GetPreciseValue(6);
                tax.TaxDescription = string.IsNullOrEmpty(entity.RelatedLineItem) ? entity.RateDescription : (entity.RateDescription + " - " + entity.RelatedLineItem);
                tax.TaxPricingTypeId = entity.TaxPricingTypeId ?? (int)OtherProductTaxPricingType.DollarOnTotalAmount;
                viewModel.Add(tax);
            }

            return viewModel;
        }
    }
}
