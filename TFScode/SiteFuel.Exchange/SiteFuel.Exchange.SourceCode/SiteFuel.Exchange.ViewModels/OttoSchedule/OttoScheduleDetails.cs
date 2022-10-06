using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
   public class OttoScheduleDetails
    {
        public string ShiftId { get; set; }
        public string ShiftName { get; set; }
    }
    public class OttoDeliveryRequests
    {
        public string Date { get; set; }
        public string ShiftTime { get; set; }
        public int Priority { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string QuantityUOM { get; set; }
        public string TankName { get; set; }
        public string LocationName { get; set; }
        public string TankCapacity { get; set; }
    }
    public class OttoNotifications
    {
        public string Id { get; set; }
        public string message { get; set; }
        public int Status { get; set; }
    }
}
