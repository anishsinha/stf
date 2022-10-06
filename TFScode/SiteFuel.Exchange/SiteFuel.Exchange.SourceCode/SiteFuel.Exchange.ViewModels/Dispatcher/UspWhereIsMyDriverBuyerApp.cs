using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class UspWhereIsMyDriverBuyerApp
    {
        public int? DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Dispatcher { get; set; }
        public int? DispatcherId { get; set; }
        public decimal? DriverLatitude { get; set; }
        public decimal? DriverLongitude { get; set; }
        public decimal Quantity { get; set; }
        public UoM UoM { get; set; }
        public string PoNumber { get; set; }
        public string Date { get; set; }
        public string ProductName { get; set; }
        public string Pickup { get; set; }
        public string Location { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
        public int JobStateId { get; set; }
        public string JobState { get; set; }
        public decimal? JobLatitude { get; set; }
        public decimal? JobLongitude { get; set; }
        public string JobCity { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public int TrackableScheduleId { get; set; }
        public int DeliveryScheduleId { get; set; }        
        public int DeliveryScheduleStatusId { get; set; }
        public int OrderId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompany { get; set; }
        public int CarrierCompanyId { get; set; }
        public string CarrierCompany { get; set; }
        public string ETA { get; set; }
        public string EnrouteDeliveryStatus { get; set;}
        public int? LdPri { get; set;}
        public string AppLastUpdatedDate { get; set; }
        public string TimeZoneName { get; set; }
        public bool AllowCustomerDriverChat { get; set; }
        public bool IsOnline { get; set; }
    }
}
