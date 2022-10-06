using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels.Dispatcher
{
    public class WhereIsMyDriverViewModel
    {
        public int Id { get; set; } // DriverId
        public string Name { get; set; } // Driver name
        public string Intl { get; set; } // Driver name initials
        public string DName { get; set; } // Dispatcher name
        public string CName { get; set; } // Customer name
        public string PhNo { get; set; } // Driver phone number
        public decimal? Lat { get; set; } // Driver current latitude
        public decimal? Lng { get; set; } // Driver current longitude
        public string LicNo { get; set; } // Driver license number
        public int LdPri { get; set; } // Load priority
        public string RgId { get; set; } // RegionId
        public string RgName { get; set; } // Region name
        public List<DropdownDisplayExtendedItem> RgStates { get; set; } // Region states
        public int StId { get; set; } // StateId
        public string StName { get; set; } // State name
        public string PoNum { get; set; } // PoNumber
        public string Pckup { get; set; } // Pickup location
        public string Loc { get; set; } // Drop location
        public decimal dLat { get; set; } // Drop latitude
        public decimal dLng { get; set; } // Drop longitude
        public string PrdtNm { get; set; } // Product name
        public string Qty { get; set; } // Drop quantity
        public string LdDate { get; set; } // Load date
        public int? SttsId { get; set; } // Delivery current status
        public string Status { get; set; } // Delivery current status
        public string DrId { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string DROPTicketNum { get; set; }
        public InventoryDataCaptureType InventoryDataCaptureType { get; set; }
        public string InventoryDataCaptureTypeName { get; set; }
        public string TrailerDisplayId { get; set; }        
        public string UniqueOrderNo { get; set; }
        public List<string> ListDROPTicketNum
        {
            get
            {
                if(!string.IsNullOrEmpty(DROPTicketNum))
                { 
                return DROPTicketNum.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                 .Where(x => !string.IsNullOrWhiteSpace(x))
                 .Select(s => s.Trim()).ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
        }

        public string AppLastUpdatedDate { get; set; }
        public bool IsOnline { get; set; }
        public bool AllowCustomerDriverChat { get; set; }
        public int FuelRetainCount  { get; set; }
    }
    public class TrailerRetainDetails
    {
        public int DriverId { get; set; }
        public string TrailerId { get; set; }
        public int RetainFuelCount { get; set; }
    }
}
