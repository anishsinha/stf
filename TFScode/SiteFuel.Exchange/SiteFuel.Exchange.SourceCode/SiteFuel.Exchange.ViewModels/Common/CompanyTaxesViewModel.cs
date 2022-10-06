using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyTaxesViewModel : StatusViewModel
    {
        public bool IsDirectTax { get; set; }

        [Display(Name = nameof(Resource.lblState), ResourceType = typeof(Resource))]
        public List<DirectTaxesViewModel> DirectTaxes { get; set; } = new List<DirectTaxesViewModel>();

        public bool IsEdit { get; set; }

        public bool IsNoraExclusion { get; set; }

    }
}
