using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class SupplierSourceViewModel
    {
        public int? Id { get; set; }

        [Display(Name = nameof(Resource.lblSupplierSource), ResourceType = typeof(Resource))]
        public string Name { get; set; }

        [Display(Name = nameof(Resource.lblContractNumber), ResourceType = typeof(Resource))]
        public string ContractNumber { get; set; }
    }
}
