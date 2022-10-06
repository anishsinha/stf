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
    public class DispalyMessageViewModel : ResponseViewModel
    {
        public DispalyMessageViewModel()
            : base(Status.Failed)
        {
            To = new List<string>();
        }

        public DispalyMessageViewModel(Status status)
            : base(status)
        {
            To = new List<string>();
        }

        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblSubject), ResourceType = typeof(Resource))]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblFrom), ResourceType = typeof(Resource))]
        public string From { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblTo), ResourceType = typeof(Resource))]
        public List<string> To { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblMessages), ResourceType = typeof(Resource))]
        public string Message { get; set; }

        public string DaysAgo { get; set; }

        public string HoursAgo { get; set; }
    }
}
