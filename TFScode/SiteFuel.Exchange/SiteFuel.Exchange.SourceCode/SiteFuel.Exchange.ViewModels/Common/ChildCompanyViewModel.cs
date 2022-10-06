using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class ChildCompanyViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CompanyType { get; set; }

        public CompanyGroupType ChildCompanyType { get; set; }

        public Nullable<int> ParentCompanyId { get; set; }

        public string SelectedCompanyNames { get; set; }
    }
}
