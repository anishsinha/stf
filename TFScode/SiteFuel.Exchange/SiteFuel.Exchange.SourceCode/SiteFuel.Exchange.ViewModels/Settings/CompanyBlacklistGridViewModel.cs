using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyBlacklistGridViewModel
    {
        public CompanyBlacklistGridViewModel()
        {
            SelectCompanyList = new List<DropdownDisplayItem>();
            BlacklistCompanies = new List<CompanyBlacklistViewModel>();
        }

        [Display(Name = nameof(Resource.lblCompanyName), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public int SelectedCompanyId { get; set; }

        [Display(Name = nameof(Resource.lblReason), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public string Reason { get; set; }

        public List<DropdownDisplayItem> SelectCompanyList { get; set; }

        public List<CompanyBlacklistViewModel> BlacklistCompanies { get; set; }
    }
}
