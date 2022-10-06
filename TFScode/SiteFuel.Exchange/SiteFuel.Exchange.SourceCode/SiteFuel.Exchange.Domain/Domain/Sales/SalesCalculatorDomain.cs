using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class SalesCalculatorDomain : BaseDomain
    {
        public SalesCalculatorDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SalesCalculatorDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetOpisTerminalPricesForCalculator(SalesCalculatorDatatableViewModel salesViewModel, DataTableSearchModel requestModel)
        {
            var pricingData = new List<SalesCalculatorGridViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var helperDomain = new HelperDomain(storedProcedureDomain);
                pricingData = await new PricingServiceDomain(this).GetOpisTerminalPricesForCalculator(salesViewModel, requestModel);
                if (salesViewModel.IsCustomPricing && salesViewModel.Amount > 0)
                {
                    foreach (var item in pricingData)
                    {
                        item.Price = helperDomain.GetCalculatedPricePerGallon(item.Price, salesViewModel.Amount, salesViewModel.CustomPricing);
                        item.Amount = item.Price.ToString(ApplicationConstants.DecimalFormat4);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesCalculatorDomain", "GetOpisTerminalPricesForCalculator", ex.Message, ex);
            }

            return pricingData;
        }

        public async Task<List<SalesCalculatorGridViewModel>> GetPlattsTerminalPricesForCalculator(SalesCalculatorViewModel viewModel, DataTableSearchModel requestModel)
        {
            var pricingData = new List<SalesCalculatorGridViewModel>();

            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var helperDomain = new HelperDomain(storedProcedureDomain);
                pricingData = await new PricingServiceDomain(this).GetPlattsTerminalPricesForCalculator(viewModel, requestModel);

                if (viewModel.IsCustomPricing && viewModel.Amount > 0)
                {
                    foreach (var item in pricingData)
                    {
                        item.Price = helperDomain.GetCalculatedPricePerGallon(item.Price, viewModel.Amount, viewModel.CustomPricing);
                        item.Amount = item.Price.ToString(ApplicationConstants.DecimalFormat4);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesCalculatorDomain", "GetPlattsTerminalPricesForCalculator", ex.Message, ex);
            }

            return pricingData;
        }

        public async Task<CityRackSalesCalculatorViewModel> GetTerminalPricesForCalculator(SalesCalculatorViewModel salesViewModel, int companyId)
        {              
            var response = new CityRackSalesCalculatorViewModel();
            var pricingData = new List<SalesCalculatorGridViewModel>();
            try
            {
                int externalProductId = 0, productId = 0, productTypeId = 0;
                decimal latitude = 0, longitude = 0;
                string stateCode = string.Empty;
                string stateName = string.Empty;
                string CountryCode = string.Empty;
                string CountryGroupCode = string.Empty;

                string zipCode = salesViewModel.IsZipCode ? salesViewModel.ZipCode : salesViewModel.Zip;
                string productCode = string.Empty;
                var helperDomain = new HelperDomain(this);

                Geocode geoCodes = new Geocode();
                
                if (salesViewModel.IsZipCode)
                {
                    geoCodes = GoogleApiDomain.GetGeocode(zipCode);
                }
                else
                {
                    if (salesViewModel.CountryId == (int)Country.CAR && salesViewModel.IsMissingAddress())
                    {
                        var state = Context.DataContext.MstStates.First(t => t.Id == salesViewModel.StateId).ToViewModel();
                        var country = Context.DataContext.MstCountries.First(t => t.Id == salesViewModel.CountryId).ToViewModel();
                        var address = string.IsNullOrWhiteSpace(salesViewModel.Address) ? state.Name : salesViewModel.Address;
                        var city = string.IsNullOrWhiteSpace(salesViewModel.City) ? state.Name : salesViewModel.City;
                        var zip = string.IsNullOrWhiteSpace(salesViewModel.ZipCode) ? state.Name : zipCode;
                        geoCodes = GoogleApiDomain.GetGeocode($"{address} {city} {state.Name} {country.Code} {zip}");
                    }
                    else
                    {
                        geoCodes = GoogleApiDomain.GetGeocode($"{salesViewModel.Address} {salesViewModel.City} {salesViewModel.StateName} {salesViewModel.CountryCode} {salesViewModel.Zip}");
                    }
                    
                }
                if (geoCodes != null)
                {
                    latitude = Convert.ToDecimal(geoCodes.Latitude);
                    longitude = Convert.ToDecimal(geoCodes.Longitude);
                    stateCode = geoCodes.StateCode;
                    stateName = geoCodes.StateName;
                    CountryCode = geoCodes.CountryCode;
                    CountryGroupCode = geoCodes.CountryGroupCode;
                }
                if (salesViewModel.SelectedFuelType == (int)ProductDisplayGroups.FuelTypesInYourArea)
                {
                    productTypeId = salesViewModel.FuelTypeInYourAreaId ?? 0;
                }
                else if (salesViewModel.SelectedFuelType == (int)ProductDisplayGroups.CommonFuelType)
                {
                    productTypeId = salesViewModel.CommonFuelTypeId ?? 0;
                }
                else
                {
                    productTypeId = salesViewModel.LessCommonFuelTypeId ?? 0;
                }

                var productMap = await Context.DataContext.MstProductMappings
                                        .Where(t => t.ProductId == productTypeId)
                                        .Select(t => new { t.ExternalProductId, t.ProductId, t.MstProduct.ProductCode })
                                        .FirstOrDefaultAsync();
                if (productMap != null)
                {
                    externalProductId = productMap.ExternalProductId;
                    productId = productMap.ProductId;
                    productCode = productMap.ProductCode;
                }

                if (externalProductId > 0 && latitude != 0 && longitude != 0)
                {
                    //var mstState = Context.DataContext.MstStates.SingleOrDefault(t => t.Code.Equals(stateCode, StringComparison.OrdinalIgnoreCase) 
                    //                                                && t.MstCountry.Code.Equals(CountryCode, StringComparison.OrdinalIgnoreCase)
                    //                                                && (string.IsNullOrEmpty(CountryGroupCode) || t.MstCountryAsGroup.Code.Equals(CountryCode, StringComparison.OrdinalIgnoreCase)));

                    var mstCountryInfo = Context.DataContext.MstCountries.Where(t => t.Code.Equals(CountryCode, StringComparison.OrdinalIgnoreCase) && t.IsActive)
                                                                     .Select(t => new { t.Code, t.Currency, t.DefaultUoM }).SingleOrDefault();
                    if (mstCountryInfo != null)
                    {
                        var countryCode = mstCountryInfo.Code;
                        var currency = mstCountryInfo.Currency;
                        var uom = mstCountryInfo.DefaultUoM;
                        var companyCountry = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault && t.IsActive)
                                                                .Select(t => new { t.CountryId, t.MstCountry.UoD }).FirstOrDefault();
                        var inputModel = new SalesCalculatorInputViewModel
                        {
                            PricingDate = salesViewModel.PriceDate,
                            ExternalProductId = externalProductId,
                            SrcLatitude = latitude,
                            SrcLongitude = longitude,  
                            RecordCount = 5,
                            CountryCode = countryCode,
                            PricingCodeId = salesViewModel.FuelPricingDetails?.PricingCode?.Id,
                            CompanyCountryId = companyCountry?.CountryId ?? 1
                        };

                        var pricingDomain = new PricingServiceDomain(this);
                        pricingData = await pricingDomain.GetTerminalPricesForCalculator(inputModel);
                        pricingData.ForEach(t => t.UoD = companyCountry?.UoD ?? "miles");

                        if (salesViewModel.IncludeTaxes && countryCode != Country.CAR.ToString())
                        {
                            string previousZip = string.Empty;
                            string previousState = string.Empty;
                            decimal previousTotalTax = 0;

                            foreach (var item in pricingData)
                            {
                                if (!string.IsNullOrWhiteSpace(previousZip) && item.ZipCode.Equals(previousZip) && item.StateCode.Equals(previousState))
                                {
                                    item.TaxValue = previousTotalTax;
                                    continue;
                                }

                                var avaTaxInputViewModel = item.ToAvaTaxViewModel(zipCode, stateCode, countryCode, productCode);
                                if (!salesViewModel.IsZipCode) 
                                {
                                    avaTaxInputViewModel.DestinationAddress = salesViewModel.Address;
                                    avaTaxInputViewModel.DestinationCity = salesViewModel.City;
                                }
                                
                                avaTaxInputViewModel.Currency = currency;
                                avaTaxInputViewModel.UoM = uom;

                                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                                avaTaxInputViewModel.CurrencyRates = currencyRateDomain.GetCurrencyRatesForAvalara(Currency.USD, currency, avaTaxInputViewModel.EffectiveDate.Date, currency);

                                var avataxDetails = AvalaraDomain.InvokeProcessTransactions_5_27_0(avaTaxInputViewModel);
                                item.TaxValue = helperDomain.GetTotalTaxValue(avataxDetails.Result);

                                previousZip = item.ZipCode;
                                previousState = item.StateCode;
                                previousTotalTax = item.TaxValue;
                            }
                        }
                    }
                }

                if (salesViewModel.IsCustomPricing && salesViewModel.Amount > 0)
                {
                    foreach (var item in pricingData)
                    {
                        item.PriceAvg = helperDomain.GetCalculatedPricePerGallon(item.PriceAvg, salesViewModel.Amount, salesViewModel.CustomPricing);
                        item.PriceLow = helperDomain.GetCalculatedPricePerGallon(item.PriceLow, salesViewModel.Amount, salesViewModel.CustomPricing);
                        item.PriceHigh = helperDomain.GetCalculatedPricePerGallon(item.PriceHigh, salesViewModel.Amount, salesViewModel.CustomPricing);
                    }
                }

                response.SalesViewModel = pricingData;
                if (salesViewModel.IsCityRackTerminal)
                {
                    response.IsCityRackTerminal = salesViewModel.IsCityRackTerminal;
                    response.CityRackInputModel.PriceDate = salesViewModel.PriceDate;
                    response.CityRackInputModel.CityTerminalPricingType = salesViewModel.CityTerminalPricingType;
                    if (salesViewModel.CityTerminalPricingType == (int)SalesCalculatorRegionType.City)
                    {
                        response.CityRackInputModel.StateOrTerminalIds = string.Join(",", salesViewModel.CityTerminalIds);
                    }
                    else if (salesViewModel.CityTerminalPricingType == (int)SalesCalculatorRegionType.State)
                    {
                        response.CityRackInputModel.StateOrTerminalIds = string.Join(",", salesViewModel.StateTerminalIds);
                    }
                    response.CityRackInputModel.ExternalProductId = externalProductId;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SalesCalculatorDomain", "GetTerminalPricesForCalculator", ex.Message, ex);
            }
            return response;
        }
    }
}
