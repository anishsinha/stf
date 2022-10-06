using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Core;
using System.ComponentModel.DataAnnotations;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductDataTableViewModel : DataTableAjaxPostModel
    {
        public ProductDataTableViewModel()
        {
        }

        public int ProductId { get; set; }

        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndDate { get; set; }
    }
}
