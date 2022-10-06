using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourcingDetailViewModel : BaseViewModel 
    {
        public SourcingDetailViewModel()
        {
            InstanceInitialize();
        }
        public SourcingDetailViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }
        private void InstanceInitialize()
        {
            CustomerDetails = new SourceCustomerDetailViewModel();
            AddressDetails = new SourcingAddressDetailViewModel();
            FuelDetails = new SourcingFuelViewModel();
            FuelDeliveryDetails = new SourceFuelDeliveryViewModel();
            FuelPricingDetails = new SourcingPricingViewModel();
            IsSupressOrderPricing = false;
            GeneralNotesHistory = new List<SourcingNotesViewModel>();
        }
        public int Id { get; set; }
        public string TruckLoadType { get; set; }
        public string FreightOnBoardType { get; set; }
        public string WBSNumber { get; set; }
        public string DisplayRequestId { get; set; }
        public string AccountingCompanyId { get; set; }
        public string RequestName { get; set; }
        public string RequestStatus { get; set; }
        public int SalesUserId { get; set; }
        public bool IsNotifyDeliveries { get; set; }
        public bool IsNotifySchedules { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public bool IsFTLEnabled { get; set; }
        public bool IsTaxExempted { get; set; }
        public bool IsSupressOrderPricing { get; set; } = false;
        public SourceCustomerDetailViewModel CustomerDetails { get; set; }
        public SourcingAddressDetailViewModel AddressDetails { get; set; }
        public SourcingFuelViewModel FuelDetails { get; set; }
        public SourceFuelDeliveryViewModel FuelDeliveryDetails { get; set; }
        public SourcingPricingViewModel FuelPricingDetails { get; set; }
        public string GeneralNote { get; set; }
        public List<SourcingNotesViewModel> GeneralNotesHistory { get; set; }

        public class SourceCustomerDetailViewModel : StatusViewModel
        {
            public SourceCustomerDetailViewModel()
            {
                IsNewCompany = true;
            }
            public SourceCustomerDetailViewModel(Status status) : base(status)
            {

            }
            public int Id { get; set; }
            public int? UserId { get; set; }
            public bool IsNewCompany { get; set; }
            public string CompanyName { get; set; }
            public string Name { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public bool IsInvitationEnabled { get; set; }
            public bool IsNotifyDeliveries { get; set; }
            public bool IsNotifySchedules { get; set; }
            public List<ContactPersonViewModel> ContactPersons { get; set; } = new List<ContactPersonViewModel>();
        }
        public class SourcingAddressDetailViewModel : BaseViewModel
        {
            public SourcingAddressDetailViewModel()
            {
                IsNewJob = true;
            }
            public SourcingAddressDetailViewModel(Status status) : base(status)
            {
            }
            public int Id { get; set; }
            public string JobName { get; set; }
            public string DisplayJobID { get; set; }
            public int? JobId { get; set; }
            public bool IsNewJob { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string CountryName { get; set; }
            public string Currency { get; set; }
            public string ZipCode { get; set; }
            public string JobLocationType { get; set; }
            public bool IsProFormaPoEnabled { get; set; }
            public bool IsRetailJob { get; set; }
            public bool SignatureEnabled { get; set; }
            public string CountyName { get; set; }
            public string Latitude { get; set; }
            public bool IsGeocodeUsed { get; set; }
            public string Longitude { get; set; }
            public string TimeZoneName { get; set; }
            public string LocationManagedType { get; set; } = "Not Specified";
            public bool IsCompanyOwned { get; set; }
            public string MarineUoM { get; set; }
            public bool IsMarineLocation { get; set; }
            public string InventoryDataCaptureType { get; set; }
            public string UOM { get; set; }
            public string DispatchRegion { get; set; }
        }
        public class SourcingFuelViewModel : BaseViewModel
        {
            public SourcingFuelViewModel()
            {
                InstanceInitialize();
                Fees = new List<FeesViewModel>();
            }

            public SourcingFuelViewModel(Status status) : base(status)
            {
                InstanceInitialize();
                Fees = new List<FeesViewModel>();
            }
            private void InstanceInitialize()
            {
                FuelQuantity = new FuelQuantityViewModel();
                FuelPricing = new FuelPricingViewModel();
                StatusId = (int)FuelRequestStatus.Draft;
                CreatedDate = DateTimeOffset.Now;
                OrderTypeId = (int)OrderType.Spot;
                FuelDisplayGroupId = (int)ProductDisplayGroups.FavoriteFuelType;
                IsOtherFuelTypeInFavorite = false;
            }
            public int Id { get; set; }
            public int FuelDisplayGroupId { get; set; }
            public string FuelType { get; set; }
            public int OrderTypeId { get; set; }
            public int QuantityTypeId { get; set; }
            public decimal Quantity { get; set; }
            public decimal MinimumQuantity { get; set; }
            public decimal MaximumQuantity { get; set; }
            public string QuantityIndicatorTypes { get; set; }
            public FuelQuantityViewModel FuelQuantity { get; set; }
            public FuelPricingViewModel FuelPricing { get; set; }
            public string NonStandardFuelName { get; set; }
            public string NonStandardFuelDescription { get; set; }
            public bool IsTierPricing { get; set; } = false;
            public int PricingTypeId { get; set; }
            public decimal PricePerGallon { get; set; } = 0;
            public Nullable<int> RackAvgTypeId { get; set; }
            public decimal RackPrice { get; set; } = 0;
            public bool EnableCityRack { get; set; }
            public string TerminalName { get; set; }
            public Nullable<int> TerminalId { get; set; }
            public Nullable<int> SupplierCostMarkupTypeId { get; set; }
            public decimal SupplierCostMarkupValue { get; set; } = 0;
            public List<FeesViewModel> Fees { get; set; }
            public int StatusId { get; set; }
            public string StatusName { get; set; }
            public int CreatedBy { get; set; }
            public DateTimeOffset CreatedDate { get; set; }
            public bool AddToFavorite { get; set; }
            public bool IsOtherFuelTypeInFavorite { get; set; }
            public int FuelDisplayJobId { get; set; }
            public int ProductTypeId { get; set; }
            public int? FreightOnBoard { get; set; }
            public bool IsFTLEnabled { get; set; }
            public bool IsMarineLocation { get; set; }
            public PickUpAddressViewModel PickUpLocation { get; set; }
            public TierPricingViewModel TierPricing { get; set; } = new TierPricingViewModel();
            public List<SourceFeesViewModel> sourceFeesViewModel { get; set; } = new List<SourceFeesViewModel>();

        }
        public class SourceFuelDeliveryViewModel
        {
            public SourceFuelDeliveryViewModel()
            {
                DeliveryTypes = DeliveryType.OneTimeDelivery.ToString();
                StartDate = Convert.ToDateTime(DateTimeOffset.Now.ToString()).ToShortDateString();
                StartTime = Convert.ToDateTime("08:00").ToShortTimeString();
                EndTime = Convert.ToDateTime("17:00").ToShortTimeString();
                PaymentTermId = (int)PaymentTerms.NetDays;
                IsPrePostDipRequired = false;
                OrderEnforcementId = OrderEnforcement.EnforceOrderLevelValues.ToString();
            }
            public int Id { get; set; }
            public string DeliveryTypes { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string SingleDeliverySubTypes { get; set; }
            public string PaymentMethods { get; set; }
            public int PaymentTermId { get; set; }
            public int NetDays { get; set; }
            public bool IsPrePostDipRequired { get; set; }
            public string OrderEnforcementId { get; set; }
            public SourcePaymentViewModel SourcePaymentViewModel { get; set; } = new SourcePaymentViewModel();

        }
        public class SourcingPricingViewModel : BaseViewModel
        {
            public SourcingPricingViewModel()
            {
                ExchangeRate = 1;
                IsTierPricingRequired = false;
                TierPricing = new SourcingTierPricingViewModel();
            }

            public SourcingPricingViewModel(Status status) : base(status)
            {
                ExchangeRate = 1;
                IsTierPricingRequired = false;
                TierPricing = new SourcingTierPricingViewModel();
            }
            public int Id { get; set; }
            public int LeadRequestId { get; set; }
            public int PricingTypeId { get; set; }
            public decimal PricePerGallon { get; set; } = 0;
            public string Code { get; set; }
            public int CodeId { get; set; }
            public string CodeDescription { get; set; }
            public Nullable<int> RackAvgTypeId { get; set; }
            public decimal RackPrice { get; set; } = 0;
            public bool EnableCityRack { get; set; }
            public string TerminalName { get; set; }
            public Nullable<int> TerminalId { get; set; }
            public Nullable<int> SupplierCostMarkupTypeId { get; set; }
            public decimal SupplierCostMarkupValue { get; set; } = 0;
            public decimal? SupplierCost { get; set; }
            public int? SupplierCostTypeId { get; set; }
            public Nullable<int> MarkertBasedPricingTypeId { get; set; }
            public int? CityGroupTerminalId { get; set; }
            public string CityGroupTerminalName { get; set; }
            public int? CityGroupTerminalStateId { get; set; }
            public decimal ExchangeRate { get; set; }
            public int? FuelTypeId { get; set; }
            public string PricingNote { get; set; }
            public string Currency { get; set; }
            public string ParameterJSON { get; set; }
            public bool IsTierPricingRequired { get; set; }
            public string DisplayPrice { get; set; }
            public SourcingTierPricingViewModel TierPricing { get; set; }
            public FuelRequestPricingViewModel FuelPricingDetails { get; set; } = new FuelRequestPricingViewModel();
        }
        public class FuelRequestPricingViewModel
        {
            public int FuelRequestId { get; set; }
            public int? FeedTypeId { get; set; }
            public int? FuelClassTypeId { get; set; }
            public int? PricingQuantityIndicatorTypeId { get; set; }
            public string PricingSourceId { get; set; } = PricingSource.Axxis.ToString();
            public int? TruckLoadTypeId { get; set; }
            public int? PricingSourceQuantityIndicatorTypeId { get; set; }
            public string TruckLoadTypes { get; set; }
            public string FreightOnBoardTypes { get; set; } = "Terminal";
            public string PricingSourceQuantityIndicatorTypes { get; set; }
            public string FuelClassTypes { get; set; }
            public int StateDefaultQuantityIndicatorId { get; set; } // required to get state's default quantity indicator
            public string WeekendDropPricingDay { get; set; } = "Saturday";
            public int RequestPriceDetailId { get; set; }
            public PricingCodeDetailViewModel PricingCode { get; set; } = new PricingCodeDetailViewModel();
        }
        public class SourceFeesViewModel
        {
            public string FeeTypeName { get; set; }
            public string FeeSubTypeName { get; set; }
            public string Fee { get; set; }
            public string FeeDetails { get; set; }
        }
        public class SourcePaymentViewModel
        {
            public string PaymentMethods { get; set; }
            public string PaymentTerms { get; set; }
            public string NetDays { get; set; }
        }
    }
}
