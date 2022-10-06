using Newtonsoft.Json;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class FuelDeliveryDetailMapper
    {
        public static FuelRequestDetail ToEntity(this FuelDeliveryDetailsViewModel viewModel, FuelRequestDetail entity = null)
        {
            if (entity == null)
                entity = new FuelRequestDetail();

            entity.FuelRequestId = viewModel.FuelRequestId;
            entity.DeliveryTypeId = viewModel.DeliveryTypeId;
            entity.StartDate = viewModel.StartDate;
            entity.EndDate = viewModel.EndDate;
            entity.StartTime = Convert.ToDateTime(viewModel.StartTime).TimeOfDay;
            entity.EndTime = Convert.ToDateTime(viewModel.EndTime).TimeOfDay;
            entity.PoContactId = viewModel.PoContactId;
            entity.IsDispatchRetainedByCustomer = viewModel.IsDispatchRetainedByCustomer;
            entity.PaymentMethod = viewModel.PaymentMethods;
            if (viewModel.CustomAttributeViewModel != null)
            entity.CustomAttribute = new FuelRequestCustomAttributeViewModel { WBSNumber = viewModel.CustomAttributeViewModel.WBSNumber }.ToString();
            entity.IsBolImageRequired = viewModel.IsBolImageRequired;
            entity.IsDropImageRequired = viewModel.IsDropImageRequired;
            entity.IsDriverToUpdateBOL = viewModel.IsDriverToUpdateBOL;
            entity.PricingQuantityIndicatorTypeId = viewModel.PricingQuantityIndicatorTypeId;
            entity.TruckLoadTypeId = (int)viewModel.TruckLoadTypes;
            entity.OrderEnforcementId = (OrderEnforcement)viewModel.OrderEnforcementId;
            entity.IsPrePostDipRequired = viewModel.IsPrePostDipRequired;

            return entity;
        }

        public static FuelDeliveryDetailsViewModel ToViewModel(this FuelRequestDetail entity, FuelDeliveryDetailsViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new FuelDeliveryDetailsViewModel(Status.Success);

            viewModel.FuelRequestId = entity.FuelRequestId;
            viewModel.DeliveryTypeId = entity.DeliveryTypeId;
            viewModel.DeliveryTypeName = entity.MstDeliveryType.Name;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.StartTime = Convert.ToDateTime(entity.StartTime.ToString()).ToShortTimeString();
            viewModel.EndTime = Convert.ToDateTime(entity.EndTime.ToString()).ToShortTimeString();
            viewModel.PoContactId = entity.PoContactId;
            viewModel.IsOrderEndDateRequired = viewModel.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && entity.EndDate.HasValue;
            viewModel.PaymentMethods = entity.PaymentMethod;
            if (!string.IsNullOrWhiteSpace(entity.CustomAttribute))
            {
                viewModel.CustomAttributeViewModel = JsonConvert.DeserializeObject<FuelRequestCustomAttributeViewModel>(entity.CustomAttribute);
            }
            viewModel.CustomAttribute = entity.CustomAttribute;
            viewModel.IsBolImageRequired = entity.IsBolImageRequired;
            viewModel.IsDropImageRequired = entity.IsDropImageRequired;
            viewModel.IsDriverToUpdateBOL = entity.IsDriverToUpdateBOL;
            viewModel.PricingQuantityIndicatorTypeId = entity.PricingQuantityIndicatorTypeId;
            viewModel.TruckLoadTypes = (TruckLoadTypes)entity.TruckLoadTypeId;
            viewModel.OrderEnforcementId = entity.OrderEnforcementId;
            viewModel.IsPrePostDipRequired = entity.IsPrePostDipRequired;
            return viewModel;
        }

        public static FuelDeliveryDetailsViewModel ToDeliveryDetailViewModel(this UspGetSupplierOrderDetail entity)
        {
            FuelDeliveryDetailsViewModel viewModel = new FuelDeliveryDetailsViewModel(Status.Success);

            viewModel.FuelRequestId = entity.FuelRequestId;
            viewModel.DeliveryTypeId = entity.DeliveryTypeId;
            viewModel.DeliveryTypeName = entity.DeliveryTypeName;
            viewModel.StartDate = entity.StartDate;
            viewModel.EndDate = entity.EndDate;
            viewModel.StartTime = Convert.ToDateTime(entity.StartTime.ToString()).ToShortTimeString();
            viewModel.EndTime = Convert.ToDateTime(entity.EndTime.ToString()).ToShortTimeString();
            viewModel.PoContactId = entity.PoContactId;
            viewModel.IsOrderEndDateRequired = viewModel.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && entity.EndDate.HasValue;
            if (!string.IsNullOrWhiteSpace(entity.CustomAttribute))
            {
                viewModel.CustomAttributeViewModel = JsonConvert.DeserializeObject<FuelRequestCustomAttributeViewModel>(entity.CustomAttribute);
            }
            viewModel.CustomAttribute = entity.CustomAttribute;
            viewModel.IsBolImageRequired = entity.IsBolImageRequired;
            viewModel.IsDropImageRequired = entity.IsDropImageRequired;
            viewModel.IsDriverToUpdateBOL = entity.FrIsDriverToUpdateBOL;
            viewModel.PricingQuantityIndicatorTypeId = entity.PricingQuantityIndicatorTypeId;
            viewModel.TruckLoadTypes = (TruckLoadTypes)entity.TruckLoadTypeId;
            viewModel.OrderEnforcementId = (OrderEnforcement)entity.OrderEnforcementId;
            viewModel.IsPrePostDipRequired = entity.IsPrePostDipRequired;
            viewModel.IsDispatchRetainedByCustomer = entity.IsDispatchRetainedByCustomer;
            return viewModel;
        }

        public static FuelRequestDetail ToDeliveryDetailsEntityForTPO(this ThirdPartyOrderViewModel viewModel, FuelRequestDetail entity = null)
        {
            if (entity == null)
                entity = new FuelRequestDetail();

            entity.FuelRequestId = viewModel.FuelRequestId;
            entity.DeliveryTypeId = viewModel.FuelDeliveryDetails.DeliveryTypeId;
            entity.StartDate = viewModel.FuelDeliveryDetails.StartDate;
            entity.EndDate = viewModel.FuelDeliveryDetails.EndDate;

            entity.StartTime = Convert.ToDateTime(viewModel.FuelDeliveryDetails.StartTime).TimeOfDay;
            entity.EndTime = Convert.ToDateTime(viewModel.FuelDeliveryDetails.EndTime).TimeOfDay;
            entity.PaymentMethod = viewModel.FuelDeliveryDetails.PaymentMethods;
            if (viewModel.FuelDeliveryDetails.CustomAttributeViewModel != null)
                entity.CustomAttribute = new FuelRequestCustomAttributeViewModel { WBSNumber = viewModel.FuelDeliveryDetails.CustomAttributeViewModel.WBSNumber }.ToString();
            entity.IsBolImageRequired = viewModel.FuelDeliveryDetails.IsBolImageRequired;
            entity.IsDropImageRequired = viewModel.FuelDeliveryDetails.IsDropImageRequired;
            entity.IsDriverToUpdateBOL = viewModel.FuelDeliveryDetails.IsDriverToUpdateBOL;
            entity.PricingQuantityIndicatorTypeId = viewModel.FuelDeliveryDetails.PricingQuantityIndicatorTypeId;
            entity.TruckLoadTypeId = (int)viewModel.FuelDeliveryDetails.TruckLoadTypes;
            entity.OrderEnforcementId = (OrderEnforcement)viewModel.FuelDeliveryDetails.OrderEnforcementId;
            entity.IsPrePostDipRequired = viewModel.FuelDeliveryDetails.IsPrePostDipRequired;

            return entity;
        }
    }
}
