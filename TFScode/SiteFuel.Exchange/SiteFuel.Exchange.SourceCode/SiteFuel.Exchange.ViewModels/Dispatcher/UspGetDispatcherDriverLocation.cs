using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class UspGetDispatcherDriverLocation
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Dispatcher { get; set; }
        public string Customer { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal Quantity { get; set; }
        public int QuantityTypeId { get; set; }
        public UoM UoM { get; set; }
        public string PoNumber { get; set; }
        public string Date { get; set; }
        public string ProductName { get; set; }
        public string Pickup { get; set; }
        public string Location { get; set; }
        public int StateId { get; set; }
        public decimal JobLatitude { get; set; }
        public decimal JobLongitude { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string DrId { get; set; }
        public int LdPri { get; set; } // Load priority
        public string RgId { get; set; } // RegionId
        public string UniqueOrderNo { get; set; } // DR_ID
        public string DROPTicketNum { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

       public string AppLastUpdatedDate { get; set; }

        public string TimeZoneName { get; set; }
        public bool AllowCustomerDriverChat { get; set; }
        public bool IsOnline { get; set; }
        public string  TrailerDisplayId { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
    }
}
