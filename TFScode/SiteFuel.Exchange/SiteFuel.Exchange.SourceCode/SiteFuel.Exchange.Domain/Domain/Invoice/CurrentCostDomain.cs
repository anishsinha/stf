using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core.Logger;
using Newtonsoft.Json;

namespace SiteFuel.Exchange.Domain
{
    public class CurrentCostDomain : BaseDomain
    {
        public CurrentCostDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public CurrentCostDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<CurrentCostGridViewModel>> GetCurrentCostGridAsync(UserContext userContext, int countryId = (int)Country.USA, Currency currency = Currency.USD)
        {
            using (var tracer = new Tracer("CurrentCostDomain", "GetCurrentCostGridAsync"))
            {
                var response = new List<CurrentCostGridViewModel>();
                try
                {
                    var userCurrentCost = await Context.DataContext.StateCurrentCosts.Where(t => t.CurrentCost.SupplierCompanyId == userContext.CompanyId
                                                    && t.IsActive && t.CurrentCost.IsGlobleCost && t.CurrentCost.Currency == currency && t.CurrentCost.CountryId == countryId
                                                    && t.IsActive)
                                                    .OrderByDescending(t => t.CurrentCost.CreatedDate)
                                                    .GroupBy(t => t.CurrentCostId)
                                                    .Select(t => new
                                                    {
                                                        currentCost = t.Select(t1 =>
                                                                      new
                                                                      {
                                                                          t1.CurrentCostId,
                                                                          t1.CurrentCost.CreatedDate,
                                                                          t1.CurrentCost.Cost,
                                                                          t1.CurrentCost.FuelTypeId,
                                                                          ProductName = t1.CurrentCost.MstTfxProduct.Name,
                                                                          t1.CurrentCost.UoM
                                                                      }).FirstOrDefault(),
                                                        stateIds = t.Select(t1 => t1.StateId),
                                                        stateCodes = t.Select(t1 => t1.MstState.Code)
                                                    })
                                                    .ToListAsync();

                    foreach (var item in userCurrentCost)
                    {
                        var currentCost = item.currentCost;
                        var stateCodes = item.stateCodes;
                        var stateIds = item.stateIds.Where(t => t.HasValue).ToList();
                        var states = string.Join(",", stateCodes);
                        if (string.IsNullOrEmpty(states))
                        {
                            states = Resource.lblAllStates;
                            stateIds = new List<int?>();
                        }

                        response.Add(new CurrentCostGridViewModel()
                        {
                            Id = currentCost.CurrentCostId,
                            CurrentCostOfFuel = currentCost.Cost,
                            FuelTypeId = currentCost.FuelTypeId,
                            FuelTypeName = currentCost.ProductName,
                            stateCodes = states,
                            stateId = stateIds,
                            UoM = currentCost.UoM
                        });
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("CurrentCostDomain", "GetCurrentCostGridAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public StateCurrentCost GetCurrentCostStateModel(UserContext userContext, int currentCostId, int? stateId = null)
        {
            StateCurrentCost currentcostXState = new StateCurrentCost()
            {
                CreatedBy = userContext.Id,
                IsActive = true,
                CreatedDate = DateTimeOffset.Now,
                StateId = stateId,
                CurrentCostId = currentCostId,
                UpdatedBy = userContext.Id,
                UpdatedDate = DateTimeOffset.Now
            };
            return currentcostXState;
        }

        public async Task<bool> AddCurrentCostGridAsync(UserContext userContext, int fuelTypeId, decimal cost, bool isNewEntry, List<int?> stateIds, int countryId, Currency currency,UoM uom)
        {
            using (var tracer = new Tracer("CurrentCostDomain", "AddCurrentCostGridAsync"))
            {
                var response = false;
                try
                {
                    var currentcost = GetCurrentCostModel(userContext, fuelTypeId, cost, countryId, currency, uom);
                    Context.DataContext.CurrentCosts.Add(currentcost);
                    await Context.CommitAsync();

                    //add states for currentcost
                    if (!stateIds.Any())
                    {
                        Context.DataContext.StateCurrentCosts.Add(GetCurrentCostStateModel(userContext, currentcost.Id));
                    }
                    else
                    {
                        foreach (var stateId in stateIds)
                        {
                            Context.DataContext.StateCurrentCosts.Add(GetCurrentCostStateModel(userContext, currentcost.Id, stateId));
                        }
                    }
                    await UpdateOrdersForGlobalCost(userContext, fuelTypeId, cost, stateIds, countryId, currency, uom);

                    await Context.CommitAsync();
                    response = true;
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("CurrentCostDomain", "AddCurrentCostGridAsync", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task UpdateOrdersForGlobalCost(UserContext userContext, int tfxfuelTypeId, decimal cost, List<int?> stateIds, int countryId, Currency currency ,UoM uom)
        {
            var ordersList = await Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedBy == userContext.Id
                                                          && tfxfuelTypeId == (t.FuelRequest.MstProduct.TfxProductId ?? t.FuelRequest.FuelTypeId)
                                                          && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)
                                                          && t.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost
                                                          //&& t.FuelRequest.SupplierCostTypeId != null
                                                          && t.FuelRequest.Currency == currency
                                                          && t.FuelRequest.UoM == uom
                                                          //&& t.FuelRequest.SupplierCostTypeId == (int)SupplierCostTypes.GlobalCost
                                                          ).ToListAsync();
            if (ordersList.Any())
            {
                var orderPriceDetailIds = ordersList.Select(t => t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId).ToList();
                var pricingDomain = new PricingServiceDomain(this);
                var filteredPriceDetails = await pricingDomain.GetFilterPriceDetailsByPricingType(new FilterPricingRequestViewModel { PriceDetailIds = orderPriceDetailIds, PricingType = (int)SupplierCostTypes.GlobalCost });

                if (filteredPriceDetails != null && filteredPriceDetails.Status == Status.Success && filteredPriceDetails.ListResult != null)
                {
                    var ordersToUpdate = ordersList.Where(t => filteredPriceDetails.ListResult.Contains(t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId));
                    if (stateIds.FirstOrDefault() == null)
                    {
                        var allStateForFuelType = Context.DataContext.StateCurrentCosts
                                                    .Where(t => t.IsActive && t.CurrentCost.FuelTypeId == tfxfuelTypeId
                                                            && t.CurrentCost.IsActive && t.CurrentCost.IsGlobleCost
                                                            && t.CurrentCost.Currency == currency
                                                            && t.CurrentCost.CountryId == countryId
                                                            && t.CurrentCost.SupplierCompanyId == userContext.CompanyId && t.StateId.HasValue)
                                                    .Select(t1 => t1.StateId).ToList();
                        ordersToUpdate = ordersToUpdate.Where(t => !allStateForFuelType.Contains(t.FuelRequest.Job.StateId)).ToList();
                    }
                    else
                    {
                        var servingstateIds = stateIds.Where(t => t.HasValue).ToList();

                        if (servingstateIds.Count > 0)
                        {
                            ordersToUpdate = ordersToUpdate.Where(t => servingstateIds.Contains(t.FuelRequest.Job.StateId)).ToList();
                        }
                    }

                    NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);

                    var priceDetailIds = ordersToUpdate.Select(t => t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId).ToList();
                    var originalPricing = await UpdateFuelCostForFuelRequest(priceDetailIds, cost, (int)SupplierCostTypes.GlobalCost);
                    if (originalPricing != null && originalPricing.Cost.Any())
                    {
                        foreach (var item in ordersToUpdate)
                        {
                            decimal originalcost = originalPricing.Cost.Where(t => t.PriceDetailId == item.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId).FirstOrDefault().previousCost;
                            if (originalcost != cost)
                            {
                                await newsfeedDomain.SetGlobalCostUpdateNewsfeed(userContext, item, NewsfeedEvent.GlobalCostUpdate, originalcost, cost);
                            }
                        }
                    }
                }
            }
        }

        public async Task<List<int?>> GetOrderExistForStatesCurrentCost(int userId, int tfxfuelTypeId, List<int?> previousStateIds, int countryId, Currency currency)
        {
            var stateForOrderExist = new List<int?>();
            foreach (var stateId in previousStateIds)
            {
                var ExistingOrder = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedBy == userId
                                                           && (tfxfuelTypeId == (t.FuelRequest.MstProduct.TfxProductId ?? t.FuelRequest.FuelTypeId))
                                                           && t.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost
                                                           //&& t.FuelRequest.SupplierCostTypeId == (int)SupplierCostTypes.GlobalCost
                                                           && t.FuelRequest.Currency == currency
                                                           && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)
                                                           );
                if (ExistingOrder != null)
                {
                    var orderPriceDetailIds = ExistingOrder.Select(t => t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId).ToList();
                    var pricingDomain = new PricingServiceDomain(this);
                    var filteredPriceDetails = await pricingDomain.GetFilterPriceDetailsByPricingType(new FilterPricingRequestViewModel { PriceDetailIds = orderPriceDetailIds, PricingType = (int)SupplierCostTypes.GlobalCost });
                    if (filteredPriceDetails != null && filteredPriceDetails.Status == Status.Success && filteredPriceDetails.ListResult != null)
                    {
                        ExistingOrder = ExistingOrder.Where(t => filteredPriceDetails.ListResult.Contains(t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId));
                        if (stateId.HasValue)
                        {
                            var isOrderExistForState = ExistingOrder.Any(t => t.FuelRequest.Job.StateId == stateId);
                            if (isOrderExistForState) { stateForOrderExist.Add(stateId); }
                        }
                        else
                        {
                            await ExistingOrder.ForEachAsync(t => stateForOrderExist.Add(t.FuelRequest.Job.StateId));
                        }
                    }
                }
                var counterOffer = Context.DataContext.FuelRequests.Where(t => t.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest
                                                                        && t.User.Id == userId
                                                                        && t.PricingTypeId == (int)PricingType.Suppliercost
                                                                        && t.Currency == currency
                                                                        && tfxfuelTypeId == (t.MstProduct.TfxProductId ?? t.FuelTypeId));
                if (counterOffer != null)
                {
                    if (stateId.HasValue)
                    {
                        var isCounterOfferExistForState = counterOffer.Any(t => t.Job.StateId == stateId);
                        if (isCounterOfferExistForState) { stateForOrderExist.Add(stateId); }
                    }
                    else
                    {
                        await counterOffer.ForEachAsync(t => stateForOrderExist.Add(t.Job.StateId));
                    }
                }

            }
            return stateForOrderExist;
        }

        public async Task<StatusViewModel> UpdateCurrentCostGridAsync(UserContext userContext, int currentCostId, decimal cost, List<int?> stateIds, int countryId, Currency currency,UoM uom,bool isFromGFCDashBoard = false)
        {
            var response = new StatusViewModel();
            try
            {
                if (stateIds.Count == 0) { stateIds.Add(null); }
                var currentCost = await Context.DataContext.CurrentCosts.FirstOrDefaultAsync(t => t.Id == currentCostId);
                var previousStateIds = await Context.DataContext.StateCurrentCosts.Where(t => t.IsActive && t.CurrentCostId == currentCostId).Select(t1 => t1.StateId).ToListAsync();
                //check if removing states have any order exist ?
                // Check if new states already exists.. (if so don't allow them..)
                if (!IsAnyCurrentCostExist(userContext.CompanyId, currentCost.FuelTypeId, currentCostId, stateIds, countryId, currency ,uom))
                {
                    var stateForOrderExist = new List<int?>();
                    if (stateIds.FirstOrDefault() != null)
                    {
                        stateForOrderExist = await GetOrderExistForStatesCurrentCost(userContext.Id, currentCost.FuelTypeId, previousStateIds, countryId, currency);
                    }

                    if (!stateForOrderExist.Except(stateIds).Any() || stateIds.FirstOrDefault() == null)
                    {

                        if (currentCost.UoM != uom) //Need to check if any order exists for existing UOM + FuelTypeCombination
                        {
                            StatusViewModel deleteStatus = await IsOpenOrderOrCounterOfferExistForFuelType(userContext, currentCost.FuelTypeId, currentCostId, currency, currentCost.UoM);
                            if (deleteStatus.StatusCode == Status.Success)
                            {
                                await UpdateExistingCostToInactive(userContext, currentCost.FuelTypeId, previousStateIds, countryId, currency, isFromGFCDashBoard ? currentCost.UoM : uom);
                                var newCurrentCost = await AddCurrentCostWithStates(userContext, currentCost.FuelTypeId, cost, stateIds, countryId, currency, uom);
                                await UpdateOrdersForGlobalCost(userContext, newCurrentCost.FuelTypeId, cost, stateIds, countryId, currency, uom);

                            }
                            else
                            {
                                response.StatusMessage = Resource.errMsgOpenOrderOrCounterOfferExist;
                                response.StatusCode = Status.Failed;
                                return response;
                            }
                        }
                        else // only price or states changed
                        {
                            await UpdateExistingCostToInactive(userContext, currentCost.FuelTypeId, previousStateIds, countryId, currency, isFromGFCDashBoard ? currentCost.UoM : uom);
                            var newCurrentCost = await AddCurrentCostWithStates(userContext, currentCost.FuelTypeId, cost, stateIds, countryId, currency, uom);
                            await UpdateOrdersForGlobalCost(userContext, newCurrentCost.FuelTypeId, cost, stateIds, countryId, currency, uom);

                        }
                        await Context.CommitAsync();
                        response.StatusCode = Status.Success;
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMsgOpenOrderOrCounterOfferExist;
                    }
                }
                else
                {
                    response.StatusMessage = Resource.errMsgSupplierCostAlreadyPresent;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "AddCurrentCostGridAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<CurrentCost> AddCurrentCostWithStates(UserContext userContext, int fuelTypeId, decimal cost, List<int?> stateIds, int countryId, Currency currency,UoM uom =UoM.None)
        {
            var newCurrentCost = GetCurrentCostModel(userContext, fuelTypeId, cost, countryId, currency, uom);
            Context.DataContext.CurrentCosts.Add(newCurrentCost);
            await Context.CommitAsync();

            foreach (var stateId in stateIds)
            {
                Context.DataContext.StateCurrentCosts.Add(GetCurrentCostStateModel(userContext, newCurrentCost.Id, stateId));
            }

            await Context.CommitAsync();
            return newCurrentCost;
        }

        public async Task<decimal?> GetFuelCostForFuelRequest(int supplierCompanyId, int tfxFueltypeId, int? fuelRequestStateId,UoM uom , Currency currency = Currency.USD)
        {
            var availableGlobalCost = Context.DataContext.CurrentCosts.Where(t => t.SupplierCompanyId == supplierCompanyId
                                                    && t.IsActive && t.IsGlobleCost 
                                                    && t.UoM==uom
                                                    && t.FuelTypeId == tfxFueltypeId
                                                    && t.Currency == currency).Select(t1 => t1.Id);
            if (availableGlobalCost.Any())
            {
                var isStateAvailable = await Context.DataContext.StateCurrentCosts.FirstOrDefaultAsync(t => availableGlobalCost.Contains(t.CurrentCostId) && t.IsActive && t.StateId == fuelRequestStateId);
                if (isStateAvailable != null)
                {
                    return isStateAvailable.CurrentCost.Cost;
                }
                else
                {
                    var isAvailableforAllStates = await Context.DataContext.StateCurrentCosts.FirstOrDefaultAsync(t => availableGlobalCost.Contains(t.CurrentCostId) && t.IsActive && t.StateId == null);
                    if (isAvailableforAllStates != null)
                    {
                        return isAvailableforAllStates.CurrentCost.Cost;
                    }
                }
            }
            return null;
        }
        public async Task<CurrentCostResponseModel> UpdateFuelCostForFuelRequest(List<int> requestPriceIds, decimal cost, int supplierCostTypeId)
        {
            var pricingServiceDomain = new PricingServiceDomain(this);
            var requestModel = new CurrentCostRequestModel()
            {
                RequestPriceDetailIds = requestPriceIds,
                Cost = cost,
                SupplierCostType = supplierCostTypeId
            };
            var response = await pricingServiceDomain.UpdateFuelCostForFuelRequestAsync(requestModel);
            return response;
        }
        public CurrentCost GetCurrentCostModel(UserContext userContext, int fuelTypeId, decimal cost, int countryId, Currency currency,UoM uom = UoM.None)
        {
            CurrentCost currentCost = new CurrentCost();
            currentCost.Cost = cost;
            currentCost.CreatedBy = userContext.Id;
            currentCost.CreatedDate = DateTimeOffset.Now;
            currentCost.FuelTypeId = fuelTypeId;
            currentCost.IsActive = true;
            currentCost.IsGlobleCost = true;
            currentCost.PricingTypeId = (int)PricingType.Suppliercost;
            currentCost.SupplierCompanyId = userContext.CompanyId;
            currentCost.UpdatedBy = userContext.Id;
            currentCost.UpdatedDate = DateTimeOffset.Now;
            currentCost.Currency = currency;
            currentCost.CountryId = countryId;
            if (uom == UoM.None)
            {
                currentCost.UoM = countryId == (int)Country.USA ? UoM.Gallons : UoM.Litres;
            }           
            else { currentCost.UoM = uom; }

            return currentCost;
        }

        public async Task InactiveCurrentFuelCost(UserContext userContext, int fuelTypeId, int currentCostId, int countryId, Currency currency ,UoM uom)
        {
            try
            {
                List<int?> stateIds = null;
                stateIds = Context.DataContext.StateCurrentCosts.Where(t => t.CurrentCostId == currentCostId && t.IsActive).Select(t => t.StateId).ToList();

                await UpdateExistingCostToInactive(userContext, fuelTypeId, stateIds, countryId, currency,uom);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "InactiveCurrentFuelCost", ex.Message, ex);
            }
        }

        public List<StateCurrentCost> GetAllStatesForCurrentCost(int CompanyId, int fuelTypeId, Currency currency)
        {
            var stateCurrentCosts = Context.DataContext.StateCurrentCosts.Where(t => t.CurrentCost.IsActive && t.CurrentCost.SupplierCompanyId == CompanyId
                                      && t.CurrentCost.IsGlobleCost && t.CurrentCost.FuelTypeId == fuelTypeId && t.IsActive
                                      && t.CurrentCost.Currency == currency).ToList();
            return stateCurrentCosts;
        }

        public async Task<StatusViewModel> IsOpenOrderOrCounterOfferExistForFuelType(UserContext userContext, int tfxfuelTypeId, int currentCostId, Currency currency,UoM uom)
        {
            StatusViewModel response = new StatusViewModel(Status.Success);
            try
            {
                var stateCurrentCosts = GetAllStatesForCurrentCost(userContext.CompanyId, tfxfuelTypeId, currency);
                var stateIds = stateCurrentCosts.Where(t => t.CurrentCostId == currentCostId && t.CurrentCost.Currency == currency).Select(t1 => t1.StateId).ToList();
                bool isOpenOrderExist = false;
                var stateIdsWONull = stateCurrentCosts.Where(t => t.StateId != null).Select(t1 => t1.StateId).ToList();
                var openOrders = Context.DataContext.Orders.Where(t => t.IsActive && t.AcceptedBy == userContext.Id
                                                                && tfxfuelTypeId == (t.FuelRequest.MstProduct.TfxProductId ?? t.FuelRequest.FuelTypeId)
                                                                && t.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost
                                                                //&& t.FuelRequest.SupplierCostTypeId == (int)SupplierCostTypes.GlobalCost
                                                                && t.FuelRequest.Currency == currency
                                                                && t.FuelRequest.UoM == uom
                                                                && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive)
                                                               );
                if (openOrders.Any())
                {
                    var orderPriceDetailIds = openOrders.Select(t => t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId).ToList();
                    var pricingDomain = new PricingServiceDomain(this);
                    var filteredPriceDetails = await pricingDomain.GetFilterPriceDetailsByPricingType(new FilterPricingRequestViewModel { PriceDetailIds = orderPriceDetailIds, PricingType = (int)SupplierCostTypes.GlobalCost });
                    if (filteredPriceDetails != null && filteredPriceDetails.Status == Status.Success && filteredPriceDetails.ListResult != null)
                    {
                        openOrders = openOrders.Where(t => filteredPriceDetails.ListResult.Contains(t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId));
                        if (stateIds.FirstOrDefault() == null && stateCurrentCosts.Count == 1)
                        {
                            isOpenOrderExist = openOrders.Any();
                        }
                        else if (stateIds.FirstOrDefault() == null)
                        {
                            isOpenOrderExist = openOrders.Any(t => !stateIdsWONull.Contains(t.FuelRequest.Job.StateId));
                        }
                        else
                        {
                            isOpenOrderExist = openOrders.Any(t => stateIds.Contains(t.FuelRequest.Job.StateId));
                        }
                    }
                }
                if (!isOpenOrderExist)
                {
                    bool isCounterOfferExist = false;
                    var counterOffer = Context.DataContext.FuelRequests.Where(t => t.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest
                                                                        && t.User.CompanyId == userContext.CompanyId
                                                                        && t.PricingTypeId == (int)PricingType.Suppliercost
                                                                        && t.Currency == currency
                                                                        && t.UoM == uom
                                                                        && t.IsActive
                                                                        && tfxfuelTypeId == (t.MstProduct.TfxProductId ?? t.FuelTypeId));
                    if (stateIds.FirstOrDefault() == null && stateCurrentCosts.Count == 1)
                    {
                        isCounterOfferExist = counterOffer.Any();
                    }
                    else if (stateIds.FirstOrDefault() == null)
                    {
                        isCounterOfferExist = counterOffer.Any(t => !stateIdsWONull.Contains(t.Job.StateId));
                    }
                    else
                    {
                        isCounterOfferExist = counterOffer.Any(t => stateIds.Contains(t.Job.StateId));
                    }

                    if (isCounterOfferExist)
                    {
                        response.StatusCode = Status.Failed;
                        response.StatusMessage = Resource.errMsgCounterOfferExist;
                    }
                }
                else
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMsgOpenOrderExist;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "IsOpenOrderOrCounterOfferExistForFuelType", ex.Message, ex);
            }
            return response;
        }

        public bool IsAnyCurrentCostExist(int companyId, int fuelTypeId, int currentCostId, List<int?> stateIds, int countryId, Currency currency,UoM uom)
        {
            var response = false;
            response = Context.DataContext.StateCurrentCosts
                .Any(t => t.CurrentCost.SupplierCompanyId == companyId &&
                                        t.IsActive && t.CurrentCost.IsGlobleCost
                                        && t.CurrentCostId != currentCostId
                                        && t.CurrentCost.FuelTypeId == fuelTypeId
                                        && t.CurrentCost.Currency == currency
                                        && t.CurrentCost.UoM == uom
                                        && t.CurrentCost.IsActive &&
                                        (stateIds.Contains(t.StateId) || stateIds.FirstOrDefault() == null)
                                        );

            return response;
        }

        public async Task UpdateExistingCostToInactive(UserContext userContext, int fuelTypeId, List<int?> stateIds, int countryId, Currency currency,UoM uom)
        {
            if (stateIds.FirstOrDefault().HasValue)
            {
                var activeCurrentcostStates = await Context.DataContext.StateCurrentCosts
                .Where(t => t.CurrentCost.SupplierCompanyId == userContext.CompanyId &&
                                        t.IsActive && t.CurrentCost.IsGlobleCost
                                        && t.CurrentCost.FuelTypeId == fuelTypeId
                                        && t.CurrentCost.Currency == currency
                                        && t.CurrentCost.IsActive 
                                        && t.CurrentCost.UoM == uom 
                                        && stateIds.Contains(t.StateId)
                                        ).ToListAsync();

                if (activeCurrentcostStates.Count > 0)
                {
                    activeCurrentcostStates.ForEach(t =>
                    {
                        t.IsActive = false;
                        t.UpdatedBy = userContext.Id;
                        t.UpdatedDate = DateTimeOffset.Now;
                        t.CurrentCost.IsActive = false;
                        t.CurrentCost.UpdatedBy = userContext.Id;
                        t.CurrentCost.UpdatedDate = DateTimeOffset.Now;
                    });

                    await Context.CommitAsync();
                }
            }
            else
            {
                var activeCurrentcost = await Context.DataContext.StateCurrentCosts
                .Where(t => t.CurrentCost.SupplierCompanyId == userContext.CompanyId &&
                                        t.IsActive && t.CurrentCost.IsGlobleCost
                                        && t.CurrentCost.FuelTypeId == fuelTypeId
                                        && t.CurrentCost.Currency == currency
                                        && t.CurrentCost.IsActive
                                        && t.CurrentCost.UoM == uom
                                        && !t.StateId.HasValue
                                        ).ToListAsync();
                if (activeCurrentcost.Count > 0)
                {
                    activeCurrentcost.ForEach(t =>
                    {
                        t.IsActive = false;
                        t.UpdatedBy = userContext.Id;
                        t.UpdatedDate = DateTimeOffset.Now;
                    });
                    var currentCost = activeCurrentcost.FirstOrDefault().CurrentCost;
                    currentCost.IsActive = false;
                    currentCost.UpdatedBy = userContext.Id;
                    currentCost.UpdatedDate = DateTimeOffset.Now;

                    await Context.CommitAsync();
                }
            }
        }

        public async Task UpdateExistingFuelRequestCostToInactive(UserContext userContext, int fuelRequestId)
        {
            var activeFuelRequestCurrentcost = await Context.DataContext.FuelRequestCurrentCosts.Where(t => t.FuelRequestId == fuelRequestId && t.IsActive).ToListAsync();
            if (activeFuelRequestCurrentcost != null)
            {
                foreach (var item in activeFuelRequestCurrentcost)
                {
                    item.IsActive = false;
                    item.UpdatedBy = userContext.Id;
                    item.UpdatedDate = DateTimeOffset.Now;
                }
                await Context.CommitAsync();
            }
        }

        public async Task<List<DropdownDisplayItem>> GetGFCNotDefinedStates(int companyId, int fuelTypeId, int currentCostId, int countryId, Currency currency)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var companyDomain = new CompanyDomain(this);
                var allServingStates = await companyDomain.GetCompanyServingStates(companyId, countryId);
                var AllStateDefinedForGFC = new List<StateCurrentCost>();
                if (fuelTypeId != 0)
                {
                    AllStateDefinedForGFC = GetAllStatesForCurrentCost(companyId, fuelTypeId, currency).Where(t => t.StateId != null && t.CurrentCostId != currentCostId).ToList();
                }
                foreach (var state in allServingStates)
                {
                   // if (!definedStates.Contains(state.Id))
                //   {
                        response.Add(new DropdownDisplayItem() { Id = state.Id, Name = state.Name });
                 //   }
                }
                return response;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "GetGFCNotDefinedStates", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int?>> GetApplicableStatesForFuelCost(UpdateCurrentCostViewModel updateCurrentCost)
        {
            var response = new List<int?>();
            try
            {
                var stateCurrentCost = await Context.DataContext.StateCurrentCosts
                                                    .FirstOrDefaultAsync(t => t.CurrentCost.FuelTypeId == updateCurrentCost.TfxFuelTypeId
                                                           && t.CurrentCost.IsActive && t.IsActive && t.CurrentCost.IsGlobleCost
                                                           && t.StateId == updateCurrentCost.JobStateId
                                                           && t.CurrentCost.Currency == updateCurrentCost.CurrencyType
                                                           && t.CurrentCost.CountryId == updateCurrentCost.CountryId);
                if (stateCurrentCost == null)
                {
                    stateCurrentCost = await Context.DataContext.StateCurrentCosts
                                                    .FirstOrDefaultAsync(t => t.CurrentCost.FuelTypeId == updateCurrentCost.TfxFuelTypeId
                                                           && t.CurrentCost.IsActive && t.IsActive && t.CurrentCost.IsGlobleCost
                                                           && t.CurrentCost.Currency == updateCurrentCost.CurrencyType
                                                           && t.CurrentCost.CountryId == updateCurrentCost.CountryId
                                                           && t.StateId == null);
                }
                response = await Context.DataContext.StateCurrentCosts
                                                    .Where(t => t.CurrentCostId == stateCurrentCost.CurrentCostId && t.IsActive).Select(t1 => t1.StateId).ToListAsync();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "GetApplicableStatesForFuelCost", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateGlobalFuelCost(UserContext userContext, UpdateCurrentCostViewModel updateCurrentCost)
        {
            var response = new StatusViewModel(Status.Success);
            var stateApplicableForCost = await GetApplicableStatesForFuelCost(updateCurrentCost);
            var ordersList = await Context.DataContext.Orders.Where(t => t.IsActive
                                                            && (t.AcceptedBy == userContext.Id
                                                            && t.FuelRequest.FuelTypeId == updateCurrentCost.FuelTypeId
                                                            && t.FuelRequest.PricingTypeId == (int)PricingType.Suppliercost
                                                            //&& t.FuelRequest.SupplierCostTypeId == (int)SupplierCostTypes.GlobalCost
                                                            && t.FuelRequest.Currency == updateCurrentCost.CurrencyType
                                                            && stateApplicableForCost.Contains(t.FuelRequest.Job.StateId)
                                                            && t.OrderXStatuses.Any(t1 => t1.StatusId == (int)OrderStatus.Open && t1.IsActive))
                                                            || t.FuelRequestId == updateCurrentCost.FuelRequestId).ToListAsync();

            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    await UpdateExistingCostToInactive(userContext, updateCurrentCost.TfxFuelTypeId, stateApplicableForCost, updateCurrentCost.CountryId, updateCurrentCost.CurrencyType,updateCurrentCost.UoM);

                    var currentCostObj = await AddCurrentCostWithStates(userContext, updateCurrentCost.TfxFuelTypeId, updateCurrentCost.FuelCost, stateApplicableForCost, updateCurrentCost.CountryId, updateCurrentCost.CurrencyType);

                    if (ordersList.Any())
                    {
                        var priceDetailIds = ordersList.Select(t => t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId).Distinct().ToList();
                        var pricingDomain = new PricingServiceDomain(this);
                        var filteredPriceDetails = await pricingDomain.GetFilterPriceDetailsByPricingType(new FilterPricingRequestViewModel { PriceDetailIds = priceDetailIds, PricingType = (int)SupplierCostTypes.GlobalCost });
                        if (filteredPriceDetails != null && filteredPriceDetails.Status == Status.Success && filteredPriceDetails.ListResult != null)
                        {
                            ordersList = ordersList.Where(t => filteredPriceDetails.ListResult.Contains(t.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId)).ToList();
                            var originalPricing = await UpdateFuelCostForFuelRequest(priceDetailIds, updateCurrentCost.FuelCost, (int)SupplierCostTypes.GlobalCost);
                            if (originalPricing != null)
                            {
                                foreach (var item in ordersList)
                                {
                                    var priceDetailId = item.FuelRequest.FuelRequestPricingDetail.RequestPriceDetailId;
                                    var originalPrice = originalPricing.Cost.FirstOrDefault(t => t.PriceDetailId == priceDetailId);

                                    await UpdateExistingFuelRequestCostToInactive(userContext, item.FuelRequestId);
                                    var frCurrentCostObj = GetFuelRequestCurrentCost(userContext, item.FuelRequestId, currentCostObj.Id);
                                    Context.DataContext.FuelRequestCurrentCosts.Add(frCurrentCostObj);
                                    await Context.CommitAsync();

                                    if (originalPrice.previousCostType == (int)SupplierCostTypes.SupplierCost)
                                    {
                                        await newsfeedDomain.SetCurrentToGlobalCostUpdateNewsfeed(userContext, item, NewsfeedEvent.CurrentCostToGlobalCost);
                                    }
                                    else if (originalPrice.previousCost != updateCurrentCost.FuelCost)
                                    {
                                        await newsfeedDomain.SetGlobalCostUpdateNewsfeed(userContext, item, NewsfeedEvent.GlobalCostUpdate, originalPrice.previousCost, updateCurrentCost.FuelCost);
                                    }
                                }
                            }
                        }
                    }
                    transaction.Commit();
                    response.StatusMessage = Resource.successMessageForGlobalFuelCost;

                    return response;
                }
                catch (Exception ex)
                {
                    response.StatusCode = Status.Failed;
                    response.StatusMessage = Resource.errMessageGlobalCostUpdateFailed;
                    transaction.Rollback();
                    LogManager.Logger.WriteException("CurrentCostDomain", "UpdateGlobalFuelCost", ex.Message, ex);
                }
            }
            return response;
        }

        public async Task<StatusViewModel> UpdateSupplierCostForOrder(UserContext userContext, UpdateCurrentCostViewModel updateCost)
        {
            var response = new StatusViewModel();

            var fuelRequestOfOrder = await Context.DataContext.FuelRequests.SingleOrDefaultAsync(t => t.Id == updateCost.FuelRequestId);
            if (updateCost.OriginalFuelCost != updateCost.FuelCost || updateCost.SupplierFuelCostTypeId != (int)SupplierCostTypes.SupplierCost)
            {
                using (var transaction = Context.DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        //add existing cost to current cost table - for history maintaining
                        var currentcost = GetCurrentCostModel(userContext, updateCost.TfxFuelTypeId, updateCost.FuelCost, updateCost.CountryId, updateCost.CurrencyType);
                        currentcost.IsGlobleCost = false;
                        Context.DataContext.CurrentCosts.Add(currentcost);
                        await Context.CommitAsync();

                        await UpdateExistingFuelRequestCostToInactive(userContext, updateCost.FuelRequestId);
                        var frCurrentCost = GetFuelRequestCurrentCost(userContext, updateCost.FuelRequestId, currentcost.Id);
                        Context.DataContext.FuelRequestCurrentCosts.Add(frCurrentCost);
                        await Context.CommitAsync();

                        var priceDetailIds = new List<int>();
                        priceDetailIds.Add(fuelRequestOfOrder.FuelRequestPricingDetail.RequestPriceDetailId);
                        var originalPrice = await UpdateFuelCostForFuelRequest(priceDetailIds, updateCost.FuelCost, (int)SupplierCostTypes.SupplierCost);

                        //fuelRequestOfOrder.SupplierCost = updateCost.FuelCost;
                        //fuelRequestOfOrder.SupplierCostTypeId = (int)SupplierCostTypes.SupplierCost;
                        //await Context.CommitAsync();
                        transaction.Commit();
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.successMessageForCurrentFuelCost;


                        if (originalPrice != null && originalPrice.Status == Status.Success)
                        {
                            //var costTypeId = originalPrice.Cost.FirstOrDefault().previousCostType;
                            //var cost = originalPrice.Cost.FirstOrDefault().previousCost;

                            NewsfeedDomain newsfeedDomain = new NewsfeedDomain(this);
                            if (updateCost.SupplierFuelCostTypeId == (int)SupplierCostTypes.GlobalCost)
                            {
                                await newsfeedDomain.SetGlobalToCurrentCostUpdateNewsfeed(userContext, fuelRequestOfOrder, NewsfeedEvent.GlobalCostToCurrentCost);
                            }
                            else if (updateCost.OriginalFuelCost != updateCost.FuelCost)
                            {
                                await newsfeedDomain.SetCurrentCostUpdateNewsfeed(userContext, fuelRequestOfOrder, NewsfeedEvent.CurrentCostUpdate, updateCost.OriginalFuelCost, updateCost.FuelCost);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        LogManager.Logger.WriteException("CurrentCostDomain", "UpdateFuelRequestsSupplierCost", ex.Message, ex);
                    }
                }
            }
            return response;
        }

        //private async Task<IntResponseModel> AddAndUpdateFuelCost(UserContext userContext, decimal cost, int countryId, int FuelTypeId, int currency, int requestPriceDetailId, List<int?> stateIds = null)
        //{
        //    var pricingServiceDomain = new PricingServiceDomain(this);
        //    var requestPriceDetailIds = new List<int>();
        //    requestPriceDetailIds.Add(requestPriceDetailId);
        //    var requestModel = new FuelCostRequestModel()
        //    {
        //        CompanyId = userContext.CompanyId,
        //        UserId = userContext.Id,
        //        Cost = cost,
        //        CountryId = countryId,
        //        FuelTypeId = FuelTypeId,
        //        Currency = currency,
        //        IsGlobalCost = false,
        //        RequestPriceDetailId = requestPriceDetailIds,
        //        StateIds = stateIds
        //    };
        //    var result = await pricingServiceDomain.AddAndUpdateCurrentCost(requestModel);
        //    return result;
        //}

        public FuelRequestCurrentCost GetFuelRequestCurrentCost(UserContext userContext, int fuelReqId, int currentCostId)
        {
            FuelRequestCurrentCost fuelRequestCurrentCost = new FuelRequestCurrentCost()
            {
                CreatedBy = userContext.Id,
                CreatedDate = DateTimeOffset.Now,
                UpdatedBy = userContext.Id,
                UpdatedDate = DateTimeOffset.Now,
                CurrentCostId = currentCostId,
                FuelRequestId = fuelReqId,
                IsActive = true
            };
            return fuelRequestCurrentCost;
        }

        public async Task<decimal> GetGlobalCost(UserContext userContext, int tfxFuelTypeId, int JobStateId,UoM uom, Currency currency)
        {
            decimal response = 0;
            var globalcost = await GetFuelCostForFuelRequest(userContext.CompanyId, tfxFuelTypeId, JobStateId, uom,currency);
            response = globalcost ?? 0;
            return response;
        }

        public bool IsSupplierCostExistForFuelType(UserContext userContext, int fuelTypeId, string stateId, int CountryId, Currency currency ,UoM uom)
        {
            var response = false;
            var stateIds = new List<int?>();
            try
            {
                stateIds = JsonConvert.DeserializeObject<List<int?>>(stateId);
                var globalFuelCosts = Context.DataContext.CurrentCosts.Where(t => t.IsActive && t.FuelTypeId == fuelTypeId
                                                                        && t.SupplierCompanyId == userContext.CompanyId
                                                                        && t.IsGlobleCost && t.Currency == currency
                                                                        && t.UoM == uom);
                if (!stateIds.Any())
                {
                    stateIds.Add(null);
                }
                if (globalFuelCosts.Any())
                {
                    //var fuelcostwithstates = Context.DataContext.StateCurrentCosts.Any(t => globalFuelCosts.Any(t1 => t1.Id == t.CurrentCostId) && t.IsActive && (stateIds.Contains(t.StateId) || t.StateId == null) && t.CurrentCost.UoM == uom);
                    if (stateIds.FirstOrDefault() == null)
                    {
                       var isStatesAlreadyAssigned = Context.DataContext.StateCurrentCosts.Any(t => globalFuelCosts.Any(t1 => t1.Id == t.CurrentCostId) && t.IsActive && ( (t.StateId.HasValue && t.StateId.Value >0) || t.StateId == null) && t.CurrentCost.UoM == uom);
                        if (isStatesAlreadyAssigned) { response = true; }
                    }
                    else
                    {
                        var fuelcostwithstates = Context.DataContext.StateCurrentCosts.Any(t => globalFuelCosts.Any(t1 => t1.Id == t.CurrentCostId) && t.IsActive && (stateIds.Contains(t.StateId) || t.StateId == null) && t.CurrentCost.UoM == uom);
                        if (fuelcostwithstates)
                        { response = true; }
                    }
                                    
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "IsSupplierCostExistForFuelType", ex.Message, ex);
            }
            return response;
        }


        public async Task<CalculateFuelCostViewModel> GetCalculatedFuelCostPriceAsync(int priceDetailId)
        {
            var response = new CalculateFuelCostViewModel();
            try
            {
                var pricingService = new PricingServiceDomain(this);
                var pricingResponse = await pricingService.GetPricingRequestDetailByIdAsync(new PricingDetailRequestViewModel { Id = priceDetailId });
                if (pricingResponse != null && pricingResponse.Status == Status.Success)
                {
                    response.FuelCost = pricingResponse.PricingRequestDetail.SupplierCost ?? 0;
                    var helperdomain = new HelperDomain(this);
                    response.CalculatedFuelCost = helperdomain.GetCalculatedPricePerGallon(pricingResponse.PricingRequestDetail.SupplierCost ?? 0, pricingResponse.PricingRequestDetail.PricePerGallon, pricingResponse.PricingRequestDetail.RackAvgTypeId ?? 0);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "GetCalculatedFuelCostPriceAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<decimal> GetCurrenSupplierCost(int priceRequestDetailId, decimal supplierCost)
        {
            decimal response = 0M;
            try
            {
                //var fuelRequest = await Context.DataContext.FuelRequests.SingleOrDefaultAsync(t => t.Id == fuelRequestId);
                if (priceRequestDetailId > 0)
                {
                    var pricingServiceDomain = new PricingServiceDomain(this);
                    var priceRequestDetail = await pricingServiceDomain.GetPricingRequestDetailByIdAsync(new PricingDetailRequestViewModel { Id = priceRequestDetailId });
                    response = priceRequestDetail.PricingRequestDetail.SupplierCost ?? 0;
                    var helperDomain = new HelperDomain(this);
                    response = helperDomain.GetCalculatedPricePerGallon(supplierCost, priceRequestDetail.PricingRequestDetail.PricePerGallon, priceRequestDetail.PricingRequestDetail.RackAvgTypeId ?? 0);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CurrentCostDomain", "GetCurrenSupplierCost", ex.Message, ex);
            }
            return response;
        }
    }
}

