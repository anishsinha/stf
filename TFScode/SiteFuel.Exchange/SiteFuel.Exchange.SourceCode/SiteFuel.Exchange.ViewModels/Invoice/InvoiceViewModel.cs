using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class InvoiceViewModel : BaseViewModel
    {
        public InvoiceViewModel()
        {
            InstanceInitialize();
        }

        public InvoiceViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CreatedDate = DateTimeOffset.Now;
            PaymentDueDate = DateTimeOffset.Now;
            StatusId = (int)InvoiceStatus.Received;
            SpecialInstructions = new List<InvoiceXSpecialInstructionViewModel>();
            InvoiceNumber = new InvoiceNumberViewModel();
            TaxDetails = new InvoiceTaxDetailsViewModel();
            WaitingForAction = (int)WaitingAction.Nothing;
            QbInvoiceNumber = string.Empty;
            DivertedOrderIds = new List<int>();
        }

        public int Version { get; set; }

        public int UserId { get; set; }

        public int Id { get; set; }

        public int InvoiceHeaderId { get; set; }

        public int? OrderId { get; set; }

        public string PoNumber { get; set; }
        public string SupplierInvoiceNumber { get; set; }

        public InvoiceNumberViewModel InvoiceNumber { get; set; }

        public int InvoiceTypeId { get; set; }

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

        public int? CityGroupTerminalId { get; set; }

        public string CityGroupTerminalName { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public ImageViewModel Image { get; set; }
        public ImageViewModel BolImage { get; set; }

        public ImageViewModel AdditionalImage { get; set; }
        public ImageViewModel TaxAffidavitImage { get; set; }
        public ImageViewModel BDNImage { get; set; }
        public ImageViewModel CoastGuardInspectionImage { get; set; }
        public ImageViewModel InspectionRequestVoucherImage { get; set; }
        public InvoiceXAdditionalDetailViewModel AdditionalDetail { get; set; }

        public List<InvoiceXSpecialInstructionViewModel> SpecialInstructions { get; set; }

        public string TransactionId { get; set; }

        public decimal TotalTaxAmount { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public InvoiceTaxDetailsViewModel TaxDetails { get; set; }

        public bool IsRecursiveCallForBrokerOrders { get; set; }

        public int? DriverId { get; set; }

        public int? TrackableScheduleId { get; set; }

        public string TraceId { get; set; }

        public List<int> DivertedOrderIds { get; set; }

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

        public CustomerSignatureViewModel Signature { get; set; }

        public CreationMethod CreationMethod { get; set; } = CreationMethod.SFX;

        public int InvoiceVersionStatusId { get; set; }

        public DateTimeOffset? CxmlCheckOutDate { get; set; }

        public string TimeZoneName { get; set; }

        public bool IsPendingInvoiceAdjustment { get; set; }

        public BolDetailViewModel BolDetails { get; set; } = new BolDetailViewModel();

        public List<BolDetailViewModel> BolLiftDetails { get; set; } = new List<BolDetailViewModel>();

        public List<InvoiceLineItemDetailViewModel> InvoiceLineItemDetails { get; set; } = new List<InvoiceLineItemDetailViewModel>();

        public bool IsVariousFobOrigin { get; set; }

        public bool IsDropLocationAvailable { get; set; }

        public DropAddressViewModel DropAddress { get; set; }

        public DispatchLocationViewModel FuelPickLocation { get; set; }

        public bool IsFTL { get; set; }

        public FuelSurchargeFreightFeeViewModel FuelSurchargeFreightFee { get; set; }

        public RebilledInvoiceViewModel RebilledInvoiceViewModel { get; set; }
        public bool IsExceptionDdt { get; set; }

        public bool IsSignatureReq { get; set; }
        public bool IsBOLImageReq { get; set; }
        public bool IsDropImageReq { get; set; }
        public bool IsInvoiceImagesAvailable { get; set; }
        public bool IsTerminalPickup { get; set; }
        public int? QuantityIndicatorTypeId { get; set; }
        public decimal? ConvertedQuantity { get; set; }
        public bool IsBDNConfirmationRequired { get; set; }
        public bool IsInvoiceConfirmationRequired { get; set; }
        public List<string> CommonNote { get; set; }
        public List<string> DispatcherNotes { get; set; }
        public TimeSpan? LiftStartTime { get; set; }

        public TimeSpan? LiftEndTime { get; set; }
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
            if (BolDetails != null)
            {
                return QuantityIndicatorTypeId == (int)QuantityIndicatorTypes.Net ? BolDetails.NetQuantity ?? 0 : BolDetails.GrossQuantity ?? 0;
            }
            else
            {
                return 0;
            }
        }

        public bool shouldShowBDNConfirmationDropdown()
        {
            bool shouldshow = false;
            if (WaitingForAction == (int)WaitingAction.AllDRCompletion && IsBDNConfirmationRequired)
            {
                shouldshow = true;
            }
            else if (WaitingForAction == (int)WaitingAction.BDNConfirmation)
            {
                shouldshow = true;
            }
            return shouldshow;
        }
        public bool shouldShowInvoiceConfirmationDropdown()
        {
            bool shouldshow = false;
            if (WaitingForAction == (int)WaitingAction.AllDRCompletion && IsInvoiceConfirmationRequired)
            {
                shouldshow = true;
            }
            else if (WaitingForAction == (int)WaitingAction.InvoiceConfirmation)
            {
                shouldshow = true;
            }
            return shouldshow;
        }

        public bool shouldShowConvertToInvoiceWithoutTax()
        {
            bool shouldshow = false;
            if (WaitingForAction == (int)WaitingAction.AvalaraTax || WaitingForAction == (int)WaitingAction.PDITaxes)
            {
                shouldshow = true;
            }
            return shouldshow;
        }
    }
}
