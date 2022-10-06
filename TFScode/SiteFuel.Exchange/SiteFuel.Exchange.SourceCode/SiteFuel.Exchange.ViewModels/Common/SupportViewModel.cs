using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupportViewModel : StatusViewModel
    {
        public SupportViewModel()
        {
        }

        public SupportViewModel(Status status)
            : base(status)
        {
        }

        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblSubject), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Subject { get; set; }

        [Display(Name = nameof(Resource.lblQuestion), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Question { get; set; }

        public bool IsContactByPhoneNumber { get; set; }

        [Display(Name = nameof(Resource.lblPhoneNumber), ResourceType = typeof(Resource))]
        public string PhoneNumber { get; set; }

        public bool IsContactByEmail { get; set; }

        [Display(Name = nameof(Resource.lblEmail), ResourceType = typeof(Resource))]
        public string Email { get; set; }
        public string SupportContactNumber { get; set; }
    }
}
