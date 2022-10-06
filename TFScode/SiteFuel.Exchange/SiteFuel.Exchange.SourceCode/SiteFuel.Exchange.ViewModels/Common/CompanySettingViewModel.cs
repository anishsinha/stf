using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanySettingViewModel 
    {
        public CompanySettingViewModel()
        {
            
        }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblProcessingFeeType), ResourceType = typeof(Resource))]
        public int? ProcessingFeeType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblProcessingFee), ResourceType = typeof(Resource))]
        public decimal ProcessingFee { get; set; }
    }
}
