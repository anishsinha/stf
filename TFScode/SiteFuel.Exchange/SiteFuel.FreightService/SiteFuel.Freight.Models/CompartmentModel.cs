using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.FreightModels
{
    public class CompartmentModel
    {
        public string Name { get; set; }
        public decimal FuelCapacity { get; set; }
    }

    public class CompartmentViewModel
    {
        public string CompartmentId { get; set; }
        public decimal Capacity { get; set; }
        public int FuelType { get; set; }
        public bool IsCompartmentAvailable { get; set; } = true;
        public RetainInfo RetainInfo { get; set; } = null;
    }

    public class TrailerCompartmentDetail
    {
        public string TrailerId { get; set; }
        public List<CompartmentViewModel> Compartments { get; set; }
        public bool IsFuelRetain { get; set; } = false;
        public bool IsCompartmentAvailable { get; set; } = true;
    }

    public class RetainInfo
    {
        public int? OrderId { get; set; }
        public int ProductId { get; set; }
        public string DeliveryReqId { get; set; }
        public DropdownDisplayItem TfxTerminal { get; set; }
        public BulkPlantAddressModel TfxBulkPlant { get; set; }
        public PickupLocationType PickupLocationType { get; set; }
        public decimal Quantity { get; set; }
    }
}