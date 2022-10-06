using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class CountryViewModel : BaseViewModel
    {
        public CountryViewModel()
        {
        }

        public CountryViewModel(Status status)
            : base(status)
        {
            Code = string.Empty;
            Name = string.Empty;
        }

        //[RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public int Id { get; set; }

        //[RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public string Code { get; set; }

        //[RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength))]
        [Display(Name = nameof(Resource.lblCountry), ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Display(Name = nameof(Resource.lblCurrency), ResourceType = typeof(Resource))]
        public Currency Currency { get; set; }

        [Display(Name = nameof(Resource.lblUomShort), ResourceType = typeof(Resource))]
        public UoM UoM { get; set; }

        public bool IsCollapsed { get; set; }
    }
}
