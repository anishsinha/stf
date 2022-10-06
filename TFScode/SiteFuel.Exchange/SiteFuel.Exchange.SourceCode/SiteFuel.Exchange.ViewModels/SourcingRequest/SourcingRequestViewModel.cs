using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class SourcingRequestViewModel : BaseViewModel
    {
        public SourcingRequestViewModel()
        {
            InstanceInitialize();
        }

        public SourcingRequestViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            CustomerDetails = new SourceCustomerViewModel();
            AddressDetails = new SourcingAddressViewModel();
            FuelDetails = new SourcingFuelDetailsViewModel();
            FuelDeliveryDetails = new SourcingFuelDeliveryViewModel();
            AdditionalDetailsViewModel = new SourcingAdditionalDetailsModel();
            FuelPricingDetails = new SourcingPricingDetailsViewModel();
            FuelOfferDetails = new FuelOfferDetailsViewModel();
            IsSupressOrderPricing = false;
            OrderAdditionalDetailsViewModel = new OrderAdditionalDetailsViewModel();
            GeneralNotesHistory = new List<SourcingNotesViewModel>();
        }
        public int Id { get; set; }
        public TruckLoadTypes TruckLoadType { get; set; }
        public FreightOnBoardTypes FreightOnBoardType { get; set; }
        public string WBSNumber { get; set; }
        public string DisplayRequestId { get; set; }
        public string AccountingCompanyId { get; set; }
        public string RequestName { get; set; }
        public SourcingRequestStatus RequestStatus { get; set; }
        public int SalesUserId { get; set; }
        public bool IsNotifyDeliveries { get; set; }
        public bool IsNotifySchedules { get; set; }
        public int? CityGroupTerminalId { get; set; }
        public bool IsFTLEnabled { get; set; }
        public FuelOfferDetailsViewModel FuelOfferDetails { get; set; }
        public bool IsTaxExempted { get; set; }
        public bool IsSupressOrderPricing { get; set; } = false;
       
        public SourceCustomerViewModel CustomerDetails { get; set; }
        public SourcingAddressViewModel AddressDetails { get; set; }
        public SourcingFuelDetailsViewModel FuelDetails { get; set; }
        public SourcingFuelDeliveryViewModel FuelDeliveryDetails { get; set; }
        public SourcingAdditionalDetailsModel AdditionalDetailsViewModel { get; set; }
        public SourcingPricingDetailsViewModel FuelPricingDetails { get; set; }
        public OrderAdditionalDetailsViewModel OrderAdditionalDetailsViewModel { get; set; }
        public string GeneralNote { get; set; }
        public List<SourcingNotesViewModel> GeneralNotesHistory { get; set; }
    }
    public class SourceCustomerViewModel : StatusViewModel
    {
        public SourceCustomerViewModel()
        {
            IsNewCompany = true;
        }
        public SourceCustomerViewModel(Status status):base(status)
        {

        }
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CompanyId { get; set; }
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

    public class SourcingAddressViewModel: BaseViewModel
    {
        public SourcingAddressViewModel()
        {
            IsNewJob = true;
            LocationManagedType = LocationManagedType.NotSpecified;
        }
        public SourcingAddressViewModel(Status status):base(status)
        {
        }
        public int Id { get; set; }
        public string JobName { get; set; }
        public string DisplayJobID { get; set; }
        public int? JobId { get; set; }
        public bool IsNewJob { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public Currency Currency { get; set; }
        public string ZipCode { get; set; }
        public JobLocationTypes JobLocationType { get; set; }
        public bool IsProFormaPoEnabled { get; set; }
        public bool IsRetailJob { get; set; }
        public bool SignatureEnabled { get; set; }
        public string CountyName { get; set; }
        public string Latitude { get; set; }
        public bool IsGeocodeUsed { get; set; }
        public string Longitude { get; set; }
        public string TimeZoneName { get; set; }
        public LocationManagedType LocationManagedType { get; set; }
        public bool IsCompanyOwned { get; set; }
        public UoM MarineUoM { get; set; }
        public bool IsMarineLocation { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public UoM UOM { get; set; }
        public string DispatchRegionId { get; set; }
    }

    public class SourcingFuelDeliveryViewModel
    {
        public SourcingFuelDeliveryViewModel()
        {
            DeliveryTypeId = (int)DeliveryType.OneTimeDelivery;
            StartDate = Convert.ToDateTime(DateTimeOffset.Now.ToString()).ToShortDateString();
            StartTime = Convert.ToDateTime("08:00").ToShortTimeString();
            EndTime = Convert.ToDateTime("17:00").ToShortTimeString();
            PaymentTermId = (int)PaymentTerms.NetDays;
            IsPrePostDipRequired = false;
            OrderEnforcementId = (OrderEnforcement)OrderEnforcement.EnforceOrderLevelValues;
        }
        public int Id { get; set; }
        public int DeliveryTypeId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public SingleDeliverySubTypes SingleDeliverySubTypes { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public int PaymentTermId { get; set; }
        public int NetDays { get; set; }
        public bool IsPrePostDipRequired { get; set; }
        public OrderEnforcement OrderEnforcementId { get; set; }

    }
    public class SourcingFuelDetailsViewModel : BaseViewModel
    {
        public SourcingFuelDetailsViewModel()
        {
            InstanceInitialize();
            Fees = new List<FeesViewModel>();
        }

        public SourcingFuelDetailsViewModel(Status status) : base(status)
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
        public int? FuelTypeId { get; set; }
        public int OrderTypeId { get; set; }
        public int QuantityTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal MinimumQuantity { get; set; }
        public decimal MaximumQuantity { get; set; }
        public QuantityIndicatorTypes QuantityIndicatorTypes { get; set; }
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

    }
    public class SourcingAdditionalDetailsModel : StatusViewModel
    {
        public int Id { get; set; }
        public bool IsAssetTracked { get; set; }
        public bool IsAssetDropStatusEnabled { get; set; }
    }

    public class SourcingPricingDetailsViewModel : BaseViewModel
    {
        public SourcingPricingDetailsViewModel()
        {
            ExchangeRate = 1;
            IsTierPricingRequired = false;
            TierPricing = new SourcingTierPricingViewModel();
        }

        public SourcingPricingDetailsViewModel(Status status) : base(status)
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
        public Currency Currency { get; set; }
        public string ParameterJSON { get; set; }
        public bool IsTierPricingRequired { get; set; }
        public SourcingTierPricingViewModel TierPricing { get; set; }
        public FuelRequestPricingDetailsViewModel FuelPricingDetails { get; set; } = new FuelRequestPricingDetailsViewModel();
    }
    public class SourcingNotesViewModel
    {
        public int Id { get; set; }
        public int LeadRequestId { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedDate { get; set; }
    }
}
