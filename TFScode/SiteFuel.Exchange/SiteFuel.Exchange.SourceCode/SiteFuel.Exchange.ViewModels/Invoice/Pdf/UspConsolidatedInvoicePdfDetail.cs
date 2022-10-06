using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels.Invoice.Pdf
{
    public class UspConsolidatedInvoicePdfDetail
    {
        public UspPdfHeaderViewModel InvoiceHeaderDetail { get; set; } = new UspPdfHeaderViewModel();
        public List<UspPickupAddressViewModel> PickupLocations { get; set; } = new List<UspPickupAddressViewModel>();
        public List<UspBolDetail> LiftDetails { get; set; }
        public List<UspBolDetail> BolDetails { get; set; }
        public List<UspPdfDetail> InvoiceDropDetails { get; set; } = new List<UspPdfDetail>();
        public List<UspInvoicePdfFuelFee> FuelFeeDetails { get; set; }
        public List<UspInvoicePdfTaxDetail> TaxDetails { get; set; }
        public List<UspInvoicePdfSpecialInstruction> SpecialInstructions { get; set; }
        public List<UspInvoicePdfAssetDrop> AssetDetails { get; set; }
    }

    public class UspPickupAddressViewModel
    {
        public string LocationName { get; set; }
        public string PickupAddress { get; set; }
        public string PickupAddressLine2 { get; set; }
        public string PickupAddressLine3 { get; set; }
        public string PickupCity { get; set; }
        public string PickupStateCode { get; set; }
        public string PickupZipCode { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public string CountryCode { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public string TerminalName { get; set; }
    }

    public class UspBolDetail : UspPickupAddressViewModel
    {
        public int Id { get; set; }
        public string BolNumber { get; set; }
        public decimal? GrossQuantity { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? DeliveredQuantity { get; set; }
        public string Carrier { get; set; }
        public decimal? LiftQuantity { get; set; }
        public string LiftTicketNumber { get; set; }
        public DateTimeOffset? LiftDate { get; set; }
        public int? ImageId { get; set; }
        public int InvoiceId { get; set; }
        public string BadgeNumber { get; set; }

        public bool IsBolEditedForLfv { get; set; }

        public string BolEditedNotes { get; set; }
    }

    public class UspPdfHeaderViewModel
    {
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingAddressLine3 { get; set; }
        public string BillingCity { get; set; }
        public string BillingCounty { get; set; }
        public string BillingStateName { get; set; }
        public string BillingCountryName { get; set; }
        public string BillingStateCode { get; set; }
        public string BillingZipCode { get; set; }
        public string PoContactName { get; set; }
        public string PoContactEmail { get; set; }
        public string PoContactPhoneNumber { get; set; }
        public string DropAddress { get; set; }
        public string DropCity { get; set; }
        public string DropStateCode { get; set; }
        public string DropZipCode { get; set; }
        public bool IsDropLocationAvailable { get; set; }
        public string JobName { get; set; }
        public string DisplayJobID { get; set; }
        public string JobAddress { get; set; }
        public string JobAddressLine2 { get; set; }
        public string JobAddressLine3 { get; set; }
        public string JobCity { get; set; }
        public string JobStateCode { get; set; }
        public string JobZipCode { get; set; }
        public string SupplierCompanyName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierAddressCity { get; set; }
        public string SupplierAddressStateCode { get; set; }
        public string SupplierAddressZipCode { get; set; }
        public int SupplierAddressCountryId { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset PaymentDueDate { get; set; }
        public UoM UoM { get; set; }
        public Currency Currency { get; set; }
        public int PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public int NetDays { get; set; }
        public string Carrier { get; set; }
        public int CompanyLogoId { get; set; }
        public byte[] CompanyLogoData { get; set; }
        public string CompanyLogoPath { get; set; }
        public bool IsBillToEnabled { get; set; }
        public string BillToName { get; set; }
        public string BillToAddress { get; set; }
        public string BillToAddressLine2 { get; set; }
        public string BillToAddressLine3 { get; set; }
        public string BillToCity { get; set; }
        public string BillToZipCode { get; set; }
        public string BillToStateCode { get; set; }
        public string BillToStateName { get; set; }
        public string BillToCountryCode { get; set; }
        public string BillToCountryName { get; set; }
        public string AccountingCompanyId { get; set; }
        public string CarrierOrderId { get; set; }
        public string DeliveryRequestId { get; set; }
        public string RouteName { get; set; }
        public int JobCountryId { get; set; }
        public string InvoiceFooterJson { get; set; }
        public int SupplierCompanyId { get; set; }
        public string Vessel { get; set; }
        public string Berth { get; set; }
        public string BDRNumber { get; set; }

    }

    public class UspPdfDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int Version { get; set; }
        public int InvoiceVersionStatusId { get; set; }
        public int StatusId { get; set; }
        public int InvoiceTypeId { get; set; }
        public int InvoiceNumberId { get; set; }
        public string DisplayInvoiceNumber { get; set; }
        public string ReferenceId { get; set; }
        public string PoNumber { get; set; }
        public string JobName { get; set; }
        public DateTimeOffset DropStartDate { get; set; }
        public DateTimeOffset DropEndDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public decimal DroppedGallons { get; set; }
        public decimal? PricePerGallon { get; set; }
        public decimal? RackPrice { get; set; }
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
        public decimal? SupplierAllowance { get; set; }
        public int BuyerCompanyId { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
        public DateTimeOffset? CxmlCheckOutDate { get; set; }
        public string Notes { get; set; }
        public bool IsSurchargeApplicable { get; set; }
        public bool IsFreightCostApplicable { get; set; }
        public bool IsShowProductDescriptionOnInvoice { get; set; }
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
        public string SuperAdminProductDescription { get; set; }
        public string TerminalName { get; set; }
        public string PickupTerminal { get; set; }
        public int PaymentTermId { get; set; }
        public string PaymentTermName { get; set; }
        public int NetDays { get; set; }
        public bool IsFTL { get; set; }
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
        public bool IsThirdPartyHardwareUsed { get; set; }
        public bool IsAssetDropAvailable { get; set; }
        public int FuelTypeId { get; set; }
        public int ProductTypeId { get; set; }
        public string OriginalInvoiceNumber { get; set; }
        public string OriginalInvoiceQbNumber { get; set; }
        public int? OriginalInvoiceNumberId { get; set; }
        public int? OriginalInvoiceTypeId { get; set; }
        public string CreditInvoiceDisplayNumber { get; set; }
        public decimal? TotalAllowance { get; set; }
        public int CreationMethod { get; set; }
        public string DropTicketNumber { get; set; }
        public bool IsExceptionDdt { get; set; }
        public decimal? GrossQuantity { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? LiftQuantity { get; set; }
        public DateTimeOffset? PDIDetailsDate { get; set; }
        public bool IsMarineLocation { get; set; }
        public decimal? ConvertedQuantity { get; set; }
        public string FRUoM { get; set; }
        public decimal? ConvertedPricing { get; set; }
        public decimal? Gravity { get; set; }
        public decimal? GallonsPerMetricTon { get; set; }
        public string IMONumber { get; set; }
        // for new marine invoice pdf. added by Yash
        public string Berth { get; set; }
        public string Vessel { get; set; }
        public DateTimeOffset DisplayDropEndDate { get; set; } // Drop end date of last invoice for marine invoice
        public string OrderQuantity { get; set; }

        public DateTimeOffset DisplayDropStartDate { get; set; } // Drop start date of first invoice
        public string DeliveryLevelPO { get; set; }
    }

    public class UspBDRPdfDetail
    {
        public UspPdfHeaderViewModel InvoiceHeaderDetail { get; set; } = new UspPdfHeaderViewModel();
        public List<UspPickupAddressViewModel> PickupLocations { get; set; } = new List<UspPickupAddressViewModel>();
        public List<UspPdfDetail> InvoiceDropDetails { get; set; } = new List<UspPdfDetail>();
        public List<BDRDetailsModel> BDRDetailsModel { get; set; } = new List<BDRDetailsModel>();
    }
}
