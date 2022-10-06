using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class FilldDetailsQueueMessage
    {
        public int SupplierCompanyId { get; set; }
        public int UserId { get; set; }
        public int DriverId { get; set; }
        public string TruckId { get; set; }
        public int JobId { get; set; }
        public string RegionId { get; set; }
        public string Locale { get; set; }
        public int TrackableScheduleId { get; set; }
        public int DeliveryGroupId { get; set; }
        public string ScheduleDate { get; set; }
    }

}
