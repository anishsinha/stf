using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class DSPublishMessageViewModel
    {
        public DeliveryGroupStatus DeliveryGroupPrevStatus { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public string StartDate { get; set; }
        public List<DSDeliveryRequestMessageViewModel> DeliveryRequests { get; set; } = new List<DSDeliveryRequestMessageViewModel>();
    }
    public class DSDeliveryRequestMessageViewModel
    {
        public decimal RequiredQuantity { get; set; }
        public int? OrderId { get; set; }
        public string ProductType { get; set; }
        public int UoM { get; set; }
    }
}
