using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SubcontractorViewModel : BaseViewModel
    {
        public SubcontractorViewModel()
        {
            
        }

        public SubcontractorViewModel(Status status) 
            : base(status)
        {
           
        }

        
        public int Id { get; set; }

        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Name { get; set; }       
    }
}
