using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.CustomAttributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class CreditAppViewModel : BaseViewModel
    {
        public CreditAppViewModel()
        {
            InstanceInitialize();
        }

        public CreditAppViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Documents = new List<DocumentViewModel>();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblEnable), ResourceType = typeof(Resource))]
        public bool IsEnabled { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public int FromEmail { get; set; }

        public List<DropdownDisplayItem> Users { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Body { get; set; }       

        public int CompanyId { get; set; }

        public List<DocumentViewModel> Documents { get; set; }
    }
}
