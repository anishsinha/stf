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
    public class CancelOrderViewModel : StatusViewModel
    {
        public CancelOrderViewModel()
        {
            IsFuelRequestReSubmit = true;
        }

        public CancelOrderViewModel(Status status)
            : base(status)
        {
            IsFuelRequestReSubmit = true;
        }

        public int OrderId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageRequired))]
        [Display(Name = nameof(Resource.lblReason), ResourceType = typeof(Resource))]
        public int ReasonId { get; set; }

        [Display(Name = nameof(Resource.lblReasonForCanceling), ResourceType = typeof(Resource))]
        public string Reason { get; set; }

        [Display(Name = nameof(Resource.lblResubmitFuelRequest), ResourceType = typeof(Resource))]
        public bool IsFuelRequestReSubmit { get; set; }

        public int CanceledBy { get; set; }
    }
}
