using Foolproof;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.FuelPricingDatail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class SalesCalculatorViewModel : DataTableAjaxPostModel
    {
        public SalesCalculatorViewModel()
        {
            IsZipCode = true;
            PriceDate = DateTime.Now;
            CityTerminalIds = new List<int>();
            StateTerminalIds = new List<int>();
            SelectedFuelType = (int)ProductDisplayGroups.CommonFuelType;
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblDate), ResourceType = typeof(Resource))]
        public DateTime PriceDate { get; set; }

        public bool IsZipCode { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        //[RequiredIfTrue("IsZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]              
        public string ZipCode { get; set; }
      
       // [RequiredIfFalse("IsZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidCompanyAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        //[RequiredIfFalse("IsZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string City { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? StateId { get; set; }

        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? CountryId { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        //[RequiredIfFalse("IsZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Zip { get; set; }

        public int SelectedFuelType { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        [RequiredIf("SelectedFuelType", ProductDisplayGroups.FuelTypesInYourArea, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? FuelTypeInYourAreaId { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        [RequiredIf("SelectedFuelType", ProductDisplayGroups.CommonFuelType, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? CommonFuelTypeId { get; set; }

        [Display(Name = nameof(Resource.lblFuelType), ResourceType = typeof(Resource))]
        [RequiredIf("SelectedFuelType", ProductDisplayGroups.LessCommonFuelType, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int? LessCommonFuelTypeId { get; set; }

        [Display(Name = nameof(Resource.lblCustomPricing), ResourceType = typeof(Resource))]
        public bool IsCustomPricing { get; set; }

        [Display(Name = nameof(Resource.lblRackPrice), ResourceType = typeof(Resource))]
        public int CustomPricing { get; set; }

        [Display(Name = nameof(Resource.gridColumnAmount), ResourceType = typeof(Resource))]
        public decimal Amount { get; set; }

        public bool IncludeTaxes { get; set; }

        [Display(Name = nameof(Resource.lblCityGroupTerminal), ResourceType = typeof(Resource))]
        public bool IsCityRackTerminal { get; set; }
        
        public int CityTerminalPricingType { get; set; }
        
        [Display(Name = nameof(Resource.lblShowPricingForCityRackTerminal), ResourceType = typeof(Resource))]
        public List<int> CityTerminalIds { get; set; }

        [Display(Name = nameof(Resource.lblShowCityRackInStateOf), ResourceType = typeof(Resource))]
        public List<int> StateTerminalIds { get; set; }

        public string StateName { get; set; }

        public string CountryCode { get; set; }

        public int? PricingCodeId { get; set; }

        public string PricingCode { get; set; }

        public FuelRequestPricingDetailsViewModel FuelPricingDetails { get; set; } = new FuelRequestPricingDetailsViewModel();

        public bool IsMissingAddress()
        {
            return string.IsNullOrWhiteSpace(ZipCode) || string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(Address);
        }
    }
}
