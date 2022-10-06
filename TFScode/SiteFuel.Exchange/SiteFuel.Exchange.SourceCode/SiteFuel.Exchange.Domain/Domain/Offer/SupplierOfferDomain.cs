using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class SupplierOfferDomain : BaseDomain
    {
        public SupplierOfferDomain()
          : base(ContextFactory.Current.ConnectionString)
        {
        }

        public SupplierOfferDomain(SiteFuelUow SiteFuelDbContext) : base(SiteFuelDbContext)
        {
        }

        public List<OfferViewModel> GetSupplierOfferGridAsync(int companyId, OfferSummaryFilter filter, int countryId, int currency)
        {
            var response = new List<OfferViewModel>();
            try
            {
                var offerPricings = Context.DataContext.OfferPricings.Where(t =>
               t.IsActive &&
               t.CountryId == countryId &&
               t.SupplierCompanyId == companyId
               &&
               (filter.FuelTypes == null || t.MstProduct.MstTFXProduct.Name.Contains(filter.FuelTypes)) &&
                   t.OfferPricingItems.Any
                   (
                       y => y.IsActive
                   &&
                   (filter.Name == null || t.Name.Contains(filter.Name))
                   &&
                   ((filter.CustomerIds.Count > 0 &&
                   (y.CustomerId != null && filter.CustomerIds.Contains(y.CustomerId.Value) ||
                   y.TierId != null && t.SupplierCompany.OfferTierMappings.Any(m => y.TierId == m.TierId && filter.CustomerIds.Contains(m.BuyerCompanyId) && m.IsActive))
                   ) || filter.CustomerIds.Count == 0)
                   &&
                       (filter.Customers == null || y.CustomerId != null && y.Company.Name.Contains(filter.Customers)
                            && (y.TierId == null || t.SupplierCompany.OfferTierMappings.Any(m => y.TierId == m.TierId && m.BuyerCompany.Name.Contains(filter.Customers)))
                       )
                   &&
                   (filter.Tier == null || y.TierId != null && y.MstTierType.Name.Contains(filter.Tier))
                   )
                );

                if (filter.OfferEnumType != OfferType.All)
                {
                    offerPricings = offerPricings.Where(x => x.OfferTypeId == (int)filter.OfferEnumType);
                }


                var offerRes = offerPricings.SelectMany(y => y.OfferPricingItems).Where(x => x.IsActive).
                Select(x =>
                new
                {
                    City = x.MstCity == null ? null : x.MstCity.Name,
                    zip = x.ZipCode,
                    State = x.MstState == null ? null : x.MstState.Name,
                    CustomerNames = x.Company.Name,
                    Name = x.OfferPricing.Name,
                    OfferTypeId = x.OfferPricing.OfferTypeId,
                    TierNames = x.MstTierType == null ? null : x.MstTierType.Name,
                    Id = x.OfferPricing.Id,
                    FuelTypeName = x.OfferPricing.MstTfxProduct.Name,
                    FuelPricing = x.OfferPricing,
                    FuelFees = x.OfferPricing.FuelFees,
                    DifferentFuelPrices = x.OfferPricing.DifferentFuelPrices,
                    x.OfferPricing.Currency,
                    x.OfferPricing.TruckLoadType,
                    x.UpdatedDate
                }).ToList();

                var offerPrices = offerRes.GroupBy(x => new { x.Id });

                foreach (var price in offerPrices)
                {
                    var offer = new OfferViewModel
                    {
                        LocationViewModel = price.GroupBy(x =>
                        new
                        {
                            x.City,
                            x.State,

                        }).Select(x => new OfferLocationViewModel
                        {

                            City = x.Key.City,
                            State = x.Key.State,
                            ZipStringList = x.Select(y => y.zip).Distinct().ToList()
                        }).ToList(),
                        Id = price.FirstOrDefault().Id,
                        CustomerNames = price.Select(x => x.CustomerNames).Distinct().ToList(),
                        Name = $"{price.FirstOrDefault().Name}",
                        FuelTypeName = price.FirstOrDefault().FuelTypeName,
                        TierNames = price.Select(x => x.TierNames).Distinct().ToList(),
                        OfferTypeId = price.FirstOrDefault().OfferTypeId,
                        FuelPricing = OfferMapper.ToFuelPricingViewModel(price.FirstOrDefault().FuelPricing, null),
                        CreatedDate = price.FirstOrDefault().FuelPricing.CreatedDate,
                        UpdatedDate = price.FirstOrDefault().UpdatedDate ?? price.FirstOrDefault().FuelPricing.CreatedDate 
                    };
                    offer.FuelDeliveryDetails.FuelFees.Currency = price.FirstOrDefault().Currency;
                    offer.FuelDeliveryDetails.FuelFees.UoM = price.FirstOrDefault().Currency == Currency.USD ? UoM.Gallons : UoM.Litres;
                    offer.FuelPricing.Currency = price.FirstOrDefault().Currency;
                    offer.FuelPricing.FuelPricingDetails.TruckLoadTypes = price.FirstOrDefault().TruckLoadType;
                    offer.FuelPricing.SetPricePerGallon();
                    offer.FuelPricing.DifferentFuelPrices = price.FirstOrDefault().DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
                    offer.FuelDeliveryDetails.FuelFees.FuelRequestFees = price.FirstOrDefault().FuelPricing.FuelFees.ToFeesViewModel();
                    
                    response.Add(offer.DoNullCleaning());
                }
                response = response.OrderByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetSupplierOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        public OfferViewModel GetSupplierOfferDetail(int companyId, int offerPriceId)
        {
            var response = new OfferViewModel();
            try
            {
                var offerPricings = Context.DataContext.OfferPricings.Where(t =>
               t.IsActive &&
               t.SupplierCompanyId == companyId && t.Id == offerPriceId
               && t.OfferPricingItems.Any(y => y.IsActive));

                var offerRes = offerPricings.SelectMany(y => y.OfferPricingItems).
                Select(x =>
               new
               {
                   City = x.MstCity == null ? null : x.MstCity.Name,
                   x.CityId,
                   zip = x.ZipCode,
                   State = x.MstState == null ? null : x.MstState.Name,
                   x.StateId,
                   x.OfferPricing.CountryId,
                   x.OfferPricing.PricingSource,
                   x.OfferPricing.TruckLoadType,
                   x.OfferPricing.Currency,
                   x.OfferPricing.UoM,
                   CustomerNames = x.Company.Name,
                   x.OfferPricing.Name,
                   x.OfferPricing.OfferTypeId,
                   TierName = x.MstTierType == null ? null : x.MstTierType.Name,
                   x.TierId,
                   x.CustomerId,
                   offerPricingId = x.OfferPricing.Id,
                   FuelTypeName = x.OfferPricing.MstTfxProduct.Name,
                   x.OfferPricing.FuelTypeId,
                   FuelPricing = x.OfferPricing,
                   x.OfferPricing.FuelFees,
                   x.OfferPricing.DifferentFuelPrices,
                   x.OfferPricing.OfferChainId,
                   isMarketOfferExist = x.OfferPricing.OfferTypeId == (int)OfferType.Exclusive 
                                                && Context.DataContext.OfferPricingDetails.Where(t => t.OfferPricingId != offerPriceId && t.IsActive
                                                && t.OfferPricing.OfferTypeId == (int)OfferType.Market && t.OfferPricing.FuelTypeId == x.OfferPricing.FuelTypeId
                                                && t.StateId == x.StateId && t.CityId == x.CityId && t.ZipCode == x.ZipCode
                                                && t.OfferPricing.StatusId == (int)OfferStatus.Open
                                                && t.OfferPricing.SupplierCompanyId == companyId).Count() > 0

               }).ToList();

                var offerPrice = offerRes.GroupBy(x => new { x.offerPricingId }).FirstOrDefault();
                var countryId = offerPrice.FirstOrDefault().CountryId;
                var offer = new OfferViewModel
                {
                    LocationViewModel = offerPrice.GroupBy(x =>
                    new
                    {
                        x.City,
                        x.CityId,
                        x.StateId,
                        x.State,

                    }).Select(x => new OfferLocationViewModel
                    {
                        City = x.Key.City,
                        CityId = x.Key.CityId,
                        StateId = x.Key.StateId,
                        State = x.Key.State,
                        ZipStringList = x.Select(y => y.zip).Distinct().ToList(),
                        ZipList = x.Select(y => new DropdownDisplayExtended { Code = y.zip, Name = y.zip, Id = y.zip }).ToList(),
                        CountryId = countryId
                    }).ToList(),
                    CompanyId = companyId,
                    Customers = offerPrice.Where(x => x.CustomerId.HasValue).Select(x => x.CustomerId.Value).Distinct().ToList(),
                    Tiers = offerPrice.Where(x => x.TierId.HasValue).Select(x => x.TierId.Value).ToList(),
                    Id = offerPrice.FirstOrDefault().offerPricingId,
                    CustomerNames = offerPrice.Where(x => x.CustomerId.HasValue).Select(x => x.CustomerNames).Distinct().ToList(),
                    Name = offerPrice.FirstOrDefault().Name,
                    FuelTypeName = offerPrice.FirstOrDefault().FuelTypeName,
                    TierNames = offerPrice.Where(t => t.TierId.HasValue).Select(x => x.TierName).Distinct().ToList(),
                    OfferTypeId = offerPrice.FirstOrDefault().OfferTypeId,
                    FuelTypes = offerPrice.Select(x => x.FuelTypeId).Distinct().ToList(),
                    FuelPricing = OfferMapper.ToFuelPricingViewModel(offerPrice.FirstOrDefault().FuelPricing, null),
                    CreatedDate = offerPrice.FirstOrDefault().FuelPricing.CreatedDate,
                    UserId = offerPrice.FirstOrDefault().FuelPricing.CreatedBy,
                    Guid = offerPrice.FirstOrDefault().OfferChainId
                };
                offer.CountryId = countryId;
                offer.FuelDeliveryDetails.FuelFees.Currency = offerPrice.FirstOrDefault().Currency;
                offer.FuelDeliveryDetails.FuelFees.UoM = offerPrice.FirstOrDefault().UoM;
                offer.FuelPricing.Currency = offerPrice.FirstOrDefault().Currency;
                offer.FuelPricing.FuelPricingDetails.PricingSourceId = (int) offerPrice.FirstOrDefault().PricingSource;
                offer.FuelPricing.FuelPricingDetails.TruckLoadTypes = offerPrice.FirstOrDefault().TruckLoadType;

                offer.FuelPricing.IsTierPricingRequired = true;
                offer.FuelPricing.SetPricePerGallon();
                offer.FuelPricing.DifferentFuelPrices = offerPrice.FirstOrDefault().DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
                offer.FuelDeliveryDetails.FuelFees.FuelRequestFees = offerPrice.FirstOrDefault().FuelPricing.FuelFees.ToFeesViewModel();
                SetOfferLocationType(offer);
                if (!offerPrice.Any(t => t.isMarketOfferExist) && offer.OfferTypeId == (int)OfferType.Exclusive)
                {
                    offer.IsApplicableToLaunch = true;
                }
                offer.IsQuickUpdated = offerPricings.SelectMany(t => t.OfferPricingItems)
                                        .Any(t => t.IsActive && t.UpdateCommandId != null);
                if (offer.TierNames.Count == 0) { offer.TierNames.Add(Resource.lblHyphen); }
                if (offer.CustomerNames.Count == 0) { offer.CustomerNames.Add(Resource.lblHyphen); }
                return offer.DoNullCleaning();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetSupplierOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetProductsOfExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);

                response = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value)))
                            .Select(x => new DropdownDisplayItem
                            {
                                Id = x.OfferPricing.MstProduct.Id,
                                Name = x.OfferPricing.MstTfxProduct.Name
                            }).Distinct()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetProductsOfExistingOffers", ex.Message, ex);
            }
            return response;
        }


        public async Task<List<DropdownDisplayItem>> GetStatesForExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);
                var offersStates = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value))
                                    && t.OfferPricing.FuelTypeId == filter.FuelTypeId)
                            .Select(x => x.StateId).Distinct()
                            .ToListAsync();
                if (offersStates.Any(t => !t.HasValue))
                {
                    var masterDomain = new MasterDomain(this);
                    response = masterDomain.GetStates((int)Country.USA);
                }
                else
                {
                    response = await Context.DataContext.MstStates.Where(t => offersStates.Contains(t.Id))
                                            .Select(x => new DropdownDisplayItem { Id = x.Id, Name = x.Name }).Distinct().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetStatesForExistingOffersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetCitiesForExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);
                var stateList = string.IsNullOrEmpty(filter.StateId) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.StateId);
                var offersCities = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value))
                                    && t.OfferPricing.FuelTypeId == filter.FuelTypeId)
                            .Select(x => new { x.StateId, x.CityId }).Distinct()
                            .ToListAsync();
                if (offersCities.Any(t => !t.StateId.HasValue || stateList.Contains(t.StateId.Value) && !t.CityId.HasValue))
                {
                    var masterDomain = new MasterDomain(this);
                    response = masterDomain.GetCities(stateList.First(), false);
                }
                else if (stateList.Count == 1)
                {
                    var cityId = offersCities.FirstOrDefault(t => t.StateId == stateList.FirstOrDefault())?.CityId;
                    response = await Context.DataContext.MstCities.Where(t => t.Id == cityId)
                                            .Select(x => new DropdownDisplayItem { Id = x.Id, Name = x.Name }).Distinct().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetCitiesForExistingOffersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayExtended>> GetZipsForExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayExtended>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);
                var stateList = string.IsNullOrEmpty(filter.StateId) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.StateId);

                var data = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value)))
                            .Select(x => new { x.ZipCode, x.StateId, x.CityId }).Distinct()
                            .ToListAsync();

                if (data.Any(t => !t.StateId.HasValue || stateList.Contains(t.StateId.Value) && !t.CityId.HasValue || stateList.Contains(t.StateId.Value) && t.CityId.HasValue && string.IsNullOrEmpty(t.ZipCode)))
                {
                    response = await ContextFactory.Current.GetDomain<ZipCodeServiceDomain>().GetZipCodeList(filter.StateName, filter.CityName);
                }
                else
                {
                    response = data.Where(t => t.StateId.HasValue && stateList.Contains(t.StateId.Value)).Select(x => new DropdownDisplayExtended { Code = x.ZipCode, Name = x.ZipCode }).Distinct()
                                .ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetZipsForExistingOffersAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DropdownDisplayItem>> GetFeeTypeForExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);
                var stateList = string.IsNullOrEmpty(filter.StateId) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.StateId);
                var zipList = string.IsNullOrEmpty(filter.Zips) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(filter.Zips);

                var data = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value))
                                    && t.OfferPricing.FuelTypeId == filter.FuelTypeId
                                    && t.OfferPricing.FuelFees.Count > 0
                                    && (stateList.Count == 0 || !t.StateId.HasValue
                                        || (filter.CityId == 0 || !t.CityId.HasValue) && stateList.Count > 0 && t.StateId.HasValue && stateList.Contains(t.StateId.Value)
                                        || (zipList.Count == 0 || string.IsNullOrEmpty(t.ZipCode)) && filter.CityId > 0 && t.CityId.HasValue && t.CityId.Value == filter.CityId
                                        || (zipList.Count > 0 && !string.IsNullOrEmpty(t.ZipCode) && zipList.Contains(t.ZipCode)))
                                    )
                            .SelectMany(x => x.OfferPricing.FuelFees
                            .Select(t1 => new { t1.FeeTypeId, t1.FeeDetails, Name = t1.OfferPricing.Currency == Currency.CAD ? t1.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : t1.MstFeeType.Name }).ToList()).Distinct()
                            .ToListAsync();


                foreach (var item in data)
                {
                    if (item.FeeTypeId == (int)FeeType.OtherFee)
                    {
                        response.Add(new DropdownDisplayItem { Id = item.FeeTypeId, Name = item.FeeDetails });
                    }
                    else
                    {
                        response.Add(new DropdownDisplayItem { Id = item.FeeTypeId, Name = item.Name });
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetFeeTypeForExistingOffersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetFeeSubTypeForExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);
                var stateList = string.IsNullOrEmpty(filter.StateId) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.StateId);
                var zipList = string.IsNullOrEmpty(filter.Zips) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(filter.Zips);

                response = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value))
                                    && t.OfferPricing.FuelTypeId == filter.FuelTypeId
                                    && t.OfferPricing.FuelFees.Where(t2 => t2.FeeTypeId == filter.FeeTypeId).Count() > 0
                                    && (stateList.Count == 0 || !t.StateId.HasValue
                                        || (filter.CityId == 0 || !t.CityId.HasValue) && stateList.Count > 0 && t.StateId.HasValue && stateList.Contains(t.StateId.Value)
                                        || (zipList.Count == 0 || string.IsNullOrEmpty(t.ZipCode)) && filter.CityId > 0 && t.CityId.HasValue && t.CityId.Value == filter.CityId
                                        || (zipList.Count > 0 && !string.IsNullOrEmpty(t.ZipCode) && zipList.Contains(t.ZipCode)))
                                    )
                            .Select(x => x.OfferPricing.FuelFees.Where(t2 => t2.FeeTypeId == filter.FeeTypeId)
                            .Select(t1 => new DropdownDisplayItem { Id = t1.FeeSubTypeId, Name = t1.OfferPricing.Currency == Currency.CAD ? t1.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : t1.MstFeeType.Name }).FirstOrDefault()).Distinct()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetFeeTypeForExistingOffersAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DropdownDisplayItem>> GetPricingTypesForExistingOffersAsync(int supplierCompanyId, QuickUpdateFilterViewModel filter)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var customerList = string.IsNullOrEmpty(filter.Customers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Customers);
                var tierList = string.IsNullOrEmpty(filter.Tiers) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.Tiers);
                var stateList = string.IsNullOrEmpty(filter.StateId) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(filter.StateId);
                var zipList = string.IsNullOrEmpty(filter.Zips) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(filter.Zips);

                response = await Context.DataContext.OfferPricingDetails
                            .Where(t => t.OfferPricing.OfferTypeId == filter.OfferTypeId
                                    && t.OfferPricing.SupplierCompanyId == supplierCompanyId
                                    && t.IsActive && t.OfferPricing.IsActive
                                    && t.OfferPricing.CountryId == (int)filter.Country
                                    && t.OfferPricing.Currency == filter.CurrencyType
                                    && t.OfferPricing.TruckLoadType == filter.TruckLoadType
                                    && t.OfferPricing.PricingSource == filter.PricingSource
                                    && (filter.OfferTypeId == (int)OfferType.Market
                                        || customerList.Count() == 0 && tierList.Count == 0
                                        || (t.CustomerId.HasValue && customerList.Contains(t.CustomerId.Value))
                                        || t.TierId.HasValue && tierList.Contains(t.TierId.Value))
                                    && t.OfferPricing.FuelTypeId == filter.FuelTypeId
                                    && (stateList.Count == 0 || !t.StateId.HasValue
                                        || (filter.CityId == 0 || !t.CityId.HasValue) && stateList.Count > 0 && t.StateId.HasValue && stateList.Contains(t.StateId.Value)
                                        || (zipList.Count == 0 || string.IsNullOrEmpty(t.ZipCode)) && filter.CityId > 0 && t.CityId.HasValue && t.CityId.Value == filter.CityId
                                        || (zipList.Count > 0 && !string.IsNullOrEmpty(t.ZipCode) && zipList.Contains(t.ZipCode)))
                                    )
                            .Select(x => new DropdownDisplayItem { Id = x.OfferPricing.PricingTypeId, Name = x.OfferPricing.MstPricingType.Name }).Distinct()
                            .ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("SuplierOfferDomain", "GetPricingTypesForExistingOffersAsync", ex.Message, ex);
            }
            return response;
        }

        private static void SetOfferLocationType(OfferViewModel offer)
        {
            if (offer.LocationViewModel.All(t => t.CityId == null && t.ZipStringList.FirstOrDefault() == null))
            {
                offer.OfferLocationTypeId = (int)OfferLocationType.State;
                if (offer.LocationViewModel.Any(t => t.StateId.HasValue))
                {
                    offer.States = offer.LocationViewModel.Select(t => t.StateId.Value).ToList();
                }
                else
                {
                    offer.States.Add(0);
                }
            }
            else
            {
                offer.OfferLocationTypeId = (int)OfferLocationType.City;
                offer.LocationViewModel.ForEach(x =>
                {
                    x.ZipList.Add(new DropdownDisplayExtended { Code = "0", Id = "0", Name = Resource.lblSelectAll });
                    if (!x.ZipStringList.Any(y => y != null))
                    {
                        x.ZipStringList.Add("0");
                    }
                });
            }
        }
    }
}
