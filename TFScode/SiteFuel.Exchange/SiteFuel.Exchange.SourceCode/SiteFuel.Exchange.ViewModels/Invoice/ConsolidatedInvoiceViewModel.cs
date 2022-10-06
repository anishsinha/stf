using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ConsolidatedInvoiceViewModel : BaseViewModel
    {
        public ConsolidatedInvoiceViewModel()
        {
            InstanceInitialize();
        }

        public ConsolidatedInvoiceViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CreatedDate = DateTimeOffset.Now;
            PaymentDueDate = DateTimeOffset.Now;
            StatusId = (int)InvoiceStatus.Received;    
            WaitingForAction = (int)WaitingAction.Nothing;
            QbInvoiceNumber = string.Empty;
        }

        public int Version { get; set; }

        public int UserId { get; set; }

        public int Id { get; set; }

        public int? OrderId { get; set; }

        public string JobName { get; set; }

        public string PoNumber { get; set; }

        public int InvoiceTypeId { get; set; }

        public int? FuelTypeId { get; set; }

        public string FuelType { get; set; }

        public string NonStandardFuelName { get; set; }

        public string NonStandardFuelDescription { get; set; }

        public string SuperAdminProductDescription { get; set; }

        public int ProductTypeId { get; set; }

        public int PricingTypeId { get; set; }

        public int FuelDisplayGroupId { get; set; }

        public decimal DroppedGallons { get; set; }

        public decimal PricePerGallon { get; set; }

        public decimal RackPrice { get; set; }

        public DateTimeOffset DropStartDate { get; set; }

        public DateTimeOffset DropEndDate { get; set; }

        public decimal StateTax { get; set; }

        public decimal FederalTax { get; set; }

        public decimal SalesTax { get; set; }

        public decimal BasicAmount { get; set; }

        public DateTimeOffset PaymentDueDate { get; set; }

        public DateTimeOffset? PaymentDate { get; set; }

        public bool IsWetHosingDelivery { get; set; }

        public bool IsOverWaterDelivery { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? TerminalId { get; set; }

        public string TerminalName { get; set; }

        public string PickupTerminal { get; set; }

        public int PaymentTermId { get; set; }

        public string PaymentTermName { get; set; }

        public int NetDays { get; set; }

        public int? CityGroupTerminalId { get; set; }

        public string CityGroupTerminalName { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }        

        public string TransactionId { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public bool IsRecursiveCallForBrokerOrders { get; set; }

        public int? DriverId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public string TraceId { get; set; }

        public string StatementNumber { get; set; }

        public int? StatementId { get; set; }

        public bool IsTaxServiceFailure { get; set; }

        public decimal TotalFees { get; set; }

        public string CsvFilePath { get; set; }

        public int DDTConversionReason { get; set; }

        public int WaitingForAction { get; set; }

        public DateTimeOffset TerminalPricingDate { get; set; }

        public bool IsApprovalWorkflowEnabledForJob { get; set; }

        public bool IsHidePricingEnabled { get; set; }

        public int PreviousStatusId { get; set; }

        public bool IsSupplierPreferenceDDT { get; set; }

        public string PricePerGallonDisplay { get; set; }

        public bool IsBuyPriceInvoice { get; set; }

        public string BrokeredChainId { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }

        public decimal ExchangeRate { get; set; }

        public string DisplayJobID { get; set; }

        public string WBSNumber { get; set; }
        public bool IsTrueFillTax { get; set; }

        public string DisplayInvoiceNumber { get; set; }
        public string ReferenceId { get; set; }

        public string QbInvoiceNumber { get; set; }

        public string OriginalInvoiceNumber { get; set; }

        public string OriginalInvoiceQbNumber { get; set; }

        public string CreditInvoiceDisplayNumber { get; set; }

        public int? OriginalInvoiceNumberId { get; set; }

        public bool IsRebillInvoice { get; set; }

        public int? OriginalInvoiceType { get; set; }

        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;

        public int InvoiceVersionStatusId { get; set; }

        public DateTimeOffset? CxmlCheckOutDate { get; set; }

        public string TimeZoneName { get; set; }

        public bool IsPendingInvoiceAdjustment { get; set; }

        public bool IsVariousFobOrigin { get; set; }

        public bool IsDropLocationAvailable { get; set; }

        public bool IsFTL { get; set; }
        public decimal? NetQuantity { get; set; }
        public decimal? GrossQuantity { get; set; }
        public decimal? LiftQuantity { get; set; }
        public bool IsExceptionDdt { get; set; }
        public bool IsBuyAndSellOrder { get; set; }
        public bool IsSignatureReq { get; set; }
        public bool IsBOLImageReq { get; set; }
        public bool IsDropImageReq { get; set; }
        public bool IsInvoiceImagesAvailable { get; set; }
        public bool IsTerminalPickup { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }

        public bool IsMarineLocation { get; set; }
        public decimal? ConvertedQuantity { get; set; }
        public string FRUoM { get; set; }

        public RebilledInvoiceViewModel RebilledInvoiceViewModel { get; set; }

        public BuyAndSellPricingDetailViewModel BuyAndSellPricingDetail { get; set; }

        public InvoiceNumberViewModel InvoiceNumber { get; set; } = new InvoiceNumberViewModel();

        public ImageViewModel AdditionalImage { get; set; }

        public InvoiceXAdditionalDetailViewModel AdditionalDetail { get; set; }

        public ImageViewModel Image { get; set; }

        public string PDIDetailsDate { get; set; }
        public string PDIDetailsTime { get; set; }

        public decimal? ConvertedPricing { get; set; }
        public decimal? Gravity { get; set; }

        public decimal? GallonsPerMetricTon { get; set; }

        public string IMONumber { get; set; }

        public string Berth { get; set; }

        public string Vessel { get; set; }
        public DateTimeOffset DisplayDropEndDate { get; set; }
        public string OrderQuantity { get; set; }
        public DateTimeOffset DisplayDropStartDate { get; set; }
        public string DeliveryLevelPO { get; set; }
        public bool ShouldHidePricing()
        {
            var response = IsHidePricingEnabled || WaitingForAction != (int)WaitingAction.Nothing;
            var isCustomerApproval = IsApprovalWorkflowEnabledForJob && IsDigitalDropTicket();
            response = isCustomerApproval || response || IsExceptionDdt;
            return response;
        }

        public bool IsDigitalDropTicket()
        {
            return InvoiceTypeId == (int)InvoiceType.DigitalDropTicketManual || InvoiceTypeId == (int)InvoiceType.DigitalDropTicketMobileApp;
        }

        public decimal GetBolQuantity()
        {
            if (this.NetQuantity.HasValue || this.GrossQuantity.HasValue)
            {
                return this.QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? this.NetQuantity ?? 0 : this.GrossQuantity ?? 0;
            }
            else
            {
                return 0;
            }
        }
    }
}
