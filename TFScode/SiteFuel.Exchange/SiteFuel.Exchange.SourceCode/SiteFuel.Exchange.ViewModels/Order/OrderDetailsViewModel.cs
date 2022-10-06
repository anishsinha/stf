using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using SiteFuel.Exchange.ViewModels.Job;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderDetailsViewModel : BaseCultureViewModel
    {
        public OrderDetailsViewModel()
        {
            InstanceInitialize();
        }

        public OrderDetailsViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            JobLocation = new AddressViewModel(status);
            ContactPersons = new List<ContactPersonViewModel>();
            Supplier = new ContactPersonViewModel(status);
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel(status);
            Qualifications = new List<int>();
            Invoices = new List<DropdownDisplayItem>();
            DropTickets = new List<DropdownDisplayItem>();
            FuelRequestFees = new FuelRequestFeeViewModel(status);
            DiscountEarlyPayment = new List<PaymentDiscountViewModel>();
            DeliveryFeeByQuantity = new List<DeliveryFeeByQuantityViewModel>();
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
            AdditionalFees = new List<AdditionalFeeViewModel>();
            DeliverySchedules = new List<DeliveryScheduleViewModel>();
            FuelRequestResale = new FuelRequestResaleViewModel(status);
            ResaleFee = new List<FuelRequestResaleFeeViewModel>();
            NextDeliverySchedule = new List<NextDeliveryScheduleViewModel>();
            OrderLicenses = new List<int>();
            FuelDetails = new FuelDetailsViewModel();
            ExternalBrokeredOrder = new TPOBrokeredOrderViewModel();
            TaxDetailsViewModel = new List<OrderTaxDetailsViewModel>();
            DropInformationDetails = new List<DropInformationViewModel>();
            IsOtherFuelTypeTaxesGiven = false;
            Country = new CountryViewModel();
            FuelDeliveryDetails.IsOrderView = true;
            UoM = UoM.Gallons;
            AllowPoEdit = true;
            FTLDetails = new FTLViewModel();
            PreferencesSettingId = 0;
            IsSupressOrderPricing = false;
            IsDriverProdutDisplayEnable = false;
            OrderBadgeDetails = new OrderBadgeViewModel();
            SourceRegion = new SourceRegionsViewModel();
            GeneralNotesHistory = new List<SourcingNotesViewModel>();
        }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        public int FuelRequestId { get; set; }

        public string PoNumber { get; set; }

        public string JobName { get; set; }

        public string DisplayJobID { get; set; }

        public int JobId { get; set; }

        public int JobStateId { get; set; }

        public bool IsJobResaleEnabled { get; set; }

        public int BuyerCompanyId { get; set; }

        public bool AllowPoEdit { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        [Remote("IsTPOCompanyExistInUpdateMode", "Validation", AreaReference.UseRoot, AdditionalFields = "BuyerCompanyId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string BuyerCompanyName { get; set; }

        //Added extra to validate while updating TPO
        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        [EmailAddressEx(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Remote("IsEmailExistTpoInUpdateMode", "Validation", AreaReference.UseRoot, AdditionalFields = "BuyerUserId", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string BuyerUserEmail { get; set; }

        public int BuyerUserId { get; set; }

        [Display(Name = nameof(Resource.lblLastName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string BuyerUserLastName { get; set; }

        [Display(Name = nameof(Resource.lblFirstName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string BuyerUserFirstName { get; set; }

        public string SupplierCompanyName { get; set; }

        public DateTimeOffset? JobEndDate { get; set; }

        public AddressViewModel JobLocation { get; set; }

        public List<ContactPersonViewModel> CustomerContacts { get; set; } = new List<ContactPersonViewModel>();
   
        public IList<ContactPersonViewModel> ContactPersons { get; set; }

        public ContactPersonViewModel Supplier { get; set; }

        public OrderVersionViewModel CurrentOrderVersion { get; set; }

        public OrderVersionViewModel PreviousOrderVersion { get; set; }

        public OrderVersionViewModel CurrentOrderVersionToEdit { get; set; }

        public int AssetsAssigned { get; set; }

        public bool IsAssetHistoryAvailable { get; set; }

        [Range(typeof(Decimal), ApplicationConstants.DecimalMinValue, ApplicationConstants.DecimalMaxValue, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public decimal GallonsOrdered { get; set; }

        public string DisplayGallonsOrdered { get; set; }

        public decimal GallonsDelivered { get; set; }

        public decimal InvoicedAmount { get; set; }

        public decimal DropTicketAmount { get; set; }

        public decimal BasicAmount { get; set; }

        public decimal AvgPricePerGallon { get; set; }

        public decimal AvgGallonsPercentagePerDelivery { get; set; }

        public decimal TotalGallonsDelivered { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string FuelDeliveredPercentage { get; set; }

        public string OrderTotalAmount { get; set; }

        public string PricePerGallon { get; set; }

        public int TerminalId { get; set; }
        public int CityGroupTerminalId { get; set; }
        
        public int? BulkPlantId { get; set; }
        public int? DriverId { get; set; }

        public string DriverName { get; set; }

        public bool IsTaxExempted { get; set; }

        public int PaymentTermId { get; set; }

        public string PaymentTermName { get; set; }

        public int NetDays { get; set; }

        public PaymentMethods PaymentMethod { get; set; }

        public int TypeOfFuel { get; set; }

        public int ProductTypeId { get; set; }

        public bool IsFuelSurchargeValid { get; set; }

        public bool IsDefaultInvoiceTypeManual { get; set; }
        
        public List<int> Qualifications { get; set; }

        public IList<DeliveryFeeByQuantityViewModel> DeliveryFeeByQuantity { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public IList<DropdownDisplayItem> Invoices { get; set; }

        public IList<DropdownDisplayItem> DropTickets { get; set; }

        public FuelRequestFeeViewModel FuelRequestFees { get; set; }

        public List<AdditionalFeeViewModel> AdditionalFees { get; set; }

        public List<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }

        public IList<PaymentDiscountViewModel> DiscountEarlyPayment { get; set; }

        public List<DeliveryScheduleViewModel> DeliverySchedules { get; set; }

        public bool IsBrokeredOrder { get; set; }

        public string TerminalName { get; set; }

        public double Distance { get; set; }

        public string FuelType { get; set; }

        public string ScheduleName { get; set; }

        public int ScheduleId { get; set; }

        public bool IsBrokerVisible { get; set; }

        public bool CanSupplierChangeTerminal { get; set; }

        public bool IsMultiOrder { get; set; }

        public bool IsOriginalOrder { get; set; }

        public OrderDetailsViewModel ParentOrderDetails { get; set; }

        public bool IsEndSupplier { get; set; }

        public List<int> OrderLicenses { get; set; }

        public bool IsHidePricingEnabled { get; set; }

        public FuelRequestResaleViewModel FuelRequestResale { get; set; }

        public List<FuelRequestResaleFeeViewModel> ResaleFee { get; set; }

        public int EstimatedGallonsPerDelivery { get; set; }

        public List<NextDeliveryScheduleViewModel> NextDeliverySchedule { get; set; }

        public int FuelRequestTypeId { get; set; }

        public string ProductDescription { get; set; }

        public string JobStartDate { get; set; }

        public bool IsInvitationEnabled { get; set; }

        public bool IsProFormaPo { get; set; }

        public bool IsRetailJob { get; set; }

        public bool CanCreateInvoice { get; set; }

        public int SuppplierCostTypeId { get; set; }

        public decimal GlobalFuelCost { get; set; }

        public decimal CurrentFuelCost { get; set; }
        public int FuelTypeId { get; set; }

        public int? TfxFuelTypeId { get; set; }

        public int PricingTypeId { get; set; }

        public decimal CalculatedPricePerGallon { get; set; }

        public FuelDetailsViewModel FuelDetails { get; set; }

        public int ExternalBrokerId { get; set; }

        public int? OrderClosingThreshold { get; set; }

        public TPOBrokeredOrderViewModel ExternalBrokeredOrder { get; set; }

        public bool IsBuyAndSellOrder { get; set; }

        public bool IsSendFileToBroker { get; set; }

        public string ExternalBrokerCompanyName { get; set; }

        public bool IsThirdPartyHardwareUsed { get; set; }

        public bool IsOtherFuelTypeTaxesGiven { get; set; }

        public List<OrderTaxDetailsViewModel> TaxDetailsViewModel { get; set; }

        public List<DropInformationViewModel> DropInformationDetails { get; set; }

        public CountryViewModel Country { get; set; }

        public UoM UoM { get; set; }

        public bool IsSingleDeliveryClosedOrderWithZeroPercent { get; set; }

        public bool IsFTLEnabled { get; set; }

        public int JobCompanyId { get; set; }

        public FTLViewModel FTLDetails { get; set; }

        public OrderAdditionalDetailsViewModel OrderAdditionalDetails { get; set; }

        public OrderBadgeViewModel OrderBadgeDetails { get; set; }

        public SourceRegionsViewModel SourceRegion { get; set; }

        public bool IsTankRentalEnabled { get; set; }

        public bool IsSignatureEnabled { get; set; }

        public int RequestPriceDetailId { get; set; }

        public int AcceptedCompanyId { get; set; }

        public int PricingCodeId { get; set; }
        public string PricingCode { get; set; }
        public string PricingCodeDescription { get; set; }

        public string GroupPoNumber { get; set; }
        public int? OrderGroupId { get; set; }

        public JobSpecificBillToViewModel BillToInfo { get; set; } = new JobSpecificBillToViewModel();

        public string SiteInstructions { get; set; }

        public string AccountingCompanyId { get; set; }
        public int? PreferencesSettingId { get; set; }
        public bool IsSupressOrderPricing { get; set; }
        public string CarrierCompanyName { get; set; }

        public bool IsDriverProdutDisplayEnable { get; set; }

        [Display(Name = nameof(Resource.lblOrderName), ResourceType = typeof(Resource))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        public string OrderName { get; set; }

        public int CompanyCountryId { get; set; }
        public string SourceRegionJsonParameters { get; set; }
        public int? LeadRequestId { get; set; }
        public List<SourcingNotesViewModel> GeneralNotesHistory { get; set; }

        public string Vessel { get; set; }

        public string IMONumber { get; set; }

        public int? CreditCheckTypeId { get; set; }
    }
}
