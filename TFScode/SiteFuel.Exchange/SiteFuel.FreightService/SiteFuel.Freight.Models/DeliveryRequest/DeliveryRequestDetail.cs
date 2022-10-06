using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightModels.DeliveryRequest
{
    public class DeliveryRequestDetail
    {
        public string Id { get; set; }
        public int LdPri { get; set; } // Load priority
        public string RgId { get; set; } // RegionId
        public string RgName { get; set; } // Region name
        public List<DropdownDisplayItem> States { get; set; } // States
    }

    public class TBDRequestDetailModel
    {
        public string DeliveryRequestId { get; set; }
        public string GroupedParentDrId { get; set; }
        public int FuelTypeId { get; set; }
        public string FuelType { get; set; }
        public int ProductTypeId { get; set; }
        public int UoM { get; set; }
        public bool IsTBD { get; set; }
    }
}
