using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class StateViewModel : BaseViewModel
    {
        public StateViewModel()
        {
            Code = string.Empty;
            Name = string.Empty;
        }

        public StateViewModel(Status status) 
            : base(status)
        {
            Code = string.Empty;
            Name = string.Empty;
        }

        //[RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public int Id { get; set; }

        //[RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public string Code { get; set; }

        //[RequiredWithCustomLabel(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMinimumLength))]
        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public string Name { get; set; }

        public int QuantityIndicatorTypeId { get; set; }
    }
}
