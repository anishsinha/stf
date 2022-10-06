using Newtonsoft.Json;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class BuyerOrderDetailMapper
    {
        public static FuelDeliveryDetailsViewModel ToFuelDeliveryDetailsViewModel(this UspGetBuyerOrderDetail entity)
        {
            var viewModel = new FuelDeliveryDetailsViewModel();
            viewModel.FuelRequestId = entity.FuelRequestId;
            viewModel.DeliveryTypeId = entity.DeliveryTypeId;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.StartTime = Convert.ToDateTime(entity.StartTime.ToString()).ToShortTimeString();
            viewModel.EndTime = Convert.ToDateTime(entity.EndTime.ToString()).ToShortTimeString();
            if (!string.IsNullOrWhiteSpace(entity.CustomAttribute))
            {
                viewModel.CustomAttributeViewModel = JsonConvert.DeserializeObject<FuelRequestCustomAttributeViewModel>(entity.CustomAttribute);
            }
            viewModel.CustomAttribute = entity.CustomAttribute;
            viewModel.OrderEnforcementId = entity.OrderEnforcementId;
            viewModel.IsPrePostDipRequired = entity.IsPrePostDipRequired;
            //viewModel.PoContactId = entity.PoContactId;
            return viewModel;
        }

        public static FuelDetailsViewModel ToFuelDetailsViewModel(this UspGetBuyerOrderDetail entity, FuelDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelDetailsViewModel(Status.Success);

            viewModel.FuelTypeId = entity.FuelTypeId;
            if (entity.ProductDisplayGroupId == (int)ProductDisplayGroups.OtherFuelType)
            {
                viewModel.NonStandardFuelName = entity.FuelTypeName;
                viewModel.NonStandardFuelDescription = entity.FuelDescription;
            }
            viewModel.FuelDisplayGroupId = entity.ProductDisplayGroupId;
            viewModel.FuelType = entity.FuelTypeName;
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
            viewModel.FuelQuantity.QuantityIndicatorTypes = (QuantityIndicatorTypes)entity.PricingQuantityIndicatorTypeId;
            viewModel.FuelPricing.PricingTypeId = entity.PricingTypeId;
            viewModel.FuelPricing.RackAvgTypeId = entity.RackAvgTypeId;
            //viewModel.FuelPricing.CityGroupTerminalId = entity.CityGroupTerminalId;
            //viewModel.FuelPricing.CityGroupTerminalStateId = entity.Job.StateId;
            viewModel.FuelPricing.CityGroupTerminalName = entity.CityGroupTerminalName;

            if (entity.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.FuelPricing.PricePerGallon = entity.PricePerGallon.GetPreciseValue(6);
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
                viewModel.FuelPricing.SupplierCost = entity.SupplierCost;
                viewModel.FuelPricing.SupplierCostMarkupTypeId = entity.RackAvgTypeId;
                viewModel.FuelPricing.SupplierCostMarkupValue = entity.PricePerGallon.GetPreciseValue(6);
            }

            if (entity.PricingTypeId == (int)PricingType.RackAverage
                || entity.PricingTypeId == (int)PricingType.RackHigh
                || entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.FuelPricing.RackPrice = entity.PricePerGallon.GetPreciseValue(6);
            }

            //viewModel.IsOverageAllowed = entity.IsOverageAllowed;
            //viewModel.OverageAllowedPercent = entity.OverageAllowedAmount.GetPreciseValue(6);
            //viewModel.OrderTypeId = entity.OrderTypeId;

            //viewModel.CreatedBy = entity.CreatedBy;
            //viewModel.CreatedDate = entity.CreatedDate;
            //viewModel.TerminalId = entity.TerminalId;
            //viewModel.FuelPricing.TerminalId = entity.TerminalId;
            //viewModel.Comment = entity.Comment;
            //if (entity.PricingTypeId == (int)PricingType.Tier)
            //{
            //    viewModel.DifferentFuelPrices = entity.DifferentFuelPrices.OrderBy(t => t.MinQuantity).Select(t => t.ToViewModel()).ToList();
            //}
            //viewModel.FuelPricing.ExchangeRate = entity.ExchangeRate;
            viewModel.FuelPricing.Currency = entity.Currency;
            viewModel.FuelQuantity.UoM = entity.UoM;

            return viewModel;
        }
    }
}
