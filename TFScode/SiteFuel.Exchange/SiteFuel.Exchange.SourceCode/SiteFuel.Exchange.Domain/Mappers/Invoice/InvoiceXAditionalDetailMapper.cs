using Newtonsoft.Json;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System.Web.UI.WebControls;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class InvoiceXAditionalDetailMapper
    {
        public static InvoiceXAdditionalDetailViewModel ToViewModel(this InvoiceXAdditionalDetail entity, InvoiceXAdditionalDetailViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new InvoiceXAdditionalDetailViewModel();

            viewModel.InvoiceId = entity.InvoiceId;
            viewModel.DriverComment = entity.DriverComment;
            viewModel.Latitude = entity.Latitude;
            viewModel.Longitude = entity.Longitude;
            viewModel.AssignedBy = entity.AssignedBy;
            viewModel.AssignedDate = entity.AssignedDate;
            viewModel.AssetFilled = entity.AssetFilled;
            viewModel.PoContactId = entity.PoContactId;
            viewModel.PoContactName = entity.PoContactName;
            viewModel.PoContactEmail = entity.PoContactEmail;
            viewModel.PoContactPhoneNumber = entity.PoContactPhoneNumber;
            viewModel.JobId = entity.JobId;
            viewModel.DisplayJobID = entity.DisplayJobID;
            if (!string.IsNullOrWhiteSpace(entity.CustomAttribute))
            {
                viewModel.CustomAttributeViewModel = JsonConvert.DeserializeObject<InvoiceCustomAttributeViewModel>(entity.CustomAttribute);
            }
            viewModel.CustomAttribute = entity.CustomAttribute;
            viewModel.JobName = entity.JobName;
            viewModel.JobAddress = entity.JobAddress;
            viewModel.JobAddressLine2 = entity.JobAddressLine2;
            viewModel.JobAddressLine3 = entity.JobAddressLine3;
            viewModel.JobCity = entity.JobCity;
            viewModel.JobStateCode = entity.JobStateCode;
            viewModel.JobStateName = entity.JobStateName;
            viewModel.JobCountryCode = entity.JobCountryCode;
            viewModel.JobCountryName = entity.JobCountryName;
            viewModel.JobZipCode = entity.JobZipCode;
            viewModel.BillingAddessId = entity.BillingAddessId;
            viewModel.BillingAddress = entity.BillingAddress;
            viewModel.BillingAddressLine2 = entity.BillingAddressLine2;
            viewModel.BillingAddressLine3 = entity.BillingAddressLine3;
            viewModel.BillingCity = entity.BillingCity;
            viewModel.BillingStateCode = entity.BillingStateCode;
            viewModel.BillingStateName = entity.BillingStateName;
            viewModel.BillingCountryCode = entity.BillingCountryCode;
            viewModel.BillingCountryName = entity.BillingCountryName;
            viewModel.BillingZipCode = entity.BillingZipCode;
            viewModel.CxmlCheckOutDate = entity.CxmlCheckOutDate;
            viewModel.SplitLoadChainId = entity.SplitLoadChainId;
            viewModel.SplitLoadSequence = entity.SplitLoadSequence;
            viewModel.Notes = entity.Notes;
            viewModel.PaymentMethod = entity.PaymentMethod;
            viewModel.DropTicketNumber = entity.DropTicketNumber;
            viewModel.TankFrequencyId = entity.TankFrequencyId;
            viewModel.TruckNumber = entity.TruckNumber;
            viewModel.DropTicketNumber = entity.DropTicketNumber;
            viewModel.CreationMethod = entity.CreationMethod;
            viewModel.SupplierAllowance = entity.SupplierAllowance;
            viewModel.TotalAllowance = entity.TotalAllowance;

            viewModel.IsJobSpecificBillToEnabled = entity.IsJobSpecificBillToEnabled;
            viewModel.BillToAddress = entity.BillToAddress;
            viewModel.BillToAddressLine2 = entity.BillToAddressLine2;
            viewModel.BillToAddressLine3 = entity.BillToAddressLine3;
            viewModel.BillToCity = entity.BillToCity;
            viewModel.BillToZipCode = entity.BillToZipCode;
            viewModel.BillToStateCode = entity.BillToStateCode;
            viewModel.BillToStateName = entity.BillToStateName;
            viewModel.BillToCountryCode = entity.BillToCountryCode;
            viewModel.BillToCountryName = entity.BillToCountryName;
            viewModel.BillToName = entity.BillToName;

            viewModel.IsSiteOutOfFuel = entity.IsSiteOutOfFuel;
            viewModel.OutOfFuelProduct = entity.OutOfFuelProduct;
            viewModel.Tracktor = entity.Tracktor;
            viewModel.LoadingBadge = entity.LoadingBadge;
            viewModel.CarrierOrderId = entity.CarrierOrderId;
            viewModel.CarrierOrder = entity.CarrierOrder;
            viewModel.OrderDate = entity.OrderDate;
            viewModel.OrderQuantity = entity.OrderQuantity;
            viewModel.ExternalRefId = entity.ExternalRefID;
            viewModel.NoDataExceptionApprovalId = entity.NoDataExceptionApprovalId;
            viewModel.PDIOrderId = entity.PDIDeliveryOrderNo;
            viewModel.IsShowProductDescriptionOnInvoice = entity.IsShowProductDescriptionOnInvoice;
            viewModel.FreightPricingMethod = entity.FreightPricingMethod;
            return viewModel;
        }

        public static InvoiceXAdditionalDetail ToEntity(this InvoiceXAdditionalDetailViewModel viewModel, InvoiceXAdditionalDetail entity = null)
        {
            if (entity == null)
                entity = new InvoiceXAdditionalDetail();

            entity.DriverComment = viewModel.DriverComment;
            entity.Latitude = viewModel.Latitude;
            entity.Longitude = viewModel.Longitude;
            entity.AssignedBy = viewModel.AssignedBy;
            entity.AssignedDate = viewModel.AssignedDate;
            entity.AssetFilled = viewModel.AssetFilled;
            entity.PoContactId = viewModel.PoContactId;
            entity.PoContactName = viewModel.PoContactName;
            entity.PoContactEmail = viewModel.PoContactEmail;
            entity.PoContactPhoneNumber = viewModel.PoContactPhoneNumber;
            entity.JobId = viewModel.JobId;
            entity.DisplayJobID = viewModel.DisplayJobID;
            entity.CustomAttribute = viewModel.CustomAttribute;
            entity.JobName = viewModel.JobName;
            entity.JobAddress = viewModel.JobAddress;
            entity.JobAddressLine2 = viewModel.JobAddressLine2;
            entity.JobAddressLine3 = viewModel.JobAddressLine3;
            entity.JobCity = viewModel.JobCity;
            entity.JobStateCode = viewModel.JobStateCode;
            entity.JobStateName = viewModel.JobStateName;
            entity.JobCountryCode = viewModel.JobCountryCode;
            entity.JobCountryName = viewModel.JobCountryName;
            entity.JobZipCode = viewModel.JobZipCode;
            entity.BillingAddessId = viewModel.BillingAddessId;
            entity.BillingAddress = viewModel.BillingAddress;
            entity.BillingAddressLine2 = viewModel.BillingAddressLine2;
            entity.BillingAddressLine3 = viewModel.BillingAddressLine3;
            entity.BillingCity = viewModel.BillingCity;
            entity.BillingStateCode = viewModel.BillingStateCode;
            entity.BillingStateName = viewModel.BillingStateName;
            entity.BillingCountryCode = viewModel.BillingCountryCode;
            entity.BillingCountryName = viewModel.BillingCountryName;
            entity.BillingZipCode = viewModel.BillingZipCode;
            entity.CxmlCheckOutDate = viewModel.CxmlCheckOutDate;
            entity.SplitLoadChainId = viewModel.SplitLoadChainId;
            entity.SplitLoadSequence = viewModel.SplitLoadSequence;
            entity.Notes = viewModel.Notes;
            entity.PaymentMethod = viewModel.PaymentMethod;
            entity.TankFrequencyId = viewModel.TankFrequencyId;
            entity.TruckNumber = viewModel.TruckNumber;
            entity.DropTicketNumber = viewModel.DropTicketNumber;
            entity.CreationMethod = viewModel.CreationMethod;
            entity.OriginalInvoiceId = viewModel.OriginalInvoiceId;
            entity.OriginalInvoiceHeaderId = viewModel.OriginalInvoiceHeaderId;
            entity.SupplierAllowance = viewModel.SupplierAllowance.HasValue && viewModel.SupplierAllowance.Value > 0 ? viewModel.SupplierAllowance : null;
            entity.TotalAllowance = viewModel.TotalAllowance;
            entity.IsJobSpecificBillToEnabled = viewModel.IsJobSpecificBillToEnabled;
            entity.BillToAddress = viewModel.BillToAddress;
            entity.BillToAddressLine2 = viewModel.BillToAddressLine2;
            entity.BillToAddressLine3 = viewModel.BillToAddressLine3;
            entity.BillToCity = viewModel.BillToCity;
            entity.BillToZipCode = viewModel.BillToZipCode;
            entity.BillToStateCode = viewModel.BillToStateCode;
            entity.BillToStateName = viewModel.BillToStateName;
            entity.BillToCountryCode = viewModel.BillToCountryCode;
            entity.BillToCountryName = viewModel.BillToCountryName;
            entity.BillToName = viewModel.BillToName;

            entity.IsSiteOutOfFuel = viewModel.IsSiteOutOfFuel;
            entity.OutOfFuelProduct = viewModel.OutOfFuelProduct;
            entity.Tracktor = viewModel.Tracktor;
            entity.LoadingBadge = viewModel.LoadingBadge;
            entity.CarrierOrderId = viewModel.CarrierOrderId;
            entity.CarrierOrder = viewModel.CarrierOrder;
            entity.OrderDate = viewModel.OrderDate;
            entity.OrderQuantity = viewModel.OrderQuantity;
            entity.ExternalRefID = viewModel.ExternalRefId;
            entity.NoDataExceptionApprovalId = viewModel.NoDataExceptionApprovalId;
            entity.IsShowProductDescriptionOnInvoice = viewModel.IsShowProductDescriptionOnInvoice;
            entity.FreightPricingMethod = viewModel.FreightPricingMethod;
            entity.PDIDeliveryOrderNo = viewModel.PDIOrderId;
            return entity;
        }

        public static InvoiceXAdditionalDetailViewModel Clone(this InvoiceXAdditionalDetailViewModel entity, InvoiceXAdditionalDetailViewModel viewModel)
        {
            if (viewModel == null)
            {
                viewModel = new InvoiceXAdditionalDetailViewModel();
            }
            viewModel.PoContactId = entity.PoContactId;
            viewModel.PoContactName = entity.PoContactName;
            viewModel.PoContactEmail = entity.PoContactEmail;
            viewModel.PoContactPhoneNumber = entity.PoContactPhoneNumber;
            viewModel.JobId = entity.JobId;
            viewModel.DisplayJobID = entity.DisplayJobID;
            viewModel.CustomAttribute = entity.CustomAttribute;
            if (!string.IsNullOrWhiteSpace(entity.CustomAttribute))
            {
                viewModel.CustomAttributeViewModel = JsonConvert.DeserializeObject<InvoiceCustomAttributeViewModel>(entity.CustomAttribute);
            }
            viewModel.JobName = entity.JobName;
            viewModel.JobAddress = entity.JobAddress;
            viewModel.JobAddressLine2 = entity.JobAddressLine2;
            viewModel.JobAddressLine3 = entity.JobAddressLine3;
            viewModel.JobCity = entity.JobCity;
            viewModel.JobStateCode = entity.JobStateCode;
            viewModel.JobStateName = entity.JobStateName;
            viewModel.JobCountryCode = entity.JobCountryCode;
            viewModel.JobCountryName = entity.JobCountryName;
            viewModel.JobZipCode = entity.JobZipCode;
            viewModel.BillingAddessId = entity.BillingAddessId;
            viewModel.BillingAddress = entity.BillingAddress;
            viewModel.BillingAddressLine2 = entity.BillingAddressLine2;
            viewModel.BillingAddressLine3 = entity.BillingAddressLine3;
            viewModel.BillingCity = entity.BillingCity;
            viewModel.BillingStateCode = entity.BillingStateCode;
            viewModel.BillingStateName = entity.BillingStateName;
            viewModel.BillingCountryCode = entity.BillingCountryCode;
            viewModel.BillingCountryName = entity.BillingCountryName;
            viewModel.SplitLoadSequence = entity.SplitLoadSequence;
            viewModel.SplitLoadChainId = entity.SplitLoadChainId;
            viewModel.BillingZipCode = entity.BillingZipCode;
            viewModel.CxmlCheckOutDate = entity.CxmlCheckOutDate;
            viewModel.Notes = entity.Notes;
            viewModel.TruckNumber = entity.TruckNumber;
            viewModel.DropTicketNumber = entity.DropTicketNumber;
            viewModel.SupplierAllowance = entity.SupplierAllowance;
            viewModel.TotalAllowance = entity.TotalAllowance;
            viewModel.ActualDropQuantity = entity.ActualDropQuantity;

            viewModel.IsJobSpecificBillToEnabled = entity.IsJobSpecificBillToEnabled;
            viewModel.BillToName = entity.BillToName;
            viewModel.BillToAddress = entity.BillToAddress;
            viewModel.BillToAddressLine2 = entity.BillToAddressLine2;
            viewModel.BillToAddressLine3 = entity.BillToAddressLine3;
            viewModel.BillToCity = entity.BillToCity;
            viewModel.BillToZipCode = entity.BillToZipCode;
            viewModel.BillToStateCode = entity.BillToStateCode;
            viewModel.BillToStateName = entity.BillToStateName;
            viewModel.BillToCountryCode = entity.BillToCountryCode;
            viewModel.BillToCountryName = entity.BillToCountryName;

            viewModel.IsSiteOutOfFuel = entity.IsSiteOutOfFuel;
            viewModel.OutOfFuelProduct = entity.OutOfFuelProduct;
            viewModel.Tracktor = entity.Tracktor;
            viewModel.LoadingBadge = entity.LoadingBadge;
            viewModel.CarrierOrderId = entity.CarrierOrderId;
            viewModel.CarrierOrder = entity.CarrierOrder;
            viewModel.OrderDate = entity.OrderDate;
            viewModel.OrderQuantity = entity.OrderQuantity;
            viewModel.ExternalRefId = entity.ExternalRefId;
            viewModel.OriginalInvoiceId = entity.OriginalInvoiceId;
            viewModel.IsShowProductDescriptionOnInvoice = entity.IsShowProductDescriptionOnInvoice;
            entity.FreightPricingMethod = viewModel.FreightPricingMethod;
            entity.FreightRateRuleType = viewModel.FreightRateRuleType;
            entity.FreightRateTableType = viewModel.FreightRateTableType;
            entity.FreightRateRuleId = viewModel.FreightRateRuleId;
            entity.FuelSurchargeTableType = viewModel.FuelSurchargeTableType;
            entity.FuelSurchargeTableId = viewModel.FuelSurchargeTableId;
            return viewModel;
        }
    }
}
