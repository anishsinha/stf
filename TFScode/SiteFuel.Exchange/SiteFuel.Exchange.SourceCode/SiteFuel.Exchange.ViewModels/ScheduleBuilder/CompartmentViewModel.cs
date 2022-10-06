using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    public class TrailerCompartmentDetail
    {
        public string TrailerId { get; set; }
        public List<CompartmentViewModel> Compartments { get; set; }
        public bool IsFuelRetain { get; set; } = false;
        public bool IsCompartmentAvailable { get; set; } = true;
    }

    public class CompartmentViewModel
    {
        public string CompartmentId { get; set; }
        public decimal Capacity { get; set; }
        public int FuelType { get; set; }
        public bool IsCompartmentAvailable { get; set; } = true;
        public RetainInfo RetainInfo { get; set; } = null;
    }
    public class RetainInfo
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string DeliveryReqId { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; }
        public DropAddressViewModel TfxBulkPlant { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public decimal Quantity { get; set; }
    }
    public class TrailerRetainCompartment
    {
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal Qty { get; set; }
    }
}
