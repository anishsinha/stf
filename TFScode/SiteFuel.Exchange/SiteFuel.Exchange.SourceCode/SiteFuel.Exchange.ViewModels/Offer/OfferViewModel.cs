using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SiteFuel.Exchange.ViewModels.Offer
{
    public class OfferViewModel : BaseInputViewModel
    {
        public OfferViewModel()
        {
            Tiers = new List<int>();
            Customers = new List<int>();
            FuelTypes = new List<int>();
            States = new List<int>();
            //Country = new List<int>();
            LocationViewModel = new List<OfferLocationViewModel>();
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel();
            FuelPricing = new FuelPricingViewModel();
            Guid = Guid.NewGuid();
            OfferStats = new List<OfferStatsViewModel>();
            Quantity = 1;
            CreatedDate = DateTimeOffset.Now;
            UpdatedDate = DateTimeOffset.Now;
        }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int Id { get; set; }

        public int OfferTypeId { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblOfferName), ResourceType = typeof(Resource))]
        public string Name { get; set; }

        public Guid Guid { get; set; }

        //[RequiredIfEmpty("Customers", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblTier), ResourceType = typeof(Resource))]
        public List<int> Tiers { get; set; }

        //[RequiredIfEmpty("Tiers", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCustomer), ResourceType = typeof(Resource))]
        public List<int> Customers { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        public List<int> FuelTypes { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblQuantity), ResourceType = typeof(Resource))]
        public int Quantity { get; set; }

        //[Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        //public List<int> Country { get; set; }

        public int CountryId { get; set; }

        public int CurrencyId { get; set; }

        [RequiredIf("OfferLocationTypeId", (int)OfferLocationType.State, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public List<int> States { get; set; }

        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string Cities { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public string ZipCodes { get; set; }

        public int OfferLocationTypeId { get; set; }

        public List<OfferLocationViewModel> LocationViewModel { get; set; }

        public FuelPricingViewModel FuelPricing { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public int StatusId { get; set; }

        public List<OfferStatsViewModel> OfferStats { get; set; }

        public List<string> TierNames { get; set; } = new List<string>();

        public List<string> CustomerNames { get; set; } = new List<string>();

        public string FuelTypeName { get; set; }

        public bool IsApplicableToLaunch { get; set; }

        public bool IsQuickUpdated { get; set; }

        public int UpdatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public OfferViewModel DoNullCleaning()
        {
            // Keeping at least one location view model to show all states at least
            var firstAllstates = LocationViewModel.FirstOrDefault(x => x.State == null);
            LocationViewModel.RemoveAll(x => x.State == null);
            if (firstAllstates != null)
            {
                LocationViewModel.Add(firstAllstates);
            }
            LocationViewModel.ForEach(x => x.ZipStringList.RemoveAll(y => y == null || y == ""));
            TierNames.RemoveAll(x => x == null || x == "");
            CustomerNames.RemoveAll(x => x == null || x == "");
            return this;
        }
    }

    public class BuyerOfferViewodel : OfferViewModel
    {
        public BuyerOfferViewodel()
        {
            OrderViewModel = new OfferOrderViewModel();
            SearchLocationViewModel = new OfferLocationViewModel();
        }
        public OfferOrderViewModel OrderViewModel { get; set; }

        [Display(Name = nameof(Resource.lblSupplier), ResourceType = typeof(Resource))]
        public List<DropdownDisplayItem> FilterFuelTypes { get; set; }

        [Display(Name = nameof(Resource.lblSupplier), ResourceType = typeof(Resource))]
        public List<DropdownDisplayItem> FilterPricingTypes { get; set; }

        [Display(Name = nameof(Resource.lblSupplier), ResourceType = typeof(Resource))]
        public List<DropdownDisplayItem> FilterStates { get; set; }

        [Display(Name = nameof(Resource.lblSupplier), ResourceType = typeof(Resource))]
        public List<DropdownDisplayItem> FilterSuppliers { get; set; }

        [Display(Name = nameof(Resource.lblSupplier), ResourceType = typeof(Resource))]
        public List<int> Suppliers { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblJob), ResourceType = typeof(Resource))]
        public int JobId { get; set; }

        [Display(Name = nameof(Resource.lblPricing), ResourceType = typeof(Resource))]
        public List<int> PricingTypes { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public string SearchZip { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string SearchCity { get; set; }

        public OfferLocationViewModel SearchLocationViewModel { get; set; }
    }

    public class OfferPricingDetailsViewModel : StatusViewModel
    {
        
        public OfferOrderViewModel OfferOrderViewModel { get; set; }

        public OfferViewModel OfferViewModel { get; set; }

        [Display(Name = nameof(Resource.lblNumber), ResourceType = typeof(Resource))]
        public string OfferNumber { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public string OfferType { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        public List<string> FuelType { get; set; }

        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public string DisplayCountry { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public List<string> DisplayStates { get; set; }

        public int OfferPricingId { get; set; }

        public int SupplierCompanyId { get; set; }

        public int JobId { get; set; }

        public int Quantity { get; set; }

        public int StateId { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public int PricingCodeId { get; set; }

        public string PricingCode { get; set; }

        public string PricingCodeDesc { get; set; }

        public bool IsActive { get; set; }
    }

    public class OfferLocationViewModel
    {
        public OfferLocationViewModel()
        {
            ZipList = new List<DropdownDisplayExtended>();
        }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public int? StateId { get; set; }

        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public int? CityId { get; set; }

        public List<DropdownDisplayExtended> ZipList { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public List<string> ZipStringList { get; set; } = new List<string>();

        public int CountryId { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(State == null ? "All States" : State);
            str.Append(City == null ? "" : $" - {City}");

            if (!ZipStringList.Contains("0") && ZipStringList.Count > 0)
            {
                str.Append(" - ");
                str.Append(string.Join(", ", ZipStringList));
            }
            return str.ToString();
        }
    }

    public class OfferQuickUpdateViewModel : StatusViewModel
    {
        public OfferQuickUpdateViewModel()
        {
            Tiers = new List<int>();
            Customers = new List<int>();
            States = new List<int>();
            Cities = new List<int>();
            ZipList = new List<DropdownDisplayExtended>();
            ZipStringList = new List<string>();
            QuickUpdatePreferenceSetting = new QuickUpdatePreferenceSetting();
        }
        public int UserId { get; set; }
        public int CompanyId { get; set; }

        public int OfferTypeId { get; set; }

        [Display(Name = nameof(Resource.lblTier), ResourceType = typeof(Resource))]
        public List<int> Tiers { get; set; }

        //[RequiredIfEmpty("Tiers", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCustomer), ResourceType = typeof(Resource))]
        public List<int> Customers { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        public int FuelTypeId { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public List<int> States { get; set; }

        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public List<int> Cities { get; set; }

        public List<DropdownDisplayExtended> ZipList { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public List<string> ZipStringList { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFeeType), ResourceType = typeof(Resource))]
        public string FeeTypeId { get; set; }

        public int? FeeSubTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblPriceType), ResourceType = typeof(Resource))]
        public int PricingTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblSelectDollarOrPercent), ResourceType = typeof(Resource))]
        public int MathOptId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblAmount), ResourceType = typeof(Resource))]
        public decimal? UpdateAmountBy { get; set; }

        public QuickUpdatePreferenceSetting QuickUpdatePreferenceSetting { get; set; }

        // Quick update history items
        public string TierNames { get; set; }
        public string CustomerNames { get; set; }
        public string StateNames { get; set; }
        public string CityNames { get; set; }
        public string FeeTypeName { get; set; }

        public int CountryId { get; set; }
        public int CurrencyId { get; set; }

        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public TruckLoadTypes TruckLoadType { get; set; } = TruckLoadTypes.LessTruckLoad;

        [Display(Name = nameof(Resource.lblIndices), ResourceType = typeof(Resource))]
        public PricingSource PricingSource { get; set; } = PricingSource.Axxis;
    }

    public class OfferPricingUpdateViewModel
    {
        public int UpdateTypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblSelectDollarOrPercent), ResourceType = typeof(Resource))]
        public int MathOptId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblAmount), ResourceType = typeof(Resource))]
        public decimal UpdateAmount { get; set; }
    }

    public class QuickUpdatePreferenceSetting
    {
        public int Id { get; set; }

        public bool IsCustomerTier { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsMarketOffer { get; set; }

        public bool IsState { get; set; }

        public bool IsCity { get; set; }

        public bool IsPricingType { get; set; }

        public bool IsFeeType { get; set; }

        public int QuickUpdateType { get; set; }
    }
}
