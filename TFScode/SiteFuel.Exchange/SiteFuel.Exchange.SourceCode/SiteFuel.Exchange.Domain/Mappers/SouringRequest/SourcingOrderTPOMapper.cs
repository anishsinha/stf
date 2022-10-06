using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class SourcingOrderTPOMapper
    {
        public static ThirdPartyOrderViewModel ToTPOViewmodel(this SourcingRequestViewModel sourcingRequestViewModel, ThirdPartyOrderViewModel viewModel = null)
        {
            if (viewModel == null)
            {
                viewModel = new ThirdPartyOrderViewModel();
            }
            viewModel.LeadRequestId = sourcingRequestViewModel.Id;
            viewModel.AccountingCompanyId = sourcingRequestViewModel.AccountingCompanyId;
            #region AddressDetails
            viewModel.AddressDetails.Address = sourcingRequestViewModel.AddressDetails.Address;
            viewModel.AddressDetails.City = sourcingRequestViewModel.AddressDetails.City;
            viewModel.AddressDetails.Country.Id = sourcingRequestViewModel.AddressDetails.CountryId;
            viewModel.AddressDetails.CountyName = sourcingRequestViewModel.AddressDetails.CountyName;
            viewModel.AddressDetails.Country.Currency = sourcingRequestViewModel.AddressDetails.Currency;
            viewModel.AddressDetails.DisplayJobID = sourcingRequestViewModel.AddressDetails.DisplayJobID;
            viewModel.AddressDetails.InventoryDataCaptureType = sourcingRequestViewModel.AddressDetails.InventoryDataCaptureType;
            viewModel.AddressDetails.IsCompanyOwned = sourcingRequestViewModel.AddressDetails.IsCompanyOwned;
            viewModel.AddressDetails.IsGeocodeUsed = sourcingRequestViewModel.AddressDetails.IsGeocodeUsed;
            viewModel.AddressDetails.IsMarineLocation = sourcingRequestViewModel.AddressDetails.IsMarineLocation;
            viewModel.AddressDetails.IsNewJob = sourcingRequestViewModel.AddressDetails.IsNewJob;
            viewModel.AddressDetails.IsProFormaPoEnabled = sourcingRequestViewModel.AddressDetails.IsProFormaPoEnabled;
            viewModel.AddressDetails.IsRetailJob = sourcingRequestViewModel.AddressDetails.IsRetailJob;
            viewModel.AddressDetails.JobId = sourcingRequestViewModel.AddressDetails.JobId;
            viewModel.AddressDetails.JobLocationType = sourcingRequestViewModel.AddressDetails.JobLocationType;
            viewModel.AddressDetails.JobName = sourcingRequestViewModel.AddressDetails.JobName;
            viewModel.AddressDetails.Latitude = Convert.ToDecimal(sourcingRequestViewModel.AddressDetails.Latitude);
            viewModel.AddressDetails.LocationManagedType = sourcingRequestViewModel.AddressDetails.LocationManagedType;
            viewModel.AddressDetails.Longitude = Convert.ToDecimal(sourcingRequestViewModel.AddressDetails.Longitude);
            viewModel.AddressDetails.MarineUoM = sourcingRequestViewModel.AddressDetails.MarineUoM;
            viewModel.AddressDetails.SignatureEnabled = sourcingRequestViewModel.AddressDetails.SignatureEnabled;
            viewModel.AddressDetails.State.Id = sourcingRequestViewModel.AddressDetails.StateId;
            viewModel.AddressDetails.TimeZoneName = sourcingRequestViewModel.AddressDetails.TimeZoneName;
            viewModel.AddressDetails.WBSNumber = sourcingRequestViewModel.WBSNumber;
            viewModel.AddressDetails.ZipCode = sourcingRequestViewModel.AddressDetails.ZipCode;
            viewModel.AddressDetails.Country.UoM = sourcingRequestViewModel.AddressDetails.UOM;
            viewModel.RegionId = sourcingRequestViewModel.AddressDetails.DispatchRegionId;
            #endregion
            viewModel.CityGroupTerminalId = sourcingRequestViewModel.CityGroupTerminalId;
            viewModel.CustomerCompanyId = sourcingRequestViewModel.CustomerCompanyId;
            #region CustomerDetails
            viewModel.CustomerDetails.CompanyId = sourcingRequestViewModel.CustomerDetails.CompanyId;
            viewModel.CustomerDetails.CompanyName = sourcingRequestViewModel.CustomerDetails.CompanyName;
            viewModel.CustomerDetails.CustomerCompanyId = sourcingRequestViewModel.CustomerDetails.CustomerCompanyId;
            viewModel.CustomerDetails.CustomerId = sourcingRequestViewModel.CustomerDetails.CustomerId;
            viewModel.CustomerDetails.DisplayMode = sourcingRequestViewModel.CustomerDetails.DisplayMode;
            viewModel.CustomerDetails.Email = sourcingRequestViewModel.CustomerDetails.Email;
            viewModel.CustomerDetails.EntityHeaderId = sourcingRequestViewModel.CustomerDetails.EntityHeaderId;
            viewModel.CustomerDetails.EntityId = sourcingRequestViewModel.CustomerDetails.EntityId;
            viewModel.CustomerDetails.EntityIds = sourcingRequestViewModel.CustomerDetails.EntityIds;
            viewModel.CustomerDetails.EntityNumber = sourcingRequestViewModel.CustomerDetails.EntityNumber;
            viewModel.CustomerDetails.FailedStatusCode = sourcingRequestViewModel.CustomerDetails.FailedStatusCode;
            viewModel.CustomerDetails.Name = sourcingRequestViewModel.CustomerDetails.Name;
            viewModel.CustomerDetails.FullName = sourcingRequestViewModel.CustomerDetails.FullName;
            viewModel.IsInvitationEnabled = sourcingRequestViewModel.CustomerDetails.IsInvitationEnabled;
            viewModel.CustomerDetails.IsNewCompany = sourcingRequestViewModel.CustomerDetails.IsNewCompany;
            viewModel.CustomerDetails.IsNotifyDeliveries = sourcingRequestViewModel.CustomerDetails.IsNotifyDeliveries;
            viewModel.CustomerDetails.IsNotifySchedules = sourcingRequestViewModel.CustomerDetails.IsNotifySchedules;
            viewModel.CustomerDetails.OttoNotificationCount = sourcingRequestViewModel.CustomerDetails.OttoNotificationCount;
            viewModel.CustomerDetails.PhoneNumber = sourcingRequestViewModel.CustomerDetails.PhoneNumber;
            viewModel.CustomerDetails.ResponseData = sourcingRequestViewModel.CustomerDetails.ResponseData;
            viewModel.CustomerDetails.StatusCode = sourcingRequestViewModel.CustomerDetails.StatusCode;
            viewModel.CustomerDetails.StatusMessage = sourcingRequestViewModel.CustomerDetails.StatusMessage;
            viewModel.CustomerDetails.UserId = sourcingRequestViewModel.CustomerDetails.UserId;
            viewModel.CustomerDetails.ContactPersons = sourcingRequestViewModel.CustomerDetails.ContactPersons;
            #endregion
            viewModel.CustomerId = sourcingRequestViewModel.CustomerId;
            viewModel.DisplayMode = sourcingRequestViewModel.DisplayMode;
            viewModel.EntityHeaderId = sourcingRequestViewModel.EntityHeaderId;
            viewModel.EntityId = sourcingRequestViewModel.EntityId;
            viewModel.EntityIds = sourcingRequestViewModel.EntityIds;
            viewModel.EntityNumber = sourcingRequestViewModel.EntityNumber;
            viewModel.FailedStatusCode = sourcingRequestViewModel.FailedStatusCode;
            #region FuelDeliveryDetails
            viewModel.FuelDeliveryDetails.DeliveryTypeId = sourcingRequestViewModel.FuelDeliveryDetails.DeliveryTypeId;
            viewModel.FuelDeliveryDetails.EndDate = !string.IsNullOrEmpty(sourcingRequestViewModel.FuelDeliveryDetails.EndDate) ? Convert.ToDateTime(sourcingRequestViewModel.FuelDeliveryDetails.EndDate) : (DateTime?)null;
            //!string.IsNullOrEmpty(sourcingRequestViewModel.FuelDeliveryDetails.EndDate) ? Convert.ToDateTime(sourcingRequestViewModel.FuelDeliveryDetails.EndDate) : Convert.ToDateTime(sourcingRequestViewModel.FuelDeliveryDetails.StartDate);
            viewModel.FuelDeliveryDetails.EndTime = sourcingRequestViewModel.FuelDeliveryDetails.EndTime;
            viewModel.FuelDeliveryDetails.IsPrePostDipRequired = sourcingRequestViewModel.FuelDeliveryDetails.IsPrePostDipRequired;
            viewModel.FuelDeliveryDetails.OrderEnforcementId = sourcingRequestViewModel.FuelDeliveryDetails.OrderEnforcementId;
            viewModel.FuelDeliveryDetails.PaymentMethods = sourcingRequestViewModel.FuelDeliveryDetails.PaymentMethods;
            viewModel.FuelDeliveryDetails.SingleDeliverySubTypes = sourcingRequestViewModel.FuelDeliveryDetails.SingleDeliverySubTypes;
            viewModel.FuelDeliveryDetails.StartDate = Convert.ToDateTime(sourcingRequestViewModel.FuelDeliveryDetails.StartDate);/* sourcingRequestViewModel.FuelDeliveryDetails.StartDate;*/
            viewModel.FuelDeliveryDetails.StartTime = sourcingRequestViewModel.FuelDeliveryDetails.StartTime;
           
            #endregion
            #region FuelDetails
            viewModel.FuelDetails.CreatedBy = sourcingRequestViewModel.FuelDetails.CreatedBy;
            viewModel.FuelDetails.CreatedDate = sourcingRequestViewModel.FuelDetails.CreatedDate;
            viewModel.FuelDetails.CustomerCompanyId = sourcingRequestViewModel.FuelDetails.CustomerCompanyId;
            viewModel.FuelDetails.CustomerId = sourcingRequestViewModel.FuelDetails.CustomerId;
            viewModel.FuelDetails.DisplayMode = sourcingRequestViewModel.FuelDetails.DisplayMode;
            viewModel.FuelDetails.FreightOnBoard = sourcingRequestViewModel.FuelDetails.FreightOnBoard;
            viewModel.FuelDetails.FuelDisplayGroupId = sourcingRequestViewModel.FuelDetails.FuelDisplayGroupId;
            viewModel.FuelDetails.FuelDisplayJobId = sourcingRequestViewModel.FuelDetails.FuelDisplayJobId;
      
            viewModel.FuelDetails.FuelQuantity = sourcingRequestViewModel.FuelDetails.FuelQuantity;
            viewModel.FuelDetails.FuelTypeId = sourcingRequestViewModel.FuelDetails.FuelTypeId;
            viewModel.FuelDetails.IsMarineLocation = sourcingRequestViewModel.FuelDetails.IsMarineLocation;
            viewModel.FuelDetails.IsTierPricing = sourcingRequestViewModel.FuelDetails.IsTierPricing;
            viewModel.FuelDetails.NonStandardFuelDescription = sourcingRequestViewModel.FuelDetails.NonStandardFuelDescription;
            viewModel.FuelDetails.NonStandardFuelName = sourcingRequestViewModel.FuelDetails.NonStandardFuelName;
            viewModel.FuelDetails.OttoNotificationCount = sourcingRequestViewModel.FuelDetails.OttoNotificationCount;

            viewModel.FuelDetails.OrderTypeId = sourcingRequestViewModel.FuelDetails.OrderTypeId;
            viewModel.FuelDetails.ProductTypeId = sourcingRequestViewModel.FuelDetails.ProductTypeId;
            viewModel.FuelDetails.FuelQuantity.QuantityTypeId = sourcingRequestViewModel.FuelDetails.QuantityTypeId;
            viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes = sourcingRequestViewModel.FuelDetails.QuantityIndicatorTypes;
            viewModel.FuelDetails.FuelQuantity.Quantity = sourcingRequestViewModel.FuelDetails.Quantity;
            viewModel.FuelDetails.FuelQuantity.MaximumQuantity = sourcingRequestViewModel.FuelDetails.MaximumQuantity;
            viewModel.FuelDetails.FuelQuantity.MinimumQuantity = sourcingRequestViewModel.FuelDetails.MinimumQuantity;
            viewModel.FuelDetails.FuelQuantity.UoM = sourcingRequestViewModel.AddressDetails.UOM;

            viewModel.FuelDetails.FuelPricing = sourcingRequestViewModel.FuelDetails.FuelPricing;
            viewModel.FuelDetails.FuelPricing.PricingTypeId = sourcingRequestViewModel.FuelDetails.PricingTypeId;
            viewModel.FuelDetails.FuelPricing.PricePerGallon = sourcingRequestViewModel.FuelDetails.PricePerGallon;
            viewModel.FuelDetails.FuelPricing.RackAvgTypeId = sourcingRequestViewModel.FuelDetails.RackAvgTypeId;
            viewModel.FuelDetails.FuelPricing.RackPrice = sourcingRequestViewModel.FuelDetails.RackPrice;
            viewModel.FuelDetails.FuelPricing.TerminalId = sourcingRequestViewModel.FuelDetails.TerminalId;
            viewModel.FuelDetails.FuelPricing.SupplierCostMarkupValue = sourcingRequestViewModel.FuelDetails.SupplierCostMarkupValue;
            viewModel.FuelDetails.FuelPricing.RackPrice = sourcingRequestViewModel.FuelDetails.RackPrice;
            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypeId = (int)sourcingRequestViewModel.TruckLoadType;
            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = sourcingRequestViewModel.TruckLoadType;

            /*TierPricing*/
            viewModel.FuelDetails.IsTierPricing = sourcingRequestViewModel.FuelPricingDetails.IsTierPricingRequired;
            viewModel.FuelDetails.TierPricing.TierPricingType = sourcingRequestViewModel.FuelPricingDetails.TierPricing.TierPricingType;
            viewModel.FuelDetails.TierPricing.IsResetCumulation = sourcingRequestViewModel.FuelPricingDetails.TierPricing.IsResetCumulation;
            viewModel.FuelDetails.TierPricing.AboveQuantityPricing = sourcingRequestViewModel.FuelPricingDetails.TierPricing.AboveQuantityPricing;

            foreach (var pricing in sourcingRequestViewModel.FuelPricingDetails.TierPricing.Pricings)
            {
                if (pricing.PricingCode.Code.StartsWith("A"))
                {
                    pricing.PricingSourceId = 1;
                }
                else 
                {
                    pricing.PricingSourceId = 2;
                }
            }

            viewModel.FuelDetails.TierPricing.Pricings = sourcingRequestViewModel.FuelPricingDetails.TierPricing.Pricings;
            viewModel.FuelDetails.TierPricing.ResetCumulationSetting = sourcingRequestViewModel.FuelPricingDetails.TierPricing.ResetCumulationSetting;
            #endregion

            viewModel.FuelOfferDetails = sourcingRequestViewModel.FuelOfferDetails;
            viewModel.FuelOfferDetails.NetDays = sourcingRequestViewModel.FuelDeliveryDetails.NetDays;
            viewModel.FuelOfferDetails.PaymentTermId = sourcingRequestViewModel.FuelDeliveryDetails.PaymentTermId;

            viewModel.IsActive = sourcingRequestViewModel.IsActive;
            viewModel.IsAssetTracked = sourcingRequestViewModel.AdditionalDetailsViewModel.IsAssetTracked;
            viewModel.IsAssetDropStatusEnabled = sourcingRequestViewModel.AdditionalDetailsViewModel.IsAssetDropStatusEnabled;
        
            viewModel.IsSupressOrderPricing = sourcingRequestViewModel.IsSupressOrderPricing;
            viewModel.IsTaxExempted = sourcingRequestViewModel.IsTaxExempted;
            viewModel.IsNotifyDeliveries = sourcingRequestViewModel.CustomerDetails.IsNotifyDeliveries;
            viewModel.IsNotifySchedules = sourcingRequestViewModel.CustomerDetails.IsNotifySchedules;
            viewModel.UpdatedBy = sourcingRequestViewModel.UpdatedBy;
            viewModel.UpdatedDate = sourcingRequestViewModel.UpdatedDate;

            #region PricingDetails
            viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id = sourcingRequestViewModel.FuelPricingDetails.CodeId;
            viewModel.PricingDetails.FuelPricingDetails.PricingCode.Code = sourcingRequestViewModel.FuelPricingDetails.Code;
            viewModel.PricingDetails.FuelPricingDetails.PricingCode.Description = sourcingRequestViewModel.FuelPricingDetails.CodeDescription;
            viewModel.PricingDetails.PricingTypeId = sourcingRequestViewModel.FuelPricingDetails.PricingTypeId;
            viewModel.PricingDetails.PricePerGallon = sourcingRequestViewModel.FuelPricingDetails.PricePerGallon;
            viewModel.PricingDetails.RackAvgTypeId = sourcingRequestViewModel.FuelPricingDetails.RackAvgTypeId;
            viewModel.PricingDetails.RackPrice = sourcingRequestViewModel.FuelPricingDetails.RackPrice;
            viewModel.PricingDetails.TerminalId = sourcingRequestViewModel.FuelPricingDetails.TerminalId;
            viewModel.PricingDetails.TerminalName = sourcingRequestViewModel.FuelPricingDetails.TerminalName;
            viewModel.PricingDetails.SupplierCostMarkupTypeId = sourcingRequestViewModel.FuelPricingDetails.SupplierCostMarkupTypeId;
            viewModel.PricingDetails.SupplierCostMarkupValue = sourcingRequestViewModel.FuelPricingDetails.SupplierCostMarkupValue;
            viewModel.PricingDetails.CityGroupTerminalId = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalId;
            viewModel.PricingDetails.CityGroupTerminalName = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalName;
            viewModel.PricingDetails.CityGroupTerminalStateId = sourcingRequestViewModel.FuelPricingDetails.CityGroupTerminalStateId ?? 0;
            if (sourcingRequestViewModel.FuelPricingDetails.PricingTypeId == (int)PricingType.PricePerGallon)
            {
                viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id = 1;
                viewModel.PricingDetails.FuelPricingDetails.PricingCode.Code = "A-120000";
            }
            else if(sourcingRequestViewModel.FuelPricingDetails.PricingTypeId == (int)PricingType.Suppliercost)
            {
                viewModel.PricingDetails.FuelPricingDetails.PricingCode.Id = 4;
                viewModel.PricingDetails.FuelPricingDetails.PricingCode.Code = "A-140000";
            }
            #endregion

            #region FuelFees

            viewModel.FuelDeliveryDetails.FuelFees.FuelRequestFees = sourcingRequestViewModel.FuelDetails.Fees;

            #endregion

            return viewModel;
        }
    }
}
