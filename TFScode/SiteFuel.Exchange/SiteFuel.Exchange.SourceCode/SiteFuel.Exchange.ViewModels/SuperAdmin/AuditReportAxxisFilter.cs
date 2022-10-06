using SiteFuel.Exchange.Core.StringResources;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AuditReportAxxisFilter
    {
        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndDate { get; set; }
    }
}