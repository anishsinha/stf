using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DropAdditionalDetailsModel
    {
        public int OrderId { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public Currency Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public UoM UoM { get; set; }
        public bool IsBolImageRequired { get; set; }
        public bool IsDropImageRequired { get; set; }
        public bool SignatureEnabled { get; set; }
        public string TimeZoneName { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
		public int? TaxQuantityIndicatorTypeId { get; set; }
		public PaymentMethods PaymentMethod { get; set; }
        public DropAddressViewModel JobAddress { get; set; }
        public bool IsApprovalWorkFlowEnabled { get; set; }
        public int RequestPriceDetailId { get; set; }
        public bool IsFtl { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompanyName { get; set; }
        public string JobName { get; set; }
        public decimal MaxQuantity { get; set; }
        public int JobCompanyId { get; set; }
        public string ProductCode { get; set; }
        public int PricingTypeId { get; set; }
        public int JobId { get; set; }
        public bool IsMarineJob { get; set; }
        public bool IsOrderAutoClosed { get; set; }
        public decimal OrderTotalDelivered { get; set; }
        public decimal DropPercentPerDelivery { get; set; }
        public bool IsDtnUploaded { get; set; }
        public bool IsApprovalWorkflowEnabled { get; set; }
        public int? ApprovalUserId { get; set; }
        public int? ApprovalUserOnboardedType { get; set; }
        public int DeliveryTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public string ApprovalUserName { get; set; }
        public string JobCompanyName { get; set; }
        public int FuelTypeId { get; set; }
        public int CountryId { get; set; }
        public int FuelRequestId { get; set; }
        public int? ParentFuelRequestId { get; set; }
        public bool IsPrePostDipDataRequired { get; set; }
        public int FuelRequestTypeId { get; set; }
        public string GroupParentDrId { get; set; }
        public bool IsBdnConfirmationRequired { get; set; }
        public bool IsInvoiceConfirmationRequired { get; set; }
        public bool IsPdieTaxRequired { get; set; }
        public bool IsIncludePricingInExternalObj { get; set; }
        public PaymentDueDateType PaymentDueDateType { get; set; } = PaymentDueDateType.InvoiceCreationDate;
    }
}
