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
    public class SupplierWorkingHoursViewModel : StatusViewModel
    {
        public SupplierWorkingHoursViewModel()
        {
        }

        public SupplierWorkingHoursViewModel(Status status) 
            : base(status)
        {
        }

        public int AddressId { get; set; }

        public int WeekDayId { get; set; }

        [Display(Name = nameof(Resource.lblDay), ResourceType = typeof(Resource))]
        public string WeekDayName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string StartTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public string EndTime { get; set; }
    }
}
