using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class TermsViewModel : StatusViewModel
    {
        public TermsViewModel()
        {
        }

        public TermsViewModel(Status status) 
            : base(status)
        {
        }

        [Display(Name = nameof(Resource.lblTerms), ResourceType = typeof(Resource))]
        public string TermsAndConditions { get; set; }

        [Display(Name = nameof(Resource.lblAcceptTerms), ResourceType = typeof(Resource))]
        public bool IsAgreed { get; set; }
    }
}
