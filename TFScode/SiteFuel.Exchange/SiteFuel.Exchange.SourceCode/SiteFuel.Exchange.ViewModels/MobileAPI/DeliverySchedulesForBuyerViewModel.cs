using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.MobileAPI
{
    public class DeliverySchedulesForBuyerViewModel
    {
        public int DeliveryScheduleId { get; set; }
        public int OrderId { get; set; }
        public int TrackableScheduleId { get; set; }
        public string FuelType { get; set; }
        public int FuelTypeId  { get; set; }
        public string ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public string GallonsOrdered { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string PoNumber { get; set; }
        public string PickUpAddress { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public string JobAddress { get; set; }
        public int DispatcherId { get; set; }
        public string Dispatcher { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset ScheduleDate { get; set; }
        public string ScheduleStartTime { get; set; }
        public string ScheduleEndTime { get; set; }
    }
}
