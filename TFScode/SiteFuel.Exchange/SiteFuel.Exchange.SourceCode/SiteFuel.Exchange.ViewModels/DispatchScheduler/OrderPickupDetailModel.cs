using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderPickupDetailModel
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public string PoNumber { get; set; }
        public string TerminalName { get; set; }
        public int TerminalId { get; set; }
        public int PickupLocationType { get; set; } = 1;
        public string BulkplantName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public int? StateId { get; set; }
        public string CountryCode { get; set; }
        public string ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string CountyName { get; set; }
        public string TimeZoneName { get; set; }
        public bool IsDispatchRetainedByCustomer { get; set; }
        public TruckLoadTypes TruckLoadType { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public int FuelTypeId { get; set; }
        public int JobId { get; set; }
        public string Badge1 { get; set; }
        public string Badge2 { get; set; }
        public string Badge3 { get; set; }
        public string DRNote { get; set; }
        public bool IsOrderInfoDisplay { get; set; } = true;
        public string UoM { get; set; }
        public int SiteId { get; set; }
    }

    public class OrderPartialDetailModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int FuelTypeId { get; set; }
        public int JobId { get; set; }
    }
}
