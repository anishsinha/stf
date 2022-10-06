using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Offer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FuelRequestMapper
    {
        public static FuelRequest ToEntity(this FuelRequestViewModel viewModel, FuelRequest entity = null)
        {
            if (entity == null)
                entity = new FuelRequest();

            entity.ProductDisplayGroupId = viewModel.FuelDetails.FuelDisplayGroupId;
            if (entity.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                entity.FuelDescription = viewModel.FuelDetails.NonStandardFuelDescription;
               // entity.FuelTypeId = viewModel.FuelDetails.FuelTypeId.Value;
            }
            //else if(!viewModel.FuelDetails.IsTierPricing)
            //{
            //    FuelRequestDomain fuelRequest = new FuelRequestDomain();
            //    entity.FuelTypeId = fuelRequest.GetFuelTypeId(viewModel.FuelDetails.FuelTypeId.Value, viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingSourceId, viewModel.FuelDetails.FuelPricing.PricingTypeId);
            //}

            entity.QuantityTypeId = viewModel.FuelDetails.FuelQuantity.QuantityTypeId;

            if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                entity.MaxQuantity = viewModel.FuelDetails.FuelQuantity.Quantity;
            }
            else if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.Range)
            {
                entity.MinQuantity = viewModel.FuelDetails.FuelQuantity.MinimumQuantity;
                entity.MaxQuantity = viewModel.FuelDetails.FuelQuantity.MaximumQuantity;
            }
            else if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
            {
                entity.MaxQuantity = ApplicationConstants.QuantityNotSpecified;
            }
            entity.EstimateGallonsPerDelivery = viewModel.FuelDetails.FuelQuantity.EstimatedGallonsPerDelivery.HasValue ? viewModel.FuelDetails.FuelQuantity.EstimatedGallonsPerDelivery.Value : 0;

            bool isMarketPricing = false;
            entity.Id = viewModel.Id;
            entity.PricingTypeId = viewModel.FuelDetails.FuelPricing.PricingTypeId;

            if (viewModel.FuelDetails.FuelPricing.PricingTypeId != (int)PricingType.Suppliercost && viewModel.FuelDetails.FuelPricing.PricingTypeId != (int)PricingType.PricePerGallon)
            {
                isMarketPricing = true;
                if (viewModel.FuelDetails.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackLow)
                {
                    entity.PricingTypeId = (int)PricingType.RackLow;
                }
                else if (viewModel.FuelDetails.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackHigh)
                {
                    entity.PricingTypeId = (int)PricingType.RackHigh;
                }
                entity.CityGroupTerminalId = viewModel.FuelDetails.FuelPricing.CityGroupTerminalId;
            }
            entity.UoM = viewModel.FuelDetails.FuelQuantity.UoM;
            entity.Currency = viewModel.FuelDetails.FuelPricing.Currency;
            entity.ExchangeRate = viewModel.FuelDetails.FuelPricing.ExchangeRate;
            entity.IsOverageAllowed = viewModel.FuelDetails.IsOverageAllowed;
            entity.OverageAllowedAmount = viewModel.FuelDetails.OverageAllowedPercent;
            entity.PaymentTermId = viewModel.FuelOfferDetails.PaymentTermId;
            entity.NetDays = viewModel.FuelOfferDetails.NetDays;
            entity.OrderTypeId = viewModel.FuelDetails.OrderTypeId;
            entity.IsPublicRequest = viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest;
            if (entity.FuelRequestXStatuses.Count > 0)
            {
                entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
            }
            FuelRequestXStatus fuelRequestStatus = new FuelRequestXStatus();
            fuelRequestStatus.StatusId = viewModel.FuelDetails.StatusId;
            fuelRequestStatus.IsActive = true;
            fuelRequestStatus.UpdatedBy = viewModel.UpdatedBy;
            fuelRequestStatus.UpdatedDate = DateTimeOffset.Now;
            entity.FuelRequestXStatuses.Add(fuelRequestStatus);

            entity.FuelRequestPricingDetail = viewModel.FuelDetails.FuelPricing.FuelPricingDetails.ToPricingDetailsEntity(isMarketPricing, entity.FuelRequestPricingDetail);

            if (viewModel.FuelDeliveryDetails.TruckLoadTypes == TruckLoadTypes.FullTruckLoad)
            {
                entity.FreightOnBoardTypeId = (int)viewModel.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes;
            }

            if (viewModel.Id > 0)
            {
                entity.UpdatedBy = viewModel.FuelDetails.UpdatedBy;
                entity.UpdatedDate = DateTimeOffset.Now;
            }
            else
            {
                entity.CreatedBy = viewModel.FuelDetails.CreatedBy;
                entity.CreatedDate = viewModel.FuelDetails.CreatedDate;
            }
            entity.ExpirationDate = viewModel.FuelDeliveryDetails.ExpirationDate;
            entity.OrderClosingThreshold = viewModel.FuelOfferDetails.OrderClosingThreshold;
            entity.ExternalPoNumber = viewModel.ExternalPoNumber;
            entity.IsActive = viewModel.FuelDetails.IsActive;
            entity.UpdatedBy = viewModel.FuelDetails.UpdatedBy;
            entity.UpdatedDate = viewModel.FuelDetails.UpdatedDate;
            entity.Currency = viewModel.FuelDetails.FuelPricing.Currency;
            entity.UoM = viewModel.FuelDetails.FuelQuantity.UoM;

            return entity;
        }

        public static FuelRequestViewModel ToViewModel(this FuelRequest entity, FuelRequestViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestViewModel(Status.Success);

            viewModel.FuelDetails = entity.ToFuelDetailsViewModel();
            viewModel.FuelRequestTypeId = entity.FuelRequestTypeId;
            viewModel.Id = entity.Id;
            viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest = entity.IsPublicRequest;
            var supplierList = entity.PrivateSupplierLists.FirstOrDefault();
            if (!entity.IsPublicRequest && supplierList != null)
            {
                var supplierIds = entity.PrivateSupplierLists.Select(t => t.Id).ToList();
                viewModel.FuelOfferDetails.PrivateSupplierList.PrivateSupplierIds = supplierIds;
                viewModel.FuelOfferDetails.PrivateSupplierList.Id = entity.PrivateSupplierLists.FirstOrDefault().Id;
            }

            viewModel.FuelDeliveryDetails = entity.FuelRequestDetail.ToViewModel();
            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = entity.FuelRequestFees.ToFeesViewModel();
            viewModel.FuelDeliveryDetails.FuelFees.Currency = entity.Currency;
            viewModel.FuelDeliveryDetails.FuelFees.UoM = entity.UoM;

            if (viewModel.FuelDetails.IsFTLEnabled)
            {
                viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(t => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
            }

            viewModel.FuelDeliveryDetails.SpecialInstructions = entity.SpecialInstructions.Select(t => t.ToViewModel()).ToList();
            viewModel.FuelDeliveryDetails.DeliverySchedules = entity.DeliverySchedules.GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() }).Select(t => t.Items.ToViewModel()).ToList();

            var paymentDiscount = entity.PaymentDiscounts.FirstOrDefault();
            if (paymentDiscount == null)
            {
                paymentDiscount = new PaymentDiscount();
            }
            viewModel.FuelOfferDetails.PaymentDiscount = paymentDiscount.ToViewModel();

            if (entity.MstSupplierQualifications.Any())
            {
                viewModel.FuelOfferDetails.SupplierQualifications = entity.MstSupplierQualifications.Select(t => t.Id).ToList();
            }

            viewModel.FuelOfferDetails.PaymentTermId = entity.PaymentTermId;
            viewModel.FuelOfferDetails.PaymentTermName = entity.MstPaymentTerm.Name;
            viewModel.FuelOfferDetails.PaymentMethod = entity.FuelRequestDetail.PaymentMethod;
            viewModel.FuelOfferDetails.NetDays = entity.NetDays;
            viewModel.FuelDeliveryDetails.ExpirationDate = entity.ExpirationDate;
            viewModel.FuelOfferDetails.OrderClosingThreshold = entity.OrderClosingThreshold;

            viewModel.ExternalPoNumber = entity.ExternalPoNumber;
            viewModel.RequestNumber = entity.RequestNumber;

            //added condition for Broker FR
            var job = entity.Job;
            if (job.IsMarine)
            {
                var jobxAsset = job.JobXAssets.Where(w => w.FuelRequestId == viewModel.Id).FirstOrDefault();
                if(jobxAsset != null)
                {
                    viewModel.Job.VessleId = jobxAsset.AssetId;
                    viewModel.Job.IMONumber = jobxAsset.Asset.AssetAdditionalDetail.IMONumber;
                    viewModel.Job.Flag = jobxAsset.Asset.AssetAdditionalDetail.Flag;
                }
            }
            viewModel.CompanyId = entity.GetCompany().Id;
            viewModel.Job.JobId = job.Id;
            viewModel.Job.Name = job.Name;
            viewModel.Job.Address = job.Address;
            viewModel.Job.City = job.City;
            viewModel.Job.Country.Currency = job.Currency;
            viewModel.Job.Country.Id = job.CountryId;
            viewModel.JobCountryId = job.CountryId;
            viewModel.Job.State.Code = job.MstState.Code;
            viewModel.Job.State.Id = job.StateId;
            viewModel.Job.Country.Code = job.MstCountry.Code;
            viewModel.Job.State.QuantityIndicatorTypeId = job.MstState.QuantityIndicatorTypeId;
            viewModel.Job.ZipCode = job.ZipCode;
            viewModel.Job.JobStartDate = Convert.ToString(job.StartDate.Date);
            viewModel.Job.JobEndDate = job.EndDate != null ? Convert.ToString(job.EndDate.Value.Date) : string.Empty;
            viewModel.Job.IsTaxExempted = job.JobBudget.IsTaxExempted;
            viewModel.Job.IsResaleEnabled = job.IsResaleEnabled;
            viewModel.Job.TimeZoneName = job.TimeZoneName;
            viewModel.Job.IsProFormaPoEnabled = job.IsProFormaPoEnabled;
            viewModel.Job.IsRetailJob = job.IsRetailJob;
            viewModel.Job.IsMarineLocation = job.IsMarine;

            var jobStatus = job.JobXStatuses.SingleOrDefault(t => t.IsActive);
            if (jobStatus != null)
                viewModel.Job.StatusId = jobStatus.StatusId;

            viewModel.Job.ContractNumber = job.ContractNumber;
            var resaleCustomer = job.ResaleCustomers.FirstOrDefault();
            if (resaleCustomer != null)
            {
                viewModel.Job.CustomerEmail = resaleCustomer.Email;
                viewModel.Job.CustomerName = resaleCustomer.Name;
            }

            var resale = entity.Resales.FirstOrDefault();
            if (resale != null)
            {
                viewModel.FuelRequestResale.FuelPricing.PricingTypeId = resale.PricingTypeId;
                viewModel.FuelRequestResale.FuelPricing.RackAvgTypeId = resale.RackAvgTypeId;

                if (resale.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    viewModel.FuelRequestResale.FuelPricing.PricePerGallon = resale.PricePerGallon.GetPreciseValue(6);
                }
                else if (resale.PricingTypeId == (int)PricingType.RackAverage)
                {
                    viewModel.FuelRequestResale.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
                }
                else if (resale.PricingTypeId == (int)PricingType.RackHigh)
                {
                    viewModel.FuelRequestResale.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                    viewModel.FuelRequestResale.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
                }
                else if (resale.PricingTypeId == (int)PricingType.RackLow)
                {
                    viewModel.FuelRequestResale.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                    viewModel.FuelRequestResale.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
                }
                else if (resale.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    viewModel.FuelRequestResale.FuelPricing.SupplierCostMarkupTypeId = resale.RackAvgTypeId;
                    viewModel.FuelRequestResale.FuelPricing.SupplierCostMarkupValue = resale.PricePerGallon.GetPreciseValue(6);
                }

                if (resale.PricingTypeId == (int)PricingType.RackAverage
                || resale.PricingTypeId == (int)PricingType.RackHigh
                || resale.PricingTypeId == (int)PricingType.RackLow)
                {
                    viewModel.FuelRequestResale.FuelPricing.RackPrice = resale.PricePerGallon.GetPreciseValue(6);
                }

                viewModel.FuelRequestResale.FuelPricing.Currency = job.Currency;
                viewModel.FuelRequestResale.IsDropTicketEnabled = resale.IsDDTEnabled;
                if (entity.PricingTypeId == (int)PricingType.Tier)
                    viewModel.FuelRequestResale.DifferentFuelPrices = resale.DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();

                viewModel.FuelDeliveryDetails.FuelRequestFee.ResaleFee = entity.FuelRequestFees.Where(t => t.FeeTypeId == (int)FeeType.ResaleFee).Select(t => t.ToResaleFeeViewModel()).ToList();
                viewModel.FuelRequestResale.ResaleCustomer = entity.ResaleCustomers.Select(t => t.ToViewModel()).ToList();
                if (viewModel.FuelRequestResale.ResaleCustomer.Count == 0)
                {
                    viewModel.FuelRequestResale.ResaleCustomer.Add(new FuelRequestResaleCustomerViewModel());
                }
            }
            viewModel.FuelDetails.FuelPricing.ExchangeRate = entity.ExchangeRate;
            viewModel.FuelDetails.FuelPricing.Currency = job.Currency;
            viewModel.FuelDetails.FuelQuantity.UoM = job.IsMarine ? entity.UoM : job.UoM;
            viewModel.FuelDeliveryDetails.SiteInstructions = job.SiteInstructions;
            return viewModel;
        }

        public static FuelRequestViewModel ToFuelRequestViewModel(this UspGetSupplierInvoiceDetails entity)
        {
            var viewModel = new FuelRequestViewModel(Status.Success);

            viewModel.FuelDetails = new FuelDetailsViewModel()
            {
                FuelDisplayGroupId = entity.FuelDisplayGroupId.Value,
                OrderTypeId = entity.OrderTypeId.Value,
                FuelType = entity.FuelType,
                NonStandardFuelDescription = entity.NonStandardFuelDescription,
                FuelPricing = new FuelPricingViewModel() { PricingTypeId = entity.PricingTypeId.Value },
                FuelQuantity = new FuelQuantityViewModel()
                {
                    QuantityTypeId = entity.QuantityTypeId.Value,
                    MaximumQuantity = entity.MaxQuantity.Value.GetPreciseValue(6),
                    UoM = entity.FuelQuantityUoM,
                    Quantity = entity.MaxQuantity.Value.GetPreciseValue(6)
                }
            };

            viewModel.FuelRequestTypeId = entity.FuelRequestTypeId.Value;
            viewModel.FuelOfferDetails.PrivateSupplierList.IsPublicRequest = entity.IsPublicRequest.Value;
            viewModel.Job.IsTaxExempted = entity.IsTaxExempted.Value;
            viewModel.Job.TimeZoneName = entity.TimeZoneName;

            return viewModel;
        }

        public static FuelDetailsViewModel ToFuelDetailsViewModel(this FuelRequest entity, FuelDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelDetailsViewModel(Status.Success);

            if (entity.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType || entity.MstProduct.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                viewModel.NonStandardFuelName = entity.MstProduct.Name;
                viewModel.NonStandardFuelDescription = entity.FuelDescription;
                viewModel.FuelTypeId = entity.MstProduct.Id;
            }
            else
            {
                viewModel.FuelTypeId = entity.MstProduct.TfxProductId.HasValue ? entity.MstProduct.TfxProductId.Value : 0;
            }
            viewModel.FuelDisplayGroupId = entity.ProductDisplayGroupId == (int)ProductDisplayGroups.FuelTypesInYourArea ? entity.MstProduct.ProductDisplayGroupId : entity.ProductDisplayGroupId;
            viewModel.FuelType = ContextFactory.Current.GetDomain<HelperDomain>().GetProductName(entity.MstProduct);
            viewModel.ProductTypeId = entity.MstProduct.ProductTypeId;
            viewModel.FuelQuantity.QuantityTypeId = entity.QuantityTypeId;

            if (entity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                viewModel.FuelQuantity.Quantity = entity.MaxQuantity.GetPreciseValue(6);
            }
            else if (entity.QuantityTypeId == (int)QuantityType.Range)
            {
                viewModel.FuelQuantity.MinimumQuantity = entity.MinQuantity.GetPreciseValue(6);
                viewModel.FuelQuantity.MaximumQuantity = entity.MaxQuantity.GetPreciseValue(6);
            }
            viewModel.FuelQuantity.EstimatedGallonsPerDelivery = entity.EstimateGallonsPerDelivery;
            viewModel.FuelPricing.CityGroupTerminalId = entity.CityGroupTerminalId;
            viewModel.FuelPricing.CityGroupTerminalStateId = entity.Job.StateId;

            viewModel.FuelPricing.FuelPricingDetails = entity.FuelRequestPricingDetail.ToFuelRequestViewModel();

            viewModel.IsFTLEnabled = entity.FuelRequestDetail != null && entity.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad;
            viewModel.FreightOnBoard = entity.FreightOnBoardTypeId;
            if (entity.FuelRequestDetail != null && entity.FuelRequestDetail.PricingQuantityIndicatorTypeId != null)
            {
                viewModel.FuelQuantity.QuantityIndicatorTypes = (QuantityIndicatorTypes)entity.FuelRequestDetail.PricingQuantityIndicatorTypeId;
            }

            if (entity.CityGroupTerminalId.HasValue && entity.CityGroupTerminalId.Value > 0)
            {
                var terminal = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalTerminal(entity.CityGroupTerminalId.Value);
                if (terminal != null)
                    viewModel.FuelPricing.CityGroupTerminalName = $"{terminal.Name}, {terminal.StateCode}";
            }

            //need to get pricingSourceId, pricingtypeId from service with requestpricingdetailId
            var pricingDetails = new FuelRequestDomain().GetRequestPricingDetail(entity.FuelRequestPricingDetail.RequestPriceDetailId, entity.FuelTypeId, entity.Job.StateId, (int)entity.Currency);
            
            if(pricingDetails !=null && pricingDetails.TierPricings!=null && pricingDetails.TierPricings.Any())
            {
                if (pricingDetails.TierPricings.First().CumulationTypeId.HasValue)
                {
                    var cumulationDetails = pricingDetails.TierPricings.First();
                    viewModel.FuelPricing.TierPricing.ResetCumulationSetting.CumulationType = (CumulationType)cumulationDetails.CumulationTypeId;
                    viewModel.FuelPricing.TierPricing.ResetCumulationSetting.Date = cumulationDetails.CumulationResetDate;
                    viewModel.FuelPricing.TierPricing.ResetCumulationSetting.Day = (WeekDay)cumulationDetails.CumulationResetDay;
                    var cumulationDisplayLabel = new HelperDomain().GetDisplayCumulationFrequencyLabel(cumulationDetails.CumulationTypeId.Value, cumulationDetails.CumulationResetDate.Value, cumulationDetails.CumulationResetDay.Value);
                    viewModel.FuelPricing.TierPricing.DisplayCumulationFrequency = cumulationDisplayLabel;
                }
               
                viewModel.FuelPricing.TierPricing.Pricings = new List<PricingViewModel>();
                viewModel.FuelPricing.TierPricing.TierPricingType =(TierPricingType) pricingDetails.TierPricings.FirstOrDefault().TierTypeId;
                viewModel.TierPricing.TierPricingType = viewModel.FuelPricing.TierPricing.TierPricingType;
                var helperDomain = new HelperDomain();
                int i = 1;
               List<DropdownDisplayItem> externalTerminals = helperDomain.GetCityRackTerminalNameByIds(pricingDetails);
               
                foreach (var item in pricingDetails.TierPricings)
                {
                    var model = new PricingViewModel();                   
                    model.PricingSourceId= item.PricingSourceId;
                    model.PricingTypeId = item.PricingTypeId;
                    model.RackAvgTypeId = item.RackAvgTypeId;
                    //model.RackTypeId = item.RackTypeId;
                    model.PricingCode.Id = item.PricingCodeId;
                    model.PricingCode.Code = item.PricingCode;
                    model.PricePerGallon = item.PricePerGallon;
                    model.BasePrice = item.BasePrice;
                    model.BaseSupplierCost = item.BaseSupplierCost;
                   // model.FromQuantity = item.MinQuantity;
                    model.FromQuantity = decimal.Parse(item.MinQuantity.ToString("0.00"));
                    model.FuelTypeId = item.FuelTypeId;
                    model.CityGroupTerminalStateId = entity.Job.StateId;
                    if(item.PricingTypeId == (int)PricingType.RackAverage)
                    {
                        model.RackPrice = item.PricePerGallon;
                        model.CityGroupTerminalId = item.CityRackTerminalId;
                    }
                    else if(item.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        model.SupplierCostMarkupValue  = item.PricePerGallon;
                        model.SupplierCostMarkupTypeId = item.SupplierCostTypeId;
                    }
                    
                    if (item.MaxQuantity != 0)
                    {
                       // model.ToQuantity = item.MaxQuantity;
                        model.ToQuantity = decimal.Parse(item.MaxQuantity.ToString("0.00"));
                        model.RowIndex = i;
                    }
                    else
                    {
                        model.Quantity = item.MinQuantity;
                        model.IsAboveQuantity = true;
                        model.RowIndex = 2;
                    }
                    if (externalTerminals != null && externalTerminals.Any() && item.CityRackTerminalId != null && item.CityRackTerminalId != 0)
                        model.CityGroupTerminalName = externalTerminals.Where(w => w.Id == item.CityRackTerminalId).FirstOrDefault().Name;

                    var rackAvgTypeId = item.RackAvgTypeId.HasValue ? item.RackAvgTypeId.Value : 0;
                    var pricingTypeId = item.PricingTypeId;
                    if(item.PricingSourceId == (int)PricingSource.Axxis && item.PricingTypeId == (int)PricingType.RackAverage)
                    {
                        pricingTypeId = item.RackTypeId;
                    }
                    else if(item.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        rackAvgTypeId = item.SupplierCostTypeId ?? 0;
                    }

                    model.DisplayPrice = helperDomain.GetPricePerGallon(item.PricePerGallon, pricingTypeId, rackAvgTypeId);
                    if (rackAvgTypeId != 0)
                    {
                        model.DisplayPrice = $"{model.DisplayPrice},{Enum.GetName(typeof(PricingSource), item.PricingSourceId)}";
                    }
                    model.IsEdit = true;
                    viewModel.FuelPricing.TierPricing.Pricings.Add(model);
                    i++;
                    if (i == 2)
                        i = 3;
                }
                viewModel.IsTierPricing = true;
                viewModel.TierPricing.Pricings = viewModel.FuelPricing.TierPricing.Pricings;
            }
            else if (pricingDetails != null)
                {
                    viewModel.FuelPricing.FuelPricingDetails.PricingSourceId = pricingDetails.PricingSourceId;
                    viewModel.FuelPricing.PricingTypeId = pricingDetails.PricingTypeId;
                    viewModel.FuelPricing.RackAvgTypeId = pricingDetails.RackAvgTypeId;
                    viewModel.FuelPricing.MarkertBasedPricingTypeId = pricingDetails.RackTypeId;
                    viewModel.FuelPricing.FuelPricingDetails.PricingCode.Id = pricingDetails.PricingCodeId;
                }
            
         
            if (pricingDetails != null && pricingDetails.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.FuelPricing.PricePerGallon = pricingDetails.PricePerGallon.GetPreciseValue(6);
            }
            else if (pricingDetails != null && pricingDetails.PricingTypeId == (int)PricingType.Suppliercost)
            {
                viewModel.FuelPricing.SupplierCost = pricingDetails.SupplierCost;
                viewModel.FuelPricing.SupplierCostMarkupTypeId = pricingDetails.RackAvgTypeId;
                viewModel.FuelPricing.SupplierCostMarkupValue = pricingDetails.PricePerGallon.GetPreciseValue(6);
            }
            else if (pricingDetails != null && pricingDetails.PricingTypeId == (int)PricingType.RackAverage)
            {
                viewModel.FuelPricing.RackPrice = pricingDetails.PricePerGallon.GetPreciseValue(6);
            }

            viewModel.IsOverageAllowed = entity.IsOverageAllowed;
            viewModel.OverageAllowedPercent = entity.OverageAllowedAmount.GetPreciseValue(6);
            viewModel.OrderTypeId = entity.OrderTypeId;
            var frStatus = entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive);
            if (frStatus.StatusId == (int)FuelRequestStatus.CounterOfferAccepted)
            {
                viewModel.StatusId = (int)FuelRequestStatus.Accepted;
                viewModel.StatusName = FuelRequestStatus.Accepted.ToString();
            }
            else
            {
                viewModel.StatusId = frStatus.StatusId;
                viewModel.StatusName = frStatus.MstFuelRequestStatus.Name;
            }

            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDate = entity.CreatedDate;
            viewModel.TerminalId = entity.TerminalId;
            viewModel.FuelPricing.TerminalId = entity.TerminalId;
            viewModel.Comment = entity.Comment;
            //if (entity.PricingTypeId == (int)PricingType.Tier)
            //{
            //    viewModel.DifferentFuelPrices = entity.DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
            //}
            viewModel.FuelPricing.ExchangeRate = entity.ExchangeRate;
            viewModel.FuelPricing.Currency = entity.Job.Currency;
            viewModel.FuelQuantity.UoM = entity.Job.UoM;

            viewModel.FreightOnBoard = entity.FreightOnBoardTypeId;

            return viewModel;
        }

        public static FuelRequestGridViewModel ToGridViewModel(this FuelRequest entity, int userId, FuelRequestDomain fuelRequestDomain, FuelRequestGridViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelRequestGridViewModel(Status.Success);

            HelperDomain helperDomain = new HelperDomain(fuelRequestDomain);
            var job = entity.Job;

            viewModel.FuelRequestId = entity.Id;
            viewModel.JobName = job.Name;
            viewModel.JobId = job.Id;
            viewModel.Customer = entity.Job.Company.Name;
            viewModel.Address = $"{job.Address}, {job.City}, {job.MstState.Code} {job.ZipCode}";
            viewModel.GallonsNeeded = helperDomain.GetQuantityRequested(entity.MaxQuantity);
            viewModel.PricePerGallon = ContextFactory.Current.GetDomain<HelperDomain>().GetPricePerGallon(entity);
            var status = entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive);
            viewModel.StatusId = status.StatusId;
            if (status.MstFuelRequestStatus != null)
            {
                viewModel.Status = status.MstFuelRequestStatus.Name;
            }
            viewModel.FuelType = ContextFactory.Current.GetDomain<HelperDomain>().GetProductName(entity.MstProduct);
            viewModel.RequestNumber = entity.RequestNumber;
            viewModel.IsCounterOfferAvailable = helperDomain.IsCounterOfferAvailable(entity.Id, 0, userId);
            var contactPerson = job.Users1.FirstOrDefault();
            if (contactPerson != null)
                viewModel.ContactPerson = $"{contactPerson.FirstName} {contactPerson.LastName} - {contactPerson.PhoneNumber} - {contactPerson.Email}";

            if (entity.Orders != null && entity.Orders.Count > 0)
            {
                var brokerdOrderDetail = entity.Orders.LastOrDefault();
                if (brokerdOrderDetail != null)
                {
                    viewModel.OrderId = brokerdOrderDetail.Id;
                    viewModel.PoNumber = brokerdOrderDetail.PoNumber;
                    if (brokerdOrderDetail.Order1 != null)
                    {
                        viewModel.OrderId = brokerdOrderDetail.Order1.Id;
                    }
                    var originalOrder = entity.Orders.FirstOrDefault();
                    if (originalOrder != null && originalOrder.FuelRequest.GetParentFuelRequest().FuelRequest1.Orders.LastOrDefault().OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId != (int)OrderStatus.Open)
                    {
                        viewModel.OrderId = originalOrder.Id;
                        viewModel.PoNumber = originalOrder.PoNumber;
                    }
                }
            }

            return viewModel;
        }

        public static MapViewModel ToMapViewModel(this FuelRequest entity, MapViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new MapViewModel(Status.Success);

            var job = entity.Job;

            viewModel.JobId = entity.Id;
            viewModel.Name = job.Name;
            viewModel.Address = job.Address;
            viewModel.City = job.City;
            viewModel.ZipCode = job.ZipCode;
            viewModel.State = job.MstState.Code;
            viewModel.Country = job.MstCountry.Code;
            viewModel.ContactPersons = job.Users1.Select(t => new ContactPersonViewModel()
            {
                Id = t.Id,
                Name = $"{t.FirstName} {t.LastName}",
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            }).ToList();
            viewModel.Longitude = job.Longitude;
            viewModel.Latitude = job.Latitude;
            return viewModel;
        }

        public static PendingRequestViewModel ToPendingRequestViewModel(this FuelRequest entity, PendingRequestViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new PendingRequestViewModel();

            viewModel.FuelRequestId = entity.Id;
            viewModel.Quantity = entity.MaxQuantity;
            viewModel.FuelType = ContextFactory.Current.GetDomain<HelperDomain>().GetProductName(entity.MstProduct);
            viewModel.StartDate = entity.FuelRequestDetail.StartDate;
            viewModel.EndDate = entity.FuelRequestDetail.EndDate;
            viewModel.StartTime = Convert.ToDateTime(entity.FuelRequestDetail.StartTime.ToString()).ToShortTimeString();
            viewModel.EndTime = Convert.ToDateTime(entity.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
            viewModel.QuantityTypeId = entity.QuantityTypeId;
            return viewModel;
        }

        public static CloneRequestViewModel ToCloneRequestViewModel(this FuelRequest entity, CloneRequestViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new CloneRequestViewModel(Status.Success);

            viewModel.Quantity = entity.MaxQuantity.GetPreciseValue(6);
            viewModel.StartDate = entity.FuelRequestDetail.StartDate;
            viewModel.EndDate = entity.FuelRequestDetail.EndDate;
            viewModel.ExpirationDate = entity.ExpirationDate;
            viewModel.StartTime = Convert.ToString(entity.FuelRequestDetail.StartTime);
            viewModel.EndTime = Convert.ToString(entity.FuelRequestDetail.EndTime);
            viewModel.CompanyId = entity.Job.Company.Id;
            viewModel.JobStartDate = entity.Job.StartDate.Date.ToString(Resource.constFormatDate);
            viewModel.JobEndDate = entity.Job.EndDate != null ? entity.Job.EndDate.Value.Date.ToString(Resource.constFormatDate) : string.Empty;
            viewModel.JobLocationCurrentDateTime = DateTimeOffset.Now.ToTargetDateTimeOffset(entity.Job.TimeZoneName).DateTime.ToString();
            viewModel.IsProFormaPoEnabled = entity.Job.IsProFormaPoEnabled;
            viewModel.QuantityTypeId = entity.QuantityTypeId;
            return viewModel;
        }

        public static CompanyAddress GetBuyerLocation(this FuelRequest entity)
        {
            if (entity.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
            {
                return entity.Job.Company.CompanyAddresses.FirstOrDefault(t => t.IsDefault && t.IsActive);
            }
            else
            {
                return entity.FuelRequest1.User.Company.CompanyAddresses.FirstOrDefault(t => t.IsDefault && t.IsActive);
            }
        }

        public static BillingAddress GetBuyerBillingLocation(this FuelRequest entity)
        {
            if (entity.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
            {
                return entity.Job.Company.BillingAddresses.OrderByDescending(t => t.UpdatedDate).FirstOrDefault(t => t.IsActive);
            }
            else
            {
                return entity.User.Company.BillingAddresses.OrderByDescending(t => t.UpdatedDate).FirstOrDefault(t => t.IsActive);
            }
        }

        public static Company GetCompany(this FuelRequest entity)
        {
            if (entity.FuelRequestTypeId != (int)FuelRequestType.BrokeredFuelRequest)
            {
                return entity.Job.Company;
            }
            else
            {
                return entity.User.Company;
            }
        }

        public static List<int> GetBrokerChainCompanyIdList(this FuelRequest entity)
        {
            List<int> response = new List<int>();
            if (entity.FuelRequest1 == null)
            {
                return response;
            }
            else
            {
                return BrokerCompany(entity, response);
            }
        }

        private static List<int> BrokerCompany(this FuelRequest entity, List<int> list)
        {
            if (entity.FuelRequest1 == null)
            {
                return list;
            }
            else
            {
                list.Add(entity.User.Company.Id);
                return BrokerCompany(entity.FuelRequest1, list);
            }
        }

        public static int GetParentId(this FuelRequest entity)
        {
            if (entity.FuelRequest1 == null)
            {
                return entity.Id;
            }
            else
            {
                return GetParentId(entity.FuelRequest1);
            }
        }

        public static Order GetOrder(this FuelRequest entity)
        {
            var parentFuelRequest = entity.GetParentFuelRequest().FuelRequest1;
            if (parentFuelRequest != null && parentFuelRequest.Orders.Any() && entity.Orders.Count == 0)
                return parentFuelRequest.Orders.LastOrDefault();
            else
                return entity.Orders.FirstOrDefault();
        }

        public static Resale ToResaleEntity(this FuelRequestViewModel viewModel, Resale entity = null)
        {
            if (entity == null)
                entity = new Resale();

            entity.Id = viewModel.Id;
            entity.PricePerGallon = 0;
            entity.RackAvgTypeId = null;
            entity.PricingTypeId = viewModel.FuelRequestResale.FuelPricing.PricingTypeId;
            if (viewModel.FuelRequestResale.FuelPricing.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                entity.PricePerGallon = viewModel.FuelRequestResale.FuelPricing.PricePerGallon;
            }
            else if (viewModel.FuelRequestResale.FuelPricing.PricingTypeId == (int)PricingType.RackAverage)
            {
                entity.PricePerGallon = viewModel.FuelRequestResale.FuelPricing.RackPrice;
                entity.RackAvgTypeId = viewModel.FuelRequestResale.FuelPricing.RackAvgTypeId;
                if (viewModel.FuelRequestResale.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackLow)
                {
                    entity.PricingTypeId = (int)PricingType.RackLow;
                }
                else if (viewModel.FuelRequestResale.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackHigh)
                {
                    entity.PricingTypeId = (int)PricingType.RackHigh;
                }
            }
            else if (viewModel.FuelRequestResale.FuelPricing.PricingTypeId == (int)PricingType.Suppliercost)
            {
                entity.RackAvgTypeId = viewModel.FuelRequestResale.FuelPricing.SupplierCostMarkupTypeId;
                entity.PricePerGallon = viewModel.FuelRequestResale.FuelPricing.SupplierCostMarkupValue ;
            }

            entity.IsDDTEnabled = viewModel.FuelRequestResale.IsDropTicketEnabled;
            entity.IsActive = viewModel.FuelRequestResale.IsActive;
            entity.UpdatedBy = viewModel.FuelRequestResale.UpdatedBy;
            entity.UpdatedDate = viewModel.FuelRequestResale.UpdatedDate;
            entity.ExchangeRate = viewModel.FuelDetails.FuelPricing.ExchangeRate;
            entity.Currency = viewModel.FuelDetails.FuelPricing.Currency;
            entity.UoM = viewModel.FuelDetails.FuelQuantity.UoM;

            return entity;
        }

        public static async Task<FuelRequest> ToFuelRequestEntityFromTPO(this ThirdPartyOrderViewModel viewModel, Job job, decimal latitude, decimal longitude, FuelRequest entity = null,int companyId=0)
        {
            if (entity == null)
                entity = new FuelRequest();
            var fuelrequestDomain = new FuelRequestDomain();
            if (viewModel.FuelDetails.FuelDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                entity.FuelDescription = viewModel.FuelDetails.NonStandardFuelDescription;
            }            
            else if(viewModel.FuelDetails.FuelDisplayGroupId !=(int)ProductDisplayGroups.AdditiveFuelType)
            {
                //tier
                if (!viewModel.FuelDetails.IsTierPricing)
                    viewModel.FuelDetails.FuelTypeId = fuelrequestDomain.GetFuelTypeId(viewModel.FuelDetails.FuelTypeId ?? 0, viewModel.PricingDetails.FuelPricingDetails.PricingSourceId, viewModel.PricingDetails.PricingTypeId);
                else
                {
                    int i = 0;
                    foreach (var item in viewModel.PricingDetails.TierPricing.Pricings)
                    {
                        item.FuelTypeId = fuelrequestDomain.GetFuelTypeId(item.FuelTypeId ?? 0, item.PricingSourceId, item.PricingTypeId);
                        if (i == 0) // set default fuel type in fuelrequest table
                            viewModel.FuelDetails.FuelTypeId = item.FuelTypeId;
                    }
                }
                
            }

            entity.FuelTypeId = viewModel.FuelDetails.FuelTypeId.Value;
            entity.ProductDisplayGroupId = viewModel.FuelDetails.FuelDisplayGroupId;
            
            entity.QuantityTypeId = viewModel.FuelDetails.FuelQuantity.QuantityTypeId;
            entity.MinQuantity = viewModel.FuelDetails.FuelQuantity.MinimumQuantity;
            entity.MaxQuantity = viewModel.FuelDetails.FuelQuantity.MaximumQuantity;
            if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                entity.MaxQuantity = viewModel.FuelDetails.FuelQuantity.Quantity;
            }
            else if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
            {
                entity.MaxQuantity = ApplicationConstants.QuantityNotSpecified;
            }
            entity.PricingTypeId = viewModel.PricingDetails.PricingTypeId;
            if (viewModel.PricingDetails.PricingTypeId == (int)PricingType.RackAverage
                || viewModel.PricingDetails.PricingTypeId == (int)PricingType.RackLow
                || viewModel.PricingDetails.PricingTypeId == (int)PricingType.RackHigh)
            {
                if (viewModel.PricingDetails.MarkertBasedPricingTypeId == (int)PricingType.RackLow)
                {
                    entity.PricingTypeId = (int)PricingType.RackLow;
                }
                else if (viewModel.PricingDetails.MarkertBasedPricingTypeId == (int)PricingType.RackHigh)
                {
                    entity.PricingTypeId = (int)PricingType.RackHigh;
                }
                entity.CityGroupTerminalId = viewModel.PricingDetails.CityGroupTerminalId;
            }

            entity.IsOverageAllowed = true;
            entity.OverageAllowedAmount = 100;
            entity.PaymentTermId = (int)PaymentTerms.DueOnReceipt;
            entity.NetDays = 0;
            entity.OrderTypeId = 2;
            entity.IsPublicRequest = true;
            if (entity.FuelRequestXStatuses.Count > 0)
            {
                entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
            }
            FuelRequestXStatus fuelRequestStatus = new FuelRequestXStatus();
            fuelRequestStatus.StatusId = (int)FuelRequestStatus.Accepted;
            fuelRequestStatus.IsActive = true;
            fuelRequestStatus.UpdatedBy = viewModel.UpdatedBy;
            fuelRequestStatus.UpdatedDate = DateTimeOffset.Now;
            entity.FuelRequestXStatuses.Add(fuelRequestStatus);

            entity.CreatedBy = (int)viewModel.CustomerDetails.UserId;
            entity.CreatedDate = DateTimeOffset.Now;

            //Tier [set Terminal id and price]
            if (viewModel.FuelDetails.IsTierPricing)
            {
                int i = 0;
                foreach (var item in viewModel.PricingDetails.TierPricing.Pricings)
                {
                    var terminalId = item.TerminalId.HasValue ? item.TerminalId.Value : 0;
                    if (viewModel.FuelDetails.FuelDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                    {
                        if (terminalId == 0)
                        {
                            ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                            var externalPricingData = await externalPricingDomain.GetClosestTerminalPriceAsync(latitude, longitude, job.MstCountry.Code, item.FuelTypeId.Value, item.PricingCode.Id);
                            if (externalPricingData.TerminalId != 0)
                            {
                                CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(externalPricingDomain);
                                item.TerminalId = externalPricingData.TerminalId;
                                item.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, job.Currency, externalPricingData.TerminalPrice, DateTimeOffset.Now);
                            }
                            else if (viewModel.PricingDetails.FuelPricingDetails.PricingSourceId != (int)PricingSource.Axxis)
                            {
                                var terminals = await externalPricingDomain.GetClosestTerminals(item.FuelTypeId.Value, latitude, longitude, job.CountryId, string.Empty, item.PricingCode.Id);
                                var terminal = terminals.FirstOrDefault(t => t.Id > 0);
                                if (terminal != null)
                                {
                                    item.TerminalId = terminal.Id;
                                }

                                var response = await externalPricingDomain.GetTerminalPriceAsync(item.TerminalId, item.FuelTypeId.Value, DateTimeOffset.Now, item.PricingCode.Id, job.Currency, item.CityGroupTerminalId);
                                if (response != null)
                                {
                                    item.CreationTimeRackPPG = response.TerminalPrice;
                                }
                            }
                        }
                        else
                        {
                            entity.TerminalId = item.TerminalId;
                            entity.CreationTimeRackPPG = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetTerminalPrice(terminalId, item.FuelTypeId.Value, item.PricingTypeId, job.Currency, DateTimeOffset.Now);
                        }
                        if (i == 0)
                        {
                            entity.TerminalId = item.TerminalId;
                            entity.CreationTimeRackPPG = item.CreationTimeRackPPG.HasValue ? item.CreationTimeRackPPG.Value:0;
                            viewModel.PricingDetails.TerminalId = item.TerminalId;
                            viewModel.PricingDetails.FuelTypeId = item.FuelTypeId;
                        }
                    }
                    i++;
                    }
                //Reset commulation for volume base Tier
                if(viewModel.PricingDetails.TierPricing.IsResetCumulation && viewModel.PricingDetails.TierPricing.TierPricingType==TierPricingType.VolumeBased)
                fuelrequestDomain.SetTierPricingResetCommulation(viewModel.PricingDetails.TierPricing.ResetCumulationSetting,job);

            }
            else if (viewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod == FreightPricingMethod.Manual) //non tier
            {
                var terminalId = viewModel.PricingDetails.TerminalId.HasValue ? viewModel.PricingDetails.TerminalId.Value : 0;

                if (viewModel.FuelDetails.FuelDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType)
                {
                    if (terminalId == 0)
                    {
                        ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                        var externalPricingData = await externalPricingDomain.GetClosestTerminalPriceAsync(latitude, longitude, job.MstCountry.Code, viewModel.FuelDetails.FuelTypeId.Value, viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id);
                        if (externalPricingData.TerminalId != 0)
                        {
                            CurrencyRateDomain currencyRateDomain = new CurrencyRateDomain(externalPricingDomain);
                            entity.TerminalId = externalPricingData.TerminalId;
                            entity.CreationTimeRackPPG = currencyRateDomain.Convert(externalPricingData.Currency, job.Currency, externalPricingData.TerminalPrice, DateTimeOffset.Now);
                        }
                        else if (viewModel.PricingDetails.FuelPricingDetails.PricingSourceId != (int)PricingSource.Axxis)
                        {
                            var terminals = await externalPricingDomain.GetClosestTerminals(viewModel.FuelDetails.FuelTypeId.Value, latitude, longitude, job.CountryId, string.Empty, viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id);
                            var terminal = terminals.FirstOrDefault(t => t.Id > 0);
                            if (terminal != null)
                            {
                                entity.TerminalId = terminal.Id;
                            }

                            var response = await externalPricingDomain.GetTerminalPriceAsync(entity.TerminalId, viewModel.FuelDetails.FuelTypeId.Value, DateTimeOffset.Now, viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id, job.Currency, entity.CityGroupTerminalId);
                            if (response != null)
                            {
                                entity.CreationTimeRackPPG = response.TerminalPrice;
                            }
                        }
                    }
                    else
                    {
                        entity.TerminalId = viewModel.PricingDetails.TerminalId;
                        entity.CreationTimeRackPPG = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetTerminalPrice(terminalId, viewModel.FuelDetails.FuelTypeId.Value, entity.PricingTypeId, job.Currency, DateTimeOffset.Now);
                    }
                }
            }
            else if (viewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod == FreightPricingMethod.Auto) //non tier
            {
                #region new tpo case to assigned terminal
                if (viewModel.SourceRegion.ApprovedTerminalId != 0)
                {
                    entity.TerminalId = viewModel.SourceRegion.ApprovedTerminalId;
                    entity.CreationTimeRackPPG = await ContextFactory.Current.GetDomain<ExternalPricingDomain>().GetTerminalPrice(entity.TerminalId.Value, viewModel.FuelDetails.FuelTypeId.Value, entity.PricingTypeId, job.Currency, DateTimeOffset.Now);
                }
                else if (viewModel.SourceRegion.SelectedTerminals.Count == 1 && viewModel.SourceRegion.ApprovedTerminalId == 0)
                {
                    entity.TerminalId = viewModel.SourceRegion.SelectedTerminals.FirstOrDefault();
                    ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                    var response = await externalPricingDomain.GetTerminalPriceAsync(entity.TerminalId, viewModel.FuelDetails.FuelTypeId.Value, DateTimeOffset.Now, viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id, job.Currency, entity.CityGroupTerminalId);
                    if (response != null)
                    {
                        entity.CreationTimeRackPPG = response.TerminalPrice;
                    }
                }
                else if (viewModel.SourceRegion.SelectedTerminals.Count > 1 && viewModel.SourceRegion.ApprovedTerminalId == 0)
                {
                    var inputModel = new SourceRegionRequestModel()
                    {
                        TerminalIds = viewModel.SourceRegion.SelectedTerminals,
                        FuelTypeId = viewModel.FuelDetails.FuelTypeId.Value,
                        PricingCodeId = viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id,
                        Latitude = job.Latitude,
                        Longitude = job.Longitude,
                        IsSupressPricing = viewModel.IsSupressOrderPricing,
                    };
                    ExternalPricingDomain externalPricingDomain = new ExternalPricingDomain();
                    int companyCountryId = await Task.Run(() => ContextFactory.Current.GetDomain<MasterDomain>().GetDefaultServingCountry(companyId));
                    var terminals = await externalPricingDomain.GetClosestTerminalsForSourceRegions(companyId, companyCountryId, inputModel);
                    if (terminals != null && terminals.Any())
                    {
                        // var selectedTerminal= terminals.Where(w => viewModel.SourceRegion.SelectedTerminals.Contains(w.Id)).ToList();
                        //   if (selectedTerminal != null && selectedTerminal.Any())
                        // {
                        entity.TerminalId = terminals.OrderBy(o => Convert.ToDouble(o.Code)).FirstOrDefault().Id;
                        var response = await externalPricingDomain.GetTerminalPriceAsync(entity.TerminalId, viewModel.FuelDetails.FuelTypeId.Value, DateTimeOffset.Now, viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id, job.Currency, entity.CityGroupTerminalId);
                        if (response != null)
                        {
                            entity.CreationTimeRackPPG = response.TerminalPrice;
                        }
                        // }
                    }
                }
                #endregion
                viewModel.PricingDetails.TerminalId = entity.TerminalId;
                viewModel.PricingDetails.FuelTypeId = entity.FuelTypeId;
            }
          

            if (!string.IsNullOrWhiteSpace(viewModel.PONumber))
            {
                entity.ExternalPoNumber = viewModel.PONumber;
            }

            entity.IsActive = true;
            entity.UpdatedBy = (int)viewModel.CustomerDetails.UserId;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }

        public static FuelRequest ToFuelRequestEntityFromOffer(this OfferOrderViewModel viewModel, OfferPricing offerPricing, User buyerUser)
        {
            var fuelRequest = new FuelRequest();

            // basic FR details
            //fuelRequest.JobId = viewModel.AddressDetails.JobId.Value;
            fuelRequest.OfferPricingId = offerPricing.Id;
            fuelRequest.ProductDisplayGroupId = (int)ProductDisplayGroups.CommonFuelType;
            fuelRequest.FuelTypeId = viewModel.FuelDetails.FuelTypeId.Value;
            fuelRequest.OrderTypeId = viewModel.FuelDetails.OrderTypeId;
            fuelRequest.IsPublicRequest = true;
            fuelRequest.QuantityTypeId = (int)QuantityType.SpecificAmount;
            if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                fuelRequest.MaxQuantity = viewModel.FuelDetails.FuelQuantity.Quantity;
            }
            else if (viewModel.FuelDetails.FuelQuantity.QuantityTypeId == (int)QuantityType.Range)
            {
                fuelRequest.MinQuantity = viewModel.FuelDetails.FuelQuantity.MinimumQuantity;
                fuelRequest.MaxQuantity = viewModel.FuelDetails.FuelQuantity.MaximumQuantity;
            }
            fuelRequest.FuelRequestTypeId = (int)FuelRequestType.OfferFuelRequest;
            fuelRequest.User = buyerUser;

            if (offerPricing.PricingTypeId == (int)PricingType.Tier)
            {
                // get applicable tier here
                var differentFuelPrice = offerPricing.DifferentFuelPrices.SingleOrDefault
                                        (
                                            t =>
                                                (
                                                    t.MinQuantity <= viewModel.FuelDetails.FuelQuantity.Quantity &&
                                                    t.MaxQuantity >= viewModel.FuelDetails.FuelQuantity.Quantity
                                                )
                                                ||
                                                (
                                                    t.MinQuantity <= viewModel.FuelDetails.FuelQuantity.Quantity && !t.MaxQuantity.HasValue
                                                )
                                        );
                if (differentFuelPrice != null)
                {
                    viewModel.FuelDetails.FuelPricing.RackAvgTypeId = differentFuelPrice.RackAvgTypeId;
                    viewModel.FuelDetails.FuelPricing.PricePerGallon = differentFuelPrice.PricePerGallon;
                    viewModel.FuelDetails.FuelPricing.PricingTypeId = differentFuelPrice.PricingTypeId;
                    //viewModel.FuelDetails.FuelPricing.SupplierCost = viewModel.FuelDetails.FuelPricing.SupplierCost;
                    viewModel.FuelDetails.FuelPricing.SupplierCostTypeId = differentFuelPrice.SupplierCostTypeId;
                }
            }
            else
            {
                viewModel.FuelDetails.FuelPricing = offerPricing.ToFuelPricingViewModel(viewModel.FuelDetails.FuelPricing);
            }
            // Save FR pricing details
            viewModel.FuelDetails.FuelQuantity.UoM = offerPricing.UoM;
            var pricingDetailId = Task.Run(() => new PricingServiceDomain().SavePricingDetails(viewModel.FuelDetails.FuelPricing, offerPricing.UoM)).Result;

            if (pricingDetailId == null || pricingDetailId.Result == 0)
            {
                throw new ArgumentNullException(Resource.errMessageFailedSaveFuelPricing);
            }

            if (pricingDetailId != null)
            {
                viewModel.FuelDetails.FuelPricing.FuelPricingDetails.RequestPriceDetailId = pricingDetailId.Result;
            }

            fuelRequest.IsOverageAllowed = true;
            fuelRequest.OverageAllowedAmount = 0;
            fuelRequest.PaymentTermId = viewModel.FuelOfferDetails.PaymentTermId;
            fuelRequest.NetDays = viewModel.FuelOfferDetails.NetDays;
            fuelRequest.UoM = offerPricing.UoM;
            fuelRequest.Currency = offerPricing.Currency;
            fuelRequest.ExchangeRate = offerPricing.ExchangeRate;

            fuelRequest.CreatedDate = DateTimeOffset.Now;
            fuelRequest.UpdatedDate = DateTimeOffset.Now;
            fuelRequest.IsActive = true;

            // fuel delivery details
            fuelRequest.FuelRequestDetail = new FuelRequestDetail();
            viewModel.FuelDeliveryDetails.FuelRequestId = fuelRequest.Id;
            fuelRequest.FuelRequestDetail.PricingQuantityIndicatorTypeId = (int)viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes;
            fuelRequest.FuelRequestDetail = viewModel.FuelDeliveryDetails.ToEntity(fuelRequest.FuelRequestDetail);

            // fuel pricing details
            fuelRequest.FuelRequestPricingDetail = new FuelRequestPricingDetail();
            fuelRequest.FreightOnBoardTypeId = (int)FreightOnBoardTypes.Terminal;
            fuelRequest.FuelRequestPricingDetail.PricingCode = viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Code;
            fuelRequest.FuelRequestPricingDetail.PricingCodeId = viewModel.FuelDetails.FuelPricing.FuelPricingDetails.PricingCode.Id;
            fuelRequest.FuelRequestPricingDetail.RequestPriceDetailId = pricingDetailId.Result;
            fuelRequest.FuelRequestPricingDetail.DisplayPrice = pricingDetailId.CustomString1;
            fuelRequest.FuelRequestPricingDetail.DisplayPriceCode = pricingDetailId.CustomString2;

            fuelRequest.PricingTypeId = viewModel.FuelDetails.FuelPricing.PricingTypeId;

            // fr status
            FuelRequestXStatus fuelRequestStatus = new FuelRequestXStatus();
            fuelRequestStatus.StatusId = (int)FuelRequestStatus.Open;
            fuelRequestStatus.IsActive = true;
            fuelRequestStatus.UpdatedBy = buyerUser.Id;
            fuelRequestStatus.UpdatedDate = DateTimeOffset.Now;
            fuelRequest.FuelRequestXStatuses.Add(fuelRequestStatus);

            return fuelRequest;
        }

        public static FuelDetailsViewModel ToFuelDetailsViewModelFromOffer(this OfferPricing entity, FuelDetailsViewModel viewModel, int quantity, int buyerCompanyId, int fuelTypeId, int stateId)
        {
            if (viewModel == null)
                viewModel = new FuelDetailsViewModel(Status.Success);

            viewModel.FuelPricing.PricingTypeId = entity.PricingTypeId;
            viewModel.FuelPricing.RackAvgTypeId = entity.RackAvgTypeId ?? 0;
            viewModel.FuelPricing.MarkertBasedPricingTypeId = 0;
            viewModel.FuelPricing.SupplierCost = 0;
            if (entity.PricingTypeId == (int)PricingType.Tier)
            {
                // get applicable tier here
                var differentFuelPrice = entity.DifferentFuelPrices.SingleOrDefault
                                        (
                                            t =>
                                                (
                                                    t.MinQuantity <= quantity &&
                                                    t.MaxQuantity >= quantity
                                                )
                                                ||
                                                (
                                                    t.MinQuantity <= quantity && !t.MaxQuantity.HasValue
                                                )
                                        );
                if (differentFuelPrice != null)//we are not using differentFuelPrice
                {
                    viewModel.FuelPricing.RackAvgTypeId = differentFuelPrice.RackAvgTypeId ?? 0;
                    if (differentFuelPrice.PricingTypeId == (int)PricingType.PricePerGallon)
                    {
                        viewModel.FuelPricing.PricePerGallon = differentFuelPrice.PricePerGallon.GetPreciseValue(6);
                        viewModel.FuelPricing.RackPrice = differentFuelPrice.PricePerGallon; // do not delete - reuired to calculate for final price
                    }
                    else if (differentFuelPrice.PricingTypeId == (int)PricingType.RackAverage)
                    {
                        viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
                    }
                    else if (differentFuelPrice.PricingTypeId == (int)PricingType.RackHigh)
                    {
                        viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                        viewModel.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
                    }
                    else if (differentFuelPrice.PricingTypeId == (int)PricingType.RackLow)
                    {
                        viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                        viewModel.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
                    }
                    else if (differentFuelPrice.PricingTypeId == (int)PricingType.Suppliercost)
                    {
                        var globalCost = ContextFactory.Current.GetDomain<CurrentCostDomain>().
                                            GetFuelCostForFuelRequest(buyerCompanyId, fuelTypeId,
                                            stateId,entity.UoM, Currency.USD).Result;
                        if (globalCost.HasValue)
                        {
                            viewModel.FuelPricing.SupplierCost = globalCost.Value;
                        }
                        viewModel.FuelPricing.SupplierCostMarkupTypeId = differentFuelPrice.RackAvgTypeId;
                        viewModel.FuelPricing.SupplierCostMarkupValue = differentFuelPrice.PricePerGallon.GetPreciseValue(6);
                        viewModel.FuelPricing.RackPrice = differentFuelPrice.PricePerGallon; // do not delete - reuired to calculate for final price
                    }

                    if (differentFuelPrice.PricingTypeId == (int)PricingType.RackAverage
                        || differentFuelPrice.PricingTypeId == (int)PricingType.RackHigh
                        || differentFuelPrice.PricingTypeId == (int)PricingType.RackLow)
                    {
                        viewModel.FuelPricing.RackPrice = differentFuelPrice.PricePerGallon.GetPreciseValue(6);
                    }
                }
            }
            else
            {
                if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
                {
                    viewModel.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
                    viewModel.FuelPricing.RackPrice = entity.PricePerGallon; // do not delete - reuired to calculate for final price
                }
                else if (entity.PricingTypeId == (int)PricingType.RackAverage)
                {
                    viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
                }
                else if (entity.PricingTypeId == (int)PricingType.RackHigh)
                {
                    viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
                    viewModel.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
                }
                else if (entity.PricingTypeId == (int)PricingType.RackLow)
                {
                    viewModel.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
                    viewModel.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
                }
                else if (entity.PricingTypeId == (int)PricingType.Suppliercost)
                {
                    var globalCost = Task.Run(() => ContextFactory.Current.GetDomain<CurrentCostDomain>().
                    GetFuelCostForFuelRequest(entity.SupplierCompanyId, fuelTypeId,
                    stateId,entity.UoM, Currency.USD)).Result;
                    if (globalCost.HasValue)
                    {
                        viewModel.FuelPricing.SupplierCost = globalCost.Value;
                    }
                    viewModel.FuelPricing.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
                    viewModel.FuelPricing.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
                    viewModel.FuelPricing.RackPrice = entity.PricePerGallon; // do not delete - reuired to calculate for final price
                }

                if (entity.PricingTypeId == (int)PricingType.RackAverage
                    || entity.PricingTypeId == (int)PricingType.RackHigh
                    || entity.PricingTypeId == (int)PricingType.RackLow)
                {
                    viewModel.FuelPricing.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
                }
            }
            return viewModel;
        }

        public static FuelRequest GetParentFuelRequest(this FuelRequest entity)
        {
            if (entity.FuelRequestTypeId != (int)FuelRequestType.CounteredFuelRequest)
            {
                return entity;
            }
            else
            {
                return GetParentFuelRequest(entity.FuelRequest1);
            }
        }

        public static Order GetFuelRequestLastOrder(this FuelRequest entity)
        {
            if (entity.Orders.Any())
            {
                return entity.Orders.LastOrDefault();
            }
            else if (entity.FuelRequests1.Any())
            {
                Order order = null;
                foreach (var fuelRequest in entity.FuelRequests1)
                {
                    order = GetFuelRequestLastOrder(fuelRequest);
                    if (order != null && order.Id > 0)
                    {
                        break;
                    }
                }
                return order;
            }
            else
            {
                return null;
            }
        }

        public static Order GetFuelRequestFirstOrder(this FuelRequest entity)
        {
            if (entity.Orders.Any())
            {
                return entity.Orders.FirstOrDefault();
            }
            else if (entity.FuelRequests1.Any())
            {
                Order order = null;
                foreach (var fuelRequest in entity.FuelRequests1)
                {
                    order = GetFuelRequestFirstOrder(fuelRequest);
                    if (order != null && order.Id > 0)
                    {
                        break;
                    }
                }
                return order;
            }
            else
            {
                return null;
            }
        }
    }
}
