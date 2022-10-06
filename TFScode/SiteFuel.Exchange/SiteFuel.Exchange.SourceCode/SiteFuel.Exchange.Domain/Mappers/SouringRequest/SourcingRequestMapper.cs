using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using static SiteFuel.Exchange.ViewModels.SourcingDetailViewModel;

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
            LeadAddressDetail leadAddressDetail = new LeadAddressDetail()
            {
                Address = viewModel.AddressDetails.Address,
                City = viewModel.AddressDetails.City,
                CountryId = viewModel.AddressDetails.CountryId,
                CountyName = viewModel.AddressDetails.CountyName,
                Currency = viewModel.AddressDetails.Currency,
                DisplayJobID = viewModel.AddressDetails.DisplayJobID,
                InventoryDataCaptureType = viewModel.AddressDetails.InventoryDataCaptureType,
                IsGeocodeUsed = viewModel.AddressDetails.IsGeocodeUsed,
                IsMarineLocation = viewModel.AddressDetails.IsMarineLocation,
                IsProFormaPoEnabled = viewModel.AddressDetails.IsProFormaPoEnabled,
                IsRetailJob = viewModel.AddressDetails.IsRetailJob,
                JobId = viewModel.AddressDetails.JobId.HasValue ? viewModel.AddressDetails.JobId.Value : 0,
                JobName = viewModel.AddressDetails.JobName,
                Latitude = Convert.ToDecimal(viewModel.AddressDetails.Latitude),
                Longitude = Convert.ToDecimal(viewModel.AddressDetails.Longitude),
                LocationManagedType = viewModel.AddressDetails.LocationManagedType,
                SignatureEnabled = viewModel.AddressDetails.SignatureEnabled,
                StateId = viewModel.AddressDetails.StateId,
                TimeZoneName = viewModel.AddressDetails.TimeZoneName,
                UOM = viewModel.AddressDetails.UOM,
                ZipCode = viewModel.AddressDetails.ZipCode,
                DispatchRegionId = viewModel.AddressDetails.DispatchRegionId
            };
            entity.LeadAddressDetail.Add(leadAddressDetail);

            LeadCustomerInformation leadCustomerInformation = new LeadCustomerInformation()
            {
                CompanyId = viewModel.CustomerDetails.CompanyId.HasValue ? viewModel.CustomerDetails.CompanyId.Value : 0,
                CompanyName = viewModel.CustomerDetails.CompanyName,
                Email = viewModel.CustomerDetails.Email,
                PhoneNumber = viewModel.CustomerDetails.PhoneNumber,
                IsInvitationEnabled = viewModel.CustomerDetails.IsInvitationEnabled,
                IsNotifyDeliveries = viewModel.CustomerDetails.IsNotifyDeliveries,
                IsNotifySchedules = viewModel.CustomerDetails.IsNotifySchedules,
                Name = viewModel.CustomerDetails.Name,
                UserId = viewModel.CustomerDetails.UserId.HasValue ? viewModel.CustomerDetails.UserId.Value : 0

            };
            entity.LeadCustomerInformations.Add(leadCustomerInformation);
            foreach (var contactperson in viewModel.CustomerDetails.ContactPersons)
            {
                LeadCustomerInformation customerInformation = new LeadCustomerInformation()
                {
                    CompanyId = viewModel.CustomerDetails.CompanyId.HasValue ? viewModel.CustomerDetails.CompanyId.Value : 0,
                    CompanyName = viewModel.CustomerDetails.CompanyName,
                    Email = contactperson.Email,
                    PhoneNumber = contactperson.PhoneNumber,
                    IsInvitationEnabled = viewModel.CustomerDetails.IsInvitationEnabled,
                    IsNotifyDeliveries = viewModel.CustomerDetails.IsNotifyDeliveries,
                    IsNotifySchedules = viewModel.CustomerDetails.IsNotifySchedules,
                    Name = contactperson.Name,
                    UserId = contactperson.Id
                };
                entity.LeadCustomerInformations.Add(customerInformation);
            }
            LeadFuelDeliveryDetail leadFuelDeliveryDetail = new LeadFuelDeliveryDetail()
            {
                DeliveryTypeId = viewModel.FuelDeliveryDetails.DeliveryTypeId,
                EndTime = viewModel.FuelDeliveryDetails.EndTime,
                EndDate = !string.IsNullOrEmpty(viewModel.FuelDeliveryDetails.EndDate) ? Convert.ToDateTime(viewModel.FuelDeliveryDetails.EndDate) : (DateTime?)null,
                IsPrePostDipRequired = viewModel.FuelDeliveryDetails.IsPrePostDipRequired,
                NetDays = viewModel.FuelDeliveryDetails.NetDays,
                OrderEnforcementId = viewModel.FuelDeliveryDetails.OrderEnforcementId,
                PaymentMethods = viewModel.FuelDeliveryDetails.PaymentMethods,
                PaymentTermId = viewModel.FuelDeliveryDetails.PaymentTermId,
                SingleDeliverySubTypes = viewModel.FuelDeliveryDetails.SingleDeliverySubTypes,
                StartDate = !string.IsNullOrEmpty(viewModel.FuelDeliveryDetails.StartDate) ? Convert.ToDateTime(viewModel.FuelDeliveryDetails.StartDate) : (DateTime?)null,
                StartTime = viewModel.FuelDeliveryDetails.StartTime
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
                FuelTypeId = viewModel.FuelDetails.FuelTypeId.HasValue ? viewModel.FuelDetails.FuelTypeId.Value : 0,
                IsTierPricing = viewModel.FuelDetails.IsTierPricing,
                MaximumQuantity = viewModel.FuelDetails.MaximumQuantity,
                MinimumQuantity = viewModel.FuelDetails.MinimumQuantity,
                NonStandardFuelDescription = viewModel.FuelDetails.NonStandardFuelDescription,
                NonStandardFuelName = viewModel.FuelDetails.NonStandardFuelName,
                PricePerGallon = viewModel.FuelDetails.PricePerGallon,
                PricingTypeId = viewModel.FuelDetails.PricingTypeId,
                Quantity = viewModel.FuelDetails.QuantityTypeId == (int)QuantityType.NotSpecified ? ApplicationConstants.QuantityNotSpecified : viewModel.FuelDetails.Quantity,
                QuantityIndicatorTypes = viewModel.FuelDetails.QuantityIndicatorTypes,
                QuantityTypeId = viewModel.FuelDetails.QuantityTypeId
            };
            entity.LeadFuelDetails.Add(leadFuelDetail);


            //if (!viewModel.FuelPricingDetails.IsTierPricingRequired)
            //{
            //    LeadPricingDetail leadPricingDetail = new LeadPricingDetail()
            //    {
            //        EnableCityRack = viewModel.FuelPricingDetails.EnableCityRack,
            //        RackAvgTypeId = viewModel.FuelPricingDetails.RackAvgTypeId,
            //        RackPrice = viewModel.FuelPricingDetails.RackPrice,
            //        SupplierCostMarkupTypeId = viewModel.FuelPricingDetails.SupplierCostMarkupTypeId,
            //        SupplierCostMarkupValue = viewModel.FuelPricingDetails.SupplierCostMarkupValue,
            //        TerminalId = viewModel.FuelPricingDetails.TerminalId,
            //        TerminalName = viewModel.FuelPricingDetails.TerminalName,
            //        PricingTypeId = viewModel.FuelPricingDetails.PricingTypeId,
            //        PricePerGallon = viewModel.FuelPricingDetails.PricePerGallon,
            //        CityGroupTerminalId = viewModel.FuelPricingDetails.CityGroupTerminalId,
            //        CityGroupTerminalName = viewModel.FuelPricingDetails.CityGroupTerminalName,
            //        CityGroupTerminalStateId = viewModel.FuelPricingDetails.CityGroupTerminalStateId,
            //        CodeDescription = viewModel.FuelPricingDetails.CodeDescription,
            //        CodeId = viewModel.FuelPricingDetails.CodeId,
            //        Code = viewModel.FuelPricingDetails.Code

            //    };
            //    entity.LeadPricingDetails.Add(leadPricingDetail);
            //}
            return entity;
        }
        public static LeadRequest ToUpdateEntity(this SourcingRequestViewModel viewModel, LeadRequest entity = null)
        {
            entity.DisplayRequestID = viewModel.DisplayRequestId;
            entity.TruckLoadType = viewModel.TruckLoadType;
            entity.FreightOnBoardType = viewModel.FreightOnBoardType;
            entity.AccountingCompanyId = viewModel.AccountingCompanyId;
            entity.Name = viewModel.RequestName;
            if (entity.LeadAddressDetail != null && entity.LeadAddressDetail.Any())
            {
                var leadAddressDetail = entity.LeadAddressDetail.First();
                leadAddressDetail.Address = viewModel.AddressDetails.Address;
                leadAddressDetail.City = viewModel.AddressDetails.City;
                leadAddressDetail.CountryId = viewModel.AddressDetails.CountryId;
                leadAddressDetail.CountyName = viewModel.AddressDetails.CountyName;
                leadAddressDetail.Currency = viewModel.AddressDetails.Currency;
                leadAddressDetail.DisplayJobID = viewModel.AddressDetails.DisplayJobID;
                leadAddressDetail.InventoryDataCaptureType = viewModel.AddressDetails.InventoryDataCaptureType;
                leadAddressDetail.IsGeocodeUsed = viewModel.AddressDetails.IsGeocodeUsed;
                leadAddressDetail.IsMarineLocation = viewModel.AddressDetails.IsMarineLocation;
                leadAddressDetail.IsProFormaPoEnabled = viewModel.AddressDetails.IsProFormaPoEnabled;
                leadAddressDetail.IsRetailJob = viewModel.AddressDetails.IsRetailJob;
                leadAddressDetail.JobId = viewModel.AddressDetails.JobId.HasValue ? viewModel.AddressDetails.JobId.Value : 0;
                leadAddressDetail.JobName = viewModel.AddressDetails.JobName;
                leadAddressDetail.Latitude = Convert.ToDecimal(viewModel.AddressDetails.Latitude);
                leadAddressDetail.Longitude = Convert.ToDecimal(viewModel.AddressDetails.Longitude);
                leadAddressDetail.LocationManagedType = viewModel.AddressDetails.LocationManagedType;
                leadAddressDetail.SignatureEnabled = viewModel.AddressDetails.SignatureEnabled;
                leadAddressDetail.StateId = viewModel.AddressDetails.StateId;
                leadAddressDetail.TimeZoneName = viewModel.AddressDetails.TimeZoneName;
                leadAddressDetail.UOM = viewModel.AddressDetails.UOM;
                leadAddressDetail.ZipCode = viewModel.AddressDetails.ZipCode;
                leadAddressDetail.DispatchRegionId = viewModel.AddressDetails.DispatchRegionId;
            }
            if (entity.LeadFuelDeliveryDetails != null && entity.LeadFuelDeliveryDetails.Any())
            {
                var leadFuelDeliveryDetail = entity.LeadFuelDeliveryDetails.First();
                leadFuelDeliveryDetail.DeliveryTypeId = viewModel.FuelDeliveryDetails.DeliveryTypeId;
                leadFuelDeliveryDetail.EndTime = viewModel.FuelDeliveryDetails.EndTime;
                leadFuelDeliveryDetail.EndDate = !string.IsNullOrEmpty(viewModel.FuelDeliveryDetails.EndDate) ? Convert.ToDateTime(viewModel.FuelDeliveryDetails.EndDate) : (DateTime?)null; ;
                leadFuelDeliveryDetail.IsPrePostDipRequired = viewModel.FuelDeliveryDetails.IsPrePostDipRequired;
                leadFuelDeliveryDetail.NetDays = viewModel.FuelDeliveryDetails.NetDays;
                leadFuelDeliveryDetail.OrderEnforcementId = viewModel.FuelDeliveryDetails.OrderEnforcementId;
                leadFuelDeliveryDetail.PaymentMethods = viewModel.FuelDeliveryDetails.PaymentMethods;
                leadFuelDeliveryDetail.PaymentTermId = viewModel.FuelDeliveryDetails.PaymentTermId;
                leadFuelDeliveryDetail.SingleDeliverySubTypes = viewModel.FuelDeliveryDetails.SingleDeliverySubTypes;
                leadFuelDeliveryDetail.StartDate = !string.IsNullOrEmpty(viewModel.FuelDeliveryDetails.StartDate) ? Convert.ToDateTime(viewModel.FuelDeliveryDetails.StartDate) : (DateTime?)null; ;
                leadFuelDeliveryDetail.StartTime = viewModel.FuelDeliveryDetails.StartTime;
            }

            if (entity.LeadAdditionalDetail != null && entity.LeadAdditionalDetail.Any())
            {
                var leadAdditionalDetail = entity.LeadAdditionalDetail.First();
                leadAdditionalDetail.IsAssetDropStatusEnabled = viewModel.AdditionalDetailsViewModel.IsAssetDropStatusEnabled;
                leadAdditionalDetail.IsAssetTracked = viewModel.AdditionalDetailsViewModel.IsAssetTracked;
            }
            if (entity.LeadFuelDetails != null && entity.LeadFuelDetails.Any())
            {
                var leadFuelDetail = entity.LeadFuelDetails.First();
                leadFuelDetail.FuelDisplayGroupId = viewModel.FuelDetails.FuelDisplayGroupId;
                leadFuelDetail.FuelTypeId = viewModel.FuelDetails.FuelTypeId.HasValue ? viewModel.FuelDetails.FuelTypeId.Value : 0;
                leadFuelDetail.IsTierPricing = viewModel.FuelDetails.IsTierPricing;
                leadFuelDetail.MaximumQuantity = viewModel.FuelDetails.MaximumQuantity;
                leadFuelDetail.MinimumQuantity = viewModel.FuelDetails.MinimumQuantity;
                leadFuelDetail.NonStandardFuelDescription = viewModel.FuelDetails.NonStandardFuelDescription;
                leadFuelDetail.NonStandardFuelName = viewModel.FuelDetails.NonStandardFuelName;
                leadFuelDetail.PricePerGallon = viewModel.FuelDetails.PricePerGallon;
                leadFuelDetail.PricingTypeId = viewModel.FuelDetails.PricingTypeId;
                leadFuelDetail.Quantity = viewModel.FuelDetails.QuantityTypeId == (int)QuantityType.NotSpecified ? ApplicationConstants.QuantityNotSpecified : viewModel.FuelDetails.Quantity;
                leadFuelDetail.QuantityIndicatorTypes = viewModel.FuelDetails.QuantityIndicatorTypes;
                leadFuelDetail.QuantityTypeId = viewModel.FuelDetails.QuantityTypeId;
            }

            return entity;
        }
        public static SourcingRequestViewModel ToViewModel(this LeadRequest entity, SourcingRequestViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new SourcingRequestViewModel();
            }
            viewModel.Id = entity.Id;
            viewModel.DisplayRequestId = entity.DisplayRequestID;
            viewModel.TruckLoadType = entity.TruckLoadType;
            viewModel.FreightOnBoardType = entity.FreightOnBoardType;
            viewModel.AccountingCompanyId = entity.AccountingCompanyId;
            viewModel.RequestName = entity.Name;
            viewModel.RequestStatus = entity.Status;
            viewModel.SalesUserId = entity.SalesUserId;

            if (entity.LeadAddressDetail != null && entity.LeadAddressDetail.Any())
            {
                var leadAddressDetail = entity.LeadAddressDetail.First();
                viewModel.AddressDetails = new SourcingAddressViewModel()
                {
                    Id = leadAddressDetail.Id,
                    Address = leadAddressDetail.Address,
                    City = leadAddressDetail.City,
                    CountryId = leadAddressDetail.CountryId,
                    CountyName = leadAddressDetail.CountyName,
                    Currency = leadAddressDetail.Currency,
                    DisplayJobID = leadAddressDetail.DisplayJobID,
                    InventoryDataCaptureType = leadAddressDetail.InventoryDataCaptureType,
                    IsGeocodeUsed = leadAddressDetail.IsGeocodeUsed,
                    IsMarineLocation = leadAddressDetail.IsMarineLocation,
                    IsProFormaPoEnabled = leadAddressDetail.IsProFormaPoEnabled,
                    IsRetailJob = leadAddressDetail.IsRetailJob,
                    JobId = leadAddressDetail.JobId,
                    JobName = leadAddressDetail.JobName,
                    Latitude = Convert.ToString(leadAddressDetail.Latitude),
                    Longitude = Convert.ToString(leadAddressDetail.Longitude),
                    LocationManagedType = leadAddressDetail.LocationManagedType,
                    SignatureEnabled = leadAddressDetail.SignatureEnabled,
                    StateId = leadAddressDetail.StateId,
                    TimeZoneName = leadAddressDetail.TimeZoneName,
                    UOM = leadAddressDetail.UOM,
                    ZipCode = leadAddressDetail.ZipCode,
                    IsNewJob = leadAddressDetail.JobId > 0 ? false : true,
                    DispatchRegionId = leadAddressDetail.DispatchRegionId
                };
            }

            if (entity.LeadCustomerInformations != null && entity.LeadCustomerInformations.Any())
            {
                var customerDetails = entity.LeadCustomerInformations.First();
                viewModel.CustomerDetails = new SourceCustomerViewModel()
                {
                    Id = customerDetails.Id,
                    CompanyId = customerDetails.CompanyId,
                    CompanyName = customerDetails.CompanyName,
                    Email = customerDetails.Email,
                    PhoneNumber = customerDetails.PhoneNumber,
                    IsInvitationEnabled = customerDetails.IsInvitationEnabled,
                    IsNotifyDeliveries = customerDetails.IsNotifyDeliveries,
                    IsNotifySchedules = customerDetails.IsNotifySchedules,
                    Name = customerDetails.Name,
                    UserId = customerDetails.UserId,
                    IsNewCompany = customerDetails.CompanyId > 0 ? false : true

                };
                foreach (var contactperson in entity.LeadCustomerInformations.Where(t => t.Id != customerDetails.Id))
                {
                    viewModel.CustomerDetails.ContactPersons.Add(
                    new ContactPersonViewModel()
                    {
                        Email = contactperson.Email,
                        PhoneNumber = contactperson.PhoneNumber,
                        Name = contactperson.Name,
                        Id = contactperson.Id
                    });
                }
            }
            if (entity.LeadFuelDeliveryDetails != null && entity.LeadFuelDeliveryDetails.Any())
            {
                var fuelDeliveryDetail = entity.LeadFuelDeliveryDetails.First();
                viewModel.FuelDeliveryDetails = new SourcingFuelDeliveryViewModel()
                {
                    Id = fuelDeliveryDetail.Id,
                    DeliveryTypeId = fuelDeliveryDetail.DeliveryTypeId,
                    EndTime = Convert.ToString(fuelDeliveryDetail.EndTime),
                    EndDate = fuelDeliveryDetail.EndDate != null ? Convert.ToDateTime(fuelDeliveryDetail.EndDate.ToString()).ToShortDateString() : string.Empty,
                    IsPrePostDipRequired = fuelDeliveryDetail.IsPrePostDipRequired,
                    NetDays = fuelDeliveryDetail.NetDays,
                    OrderEnforcementId = fuelDeliveryDetail.OrderEnforcementId,
                    PaymentMethods = fuelDeliveryDetail.PaymentMethods,
                    PaymentTermId = fuelDeliveryDetail.PaymentTermId,
                    SingleDeliverySubTypes = fuelDeliveryDetail.SingleDeliverySubTypes,
                    StartDate = Convert.ToDateTime(fuelDeliveryDetail.StartDate.ToString()).ToShortDateString(),
                    StartTime = Convert.ToString(fuelDeliveryDetail.StartTime)
                };
            }

            var additionalDetail = entity.LeadAdditionalDetail.First();
            viewModel.AdditionalDetailsViewModel = new SourcingAdditionalDetailsModel()
            {
                Id = additionalDetail.Id,
                IsAssetDropStatusEnabled = additionalDetail.IsAssetDropStatusEnabled,
                IsAssetTracked = additionalDetail.IsAssetTracked
            };

            if (entity.LeadFuelDetails != null && entity.LeadFuelDetails.Any())
            {
                var fuelDetail = entity.LeadFuelDetails.First();
                viewModel.FuelDetails = new SourcingFuelDetailsViewModel()
                {
                    Id = fuelDetail.Id,
                    FuelDisplayGroupId = fuelDetail.FuelDisplayGroupId,
                    FuelTypeId = fuelDetail.FuelTypeId,
                    IsTierPricing = fuelDetail.IsTierPricing,
                    MaximumQuantity = fuelDetail.MaximumQuantity,
                    MinimumQuantity = fuelDetail.MinimumQuantity,
                    NonStandardFuelDescription = fuelDetail.NonStandardFuelDescription,
                    NonStandardFuelName = fuelDetail.NonStandardFuelName,
                    PricePerGallon = fuelDetail.PricePerGallon,
                    PricingTypeId = fuelDetail.PricingTypeId,
                    Quantity = fuelDetail.Quantity,
                    QuantityIndicatorTypes = fuelDetail.QuantityIndicatorTypes,
                    QuantityTypeId = fuelDetail.QuantityTypeId
                };

            }
            if (entity.LeadRequestPriceDetails != null && entity.LeadRequestPriceDetails.Any())
            {
                var leadRequestPriceDetails = entity.LeadRequestPriceDetails.First();
                viewModel.FuelPricingDetails.TierPricing.RequestPriceDetailId = leadRequestPriceDetails.Id;
                viewModel.FuelPricingDetails.IsTierPricingRequired = leadRequestPriceDetails.IsTierPricingRequired;
                viewModel.FuelPricingDetails.PricingTypeId = leadRequestPriceDetails.PricingTypeId ?? 0;
                viewModel.FuelPricingDetails.TierPricing.TierPricingType = (TierPricingType)leadRequestPriceDetails.TierTypeId;
                viewModel.FuelPricingDetails.TierPricing.IsResetCumulation = leadRequestPriceDetails.CumulationTypeId > 0 ? true : false;
                viewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.CumulationType = leadRequestPriceDetails.CumulationTypeId.HasValue ? (CumulationType)leadRequestPriceDetails.CumulationTypeId : CumulationType.Unknown;
                viewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.Date = leadRequestPriceDetails.CumulationResetDate;
                viewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.Day = leadRequestPriceDetails.CumulationResetDay.HasValue && leadRequestPriceDetails.CumulationResetDay.Value > 0 ? (WeekDay)leadRequestPriceDetails.CumulationResetDay : WeekDay.Monday;
                viewModel.IsSupressOrderPricing = leadRequestPriceDetails.IsSuppressPricing;

                if (leadRequestPriceDetails.IsTierPricingRequired && leadRequestPriceDetails.LeadPricingDetails.Any())
                {
                    foreach (var pricing in leadRequestPriceDetails.LeadPricingDetails)
                    {
                        var helperDomain = new HelperDomain();
                        var pricingModel = new PricingViewModel();
                        pricingModel.PricingCode.Code = pricing.Code;
                        pricingModel.PricingCode.Id = pricing.CodeId;
                        pricingModel.PricingCode.Description = pricing.CodeDescription;
                        pricingModel.RackAvgTypeId = pricing.RackAvgTypeId;
                        pricingModel.RackPrice = pricing.RackPrice;
                        pricingModel.SupplierCostMarkupTypeId = pricing.SupplierCostMarkupTypeId;
                        pricingModel.SupplierCostMarkupValue = pricing.SupplierCostMarkupValue;
                        pricingModel.TerminalId = pricing.TerminalId;
                        pricingModel.TerminalName = pricing.TerminalName;
                        pricingModel.PricingTypeId = pricing.PricingTypeId;
                        pricingModel.PricePerGallon = pricing.PricePerGallon;
                        pricingModel.CityGroupTerminalStateId = pricing.CityGroupTerminalStateId;
                        pricingModel.CityGroupTerminalName = pricing.CityGroupTerminalName;
                        pricingModel.CityGroupTerminalId = pricing.CityGroupTerminalId;
                        pricingModel.FromQuantity = pricing.MinQuantity ?? 0;
                        pricingModel.ToQuantity = pricing.MaxQuantity ?? 0;
                        if (pricing.PricingTypeId == (int)PricingType.Suppliercost)
                        {
                            pricingModel.DisplayPrice = helperDomain.GetPricePerGallon(pricing.SupplierCostMarkupValue, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1);
                        }
                        else if (pricing.PricingTypeId == (int)PricingType.RackAverage)
                        {
                            pricingModel.DisplayPrice = helperDomain.GetPricePerGallon(pricing.RackPrice, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1);
                        }
                        else
                        {
                            pricingModel.DisplayPrice = helperDomain.GetPricePerGallon(pricing.PricePerGallon, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1);
                        }
                        viewModel.FuelPricingDetails.TierPricing.Pricings.Add(pricingModel);
                    }
                    if (viewModel.FuelPricingDetails.TierPricing.Pricings != null && viewModel.FuelPricingDetails.TierPricing.Pricings.Any())
                    {
                        var LastEntity = viewModel.FuelPricingDetails.TierPricing.Pricings.Last();
                        LastEntity.IsAboveQuantity = true;
                    }
                }
                else
                {
                    foreach (var pricing in leadRequestPriceDetails.LeadPricingDetails)
                    {
                        viewModel.FuelPricingDetails.Code = pricing.Code;
                        viewModel.FuelPricingDetails.CodeId = pricing.CodeId;
                        viewModel.FuelPricingDetails.CodeDescription = pricing.CodeDescription;
                        viewModel.FuelPricingDetails.RackAvgTypeId = pricing.RackAvgTypeId;
                        viewModel.FuelPricingDetails.RackPrice = pricing.RackPrice;
                        viewModel.FuelPricingDetails.SupplierCostMarkupTypeId = pricing.SupplierCostMarkupTypeId;
                        viewModel.FuelPricingDetails.SupplierCostMarkupValue = pricing.SupplierCostMarkupValue;
                        viewModel.FuelPricingDetails.TerminalId = pricing.TerminalId;
                        viewModel.FuelPricingDetails.TerminalName = pricing.TerminalName;
                        viewModel.FuelPricingDetails.PricingTypeId = pricing.PricingTypeId;
                        viewModel.FuelPricingDetails.PricePerGallon = pricing.PricePerGallon;
                        viewModel.FuelPricingDetails.CityGroupTerminalStateId = pricing.CityGroupTerminalStateId;
                        viewModel.FuelPricingDetails.CityGroupTerminalName = pricing.CityGroupTerminalName;
                        viewModel.FuelPricingDetails.CityGroupTerminalId = pricing.CityGroupTerminalId;
                        viewModel.FuelPricingDetails.EnableCityRack = pricing.EnableCityRack;
                    }
                }
            }
            return viewModel;
        }
        public static List<FuelFee> ToFeesEntity(this List<FeesViewModel> viewModel, List<FuelFee> entities = null)
        {
            if (entities == null)
                entities = new List<FuelFee>();

            foreach (var fee in viewModel)
            {
                var isCommonFee = int.TryParse(fee.FeeTypeId, out int feeTypeId);
                if (!isCommonFee)
                {
                    feeTypeId = (int)FeeType.OtherFee;
                }

                if ((feeTypeId > 0 && fee.Fee.HasValue && fee.Fee.Value > 0) || fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    FuelFee entity = new FuelFee();
                    entity.FeeTypeId = feeTypeId;
                    entity.Fee = fee.Fee.HasValue ? fee.Fee.Value : 0;
                    entity.FeeSubTypeId = fee.FeeSubTypeId;
                    entity.OtherFeeTypeId = fee.OtherFeeTypeId;
                    entity.IncludeInPPG = fee.IncludeInPPG;
                    entity.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                    entity.SpecialDate = fee.SpecialDate;
                    entity.Currency = fee.Currency;
                    entity.UoM = fee.UoM;
                    entity.WaiveOffTime = fee.WaiveOffTime;
                    if (!fee.CommonFee)
                    {
                        entity.FeeDetails = fee.OtherFeeDescription;
                    }
                    if (fee.MarginTypeId != (int)MarginType.NoChange)
                    {
                        entity.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee.Value, fee.MarginTypeId);
                        entity.MarginTypeId = fee.MarginTypeId;
                        entity.Margin = fee.Margin ?? 0;
                    }
                    if (feeTypeId == (int)FeeType.UnderGallonFee)
                    {
                        entity.MinimumGallons = fee.MinimumGallons;
                    }

                    if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    {
                        foreach (var deliveryFees in fee.DeliveryFeeByQuantity)
                        {
                            var feeByQuantity = new FeeByQuantity();
                            feeByQuantity.Id = deliveryFees.Id;
                            feeByQuantity.FeeTypeId = feeTypeId;
                            feeByQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                            feeByQuantity.MinQuantity = deliveryFees.MinQuantity;
                            feeByQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                            feeByQuantity.Fee = deliveryFees.Fee;
                            feeByQuantity.Currency = fee.Currency;
                            feeByQuantity.UoM = fee.UoM;
                            entity.FeeByQuantities.Add(feeByQuantity);
                        }
                    }
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public static List<FuelFee> ToUpdateFeesEntity(this List<FeesViewModel> viewModel, List<FuelFee> entity = null)
        {
            if (viewModel != null && viewModel.Any())
            {
                int index = entity.IndexOf(entity.GetEnumerator().Current);
                foreach (var fee in viewModel)
                {
                    var isCommonFee = int.TryParse(fee.FeeTypeId, out int feeTypeId);
                    if (!isCommonFee)
                    {
                        feeTypeId = (int)FeeType.OtherFee;
                    }

                    if ((feeTypeId > 0 && fee.Fee.HasValue && fee.Fee.Value > 0) || fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                    {
                        index++;
                        if (entity.Count() > index)
                        {
                            var fuelFee = entity[index];

                            fuelFee.FeeTypeId = feeTypeId;
                            fuelFee.Fee = fee.Fee.HasValue ? fee.Fee.Value : 0;
                            fuelFee.FeeSubTypeId = fee.FeeSubTypeId;
                            fuelFee.OtherFeeTypeId = fee.OtherFeeTypeId;
                            fuelFee.IncludeInPPG = fee.IncludeInPPG;
                            fuelFee.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                            fuelFee.SpecialDate = fee.SpecialDate;
                            fuelFee.Currency = fee.Currency;
                            fuelFee.UoM = fee.UoM;
                            fuelFee.WaiveOffTime = fee.WaiveOffTime;
                            if (!fee.CommonFee)
                            {
                                fuelFee.FeeDetails = fee.OtherFeeDescription;
                            }
                            if (fee.MarginTypeId != (int)MarginType.NoChange)
                            {
                                fuelFee.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee.Value, fee.MarginTypeId);
                                fuelFee.MarginTypeId = fee.MarginTypeId;
                                fuelFee.Margin = fee.Margin ?? 0;
                            }
                            if (feeTypeId == (int)FeeType.UnderGallonFee)
                            {
                                fuelFee.MinimumGallons = fee.MinimumGallons;
                            }

                            if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                            {
                                if (fuelFee.FeeByQuantities != null)
                                {
                                    var fuelFeeByQuantity = fuelFee.FeeByQuantities.ToList();
                                    var feesIndex = fuelFeeByQuantity.IndexOf(fuelFeeByQuantity.GetEnumerator().Current);
                                    foreach (var deliveryFees in fee.DeliveryFeeByQuantity)
                                    {
                                        feesIndex++;
                                        if (fuelFeeByQuantity.Count() > feesIndex)
                                        {
                                            var feeByQuantity = fuelFeeByQuantity[feesIndex];

                                            feeByQuantity.FeeTypeId = feeTypeId;
                                            feeByQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                                            feeByQuantity.MinQuantity = deliveryFees.MinQuantity;
                                            feeByQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                                            feeByQuantity.Fee = deliveryFees.Fee;
                                            feeByQuantity.Currency = fee.Currency;
                                            feeByQuantity.UoM = fee.UoM;
                                        }
                                        else
                                        {
                                            var feeByQuantity = new FeeByQuantity();
                                            feeByQuantity.Id = deliveryFees.Id;
                                            feeByQuantity.FeeTypeId = feeTypeId;
                                            feeByQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                                            feeByQuantity.MinQuantity = deliveryFees.MinQuantity;
                                            feeByQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                                            feeByQuantity.Fee = deliveryFees.Fee;
                                            feeByQuantity.Currency = fee.Currency;
                                            feeByQuantity.UoM = fee.UoM;
                                            fuelFee.FeeByQuantities.Add(feeByQuantity);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            FuelFee feeEntity = new FuelFee();
                            feeEntity.FeeTypeId = feeTypeId;
                            feeEntity.Fee = fee.Fee.HasValue ? fee.Fee.Value : 0;
                            feeEntity.FeeSubTypeId = fee.FeeSubTypeId;
                            feeEntity.OtherFeeTypeId = fee.OtherFeeTypeId;
                            feeEntity.IncludeInPPG = fee.IncludeInPPG;
                            feeEntity.FeeConstraintTypeId = fee.FeeConstraintTypeId;
                            feeEntity.SpecialDate = fee.SpecialDate;
                            feeEntity.Currency = fee.Currency;
                            feeEntity.UoM = fee.UoM;
                            feeEntity.WaiveOffTime = fee.WaiveOffTime;
                            if (!fee.CommonFee)
                            {
                                feeEntity.FeeDetails = fee.OtherFeeDescription;
                            }
                            if (fee.MarginTypeId != (int)MarginType.NoChange)
                            {
                                feeEntity.Fee = HelperDomain.GetPriceWithMargin(fee.Margin ?? 0, fee.Fee.Value, fee.MarginTypeId);
                                feeEntity.MarginTypeId = fee.MarginTypeId;
                                feeEntity.Margin = fee.Margin ?? 0;
                            }
                            if (feeTypeId == (int)FeeType.UnderGallonFee)
                            {
                                feeEntity.MinimumGallons = fee.MinimumGallons;
                            }

                            if (fee.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                            {
                                foreach (var deliveryFees in fee.DeliveryFeeByQuantity)
                                {
                                    var feeByQuantity = new FeeByQuantity();
                                    feeByQuantity.Id = deliveryFees.Id;
                                    feeByQuantity.FeeTypeId = feeTypeId;
                                    feeByQuantity.FeeSubTypeId = fee.FeeSubTypeId;
                                    feeByQuantity.MinQuantity = deliveryFees.MinQuantity;
                                    feeByQuantity.MaxQuantity = deliveryFees.MaxQuantity;
                                    feeByQuantity.Fee = deliveryFees.Fee;
                                    feeByQuantity.Currency = fee.Currency;
                                    feeByQuantity.UoM = fee.UoM;
                                    feeEntity.FeeByQuantities.Add(feeByQuantity);
                                }
                            }
                            entity.Add(feeEntity);

                        }
                    }
                }
            }
            return entity;
        }

        public static List<FeesViewModel> ToSourcingFeesViewModel(this ICollection<FuelFee> entities, List<FeesViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<FeesViewModel>();

            foreach (FuelFee entity in entities.Where(t => (t.FeeTypeId != (int)FeeType.ResaleFee && t.FeeTypeId != (int)FeeType.SurchargeFreightFee && t.FeeTypeId != (int)FeeType.FreightCost) && t.DiscountLineItemId == null))
            {
                FeesViewModel model = new FeesViewModel();
                model.FeeTypeId = entity.FeeTypeId.ToString();
                model.FeeSubTypeId = entity.FeeSubTypeId;
                model.FeeTypeName = entity.Currency == Currency.CAD ? entity.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeType.Name;
                model.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeSubType.Name;
                model.Fee = entity.Fee.GetPreciseValue(6);
                model.OtherFeeTypeId = entity.OtherFeeTypeId;
                model.TotalFee = entity.TotalFee ?? 0;
                model.FeeSubQuantity = entity.FeeSubQuantity ?? 0;
                model.Margin = 0;
                model.MarginTypeId = (int)MarginType.NoChange;
                model.IncludeInPPG = entity.IncludeInPPG;
                model.FeeConstraintTypeId = entity.FeeConstraintTypeId;
                model.SpecialDate = entity.SpecialDate;
                model.MinimumGallons = entity.MinimumGallons?.GetPreciseValue(6);
                model.CommonFee = !(entity.OtherFeeTypeId.HasValue || entity.FeeDetails != null);
                model.AddToCommonFees = entity.OtherFeeTypeId.HasValue;
                model.OtherFeeDescription = entity.FeeDetails;
                model.WaiveOffTime = entity.WaiveOffTime;
                model.TruckLoadCategoryId = entity.MstFeeType.TruckLoadCategoryId;
                if (entity.FeeTypeId == (int)FeeType.OtherFee && entity.OtherFeeTypeId.HasValue && string.IsNullOrWhiteSpace(entity.FeeDetails))
                {
                    model.FeeTypeId = Constants.OtherCommonFeeCode + "-" + entity.OtherFeeTypeId;
                    model.OtherFeeDescription = entity.MstOtherFeeType.Name;
                    var name = entity.FeeDetails ?? entity.MstOtherFeeType?.Name;
                    model.FeeTypeName = name ?? model.FeeTypeName;
                }
                else if (entity.FeeTypeId == (int)FeeType.OtherFee && !entity.OtherFeeTypeId.HasValue)
                {
                    model.OtherFeeDescription = entity.FeeDetails;
                }

                if (entity.FeeSubTypeId == (int)FeeSubType.ByQuantity)
                {
                    model.DeliveryFeeByQuantity.AddRange(entity.FeeByQuantities.OrderBy(t => t.MinQuantity).Select(t => t.ToSourceViewModel()).ToList());
                }
                model.UoM = entity.UoM;
                model.Currency = entity.Currency;

                model.StartTime = entity.StartTime;
                model.EndTime = entity.EndTime;
                model.WaiveOffTime = entity.WaiveOffTime;
                if (entity.TaxDetailId != null)
                {
                    model.FeeTaxDetails = new FeeTaxDetails
                    {
                        Amount = entity.TaxDetails.TaxAmount,
                        Description = entity.TaxDetails.RateDescription,
                        Percentage = entity.TaxDetails.TaxRate
                    };
                }

                if (IsFTLSpecificFee(entity.FeeTypeId))
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = ((int)entity.FeeSubQuantity.Value / 60) + entity.WaiveOffTime;
                    }
                }
                else if (entity.FeeTypeId == (int)FeeType.Retain)
                {
                    model.TruckLoadType = (int)TruckLoadTypes.FullTruckLoad;
                    if (entity.FeeSubQuantity.HasValue)
                    {
                        model.TimeInMinutes = (int)entity.FeeSubQuantity.Value / 60;
                    }
                }
                viewModel.Add(model);
            }

            return viewModel;
        }
        public static DeliveryFeeByQuantityViewModel ToSourceViewModel(this FeeByQuantity entity, DeliveryFeeByQuantityViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new DeliveryFeeByQuantityViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.FeeTypeId = entity.FeeTypeId;
            viewModel.FeeSubTypeId = entity.FeeSubTypeId;
            viewModel.MinQuantity = entity.MinQuantity.GetPreciseValue(6);
            viewModel.MaxQuantity = entity.MaxQuantity.HasValue ? entity.MaxQuantity.Value.GetPreciseValue(6) : entity.MaxQuantity;
            viewModel.Fee = entity.Fee.GetPreciseValue(6);
            viewModel.Currency = entity.Currency;
            viewModel.UoM = entity.UoM;

            return viewModel;
        }
        private static bool IsFTLSpecificFee(int feeTypeId)
        {
            return feeTypeId == (int)FeeType.DemurrageFeeDestination || feeTypeId == (int)FeeType.DemurrageFeeTerminal
                    || feeTypeId == (int)FeeType.DemurrageOther || feeTypeId == (int)FeeType.FreightFee || feeTypeId == (int)FeeType.SplitTank
                    || feeTypeId == (int)FeeType.PumpCharge || feeTypeId == (int)FeeType.StopOffFee;
        }

        public static List<LeadPricingDetail> ToUpdateTierEntity(this List<PricingViewModel> viewModel, List<LeadPricingDetail> entity = null)
        {
            if (viewModel != null && viewModel.Any())
            {
                int index = entity.IndexOf(entity.GetEnumerator().Current);
                foreach (var pricing in viewModel)
                {
                    if (pricing != null)
                    {
                        index++;
                        if (entity.Count() > index)
                        {
                            var leadPricingDetail = entity[index];
                            leadPricingDetail.RackAvgTypeId = pricing.RackAvgTypeId;
                            leadPricingDetail.RackPrice = pricing.RackPrice ?? 0;
                            leadPricingDetail.SupplierCostMarkupTypeId = pricing.SupplierCostMarkupTypeId ?? 0;
                            leadPricingDetail.SupplierCostMarkupValue = pricing.SupplierCostMarkupValue ?? 0;
                            leadPricingDetail.TerminalId = pricing.TerminalId;
                            leadPricingDetail.TerminalName = pricing.TerminalName;

                            leadPricingDetail.PricingTypeId = pricing.PricingTypeId;
                            leadPricingDetail.PricePerGallon = pricing.PricePerGallon ?? 0;
                            leadPricingDetail.MinQuantity = pricing.FromQuantity;
                            leadPricingDetail.MaxQuantity = pricing.ToQuantity;
                            leadPricingDetail.CityGroupTerminalId = pricing.CityGroupTerminalId;
                            leadPricingDetail.CityGroupTerminalName = pricing.CityGroupTerminalName;
                            leadPricingDetail.CityGroupTerminalStateId = pricing.CityGroupTerminalStateId;
                            leadPricingDetail.CodeDescription = pricing.PricingCode.Description;
                            leadPricingDetail.CodeId = pricing.PricingCode.Id;
                            leadPricingDetail.Code = pricing.PricingCode.Code;
                        }
                        else
                        {
                            var leadPricingEntity = new LeadPricingDetail();
                            leadPricingEntity.RackAvgTypeId = pricing.RackAvgTypeId;
                            leadPricingEntity.RackPrice = pricing.RackPrice ?? 0;
                            leadPricingEntity.SupplierCostMarkupTypeId = pricing.SupplierCostMarkupTypeId ?? 0;
                            leadPricingEntity.SupplierCostMarkupValue = pricing.SupplierCostMarkupValue ?? 0;
                            leadPricingEntity.TerminalId = pricing.TerminalId;
                            leadPricingEntity.TerminalName = pricing.TerminalName;

                            leadPricingEntity.PricingTypeId = pricing.PricingTypeId;
                            leadPricingEntity.PricePerGallon = pricing.PricePerGallon ?? 0;
                            leadPricingEntity.MinQuantity = pricing.FromQuantity;
                            leadPricingEntity.MaxQuantity = pricing.ToQuantity;
                            leadPricingEntity.CityGroupTerminalId = pricing.CityGroupTerminalId;
                            leadPricingEntity.CityGroupTerminalName = pricing.CityGroupTerminalName;
                            leadPricingEntity.CityGroupTerminalStateId = pricing.CityGroupTerminalStateId;
                            leadPricingEntity.CodeDescription = pricing.PricingCode.Description;
                            leadPricingEntity.CodeId = pricing.PricingCode.Id;
                            leadPricingEntity.Code = pricing.PricingCode.Code;

                            entity.Add(leadPricingEntity);
                        }
                    }
                }
            }
            return entity;
        }
        public static SourcingDetailViewModel ToModel(this LeadRequest entity, SourcingDetailViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new SourcingDetailViewModel();
            }
            viewModel.Id = entity.Id;
            viewModel.DisplayRequestId = entity.DisplayRequestID;
            viewModel.TruckLoadType = EnumHelperMethods.GetDisplayName((TruckLoadTypes)entity.TruckLoadType);
            viewModel.FreightOnBoardType = EnumHelperMethods.GetDisplayName((FreightOnBoardTypes)entity.FreightOnBoardType);
            viewModel.AccountingCompanyId = entity.AccountingCompanyId;
            viewModel.RequestName = entity.Name;
            viewModel.RequestStatus = entity.Status.ToString();
            viewModel.SalesUserId = entity.SalesUserId;

            var leadAddressDetail = entity.LeadAddressDetail.FirstOrDefault();
            if (entity.LeadAddressDetail != null && entity.LeadAddressDetail.Any())
            {
                viewModel.AddressDetails = new SourcingAddressDetailViewModel()
                {
                    Id = leadAddressDetail.Id,
                    Address = leadAddressDetail.Address,
                    City = leadAddressDetail.City,
                    CountryName = EnumHelperMethods.GetDisplayName((Country)leadAddressDetail.CountryId),
                    CountyName = leadAddressDetail.CountyName,
                    Currency = EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency),
                    DisplayJobID = leadAddressDetail.DisplayJobID,
                    InventoryDataCaptureType = EnumHelperMethods.GetDisplayName((InventoryDataCaptureType)leadAddressDetail.InventoryDataCaptureType),
                    IsGeocodeUsed = leadAddressDetail.IsGeocodeUsed,
                    IsMarineLocation = leadAddressDetail.IsMarineLocation,
                    IsProFormaPoEnabled = leadAddressDetail.IsProFormaPoEnabled,
                    IsRetailJob = leadAddressDetail.IsRetailJob,
                    JobId = leadAddressDetail.JobId,
                    JobName = leadAddressDetail.JobName,
                    Latitude = Convert.ToString(leadAddressDetail.Latitude),
                    Longitude = Convert.ToString(leadAddressDetail.Longitude),
                    LocationManagedType = EnumHelperMethods.GetDisplayName((LocationManagedType)leadAddressDetail.LocationManagedType),
                    SignatureEnabled = leadAddressDetail.SignatureEnabled,
                    TimeZoneName = leadAddressDetail.TimeZoneName,
                    UOM = EnumHelperMethods.GetDisplayName((UoM)leadAddressDetail.UOM),
                    ZipCode = leadAddressDetail.ZipCode,
                };
            }
            if (entity.LeadCustomerInformations != null && entity.LeadCustomerInformations.Any())
            {
                var customerDetails = entity.LeadCustomerInformations.FirstOrDefault();
                viewModel.CustomerDetails = new SourceCustomerDetailViewModel()
                {
                    Id = customerDetails.Id,
                    CompanyName = customerDetails.CompanyName,
                    Email = customerDetails.Email,
                    PhoneNumber = customerDetails.PhoneNumber,
                    IsInvitationEnabled = customerDetails.IsInvitationEnabled,
                    IsNotifyDeliveries = customerDetails.IsNotifyDeliveries,
                    IsNotifySchedules = customerDetails.IsNotifySchedules,
                    Name = customerDetails.Name,
                    UserId = customerDetails.UserId,
                    IsNewCompany = customerDetails.CompanyId > 0 ? false : true

                };
                foreach (var contactperson in entity.LeadCustomerInformations.Where(t => t.Id != customerDetails.Id))
                {
                    viewModel.CustomerDetails.ContactPersons.Add(
                    new ContactPersonViewModel()
                    {
                        Email = contactperson.Email,
                        PhoneNumber = contactperson.PhoneNumber,
                        Name = contactperson.Name,
                        Id = contactperson.Id
                    });
                }
            }
            if (entity.LeadFuelDeliveryDetails != null && entity.LeadFuelDeliveryDetails.Any())
            {
                var fuelDeliveryDetail = entity.LeadFuelDeliveryDetails.FirstOrDefault();
                viewModel.FuelDeliveryDetails = new SourceFuelDeliveryViewModel()
                {
                    Id = fuelDeliveryDetail.Id,
                    DeliveryTypes = fuelDeliveryDetail.DeliveryTypeId == 1 ? DeliveryType.OneTimeDelivery.ToString() : DeliveryType.MultipleDeliveries.ToString(),
                    EndTime = Convert.ToString(fuelDeliveryDetail.EndTime),
                    EndDate = fuelDeliveryDetail.EndDate != null ? Convert.ToDateTime(fuelDeliveryDetail.EndDate.ToString()).ToShortDateString() : string.Empty,
                    IsPrePostDipRequired = fuelDeliveryDetail.IsPrePostDipRequired,
                    NetDays = fuelDeliveryDetail.NetDays,
                    OrderEnforcementId = fuelDeliveryDetail.OrderEnforcementId.ToString(),
                    PaymentMethods = EnumHelperMethods.GetDisplayName((PaymentMethods)fuelDeliveryDetail.PaymentMethods),
                    PaymentTermId = fuelDeliveryDetail.PaymentTermId,
                    SingleDeliverySubTypes = EnumHelperMethods.GetDisplayName((SingleDeliverySubTypes)fuelDeliveryDetail.SingleDeliverySubTypes),
                    StartDate = Convert.ToDateTime(fuelDeliveryDetail.StartDate.ToString()).ToShortDateString(),
                    StartTime = Convert.ToString(fuelDeliveryDetail.StartTime),
                    SourcePaymentViewModel = new SourcePaymentViewModel()
                    {
                        PaymentMethods = EnumHelperMethods.GetDisplayName((PaymentMethods)fuelDeliveryDetail.PaymentMethods),
                        PaymentTerms = EnumHelperMethods.GetDisplayName((PaymentTerms)fuelDeliveryDetail.PaymentTermId),
                        NetDays = fuelDeliveryDetail.NetDays.ToString()
                    }
                };
            }
            if (entity.LeadFuelDetails != null && entity.LeadFuelDetails.Any())
            {
                var fuelDetail = entity.LeadFuelDetails.FirstOrDefault();
                viewModel.FuelDetails = new SourcingFuelViewModel()
                {
                    Id = fuelDetail.Id,
                    FuelDisplayGroupId = fuelDetail.FuelDisplayGroupId,
                    IsTierPricing = fuelDetail.IsTierPricing,
                    MaximumQuantity = fuelDetail.MaximumQuantity,
                    MinimumQuantity = fuelDetail.MinimumQuantity,
                    NonStandardFuelDescription = fuelDetail.NonStandardFuelDescription,
                    NonStandardFuelName = fuelDetail.NonStandardFuelName,
                    PricePerGallon = fuelDetail.PricePerGallon,
                    PricingTypeId = fuelDetail.PricingTypeId,
                    Quantity = fuelDetail.Quantity,
                    QuantityIndicatorTypes = EnumHelperMethods.GetDisplayName((QuantityIndicatorTypes)fuelDetail.QuantityIndicatorTypes),
                    QuantityTypeId = fuelDetail.QuantityTypeId
                };

            }
            if (entity.LeadRequestPriceDetails != null && entity.LeadRequestPriceDetails.Any())
            {
                var leadRequestPriceDetails = entity.LeadRequestPriceDetails.FirstOrDefault();
                viewModel.FuelPricingDetails.TierPricing.RequestPriceDetailId = leadRequestPriceDetails.Id;
                viewModel.FuelPricingDetails.IsTierPricingRequired = leadRequestPriceDetails.IsTierPricingRequired;
                viewModel.FuelPricingDetails.PricingTypeId = leadRequestPriceDetails.PricingTypeId ?? 0;
                viewModel.FuelPricingDetails.TierPricing.TierPricingType = (TierPricingType)leadRequestPriceDetails.TierTypeId;
                viewModel.FuelPricingDetails.TierPricing.IsResetCumulation = leadRequestPriceDetails.CumulationTypeId > 0 ? true : false;
                viewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.CumulationType = leadRequestPriceDetails.CumulationTypeId.HasValue ? (CumulationType)leadRequestPriceDetails.CumulationTypeId : CumulationType.Unknown;
                viewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.Date = leadRequestPriceDetails.CumulationResetDate;
                viewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting.Day = leadRequestPriceDetails.CumulationResetDay.HasValue && leadRequestPriceDetails.CumulationResetDay.Value > 0 ? (WeekDay)leadRequestPriceDetails.CumulationResetDay : WeekDay.Monday;
                viewModel.IsSupressOrderPricing = leadRequestPriceDetails.IsSuppressPricing;

                if (leadRequestPriceDetails.IsTierPricingRequired && leadRequestPriceDetails.LeadPricingDetails.Any())
                {
                    foreach (var pricing in leadRequestPriceDetails.LeadPricingDetails)
                    {
                        var helperDomain = new HelperDomain();
                        var pricingModel = new PricingViewModel();
                        pricingModel.PricingCode.Code = pricing.Code;
                        pricingModel.PricingCode.Id = pricing.CodeId;
                        pricingModel.PricingCode.Description = pricing.CodeDescription;
                        pricingModel.RackAvgTypeId = pricing.RackAvgTypeId;
                        pricingModel.RackPrice = pricing.RackPrice;
                        pricingModel.SupplierCostMarkupTypeId = pricing.SupplierCostMarkupTypeId;
                        pricingModel.SupplierCostMarkupValue = pricing.SupplierCostMarkupValue;
                        pricingModel.TerminalId = pricing.TerminalId;
                        pricingModel.TerminalName = pricing.TerminalName;
                        pricingModel.PricingTypeId = pricing.PricingTypeId;
                        pricingModel.PricePerGallon = pricing.PricePerGallon;
                        pricingModel.CityGroupTerminalStateId = pricing.CityGroupTerminalStateId;
                        pricingModel.CityGroupTerminalName = pricing.CityGroupTerminalName;
                        pricingModel.CityGroupTerminalId = pricing.CityGroupTerminalId;
                        pricingModel.FromQuantity = pricing.MinQuantity ?? 0;
                        pricingModel.ToQuantity = pricing.MaxQuantity ?? 0;
                        if (pricing.PricingTypeId == (int)PricingType.Suppliercost)
                        {
                            pricingModel.DisplayPrice = string.Join(" ", (helperDomain.GetPricePerGallon(pricing.SupplierCostMarkupValue, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1))
                                                                       , EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency));
                        }
                        else if (pricing.PricingTypeId == (int)PricingType.RackAverage)
                        {
                            pricingModel.DisplayPrice = string.Join(" ", (helperDomain.GetPricePerGallon(pricing.RackPrice, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1))
                                                                       , EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency));
                        }
                        else
                        {
                            pricingModel.DisplayPrice = string.Join(" ", (helperDomain.GetPricePerGallon(pricing.PricePerGallon, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1))
                                                                       , EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency));
                        }
                        if (string.IsNullOrEmpty(pricingModel.PricingCode.Description))
                        {
                            if (pricing.Code.Contains("A-120000") && pricing.PricePerGallon > 0)
                            {
                                pricingModel.PricingCode.Description = "Axxis, Fixed";
                            }
                            else if (pricing.Code.Contains("A-120000") && pricing.SupplierCostMarkupValue > 0)
                            {
                                pricingModel.PricingCode.Description = "Axxis, Fuel Cost";
                            }
                            else if (pricing.Code.Contains("A-120000") && pricing.RackPrice > 0)
                            {
                                pricingModel.PricingCode.Description = "Axxis, Rack Avg";
                            }
                            else if (pricing.Code.Contains("A-140000") && pricing.PricePerGallon > 0)
                            {
                                pricingModel.PricingCode.Description = "OPIS, Fixed";
                            }
                            else if (pricing.Code.Contains("A-140000") && pricing.SupplierCostMarkupValue > 0)
                            {
                                pricingModel.PricingCode.Description = "OPIS, Fuel Cost";
                            }
                            else if (pricing.Code.Contains("A-140000") && pricing.RackPrice > 0)
                            {
                                pricingModel.PricingCode.Description = "OPIS, Rack Avg";
                            }
                        }
                        viewModel.FuelPricingDetails.TierPricing.Pricings.Add(pricingModel);
                    }
                    if (viewModel.FuelPricingDetails.TierPricing.Pricings != null && viewModel.FuelPricingDetails.TierPricing.Pricings.Any())
                    {
                        var LastEntity = viewModel.FuelPricingDetails.TierPricing.Pricings.Last();
                        LastEntity.IsAboveQuantity = true;
                    }
                }
                else
                {
                    foreach (var pricing in leadRequestPriceDetails.LeadPricingDetails)
                    {
                        var helperDomain = new HelperDomain();
                        viewModel.FuelPricingDetails.Code = pricing.Code;
                        viewModel.FuelPricingDetails.CodeId = pricing.CodeId;
                        viewModel.FuelPricingDetails.CodeDescription = pricing.CodeDescription;
                        viewModel.FuelPricingDetails.RackAvgTypeId = pricing.RackAvgTypeId;
                        viewModel.FuelPricingDetails.RackPrice = pricing.RackPrice;
                        viewModel.FuelPricingDetails.SupplierCostMarkupTypeId = pricing.SupplierCostMarkupTypeId;
                        viewModel.FuelPricingDetails.SupplierCostMarkupValue = pricing.SupplierCostMarkupValue;
                        viewModel.FuelPricingDetails.TerminalId = pricing.TerminalId;
                        viewModel.FuelPricingDetails.TerminalName = pricing.TerminalName;
                        viewModel.FuelPricingDetails.PricingTypeId = pricing.PricingTypeId;
                        viewModel.FuelPricingDetails.PricePerGallon = pricing.PricePerGallon;
                        viewModel.FuelPricingDetails.CityGroupTerminalStateId = pricing.CityGroupTerminalStateId;
                        viewModel.FuelPricingDetails.CityGroupTerminalName = pricing.CityGroupTerminalName;
                        viewModel.FuelPricingDetails.CityGroupTerminalId = pricing.CityGroupTerminalId;
                        viewModel.FuelPricingDetails.EnableCityRack = pricing.EnableCityRack;
                        if (pricing.PricingTypeId == (int)PricingType.Suppliercost)
                        {
                            viewModel.FuelPricingDetails.DisplayPrice = string.Join(" ", (helperDomain.GetPricePerGallon(pricing.SupplierCostMarkupValue, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1))
                                                                       , EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency));
                        }
                        else if (pricing.PricingTypeId == (int)PricingType.RackAverage)
                        {
                            viewModel.FuelPricingDetails.DisplayPrice = string.Join(" ", (helperDomain.GetPricePerGallon(pricing.RackPrice, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1))
                                                                       , EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency));
                        }
                        else
                        {
                            viewModel.FuelPricingDetails.DisplayPrice = string.Join(" ", (helperDomain.GetPricePerGallon(pricing.PricePerGallon, pricing.PricingTypeId, pricing.RackAvgTypeId ?? 1))
                                                                       , EnumHelperMethods.GetDisplayName((Currency)leadAddressDetail.Currency));
                        }
                        if (string.IsNullOrEmpty(viewModel.FuelPricingDetails.CodeDescription))
                        {
                            if (pricing.PricePerGallon > 0)
                            {
                                viewModel.FuelPricingDetails.CodeDescription = "Axxis, Fixed";
                            }
                            else if (pricing.SupplierCostMarkupValue > 0)
                            {
                                viewModel.FuelPricingDetails.CodeDescription = "Axxis, Fuel Cost";
                            }
                            else if (pricing.RackPrice > 0)
                            {
                                viewModel.FuelPricingDetails.CodeDescription = "Axxis, Rack Avg";
                            }
                        }
                    }
                }
            }
            return viewModel;
        }
        public static List<SourceFeesViewModel> ToFeesModel(this ICollection<FuelFee> entities, List<SourceFeesViewModel> viewModel = null)
        {
            if (viewModel == null)
                viewModel = new List<SourceFeesViewModel>();

            foreach (FuelFee entity in entities.Where(t => (t.FeeTypeId != (int)FeeType.ResaleFee && t.FeeTypeId != (int)FeeType.SurchargeFreightFee && t.FeeTypeId != (int)FeeType.FreightCost) && t.DiscountLineItemId == null))
            {
                SourceFeesViewModel model = new SourceFeesViewModel();

                model.FeeTypeName = entity.Currency == Currency.CAD ? entity.MstFeeType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeType.Name;
                model.FeeSubTypeName = entity.Currency == Currency.CAD ? entity.MstFeeSubType.Name.Replace(Constants.Gallon, Constants.Litre) : entity.MstFeeSubType.Name;
                model.Fee = entity.Fee.ToString();
                model.FeeDetails = string.Format("{0} ${1}", model.FeeSubTypeName, model.Fee);
                viewModel.Add(model);
            }
            return viewModel;
        }
    }
}
