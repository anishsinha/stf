using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class ThirdPartyOrderMapper
    {
        public static UserViewModel ToUserViewModel(this TPOCustomerViewModel entity, UserViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new UserViewModel(AuthStatus.Success);

           
            return viewModel;
        }

        public static TPOBrokeredOrderViewModel ToViewModel(this ExternalBrokerOrderDetail entity, TPOBrokeredOrderViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TPOBrokeredOrderViewModel();

            viewModel.InvoicePreferenceId = entity.InvoicePreferenceId;
            viewModel.CustomerNumber = entity.CustomerNumber;
            viewModel.ProductCode = entity.ProductCode;
            viewModel.ShipTo = entity.ShipTo;
            viewModel.Source = entity.Source;
            viewModel.VendorId = entity.VendorId;
            viewModel.IsActive = entity.IsActive;
            viewModel.UpdatedDate = entity.UpdatedDate;
            viewModel.CustomerId = entity.Order.ExternalBrokerId;
            viewModel.OrderId = entity.OrderId;
            viewModel.ThirdPartyNozzleId = entity.ThirdPartyNozzleId;
            return viewModel;
        }

        public static TPOBrokeredCustomerViewModel ToViewModel(this ExternalBroker entity, TPOBrokeredCustomerViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new TPOBrokeredCustomerViewModel();

            viewModel.Address = entity.Address;
            viewModel.City = entity.City;
            viewModel.CustomerCompanyName = entity.CompanyName;
            viewModel.CountryId = entity.CountryId;
            viewModel.CustomerEmail = entity.Email;
            viewModel.StateId = entity.StateId;
            viewModel.PhoneNumber = entity.PhoneNumber;
            viewModel.StateId = entity.StateId;
            viewModel.ZipCode = entity.ZipCode;

            return viewModel;
        }

        public static ExternalBrokerOrderDetail ToEntity(this TPOBrokeredOrderViewModel viewModel, ExternalBrokerOrderDetail entity = null)
        {
            if (entity == null)
                entity = new ExternalBrokerOrderDetail();

            entity.InvoicePreferenceId = viewModel.InvoicePreferenceId;
            entity.CustomerNumber = viewModel.CustomerNumber;
            entity.ProductCode = viewModel.ProductCode;
            entity.ShipTo = viewModel.ShipTo;
            entity.Source = viewModel.Source;
            entity.VendorId = viewModel.VendorId;
            entity.IsActive = viewModel.IsActive;
            entity.UpdatedDate = viewModel.UpdatedDate;

            return entity;
        }

        public static ThirdPartyOrderViewModel ToThirdPartyViewModel(this UspTPOViewModel entity, ThirdPartyOrderViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new ThirdPartyOrderViewModel();

            viewModel.CustomerDetails.IsNewCompany = false;
            viewModel.CustomerDetails.CompanyId = entity.BuyerCompanyId;
            viewModel.CustomerDetails.CompanyName = entity.BuyerCompanyName;
            viewModel.CustomerDetails.UserId = entity.UserId;
            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.TruckLoadTypes = entity.IsFTL ? TruckLoadTypes.FullTruckLoad : TruckLoadTypes.LessTruckLoad;
            viewModel.FuelDetails.FuelPricing.FuelPricingDetails.FreightOnBoardTypes = (FreightOnBoardTypes)entity.FreightOnBoardTypeId;
            viewModel.AddressDetails.IsNewJob = false;
            if (entity.IsMarineLocation)
            {
                viewModel.AddressDetails.IsMarineLocation = entity.IsMarineLocation;
                viewModel.AddressDetails.MarineUoM = (UoM)entity.UoM;
                viewModel.FuelDetails.IsMarineLocation = entity.IsMarineLocation;
                viewModel.OrderAdditionalDetailsViewModel.IsPdiTaxRequired = entity.IsPDITaxRequired;              
                viewModel.DefaultInvoiceType = (int)InvoiceType.Manual;
                viewModel.OrderAdditionalDetailsViewModel.DestinedForInternationalWaters = entity.InternationalWatersType;
                viewModel.OrderAdditionalDetailsViewModel.Berth = entity.Berth;
                viewModel.OrderAdditionalDetailsViewModel.IsManualBDNConfirmationRequired = entity.IsBDNConfirmationRequired;
                viewModel.OrderAdditionalDetailsViewModel.IsManualInvoiceConfirmationRequired = entity.IsInvoiceConfirmationRequired;
                viewModel.AddressDetails.VessleId = entity.VessleId;
                viewModel.AddressDetails.IMONumber = entity.IMONumber;
                viewModel.AddressDetails.Flag = entity.Flag;
                viewModel.FuelDetails.FuelQuantity.UoM = (UoM)entity.UoM;
                viewModel.FuelDeliveryDetails.OrderEnforcementId = OrderEnforcement.NoEnforcement;
            }
            viewModel.AddressDetails.JobId = entity.JobId;
            viewModel.AddressDetails.OnsiteContactUserId = entity.OnsiteContactUserId;
            viewModel.AddressDetails.OnsiteFirstName = entity.OnsiteFirstName;
            viewModel.AddressDetails.OnsiteLastName = entity.OnsiteLastName;
            viewModel.IsTaxExempted = entity.IsTaxExempted;
            viewModel.FuelDeliveryDetails.CustomAttributeViewModel.WBSNumber = entity.WBSNumber;
            viewModel.FuelDetails.FuelDisplayGroupId = entity.ProductDisplayGroupId;
            viewModel.FuelDetails.FuelQuantity.QuantityTypeId = entity.QuantityTypeId;
            if(entity.QuantityTypeId == (int)QuantityType.Range)
            {
                viewModel.FuelDetails.FuelQuantity.MinimumQuantity = entity.MinQuantity;
                viewModel.FuelDetails.FuelQuantity.MaximumQuantity = entity.MaxQuantity;
            }
            else if(entity.QuantityTypeId == (int)QuantityType.SpecificAmount)
            {
                viewModel.FuelDetails.FuelQuantity.Quantity= entity.MaxQuantity;
            }
            viewModel.FuelDetails.FuelQuantity.QuantityIndicatorTypes = entity.PricingQuantityIndicatorTypeId;
            viewModel.FuelDeliveryDetails.DeliveryTypeId = entity.DeliveryTypeId;
            viewModel.FuelOfferDetails.PaymentTermId = entity.PaymentTermId;
            viewModel.FuelOfferDetails.NetDays = entity.NetDays;
            viewModel.FuelDeliveryDetails.PaymentMethods = entity.PaymentMethod;
            viewModel.OrderAdditionalDetailsViewModel.BOLInvoicePreferenceTypes = entity.BOLInvoicePreferenceId;
            viewModel.Carrier = new CarrierViewModel { Id = entity.CarrierId ?? 0, Name = entity.Carrier };
            viewModel.OrderAdditionalDetailsViewModel.SupplierSource = new SupplierSourceViewModel 
                     { Id = entity.SupplierSourceId ?? 0, Name = entity.SupplierSourceName, ContractNumber = entity.SupplierContract };
            viewModel.OrderAdditionalDetailsViewModel.LoadCode = entity.LoadCode;
            viewModel.OrderAdditionalDetailsViewModel.Notes = entity.Notes;
            viewModel.OrderAdditionalDetailsViewModel.DRNotes = entity.DRNotes;
            viewModel.FuelOfferDetails.OrderClosingThreshold = entity.OrderClosingThreshold;
            viewModel.IsAssetTracked = entity.IsAssetTracked;
            viewModel.IsAssetDropStatusEnabled = entity.IsAssetDropStatusEnabled;
            viewModel.OrderAdditionalDetailsViewModel.IsDriverToUpdateBOL = entity.IsDriverToUpdateBOL;
            viewModel.OrderAdditionalDetailsViewModel.FreightPricingMethod = entity.FreightPricingMethod;
            viewModel.FuelDeliveryDetails.IsBolImageRequired = entity.IsBolImageRequired;
            viewModel.FuelDeliveryDetails.IsDropImageRequired = entity.IsDropImageRequired;
            viewModel.AddressDetails.SignatureEnabled = entity.SignatureEnabled;
            viewModel.BillToInfo.Address = entity.BillToAddress;
            viewModel.BillToInfo.City = entity.BillToCity;
            viewModel.BillToInfo.State = new StateViewModel { Id = entity.BillToStateId ?? 0 };
            viewModel.BillToInfo.Country = new CountryViewModel { Id = entity.BillToCountryId ?? 0 };
            viewModel.BillToInfo.Name = entity.BillToName;
            viewModel.BillToInfo.ZipCode = entity.BillToZipCode;

            viewModel.IsSupressOrderPricing = entity.IsSuppressPricingEnabled;
            if (entity.FuelRequestTypeId != (int)FuelRequestType.ThirdPartyRequest && viewModel.IsSupressOrderPricing)
                viewModel.IsSupressOrderPricing = false;
            if (entity.LocationInventoryManagedBy != null)
            {
                viewModel.LocationInventoryManagedBy = new System.Collections.Generic.List<LocationInventoryManagedBy>();
                viewModel.LocationInventoryManagedBy.Add((LocationInventoryManagedBy)entity.LocationInventoryManagedBy);
            }
            return viewModel;
        }
    }
}
