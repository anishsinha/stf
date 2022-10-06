using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OfferMapper
    {
        #region ToViewModel
        public static OfferViewModel ToOfferViewModel(this OfferPricing entity, OfferViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new OfferViewModel();
            }
            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.OfferTypeId = entity.OfferTypeId;
            viewModel.UserId = entity.CreatedBy;
            viewModel.CompanyId = entity.SupplierCompanyId;
            viewModel.Guid = entity.OfferChainId;
            viewModel.StatusId = entity.StatusId;
            viewModel.CountryId = entity.CountryId;
            
            //pricing
            viewModel.FuelPricing = entity.ToFuelPricingViewModel();
            viewModel.FuelPricing.FuelPricingDetails.TruckLoadTypes = entity.TruckLoadType;
            viewModel.FuelPricing.FuelPricingDetails.PricingSourceId = (int)entity.PricingSource;

            //tier pricing
            if (entity.PricingTypeId == (int)PricingType.Tier)
            {
                viewModel.FuelPricing.DifferentFuelPrices = entity.DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
            }

            //locationviewmodel
            ToOfferLocationViewModel(viewModel, entity);

            //fees
            var feesViewModels = entity.FuelFees.ToFeesViewModel();
            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = feesViewModels;
            if (feesViewModels.Count > 0)
            {
                var firstFee = feesViewModels.First();
                viewModel.FuelDeliveryDetails.FuelFees.Currency = firstFee.Currency;
                viewModel.FuelDeliveryDetails.FuelFees.UoM = firstFee.UoM;
            }

            //stats
            viewModel.OfferStats = ToStatsViewModel(entity, viewModel.OfferStats);

            
            viewModel.FuelDeliveryDetails.FuelFees.Currency = entity.Currency;
            viewModel.FuelDeliveryDetails.FuelFees.UoM = entity.CountryId == (int)Country.USA ? UoM.Gallons : UoM.Litres;
            viewModel.FuelPricing.Currency = entity.Currency;
            viewModel.FuelPricing.IsTierPricingRequired = true;

            return viewModel;
        }

        public static OfferPricingDetailsViewModel ToViewModel(this OfferPricing entity, OfferPricingDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new OfferPricingDetailsViewModel();
            }

            viewModel.OfferViewModel = entity.ToOfferViewModel();
            viewModel.OfferPricingId = entity.Id;
            viewModel.OfferType = Enum.GetName(typeof(OfferType), entity.OfferTypeId);
            viewModel.OfferNumber = entity.Name;
            viewModel.SupplierCompanyId = entity.SupplierCompanyId;
            
            //fuel types
            viewModel.FuelType = new List<string>();
            viewModel.FuelType.Add(entity.MstTfxProduct.Name);

            //states
            viewModel.DisplayStates = entity.OfferPricingItems.Where(t => t.StateId.HasValue).Select(t => t.MstState.Name).Distinct().ToList();

            //cities
            viewModel.OfferViewModel.Cities = string.Join(",", string.Join(",", entity.OfferPricingItems.Where(t => t.CityId.HasValue).Select(t => t.MstCity.Name).Distinct().ToArray()));

            //zips
            viewModel.OfferViewModel.ZipCodes = string.Join(",", string.Join(",", entity.OfferPricingItems.Where(t => !string.IsNullOrWhiteSpace(t.ZipCode)).Select(t => t.ZipCode).ToArray()));
            viewModel.IsActive = entity.IsActive;

            viewModel.StatusCode = Status.Success;
            return viewModel;
        }

        private static List<OfferStatsViewModel> ToStatsViewModel(OfferPricing entity, List<OfferStatsViewModel> viewModels)
        {
            var offerStats = entity.OfferBuyerStatuses.Select(t => new
            {
                CustomerName = t.Company.Name,
                Action = t.MstBuyerOfferStatus.Name,
                ActionDate = t.CreatedDate,
                t.Reason
            });

            foreach (var item in offerStats)
            {
                viewModels.Add(new OfferStatsViewModel()
                {
                    Action = item.Action,
                    ActionDateTime = item.ActionDate,
                    CustomeName = item.CustomerName,
                    Reason = item.Reason
                });
            }

            return viewModels;
        }

        public static FuelPricingViewModel ToFuelPricingViewModel(this OfferPricing entity, FuelPricingViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new FuelPricingViewModel();
            }

            viewModel.PricingTypeId = entity.PricingTypeId;
            viewModel.RackAvgTypeId = entity.RackAvgTypeId;
            viewModel.CityGroupTerminalId = viewModel.CityGroupTerminalId == null ? entity.CityGroupTerminalId : viewModel.CityGroupTerminalId;

            if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            }
            else if (entity.PricingTypeId == (int)PricingType.RackAverage)
            {
                viewModel.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
            }
            else if (entity.PricingTypeId == (int)PricingType.RackHigh)
            {
                viewModel.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                viewModel.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
            }
            else if (entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                viewModel.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
            }
            else if (entity.PricingTypeId == (int)PricingType.Suppliercost)
            {
                //viewModel.PricePerGallon = entity.PricePerGallon;
                viewModel.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
                viewModel.SupplierCost = entity.SupplierCost;
                viewModel.SupplierCostTypeId = entity.SupplierCostTypeId;
                viewModel.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
            }

            if (entity.PricingTypeId == (int)PricingType.RackAverage
                || entity.PricingTypeId == (int)PricingType.RackHigh
                || entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
            }

            viewModel.TerminalId = entity.TerminalId;
            viewModel.ExchangeRate = entity.ExchangeRate;
            viewModel.Currency = entity.Currency;

            return viewModel;
        }

        public static List<OfferPricingItem> ToPricingItemViewModel(OfferViewModel viewModel)
        {
            var response = new List<OfferPricingItem>();

            if (viewModel.OfferTypeId == (int)OfferType.Exclusive)
            {
                foreach (var tier in viewModel.Tiers)
                {
                    AddPricingItemToList(viewModel, response, tier, true);
                }

                foreach (var customer in viewModel.Customers)
                {
                    AddPricingItemToList(viewModel, response, customer, false);
                }
            }
            else
            {
                AddPricingItemToList(viewModel, response, null, false);
            }

            return response;
        }

        #endregion

        #region ToEntity
        public static OfferPricing ToOfferPricingEntity(this OfferViewModel viewModel, int FuelTypeId)
        {
            var entity = new OfferPricing();
            entity.FuelTypeId = FuelTypeId;
            entity.PricePerGallon = 0;
            entity.RackAvgTypeId = entity.SupplierCostTypeId = null;
            entity.PricingTypeId = viewModel.FuelPricing.PricingTypeId;
            entity.OfferChainId = viewModel.Guid;
            entity.Name = string.IsNullOrWhiteSpace(viewModel.Name) ? $"{ApplicationConstants.OfferNumberPrefix}" : viewModel.Name.Trim();
            entity.OfferTypeId = viewModel.OfferTypeId;
            entity.StatusId = (int)OfferStatus.Open;
            entity.SupplierCompanyId = viewModel.CompanyId;
            entity.PricingSource = (PricingSource) viewModel.FuelPricing.FuelPricingDetails.PricingSourceId;
            entity.TruckLoadType = viewModel.FuelPricing.FuelPricingDetails.TruckLoadTypes;

            if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                entity.PricePerGallon = viewModel.FuelPricing.PricePerGallon ;
            }
            else if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                entity.RackAvgTypeId = viewModel.FuelPricing.SupplierCostMarkupTypeId;
                entity.SupplierCost = viewModel.FuelPricing.SupplierCost;
                entity.SupplierCostTypeId = (int)SupplierCostTypes.GlobalCost;
                entity.PricePerGallon = viewModel.FuelPricing.SupplierCostMarkupValue ;
                entity.BaseSupplierCost = MoneyConverter.GetBaseAmount(Currency.USD, entity.PricePerGallon, viewModel.FuelPricing.ExchangeRate);
            }
            else if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                entity.PricingTypeId = (int)PricingType.Tier;
            }
            else
            {
                entity.PricePerGallon = viewModel.FuelPricing.RackPrice  ;
                entity.RackAvgTypeId = viewModel.FuelPricing.RackAvgTypeId;
                if (viewModel.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackLow)
                {
                    entity.PricingTypeId = (int)PricingType.RackLow;
                }
                else if (viewModel.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackHigh)
                {
                    entity.PricingTypeId = (int)PricingType.RackHigh;
                }
                entity.CityGroupTerminalId = viewModel.FuelPricing.CityGroupTerminalId;
            }
            entity.CountryId = viewModel.CountryId;
            entity.UoM = viewModel.FuelDeliveryDetails.FuelFees.UoM;
            entity.Currency = viewModel.FuelDeliveryDetails.FuelFees.Currency;
            entity.ExchangeRate = viewModel.FuelPricing.ExchangeRate;
            entity.BasePrice = MoneyConverter.GetBaseAmount(entity.Currency, entity.PricePerGallon, entity.ExchangeRate);

            // from offer entity
            if (viewModel.FuelPricing.PricingTypeId == (int)PricingType.Tier)
            {
                AddLastTierWithMaxQuantityNull(viewModel.FuelPricing.DifferentFuelPrices);
                viewModel.FuelPricing.DifferentFuelPrices.ForEach(t => entity.DifferentFuelPrices.Add(t.ToEntity()));

                //Added for dashboard performance to calculate totalFrValue
                var lastRecord = viewModel.FuelPricing.DifferentFuelPrices.LastOrDefault();
                if (lastRecord != null && lastRecord.PricingTypeId == (int)PricingType.PricePerGallon && lastRecord.PricePerGallon.HasValue)
                {
                    entity.CreationTimeRackPPG = lastRecord.PricePerGallon.Value;
                }
            }

            //add fees
            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(delegate (FeesViewModel t) { t.UoM = entity.UoM; t.Currency = entity.Currency; });
            var fuelFees = viewModel.FuelDeliveryDetails.FuelFees.ToEntity();
            fuelFees.ForEach(t => entity.FuelFees.Add(t));

            //add pricingitems
            var pricingItems = ToPricingItemViewModel(viewModel);
            pricingItems.ForEach(t => entity.OfferPricingItems.Add(t));
            // offer entity end

            entity.CreatedBy = viewModel.UserId;
            entity.CreatedDate = viewModel.CreatedDate;

            return entity;
        }

        #endregion

        #region PrivateMethods

        private static void AddPricingItemToList(OfferViewModel viewModel, List<OfferPricingItem> response, int? itemId, bool isItemForTier)
        {
            if (viewModel.OfferLocationTypeId == (int)OfferLocationType.State)
            {
                if (viewModel.States.Contains(0))
                {
                    response.Add(GetTierPricingItem(itemId, viewModel, isItemForTier));
                }
                else
                {
                    foreach (var state in viewModel.States)
                    {
                        if (state != 0)
                        {
                            response.Add(GetTierPricingItem(itemId, viewModel, isItemForTier, state));
                        }
                    }
                }
            }
            else
            {
                foreach (var location in viewModel.LocationViewModel)
                {
                    if (location.ZipStringList.Count > 0 && !location.ZipStringList.Contains(Resource.lblSelectAll) && !location.ZipStringList.Contains("0"))
                    {
                        foreach (var zip in location.ZipStringList)
                        {
                            if (!string.IsNullOrWhiteSpace(zip) && !zip.Equals(Resource.lblSelectAll) && !zip.Equals("0"))
                            {
                                response.Add(GetTierPricingItem(itemId, viewModel, isItemForTier, location.StateId, location.CityId, zip));
                            }
                        }
                    }
                    else
                    {
                        response.Add(GetTierPricingItem(itemId, viewModel, isItemForTier, location.StateId, location.CityId));
                    }
                }
            }
        }

        private static OfferPricingItem GetTierPricingItem(int? itemId, OfferViewModel viewModel, bool isItemForTier, int? stateId = null, int? cityId = null, string zip = null)
        {
            var pricingItem = new OfferPricingItem();
            pricingItem.CreatedBy = viewModel.UserId;
            pricingItem.CreatedDate = viewModel.CreatedDate;
            pricingItem.UpdatedBy = viewModel.UpdatedBy;
            pricingItem.UpdatedDate = DateTimeOffset.Now;
            pricingItem.IsActive = true;

            if (isItemForTier)
            {
                pricingItem.TierId = itemId;
            }
            else
            {
                pricingItem.CustomerId = itemId;
            }

            pricingItem.StateId = stateId;
            pricingItem.CityId = cityId;
            pricingItem.ZipCode = zip;

            return pricingItem;
        }


        private static void AddLastTierWithMaxQuantityNull(List<DifferentFuelPriceViewModel> differentFuelPrices)
        {
            var lastTier = differentFuelPrices.LastOrDefault();
            if (lastTier != null && lastTier.MaxQuantity != null)
            {
                var newTier = new DifferentFuelPriceViewModel()
                {
                    MinQuantity = lastTier.MaxQuantity.Value + 1,
                    MaxQuantity = null,
                    PricingTypeId = lastTier.PricingTypeId,
                    RackAvgTypeId = lastTier.RackAvgTypeId,
                    PricePerGallon = lastTier.PricePerGallon
                };
                differentFuelPrices.Add(newTier);
            }
        }

        private static void ToOfferLocationViewModel(OfferViewModel viewModel, OfferPricing offerPricing)
        {
            var allLocations = offerPricing.OfferPricingItems.Select(t => new
            {
                t.StateId,
                t.CityId,
                t.ZipCode,
                City = t.MstCity == null ? Resource.lblAll : t.MstCity.Name,
                State = t.MstState == null ? Resource.lblAll : t.MstState.Name
            }).ToList();

            if (allLocations.Any(t => t.StateId.HasValue))
            {
                if (allLocations.All(t => t.StateId.HasValue && t.CityId == null && t.ZipCode == null))
                {
                    viewModel.OfferLocationTypeId = (int)OfferLocationType.State;
                    viewModel.States = allLocations.Select(t => t.StateId.Value).ToList();
                }
                else
                {
                    viewModel.OfferLocationTypeId = (int)OfferLocationType.City;
                    foreach (var item in allLocations)
                    {
                        var prevlocation = viewModel.LocationViewModel.FirstOrDefault(x => x.StateId == item.StateId && x.CityId == item.CityId);
                        if (prevlocation == null)
                        {
                            var location = new OfferLocationViewModel()
                            {
                                StateId = item.StateId.Value,
                                CityId = item.CityId.Value,
                                City = item.City,
                                State = item.State
                            };
                            if (string.IsNullOrWhiteSpace(item.ZipCode))
                            {
                                location.ZipList.Add(new DropdownDisplayExtended() { Code = "0", Name = Resource.lblSelectAll });
                                location.ZipStringList.Add("0");
                            }
                            else
                            {
                                location.ZipList.Add(new DropdownDisplayExtended() { Code = item.ZipCode, Name = item.ZipCode });
                                location.ZipStringList.Add(item.ZipCode);
                            }
                            viewModel.LocationViewModel.Add(location);
                        }
                        else
                        {
                            prevlocation.ZipList.Add(new DropdownDisplayExtended() { Code = item.ZipCode, Name = item.ZipCode });
                            prevlocation.ZipStringList.Add(item.ZipCode);
                        }

                    }
                }
            }
            else
            {
                viewModel.OfferLocationTypeId = (int)OfferLocationType.State;
                viewModel.States.Add(0);
            }
        } 
        #endregion
    }
}
