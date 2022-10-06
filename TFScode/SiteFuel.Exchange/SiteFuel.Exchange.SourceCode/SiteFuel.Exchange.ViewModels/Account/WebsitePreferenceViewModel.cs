using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class WebsitePreferenceViewModel : StatusViewModel
    {
        public int OnBoradId { get; set; }

        [Display(Name = nameof(Resource.lblUploadLogo), ResourceType = typeof(Resource))]
        
        public string ImageFilePath { get; set; }
        public string hdnImageFilePath { get; set; }
        
        public bool IsRemoved { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.lblURLNameRegularExpression))]
        [Display(Name = nameof(Resource.lblURLName), ResourceType = typeof(Resource))]
        public string URLName { get; set; }
    }
}
