using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalCompanyViewModel : BaseViewModel
    {
        public ExternalCompanyViewModel()
        {
            
        }

        public ExternalCompanyViewModel(Status status) : base(status)
        {
           
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(256)]
        [Display(Name = nameof(Resource.lblName), ResourceType = typeof(Resource))]
        [Remote("IsExternalCompanyExist", "Validation", AreaReference.UseRoot, AdditionalFields = "Id", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblType), ResourceType = typeof(Resource))]
        public int CompanyTypeId { get; set; }

        public string Website { get; set; }

        [Display(Name = nameof(Resource.lblInPipedrive), ResourceType = typeof(Resource))]
        public bool InPipedrive { get; set; }

        public string Notes { get; set; }

    }
}
