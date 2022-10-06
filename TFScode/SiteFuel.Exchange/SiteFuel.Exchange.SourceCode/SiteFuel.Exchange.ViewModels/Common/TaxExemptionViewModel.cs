using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class TaxExemptionViewModel : StatusViewModel
    {
        public TaxExemptionViewModel()
        {
            CompanyAddress = new CompanyAddressViewModel();
            State = new StateViewModel();
            Country = new CountryViewModel();
            IsSameCompanyAddress = false;
            IsUpdateRequest = false;
        }

        public TaxExemptionViewModel(Status status)
            : base(status)
        {
            CompanyAddress = new CompanyAddressViewModel();
            State = new StateViewModel();
            Country = new CountryViewModel();
            IsSameCompanyAddress = false;
            IsUpdateRequest = false;
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblEffectiveDate), ResourceType = typeof(Resource))]
        public DateTimeOffset EffectiveDate { get; set; }

        public DateTimeOffset CompanyEffectiveDate { get; set; }

        public DateTimeOffset? ObsoleteDate { get; set; }

        public string EntityCustomId { get; set; }

        public string AccountCustomId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public CompanyAddressViewModel CompanyAddress { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        //[Remote("IsValidJobAddress", "Validation", AreaReference.UseRoot, AdditionalFields = "City,State.Id,Country.Id,ZipCode", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageInvalid))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public StateViewModel State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public CountryViewModel Country { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]        
        public string ZipCode { get; set; }

        public string Jurisdiction { get; set; }

        public string County { get; set; }

        [Display(Name = nameof(Resource.lblLegalName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string LegalName { get; set; }

        [Display(Name = nameof(Resource.lblIDType), ResourceType = typeof(Resource))]
        public string IDType { get; set; }

        [Display(Name = nameof(Resource.lblIDCode), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string IDCode { get; set; }

        [Display(Name = nameof(Resource.lblTradeName), ResourceType = typeof(Resource))]
        public string TradeName { get; set; }

        [Display(Name = nameof(Resource.lblBusinessSubType), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int BusinessSubType { get; set; }

        [Display(Name = nameof(Resource.lblLicenseNumber), ResourceType = typeof(Resource))]
        [Remote("IsLicenseNumberExist", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string LicenseNumber { get; set; }

        public decimal LicensePercentage { get; set; }

        public bool IsSameCompanyAddress { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int BusinessType { get; set; }

        public string BusinessSubTypeVal { get; set; }

        public bool IsDefault { get; set; }

        public bool RequiredTrue { get { return true; } }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("RequiredTrue", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAgreeTerms))]
        public bool IsAgreed { get; set; }

        public bool IsUpdateRequest { get; set; }
    }
}
