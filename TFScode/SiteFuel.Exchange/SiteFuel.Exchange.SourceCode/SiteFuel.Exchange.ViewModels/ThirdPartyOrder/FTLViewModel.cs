using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class FTLViewModel
    {
        [Display(Name = nameof(Resource.lblBOL), ResourceType = typeof(Resource))]
        [StringLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageMaximumLength), MinimumLength = 0)]
        public string BOL { get; set; }

        [Display(Name = nameof(Resource.lblGrossGallons), ResourceType = typeof(Resource))]
        public decimal? GrossGallons { get; set; }

        [Display(Name = nameof(Resource.lblNetGallons), ResourceType = typeof(Resource))]
        public decimal? NetGallons { get; set; }
    }
}
