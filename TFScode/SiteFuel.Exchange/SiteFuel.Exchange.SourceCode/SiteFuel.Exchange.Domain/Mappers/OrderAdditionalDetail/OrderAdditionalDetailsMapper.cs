using Newtonsoft.Json;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class OrderAdditionalDetailsMapper
    {

        public static OrderAdditionalDetail ToOrderAdditionalDetailsEntityForTPO(this ThirdPartyOrderViewModel viewModel, OrderAdditionalDetail entity = null)
        {
            if (entity == null)
                entity = new OrderAdditionalDetail();

            entity.BOLInvoicePreferenceId = (int)viewModel.OrderAdditionalDetailsViewModel.BOLInvoicePreferenceTypes;
            entity.Allowance = viewModel.OrderAdditionalDetailsViewModel.Allowance;
            entity.IsDriverToUpdateBOL = viewModel.OrderAdditionalDetailsViewModel.IsDriverToUpdateBOL;
            entity.SupplierSourceId = viewModel.OrderAdditionalDetailsViewModel.SupplierSource.Id;
            entity.SupplierContract = viewModel.OrderAdditionalDetailsViewModel.SupplierSource.ContractNumber;
            entity.LoadCode = viewModel.OrderAdditionalDetailsViewModel.LoadCode;
            entity.Notes = viewModel.OrderAdditionalDetailsViewModel.Notes;
            entity.DRNotes = viewModel.OrderAdditionalDetailsViewModel.DRNotes;
            entity.IsFuelSurcharge = viewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge;
            //entity.LocationManagedType = viewModel.OrderAdditionalDetailsViewModel.LocationManagedType;
            // entity.SupplierAssignedProductName = viewModel.OrderAdditionalDetailsViewModel.SupplierAssignedProductName;
            if (viewModel.OrderAdditionalDetailsViewModel.IsFuelSurcharge)
                entity.FuelSurchagePricingType = (int)viewModel.OrderAdditionalDetailsViewModel.FuelSurchagePricingType.Value;
            entity.IsIncludePricingInExternalObj = viewModel.OrderAdditionalDetailsViewModel.IsIncludePricing;

            if (!viewModel.IsSupressOrderPricing)
                if (viewModel.FuelDetails.FuelDisplayGroupId != (int)ProductDisplayGroups.OtherFuelType && entity.BOLInvoicePreferenceId == (int)InvoiceNotificationPreferenceTypes.SendPDIDeliveryDetails)
                    entity.IsPDITaxRequired = viewModel.OrderAdditionalDetailsViewModel.IsPdiTaxRequired;

            entity.FreightPricingMethod = viewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod;
            entity.Berth = viewModel.OrderAdditionalDetailsViewModel.Berth;
            entity.InternationalWatersType = viewModel.OrderAdditionalDetailsViewModel.DestinedForInternationalWaters;
            entity.IsManualBDNConfirmationRequired = viewModel.OrderAdditionalDetailsViewModel.IsManualBDNConfirmationRequired;
            entity.IsManualInvoiceConfirmationRequired = viewModel.OrderAdditionalDetailsViewModel.IsManualInvoiceConfirmationRequired;
            entity.IsSupressPricingEnabled = viewModel.IsSupressOrderPricing;
            entity.IsFreightCost = viewModel.OrderAdditionalDetailsViewModel.IsFreightCost;
            if (viewModel.OrderAdditionalDetailsViewModel.FreightRateRuleId.HasValue && viewModel.OrderAdditionalDetailsViewModel.FreightRateRuleId.Value > 0)
            {
                entity.FreightRateRuleType = viewModel.OrderAdditionalDetailsViewModel.FreightRateRuleType;
                entity.FreightRateTableType = viewModel.OrderAdditionalDetailsViewModel.FreightRateTableType;
                entity.FreightRateRuleId = viewModel.OrderAdditionalDetailsViewModel.FreightRateRuleId;
            }
            if (viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableId.HasValue && viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableId.Value > 0)
            {
                entity.FuelSurchargeTableType = viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableType;
                entity.FuelSurchargeTableId = viewModel.OrderAdditionalDetailsViewModel.FuelSurchargeTableId;
            }
            if (viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeId.HasValue && viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeId.Value > 0)
            {
                entity.AccessorialFeeTableType = viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeTableType;
                entity.AccessorialFeeId = viewModel.OrderAdditionalDetailsViewModel.AccessorialFeeId;
            }
            return entity;
        }

        public static OrderAdditionalDetail ToOrderAdditionalDetailsEntityForChooseOrder(this Order selectedOrder, OrderAdditionalDetail entity = null)
        {
            if (entity == null)
                entity = new OrderAdditionalDetail();

            bool isNullEntity = (selectedOrder != null && selectedOrder.OrderAdditionalDetail != null) ? false : true;
            entity.BOLInvoicePreferenceId = !isNullEntity ? selectedOrder.OrderAdditionalDetail.BOLInvoicePreferenceId : (int)InvoiceNotificationPreferenceTypes.None;
            entity.Allowance = !isNullEntity ? selectedOrder.OrderAdditionalDetail.Allowance : null;
            entity.IsDriverToUpdateBOL = !isNullEntity ? selectedOrder.OrderAdditionalDetail.IsDriverToUpdateBOL : false;
            entity.SupplierSourceId = !isNullEntity ? selectedOrder.OrderAdditionalDetail.SupplierSourceId : null;
            entity.SupplierContract = !isNullEntity ? selectedOrder.OrderAdditionalDetail.SupplierContract : null;
            entity.LoadCode = !isNullEntity ? selectedOrder.OrderAdditionalDetail.LoadCode : null;
            entity.Notes = !isNullEntity ? selectedOrder.OrderAdditionalDetail.Notes : null;
            entity.DRNotes = !isNullEntity ? selectedOrder.OrderAdditionalDetail.DRNotes : null;
            entity.IsFuelSurcharge = !isNullEntity ? selectedOrder.OrderAdditionalDetail.IsFuelSurcharge : false;
            if (entity.IsFuelSurcharge)
                entity.FuelSurchagePricingType = !isNullEntity ? selectedOrder.OrderAdditionalDetail.FuelSurchagePricingType : null;
            entity.IsIncludePricingInExternalObj = !isNullEntity ? selectedOrder.OrderAdditionalDetail.IsIncludePricingInExternalObj : false;

            entity.FreightPricingMethod = !isNullEntity ? selectedOrder.OrderAdditionalDetail.FreightPricingMethod : FreightPricingMethod.Manual;
            entity.Berth = !isNullEntity ? selectedOrder.OrderAdditionalDetail.Berth : null;
            entity.InternationalWatersType = !isNullEntity ? selectedOrder.OrderAdditionalDetail.InternationalWatersType : DestinedForInternationalWaters.Unknown;
            entity.IsManualBDNConfirmationRequired = !isNullEntity ? selectedOrder.OrderAdditionalDetail.IsManualBDNConfirmationRequired : false;
            entity.IsManualInvoiceConfirmationRequired = !isNullEntity ? selectedOrder.OrderAdditionalDetail.IsManualInvoiceConfirmationRequired : false;
            entity.IsSupressPricingEnabled = !isNullEntity ? selectedOrder.OrderAdditionalDetail.IsSupressPricingEnabled : false;
            return entity;
        }



    }
}
