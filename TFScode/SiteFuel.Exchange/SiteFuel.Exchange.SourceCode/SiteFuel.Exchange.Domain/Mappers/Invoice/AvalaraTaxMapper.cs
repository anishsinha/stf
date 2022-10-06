using SiteFuel.Exchange.Domain.AvaTaxExciseWebService;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class AvalaraTaxMapper
    {
        public static InvoiceTaxDetailsViewModel ToResponseViewModel(this AvalaraResponseViewModel avaViewModel, bool isTaxExcepted, decimal exchangeRate, InvoiceTaxDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceTaxDetailsViewModel(Status.Success);
            var entity = avaViewModel.Result;

            if (entity.NumberSuccess > 0)
            {
                viewModel.AvaTaxDetails = new System.Collections.Generic.List<TaxDetailsViewModel>();
                foreach (var item in entity.TransactionResults)
                {
                    if (item.Status.Equals(Constants.Success))
                    {
                        viewModel.TranId = item.TranId;
                        viewModel.TotalTaxAmount = Math.Round(item.TotalTaxAmount, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                        viewModel.ReturnCode = item.ReturnCode;
                        UpdateTruefillTax(viewModel, item.UserReturnValue);
                        foreach (var transAmount in item.TransactionTaxes)
                        {
                            UpdateTruefillTax(viewModel, transAmount.UserReturnedValue);
                            if (isTaxExcepted && transAmount.TaxType == ApplicationConstants.ExternalTaxTypeSALESUSE)
                            {
                                if (transAmount.TaxExemptionInd != ApplicationConstants.AvaTaxExemptedInd)
                                {
                                    viewModel.TotalTaxAmount -= Math.Round(transAmount.TaxAmount ?? 0, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay);
                                    continue;
                                }
                            }

                            string relatedLineItem = null;

                            if (avaViewModel.Request != null && avaViewModel.Request.TransactionLines.Any())
                            {
                                relatedLineItem = avaViewModel.Request.TransactionLines.Where(t => t.InvoiceLine == transAmount.InvoiceLine).Select(t => t.BillOfLadingNumber).FirstOrDefault();
                            }

                            if (viewModel.AvaTaxDetails.Any(t => t.RateDescription.Equals(transAmount.RateDescription) 
                                                                        && t.TaxExemptionInd == transAmount.TaxExemptionInd
                                                                        && t.TaxType == transAmount.TaxType
                                                                        && t.RateSubtype == transAmount.RateSubtype))
                            {
                                var existingTaxDetails = viewModel.AvaTaxDetails.Where(t => t.RateDescription.Equals(transAmount.RateDescription) 
                                                                        && t.TaxExemptionInd == transAmount.TaxExemptionInd
                                                                        && t.TaxType == transAmount.TaxType
                                                                        && t.RateSubtype == transAmount.RateSubtype).FirstOrDefault();
                                if (existingTaxDetails != null)
                                {
                                    existingTaxDetails.TaxAmount = existingTaxDetails.TaxAmount + (Math.Round(transAmount.TaxAmount ?? 0, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay));
                                    existingTaxDetails.TradingTaxAmount = existingTaxDetails.TradingTaxAmount + (Math.Round(transAmount.ReportingTaxAmount ?? 0, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay));
                                    existingTaxDetails.SalesTaxBaseAmount = existingTaxDetails.SalesTaxBaseAmount + transAmount.SalesTaxBaseAmount ?? 0;
                                }
                            }
                            else
                            {
                                viewModel.AvaTaxDetails.Add(new TaxDetailsViewModel()
                                {
                                    CalculationTypeInd = transAmount.CalculationTypeInd,
                                    Currency = transAmount.Currency,
                                    ProductCategory = transAmount.ProductCategory,
                                    RateDescription = transAmount.RateDescription,
                                    RateSubtype = transAmount.RateSubtype,
                                    RateType = transAmount.RateType,
                                    SalesTaxBaseAmount = transAmount.SalesTaxBaseAmount ?? 0,
                                    TaxAmount = Math.Round(transAmount.TaxAmount ?? 0, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay),
                                    TaxExemptionInd = transAmount.TaxExemptionInd,
                                    TaxingLevel = transAmount.TaxingLevel,
                                    TaxRate = transAmount.TaxRate ?? 0,
                                    TaxType = transAmount.TaxType,
                                    UnitOfMeasure = transAmount.UnitOfMeasure,
                                    LicenseNumber = transAmount.LicenseNumber,
                                    TradingTaxAmount = Math.Round(transAmount.ReportingTaxAmount ?? 0, ApplicationConstants.InvoiceTaxTotalAmountDecimalDisplay),
                                    TradingCurrency = transAmount.ReportingTaxCurrency ?? Currency.None.ToString(),
                                    ExchangeRate = exchangeRate,
                                    RelatedLineItem = relatedLineItem
                                });
                            }
                        }
                    }
                    else
                    {
                        viewModel.StatusMessage = Constants.RequestError;
                        viewModel.StatusCode = Status.Failed;
                    }
                }
                return viewModel;
            }
            else if (entity.NumberFailed > 0)
            {
                foreach (var item in entity.TransactionResults)
                {
                    foreach (var error in item.TransactionErrors)
                    {
                        if (error.ErrorCode.Equals("-995"))
                        {
                            viewModel.FailedStatusCode = (int)DDTConversionReason.AvalaraProductNotMapped;
                            viewModel.StatusCode = Status.Failed;
                            return viewModel;
                        }
                    }
                }
            }

            viewModel.StatusMessage = Constants.RequestError;
            viewModel.StatusCode = Status.Failed;

            return viewModel;
        }

        private static void UpdateTruefillTax(InvoiceTaxDetailsViewModel viewModel, string userReturnedValue)
        {
            if (Int32.TryParse(userReturnedValue, out int returnValue) && returnValue == 1)
            {
                viewModel.IsTrueFillTax = true;
            }
        }

        public static AvalaraTaxInputViewModel ToAvaTaxViewModel(this SalesCalculatorGridViewModel entity, string destZipCode, string destStateCode, string countryCode, string productCode, AvalaraTaxInputViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new AvalaraTaxInputViewModel(Status.Success);

            viewModel.DestinationPostalCode = destZipCode;
            viewModel.DestinationJurisdiction = destStateCode;
            viewModel.DestinationCountryCode = countryCode;

            viewModel.EffectiveDate = entity.PricingDate.Date;
            viewModel.InvoiceDate = entity.PricingDate.Date;
            viewModel.NetUnitsDropped = 1;
            viewModel.BilledUnitsDropped = 1;
            viewModel.GrossUnitsDropped = 1;

            viewModel.OriginAddress = entity.Address;
            viewModel.OriginCity = entity.City;
            viewModel.OriginCountryCode = countryCode;
            viewModel.OriginJurisdiction = entity.StateCode;
            viewModel.OriginPostalCode = entity.ZipCode;
            viewModel.ProductCode = productCode;
            viewModel.UnitPrice = entity.PriceAvg;
            return viewModel;
        }
    }
}
