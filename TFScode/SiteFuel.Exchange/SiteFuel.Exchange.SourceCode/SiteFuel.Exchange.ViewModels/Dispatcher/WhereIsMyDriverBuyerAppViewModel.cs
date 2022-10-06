using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class WhereIsMyDriverBuyerAppViewModel
    {
        public List<WhereIsMyDriverBuyerAppLocation> Locations { get; set; } = new List<WhereIsMyDriverBuyerAppLocation>();
    }

    public class WhereIsMyDriverBuyerAppLocation
    {
        public int JobId { get; set; }
        public string JobName { get; set; }
        public WhereIsMyDriverBuyerAppLocationsAddress Address { get; set; }
        public List<WhereIsMyDriverBuyerAppLocationsSchedules> Schedules { get; set; } = new List<WhereIsMyDriverBuyerAppLocationsSchedules>();
    }
    public class WhereIsMyDriverBuyerAppLocationsAddress
    {
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
    public class WhereIsMyDriverBuyerAppLocationsSchedules
    {
        public int DeliveryScheduleId { get; set; }
        public int TrackableScheduleId { get; set; }
        public string EnrouteDeliveryStatus { get; set; }
        public int OrderId { get; set; }
        public int SupplierCompanyId { get; set; }
        public string SupplierCompany { get; set; }
        public int CarrierCompanyId { get; set; }
        public string CarrierCompany { get; set; }
        public List<WhereIsMyDriverBuyerAppDriverDetails> Drivers { get; set; } = new List<WhereIsMyDriverBuyerAppDriverDetails>();
    }
    public class WhereIsMyDriverBuyerAppDriverDetails
    {
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string LastUpdatedDate { get; set; }
        public string ETA { get; set; }
        public bool IsOnline { get; set; }
        public bool AllowCustomerDriverChat { get; set; }
    }
}
