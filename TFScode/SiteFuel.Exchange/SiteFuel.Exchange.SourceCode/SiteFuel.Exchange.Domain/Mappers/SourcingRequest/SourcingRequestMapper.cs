using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingRequestMapper
    {
        public static LeadRequest ToEntity(this SourcingRequestViewModel viewModel, LeadRequest entity = null)
        {
            if (entity == null)
            {
                entity = new LeadRequest();
            }

            entity.DisplayRequestID = viewModel.DisplayRequestId;
            entity.TruckLoadType = viewModel.TruckLoadType;
            entity.FreightOnBoardType = viewModel.FreightOnBoardType;
            entity.AccountingCompanyId = viewModel.AccountingCompanyId;
            entity.Name = viewModel.RequestName;
            entity.IsActive = true;
            entity.Status = SourcingRequestStatus.Submitted;
            entity.CreatedDate = DateTimeOffset.Now;
            entity.UpdatedDate = DateTimeOffset.Now;
            entity.Version = 1;

            LeadAddressDetail leadAddressDetail = new LeadAddressDetail()
            {
                Address = viewModel.AddressDetails.Address,
                City = viewModel.AddressDetails.City,
                CountryId = viewModel.AddressDetails.CountryId,
                CountyName = viewModel.AddressDetails.CountyName ?? "",
                Currency = viewModel.AddressDetails.Currency,
                DisplayJobID = viewModel.AddressDetails.DisplayJobID,
                InventoryDataCaptureType = viewModel.AddressDetails.InventoryDataCaptureType,
                IsGeocodeUsed = viewModel.AddressDetails.IsGeocodeUsed,
                IsMarineLocation = viewModel.AddressDetails.IsMarineLocation,
                IsProFormaPoEnabled = viewModel.AddressDetails.IsProFormaPoEnabled,
                IsRetailJob = viewModel.AddressDetails.IsRetailJob,
                JobId = viewModel.AddressDetails.JobId.Value > 0 ? viewModel.AddressDetails.JobId.Value : 0,
                JobName = viewModel.AddressDetails.JobName,
                Latitude = viewModel.AddressDetails.Latitude,
                Longitude = viewModel.AddressDetails.Longitude,
                LocationManagedType = viewModel.AddressDetails.LocationManagedType,
                SignatureEnabled = viewModel.AddressDetails.SignatureEnabled,
                StateId = viewModel.AddressDetails.StateId,
                TimeZoneName = viewModel.AddressDetails.TimeZoneName,
                UOM = viewModel.AddressDetails.UoM,
                ZipCode = viewModel.AddressDetails.ZipCode
            };
            entity.LeadAddressDetail.Add(leadAddressDetail);

            LeadCustomerInformation leadCustomerInformation = new LeadCustomerInformation()
            {
                CompanyId = viewModel.CustomerDetails.CompanyId.Value > 0 ? viewModel.CustomerDetails.CompanyId.Value : 0,
                CompanyName = viewModel.CustomerDetails.CompanyName,
                Email = viewModel.CustomerDetails.Email,
                IsInvitationEnabled = viewModel.CustomerDetails.IsInvitationEnabled,
                IsNotifyDeliveries = viewModel.CustomerDetails.IsNotifyDeliveries,
                IsNotifySchedules = viewModel.CustomerDetails.IsNotifySchedules,
                Name = viewModel.CustomerDetails.Name,
                UserId = viewModel.CustomerDetails.UserId.Value > 0 ? viewModel.CustomerDetails.UserId.Value : 0
            };
            entity.LeadCustomerInformations.Add(leadCustomerInformation);

            LeadFuelDeliveryDetail leadFuelDeliveryDetail = new LeadFuelDeliveryDetail()
            {
                DeliveryTypeId = viewModel.FuelDeliveryDetails.DeliveryTypeId,
                EndTime = Convert.ToDateTime(viewModel.FuelDeliveryDetails.EndTime).TimeOfDay,
                EndDate = viewModel.FuelDeliveryDetails.EndDate,
                IsPrePostDipRequired = viewModel.FuelDeliveryDetails.IsPrePostDipRequired,
                NetDays = viewModel.FuelDeliveryDetails.NetDays,
                OrderEnforcementId = viewModel.FuelDeliveryDetails.OrderEnforcementId,
                PaymentMethods = viewModel.FuelDeliveryDetails.PaymentMethods,
                PaymentTermId = viewModel.FuelDeliveryDetails.PaymentTermId,
                SingleDeliverySubTypes = viewModel.FuelDeliveryDetails.SingleDeliverySubTypes,
                StartDate = viewModel.FuelDeliveryDetails.StartDate,
                StartTime = Convert.ToDateTime(viewModel.FuelDeliveryDetails.StartTime).TimeOfDay
            };

            entity.LeadFuelDeliveryDetails.Add(leadFuelDeliveryDetail);

            LeadAdditionalDetail leadAdditionalDetail = new LeadAdditionalDetail()
            {
                IsAssetDropStatusEnabled = viewModel.AdditionalDetailsViewModel.IsAssetDropStatusEnabled,
                IsAssetTracked = viewModel.AdditionalDetailsViewModel.IsAssetTracked
            };

            entity.LeadAdditionalDetail.Add(leadAdditionalDetail);

            LeadFuelDetail leadFuelDetail = new LeadFuelDetail()
            {
                FuelDisplayGroupId = viewModel.FuelDetails.FuelDisplayGroupId,
                FuelTypeId = viewModel.FuelDetails.FuelTypeId.Value > 0 ? viewModel.FuelDetails.FuelTypeId.Value : 0,
                IsTierPricing = viewModel.FuelDetails.IsTierPricing,
                MaximumQuantity = viewModel.FuelDetails.MaximumQuantity,
                MinimumQuantity = viewModel.FuelDetails.MinimumQuantity,
                NonStandardFuelDescription = viewModel.FuelDetails.NonStandardFuelDescription,
                NonStandardFuelName = viewModel.FuelDetails.NonStandardFuelName,
                PricePerGallon = viewModel.FuelDetails.PricePerGallon,
                PricingTypeId = viewModel.FuelDetails.PricingTypeId,
                Quantity = viewModel.FuelDetails.Quantity,
                QuantityIndicatorTypes = viewModel.FuelDetails.QuantityIndicatorTypes,
                QuantityTypeId = viewModel.FuelDetails.QuantityTypeId
            };

            if (viewModel.FuelDetails.FuelRequestFees != null && viewModel.FuelDetails.FuelRequestFees.Any())
            {
                foreach (var item in viewModel.FuelDetails.FuelRequestFees)
                {
                    LeadFuelFee leadFuelFee = new LeadFuelFee()
                    {
                        Fee = item.Fee.Value > 0 ? item.Fee.Value : 0,
                        FeeConstraintTypeId = item.FeeConstraintTypeId,
                        FeeSubTypeId = item.FeeSubTypeId,
                        FeeTypeId = item.FeeTypeId,
                        IncludeInPPG = item.IncludeInPPG
                    };
                    leadFuelDetail.LeadFuelFees.Add(leadFuelFee);
                }
            }
            entity.LeadFuelDetails.Add(leadFuelDetail);

            return entity;
        }
    }
}
