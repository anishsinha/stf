using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BrokerFuelRequestMapper
    {
        public static FuelRequest ToEntity(this BrokerFuelRequestViewModel viewModel, FuelRequest entity = null)
        {
            if (entity == null)
                entity = new FuelRequest();
           
            entity.FuelRequestTypeId = viewModel.Type;
            entity.ParentId = viewModel.ParentId;
            entity.ProductDisplayGroupId = viewModel.Details.FuelDisplayGroupId;
            entity.FuelDescription = viewModel.Details.NonStandardFuelDescription;
            entity.QuantityTypeId = viewModel.Details.FuelQuantity.QuantityTypeId;
            entity.FreightOnBoardTypeId = viewModel.Details.FreightOnBoard;
            if (viewModel.Details.FuelQuantity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                entity.MaxQuantity = viewModel.Details.FuelQuantity.Quantity;
            }
            else if (viewModel.Details.FuelQuantity.QuantityTypeId == (int)QuantityType.Range)
            {
                entity.MinQuantity = viewModel.Details.FuelQuantity.MinimumQuantity;
                entity.MaxQuantity = viewModel.Details.FuelQuantity.MaximumQuantity;
            }
            else if (viewModel.Details.FuelQuantity.QuantityTypeId == (int)QuantityType.NotSpecified)
            {
                entity.MaxQuantity = ApplicationConstants.QuantityNotSpecified;
            }
            entity.EstimateGallonsPerDelivery = viewModel.Details.FuelQuantity.EstimatedGallonsPerDelivery.HasValue ? viewModel.Details.FuelQuantity.EstimatedGallonsPerDelivery.Value : 0;
            entity.PricingTypeId = viewModel.Details.FuelPricing.PricingTypeId;

            bool isMarketPricing = false;

            if (viewModel.Details.FuelPricing.PricingTypeId == (int)PricingType.RackAverage ||
                     viewModel.Details.FuelPricing.PricingTypeId == (int)PricingType.RackHigh ||
                     viewModel.Details.FuelPricing.PricingTypeId == (int)PricingType.RackLow)
            {
                isMarketPricing = true;
                entity.CityGroupTerminalId = viewModel.Details.FuelPricing.CityGroupTerminalId;
                if (viewModel.Details.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackLow)
                {
                    entity.PricingTypeId = (int)PricingType.RackLow;
                }
                else if (viewModel.Details.FuelPricing.MarkertBasedPricingTypeId == (int)PricingType.RackHigh)
                {
                    entity.PricingTypeId = (int)PricingType.RackHigh;
                }
            }
            entity.IsOverageAllowed = viewModel.Details.IsOverageAllowed;
            if (entity.IsOverageAllowed)
            {
                entity.OverageAllowedAmount = viewModel.Details.OverageAllowedPercent;
            }
            else
            {
                entity.OverageAllowedAmount = 0;
            }
            entity.OrderTypeId = viewModel.Details.OrderTypeId;
            entity.IsPublicRequest = viewModel.Details.PrivateSupplierList.IsPublicRequest;
            entity.PaymentTermId = viewModel.Terms.PaymentTermId;
            entity.NetDays = viewModel.Terms.NetDays;

            if (entity.FuelRequestXStatuses.Count > 0)
                entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).IsActive = false;
            FuelRequestXStatus fuelRequestStatus = new FuelRequestXStatus();
            fuelRequestStatus.StatusId = (int)FuelRequestStatus.Open;
            fuelRequestStatus.IsActive = true;
            fuelRequestStatus.UpdatedBy = viewModel.UpdatedBy;
            fuelRequestStatus.UpdatedDate = DateTimeOffset.Now;
            entity.FuelRequestXStatuses.Add(fuelRequestStatus);

            entity.FuelRequestPricingDetail = viewModel.Details.FuelPricing.FuelPricingDetails.ToPricingDetailsEntity(isMarketPricing);
            entity.CreatedBy = viewModel.Terms.CreatedBy;
            entity.CreatedDate = viewModel.Terms.CreatedDate;
            entity.ExpirationDate = viewModel.Details.FuelDeliveryDetails.ExpirationDate;
            entity.OrderClosingThreshold = viewModel.Terms.OrderClosingThreshold;
            entity.ExternalPoNumber = viewModel.Terms.ExternalPoNumber;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedBy = viewModel.UpdatedBy;
            entity.UpdatedDate = viewModel.UpdatedDate;
            entity.Currency = viewModel.Details.FuelPricing.Currency;
            entity.UoM = viewModel.Details.FuelQuantity.UoM;

            return entity;
        } 

        public static BrokerFuelRequestViewModel ToBrokerViewModel(this FuelRequest entity, bool setMargin = false, bool isNewRequest = false, BrokerFuelRequestViewModel viewModel = null, bool setDeliverySchedules = true)
        {
            if (viewModel == null)
                viewModel = new BrokerFuelRequestViewModel();
            viewModel.Details.FuelTypeId = entity.MstProduct.TfxProductId ?? entity.FuelTypeId;
            viewModel.Details.FuelDisplayGroupId = entity.MstProduct.ProductDisplayGroupId;
            viewModel.Details.NonStandardFuelDescription = entity.FuelDescription;
            viewModel.Details.FuelType = ContextFactory.Current.GetDomain<HelperDomain>().GetProductName(entity.MstProduct);

            viewModel.Details.FuelPricing.FuelPricingDetails = entity.FuelRequestPricingDetail.ToViewModel();
            viewModel.Details.IsFTLEnabled = entity.FuelRequestDetail != null && entity.FuelRequestDetail.TruckLoadTypeId == (int)TruckLoadTypes.FullTruckLoad;
            viewModel.Details.FreightOnBoard = entity.FreightOnBoardTypeId;
            if (viewModel.Details.IsFTLEnabled)
            {
                viewModel.Details.FuelQuantity.QuantityIndicatorTypes = (QuantityIndicatorTypes)entity.FuelRequestDetail.PricingQuantityIndicatorTypeId;
            }
            else
                viewModel.Details.FuelQuantity.QuantityIndicatorTypes = QuantityIndicatorTypes.Net;

            var order = entity.GetOrder();
            var orderDetailVersionActive = order.OrderDetailVersions.Last();
            if (order != null)
            {
                viewModel.Details.OrderId = viewModel.Terms.OrderId = order.Id;
                viewModel.Details.PoNumber = order.PoNumber;
            }

            viewModel.Details.FuelQuantity.QuantityTypeId = entity.QuantityTypeId;
            if (entity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                viewModel.Details.FuelQuantity.Quantity = isNewRequest ? (order.BrokeredMaxQuantity ?? entity.MaxQuantity) - (entity.HedgeDroppedGallons + entity.SpotDroppedGallons) : (order.BrokeredMaxQuantity ?? entity.MaxQuantity);
            }
            else if (entity.QuantityTypeId == (int)QuantityType.Range)
            {
                viewModel.Details.FuelQuantity.MinimumQuantity = entity.MinQuantity;
                viewModel.Details.FuelQuantity.MaximumQuantity = isNewRequest ? (order.BrokeredMaxQuantity ?? entity.MaxQuantity) - (entity.HedgeDroppedGallons + entity.SpotDroppedGallons) : (order.BrokeredMaxQuantity ?? entity.MaxQuantity);
            }

            viewModel.Details.FuelQuantity.EstimatedGallonsPerDelivery = entity.EstimateGallonsPerDelivery;
            viewModel.StatusId = entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
            //viewModel.Details.FuelPricing.PricingTypeId = entity.PricingTypeId;
            //viewModel.Details.FuelPricing.RackAvgTypeId = entity.RackAvgTypeId;
            viewModel.Details.FuelPricing.CityGroupTerminalId = entity.CityGroupTerminalId;

            if (entity.CityGroupTerminalId.HasValue && entity.CityGroupTerminalId.Value > 0)
            {
                var terminal = ContextFactory.Current.GetDomain<HelperDomain>().GetExternalTerminal(entity.CityGroupTerminalId.Value);
                if (terminal != null)
                    viewModel.Details.FuelPricing.CityGroupTerminalName = $"{terminal.Name}, {terminal.StateCode}";
            }

            //if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
            //{
            //    viewModel.Details.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
            //}
            //else if (entity.PricingTypeId == (int)PricingType.RackAverage)
            //{
            //    viewModel.Details.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
            //}
            //else if (entity.PricingTypeId == (int)PricingType.RackHigh)
            //{
            //    viewModel.Details.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
            //    viewModel.Details.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack High
            //}
            //else if (entity.PricingTypeId == (int)PricingType.RackLow)
            //{
            //    viewModel.Details.FuelPricing.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
            //    viewModel.Details.FuelPricing.PricingTypeId = (int)PricingType.RackAverage; //Do not change this to Rack Low
            //}
            //else if (entity.PricingTypeId == (int)PricingType.Suppliercost)
            //{
            //    viewModel.Details.FuelPricing.SupplierCost = entity.SupplierCost;
            //    viewModel.Details.FuelPricing.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
            //    viewModel.Details.FuelPricing.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
            //}

            //if (entity.PricingTypeId == (int)PricingType.RackAverage || entity.PricingTypeId == (int)PricingType.RackHigh
            //    || entity.PricingTypeId == (int)PricingType.RackLow)
            //{
            //    viewModel.Details.FuelPricing.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
            //}

            viewModel.Details.FuelDeliveryDetails.StartDate = entity.FuelRequestDetail.StartDate;
            viewModel.Details.FuelDeliveryDetails.EndDate = entity.FuelRequestDetail.EndDate;
            viewModel.Details.FuelDeliveryDetails.StartTime = entity.FuelRequestDetail.StartTime.ToString();
            viewModel.Details.FuelDeliveryDetails.EndTime = entity.FuelRequestDetail.EndTime.ToString();
            viewModel.Terms.IsProFormaPoEnabled = entity.Job.IsProFormaPoEnabled;
            viewModel.Details.IsOverageAllowed = entity.IsOverageAllowed;
            viewModel.Details.OverageAllowedPercent = entity.OverageAllowedAmount;
            viewModel.Details.OrderTypeId = entity.OrderTypeId;
            viewModel.Details.PrivateSupplierList.IsPublicRequest = entity.IsPublicRequest;
            var supplierList = entity.PrivateSupplierLists.FirstOrDefault();
            if (!entity.IsPublicRequest && supplierList != null)
            {
                var supplierIds = entity.PrivateSupplierLists.Select(t => t.Id).ToList();
                viewModel.Details.PrivateSupplierList.PrivateSupplierIds = supplierIds;
            }
            viewModel.Terms.StatusId = entity.FuelRequestXStatuses.FirstOrDefault(t => t.IsActive).StatusId;
            viewModel.Terms.CreatedBy = entity.CreatedBy;
            viewModel.Terms.CreatedDate = entity.CreatedDate;
            viewModel.Terms.TerminalId = entity.TerminalId;

            viewModel.Details.FuelDeliveryDetails = entity.FuelRequestDetail.ToViewModel();
            viewModel.Details.FuelDeliveryDetails.TruckLoadTypes = order.IsFTL ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;

            viewModel.Details.FuelDeliveryDetails.FuelFees.FuelRequestFees = entity.FuelRequestFees.ToFeesViewModel();
            viewModel.Details.FuelDeliveryDetails.FuelFees.FuelRequestFees.RemoveAll(t => t.FeeTypeId.Equals(((int)FeeType.ProcessingFee).ToString()));
            viewModel.Details.FuelDeliveryDetails.FuelFees.Currency = entity.Currency;
            viewModel.Details.FuelDeliveryDetails.FuelFees.UoM = entity.UoM;

            if (order.IsFTL)
            {
                viewModel.Details.FuelDeliveryDetails.FuelFees.FuelRequestFees.ForEach(t => t.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad);
            }

            if (setDeliverySchedules && order != null && viewModel.Details.FuelDeliveryDetails.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
            {
                var orderDomain = new OrderDomain();
                var latestOrderSchedule = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                var orderDeliverySchedules = latestOrderSchedule.Where(t => t.DeliveryRequestId.HasValue).Select(t => t.DeliverySchedule).ToList();
                if (orderDeliverySchedules.Any())
                {
                    viewModel.Details.FuelDeliveryDetails.DeliverySchedules = orderDeliverySchedules.GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() }).Select(t => t.Items.ToViewModel()).ToList();
                }
                else
                {
                    var deliverySchedulesFromFR = order.FuelRequest.DeliverySchedules.ToList();
                    if (deliverySchedulesFromFR.Any())
                    {
                        viewModel.Details.FuelDeliveryDetails.DeliverySchedules = deliverySchedulesFromFR.GroupBy(t => t.GroupId).Select(g => new { Items = g.ToList() }).Select(t => t.Items.ToViewModel()).ToList();
                    }
                }
            }

            //if (entity.PricingTypeId == (int)PricingType.Tier)
            //    viewModel.Details.DifferentFuelPrices = entity.DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();

            viewModel.Terms.SpecialInstructions = entity.SpecialInstructions.Select(t => t.ToViewModel()).ToList();

            var paymentDiscount = entity.PaymentDiscounts.FirstOrDefault();
            if (paymentDiscount == null)
            {
                paymentDiscount = new PaymentDiscount();
            }
            viewModel.Terms.PaymentDiscount = paymentDiscount.ToViewModel();

            if (entity.MstSupplierQualifications.Any())
            {
                viewModel.Terms.SupplierQualifications = entity.MstSupplierQualifications.Select(t => t.Id).ToList();
            }

            viewModel.Terms.PaymentTermId = orderDetailVersionActive.PaymentTermId;
            viewModel.Terms.PaymentTermName = orderDetailVersionActive.PaymentTerm.Name;
            viewModel.Terms.NetDays = orderDetailVersionActive.NetDays;
            viewModel.Details.FuelDeliveryDetails.ExpirationDate = entity.ExpirationDate;

            viewModel.Terms.OrderClosingThreshold = entity.OrderClosingThreshold;
            viewModel.Terms.RequestNumber = entity.RequestNumber;
            viewModel.Terms.ExternalPoNumber = orderDetailVersionActive.PoNumber;

            var job = entity.Job;
            viewModel.Details.JobId = job.Id;
            viewModel.Details.JobName = job.Name;
            viewModel.Details.Address = job.Address;
            viewModel.Details.StateId = job.StateId;
            viewModel.Details.Latitude = job.Latitude;
            viewModel.Details.Longitude = job.Longitude;
            viewModel.Details.Location = $"{job.City}{","} {job.MstState.Code} {job.ZipCode}";
            viewModel.Details.CountryId = job.CountryId;
            viewModel.Details.CountryCode = job.MstCountry.Code;
            viewModel.Details.IsMarineLocation = job.IsMarine;
            if (job.LocationType == JobLocationTypes.Various)
            {
                viewModel.Details.IsVarious = true;
            }
            ////for edit purpose
            //if (setMargin)
            //{
            //    viewModel.Details.FuelPriceMargin.Margin = entity.Margin.GetPreciseValue(6);
            //    viewModel.Details.FuelPriceMargin.MarginTypeId = entity.MarginTypeId.HasValue ? entity.MarginTypeId.Value : (int)MarginType.NoChange;
            //}

            viewModel.RequestNumber = entity.RequestNumber;
            viewModel.Details.FuelPricing.CityGroupTerminalStateId = job.StateId;
            viewModel.Details.FuelPricing.Currency = entity.Currency;
            viewModel.Details.FuelQuantity.UoM = entity.UoM;
            viewModel.Details.FuelDeliveryDetails.IsDriverToUpdateBOL = entity.FuelRequestDetail.IsDriverToUpdateBOL;
            viewModel.Details.FuelDeliveryDetails.IsBolImageRequired = entity.FuelRequestDetail.IsBolImageRequired;
            viewModel.Details.FuelDeliveryDetails.IsPrePostDipRequired = entity.FuelRequestDetail.IsPrePostDipRequired;
            viewModel.Details.FuelDeliveryDetails.IsDropImageRequired = entity.FuelRequestDetail.IsDropImageRequired;
            viewModel.Details.FuelDeliveryDetails.OrderEnforcementId = entity.FuelRequestDetail.OrderEnforcementId;
            viewModel.Details.FuelDeliveryDetails.IsDispatchRetainedByCustomer = entity.FuelRequestDetail.IsDispatchRetainedByCustomer;

            return viewModel;
        }
    }
}
