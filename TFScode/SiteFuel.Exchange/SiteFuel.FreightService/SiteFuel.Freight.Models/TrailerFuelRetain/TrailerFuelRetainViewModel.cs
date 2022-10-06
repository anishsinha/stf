using System;

namespace SiteFuel.FreightModels
{
    public class TrailerFuelRetainViewModel
    {
        public string Id { get; set; }
        public string TrailerId { get; set; }
        public string CompartmentId { get; set; }
        public decimal CompartmentCapacity { get; set; }
        public decimal Quantity { get; set; }
        public int UOM { get; set; }
        public string DeliveryRequestId { get; set; }
        public string ProductType { get; set; }
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public int TfxDriverId { get; set; }
        
    }
}
