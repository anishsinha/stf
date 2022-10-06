using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Forcasting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class OnboardingViewModel : StatusViewModel
    {
        public OnboardingViewModel()
        {
            InstanceInitialize();
        }

        public OnboardingViewModel(Status status) : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            User = new RegisterViewModel();
            CompanyAddress = new CompanyAddressViewModel();
            SupplierProfile = new SupplierProfileViewModel();
            Card = new PaymentCardViewModel();
            State = new StateViewModel();
            Country = new CountryViewModel();
            BillingAddress = new BillingAddressViewModel();
            HaulerPricingMatrices = new List<HaulerPricingMatrixViewModel>();
        }

        public RegisterViewModel User { get; set; }

        public CompanyAddressViewModel CompanyAddress { get; set; }

        public SupplierProfileViewModel SupplierProfile { get; set; }

        public int PaymentCardId { get; set; }

        public int CompanyId { get; set; }

        public string CardToken { get; set; }

        public string AddedBy { get; set; }

        public PaymentCardViewModel Card { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress), ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress2), ResourceType = typeof(Resource))]
        public string AddressLine2 { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblAddress3), ResourceType = typeof(Resource))]
        public string AddressLine3 { get; set; }

        [StringLength(128, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength), MinimumLength = 1)]
        [Display(Name = nameof(Resource.lblCity), ResourceType = typeof(Resource))]
        public string City { get; set; }

        public StateViewModel State { get; set; }

        public CountryViewModel Country { get; set; }

        [Display(Name = nameof(Resource.lblZipcode), ResourceType = typeof(Resource))]
        public string ZipCode { get; set; }

        public TermsViewModel Terms { get; set; }

        public BillingAddressViewModel BillingAddress { get; set; }

        public List<HaulerPricingMatrixViewModel> HaulerPricingMatrices { get; set; }

        public bool IsExternalSupplier { get; set; }

        public int ExternalSupplierId { get; set; }

        public OnboardingPreferenceViewModel PreferencesSetting { get; set; } = new OnboardingPreferenceViewModel();
    }
}
