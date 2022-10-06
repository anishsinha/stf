using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Core;
using System.ComponentModel.DataAnnotations;
using SiteFuel.Exchange.Core.StringResources;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderDataTableViewModel : DataTableAjaxPostModel
    {
        public int JobId { get; set; }
        public int OrderId { get; set; }
        public OrderFilterType Filter { get; set; }
        public int FuelTypeId { get; set; }
        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartDate { get; set; }

        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndDate { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }

        public string GroupIds { get; set; }
        public List<int> CustomerIds { get; set; } = new List<int>();
        public List<int> LocationIds { get; set; } = new List<int>();
        public List<int> VesselIds { get; set; } = new List<int>();
        public bool IsMarine { get; set; }
    }
}
