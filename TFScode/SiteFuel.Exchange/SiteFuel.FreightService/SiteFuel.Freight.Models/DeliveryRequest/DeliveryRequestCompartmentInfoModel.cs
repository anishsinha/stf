using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels
{
    public class DeliveryRequestCompartmentInfoModel
    {
        public string DeliveryRequestId { get; set; }

        public List<CompartmentsInfoViewModel> Compartments = new List<CompartmentsInfoViewModel>();
    }
}
