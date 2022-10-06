using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
   public class JobLocationRelatedDetailsModel
    {
        public string JobID { get; set; }
        public int CompanyID { get; set; }
        public bool IsBuyerCompany { get; set; }
        public List<DeliveryRequestViewModel> deliveryRequestViewModels { get; set; } = new List<DeliveryRequestViewModel>();
        public List<JobAdditionalDetailsModel> jobAdditionalDetailsModels { get; set; } = new List<JobAdditionalDetailsModel>();
    }
}
