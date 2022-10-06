using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.FuelPricing;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ExternalPricingDomain : BaseDomain
    {
        public ExternalPricingDomain()
          : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ExternalPricingDomain(SiteFuelUow SiteFuelDbContext)
          : base(SiteFuelDbContext)
        {
        }

        public ExternalPricingDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<ExternalPricingDataViewModel> GetTerminalPriceAsync(int? terminalId, int productId, DateTimeOffset dropEndDate, int pricingCodeId, Currency currency, int? cityGroupTerminalId = 0)
        {
            var response = new ExternalPricingDataViewModel();
            var pricingData = await new PricingServiceDomain(this).GetTerminalPrice(terminalId, cityGroupTerminalId, productId, dropEndDate, pricingCodeId);
            if (pricingData != null)
            {
                if (pricingData.Price != 0)
                {
                    CurrencyRateDomain _currencyRateDomain = new CurrencyRateDomain();
                    pricingData.Price = _currencyRateDomain.Convert(pricingData.Currency, currency, pricingData.Price, dropEndDate);
                }
                response.TerminalPrice = pricingData.Price;
                response.TerminalPricingDate = pricingData.EffectiveDate;
                response.Currency = pricingData.Currency;
                response.PriceLastUpdatedDate = pricingData.PriceLastUpdatedDate;
            }
            return response;
        }

        public async Task<decimal> GetTerminalRackPriceAsync(int? terminalId, int productId, int pricingCodeId, DateTimeOffset dropEndDate, Currency currency, int? cityGroupTerminalId)
        {
            var response = 0.0M;
            var pricingData = await new PricingServiceDomain(this).GetTerminalPrice(terminalId, cityGroupTerminalId, productId, dropEndDate, pricingCodeId);
            if (pricingData != null)
            {
                if (pricingData.Price != 0)
                {
                    CurrencyRateDomain _currencyRateDomain = new CurrencyRateDomain();
                    pricingData.Price = _currencyRateDomain.Convert(pricingData.Currency, currency, pricingData.Price, dropEndDate);
                }
                response = pricingData.Price;
                return response;
            }
            return response;
        }

        public async Task<FuelPricingResponseViewModel> GetFuelPriceAsync(FuelPricingRequestViewModel fuelPricingRequestViewModel)
        {
            var pricing = await new PricingServiceDomain(this).GetFuelPrice(fuelPricingRequestViewModel);
            if (pricing != null && pricing.TerminalPrice > 0)
            {
                var decimalDigit = ConfigHelperMethods.GetConfigSetting(ApplicationConstants.DefaultPricingDecimalPlaces, "4");
                pricing.TerminalPrice = pricing.TerminalPrice.GetPreciseValue(Convert.ToInt32(decimalDigit));
                pricing.PricePerGallon = pricing.PricePerGallon.GetPreciseValue(Convert.ToInt32(decimalDigit));
            }
            return pricing;
        }

        public async Task<List<FuelPricingResponseViewModel>> GetFuelPriceAsync(List<FuelPriceRequestModel> priceRequestModels)
        {
            var pricing = await new PricingServiceDomain(this).GetFuelPrice(priceRequestModels);
            if (pricing != null && pricing.Any(t => t.TerminalPrice > 0))
            {
                var decimalDigit = ConfigHelperMethods.GetConfigSetting(ApplicationConstants.DefaultPricingDecimalPlaces, "4");
                pricing.ForEach(t => { t.TerminalPrice = t.TerminalPrice.GetPreciseValue(Convert.ToInt32(decimalDigit)); t.PricePerGallon = t.PricePerGallon.GetPreciseValue(Convert.ToInt32(decimalDigit)); });
            }
            return pricing;
        }

        public async Task<OfferLoadedPriceViewModel> CalculateLoadedPrice(int jobId, int productId, int pricingType, int rackType, decimal rackPrice, bool includeTaxes, int marketBasedType, int cityRackTerminalId, decimal suppliercost, int qty = 1, string zipcode = "", int pricingSourceId = 0, int pricingCodeId = 0)
        {
            OfferLoadedPriceViewModel response = new OfferLoadedPriceViewModel();
            try
            {
                var helperDomain = new HelperDomain(this);

                decimal tax = 0, priceToUse = 0, latitude = 0, longitude = 0;
                string city, stateCode, countryCode, countyName, destinationAddress, originAddress;
                city = stateCode = countryCode = countyName = destinationAddress = originAddress = string.Empty;
                Currency cur = Currency.USD;
                UoM uom = UoM.Gallons;
                MstExternalTerminal nearestTerminal = null;
                if (pricingType == (int)PricingType.PricePerGallon)
                {
                    priceToUse = rackPrice;
                }
                else if (pricingType == (int)PricingType.Suppliercost)
                {
                    priceToUse = suppliercost;
                }
                else
                {
                    if (jobId > 0)
                    {
                        Job jobDetail = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                        if (jobDetail != null)
                        {
                            latitude = jobDetail.Latitude;
                            longitude = jobDetail.Longitude;
                            countryCode = jobDetail.MstCountry.Code;
                            stateCode = jobDetail.MstState.Code;
                            city = jobDetail.City;
                            zipcode = jobDetail.ZipCode;
                            countyName = jobDetail.CountyName;
                            cur = jobDetail.Currency;
                            uom = jobDetail.UoM;
                            destinationAddress = jobDetail.Address;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(zipcode))
                    {
                        var geoCodes = GoogleApiDomain.GetGeocode(zipcode);
                        if (geoCodes != null)
                        {
                            latitude = Convert.ToDecimal(geoCodes.Latitude);
                            longitude = Convert.ToDecimal(geoCodes.Longitude);
                            countryCode = geoCodes.CountryCode;
                            stateCode = geoCodes.StateCode;
                            city = geoCodes.City;
                            countyName = geoCodes.CountyName;
                        }
                    }
                    var terminalPrice = await GetClosestTerminalPriceAsync(latitude, longitude, countryCode, productId, pricingCodeId, cityRackTerminalId, pricingSourceId);
                    if (terminalPrice.TerminalId > 0)
                    {
                        nearestTerminal = Context.DataContext.MstExternalTerminals.Include(t => t.MstState).First(t => t.Id == terminalPrice.TerminalId);
                        if (pricingType == (int)PricingType.Tier)
                        {
                            if (marketBasedType > 0)
                            {
                                if (marketBasedType == (int)PricingType.RackHigh)
                                {
                                    priceToUse = terminalPrice.HighPrice;
                                }
                                else if (marketBasedType == (int)PricingType.RackLow)
                                {
                                    priceToUse = terminalPrice.LowPrice;
                                }
                                else if (marketBasedType == (int)PricingType.RackAverage)
                                {
                                    priceToUse = terminalPrice.AvgPrice;
                                }
                            }
                            else
                            {
                                priceToUse = rackPrice;
                            }
                        }
                        else if (marketBasedType == (int)PricingType.RackHigh)
                        {
                            priceToUse = terminalPrice.HighPrice;
                        }
                        else if (marketBasedType == (int)PricingType.RackLow)
                        {
                            priceToUse = terminalPrice.LowPrice;
                        }
                        else if (marketBasedType == (int)PricingType.RackAverage)
                        {
                            priceToUse = terminalPrice.AvgPrice;
                        }
                    }
                }
                if (priceToUse == 0)
                {
                    return response;
                }
                if (includeTaxes && productId > 0 && nearestTerminal != null && !string.IsNullOrWhiteSpace(stateCode))
                {
                    var productCode = Context.DataContext.MstProducts.Where(t => t.Id == productId).Select(t => t.ProductCode).SingleOrDefault();
                    var avaTaxinput = new AvalaraTaxInputViewModel()
                    {
                        DestinationCity = city,
                        DestinationCountryCode = countryCode,
                        DestinationJurisdiction = stateCode,
                        DestinationPostalCode = zipcode,
                        DestinationCounty = countyName,
                        DestinationAddress = destinationAddress,
                        OriginCity = nearestTerminal.City,
                        OriginJurisdiction = nearestTerminal.MstState.Name,
                        OriginPostalCode = nearestTerminal.ZipCode,
                        OriginCountryCode = nearestTerminal.CountryCode,
                        OriginCounty = nearestTerminal.CountyName,
                        OriginAddress = nearestTerminal.Address,

                        EffectiveDate = DateTimeOffset.Now.Date,
                        InvoiceDate = DateTimeOffset.Now.Date,
                        NetUnitsDropped = qty,
                        BilledUnitsDropped = qty,
                        GrossUnitsDropped = qty,
                        ProductCode = productCode,
                        UnitPrice = priceToUse,
                        Currency = cur,
                        UoM = uom
                    };
                    var avalaraTaxDetails = AvalaraDomain.InvokeProcessTransactions_5_27_0(avaTaxinput);
                    tax = helperDomain.GetTotalTaxValue(avalaraTaxDetails.Result);
                }

                response.TotalDropAmount = helperDomain.GetCalculatedPricePerGallon(priceToUse, rackPrice, rackType) * qty;
                if (includeTaxes)
                {
                    response.TotalTaxAmount = tax;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "CalculateLoadedPrice", ex.Message, ex);
            }
            return response;
        }

        public async Task<decimal> CalculateTerminalPrice(int jobId, int productId, int rackType, decimal rackPrice, bool includeTaxes, int pricingCodeId, int cityRackTerminalId, int sourceId)
        {
            decimal response = 0;
            try
            {
                var fuelRequestDomain = new FuelRequestDomain(this);
                int fuelTypeId = fuelRequestDomain.GetFuelTypeId(productId, sourceId, (int)PricingType.RackHigh);

                var helperDomain = new HelperDomain(this);
                var jobDetail = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == jobId);
                if (jobDetail != null)
                {
                    var terminalPrice = await GetClosestTerminalPriceAsync(jobDetail.Latitude, jobDetail.Longitude, jobDetail.MstCountry.Code, fuelTypeId, pricingCodeId, cityRackTerminalId);
                    if (terminalPrice.TerminalId > 0)
                    {
                        var nearestTerminal = Context.DataContext.MstExternalTerminals.Include(t => t.MstState).First(t => t.Id == terminalPrice.TerminalId);

                        decimal tax = 0, estimatedPrice = 0, rackPriceToUse = terminalPrice.TerminalPrice;
                        if (rackPriceToUse == 0)
                        {
                            return response;
                        }
                        if (includeTaxes && productId > 0)
                        {
                            var productCode = Context.DataContext.MstProducts.Where(t => t.Id == fuelTypeId).Select(t => t.ProductCode).SingleOrDefault();
                            var avaTaxinput = new AvalaraTaxInputViewModel()
                            {
                                DestinationCity = jobDetail.City,
                                DestinationCountryCode = jobDetail.MstCountry.Code,
                                DestinationJurisdiction = jobDetail.MstState.Code,
                                DestinationPostalCode = jobDetail.ZipCode,
                                DestinationCounty = jobDetail.CountyName,
                                DestinationAddress = jobDetail.Address,
                                OriginCity = nearestTerminal.City,
                                OriginJurisdiction = nearestTerminal.MstState.Name,
                                OriginPostalCode = nearestTerminal.ZipCode,
                                OriginCountryCode = nearestTerminal.CountryCode,
                                OriginCounty = nearestTerminal.CountyName,
                                OriginAddress = nearestTerminal.Address,

                                EffectiveDate = DateTimeOffset.Now.Date,
                                InvoiceDate = DateTimeOffset.Now.Date,
                                NetUnitsDropped = 1,
                                BilledUnitsDropped = 1,
                                GrossUnitsDropped = 1,
                                ProductCode = productCode,
                                UnitPrice = rackPriceToUse,
                                Currency = jobDetail.Currency,
                                UoM = jobDetail.UoM
                            };

                            var avalaraTaxDetails = AvalaraDomain.InvokeProcessTransactions_5_27_0(avaTaxinput);
                            tax = helperDomain.GetTotalTaxValue(avalaraTaxDetails.Result);
                        }

                        estimatedPrice = helperDomain.GetCalculatedPricePerGallon(rackPriceToUse, rackPrice, rackType);
                        if (includeTaxes)
                        {
                            estimatedPrice = estimatedPrice + tax;
                        }

                        CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain();
                        estimatedPrice = currencyRateDomain.Convert(nearestTerminal.Currency, jobDetail.Currency, estimatedPrice, DateTimeOffset.Now);

                        response = estimatedPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "CalculateTerminalPrice", ex.Message, ex);
            }
            return response;
        }

        // Used for TPO case
        public async Task<ExternalPricingDataViewModel> GetClosestTerminalPriceAsync(decimal latitude, decimal longitude, string countryCode, int productId, int? pricingCodeId = null, int cityRackTerminalId = 0, int pricingSourceId = 0)
        {
            ExternalPricingDataViewModel response = new ExternalPricingDataViewModel();
            try
            {
                if (productId > 0)
                {
                    var productMapping = Context.DataContext.MstProductMappings.FirstOrDefault(t => t.ProductId == productId);
                    if (productMapping != null)
                    {
                        var pricingDomain = new PricingServiceDomain(this);
                        var pricingData = await pricingDomain.GetTerminalPricesForCalculator(new SalesCalculatorInputViewModel()
                        {
                            ExternalProductId = productMapping.ExternalProductId,
                            PricingDate = DateTimeOffset.Now,
                            RecordCount = 5,
                            SrcLatitude = latitude,
                            SrcLongitude = longitude,
                            CityGroupTerminalId = cityRackTerminalId,
                            CountryCode = countryCode,
                            PricingCodeId = pricingCodeId
                        });

                        var closestTerminalData = pricingData.FirstOrDefault(t => t.PricingDate >= DateTimeOffset.Now.AddDays(-5));
                        if (closestTerminalData != null)
                        {
                            response.AvgPrice = closestTerminalData.PriceAvg;
                            response.LowPrice = closestTerminalData.PriceLow;
                            response.HighPrice = closestTerminalData.PriceHigh;
                            response.TerminalPrice = closestTerminalData.Price;
                            response.TerminalId = closestTerminalData.TerminalId;
                            response.TerminalPricingDate = closestTerminalData.PricingDate;
                            response.Currency = closestTerminalData.Currency;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalPrice", ex.Message, ex);
            }

            return response;
        }

        public async Task<ExternalPricingDataViewModel> GetClosestTerminalPriceAsync(int jobId, int productId)
        {
            using (var tracer = new Tracer("ExternalPricingDomain", "GetClosestTerminalPrice"))
            {
                var response = new ExternalPricingDataViewModel();
                try
                {
                    var job = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new
                    {
                        t.Latitude,
                        t.Longitude,
                        CountryCode = t.MstCountry.Code
                    }).FirstOrDefault();

                    if (job != null)
                    {
                        response = await GetClosestTerminalPriceAsync(job.Latitude, job.Longitude, job.CountryCode, productId, pricingCodeId: 0);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalPrice", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<decimal> GetTerminalPrice(int terminalId, int productId, int pricingTypeId, Currency currency, DateTimeOffset pricingDateTime)
        {
            var response = 0.0M;
            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain();

            try
            {
                if (productId > 0)
                {
                    var productMapping = Context.DataContext.MstProductMappings.FirstOrDefault(t => t.ProductId == productId && t.ExternalTerminalId == terminalId);
                    if (productMapping != null)
                    {
                        var domain = new PricingServiceDomain(this);
                        var externalPricing = await domain.GetTerminalPrice(productMapping.ExternalProductId, terminalId);
                        if (externalPricing != null)
                        {
                            if (pricingTypeId == (int)PricingType.RackLow)
                            {
                                response = externalPricing.LowPrice;
                            }
                            else if (pricingTypeId == (int)PricingType.RackHigh)
                            {
                                response = externalPricing.HighPrice;
                            }
                            else if (pricingTypeId == (int)PricingType.RackAverage)
                            {
                                response = externalPricing.AvgPrice;
                            }
                            response = currencyRateDomain.Convert(externalPricing.Currency, currency, response, pricingDateTime);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetTerminalPrice", ex.Message, ex);
            }

            return response;
        }

        public async Task<bool> IsTerminalAvailable(int orderId)
        {
            var response = false;
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new
                {
                    t.TerminalId,
                    t.FuelRequest.ProductDisplayGroupId,
                    MasterProductDisplayGroupId = t.FuelRequest.MstProduct.ProductDisplayGroupId
                }).FirstOrDefaultAsync();

                if (order != null)
                {
                    if (order.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType || order.MasterProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
                    {
                        response = true;
                    }
                    else
                    {
                        if (order.TerminalId != null)
                        {
                            response = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "IsTerminalAvailable", ex.Message, ex);
            }

            return response;
        }

        public List<DropdownDisplayStateItem> GetBuyerOfferCityGroupTerminals(bool fromJobSearch, int jobId, int stateId, bool allStates, PricingSource sourceId)
        {
            var terminals = new List<DropdownDisplayStateItem>();
            try
            {
                var dummyZip = Constants.DummyAxxisZipCode;
                if (sourceId == PricingSource.OPIS)
                {
                    dummyZip = Constants.DummyOpisZipCode;
                }
                else if (sourceId == PricingSource.PLATTS)
                {
                    dummyZip = Constants.DummyPlattsZipCode;
                }
                string countryCode = Country.USA.ToString();
                if (fromJobSearch)
                {
                    var jobDetails = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.StateId, t.MstCountry.Code }).SingleOrDefault();
                    stateId = jobDetails.StateId;
                    countryCode = jobDetails.Code;
                }
                else
                    countryCode = Context.DataContext.MstStates.Where(t => t.Id == stateId).Select(t => t.MstCountry.Code).FirstOrDefault();

                var terminalList = Context.DataContext.MstExternalTerminals
                                    .Where(t => t.PricingSourceId == (int)sourceId && t.ZipCode == dummyZip && t.CountryCode == countryCode)
                                    .Select(t => new DropdownDisplayStateItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name + ", " + t.StateCode,
                                        IsWithinState = t.StateId == stateId
                                    }).Distinct().ToList();

                terminals.AddRange(terminalList);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetBuyerOfferCityGroupTerminals", ex.Message, ex);
            }
            return terminals;
        }

        public List<DropdownDisplayStateItem> GetCityGroupTerminalsByStateId(int stateId, bool allStates, int selectedCityRackId, PricingSource sourceId)
        {
            var terminals = new List<DropdownDisplayStateItem>();
            try
            {
                var dummyZip = Constants.DummyAxxisZipCode;
                if (sourceId == PricingSource.OPIS)
                {
                    dummyZip = Constants.DummyOpisZipCode;
                }
                else if (sourceId == PricingSource.PLATTS)
                {
                    dummyZip = Constants.DummyPlattsZipCode;
                }
                var countryCode = Context.DataContext.MstStates.Where(t => t.Id == stateId).Select(t => t.MstCountry.Code).FirstOrDefault();
                var terminalList = Context.DataContext.MstExternalTerminals
                                    .Where(t => t.PricingSourceId == (int)sourceId && t.ZipCode == dummyZip && t.CountryCode == countryCode)
                                    .Select(t => new DropdownDisplayStateItem
                                    {
                                        Id = t.Id,
                                        Name = t.Name + ", " + t.StateCode,
                                        IsWithinState = t.StateId == stateId
                                    }).Distinct().ToList();

                terminals.AddRange(terminalList);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetCityGroupTerminals", ex.Message, ex);
            }
            return terminals;
        }

        public bool IsCityGroupTerminalPriceAvailable(int jobId, int fueltypeId, int cityGroupTerminalId, DateTimeOffset datetime, float lattitude, float longitude, string countryCode, PricingSource pricingSource = PricingSource.Axxis)
        {
            bool response = false;
            //var pricingService = new PricingServiceDomain(this);
            //response = pricingService.IsCityRackPriceAvailable(fueltypeId, cityGroupTerminalId, pricingSource);

            //if (pricingSource == PricingSource.Axxis)
            //{
            //    response = IsAxxisCityRackPriceAvaiable(jobId, fueltypeId, cityGroupTerminalId, lattitude, longitude, countryCode);
            //}
            //else if (pricingSource == PricingSource.OPIS)
            //{
            //    response = IsOpisCityRackPriceAvailable(fueltypeId, cityGroupTerminalId);
            //}
            //else if (pricingSource == PricingSource.PLATTS)
            //{
            //    response = IsPlattsCityRackPriceAvailable(fueltypeId, cityGroupTerminalId);
            //}
            return response;
        }

        public async Task GetTerminalWithPrice(FuelRequestPricingDetailsViewModel pricingDetails, FuelRequest fuelRequest, int jobId, Currency currency)
        {
            var job = Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new
            {
                t.Latitude,
                t.Longitude,
                CountryCode = t.MstCountry.Code,
                CountryId = t.CountryId
            }).FirstOrDefault();

            if (job != null)
            {
                if (pricingDetails.PricingSourceId == (int)PricingSource.Axxis)
                {
                    await GetAxxisTerminalWithPriceAsync(currency, fuelRequest, job.Latitude, job.Longitude, job.CountryCode, pricingDetails.PricingCode.Id);
                }
                else
                {
                    await GetOtherSourceTerminalWithPrice(pricingDetails, fuelRequest, job.Latitude, job.Longitude, job.CountryId);
                }
            }
        }

        private async Task GetAxxisTerminalWithPriceAsync(Currency currency, FuelRequest fuelRequest, decimal latitude, decimal longitude, string countryCode, int pricingCodeId)
        {
            ExternalPricingDataViewModel externalPricingData = await GetClosestTerminalPriceAsync(latitude, longitude, countryCode, fuelRequest.FuelTypeId, pricingCodeId);
            if (externalPricingData != null && externalPricingData.TerminalId > 0)
            {
                fuelRequest.TerminalId = externalPricingData.TerminalId;
                fuelRequest.CreationTimeRackPPG = externalPricingData.TerminalPrice;
                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                fuelRequest.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, currency, fuelRequest.CreationTimeRackPPG, DateTimeOffset.Now);
            }
        }

        private async Task GetOtherSourceTerminalWithPrice(FuelRequestPricingDetailsViewModel viewModel, FuelRequest fuelRequest, decimal latitude, decimal longitude, int countryId)
        {
            var terminals = await GetClosestTerminals(fuelRequest.FuelTypeId, latitude, longitude, countryId, string.Empty, viewModel.PricingCode.Id);
            var terminal = terminals.FirstOrDefault(t => t.Id > 0);
            if (terminal != null)
            {
                fuelRequest.TerminalId = terminal.Id;
            }
            var terminalPrice = await new PricingServiceDomain(this).GetTerminalPrice(fuelRequest.TerminalId, fuelRequest.CityGroupTerminalId, fuelRequest.FuelTypeId, DateTimeOffset.Now, fuelRequest.FuelRequestPricingDetail.PricingCodeId);
            if (terminalPrice != null)
            {
                if (terminalPrice.Price != 0)
                {
                    CurrencyRateDomain _currencyRateDomain = new CurrencyRateDomain();
                    terminalPrice.Price = _currencyRateDomain.Convert(terminalPrice.Currency, fuelRequest.Currency, terminalPrice.Price, DateTimeOffset.Now);
                }
                fuelRequest.CreationTimeRackPPG = terminalPrice.Price;
            }
        }


        public async Task GetTerminalWithPriceForTierItems(PricingViewModel pricingDetails, Job job, Currency currency)
        {

            if (job != null)
            {
                if (pricingDetails.PricingSourceId == (int)PricingSource.Axxis)
                {
                    await GetAxxisTerminalWithPriceForTierAsync(currency, pricingDetails, job.Latitude, job.Longitude, job.MstCountry.Code, pricingDetails.PricingCode.Id);
                }
                else
                {
                    await GetOtherSourceTerminalWithPriceForTier(pricingDetails, job.Latitude, job.Longitude, job.CountryId, currency);
                }
            }
        }
        private async Task GetAxxisTerminalWithPriceForTierAsync(Currency currency, PricingViewModel model, decimal latitude, decimal longitude, string countryCode, int pricingCodeId)
        {
            ExternalPricingDataViewModel externalPricingData = await GetClosestTerminalPriceAsync(latitude, longitude, countryCode, model.FuelTypeId.Value, pricingCodeId);
            if (externalPricingData != null && externalPricingData.TerminalId > 0)
            {
                model.TerminalId = externalPricingData.TerminalId;
                model.CreationTimeRackPPG = externalPricingData.TerminalPrice;
                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(this);
                model.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, currency, model.CreationTimeRackPPG.Value, DateTimeOffset.Now);
            }
        }

        private async Task GetOtherSourceTerminalWithPriceForTier(PricingViewModel model, decimal latitude, decimal longitude, int countryId, Currency Currency)
        {
            var terminals = await GetClosestTerminals(model.FuelTypeId.Value, latitude, longitude, countryId, string.Empty, model.PricingCode.Id);
            var terminal = terminals.FirstOrDefault(t => t.Id > 0);
            if (terminal != null)
            {
                model.TerminalId = terminal.Id;
            }
            var terminalPrice = await new PricingServiceDomain(this).GetTerminalPrice(model.TerminalId, model.CityGroupTerminalId, model.FuelTypeId.Value, DateTimeOffset.Now, model.PricingCode.Id);
            if (terminalPrice != null)
            {
                if (terminalPrice.Price != 0)
                {
                    CurrencyRateDomain _currencyRateDomain = new CurrencyRateDomain();
                    terminalPrice.Price = _currencyRateDomain.Convert(terminalPrice.Currency, Currency, terminalPrice.Price, DateTimeOffset.Now);
                }
                model.CreationTimeRackPPG = terminalPrice.Price;
            }
        }
        public async Task<List<DropdownDisplayItem>> GetAxxisFuelProducts(ProductDisplayGroups displayGroupId, int companyId = 0, int jobId = 0, decimal radius = 100, string zipCode = "")
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                string countryCode = string.Empty;
                decimal latitude = 0, longitude = 0;
                if (jobId <= 0 && zipCode != "")
                {
                    var geoCodes = GoogleApiDomain.GetGeocode(zipCode);
                    if (geoCodes != null)
                    {
                        latitude = Convert.ToDecimal(geoCodes.Latitude);
                        longitude = Convert.ToDecimal(geoCodes.Longitude);
                        countryCode = Convert.ToString(geoCodes.CountryCode);
                    }
                }
                var storedProcedureDomain = new StoredProcedureDomain(this);
                if (companyId > 0 && displayGroupId == ProductDisplayGroups.FavoriteFuelType)
                {
                    response = storedProcedureDomain.GetFavoriteFuelTypes(companyId, jobId, countryCode);
                }
                else if (displayGroupId == ProductDisplayGroups.FuelTypesInYourArea)
                {
                    if (jobId > 0)
                    {
                        var job = await Context.DataContext.Jobs.Where(t => t.Id == jobId).Select(t => new { t.Latitude, t.Longitude, t.MstCountry.Code }).FirstOrDefaultAsync();
                        if (job != null)
                        {
                            latitude = job.Latitude;
                            longitude = job.Longitude;
                            countryCode = job.Code;
                        }
                    }
                    response = await new PricingServiceDomain(this).GetProductsInYourArea(radius, latitude, longitude, countryCode, companyId);
                }
                else
                {
                    response = storedProcedureDomain.GetProductsByDisplayGroup(jobId, (int)displayGroupId, countryCode, companyId);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetAxxisFuelProducts", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetSourceBasedFuelProducts(PricingSource source)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                response = storedProcedureDomain.GetSourceBasedProducts(source);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetSourceBasedFuelProducts", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetOpisTerminals(int cityRackId = 0, decimal latitude = 0, decimal longitude = 0, int countryId = 1, string searchStringTeminal = "", PricingSource source = PricingSource.Axxis, int companyId = 0)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                int companyCountryId = 0;
                if (companyId > 0)
                {
                    companyCountryId = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault && t.IsActive).Select(t => t.CountryId).FirstOrDefault();
                }
                companyCountryId = companyCountryId > 0 ? companyCountryId : countryId;
                var terminals = storedProcedureDomain.GetOpisTerminals(cityRackId, latitude, longitude, countryId, searchStringTeminal, source, companyCountryId);

                response.Insert(0, new DropdownDisplayItem { Id = 0, Name = ResourceMessages.GetMessage(Resource.valMessageSelect, new object[] { Resource.lblTerminal }) });
                response.AddRange(terminals.Select(t => new DropdownDisplayItem
                {
                    Id = t.Id,
                    Name = $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}"
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetOpisTerminals", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetClosestTerminals(int orderId, string searchStringTeminal)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                var order = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new
                {
                    FreightPricingMethod = t.OrderAdditionalDetail.FreightPricingMethod,
                }).FirstOrDefault();

                if (order.FreightPricingMethod == FreightPricingMethod.Manual)
                {
                    terminals = await GetClosestTerminalsForManualFreightMethod(orderId, searchStringTeminal);
                }
                else if (order.FreightPricingMethod == FreightPricingMethod.Auto)
                {
                    List<int> orderIds = new List<int>();
                    orderIds.Add(orderId);
                    terminals = await GetClosestTerminalsForAutoFreightMethod(orderIds, searchStringTeminal);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminals", ex.Message, ex);
            }
            return terminals;
        }

        public async Task<List<DropdownDisplayItem>> GetClosestTerminalsForAutoFreightMethod(List<int> orderIds, string searchStringTeminal)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                var orders = Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id)).Select(t => new
                {
                    t.FuelRequest.FuelTypeId,
                    t.AcceptedCompanyId,
                    t.FuelRequest.Currency,
                    t.FuelRequest.Job.StateId,
                    t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                }).ToList();

                foreach (var order in orders)
                {
                    var pricingDetails = await ContextFactory.Current.GetDomain<OrderDomain>().GetRequestPricingDetail(order.RequestPriceDetailId, (int)order.Currency, order.AcceptedCompanyId, order.FuelTypeId, order.StateId);
                    if (pricingDetails != null)
                    {
                        if (!string.IsNullOrWhiteSpace(pricingDetails.SourceRegionJsonParameters))
                        {
                            SourceRegionJSONParameter sourceRegionsParameters = JsonConvert.DeserializeObject<SourceRegionJSONParameter>(pricingDetails.SourceRegionJsonParameters);
                            if (sourceRegionsParameters != null)
                            {
                                if (!string.IsNullOrWhiteSpace(sourceRegionsParameters.SelectedTerminals))
                                {
                                    List<int> terminalIds = sourceRegionsParameters.SelectedTerminals.Split(',').Select(int.Parse).ToList();
                                    terminals = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalsForAutoFreightMethod", ex.Message, ex);
            }
            return terminals.Distinct().ToList();
        }

        public async Task<List<BulkPlantViewModel>> GetBulkPlantsForAutoFreightMethod(List<int> orderIds, decimal latitude, decimal longitude)
        {
            var response = new List<BulkPlantViewModel>();
            var bulkPlantIds = new List<int>();
            try
            {
                var orders = await Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id)).Select(t => new
                {
                    t.FuelRequest.FuelTypeId,
                    t.AcceptedCompanyId,
                    t.FuelRequest.Currency,
                    t.FuelRequest.Job.StateId,
                    t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                }).ToListAsync();

                foreach (var order in orders)
                {
                    var pricingDetails = await ContextFactory.Current.GetDomain<OrderDomain>().GetRequestPricingDetail(order.RequestPriceDetailId, (int)order.Currency, order.AcceptedCompanyId, order.FuelTypeId, order.StateId);
                    if (pricingDetails != null)
                    {
                        if (!string.IsNullOrWhiteSpace(pricingDetails.SourceRegionJsonParameters))
                        {
                            SourceRegionJSONParameter sourceRegionsParameters = JsonConvert.DeserializeObject<SourceRegionJSONParameter>(pricingDetails.SourceRegionJsonParameters);
                            if (sourceRegionsParameters != null)
                            {
                                if (!string.IsNullOrWhiteSpace(sourceRegionsParameters.SelectedBulkPlants))
                                {
                                    List<int> selectedBulkPlantIds = sourceRegionsParameters.SelectedBulkPlants.Split(',').Select(int.Parse).ToList();
                                    if (selectedBulkPlantIds != null && selectedBulkPlantIds.Any())
                                        bulkPlantIds.AddRange(selectedBulkPlantIds);
                                }
                            }
                        }
                    }
                }

                bulkPlantIds = bulkPlantIds.Distinct().ToList();

                HelperDomain helperDomain = new HelperDomain(this);
                var bulkPlants = Context.DataContext.BulkPlantLocations.Where(t => bulkPlantIds.Contains(t.Id)
                            && t.IsActive && t.Name != null && t.Latitude != 0 && t.Longitude != 0).ToList();
                foreach (var item in bulkPlants)
                {
                    var bulkPlantModel = new BulkPlantViewModel();
                    bulkPlantModel.Id = item.Id;
                    bulkPlantModel.BulkPlantName = item.Name;
                    bulkPlantModel.Address = item.Address;
                    bulkPlantModel.City = item.City;
                    bulkPlantModel.CountryCode = item.CountryCode;
                    bulkPlantModel.CountyName = item.CountyName;
                    bulkPlantModel.StateCode = item.StateCode;
                    bulkPlantModel.ZipCode = item.ZipCode;
                    bulkPlantModel.Latitude = item.Latitude;
                    bulkPlantModel.Longitude = item.Longitude;
                    bulkPlantModel.Distance = Convert.ToDecimal(helperDomain.CalculateDistance(latitude, longitude, item.Latitude, item.Longitude));
                    response.Add(bulkPlantModel);
                }

                response = response.OrderBy(t => t.Distance).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetBulkPlantsForAutoFreightMethod", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetBulkPlantsForAutoFreightMethod(int orderId, string searchStringBulkPlant)
        {
            List<DropdownDisplayItem> bulkPlants = new List<DropdownDisplayItem>();
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new
                {
                    t.FuelRequest.FuelTypeId,
                    t.AcceptedCompanyId,
                    t.FuelRequest.Currency,
                    t.FuelRequest.Job.StateId,
                    t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId,
                }).FirstOrDefaultAsync();

                var pricingDetails = await ContextFactory.Current.GetDomain<OrderDomain>().GetRequestPricingDetail(order.RequestPriceDetailId, (int)order.Currency, order.AcceptedCompanyId, order.FuelTypeId, order.StateId);
                if (pricingDetails != null)
                {
                    if (!string.IsNullOrWhiteSpace(pricingDetails.SourceRegionJsonParameters))
                    {
                        SourceRegionJSONParameter sourceRegionsParameters = JsonConvert.DeserializeObject<SourceRegionJSONParameter>(pricingDetails.SourceRegionJsonParameters);
                        if (sourceRegionsParameters != null)
                        {
                            if (!string.IsNullOrWhiteSpace(sourceRegionsParameters.SelectedBulkPlants))
                            {
                                List<int> bulkPlantIds = sourceRegionsParameters.SelectedBulkPlants.Split(',').Select(int.Parse).ToList();
                                bulkPlants = Context.DataContext.BulkPlantLocations.Where(t => bulkPlantIds.Contains(t.Id)).Select(t => new DropdownDisplayItem { Id = t.Id, Name = t.Name }).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetBulkPlantsForAutoFreightMethod", ex.Message, ex);
            }
            return bulkPlants;
        }

        public async Task<List<DropdownDisplayItem>> GetClosestTerminalsForManualFreightMethod(int orderId, string searchStringTeminal)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                var order = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new
                {
                    t.FuelRequest.FuelTypeId,
                    t.TerminalId,
                    t.CityGroupTerminalId,
                    t.FuelRequest.Job.LocationType,
                    t.FuelRequest.Job.Latitude,
                    t.FuelRequest.Job.Longitude,
                    t.FuelRequest.Job.CountryId,
                    t.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                    //IsSupressOrderPricing = t.OrderAdditionalDetail.OnboardingPreference != null && t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing,
                    IsSupressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                    CompanyCountryId = t.Company.CompanyAddresses.Where(t1 => t1.IsDefault && t1.IsActive).Select(t1 => t1.CountryId).FirstOrDefault()
                }).FirstOrDefault();

                if (order != null)
                {
                    decimal latitude = order.Latitude, longitude = order.Longitude;
                    if (order.LocationType == JobLocationTypes.Various)
                    {
                        if (order.CityGroupTerminalId.HasValue && order.CityGroupTerminalId > 0)
                        {
                            var cityGroupTerminal = Context.DataContext.MstExternalTerminals.Where(t => t.Id == order.CityGroupTerminalId).Select(t => new { t.Latitude, t.Longitude }).FirstOrDefault();
                            if (cityGroupTerminal != null)
                            {
                                latitude = cityGroupTerminal.Latitude; longitude = cityGroupTerminal.Longitude;
                            }
                        }
                        //else
                        //{
                        //    latitude = 0; longitude = 0;
                        //}
                    }
                    int companyCountryId = order.CompanyCountryId > 0 ? order.CompanyCountryId : order.CountryId;

                    var terminalDetails = await pricingServiceDomain.GetClosestTerminalsAsync(order.CountryId, order.PricingCodeId, order.CompanyCountryId, order.FuelTypeId, latitude, longitude, searchStringTeminal, order.IsSupressOrderPricing);
                    if (terminalDetails.Any(t => t.Id == order.TerminalId))
                    {
                        var defaultTerminal = terminalDetails.Single(t => t.Id == order.TerminalId);
                        terminals.Insert(0, new DropdownDisplayItem { Id = defaultTerminal.Id, Name = defaultTerminal.Distance > 0 ? $"{defaultTerminal.Name} : {defaultTerminal.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}" : defaultTerminal.Name });
                    }

                    //Push rest of the elements
                    terminals.AddRange(terminalDetails.Where(t => t.Id != order.TerminalId).Select(t => new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = t.Distance > 0 ? $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}" : t.Name
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalsForManualFreightMethod", ex.Message, ex);
            }
            return terminals;
        }

        public async Task<List<TerminalViewModel>> GetClosestTerminals(List<int> orderIds, decimal lat, decimal lon)
        {
            var response = new List<TerminalViewModel>();
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                var orders = Context.DataContext.Orders.Where(t => orderIds.Contains(t.Id)).Select(t => new
                {
                    t.FuelRequest.FuelTypeId,
                    t.TerminalId,
                    t.CityGroupTerminalId,
                    t.FuelRequest.Job.LocationType,
                    t.FuelRequest.Job.CountryId,
                    t.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                    t.AcceptedCompanyId,
                    //IsSupressOrderPricing = t.OrderAdditionalDetail.OnboardingPreference != null && t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing
                    IsSupressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                    FreightPricingMethod = t.OrderAdditionalDetail != null ? t.OrderAdditionalDetail.FreightPricingMethod : FreightPricingMethod.Manual
                }).ToList();

                if (orders.Any() && orders != null)
                {
                    decimal latitude = lat, longitude = lon;
                    int acceptedCompanyId = orders[0].AcceptedCompanyId;
                    int companyCountryId = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == acceptedCompanyId && t.IsDefault && t.IsActive).Select(t => t.CountryId).FirstOrDefault();
                    companyCountryId = companyCountryId > 0 ? companyCountryId : orders[0].CountryId;
                    foreach (var order in orders)
                    {
                        if (order.FreightPricingMethod == FreightPricingMethod.Manual)
                        {
                            var terminalDetails = await pricingServiceDomain.GetClosestTerminalsAsync(order.CountryId, order.PricingCodeId, companyCountryId, order.FuelTypeId, latitude, longitude, string.Empty, order.IsSupressOrderPricing);
                            terminals.AddRange(terminalDetails.Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Distance > 0 ? $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}" : t.Name
                            }));
                        }
                        else if (order.FreightPricingMethod == FreightPricingMethod.Auto)
                        {
                            terminals = await GetClosestTerminalsForAutoFreightMethod(orderIds, string.Empty);
                        }
                    }

                    var terminalIds = terminals.Select(t => t.Id).Distinct().ToList();

                    HelperDomain helperDomain = new HelperDomain(this);
                    var externalTerminals = Context.DataContext.MstExternalTerminals.Where(t => terminalIds.Contains(t.Id)).ToList();
                    foreach (var item in externalTerminals)
                    {
                        var terminal = new TerminalViewModel();
                        terminal.TerminalId = item.Id;
                        terminal.TerminalName = item.Name;
                        terminal.Address = item.Address;
                        terminal.City = item.City;
                        terminal.StateCode = item.StateCode;
                        terminal.ZipCode = item.ZipCode;
                        terminal.Latitude = item.Latitude;
                        terminal.Longitude = item.Longitude;
                        terminal.Distance = helperDomain.CalculateDistance(lat, lon, item.Latitude, item.Longitude);
                        response.Add(terminal);
                    }

                    response = response.OrderBy(t => t.Distance).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminals", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetClosestTerminals(int fuelTypeId, decimal latitude, decimal longitude, int countryId, string searchStringTeminal, int pricingCodeId, int companyId = 0)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                int companyCountryId = 0;
                if (companyId > 0)
                {
                    companyCountryId = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsDefault && t.IsActive).Select(t => t.CountryId).FirstOrDefault();
                }
                companyCountryId = companyCountryId > 0 ? companyCountryId : countryId;
                PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                if (fuelTypeId > 0 && latitude != 0 && longitude != 0)
                {
                    var terminalDetails = await pricingServiceDomain.GetClosestTerminalsAsync(countryId, pricingCodeId, companyCountryId, fuelTypeId, latitude, longitude, searchStringTeminal);

                    terminals.Insert(0, new DropdownDisplayItem { Id = 0, Name = ResourceMessages.GetMessage(Resource.valMessageSelect, new object[] { Resource.lblTerminal }) });

                    //Push rest of the elements
                    terminals.AddRange(terminalDetails.Select(t => new DropdownDisplayItem
                    {
                        Id = t.Id,
                        Name = $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}"
                    }));
                }
                else
                {
                    terminals.Insert(0, new DropdownDisplayItem { Id = 0, Name = ResourceMessages.GetMessage(Resource.valMessageSelect, new object[] { Resource.lblTerminal }) });
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminals", ex.Message, ex);
            }
            return terminals;
        }

        public async Task<List<DropdownDisplayItem>> GetClosestTerminalsForOrders(int companyId, List<int> orderList, string searchStringTeminal)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                if (orderList != null && orderList.Any())
                {
                    var orderDetailsList = Context.DataContext.Orders.Where(t => orderList.Contains(t.Id)).Select(t => new
                    {
                        t.FuelRequest.FuelTypeId,
                        t.TerminalId,
                        t.CityGroupTerminalId,
                        t.FuelRequest.Job.LocationType,
                        t.FuelRequest.Job.Latitude,
                        t.FuelRequest.Job.Longitude,
                        t.FuelRequest.Job.CountryId,
                        t.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                        // IsSupressOrderPricing = t.OrderAdditionalDetail.OnboardingPreference != null && t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing
                        IsSupressOrderPricing = t.OrderAdditionalDetail.IsSupressPricingEnabled,
                        FreightPricingMethod = t.OrderAdditionalDetail != null ? t.OrderAdditionalDetail.FreightPricingMethod : FreightPricingMethod.Manual
                    }).ToList();

                    if (orderDetailsList.Any())
                    {
                        if (orderDetailsList != null)
                        {
                            int companyCountryId = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsActive && t.IsDefault).Select(t => t.CountryId).FirstOrDefault();
                            List<int> fuelTypeList = new List<int>();
                            var firstOrder = orderDetailsList.First();
                            companyCountryId = companyCountryId > 0 ? companyCountryId : firstOrder.CountryId;
                            foreach (var ord in orderDetailsList)
                            {
                                fuelTypeList.Add(ord.FuelTypeId);
                            }

                            if (firstOrder.FreightPricingMethod == FreightPricingMethod.Manual)
                            {
                                terminals = await GetTerminalsForMultipleProducts(firstOrder.CountryId,
                                                        firstOrder.PricingCodeId, fuelTypeList, companyCountryId,
                                                        orderDetailsList.Any(t => t.IsSupressOrderPricing),
                                                        firstOrder.Latitude, firstOrder.Longitude, searchStringTeminal ?? string.Empty);
                            }
                            else
                            {
                                terminals = await GetClosestTerminalsForAutoFreightMethod(orderList, searchStringTeminal ?? string.Empty);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalsForOrders", ex.Message, ex);
            }
            return terminals;
        }

        public async Task<List<DropdownDisplayItem>> GetTerminalsForMultipleProducts(int jobCountryId, int pricingCodeId, List<int> fuelTypeList, int companyCountryId, bool isSupressOrderPricing, decimal jobLatitude, decimal jobLongitude, string searchStringTeminal)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                var terminalDetails = await new PricingServiceDomain(this).GetClosestTerminalsForFueltypesAsync(jobCountryId,
                                                   pricingCodeId, fuelTypeList, companyCountryId,
                                                   isSupressOrderPricing,
                                                   jobLatitude, jobLongitude, searchStringTeminal ?? string.Empty);

                terminals.AddRange(terminalDetails.Select(t => new DropdownDisplayItem
                {
                    Id = t.Id,
                    Name = t.Distance > 0 ? $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}" : t.Name
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetTerminalsForMultipleProducts", ex.Message, ex);
            }
            return terminals;
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetClosestTerminalsForSourceRegions(int companyId, int companyCountryId, SourceRegionRequestModel inputModel)
        {
            List<DropdownDisplayExtendedItem> terminals = new List<DropdownDisplayExtendedItem>();
            try
            {
                companyCountryId = companyCountryId > 0 ? companyCountryId : inputModel.CountryId;
                PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                if (inputModel.FuelTypeId > 0 && inputModel.Latitude != 0 && inputModel.Longitude != 0)
                {
                    var terminalIds = string.Join(",", inputModel.TerminalIds);
                    var terminalDetails = await pricingServiceDomain.GetClosestTerminalsForSouceRegionsAsync(inputModel.CountryId, inputModel.PricingCodeId, companyCountryId, inputModel.FuelTypeId, inputModel.Latitude, inputModel.Longitude, terminalIds, inputModel.PricingTypeId);

                    if (terminalDetails != null && terminalDetails.Any())
                    {
                        //Push rest of the elements
                        terminals.AddRange(terminalDetails.Select(t => new DropdownDisplayExtendedItem
                        {
                            Id = t.Id,
                            Code=t.Distance.ToString(),
                            Name = $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}"
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalsForSourceRegions", ex.Message, ex);
            }
            return terminals;
        }

        #region PricingSync
        private static bool _startPricing;
        //private Task _pricingSyncTask;
        public static bool StartPricingSync()
        {
            _startPricing = true;
            return _startPricing;
        }

        //public bool TriggerOpisPricingTask()
        //{
        //    if (_pricingSyncTask == null)
        //    {
        //        _pricingSyncTask = Task.Factory.StartNew(ExecutePricingSync);
        //    }
        //    return true;
        //}

        public async Task ExecutePricingSync()
        {
            // while (true)
            // {
            var watch = Stopwatch.StartNew();
            try
            {
                var domain = ContextFactory.Current.GetDomain<StoredProcedureDomain>();

                if (!await domain.OPISPlattsPricingSyncStatus(false, true)) return;
                await domain.OPISPlattsPricingSyncStatus(false, false);
                // if (_startPricing)
                //  {
                //  _startPricing = false;
                using (var tracer = new Tracer("ExternalPricingDomain", "ExecutePricingSync"))
                {
                    bool isProcessInvoice = false;
                    var pricingServiceDomain = new PricingServiceDomain(this);
                    var pricingResponse = await pricingServiceDomain.SyncOPISPlattsPricing();
                    if (pricingResponse != null && pricingResponse.Status == Status.Success && pricingResponse.PricingResponse != null && pricingResponse.PricingResponse.Count > 0 && pricingResponse.PricingResponse.FirstOrDefault().RecordInserted > 0)
                    {
                        isProcessInvoice = true;
                        await SaveNewProduct(pricingResponse);
                    }

                    // Sync actual opis pricing data.
                    var syncActualOpisPricingResponse = await pricingServiceDomain.SyncActualOPISPricing();
                    if (syncActualOpisPricingResponse != null && syncActualOpisPricingResponse.Status == Status.Success && syncActualOpisPricingResponse.PricingResponse != null && syncActualOpisPricingResponse.PricingResponse.Count > 0 && syncActualOpisPricingResponse.PricingResponse.FirstOrDefault().RecordInserted > 0)
                    {
                        isProcessInvoice = true;
                        await SaveNewProduct(syncActualOpisPricingResponse, true);
                    }
                    Thread.Sleep(1000);
                    if (isProcessInvoice)
                        await ProcessInvoicesWaitingForUpdatedPrice();
                }
                // }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "ExecutePricingSync", ex.Message, ex);
            }
            watch.Stop();
            LogManager.Logger.WriteInfo("CJob.ExecutePricingSync", "ExecutePricingSync", "End:TotalTime:" + watch.ElapsedMilliseconds);
        }

        private async Task SaveNewProduct(SyncPricingResponseModel pricingResponse, bool isActualOpis = false)
        {
            // need to call insert product api with pricingResponse (pricingResponse.PricingResponse.ProductId != null)
            var newProducts = pricingResponse.PricingResponse.Where(t => t.ProductId.HasValue).ToList();
            if (newProducts.Any())
            {
                var productDomain = new ProductDomain(this);
                if (!isActualOpis)
                {
                    await productDomain.SaveOpisPlattsProduct(newProducts);
                }
                else
                {
                    await productDomain.SaveActualOpisProduct(newProducts);
                }
            }
        }

        private async Task ProcessInvoicesWaitingForUpdatedPrice()
        {
            using (var tracer = new Tracer("ExternalPricingDomain", "ProcessInvoicesWaitingForUpdatedPrice"))
            {
                var invoiceDomain = new ConsolidatedDdtToInvoiceDomain(this);
                await invoiceDomain.ProcessInvoicesWaitingForUpdatedPrice();
            }
        }
        #endregion
        #region OptionPickupLocation
        public async Task<List<DropdownDisplayItem>> GetClosestTerminalsForOptionalPickup(int companyId, List<int> orderList, string searchStringTeminal, List<int> fuelTypeId)
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                if (orderList != null && orderList.Any())
                {
                    PricingServiceDomain pricingServiceDomain = new PricingServiceDomain(this);
                    var orderDetailsList = Context.DataContext.Orders.Where(t => orderList.Contains(t.Id)).Select(t => new
                    {
                        t.FuelRequest.FuelTypeId,
                        t.TerminalId,
                        t.CityGroupTerminalId,
                        t.FuelRequest.Job.LocationType,
                        t.FuelRequest.Job.Latitude,
                        t.FuelRequest.Job.Longitude,
                        t.FuelRequest.Job.CountryId,
                        t.FuelRequest.FuelRequestPricingDetail.PricingCodeId,
                        //IsSupressOrderPricing = t.OrderAdditionalDetail.OnboardingPreference != null && t.OrderAdditionalDetail.OnboardingPreference.IsSupressOrderPricing
                        IsSupressOrderPricing=t.OrderAdditionalDetail.IsSupressPricingEnabled
                    }).ToList();

                    if (orderDetailsList.Any())
                    {
                        if (fuelTypeId.Any())
                        {
                            orderDetailsList = orderDetailsList.Where(top => fuelTypeId.Contains(top.FuelTypeId)).ToList();
                        }
                        if (orderDetailsList != null)
                        {
                            int companyCountryId = Context.DataContext.CompanyAddresses.Where(t => t.CompanyId == companyId && t.IsActive && t.IsDefault).Select(t => t.CountryId).FirstOrDefault();
                            List<int> fuelTypeList = new List<int>();
                            var firstOrder = orderDetailsList.First();
                            companyCountryId = companyCountryId > 0 ? companyCountryId : firstOrder.CountryId;
                            foreach (var ord in orderDetailsList)
                            {
                                fuelTypeList.Add(ord.FuelTypeId);
                            }

                            var terminalDetails = await pricingServiceDomain.GetClosestTerminalsForFueltypesAsync(firstOrder.CountryId,
                                                    firstOrder.PricingCodeId, fuelTypeList, companyCountryId,
                                                    orderDetailsList.Any(t => t.IsSupressOrderPricing),
                                                    firstOrder.Latitude, firstOrder.Longitude, searchStringTeminal ?? string.Empty);

                            terminals.AddRange(terminalDetails.Select(t => new DropdownDisplayItem
                            {
                                Id = t.Id,
                                Name = t.Distance > 0 ? $"{t.Name} : {t.Distance.ToString("N2")}{(companyCountryId == (int)Country.CAN ? Resource.lblKiloMeters : Resource.lblMiles.ToLower())}" : t.Name
                            }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetClosestTerminalsForOrders", ex.Message, ex);
            }
            return terminals;
        }
        public async Task<OrderFuelInfo> GetOrderFuelType(List<int> orderList)
        {
            OrderFuelInfo orderFuelInfo = new OrderFuelInfo();
            List<OrderFuelDetails> orderFuelDetails = new List<OrderFuelDetails>();
            try
            {
                if (orderList != null && orderList.Any())
                {   
                    var orderDetailsList = await Context.DataContext.Orders.Where(t => orderList.Contains(t.Id)).Select(t => new
                    {
                        t.Id,
                        t.FuelRequest.FuelTypeId,
                        Name = !string.IsNullOrEmpty(t.FuelRequest.MstProduct.DisplayName) ? t.FuelRequest.MstProduct.DisplayName : t.FuelRequest.MstProduct.Name
                    }).ToListAsync();
                    foreach (var item in orderDetailsList)
                    {
                        var orderFuelIndex = orderFuelDetails.FindIndex(x => x.OrderId == item.Id);
                        if (orderFuelIndex == -1)
                        {
                            OrderFuelDetails orderInfo = new OrderFuelDetails();
                            orderInfo.OrderId = item.Id;
                            orderInfo.FuelTypeDetails.Add(new DropdownDisplayItem { Id = item.FuelTypeId, Name = item.Name });
                            orderFuelDetails.Add(orderInfo);
                        }
                        else
                        {
                            var orderFuel = orderFuelDetails.FirstOrDefault(x => x.OrderId == item.Id);
                            if (orderFuel != null)
                            {
                                var orderFuelTypeExists = orderFuel.FuelTypeDetails.FindIndex(x => x.Id == item.FuelTypeId);
                                if (orderFuelTypeExists == -1)
                                {
                                    orderFuel.FuelTypeDetails.Add(new DropdownDisplayItem { Id = item.FuelTypeId, Name = item.Name });
                                }
                            }
                        }
                    }
                    orderFuelInfo.OrderFuelDetails = orderFuelDetails;
                }
            }
            catch (Exception ex)
            {
                orderFuelInfo.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetOrderFuelType", ex.Message, ex);
            }
            return orderFuelInfo;
        }
        public async Task<List<DropdownDisplayItem>> GetTBDTerminalsForOrders()
        {
            List<DropdownDisplayItem> terminals = new List<DropdownDisplayItem>();
            try
            {
                terminals = await new PricingServiceDomain(this).GetAllTerminals();

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ExternalPricingDomain", "GetTBDTerminalsForOrders", ex.Message, ex);
            }
            return terminals;
        }

        #endregion
    }
}
