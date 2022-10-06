using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using SiteFuel.Exchange.ViewModels.Queue;
//using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class OfferDomain : BaseDomain
    {
        private static Random rndGenerator = new Random();
        private readonly int _allStates = 0;
        public OfferDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public OfferDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<OfferQuickUpdateViewModel> InitialisePricingTable(UserContext userContext, int countryId)
        {
            var pricingTableViewModel = new OfferQuickUpdateViewModel() { CompanyId = userContext.CompanyId, UserId = userContext.Id, CountryId = countryId };
            pricingTableViewModel.QuickUpdatePreferenceSetting = new QuickUpdatePreferenceSetting() { QuickUpdateType = (int)OfferQuickUpdateType.Fees };

            var preferences = await Context.DataContext.OfferQuickUpdatePreferences.FirstOrDefaultAsync(t => t.CompanyId == userContext.CompanyId && t.IsActive);
            if (preferences != null)
            {
                pricingTableViewModel.QuickUpdatePreferenceSetting.Id = preferences.Id;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsCustomerTier = preferences.IsCustomerTier;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsCustomer = preferences.IsCustomer;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsCity = preferences.IsCity;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsFeeType = preferences.IsFeeType;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsMarketOffer = preferences.IsMarketOffer;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsPricingType = preferences.IsPricingType;
                pricingTableViewModel.QuickUpdatePreferenceSetting.IsState = preferences.IsState;
            }

            pricingTableViewModel.OfferTypeId = pricingTableViewModel.QuickUpdatePreferenceSetting.IsMarketOffer ? (int)OfferType.Market : (int)OfferType.Exclusive;
            return pricingTableViewModel;
        }

        public async Task<BuyerOfferViewodel> GetBuyerOfferViewModel(UserContext userContext, Currency currency, Country country)
        {
            try
            {
                var response = new BuyerOfferViewodel() { CountryId = (int)country, CurrencyId = (int)currency, CompanyId = userContext.CompanyId, UserId = userContext.Id, OfferTypeId = (int)OfferType.Exclusive };
                response.SearchLocationViewModel.CountryId = (int)country;
                response.FuelDeliveryDetails.FuelFees.Currency = currency; //USA currency
                response.FuelDeliveryDetails.FuelFees.UoM = country == Country.USA ? UoM.Gallons : UoM.Litres; //USA unit of measurement
                response.FuelPricing.Currency = currency;
                response.FuelPricing.FuelPricingDetails.PricingSourceId = (int)PricingSource.Axxis;
                response.FuelPricing.FuelPricingDetails.TruckLoadTypes = TruckLoadTypes.LessTruckLoad;
                //response.FuelPricing.FuelPricingDetails.FeedTypeId = (int)PricingSourceFeedTypes.Contract_10AM_EST;
                //response.FuelPricing.FuelPricingDetails.PricingSourceQuantityIndicatorTypes = QuantityIndicatorTypes.Net;
                //response.FuelPricing.FuelPricingDetails.FuelClassTypes = FuelClassTypes.Branded;

                StoredProcedureDomain storedProcedureDomain = new StoredProcedureDomain(this);
                response.FilterFuelTypes = await storedProcedureDomain.GetAllOfferProductsForCountry(userContext.CompanyId, (int)country);
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetBuyerOfferViewModel", ex.Message, ex);
            }
            return new BuyerOfferViewodel();
        }

        public OfferOrderViewModel GetOfferAcceptViewModel(int jobId, int fueltypeId, int offerPricingId, int quantity, int truckLoadType, int source)
        {
            var response = new OfferOrderViewModel();
            try
            {
                response.AddressDetails.IsNewJob = jobId <= 0;
                response.AddressDetails.JobId = jobId;
                response.OfferPricingId = offerPricingId;
                response.FuelTypes = GetOfferApplicableFuelTypes(fueltypeId);
                var selectedFuelType = response.FuelTypes.FirstOrDefault(t => t.Id == fueltypeId);
                if (response.FuelTypes != null && selectedFuelType != null)
                {
                    response.FuelDetails.FuelTypeId = fueltypeId;
                    response.FuelDetails.FuelType = selectedFuelType.Name;
                }
                response.FuelDetails.FuelDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType;
                response.FuelDetails.FuelQuantity.QuantityTypeId = (int)QuantityType.SpecificAmount;
                response.FuelDetails.FuelQuantity.Quantity = quantity;
                response.FuelDetails.FuelQuantity.UoM = UoM.Gallons;
                response.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = (TruckLoadTypes)truckLoadType;
                response.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId = source;
                response.FuelDeliveryDetails.DeliveryTypeId = quantity > 500 ? (int)DeliveryType.MultipleDeliveries : (int)DeliveryType.OneTimeDelivery;
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errLoadOfferDetailsFailed;
                LogManager.Logger.WriteException("OfferDomain", "GetOfferAcceptViewModel", ex.Message, ex);
            }
            return response;
        }

        public OfferViewModel GetOfferViewModel(UserContext userContext, Currency currency, Country country)
        {
            try
            {
                var response = new OfferViewModel() { CountryId = (int)country, CurrencyId = (int)currency, CompanyId = userContext.CompanyId, UserId = userContext.Id, OfferTypeId = (int)OfferType.Exclusive, CreatedDate = DateTimeOffset.Now };
                response.FuelDeliveryDetails.FuelFees.Currency = currency; //USA currency
                response.FuelDeliveryDetails.FuelFees.UoM = country == Country.USA ? UoM.Gallons : UoM.Litres;
                response.FuelPricing.FuelPricingDetails.TruckLoadTypes = TruckLoadTypes.LessTruckLoad;
                response.FuelPricing.FuelPricingDetails.PricingSourceId = (int)PricingSource.Axxis;
                response.FuelPricing.Currency = currency;
                response.FuelPricing.IsTierPricingRequired = true;
                response.FuelPricing.DifferentFuelPrices.Add(new DifferentFuelPriceViewModel());
                response.LocationViewModel.Add(new OfferLocationViewModel() { CountryId = (int)country });
                response.OfferLocationTypeId = (int)OfferLocationType.State;
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetOfferViewModel", ex.Message, ex);
            }
            return new OfferViewModel();
        }

        private async Task<FuelRequest> CreateFuelRequestFromOffer(OfferOrderViewModel viewModel, OfferPricing offerPricing, UserContext userContext, Job job)
        {
            try
            {
                //FuelRequests Entity
                var buyerUser = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userContext.Id);
                var fuelRequest = viewModel.ToFuelRequestEntityFromOffer(offerPricing, buyerUser);
                fuelRequest.CreatedBy = buyerUser.Id;

                ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain(this);
                if (viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId == (int)PricingSource.Axxis)
                {
                    // set terminal id
                    ExternalPricingDataViewModel externalPricingData = await externalPricingDomain.GetClosestTerminalPriceAsync(viewModel.AddressDetails.JobId.Value, fuelRequest.FuelTypeId);

                    if (externalPricingData.TerminalId != 0)
                    {
                        fuelRequest.TerminalId = externalPricingData.TerminalId;
                    }
                }
                else
                {
                    var terminals = await externalPricingDomain.GetClosestTerminals(viewModel.FuelDetails.FuelTypeId.Value, job.Latitude,
                                                                            job.Longitude, viewModel.AddressDetails.Country.Id, string.Empty,
                                                                            fuelRequest.FuelRequestPricingDetail.PricingCodeId);
                    var terminal = terminals.FirstOrDefault(t => t.Id > 0);
                    if (terminal != null)
                    {
                        fuelRequest.TerminalId = terminal.Id;
                    }

                    fuelRequest.CityGroupTerminalId = viewModel.FuelDetails.FuelPricing.CityGroupTerminalId;
                    var response = await externalPricingDomain.GetTerminalPriceAsync(fuelRequest.TerminalId, viewModel.FuelDetails.FuelTypeId.Value, DateTimeOffset.Now, fuelRequest.FuelRequestPricingDetail.PricingCodeId, fuelRequest.Currency, fuelRequest.CityGroupTerminalId);
                    if (response != null)
                    {
                        fuelRequest.CreationTimeRackPPG = response.TerminalPrice;
                    }
                }

                return fuelRequest;
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLowerInvariant().Contains(Resource.errMessageFailedSaveFuelPricing.ToLower()))
                {
                    throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
                }
                
                LogManager.Logger.WriteException("OfferDomain", "CreateFuelRequestFromOffer", ex.Message, ex);
                return null;
            }
        }

        public List<DropdownDisplayItem> GetOfferApplicableFuelTypes(int fuelTypeId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                //TODO : need to handle case if no record in OfferFuelTypes i.e. all fuel types for offer
                // issue : if all fuel types for offer then only FT's supported by supplier should be taken
                // but supplier supports different fuel types for diffferent states
                // how to handle this case ?
                //response = (from offerPricingFuelTypes in Context.DataContext.OfferFuelTypes
                //            join prods in Context.DataContext.MstProducts on offerPricingFuelTypes.FuelTypeId equals prods.Id
                //            where offerPricingFuelTypes.OfferPricingId == offerPricingId
                //            select new DropdownDisplayItem()
                //            {
                //                Id = prods.Id,
                //                Name = prods.Name
                //            }
                //            ).OrderByDescending(t => t.Id).ToList();

                response = (from products in Context.DataContext.MstTfxProducts
                            where products.Id == fuelTypeId
                            select new DropdownDisplayItem()
                            {
                                Id = products.Id,
                                Name = products.Name
                            }
                            ).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetOfferApplicableFuelTypes", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> AcceptOfferPricing(OfferOrderViewModel viewModel, UserContext userContext)
        {
            StatusViewModel response = new StatusViewModel();
            Job job;
            var offerPricing = Context.DataContext.OfferPricings.SingleOrDefault(t => t.Id == viewModel.OfferPricingId && t.IsActive);
            if (offerPricing == null)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageOfferNoLongerValid;
                return response;
            }
            var supplierId = offerPricing.CreatedBy;
            var offerSupplier = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == supplierId);
            var buyerUser = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userContext.Id);
            ThirdPartyOrderDomain tpoDomain = new ThirdPartyOrderDomain(this);
            HelperDomain helperDomain = new HelperDomain(this);
            ThirdPartyOrderViewModel tpoViewModel = new ThirdPartyOrderViewModel();
            var buyerOfferStatus = new OfferBuyerStatus()
            {
                OfferPricingId = offerPricing.Id,
                CreatedBy = userContext.Id,
                CreatedDate = DateTimeOffset.Now,
                StatusId = (int)OfferBuyerStatuses.Accepted,
                BuyerCompanyId = userContext.CompanyId
            };

            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    if (viewModel.AddressDetails.JobId.HasValue)
                    {
                        job = await Context.DataContext.Jobs.SingleOrDefaultAsync(t => t.Id == viewModel.AddressDetails.JobId.Value);

                        if (job.IsRetailJob)
                        {
                            response = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().ValidateFuelType(Convert.ToInt32(viewModel.AddressDetails.JobId), Convert.ToInt32(viewModel.FuelDetails.FuelTypeId), true, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId,true);
                            if (response.StatusCode == Status.Warning)
                            {
                                response.StatusCode = Status.Failed;
                                return response;
                            }

                        }
                    }
                    else
                    {
                        var isJobLocationValidForOffer = IsJobLocationValidForOffer(offerPricing, viewModel.AddressDetails.State.Id, viewModel.AddressDetails.City, viewModel.AddressDetails.ZipCode);
                        if (!isJobLocationValidForOffer)
                        {
                            // job location is not valid for the offer
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageJobLocationNotValidForOffer;
                            return response;
                        }
                        viewModel.UpdatedBy = userContext.Id;
                        job = CreateJobFromOfferAsync(viewModel);
                        if (job == null)
                        {
                            // job could not be created
                            transaction.Rollback();
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageAcceptOfferFailed;
                            return response;
                        }
                    }

                    if (job.Id == 0)
                    {
                        job.CreatedBy = buyerUser.Id;
                        job.CreatedDate = DateTimeOffset.Now;
                        job.CreatedByCompanyId = userContext.CompanyId;
                        buyerUser.Company.Jobs.Add(job);
                        await Context.CommitAsync();
                    }
                    viewModel.AddressDetails.JobId = job.Id;

                    var fuelRequest = await CreateFuelRequestFromOffer(viewModel, offerPricing, userContext, job);
                    if (fuelRequest == null)
                    {
                        // FR not mapped correctly
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAcceptOfferFailed;
                        return response;
                    }
                    fuelRequest.FuelRequestDetail.IsDropImageRequired= helperDomain.GetDropImageRequired(offerSupplier.CompanyId.Value);
                    job.FuelRequests.Add(fuelRequest);
                    await Context.CommitAsync();

                    if (viewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries && viewModel.FuelDeliveryDetails.DeliverySchedules != null)
                    {
                        // add DS
                        viewModel.FuelDeliveryDetails.DeliverySchedules.ForEach(t => t.UoM = fuelRequest.UoM);
                        tpoDomain.AddDeliveryscheduleToFuelRequest(fuelRequest, viewModel.FuelDeliveryDetails.DeliverySchedules, buyerUser.Id, buyerUser.CompanyId.Value);
                    }

                    if (viewModel.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                    {
                        offerPricing.FuelFees.Where(t => !t.FeeConstraintTypeId.HasValue).ToList().ForEach(t => fuelRequest.FuelRequestFees.Add(t));
                    }
                    else
                    {
                        offerPricing.FuelFees.ToList().ForEach(t => fuelRequest.FuelRequestFees.Add(t));
                    }

                    tpoViewModel.IsBuyAndSellOrder = false;
                    tpoViewModel.DefaultInvoiceType = (int)InvoiceType.Manual;
                    tpoViewModel.AddressDetails.IsProFormaPoEnabled = job.IsProFormaPoEnabled;
                    tpoViewModel.AddressDetails.SignatureEnabled = job.SignatureEnabled;
                    tpoViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = viewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes;

                    Context.DataContext.OfferBuyerStatuses.Add(buyerOfferStatus);
                    await Context.CommitAsync();
                    var order = await tpoDomain.AcceptFuelRequestFromTPO(userContext, fuelRequest, tpoViewModel, offerSupplier);

                    if (order != null)
                    {
                        // commit only if the complete flow is complete
                        transaction.Commit();
                        NewsfeedDomain newsfeedDomain = new NewsfeedDomain(tpoDomain);
                        await newsfeedDomain.SetOrderCreateOnAcceptOfferNewsfeed(userContext, fuelRequest, order, offerPricing);
                        //settingsDomain.SetBuyerSupplierInformation(offerPricing.SupplierCompanyId, userContext.CompanyId,string.Empty,false,OrderCreationMethod.FromOffer,userContext);
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageAcceptOfferSuccess;
                        if (viewModel.AddressDetails.IsNewJob)
                        {
                            response.StatusMessage = string.Format(Resource.errMessageAcceptOfferJobCreateSuccess, job.Name);
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageAcceptOfferFailed;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLowerInvariant().Contains(Resource.errMessageFailedSaveFuelPricing.ToLower()))
                    {
                        response.StatusMessage = Resource.errMessageFailedSaveFuelPricing;
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageAcceptOfferFailed;
                    }
                    response.StatusCode = Status.Failed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OfferDomain", "AcceptOfferPricing", ex.Message, ex);
                }
            }

            return response;
        }

        private Job CreateJobFromOfferAsync(OfferOrderViewModel viewModel)
        {
            var helperDomain = new HelperDomain(this);
            if (viewModel.AddressDetails.Latitude == 0 || viewModel.AddressDetails.Longitude == 0)
            {
                var stateCode = Context.DataContext.MstStates.First(t => t.Id == viewModel.AddressDetails.State.Id).Code;
                var countryCode = Context.DataContext.MstCountries.First(t => t.Id == viewModel.AddressDetails.Country.Id).Code;
                var point = GoogleApiDomain.GetGeocode($"{viewModel.AddressDetails.Address} {viewModel.AddressDetails.City} {stateCode} {countryCode} {viewModel.AddressDetails.ZipCode}");
                if (point != null)
                {
                    viewModel.AddressDetails.Latitude = Convert.ToDecimal(point.Latitude);
                    viewModel.AddressDetails.Longitude = Convert.ToDecimal(point.Longitude);
                    viewModel.AddressDetails.CountyName = point.CountyName;
                    string timeZoneName = GoogleApiDomain.GetTimeZone(viewModel.AddressDetails.Latitude, viewModel.AddressDetails.Longitude);
                    if (!string.IsNullOrEmpty(timeZoneName))
                    {
                        viewModel.AddressDetails.TimeZoneName = timeZoneName;
                    }
                }
                else
                {
                    return null;
                }
            }

            if (string.IsNullOrEmpty(viewModel.AddressDetails.CountyName))
            {
                var geoAddress = GoogleApiDomain.GetAddress(Convert.ToDouble(viewModel.AddressDetails.Latitude), Convert.ToDouble(viewModel.AddressDetails.Longitude));
                if (geoAddress != null)
                {
                    viewModel.AddressDetails.CountyName = geoAddress.CountyName;
                }
            }
            var job = viewModel.ToEntityFromOffer();

            job.Name = GenerateRandomJobName("auto-", 5);
            job.JobBudget = viewModel.ToBudgetEntityFromOffer();
            job.JobBudget.IsAssetTracked = true;

            var currencyRateDomain = new CurrencyRateDomain(this);
            job.JobBudget.ExchangeRate = currencyRateDomain.GetCurrencyRate(Currency.USD, job.Currency, DateTimeOffset.Now);
            job.TerminalId = helperDomain.GetClosestTerminalId(job.Latitude, job.Longitude, job.StateId);

            return job;
        }

        public async Task<StatusViewModel> SaveOffer(OfferViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (await ValidateOffer(viewModel, response))
                {
                    //save offer details
                    //var offer = viewModel.ToEntity();
                    //if (offer != null)
                    //{
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            List<OfferPricing> offerPricings = new List<OfferPricing>();
                            var feeDomain = new FuelFeesDomain(this);

                            foreach (var fuelType in viewModel.FuelTypes)
                            {
                                await feeDomain.SaveOfferFuelFees(viewModel, userContext);

                                var offerPricingObj = viewModel.ToOfferPricingEntity(fuelType);
                                offerPricings.Add(offerPricingObj);

                                Context.DataContext.OfferPricings.Add(offerPricingObj);
                                Context.DataContext.SaveChanges();

                                if (offerPricingObj.Name.Equals(ApplicationConstants.OfferNumberPrefix))
                                {
                                    offerPricingObj.Name = ApplicationConstants.OfferNumberPrefix + offerPricingObj.Id.ToString().PadLeft(7, '0');
                                }

                                //var queueRequest = GetEnqueueMessageRequestViewModel(userContext, offerPricingObj, fuelType);
                                //var queueId = queueDomain.EnqeueMessage(queueRequest);
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Resource.successMessageOfferSavedSuccessfully;
                        }
                        catch (Exception ex)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageOfferCreationFailed;
                            transaction.Rollback();
                            LogManager.Logger.WriteException("OfferDomain", "SaveOffer", ex.Message, ex);
                        }
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "SaveOffer", ex.Message, ex);
            }
            return response;
        }

        //private QueueMessageViewModel GetEnqueueMessageRequestViewModel(UserContext userContext, OfferPricing offerPricingObj, int fuelTypeId)
        //{
        //    var jsonViewModel = new NotificationOfferQueMsg();
        //    jsonViewModel.OfferId = offerPricingObj.Id;
        //    jsonViewModel.OfferNumber = offerPricingObj.Name;
        //    jsonViewModel.CreatedByCompanyId = offerPricingObj.SupplierCompanyId;
        //    jsonViewModel.CreatedByCompanyName = userContext.CompanyName;
        //    jsonViewModel.CreatedByUserName = userContext.Name;
        //    jsonViewModel.FuelTypeId = fuelTypeId;
        //    jsonViewModel.OfferTypeId = offerPricingObj.OfferTypeId;

        //    string json = JsonConvert.SerializeObject(jsonViewModel);

        //    return new QueueMessageViewModel()
        //    {
        //        CreatedBy = userContext.Id,
        //        QueueProcessType = QueueProcessType.OfferCreated,
        //        JsonMessage = json
        //    };
        //}
        public async Task<StatusViewModel> UpdateOffer(OfferViewModel viewModel)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                if (await ValidateOffer(viewModel, response))
                {
                    var existingOfferPricing = Context.DataContext.OfferPricings.SingleOrDefault(y => y.Id == viewModel.Id);
                    if (existingOfferPricing != null)
                    {
                        using (var transaction = Context.DataContext.Database.BeginTransaction())
                        {
                            try
                            {
                                SetExistingOfferPricingInactive(viewModel.UserId, existingOfferPricing);
                                existingOfferPricing.Name = viewModel.Name;

                                var fuelType = viewModel.FuelTypes.First();
                                var offerPricingObj = viewModel.ToOfferPricingEntity(fuelType);
                                Context.DataContext.OfferPricings.Add(offerPricingObj);
                                Context.DataContext.SaveChanges();

                                if (offerPricingObj.Name.Equals(ApplicationConstants.OfferNumberPrefix))
                                {
                                    offerPricingObj.Name = StringFormatter.Format(ApplicationConstants.OfferNumberPrefix, offerPricingObj.Id);
                                }

                                await Context.CommitAsync();
                                transaction.Commit();

                                response.StatusCode = Status.Success;

                                response.StatusMessage = Resource.successMessageOfferEditedSuccessfully;
                            }
                            catch (Exception ex)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageOfferEditingFailed;
                                transaction.Rollback();
                                LogManager.Logger.WriteException("OfferDomain", "UpdateOffer", ex.Message, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "UpdateOffer", ex.Message, ex);
            }
            return response;
        }

        private static void SetExistingOfferPricingInactive(int updatedByUserId, OfferPricing existingPricing)
        {
            existingPricing.IsActive = false;
            existingPricing.IsOfferUpdated = true;
            existingPricing.OfferPricingItems.ToList().ForEach(x =>
            {
                x.IsActive = false;
                x.UpdatedDate = DateTimeOffset.Now;
                x.UpdatedBy = updatedByUserId;
            });
        }

        public async Task<List<UspBuyerOfferGridViewModel>> GetBuyerOfferGridAsync(int companyId, DataTableSearchModel dataTableSearchModel, OfferFilterViewModel filter)
        {
            var response = new List<UspBuyerOfferGridViewModel>();
            try
            {
                var selectedStates = string.Join(",", filter.States);
                var selectedCities = filter.Cities;
                var selectedZipCodes = filter.ZipCodes;

                decimal latitude = 0, longitude = 0;
                if (filter.IsJobSearch)
                {
                    var job = await Context.DataContext.Jobs.Where(t => t.Id == filter.JobId)
                                                            .Select(t => new { t.Latitude, t.Longitude })
                                                            .SingleOrDefaultAsync();
                    latitude = Convert.ToDecimal(job.Latitude);
                    longitude = Convert.ToDecimal(job.Longitude);
                }
                else
                {
                    var geoCodes = GoogleApiDomain.GetGeocode(filter.ZipCodes);
                    if (geoCodes != null)
                    {
                        latitude = Convert.ToDecimal(geoCodes.Latitude);
                        longitude = Convert.ToDecimal(geoCodes.Longitude);
                    }
                }

                if (filter.TruckLoadType == TruckLoadTypes.FullTruckLoad && filter.PricingSource != PricingSource.Axxis)
                {
                    UspBuyerFtlOfferGridRequestViewModel filteredOffers = GetBuyerFtlOfferGridReqViewModel(companyId, dataTableSearchModel, filter, selectedStates, selectedCities, selectedZipCodes, latitude, longitude);
                    var priceData = await new PricingServiceDomain(this).GetLatestTerminalPriceAsync(filteredOffers.CityGroupTerminalId, filteredOffers.PricingSource, filteredOffers.FuelTypes, filter.PricingCodeId);
                    filteredOffers.LowPrice = priceData.LowPrice;
                    filteredOffers.AvgPrice = priceData.AvgPrice;
                    filteredOffers.HighPrice = priceData.HighPrice;
                    filteredOffers.PricingTypeId = priceData.PricingTypeId;

                    response = await new StoredProcedureDomain(this).GetBuyerFtlOfferGrid(filteredOffers);
                    SetEstimatedPriceToOffers(filter, response, selectedStates, selectedCities, selectedZipCodes, filteredOffers.FuelTypes);
                }
                else
                {
                    UspBuyerOfferGridRequestViewModel filteredOffers = GetBuyerOfferGridReqViewModel(companyId, dataTableSearchModel, filter, selectedStates, selectedCities, selectedZipCodes, latitude, longitude);
                    var priceData = await new PricingServiceDomain(this).GetLatestTerminalPriceAsync(0, (int)PricingSource.Axxis, filteredOffers.FuelTypes, filteredOffers.PricingCodeId, Convert.ToDecimal(filteredOffers.Latitude), Convert.ToDecimal(filteredOffers.Longitude));
                    filteredOffers.LowPrice = priceData.LowPrice;
                    filteredOffers.AvgPrice = priceData.AvgPrice;
                    filteredOffers.HighPrice = priceData.HighPrice;

                    response = await new StoredProcedureDomain(this).GetBuyerOfferGrid(filteredOffers);
                    SetEstimatedPriceToOffers(filter, response, selectedStates, selectedCities, selectedZipCodes, filteredOffers.FuelTypes);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetBuyerOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<UspBuyerOfferGridViewModel>> GetWebBuyerOfferGridAsync(int companyId, DataTableSearchModel dataTableSearchModel, OfferFilterViewModel filter)
        {
            var response = new List<UspBuyerOfferGridViewModel>();
            try
            {
                var selectedStates = string.Join(",", filter.States);
                var selectedCities = filter.Cities;
                var selectedZipCodes = filter.ZipCodes;

                decimal latitude = 0, longitude = 0;
                if (filter.IsJobSearch)
                {
                    var job = await Context.DataContext.Jobs.Where(t => t.Id == filter.JobId)
                                                            .Select(t => new { t.Latitude, t.Longitude })
                                                            .SingleOrDefaultAsync();
                    latitude = Convert.ToDecimal(job.Latitude);
                    longitude = Convert.ToDecimal(job.Longitude);
                }
                else
                {
                    var geoCodes = GoogleApiDomain.GetGeocode(filter.ZipCodes);
                    if (geoCodes != null)
                    {
                        latitude = Convert.ToDecimal(geoCodes.Latitude);
                        longitude = Convert.ToDecimal(geoCodes.Longitude);
                    }
                }

                if (!(filter.PricingTypeId == (int)PricingType.PricePerGallon || filter.PricingTypeId == (int)PricingType.Suppliercost) &&  filter.PricingSource != PricingSource.Axxis)
                {
                    if (filter.CityGroupTerminalId == null)
                    {
                        return response;
                    }

                    UspBuyerFtlOfferGridRequestViewModel filteredOffers = GetBuyerFtlOfferGridReqViewModel(companyId, dataTableSearchModel, filter, selectedStates, selectedCities, selectedZipCodes, latitude, longitude);
                    var productid = new FuelRequestDomain(this).GetFuelTypeId(Convert.ToInt32(filteredOffers.FuelTypes), (int)filter.PricingSource).ToString();
                    var priceData = await new PricingServiceDomain(this).GetLatestTerminalPriceAsync(filteredOffers.CityGroupTerminalId, filteredOffers.PricingSource, productid, filter.PricingCodeId);
                    filteredOffers.LowPrice = priceData.LowPrice;
                    filteredOffers.AvgPrice = priceData.AvgPrice;
                    filteredOffers.HighPrice = priceData.HighPrice;
                    
                    response = await new StoredProcedureDomain(this).GetBuyerFtlOfferGrid(filteredOffers);
                    SetEstimatedPriceToWebOffers(filter, response, selectedStates, selectedCities, selectedZipCodes, filteredOffers.FuelTypes);
                }
                else
                {
                    UspBuyerOfferGridRequestViewModel filteredOffers = GetBuyerOfferGridReqViewModel(companyId, dataTableSearchModel, filter, selectedStates, selectedCities, selectedZipCodes, latitude, longitude);
                    
                    if(!(filteredOffers.PricingTypeId == (int)PricingType.PricePerGallon || filteredOffers.PricingTypeId == (int)PricingType.Suppliercost))
                    {
                        var productid = new FuelRequestDomain(this).GetFuelTypeId(Convert.ToInt32(filteredOffers.FuelTypes), (int)filter.PricingSource).ToString();
                        var priceData = await new PricingServiceDomain(this).GetLatestTerminalPriceAsync(0, (int)PricingSource.Axxis, productid, filteredOffers.PricingCodeId, Convert.ToDecimal(filteredOffers.Latitude), Convert.ToDecimal(filteredOffers.Longitude));
                        filteredOffers.LowPrice = priceData.LowPrice;
                        filteredOffers.AvgPrice = priceData.AvgPrice;
                        filteredOffers.HighPrice = priceData.HighPrice;
                    }

                    response = await new StoredProcedureDomain(this).GetBuyerOfferGrid(filteredOffers);
                    SetEstimatedPriceToWebOffers(filter, response, selectedStates, selectedCities, selectedZipCodes, filteredOffers.FuelTypes);
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetWebBuyerOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        private void SetEstimatedPriceToOffers(OfferFilterViewModel filter, List<UspBuyerOfferGridViewModel> response, string selectedStates, string selectedCities, string selectedZipCodes, string fuelTypes)
        {
            Dictionary<int, decimal> offerFees = new Dictionary<int, decimal>();

            foreach (var item in response)
            {
                string queryUrl = string.Join("&", item.OfferPricingId, filter.JobId, fuelTypes, filter.Quantity, selectedStates, selectedCities, selectedZipCodes);
                item.EncrptedUrl = Convert.ToBase64String(Encoding.ASCII.GetBytes(queryUrl));
                if (offerFees.ContainsKey(item.OfferPricingId))
                {
                    var fee = offerFees[item.OfferPricingId];
                    item.LoadedPrice += fee;
                }
                else
                {
                    var allFees = GetEstimatedOfferPricingFees(item.OfferPricingId, 1);
                    var totalFees = allFees.Sum(t => t.Fee);
                    item.LoadedPrice += totalFees;
                    offerFees.Add(item.OfferPricingId, totalFees);
                }
            }
        }

        private void SetEstimatedPriceToWebOffers(OfferFilterViewModel filter, List<UspBuyerOfferGridViewModel> response, string selectedStates, string selectedCities, string selectedZipCodes, string fuelTypes)
        {
            Dictionary<int, decimal> offerFees = new Dictionary<int, decimal>();

            foreach (var item in response)
            {
                string queryUrl = string.Join("&", item.OfferPricingId, filter.JobId, fuelTypes, filter.Quantity, selectedStates,
                    selectedCities, selectedZipCodes, filter.CityGroupTerminalId, filter.PricingCodeId, filter.PricingCode, filter.PricingCodeDesc);
                item.EncrptedUrl = Convert.ToBase64String(Encoding.ASCII.GetBytes(queryUrl));
                if (offerFees.ContainsKey(item.OfferPricingId))
                {
                    var fee = offerFees[item.OfferPricingId];
                    item.LoadedPrice += fee;
                }
                else
                {
                    var allFees = GetEstimatedOfferPricingFees(item.OfferPricingId, 1);
                    var totalFees = allFees.Sum(t => t.Fee);
                    item.LoadedPrice += totalFees;
                    offerFees.Add(item.OfferPricingId, totalFees);
                }
            }
        }

        private static UspBuyerOfferGridRequestViewModel GetBuyerOfferGridReqViewModel(int companyId, DataTableSearchModel dataTableSearchModel, OfferFilterViewModel filter, string selectedStates, string selectedCities, string selectedZipCodes, decimal latitude, decimal longitude)
        {
            return new UspBuyerOfferGridRequestViewModel()
            {
                JobId = filter.IsJobSearch ? filter.JobId : 0,
                CompanyId = companyId,
                OfferType = (int)filter.OfferType,
                States = selectedStates,
                Cities = selectedCities,
                ZipCodes = selectedZipCodes,
                FuelTypes = string.Join(",", filter.FuelTypes),
                CountryId = filter.CountryId,
                CurrencyType = filter.CurrencyType,
                Quantity = filter.Quantity,
                Latitude = (float)latitude,
                Longitude = (float)longitude,
                PricingTypeId = filter.PricingTypeId,
                PricingCodeId = filter.PricingCodeId,
                dataTableSearchValues = dataTableSearchModel
            };
        }

        private static UspBuyerFtlOfferGridRequestViewModel GetBuyerFtlOfferGridReqViewModel(int companyId, DataTableSearchModel dataTableSearchModel, OfferFilterViewModel filter, string selectedStates, string selectedCities, string selectedZipCodes, decimal latitude, decimal longitude)
        {
            return new UspBuyerFtlOfferGridRequestViewModel()
            {
                JobId = filter.IsJobSearch ? filter.JobId : 0,
                CompanyId = companyId,
                OfferType = (int)filter.OfferType,
                States = selectedStates,
                Cities = selectedCities,
                ZipCodes = selectedZipCodes,
                FuelTypes = string.Join(",", filter.FuelTypes),
                CountryId = filter.CountryId,
                CurrencyType = filter.CurrencyType,
                Quantity = filter.Quantity,
                Latitude = (float)latitude,
                Longitude = (float)longitude,
                PricingTypeId = filter.PricingTypeId,
                TruckLoadType = (int)filter.TruckLoadType,
                PricingSource = (int)filter.PricingSource,
                CityGroupTerminalId = filter.CityGroupTerminalId ?? 0,
                TerminalId = filter.TerminalId ?? 0,
                dataTableSearchValues = dataTableSearchModel
            };
        }

        //public async Task<List<UspSupplierOfferGridViewModel>> GetSupplierOfferGridAsync(int companyId, int id, DataTableSearchModel dataTableSearchModel, OfferFilterViewModel filter, int countryId, int currency)
        //{
        //    var response = new List<UspSupplierOfferGridViewModel>();
        //    try
        //    {
        //        var filteredOffers = new UspSupplierOfferGridRequestViewModel()
        //        {
        //            OfferType = (int)filter.OfferType,
        //            CompanyId = companyId,
        //            Customers = string.Join(",", filter.Customers),
        //            dataTableSearchValues = dataTableSearchModel
        //        };
        //        response = await new StoredProcedureDomain(this).GetSupplierOfferGrid(filteredOffers);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Logger.WriteException("OfferDomain", "GetSupplierOfferGridAsync", ex.Message, ex);
        //    }
        //    return response;
        //}

        public async Task<List<PricingTableGridViewModel>> GetSupplierPricingTableAsync(int companyId, DataTableSearchModel dataTableSearchModel, OfferFilterViewModel filter, int countryId, int currency)
        {
            var response = new List<PricingTableGridViewModel>();
            try
            {
                var filteredOffers = new UspSupplierOfferGridRequestViewModel()
                {
                    OfferType = (int)filter.OfferType,
                    CompanyId = companyId,
                    Customers = string.Empty,
                    CountryId = countryId,
                    dataTableSearchValues = dataTableSearchModel
                };
                response = await new StoredProcedureDomain(this).GetSupplierPricingTableGrid(filteredOffers);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetSupplierPricingTableAsync", ex.Message, ex);
            }
            return response;
        }

        public bool IsOfferNameExists(int companyId, string offerName, int offerPricingId, Guid guid)
        {
            return Context.DataContext.OfferPricings.Any(t => t.SupplierCompanyId == companyId
                                        && t.Id != offerPricingId && t.IsActive
                                        && t.Name.ToLower().Equals(offerName.ToLower())
                                        && t.OfferChainId != guid);
        }

        public async Task<bool> ValidateOffer(OfferViewModel viewModel, StatusViewModel statusViewModel)
        {
            bool keepRunning = true;

            if (IsOfferNameExists(viewModel.CompanyId, viewModel.Name, viewModel.Id, viewModel.Guid))
            {
                statusViewModel.StatusCode = Status.Failed;
                statusViewModel.StatusMessage = Resource.errMessageOfferNameAlreadyExists;
                return false;
            }

            if (viewModel.OfferTypeId == (int)OfferType.Exclusive && (viewModel.Tiers.Count == 0 && viewModel.Customers.Count == 0))
            {
                statusViewModel.StatusCode = Status.Failed;
                statusViewModel.StatusMessage = Resource.errMessageEitherTierOrCustomersRequired;
                keepRunning = false;
            }

            if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                keepRunning = await IsValidFuelCostPricingFormat(viewModel, statusViewModel, keepRunning);
            }

            if (viewModel.OfferLocationTypeId == (int)OfferLocationType.City)
            {
                if (viewModel.LocationViewModel == null || viewModel.LocationViewModel.Any(t => t.CityId == 0) || !viewModel.LocationViewModel.Any())
                {
                    statusViewModel.StatusCode = Status.Failed;
                    statusViewModel.StatusMessage = Resource.errMessageCityIsRequiredForCitySpecificOffers;
                    keepRunning = false;
                }
            }

            var pricingItems = OfferMapper.ToPricingItemViewModel(viewModel);
            if (IsOfferPricingItemExists(pricingItems, viewModel))
            {
                statusViewModel.StatusCode = Status.Failed;
                statusViewModel.StatusMessage = Resource.errMessageOfferPricingItemAlreadyExists;
                keepRunning = false;
            }

            return keepRunning;
        }

        private bool IsOfferPricingItemExists(List<OfferPricingItem> pricingItems, OfferViewModel viewModel)
        {
            var response = false;
            var existingItems = Context.DataContext.OfferPricingDetails.Where(t => t.OfferPricingId != viewModel.Id && t.IsActive && t.OfferPricing.SupplierCompanyId == viewModel.CompanyId && t.OfferPricing.TruckLoadType == viewModel.FuelPricing.FuelPricingDetails.TruckLoadTypes && t.OfferPricing.IsActive)
                                .Select(t => new { t.TierId, t.CustomerId, t.StateId, t.CityId, t.ZipCode, t.OfferPricingId, t.OfferPricing.FuelTypeId })
                                .ToList();

            foreach (var item in pricingItems)
            {
                if (existingItems.Any(t => viewModel.FuelTypes.Contains(t.FuelTypeId)
                                            && t.TierId == item.TierId
                                            && t.CustomerId == item.CustomerId
                                            && t.StateId == item.StateId
                                            && t.CityId == item.CityId
                                            && t.ZipCode == item.ZipCode
                                             ))
                {
                    response = true;
                    break;
                }
            }
            return response;
        }

        public async Task<OfferLoadedPriceViewModel> GetLoadedPrice(int jobId, int offerPricingId, int productId, int quantity, int pricingType, int rackType, decimal rackPrice, bool includeTaxes, int marketBasedType, string zipcode, decimal supplierCost, int pricingSourceId = 0, int pricingCodeId = 0)
        {
            OfferLoadedPriceViewModel response = new OfferLoadedPriceViewModel();
            try
            {
                response = await new ExternalPricingDomain(this).CalculateLoadedPrice(jobId, productId, pricingType, rackType, rackPrice, includeTaxes, marketBasedType, 0, supplierCost, quantity, zipcode, pricingSourceId, pricingCodeId);
                response.EstimatedFees = GetEstimatedOfferPricingFees(offerPricingId, quantity);
                foreach (var item in response.EstimatedFees)
                {
                    response.TotalFeesAmount += item.Fee;
                }
                response.TotalAmount = response.TotalDropAmount + response.TotalTaxAmount + response.TotalFeesAmount;
                response.StatusCode = Status.Success;
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                LogManager.Logger.WriteException("OfferDomain", "GetLoadedPrice", ex.Message, ex);
            }
            return response;
        }

        private List<string> DecryptParameters(string request)
        {
            var response = new List<string>();
            try
            {
                byte[] bytes;
                string decryptedRq;
                bytes = Convert.FromBase64String(request);
                decryptedRq = Encoding.ASCII.GetString(bytes);
                response = decryptedRq.Split('&').ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<StatusViewModel> AcceptOfferFromMobile(AcceptOfferMobileViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            var helperDomain = new HelperDomain(this);
            var offerPricingModel = await GetOfferPricingDetailsAsync(viewModel.EncryptedRequest, userContext);
            if (viewModel.JobId != 0)
            {
                //select existing Job
                offerPricingModel.OfferOrderViewModel.AddressDetails.JobId = viewModel.JobId;
            }
            else if (viewModel.JobId == 0 && offerPricingModel.JobId != 0)
            {
                offerPricingModel.OfferOrderViewModel.AddressDetails.JobId = offerPricingModel.JobId;
            }
            else if (viewModel.JobId == 0 && offerPricingModel.JobId == 0)
            {
                //Search by location
                offerPricingModel.OfferOrderViewModel.AddressDetails = ContextFactory.Current.GetDomain<ThirdPartyOrderDomain>().GetTPOAddressDetails(offerPricingModel.StateId, viewModel.Address, offerPricingModel.City, offerPricingModel.ZipCode);
                bool isValidAddress = helperDomain.IsValidAddress(offerPricingModel.OfferOrderViewModel.AddressDetails.Address);
                if (!isValidAddress)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageInvalidAddress;
                    return response;
                }
            }

            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.DeliveryTypeId = viewModel.DeliveryTypeId;
            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.StartDate = viewModel.StartDate;
            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.EndDate = viewModel.EndDate;
            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.StartTime = viewModel.StartTime;
            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.EndTime = viewModel.EndTime;
            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.DeliverySchedules = viewModel.DeliverySchedules;
            offerPricingModel.OfferOrderViewModel.FuelDeliveryDetails.ExpirationDate = viewModel.ExpirationDate;
            offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = viewModel.TruckLoadTypes;
            offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId = (int)viewModel.PricingSourceId;
            offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.CityGroupTerminalId = viewModel.CityGroupTerminalId;

            // get pricing code from pricing service
            var pricingModel = new PricingCodesRequestViewModel() { PricingSourceId = (int)viewModel.PricingSourceId, PricingTypeId = viewModel.PricingTypeId, FeedTypeId = (int)viewModel.FeedTypeId, FuelClassTypeId = (int)viewModel.FuelClassTypes, QuantityIndicatorId = (int)viewModel.QuantityIndicatorTypes };
            var pricing = await ContextFactory.Current.GetDomain<PricingServiceDomain>().GetPricingCodesAsync(pricingModel);
            if (pricing != null && pricing.PricingCodes != null && pricing.PricingCodes.Any())
            {
                var pricingCode = pricing.PricingCodes.OrderByDescending(t => t.Id).FirstOrDefault();
                offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Id = pricingCode.Id;
            }

            //offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.FeedTypeId = (int)viewModel.FeedTypeId;
            //offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.FuelClassTypes = viewModel.FuelClassTypes;
            //offerPricingModel.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceQuantityIndicatorTypes = viewModel.QuantityIndicatorTypes;
            response = await AcceptOfferPricing(offerPricingModel.OfferOrderViewModel, userContext);
            return response;
        }

        public async Task<OfferPricingDetailsViewModel> GetOfferPricingDetailsAsync(string request, UserContext userContext)
        {
            int offerPricingId = 0, jobid = 0, fueltypeid = 0, qty = 0, stateId = 0;
            OfferPricingDetailsViewModel response = new OfferPricingDetailsViewModel();
            try
            {
                string zipcode = string.Empty, city = string.Empty;
                List<string> decryptedParams = DecryptParameters(request);
                if (decryptedParams.Count == 7)
                {
                    offerPricingId = Convert.ToInt32(decryptedParams[0]);
                    jobid = Convert.ToInt32(decryptedParams[1]);
                    fueltypeid = Convert.ToInt32(decryptedParams[2]);
                    qty = Convert.ToInt32(decryptedParams[3]);
                    stateId = string.IsNullOrWhiteSpace(decryptedParams[4].Trim()) ? 0 : Convert.ToInt32(decryptedParams[4]);
                    city = decryptedParams[5].Trim();
                    zipcode = decryptedParams[6].Trim();
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errLoadOfferDetailsFailed;
                    return response;
                }
                var offerPricing = await Context.DataContext.OfferPricings.SingleOrDefaultAsync(t => t.Id == offerPricingId);
                if (offerPricing != null)
                {
                    response = offerPricing.ToViewModel();
                    response.OfferOrderViewModel = GetOfferAcceptViewModel(jobid, fueltypeid, offerPricingId, qty, (int)response.OfferViewModel.FuelPricing.FuelPricingDetails.TruckLoadTypes, response.OfferViewModel.FuelPricing.FuelPricingDetails.PricingSourceId);
                    response.OfferOrderViewModel.FuelDetails.FuelQuantity.UoM = offerPricing.UoM;
                    response.OfferOrderViewModel.AddressDetails.Country.Id = offerPricing.CountryId;
                    response.OfferOrderViewModel.AddressDetails.State.Id = stateId;
                    response.OfferOrderViewModel.FuelDetails.FuelTypeId = fueltypeid;
                    response.OfferOrderViewModel.FuelDetails = offerPricing.ToFuelDetailsViewModelFromOffer(response.OfferOrderViewModel.FuelDetails, qty, userContext.CompanyId, fueltypeid, stateId);
                    response.JobId = jobid;
                    response.StateId = stateId;
                    if (!string.IsNullOrWhiteSpace(city))
                    {
                        var cityId = Convert.ToInt32(city);
                        var cityDetails = await Context.DataContext.MstCities.FirstOrDefaultAsync(t => t.Id == cityId);
                        if (cityDetails != null)
                        {
                            response.City = cityDetails.Name;
                        }
                    }
                    response.ZipCode = zipcode;
                    response.Quantity = qty;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errLoadOfferDetailsFailed;
                LogManager.Logger.WriteException("OfferDomain", "GetOfferPricingDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<OfferPricingDetailsViewModel> GetWebOfferPricingDetailsAsync(string request, UserContext userContext)
        {
            int offerPricingId = 0, jobid = 0, fueltypeid = 0, qty = 0, stateId = 0, cityGroupTerminalId = 0, pricingCodeId = 0;
            string pricingCode = string.Empty, pricingCodeDesc = string.Empty;
            OfferPricingDetailsViewModel response = new OfferPricingDetailsViewModel();
            try
            {
                string zipcode = string.Empty, city = string.Empty;
                List<string> decryptedParams = DecryptParameters(request);
                if (decryptedParams.Count == 11)
                {
                    offerPricingId = Convert.ToInt32(decryptedParams[0]);
                    jobid = Convert.ToInt32(decryptedParams[1]);
                    fueltypeid = Convert.ToInt32(decryptedParams[2]);
                    qty = Convert.ToInt32(decryptedParams[3]);
                    stateId = string.IsNullOrWhiteSpace(decryptedParams[4].Trim()) ? 0 : Convert.ToInt32(decryptedParams[4]);
                    city = decryptedParams[5].Trim();
                    zipcode = decryptedParams[6].Trim();
                    int.TryParse(decryptedParams[7], out cityGroupTerminalId);
                    int.TryParse(decryptedParams[8], out pricingCodeId);
                    pricingCode = decryptedParams[9].Trim();
                    pricingCodeDesc = decryptedParams[10].Trim();
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errLoadOfferDetailsFailed;
                    return response;
                }
                var offerPricing = await Context.DataContext.OfferPricings.SingleOrDefaultAsync(t => t.Id == offerPricingId);
                if (offerPricing != null)
                {
                    response = offerPricing.ToViewModel();
                    response.OfferOrderViewModel = GetOfferAcceptViewModel(jobid, fueltypeid, offerPricingId, qty, (int)response.OfferViewModel.FuelPricing.FuelPricingDetails.TruckLoadTypes, response.OfferViewModel.FuelPricing.FuelPricingDetails.PricingSourceId);
                    response.OfferOrderViewModel.FuelDetails.FuelQuantity.UoM = offerPricing.UoM;
                    response.OfferOrderViewModel.AddressDetails.Country.Id = offerPricing.CountryId;
                    response.OfferOrderViewModel.AddressDetails.State.Id = stateId;
                    response.OfferOrderViewModel.FuelDetails.FuelTypeId = fueltypeid;
                    response.OfferOrderViewModel.FuelDetails = offerPricing.ToFuelDetailsViewModelFromOffer(response.OfferOrderViewModel.FuelDetails, qty, userContext.CompanyId, fueltypeid, stateId);

                    response.OfferOrderViewModel.FuelDetails.FuelPricing.CityGroupTerminalId = cityGroupTerminalId;
                    response.JobId = jobid;
                    response.StateId = stateId;
                    if (!string.IsNullOrWhiteSpace(city))
                    {
                        var cityId = Convert.ToInt32(city);
                        var cityDetails = await Context.DataContext.MstCities.FirstOrDefaultAsync(t => t.Id == cityId);
                        if (cityDetails != null)
                        {
                            response.City = cityDetails.Name;
                        }
                    }
                    response.ZipCode = zipcode;
                    response.Quantity = qty;
                    response.PricingCodeId = pricingCodeId;
                    response.PricingCode = pricingCode;
                    response.PricingCodeDesc = pricingCodeDesc;
                    response.OfferOrderViewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode = new PricingCodeDetailViewModel() { Id = pricingCodeId, Code = pricingCode, Description = pricingCodeDesc };
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errLoadOfferDetailsFailed;
                LogManager.Logger.WriteException("OfferDomain", "GetWebOfferPricingDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        public FuelDeliveryDetailsViewModel GetOfferFeesAsync(UserContext userContext, int offerPriceId)
        {
            var response = new FuelDeliveryDetailsViewModel();
            try
            {
                var offerPricing = Context.DataContext.OfferPricings.FirstOrDefault(t => t.Id == offerPriceId);
                if (offerPricing != null)
                {
                    //fees
                    var feesViewModels = offerPricing.FuelFees.ToFeesViewModel();
                    response.FuelFees.FuelRequestFees = feesViewModels;
                    if (feesViewModels.Count > 0)
                    {
                        var firstFee = feesViewModels.First();
                        response.FuelFees.Currency = firstFee.Currency;
                        response.FuelFees.UoM = firstFee.UoM;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetOfferDetails", ex.Message, ex);
            }
            return response;
        }

        private HourlyFeeCalculationViewModel HourlyFeeCalculation(FuelFee feeDetails)
        {
            HourlyFeeCalculationViewModel response = new HourlyFeeCalculationViewModel();

            if (feeDetails != null && feeDetails.Fee > 0)
            {
                if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                {
                    int hours = 0; double mins = 0; double difference = 0;                    
                    difference = 60; // we assume the drop is made for 1 minute , so 60 seconds

                    hours = (int)(difference / 3600);
                    mins = (difference - (hours * 3600)) / 60;
                    response.Fee = feeDetails.Fee * Convert.ToDecimal(difference) / 3600;
                    feeDetails.FeeSubQuantity = (decimal)difference;
                    response.TotalHours = hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                    : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
                }
            }

            return response;
        }

        private static decimal GetDeliveryFee(decimal droppedGallons, FuelFee feeDetails)
        {
            var response = 0.0M;
            if (feeDetails.FeeSubTypeId == (int)FeeSubType.FlatFee)
            {
                response = feeDetails.Fee;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                response = feeDetails.Fee * droppedGallons;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.ByQuantity)
            {
                var byQantity = feeDetails.FeeByQuantities.FirstOrDefault(t => droppedGallons >= t.MinQuantity && droppedGallons <= (t.MaxQuantity ?? droppedGallons));
                if (byQantity != null)
                {
                    response = byQantity.Fee;
                }
            }

            return response;
        }

        private WetHoseOverWaterCalculationViewModel CalucateHourlyOrPerAssetFee(FuelFee feeDetails)
        {
            WetHoseOverWaterCalculationViewModel response = new WetHoseOverWaterCalculationViewModel();

            if (feeDetails != null && feeDetails.Fee > 0)
            {
                if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                {
                    int hours = 0; double mins = 0;
                    double difference = 60; // we assume 60 seconds i.e. 1 minute for fee estimate                    
                    hours = (int)(difference / 3600);
                    mins = (difference - (hours * 3600)) / 60;
                    response.Fee = feeDetails.Fee * Convert.ToDecimal(difference) / 3600;
                    feeDetails.FeeSubQuantity = (decimal)difference;
                    if (feeDetails.FeeTypeId == (int)FeeType.WetHoseFee)
                    {
                        response.WetHoseHours = hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                        : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.OverWaterFee)
                    {
                        response.OverWaterHours = hours > 0 ? string.Format(Resource.constTimeFormat, hours, string.Format(Resource.constFormatDecimal2, mins))
                                                                        : string.Format(Resource.constMinuteFormat, string.Format(Resource.constFormatDecimal2, mins));
                    }
                }
                else if (feeDetails.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
                {
                    var assetCount = 1; // for estimate we assume 1 asset
                    response.Fee = (feeDetails.Fee * (assetCount));
                    feeDetails.FeeSubQuantity = assetCount;
                    if (feeDetails.FeeTypeId == (int)FeeType.WetHoseFee)
                    {
                        response.WetHoseAssetQuantity = assetCount;
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.OverWaterFee)
                    {
                        response.OverWaterAssetQuantity = assetCount;
                    }
                }
            }

            return response;
        }

        private static decimal GetFreightFee(FuelFee feeDetails, decimal droppedGallons)
        {
            decimal response;
            int assetCount = 1;// for estimate we assume 1 asset
            if (feeDetails.FeeSubTypeId == (int)FeeSubType.ByAssetCount)
            {
                response = feeDetails.Fee * assetCount;
            }
            else if (feeDetails.FeeSubTypeId == (int)FeeSubType.PerGallon)
            {
                response = (feeDetails.Fee * droppedGallons);
            }
            else
            {
                response = feeDetails.Fee;
            }
            return response;
        }

        public List<OfferEstimatedFeeViewModel> GetEstimatedOfferPricingFees(int offerPricingId, decimal droppedGallons)
        {
            var pricing = Context.DataContext.OfferPricings.FirstOrDefault(t => t.Id == offerPricingId);
            List<OfferEstimatedFeeViewModel> response = new List<OfferEstimatedFeeViewModel>();
            var duration = "1 min";
            try
            {
                foreach (FuelFee feeDetails in pricing.FuelFees.Where(t => t.FeeConstraintTypeId == null && t.FeeTypeId != (int)FeeType.DryRunFee)) // we consider only non-weekend-special date fees
                {
                    if (feeDetails.FeeTypeId == (int)FeeType.DeliveryFee || feeDetails.FeeTypeId == (int)FeeType.OtherFee)
                    {
                        if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            var hourlyFeeDetail = HourlyFeeCalculation(feeDetails);
                            response.Add(new OfferEstimatedFeeViewModel
                            {
                                Duration = duration,
                                FeeType = feeDetails.FeeTypeId == (int)FeeType.DeliveryFee ? Resource.lblDeliveryFee : string.IsNullOrWhiteSpace(feeDetails.FeeDetails) ? Resource.lblOtherFee : feeDetails.FeeDetails,
                                Fee = hourlyFeeDetail.Fee,
                                FeeSubType = nameof(FeeSubType.HourlyRate)
                            });
                        }
                        else
                        {
                            var fee = GetDeliveryFee(droppedGallons, feeDetails);
                            response.Add(new OfferEstimatedFeeViewModel
                            {
                                FeeType = feeDetails.FeeTypeId == (int)FeeType.DeliveryFee ? Resource.lblDeliveryFee : string.IsNullOrWhiteSpace(feeDetails.FeeDetails) ? Resource.lblOtherFee : feeDetails.FeeDetails,
                                Fee = fee
                            });
                        }
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.WetHoseFee)
                    {
                        response.Add(new OfferEstimatedFeeViewModel
                        {
                            FeeType = Resource.lblWetHoseFee,
                            Fee = CalucateHourlyOrPerAssetFee(feeDetails).Fee
                        });
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.OverWaterFee)
                    {
                        response.Add(new OfferEstimatedFeeViewModel
                        {
                            FeeType = Resource.lblOverWaterFee,
                            Fee = CalucateHourlyOrPerAssetFee(feeDetails).Fee
                        });
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.FreightFee)
                    {
                        response.Add(new OfferEstimatedFeeViewModel
                        {
                            FeeType = Resource.lblFreight.ToString() + " " + Resource.lblLabelFees,
                            Fee = GetFreightFee(feeDetails, droppedGallons)
                        });
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.UnderGallonFee && (feeDetails.MinimumGallons ?? 0) > droppedGallons)
                    {
                        response.Add(new OfferEstimatedFeeViewModel
                        {
                            FeeType = Resource.lblMinimumGallonFee,
                            Fee = feeDetails.TotalFee ?? 0
                        });
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.EnvironmentalFee || feeDetails.FeeTypeId == (int)FeeType.LoadFee || feeDetails.FeeTypeId == (int)FeeType.AdditiveFee || feeDetails.FeeTypeId == (int)FeeType.ProcessingFee)
                    {
                        response.Add(new OfferEstimatedFeeViewModel
                        {
                            FeeType = ((FeeType)feeDetails.FeeTypeId).GetDisplayName(),
                            Fee = HelperDomain.GetFlatPerGallonFee(feeDetails, droppedGallons)
                        });
                    }
                    else if (feeDetails.FeeTypeId == (int)FeeType.ServiceFee || feeDetails.FeeTypeId == (int)FeeType.SurchargeFee)
                    {
                        if (feeDetails.FeeSubTypeId == (int)FeeSubType.HourlyRate)
                        {
                            var hourlyFeeDetail = HourlyFeeCalculation(feeDetails);
                            response.Add(new OfferEstimatedFeeViewModel
                            {
                                FeeType = feeDetails.FeeTypeId == (int)FeeType.ServiceFee ? Resource.lblServiceFee : string.Format("{0} {1}", Resource.lblSurcharge, Resource.lblFee),
                                Fee = hourlyFeeDetail.Fee
                            });
                        }
                        else
                        {
                            response.Add(new OfferEstimatedFeeViewModel
                            {
                                FeeType = feeDetails.FeeTypeId == (int)FeeType.ServiceFee ? Resource.lblServiceFee : string.Format("{0} {1}", Resource.lblSurcharge, Resource.lblFee),
                                Fee = HelperDomain.GetServiceSurchageFee(droppedGallons, feeDetails)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetEstimatedOfferPricingFees", ex.Message, ex);
            }

            return response;
        }

        public FuelPricingViewModel GetOfferPricingAsync(UserContext userContext, int offerPriceId)
        {
            var response = new FuelPricingViewModel();
            try
            {
                var offerPricing = Context.DataContext.OfferPricings.FirstOrDefault(t => t.Id == offerPriceId);
                if (offerPricing != null)
                {
                    response.DifferentFuelPrices = offerPricing.DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetOfferDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<OfferSupplierProfileViewModel> BaseballCardDetails(int buyerCompanyId, int supplierCompanyId)
        {
            var response = new OfferSupplierProfileViewModel();
            try
            {
                response.BaseBallCardDetails = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetBaseballCardDetailsAsync(buyerCompanyId, supplierCompanyId);

                var defaultAddress = Context.DataContext.CompanyAddresses.Select(x => new { x.CompanyId, x.IsDefault, x.IsActive, x.Address, x.City, x.MstState.Code, x.ZipCode, x.PhoneNumber, x.Company.CreatedDate, x.SupplierAddressXWorkingHours })
                                                            .FirstOrDefault(t => t.CompanyId == supplierCompanyId && t.IsDefault && t.IsActive);

                response.Address = defaultAddress.Address + " " + defaultAddress.City + ", " + defaultAddress.Code + " " + defaultAddress.ZipCode;
                response.PhoneNumber = defaultAddress.PhoneNumber;
                response.MemberSince = defaultAddress.CreatedDate.ToString("MMM dd, yyyy");
                if (defaultAddress.SupplierAddressXWorkingHours != null)
                {
                    var activeHour = defaultAddress.SupplierAddressXWorkingHours.FirstOrDefault(t => t.MstWeekDay.Name == DateTimeOffset.Now.DayOfWeek.ToString());
                    response.ActiveHours = activeHour.StartTime.GetTimeInAmPmFormat() + " - " + activeHour.EndTime.GetTimeInAmPmFormat();
                }
                var orders = Context.DataContext.Orders.Select(x => new { x.FuelRequest.SpotDroppedGallons, x.FuelRequest.HedgeDroppedGallons, x.AcceptedCompanyId, x.FuelRequest.IsActive }).Where(t => t.AcceptedCompanyId == supplierCompanyId && t.IsActive).ToList();
                response.TotalOrders = orders.Count();
                response.GallonsDelivered = orders.Sum(x => x.SpotDroppedGallons) + orders.Sum(x => x.HedgeDroppedGallons);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "BaseballCardDetails", ex.Message, ex);
            }
            return response;
        }

        public int GetFeeTypeOrOtherFeeType(string feeTypeId)
        {
            int.TryParse(feeTypeId, out int feeType);
            if (feeType > 0)
            {
                return feeType;
            }
            else
            {
                int.TryParse(feeTypeId.Split('-')[1], out int otherFeeTypeId);
                return otherFeeTypeId;
            }
        }

        public async Task<StatusViewModel> SavePreferenceSetting(OfferQuickUpdateViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var IsUpdate = false;
                        var existingPreferences = Context.DataContext.OfferQuickUpdatePreferences.Where(t => t.CompanyId == userContext.CompanyId);
                        if (existingPreferences != null && existingPreferences.Any())
                        {
                            await existingPreferences.ForEachAsync(t => t.IsActive = false);
                            IsUpdate = true;
                        }

                        var offerQuickUpdatePreference = new OfferQuickUpdatePreference
                        {
                            CompanyId = userContext.CompanyId,
                            IsCustomerTier = viewModel.QuickUpdatePreferenceSetting.IsCustomerTier,
                            IsCustomer = viewModel.QuickUpdatePreferenceSetting.IsCustomer,
                            IsCity = viewModel.QuickUpdatePreferenceSetting.IsCity,
                            IsState = viewModel.QuickUpdatePreferenceSetting.IsState,
                            IsFeeType = viewModel.QuickUpdatePreferenceSetting.QuickUpdateType == (int)OfferQuickUpdateType.Fees,
                            IsMarketOffer = viewModel.QuickUpdatePreferenceSetting.IsMarketOffer,
                            IsPricingType = viewModel.QuickUpdatePreferenceSetting.QuickUpdateType == (int)OfferQuickUpdateType.Pricing,
                            IsActive = true,
                            CreatedBy = userContext.Id,
                            CreatedDate = DateTimeOffset.Now,
                            UpdatedBy = userContext.Id,
                            UpdatedDate = DateTimeOffset.Now,
                        };
                        Context.DataContext.OfferQuickUpdatePreferences.Add(offerQuickUpdatePreference);
                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        if (IsUpdate)
                        {
                            response.StatusMessage = Resource.successMessageOfferPreferenceUpdatedSuccessfully;
                        }
                        else
                        {
                            response.StatusMessage = Resource.successMessageOfferPreferenceSavedSuccessfully;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMessageFailedToSaveUpdateOfferPreferences;
                        transaction.Rollback();
                        LogManager.Logger.WriteException("OfferDomain", "SavePreferenceSetting", ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "SavePreferenceSetting", ex.Message, ex);
            }
            return response;
        }

        #region privateMethods
        private async Task<bool> IsValidFuelCostPricingFormat(OfferViewModel viewModel, StatusViewModel statusViewModel, bool keepRunning)
        {
            //NEED TO DISCUSS WITH RAJEEV - AS AT THIS POINT WE NEED TO CALL OTHER DOMAIN'S METHOD
            var currentCostDomain = new CurrentCostDomain(this);
            foreach (var item in viewModel.FuelTypes)
            {
                if (!keepRunning)
                {
                    break;
                }

                if (viewModel.States.Any(t => t > 0))
                {
                    foreach (var state in viewModel.States)
                    {
                        var globalFuelCost = await currentCostDomain.GetFuelCostForFuelRequest(viewModel.CompanyId, item, state, (viewModel.CountryId == (int)Country.USA ? UoM.Gallons : UoM.Litres));                        
                        if (!globalFuelCost.HasValue)
                        {
                            statusViewModel.StatusCode = Status.Failed;
                            statusViewModel.StatusMessage = Resource.errFuelCostNotProvidedForOffer;
                            keepRunning = false;
                            break;
                        }
                        else
                        {
                            viewModel.FuelPricing.SupplierCost = globalFuelCost;
                        }
                    }
                }
                else if (viewModel.LocationViewModel.Any())
                {
                    foreach (var offerLocation in viewModel.LocationViewModel)
                    {
                        var globalFuelCost = await currentCostDomain.GetFuelCostForFuelRequest(viewModel.CompanyId, item, offerLocation.StateId, (viewModel.CountryId == (int)Country.USA ? UoM.Gallons : UoM.Litres));
                        if (!globalFuelCost.HasValue)
                        {
                            statusViewModel.StatusCode = Status.Failed;
                            statusViewModel.StatusMessage = Resource.errFuelCostNotProvidedForOffer;
                            keepRunning = false;
                            break;
                        }
                        else
                        {
                            viewModel.FuelPricing.SupplierCost = globalFuelCost;
                        }
                    }
                }
                else
                {
                    var globalFuelCost = await currentCostDomain.GetFuelCostForFuelRequest(viewModel.CompanyId, item, _allStates, (viewModel.CountryId == (int)Country.USA ? UoM.Gallons : UoM.Litres));
                    if (!globalFuelCost.HasValue)
                    {
                        statusViewModel.StatusCode = Status.Failed;
                        statusViewModel.StatusMessage = Resource.errFuelCostNotProvidedForOffer;
                        keepRunning = false;
                        break;
                    }
                    else
                    {
                        viewModel.FuelPricing.SupplierCost = globalFuelCost;
                    }
                }
            }

            return keepRunning;
        }

        private static string GenerateRandomJobName(string prefix, int stringLength)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rndGenerator.Next(0, allowedChars.Length)];
            }

            return String.Concat(prefix, new string(chars));
        }

        private bool IsJobLocationValidForOffer(OfferPricing offerPricing, int stateId, string city, string zipcode)
        {
            bool response = false;
            var offerPricingItems = offerPricing.OfferPricingItems;

            response = offerPricingItems.Any(t => !t.StateId.HasValue) ||
                        offerPricingItems.Count == 0 ||
                        (
                            // job location exactly matches with offer location
                            offerPricingItems.Any(t1 => t1.StateId.HasValue && t1.StateId.Value == stateId &&
                            //t1.CityId.HasValue && t1.MstCity.Name == city && 
                            t1.ZipCode != null && t1.ZipCode == zipcode)
                        ) ||
                        (
                            // jobs state & city matches with offer location & offer zip is null
                            offerPricingItems.Any(t1 => t1.StateId.HasValue && t1.StateId.Value == stateId &&
                            //t1.CityId.HasValue && t1.MstCity.Name == city && 
                            t1.ZipCode == null)
                        )
                        //||
                        //(
                        //    // jobs state matches with offer location & offer city + zip is null
                        //    offerLocations.Any(t1 => t1.StateId.HasValue && t1.StateId.Value == stateId &&
                        //    !t1.CityId.HasValue &&
                        //    t1.ZipCode == null)
                        //)
                        ;

            return response;
        }
        #endregion
    }
}
