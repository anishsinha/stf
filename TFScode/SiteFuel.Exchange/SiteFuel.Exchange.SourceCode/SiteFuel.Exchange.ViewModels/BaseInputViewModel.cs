using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class BaseInputViewModel
    {
        public BaseInputViewModel()
        {
            Country = new CountryViewModel();
        }

        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndDate { get; set; }

        public CountryViewModel Country { get; set; }

        public string GroupIds { get; set; }
    }
}
