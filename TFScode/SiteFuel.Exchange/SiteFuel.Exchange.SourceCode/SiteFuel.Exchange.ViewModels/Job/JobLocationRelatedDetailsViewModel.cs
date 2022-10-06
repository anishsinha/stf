using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Job
{
   public class JobLocationRelatedDetailsViewModel
    {
        public string JobID { get; set; }
        public int CompanyID { get; set; }
        public List<DeliveryRequestViewModel> deliveryRequestViewModels { get; set; } = new List<DeliveryRequestViewModel>();
        public List<JobAdditionalDetailsViewModel> jobAdditionalDetailsModels { get; set; } = new List<JobAdditionalDetailsViewModel>();
    }
}
