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
    public class JobToRegionAssignViewModel
    {
        [Display(Name = nameof(Resource.lblDispatchRegion), ResourceType = typeof(Resource))]
        public string RegionId { get; set; }
        public int JobId { get; set; }

        [Display(Name = nameof(Resource.lblLocation), ResourceType = typeof(Resource))]
        public string JobName { get; set; }

        public int UpdatedBy { get; set; }

        public int CompanyId { get; set; }
        [Display(Name = nameof(Resource.lblAssignRoute), ResourceType = typeof(Resource))]
        public string RouteId { get; set; }
        public string DistanceCovered { get; set; }
    }
}
