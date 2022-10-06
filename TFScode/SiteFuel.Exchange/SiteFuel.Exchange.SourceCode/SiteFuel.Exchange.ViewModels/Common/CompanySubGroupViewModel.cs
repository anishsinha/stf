using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanySubGroupViewModel : BaseViewModel
    {
        public CompanySubGroupViewModel()
        {
            Companies = new List<ChildCompanyViewModel>();
            SubGroupCompanyIds = new List<int>();
            CompanyGroup = new CompanyGroupViewModel();
            GroupIds = new List<int>();
        }

        public int Id { get; set; }

        public List<int> GroupIds { get; set; }

        public string EncryptedGroupIds { get; set; }

        public int OwnerCompanyId { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string SubGroupName { get; set; }

        [Required]
        [Display(Name = "Account Type")]
        public CompanyGroupType CompanyType { get; set; }

        [Required]
        [Display(Name = "Accounts")]
        public IList<int> SubGroupCompanyIds { get; set; }

        public CompanyGroupViewModel CompanyGroup { get; set; }

        public List<ChildCompanyViewModel> Companies { get; set; }
    }
}
