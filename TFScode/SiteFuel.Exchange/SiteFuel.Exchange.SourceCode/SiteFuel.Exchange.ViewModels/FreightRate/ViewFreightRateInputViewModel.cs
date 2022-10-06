using Foolproof;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class ViewFreightRateInputViewModel
    {
        public ViewFreightRateInputViewModel()
        {
        }

        [Display(Name = nameof(Resource.lblStartDate), ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        public DateTimeOffset? StartDate { get; set; }

        [Display(Name = nameof(Resource.lblEndDate), ResourceType = typeof(Resource))]
        [GreaterThanOrEqualTo("StartDate", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageGreaterThan), PassOnNull = true)]
        public DateTimeOffset? EndDate { get; set; }

        public string FreightRateRuleTypeIds { get; set; }

        public string TableTypeIds { get; set; }

        public string CustomerIds { get; set; }

        public string CarrierIds { get; set; }

        public string TerminalIds { get; set; }

        public string SourceRegionIds { get; set; }

        public string BulkPlantIds { get; set; }

        public bool IsArchived { get; set; }
    }
}
