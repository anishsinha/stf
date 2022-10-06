using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyViewModel : BaseViewModel
    {
        public CompanyViewModel()
        {
            InstanceInitialize();
        }

        public CompanyViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            CompanyLogo = new ImageViewModel(status);
            BudgetAlertPercentage = 90;
            FleetInfo = new FleetInfo();
            ServiceOffering = new List<ServiceOffering>();
            FuelAssets = new FleetTrailers();
            DefAssets = new FleetTrailers();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256)]
        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Remote("IsCompanyExist", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public int CompanyTypeId { get; set; }

        public bool IsTPOCompany { get; set; }

        public ImageViewModel CompanyLogo { get; set; }

        public bool IsOnboarding { get; set; }

        [Display(Name = nameof(Resource.lblBudgetAlertPercentage), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Range(1, 100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRange))]
        public int BudgetAlertPercentage { get; set; }

        [Display(Name = nameof(Resource.lblEnableAssetTrackingForJob), ResourceType = typeof(Resource))]
        public bool IsAssetTrackingEnabled { get; set; }

        [Display(Name = nameof(Resource.lblResaleForJob), ResourceType = typeof(Resource))]
        public bool IsResaleEnabled { get; set; }

        [Display(Name = nameof(Resource.lblSupplierCode), ResourceType = typeof(Resource))]
        public string SupplierCode { get; set; }

        public int? AccountOwnerId { get; set; }

        public int ThemeId { get; set; }

        //public List<OnboardingQuestionViewModel> Questions { get; set; }

        [Display(Name = nameof(Resource.lblWorkPreference), ResourceType = typeof(Resource))]
        public WorkPreference WorkPreference { get; set; }
        public FleetInfo FleetInfo { get; set; }
        public List<ServiceOffering> ServiceOffering { get; set; }
        public FleetTrailers FuelAssets { get; set; }
        public FleetTrailers DefAssets { get; set; }
    }
}
