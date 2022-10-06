using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingRequestGridMapper
    {
        public static SourcingRequestGridViewModel ToGridViewModel(this UspGetSoucingRequest entity, SourcingRequestGridViewModel viewModel = null)
        {
            var helperDomain = new HelperDomain();
            if (viewModel == null)
                viewModel = new SourcingRequestGridViewModel();
            viewModel.Id = entity.Id;
            viewModel.RequestNumber = entity.RequestNumber;
            viewModel.JobName = entity.JobName;
            viewModel.FuelType = entity.FuelType;
            if (entity.Quantity != ApplicationConstants.QuantityNotSpecified)
            {
                viewModel.Quantity = helperDomain.GetQuantityRequested(entity.Quantity) + " " + helperDomain.GetUOM(entity.UOM);
            }
            else
            {
                viewModel.Quantity = helperDomain.GetQuantityRequested(entity.Quantity);
            }
            if (entity.DeliveryDate != null)
            {
                viewModel.DeliveryDate = entity.DeliveryDate.Value.Date.ToString("MM-dd-yyyy");
            }
            else
            {
                viewModel.DeliveryDate = string.Empty;
            }
            viewModel.DeliveryType = entity.DeliveryType;
            viewModel.Status = EnumHelperMethods.GetDisplayName((SourcingRequestStatus)entity.Status);
            viewModel.ModifiedBy = entity.ModifiedBy;
            viewModel.ViewedModified = entity.ViewedModified;
            viewModel.PricePerGallon = entity.PricePerGallon ?? 0;
            viewModel.PricingTypeId = entity.PricingTypeId;
            viewModel.RackAvgTypeId = entity.RackAvgTypeId;
            viewModel.RackPrice = entity.RackPrice ?? 0;
            viewModel.SupplierCostMarkupValue = entity.SupplierCostMarkupValue ?? 0;
            viewModel.SupplierCostMarkupTypeId = entity.SupplierCostMarkupTypeId;
            if (entity.PricingTypeId == (int)PricingType.RackAverage)
            {
                viewModel.MarkertBasedPricingTypeId = (int)PricingType.RackAverage;
            }
            else if (entity.PricingTypeId == (int)PricingType.RackHigh)
            {
                viewModel.MarkertBasedPricingTypeId = (int)PricingType.RackHigh;
            }
            else if (entity.PricingTypeId == (int)PricingType.RackLow)
            {
                viewModel.MarkertBasedPricingTypeId = (int)PricingType.RackLow;
            }
            viewModel.Pricing = viewModel.SetPricePerGallon();

            return viewModel;
        }
        public static UspGetSoucingRequest ToViewModel(this List<UspGetSoucingRequest> entity, UspGetSoucingRequest viewModel = null)
        {
            if (viewModel == null)
                viewModel = new UspGetSoucingRequest();
            if (entity !=null)
            {
                foreach (var sourcingRequest in entity)
                {
                    viewModel.Id = sourcingRequest.Id;
                    viewModel.RequestNumber = sourcingRequest.RequestNumber;
                    viewModel.JobName = sourcingRequest.JobName;
                    viewModel.FuelType = sourcingRequest.FuelType;
                    viewModel.Quantity = sourcingRequest.Quantity;
                    viewModel.DeliveryType = sourcingRequest.DeliveryType;
                    viewModel.DeliveryDate = sourcingRequest.DeliveryDate;
                    viewModel.Status = sourcingRequest.Status;
                    viewModel.ModifiedBy = sourcingRequest.ModifiedBy;
                    viewModel.ViewedModified = sourcingRequest.ViewedModified;
                    viewModel.PricePerGallon = sourcingRequest.PricePerGallon;
                    viewModel.PricingTypeId = sourcingRequest.PricingTypeId;
                    viewModel.RackAvgTypeId = sourcingRequest.RackAvgTypeId;
                    viewModel.RackPrice = sourcingRequest.RackPrice;
                    viewModel.SupplierCostMarkupValue = sourcingRequest.SupplierCostMarkupValue;
                    viewModel.SupplierCostMarkupTypeId = sourcingRequest.SupplierCostMarkupTypeId;
                    viewModel.UOM = sourcingRequest.UOM;
                }
            }
            return viewModel;
        }
       
    }
}
