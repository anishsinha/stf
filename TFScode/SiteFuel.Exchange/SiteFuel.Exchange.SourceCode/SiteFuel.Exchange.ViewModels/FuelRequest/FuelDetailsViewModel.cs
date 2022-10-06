using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using SiteFuel.Exchange.ViewModels.CustomAttributes;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelDetailsViewModel : BaseViewModel
    {
        public FuelDetailsViewModel()
        {
            InstanceInitialize();
        }

        public FuelDetailsViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            //DON NOT USE THIS DifferentFuelPrices PROPERTY FROM THIS VIEWMODEL, USE IF FROM FuelPricing
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
            FuelQuantity = new FuelQuantityViewModel();
            FuelPricing = new FuelPricingViewModel();
            StatusId = (int)FuelRequestStatus.Draft;
            CreatedDate = DateTimeOffset.Now;
            IsOverageAllowed = false;
            OrderTypeId = (int)OrderType.Spot;
            FuelDisplayGroupId = (int)ProductDisplayGroups.FavoriteFuelType;
            IsOtherFuelTypeInFavorite = false;
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int FuelDisplayGroupId { get; set; }

        [Display(Name = nameof(Resource.lblFuel), ResourceType = typeof(Resource))]
        [RequiredIfNot("FuelDisplayGroupId", (int)ProductDisplayGroups.OtherFuelType, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? FuelTypeId { get; set; }

        public string FuelType { get; set; }

        public FuelQuantityViewModel FuelQuantity { get; set; }

        public FuelPricingViewModel FuelPricing { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblOverageAllowed), ResourceType = typeof(Resource))]
        public bool IsOverageAllowed { get; set; }

        [RequiredIfTrue("IsOverageAllowed", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public decimal OverageAllowedPercent { get; set; }

        [Display(Name = nameof(Resource.lblOrderType), ResourceType = typeof(Resource))]
        public int OrderTypeId { get; set; }

        [Display(Name = nameof(Resource.lblFuelPriceByQuantity), ResourceType = typeof(Resource))]
        public bool IsDifferentFuelPriceEntered { get; set; }

        //DON NOT USE THIS DifferentFuelPrices PROPERTY FROM THIS VIEWMODEL, USE IF FROM FuelPricing
        [RequiredIfTrue("IsDifferentFuelPriceEntered", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public List<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }

        [Display(Name = nameof(Resource.lblStatus), ResourceType = typeof(Resource))]
        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public Nullable<int> TerminalId { get; set; }

        public string Comment { get; set; }

        [RequiredIf("FuelDisplayGroupId", (int)ProductDisplayGroups.OtherFuelType, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblProductName), ResourceType = typeof(Resource))]
        public string NonStandardFuelName { get; set; }

        [Display(Name = nameof(Resource.lblProductDescription), ResourceType = typeof(Resource))]
        public string NonStandardFuelDescription { get; set; }

        public bool AddToFavorite { get; set; }

        public bool IsOtherFuelTypeInFavorite { get; set; }

        public int FuelDisplayJobId { get; set; }

        public int ProductTypeId { get; set; }

        public int? FreightOnBoard { get; set; }

        public bool IsFTLEnabled { get; set; }

        public bool IsMarineLocation { get; set; }

        public PickUpAddressViewModel PickUpLocation { get; set; }

        public List<TankRentalFrequencyViewModel> TankFrequencies { get; set; } = new List<TankRentalFrequencyViewModel>();

        public bool IsTierPricing { get; set; } = false;

        public TierPricingViewModel TierPricing { get; set; } = new TierPricingViewModel();

        public string Berth { get; set; }

        [Display(Name = nameof(Resource.lblVessle), ResourceType = typeof(Resource))]
        public int? VessleId { get; set; }

        [Display(Name = nameof(Resource.lblIMONumber), ResourceType = typeof(Resource))]
        public string IMONumber { get; set; }

        [Display(Name = nameof(Resource.lblFlag), ResourceType = typeof(Resource))]
        public string Flag { get; set; }

        public int? JobXAssetId { get; set; }
    }
}
