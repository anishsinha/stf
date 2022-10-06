using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Invoice.Pdf
{
    public class UspInvoicePdfDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Version { get; set; }
        public int InvoiceVersionStatusId { get; set; }
        public int InvoiceTypeId { get; set; }
        public int InvoiceNumberId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string PoNumber { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal PricePerGallon { get; set; }
        public decimal RackPrice { get; set; }
        public decimal BasicAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal TotalFeeAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public bool IsWetHosingDelivery { get; set; }
        public bool IsOverWaterDelivery { get; set; }
        public string QbInvoiceNumber { get; set; }
        public decimal StateTax { get; set; }
        public decimal FedTax { get; set; }
        public decimal SalesTax { get; set; }
        public int WaitingFor { get; set; }
        public int? ImageId { get; set; }
        public string BolNumber { get; set; }
        public string Carrier { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? GrossQuantity { get; set; }
        public decimal? LiftQuantity { get; set; }
        public decimal? SupplierAllowance { get; set; }
        public string SupplierCompanyName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierAddressCity { get; set; }
        public string SupplierAddressStateCode { get; set; }
        public string SupplierAddressZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingAddressLine3 { get; set; }
        public string BillingCity { get; set; }
        public string BillingStateCode { get; set; }
        public string BillingZipCode { get; set; }
        public string JobName { get; set; }
        public string DisplayJobID { get; set; }
        public string JobAddress { get; set; }
        public string JobAddressLine2 { get; set; }
        public string JobAddressLine3 { get; set; }
        public string JobCity { get; set; }
        public string JobStateCode { get; set; }
        public string JobZipCode { get; set; }
        public string PoContactName { get; set; }
        public string PoContactEmail { get; set; }
        public string PoContactPhoneNumber { get; set; }
        public DateTimeOffset? CxmlCheckOutDate { get; set; }
        public string Notes { get; set; }
        public bool IsSurchargeApplicable { get; set; }
        public int SurchargePricingType { get; set; }
        public decimal? SurchargePercentage { get; set; }
        public decimal? SurchargeEIAPrice { get; set; }
        public string WBSNumber { get; set; }
        public bool IsTrueFillTax { get; set; }
        public int PricingTypeId { get; set; }
        public string FuelRequestPPG { get; set; }
        public int UoM { get; set; }
        public int Currency { get; set; }
        public int ProductDisplayGroupId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string TerminalName { get; set; }
        public string PickupTerminal { get; set; }
        public int PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public int NetDays { get; set; }
        public bool IsFTL { get; set; }
        public int CompanyLogoId { get; set; }
        public byte[] CompanyLogoData { get; set; }
        public bool IsVariousFobOrigin { get; set; }
        public bool IsApprovalWorkflowEnabled { get; set; }
        public bool IsHidePricingEnabledForBuyer { get; set; }
        public bool IsHidePricingEnabledForSupplier { get; set; }
        public bool IsBuyAndSellOrder { get; set; }
        public bool IsBuyPriceInvoice { get; set; }
        public decimal BasePrice { get; set; }
        public decimal BrokerMarkUp { get; set; }
        public decimal SupplierMarkUp { get; set; }
        public string StatementNumber { get; set; }
        public int? StatementId { get; set; }
        public string PickupAddress { get; set; }
        public string PickupCity { get; set; }
        public string PickupStateCode { get; set; }
        public string PickupZipCode { get; set; }
        public int PickupLocationType { get; set; }
        public string DropAddress { get; set; }
        public string DropCity { get; set; }
        public string DropStateCode { get; set; }
        public string DropZipCode { get; set; }
        public bool IsDropLocationAvailable { get; set; }
        public bool IsThirdPartyHardwareUsed { get; set; }
        public bool IsAssetDropAvailable { get; set; }
        public bool IsInstructionAvailable { get; set; }
        public decimal? TotalAllowance { get; set; }
        public int FuelTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public string OriginalInvoiceQbNumber { get; set; }
        public string CreditInvoiceDisplayNumber { get; set; }
        public int? OriginalInvoiceNumberId { get; set; }
        public int? OriginalInvoiceTypeId { get; set; }
        public  int CreationMethod { get; set; }
        public string LiftTicketNumber { get; set; }
        public string DropTicketNumber { get; set; }
        public InvoiceStatus StatusId { get; set; }
        public bool IsExceptionDdt { get; set; }
        public string BulkPlantName { get; set; }

        public bool IsBillToEnabled { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToCity { get; set; }
        public string BillToZipCode { get; set; }
        public string BillToStateCode { get; set; }
        public string BillToStateName { get; set; }
        public string BillToCountryCode { get; set; }
        public string BillToCountryName { get; set; }
        public string BillToCounty { get; set; }
        public string CarrierOrderId { get; set; }

    }
}
