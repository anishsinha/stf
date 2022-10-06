using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyGroupViewModel : BaseViewModel
    {
        public CompanyGroupViewModel()
        {
            Companies = new List<ChildCompanyViewModel>();
        }

        [Display(Name="Parent Account")]
        [Required]
        public int OwnerCompanyId { get; set; }

        public string CompanyName { get; set; }

        public CompanyGroupType OwnerCompanyType { get; set; }

        public string CompanyType { get; set; }

        public Nullable<int> ParentCompanyId { get; set; }

        public List<ChildCompanyViewModel> Companies { get; set; }

        public string SeletedChildCompanies { get; set; }

        public bool IsEditAccount { get; set; }

        public bool IsSubGroupExist { get; set; }
    }
}
