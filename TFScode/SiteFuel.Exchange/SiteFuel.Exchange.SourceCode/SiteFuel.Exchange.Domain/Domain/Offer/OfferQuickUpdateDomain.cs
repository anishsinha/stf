using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.Model;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class OfferQuickUpdateDomain : BaseDomain
    {
        public OfferQuickUpdateDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public OfferQuickUpdateDomain(BaseDomain domain) : base(domain)
        {
        }

        #region PublicMethods

        public StatusViewModel QuickUpdate(OfferQuickUpdateViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                var offerPricings = GetListOfFilteredOffers(viewModel, userContext.CompanyId, response);
                if (offerPricings != null && offerPricings.Any())
                {
                    var offerUpdateCommand = GetOfferUpdateCommandEntity(viewModel, userContext);
                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var offer in offerPricings)
                            {
                                Context.DataContext.OfferPricings.Add(offer);
                            }

                            offerUpdateCommand.OfferPricingItems = offerPricings.SelectMany(t => t.OfferPricingItems).ToList();
                            Context.DataContext.OfferUpdateHistories.Add(offerUpdateCommand);

                            Context.DataContext.SaveChanges();
                            transaction.Commit();

                            response.StatusCode = Status.Success;
                            response.StatusMessage = $"{offerPricings.Count} offer pricing record updated!";
                        }
                        catch (Exception ex)
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = Resource.errMessageOfferEditingFailed;
                            transaction.Rollback();
                            LogManager.Logger.WriteException("OfferDomain", "QuickUpdate", ex.Message, ex);
                        }
                    }
                }
                else
                {
                    response.StatusMessage = Resource.errMessageQuickUpdateRecordsNotPresent;
                    response.StatusCode = Status.Failed;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "QuickUpdate", ex.Message, ex);
            }
            return response;
        }

        public async Task<DatatableResponse> GetQuickUpdateHistory(UserContext userContext, int countryId)
        {
            var response = new DatatableResponse();
            try
            {
                var spDomain = new StoredProcedureDomain();
                var updateHistory = await spDomain.GetQuickUpdateHistory(userContext.CompanyId, countryId);
                var gridRows = updateHistory.Select(t => t.ToHistoryModel()).ToList();
                var totalCount = updateHistory.Select(t => t.TotalCount).FirstOrDefault();
                response = new DatatableResponse()
                {
                    data = gridRows,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount
                };
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "GetQuickUpdateHistory", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> QuickUpdateUndoAsync(UserContext userContext, int updateCommandId)
        {
            var response = new StatusViewModel();
            try
            {
                var updateCommandToUndo = await Context.DataContext.OfferUpdateHistories
                                            .Where(t => t.UndoExecutedBy == null && t.Id == updateCommandId
                                            && t.UpdatedByCompanyId == userContext.CompanyId).Select(t => new
                                            {
                                                Command = t,
                                                Items = t.OfferPricingItems.Select(t1 => new
                                                {
                                                    PricingItem = t1,
                                                    t1.OfferPricing.IsOfferUpdated
                                                })
                                            }).FirstOrDefaultAsync();
                if (updateCommandToUndo != null)
                {
                    // Get items updated by this quick update command
                    var updatedItemsByThisCmd = updateCommandToUndo.Items.Where(t => t.PricingItem.IsActive
                                                && !t.IsOfferUpdated && t.PricingItem.ParentId != null)
                                                .Select(t => t.PricingItem).ToList();

                    // Get previous version of the items to perform undo by cloning items.
                    var updatedOldItemIds = updatedItemsByThisCmd.Select(t => t.ParentId).ToList();
                    var updatedOldItems = Context.DataContext.OfferPricingDetails.Where(t => updatedOldItemIds.Contains(t.Id)).ToList();
                    var undoItemsByThisCmd = updatedOldItems.Select(t => t.CloneWithId(userContext.Id)).ToList();

                    // Set who is doing undo to show in the history
                    updateCommandToUndo.Command.UndoExecutedBy = userContext.Id;
                    updateCommandToUndo.Command.UndoExecutedOn = DateTimeOffset.Now;

                    response = await SaveQuickUpdateUndoItems(updateCommandToUndo.Command, updatedItemsByThisCmd, undoItemsByThisCmd);
                    SetStatusMessageToViewModelViewModel(response, updatedItemsByThisCmd.Any());
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OfferDomain", "QuickUpdateUndoAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<OfferViewModel>> GetQuickUpdatedItemsAsync(int commandId)
        {
            var response = new List<OfferViewModel>();
            try
            {
                var updatedItems = await Context.DataContext.OfferPricingDetails.Where(t => t.UpdateCommandId == commandId)
                                    .Select(x => new
                                    {
                                        City = x.MstCity == null ? null : x.MstCity.Name,
                                        zip = x.ZipCode,
                                        State = x.MstState == null ? null : x.MstState.Name,
                                        CustomerNames = x.Company.Name,
                                        x.OfferPricing.Name,
                                        x.OfferPricing.OfferTypeId,
                                        TierNames = x.MstTierType == null ? null : x.MstTierType.Name,
                                        x.OfferPricing.Id,
                                        FuelTypeName = x.OfferPricing.MstProduct.TfxProductId.HasValue ? x.OfferPricing.MstProduct.MstTFXProduct.Name : x.OfferPricing.MstProduct.Name,
                                        FuelPricing = x.OfferPricing,
                                        x.OfferPricing.FuelFees,
                                        x.OfferPricing.DifferentFuelPrices,
                                    }).ToListAsync();

                var offerPrices = updatedItems.GroupBy(x => new { x.Id });

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
                        CreatedDate = price.FirstOrDefault().FuelPricing.CreatedDate
                    };
                    offer.FuelDeliveryDetails.FuelFees.Currency = Currency.USD; //USA currency
                    offer.FuelDeliveryDetails.FuelFees.UoM = UoM.Gallons; //USA unit of measurement
                    offer.FuelPricing.Currency = Currency.USD;
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

        #endregion

        #region PrivateMethods

        private static void SetStatusMessageToViewModelViewModel(StatusViewModel response, bool isItemsUndoSuccess)
        {
            if (response.StatusCode == Status.Success)
            {
                if (isItemsUndoSuccess)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageUndoSucess;
                }
                else
                {
                    response.StatusMessage = Resource.errMessageUndoFailed;
                }
            }
        }

        private async Task<StatusViewModel> SaveQuickUpdateUndoItems(OfferUpdateCommand updateCommandToUndo, List<OfferPricingItem> updatedItemsByThisCmd, List<OfferPricingItem> undoItemsByThisCmd)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    updatedItemsByThisCmd.ForEach(t => t.IsActive = false);
                    Context.DataContext.OfferPricingDetails.AddRange(undoItemsByThisCmd);

                    await Context.CommitAsync();
                    transaction.Commit();
                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("OfferDomain", "SaveQuickUpdateUndoItems", ex.Message, ex);
                }
            }
            return response;
        }

        private void SetUpdatedPriceForOffers(OfferPricing pricing, OfferQuickUpdateViewModel viewModel, HelperDomain helperDomain)
        {
            //Update pricing
            if (viewModel.PricingTypeId == (int)PricingType.Tier)
            {
                foreach (var tierPricing in pricing.DifferentFuelPrices)
                {
                    tierPricing.PricePerGallon = helperDomain.GetCalculatedPricePerGallon(tierPricing.PricePerGallon, viewModel.UpdateAmountBy.Value, viewModel.MathOptId);
                }
            }
            else if (viewModel.PricingTypeId > 0)
            {
                pricing.PricePerGallon = helperDomain.GetCalculatedPricePerGallon(pricing.PricePerGallon, viewModel.UpdateAmountBy.Value, viewModel.MathOptId);
                pricing.BasePrice = pricing.PricePerGallon;
            }

            //update fees
            if (pricing.FuelFees.Any() && !string.IsNullOrWhiteSpace(viewModel.FeeTypeId))
            {
                List<FuelFee> feesToUpdate = new List<FuelFee>();
                int feeTypeId = new OfferDomain(helperDomain).GetFeeTypeOrOtherFeeType(viewModel.FeeTypeId);
                if (feeTypeId > 0)
                {
                    feesToUpdate = pricing.FuelFees.Where(t => (t.FeeTypeId == feeTypeId) || (t.FeeTypeId == (int)FeeType.OtherFee && t.OtherFeeTypeId == feeTypeId)).ToList();
                }

                if (viewModel.FeeSubTypeId.HasValue && viewModel.FeeSubTypeId.Value > 0)
                {
                    feesToUpdate = feesToUpdate.Where(t => t.FeeSubTypeId == viewModel.FeeSubTypeId.Value).ToList();
                }

                if (viewModel.FeeSubTypeId.HasValue && viewModel.FeeSubTypeId.Value == (int)FeeSubType.ByQuantity)
                {
                    foreach (var feePrice in feesToUpdate)
                    {
                        foreach (var feebyQuantity in feePrice.FeeByQuantities)
                        {
                            feebyQuantity.Fee = helperDomain.GetCalculatedPricePerGallon(feebyQuantity.Fee, viewModel.UpdateAmountBy.Value, viewModel.MathOptId);
                        }
                    }
                }
                else
                {
                    foreach (var feePrice in feesToUpdate)
                    {
                        feePrice.Fee = helperDomain.GetCalculatedPricePerGallon(feePrice.Fee, viewModel.UpdateAmountBy.Value, viewModel.MathOptId);
                    }
                }
            }
        }

        public async Task<StatusViewModel> LaunchToMarketAsync(int offerPricingId, int userId, int companyId)
        {
            var response = new StatusViewModel();
            try
            {
                var updatedOfferpricings = new List<OfferPricing>();
                var finalOfferpricings = new List<OfferPricing>();

                var offerPricingItems = Context.DataContext.OfferPricingDetails
                                        .Where(t => offerPricingId == t.OfferPricingId && t.IsActive && t.OfferPricing.SupplierCompanyId == companyId)
                                        .Select(x =>
                                                new OfferQuickUpdateTempItem
                                                {
                                                    OfferPricingItem = x,
                                                    OfferPricing = x.OfferPricing,
                                                    FuelFees = x.OfferPricing.FuelFees.ToList()
                                                }).ToList();

                var isMarketOfferPricingExist = IsMarketOfferPricingExists(offerPricingId, offerPricingItems, companyId);
                if (isMarketOfferPricingExist)
                {
                    response.StatusMessage = Resource.errMessageMarketOfferAlreadyExist;
                    return response;
                }

                if (offerPricingItems != null)
                {
                    foreach (var offer in offerPricingItems)
                    {
                        offer.OfferPricingItem.IsActive = false;
                        offer.OfferPricing.IsActive = false;
                        offer.OfferPricing.IsOfferUpdated = true;
                        if (!updatedOfferpricings.Any(t => t.OfferPricingItems.Any(t1 => t1.StateId == offer.OfferPricingItem.StateId && t1.CityId == offer.OfferPricingItem.CityId && t1.ZipCode == offer.OfferPricingItem.ZipCode)))
                        {
                            var newPricingItem = offer.OfferPricingItem.Clone(userId);
                            newPricingItem.TierId = null;
                            newPricingItem.CustomerId = null;
                            newPricingItem.UpdateCommandId = null;
                            var offerPricing = UpdatePricingItem(newPricingItem, userId, offer.OfferPricing, offer.FuelFees);
                            offerPricing.OfferTypeId = (int)OfferType.Market;
                            updatedOfferpricings.Add(offerPricing);
                        }
                    }

                    var groupedPricings = updatedOfferpricings.GroupBy(x => x.Id);

                    foreach (var item in groupedPricings)
                    {
                        var offerPrices = item.ToList();
                        var newOffer = offerPrices[0].Clone(userId, new List<FuelFee>());
                        offerPrices.ForEach(x => newOffer.OfferPricingItems.Add(x.OfferPricingItems.First()));
                        finalOfferpricings.Add(newOffer);
                    }

                    Context.DataContext.OfferPricings.AddRange(finalOfferpricings);
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;

                    response.StatusMessage = Resource.successMessageOfferLaunchToMarket;
                }
                else
                {
                    response.StatusMessage = string.Format(Resource.valMessageInvalid, Resource.lblOffer);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageOfferLaunchFailed;
                LogManager.Logger.WriteException("OfferQuickUpdateDomain", "GetSupplierOfferGridAsync", ex.Message, ex);
            }
            return response;
        }

        private bool IsMarketOfferPricingExists(int offerPricingId, List<OfferQuickUpdateTempItem> offerPricingItems, int companyId)
        {
            var isExist = false;
            var existingMarketOfferPricing = Context.DataContext.OfferPricingDetails.Where(t1 => t1.OfferPricingId != offerPricingId
                                            && t1.OfferPricing.StatusId == (int)OfferStatus.Open
                                            && t1.OfferPricing.SupplierCompanyId == companyId
                                            && t1.OfferPricing.OfferTypeId == (int)OfferType.Market && t1.OfferPricing.IsActive && t1.IsActive
                                            ).ToList();

            foreach (var item in existingMarketOfferPricing)
            {
                var isPricingExist = offerPricingItems.Any(x => x.OfferPricing.FuelTypeId == item.OfferPricing.FuelTypeId
                                                                 && x.OfferPricing.IsActive && x.OfferPricingItem.IsActive
                                                                 && x.OfferPricingItem.StateId == item.StateId
                                                                 && x.OfferPricingItem.CityId == item.CityId
                                                                 && x.OfferPricingItem.ZipCode == item.ZipCode);
                if (isPricingExist)
                {
                    return true;
                }
            }
            return isExist;
        }
        private List<OfferPricing> GetListOfFilteredOffers(OfferQuickUpdateViewModel viewModel, int companyId, StatusViewModel response)
        {
            var updatedOfferpricings = new List<OfferPricing>();
            var finalOfferpricings = new List<OfferPricing>();
            HelperDomain helperDomain = new HelperDomain(this);
            if (viewModel != null && (viewModel.PricingTypeId > 0 || !string.IsNullOrWhiteSpace(viewModel.FeeTypeId)))
            {
                response.StatusCode = Status.Success;
                var offerPricings = GetOfferPricingListToUpdate(viewModel, companyId);
                if (offerPricings.Count > 0)
                {
                    foreach (var offer in offerPricings)
                    {
                        bool isCompleteMatch = IsCompleteMatch(offer.OfferPricingItem, viewModel);
                        if (isCompleteMatch)
                        {
                            offer.OfferPricingItem.IsActive = false;
                            var newPricingItem = offer.OfferPricingItem.Clone(viewModel.UserId);
                            var offerPricing = UpdatePricingItem(newPricingItem, viewModel.UserId, offer.OfferPricing, offer.FuelFees);
                            updatedOfferpricings.Add(offerPricing);
                        }
                        else
                        {
                            var newlyCreatedPricingItems = RefreshPricingItemWithUpdatedValues(offer.OfferPricingItem, viewModel, offer);
                            foreach (var item in newlyCreatedPricingItems)
                            {
                                var offerPricing = UpdatePricingItem(item, viewModel.UserId, offer.OfferPricing, offer.FuelFees);
                                updatedOfferpricings.Add(offerPricing);
                            }
                        }
                    }

                    var groupedPricings = updatedOfferpricings.GroupBy(x => x.Id);

                    foreach (var item in groupedPricings)
                    {
                        var offerPrices = item.ToList();
                        var newOffer = offerPrices[0].Clone(viewModel.UserId, new List<FuelFee>());

                        offerPrices.ForEach(x => newOffer.OfferPricingItems.Add(x.OfferPricingItems.First()));

                        SetUpdatedPriceForOffers(newOffer, viewModel, helperDomain);

                        finalOfferpricings.Add(newOffer);
                    }
                }
                else
                    response.StatusMessage = string.Format(Resource.SuccessMsgNoOfRecordsUpdatedInPricingTable, offerPricings.Count);

            }
            return finalOfferpricings;
        }

        private List<OfferPricingItem> RefreshPricingItemWithUpdatedValues(OfferPricingItem newPricingItem, OfferQuickUpdateViewModel viewModel, OfferQuickUpdateTempItem updateTempItem)
        {
            var res = new List<OfferPricingItem>();

            if (viewModel.OfferTypeId == (int)OfferType.Exclusive)
            {
                foreach (var item in viewModel.Tiers)
                {
                    ApplyUpdatesForEachStateCityAndZips(newPricingItem, viewModel, res, null, updateTempItem);
                }

                foreach (var custId in viewModel.Customers)
                {
                    if (custId == newPricingItem.CustomerId || !newPricingItem.CustomerId.HasValue)
                    {
                        newPricingItem.TierId = null; // updating customer where tier already exists in prcing item
                        ApplyUpdatesForEachStateCityAndZips(newPricingItem, viewModel, res, custId, updateTempItem);
                    }
                }
            }
            else if (viewModel.OfferTypeId == (int)OfferType.Market)
            {
                ApplyUpdatesForEachStateCityAndZips(newPricingItem, viewModel, res, null, updateTempItem);
            }

            return res;
        }

        private static void ApplyUpdatesForEachStateCityAndZips(OfferPricingItem newPricingItem, OfferQuickUpdateViewModel viewModel, List<OfferPricingItem> res, int? custId, OfferQuickUpdateTempItem updateTempItem)
        {

            if (viewModel.ZipStringList.Count > 0)
                foreach (var zip in viewModel.ZipStringList)
                {
                    var newCustomerPricingItem = newPricingItem.Clone(viewModel.UserId);
                    newCustomerPricingItem.CustomerId = custId;
                    newCustomerPricingItem.StateId = viewModel.States[0];
                    newCustomerPricingItem.CityId = viewModel.Cities[0];
                    newCustomerPricingItem.ZipCode = zip;
                    res.Add(newCustomerPricingItem);
                }

            // When no zip are provided but multiple cities
            else if (viewModel.Cities.Count > 0)
            {
                foreach (var city in viewModel.Cities)
                {
                    var newCustomerPricingItem = newPricingItem.Clone(viewModel.UserId);
                    newCustomerPricingItem.CustomerId = custId;
                    newCustomerPricingItem.StateId = viewModel.States[0];
                    newCustomerPricingItem.CityId = city; // Setting state in case of all cities or same city
                    res.Add(newCustomerPricingItem);
                }
            }
            else if (viewModel.States.Count > 0)
            {
                foreach (var state in viewModel.States)
                {
                    var newCustomerPricingItem = newPricingItem.Clone(viewModel.UserId);
                    newCustomerPricingItem.CustomerId = custId;
                    newCustomerPricingItem.StateId = state == 0 ? default(int?) : state; // Setting state in case of all states or same state
                    res.Add(newCustomerPricingItem);
                }
            }
            else
            {
                var newCustomerPricingItem = newPricingItem.Clone(viewModel.UserId);
                newCustomerPricingItem.CustomerId = custId;
                //newCustomerPricingItem.StateId = state == 0 ? default(int?) : state; // Setting state in case of all states or same state
                res.Add(newCustomerPricingItem);
            }
        }

        private OfferPricing UpdatePricingItem(OfferPricingItem pricingItem, int userId, OfferPricing offerPricing, List<FuelFee> fuelFee)
        {
            pricingItem.Id = 0;
            var pricing = offerPricing.Clone(userId, fuelFee);
            pricing.Id = offerPricing.Id; // Keep the Id for re-grouping later on
            pricing.OfferPricingItems.Add(pricingItem);

            return pricing;
        }

        private bool IsCompleteMatch(OfferPricingItem offerPricingItem, OfferQuickUpdateViewModel viewModel)
        {
            var tierMatched = (offerPricingItem.TierId.HasValue && viewModel.Tiers.Contains(offerPricingItem.TierId.Value) || offerPricingItem.TierId == null && viewModel.Tiers.Count == 0);

            var custMatched = (offerPricingItem.CustomerId.HasValue && viewModel.Customers.Contains(offerPricingItem.CustomerId.Value) || offerPricingItem.CustomerId == null && viewModel.Customers.Count == 0);
            var stateMatched = (offerPricingItem.StateId.HasValue && viewModel.States.Contains(offerPricingItem.StateId.Value) ||
               viewModel.States.Count == 0 || viewModel.States[0] == 0);
            var cityMatched = (offerPricingItem.CityId.HasValue && viewModel.Cities.Contains(offerPricingItem.CityId.Value) || viewModel.Cities.Count == 0);
            var zipMatched = (!string.IsNullOrEmpty(offerPricingItem.ZipCode) && viewModel.ZipStringList.Contains(offerPricingItem.ZipCode) || viewModel.ZipStringList.Count == 0);

            return (tierMatched || custMatched) && stateMatched && cityMatched && zipMatched;
        }

        private List<OfferQuickUpdateTempItem> GetOfferPricingListToUpdate(OfferQuickUpdateViewModel viewModel, int companyId)
        {
            RequestCleanup(viewModel);
            int feeTypeId = 0;
            if (!string.IsNullOrEmpty(viewModel.FeeTypeId))
                feeTypeId = new OfferDomain(this).GetFeeTypeOrOtherFeeType(viewModel.FeeTypeId);

            var pricingitems = Context.DataContext.OfferPricingDetails.Where(x => x.IsActive &&
             x.OfferPricing.IsActive
             &&
             x.OfferPricing.TruckLoadType == viewModel.TruckLoadType
             &&
             x.OfferPricing.PricingSource == viewModel.PricingSource
             &&
             x.OfferPricing.SupplierCompanyId == companyId
             &&
             x.OfferPricing.FuelTypeId == viewModel.FuelTypeId
             &&
             x.OfferPricing.OfferTypeId == viewModel.OfferTypeId
             &&
            (    // Pricing type match
                 viewModel.PricingTypeId > 0 && x.OfferPricing.PricingTypeId == viewModel.PricingTypeId
                 || // Fee Type match
                  ((viewModel.FeeTypeId != null && viewModel.FeeTypeId != "") &&
                     (viewModel.FeeSubTypeId.HasValue && viewModel.FeeSubTypeId.Value > 0
                     &&
                     x.OfferPricing.FuelFees.Any(f => (f.FeeTypeId == feeTypeId || (f.FeeTypeId == (int)FeeType.OtherFee && f.OtherFeeTypeId == feeTypeId)) && f.FeeSubTypeId == viewModel.FeeSubTypeId.Value)
                     ||
                     x.OfferPricing.FuelFees.Any(f => (f.FeeTypeId == feeTypeId || (f.FeeTypeId == (int)FeeType.OtherFee && f.OtherFeeTypeId == feeTypeId)) && !viewModel.FeeSubTypeId.HasValue)
                     )
                  )

             )
             &&
              // Customer/Tier
              (viewModel.Tiers.Count == 0 && viewModel.Customers.Count == 0 ||
               x.TierId.HasValue && viewModel.Tiers.Contains(x.TierId.Value)
               ||
               (
                viewModel.Tiers.Count == 0 && viewModel.Customers.Count == 0 ||
                x.CustomerId.HasValue && viewModel.Customers.Contains(x.CustomerId.Value)
               ))
              );

            var pricingItemDetails = pricingitems.Select(x =>
            new OfferQuickUpdateTempItem
            {
                OfferPricingItem = x,
                OfferPricing = x.OfferPricing,
                FuelFees = x.OfferPricing.FuelFees.ToList(),
                CustomerTierId = x.CustomerId.HasValue ? x.Company.OfferTierMappings.FirstOrDefault(t => t.SupplierCompanyId == companyId && t.BuyerCompanyId == x.CustomerId).TierId : (int?)null
            }
            ).ToList();
            return RemovePricingItemAsPerPriority(pricingItemDetails, viewModel);
        }

        private List<OfferQuickUpdateTempItem> RemovePricingItemAsPerPriority(List<OfferQuickUpdateTempItem> pricingItemDetails, OfferQuickUpdateViewModel viewModel)
        {
            List<OfferQuickUpdateTempItem> morePriorityUpdateItems = new List<OfferQuickUpdateTempItem>();

            if (viewModel.States[0] == 0)
                return pricingItemDetails; // Search by all states

            else if (viewModel.Cities.Count == 0 && viewModel.States.Count > 0) // Search by all cities
            {
                morePriorityUpdateItems = pricingItemDetails.Where(x => x.OfferPricingItem.StateId.HasValue &&
                viewModel.States.Contains(x.OfferPricingItem.StateId.Value)).ToList();

                if (morePriorityUpdateItems.Count == 0 || morePriorityUpdateItems.All(x => x.OfferPricingItem.CityId.HasValue))
                    morePriorityUpdateItems.AddRange(pricingItemDetails.Where(x => x.OfferPricingItem.StateId == null));
                return morePriorityUpdateItems;
            }
            else if (viewModel.Cities.Count > 0 && viewModel.ZipStringList.Count == 0) // Search by all zip but match City and state
            {
                morePriorityUpdateItems = pricingItemDetails.Where(x => x.OfferPricingItem.CityId.HasValue
                && viewModel.Cities.Contains(x.OfferPricingItem.CityId.Value)).ToList();

                if (morePriorityUpdateItems.All(x => x.OfferPricingItem.ZipCode != null)) // Check if record for whole city already exists in morePriorityUpdateItems
                {
                    if (morePriorityUpdateItems.All(x => x.OfferPricingItem.CityId != null))

                        morePriorityUpdateItems.AddRange(pricingItemDetails.Where(x => x.OfferPricingItem.CityId == null
                        && x.OfferPricingItem.StateId.HasValue &&
                        viewModel.States.Contains(x.OfferPricingItem.StateId.Value))); // Add whole state in the list if updated by city but no zip
                }

                if (morePriorityUpdateItems.Count > 0)
                    return morePriorityUpdateItems;

                // Only Match state
                morePriorityUpdateItems = pricingItemDetails.Where(x => x.OfferPricingItem.StateId.HasValue &&
                viewModel.States.Contains(x.OfferPricingItem.StateId.Value) && x.OfferPricingItem.CityId == null).ToList();

                if (morePriorityUpdateItems.Count > 0)
                    return morePriorityUpdateItems;
                else // All states
                    return pricingItemDetails.Where(x => x.OfferPricingItem.StateId == null).ToList();

            }
            else if (viewModel.ZipStringList.Count > 0)
            {
                morePriorityUpdateItems = pricingItemDetails.Where(x => viewModel.ZipStringList.Contains(x.OfferPricingItem.ZipCode)
                && x.OfferPricingItem.StateId.HasValue
                && viewModel.States.Contains(x.OfferPricingItem.StateId.Value) && x.OfferPricingItem.CityId.HasValue
                && viewModel.Cities.Contains(x.OfferPricingItem.CityId.Value)).ToList();
                if (morePriorityUpdateItems.Count > 0)
                    return morePriorityUpdateItems;

                morePriorityUpdateItems = pricingItemDetails.Where(x => x.OfferPricingItem.StateId.HasValue
                && viewModel.States.Contains(x.OfferPricingItem.StateId.Value) && x.OfferPricingItem.CityId.HasValue
                && viewModel.Cities.Contains(x.OfferPricingItem.CityId.Value) && x.OfferPricingItem.ZipCode == null).ToList();

                if (morePriorityUpdateItems.Count > 0)
                    return morePriorityUpdateItems;
                else
                {
                    morePriorityUpdateItems = pricingItemDetails.Where(x => x.OfferPricingItem.StateId.HasValue
                    && viewModel.States.Contains(x.OfferPricingItem.StateId.Value) && x.OfferPricingItem.CityId == null).ToList();
                    if (morePriorityUpdateItems.Count > 0)
                        return morePriorityUpdateItems;
                    else
                        return pricingItemDetails.Where(x => x.OfferPricingItem.StateId == null).ToList();
                }
            }
            return morePriorityUpdateItems;
        }

        private static void RequestCleanup(OfferQuickUpdateViewModel viewModel)
        {
            if (viewModel.OfferTypeId == (int)OfferType.Market)
            {
                viewModel.Tiers.Clear();
                viewModel.Customers.Clear();
            }

            viewModel.Cities.Remove(0);
            viewModel.ZipStringList.Remove("0");
        }

        private static OfferUpdateCommand GetOfferUpdateCommandEntity(OfferQuickUpdateViewModel viewModel, UserContext userContext)
        {
            var commandJson = JsonConvert.SerializeObject(viewModel);
            var command = new OfferUpdateCommand
            {
                UpdateType = viewModel.QuickUpdatePreferenceSetting.QuickUpdateType,
                MathOperationId = viewModel.MathOptId,
                UpdatedAmount = viewModel.UpdateAmountBy ?? 0,
                UpdatedBy = userContext.Id,
                UpdatedDate = DateTimeOffset.Now,
                CommandJson = commandJson,
                IsActive = true,
                UpdatedByCompanyId = userContext.CompanyId,
                OfferTypeId = viewModel.OfferTypeId,
                Tiers = viewModel.TierNames,
                Customers = viewModel.CustomerNames,
                FuelTypeId = viewModel.FuelTypeId,
                States = viewModel.StateNames,
                Cities = viewModel.CityNames,
                ZipCodes = string.Join(", ", viewModel.ZipStringList),
                PriceTypeId = viewModel.PricingTypeId > 0 ? viewModel.PricingTypeId : (int?)null,
                FeeTypeName = viewModel.FeeTypeName,
                FeeSubTypeId = viewModel.FeeSubTypeId
            };
            return command;
        }

        #endregion
    }
}
