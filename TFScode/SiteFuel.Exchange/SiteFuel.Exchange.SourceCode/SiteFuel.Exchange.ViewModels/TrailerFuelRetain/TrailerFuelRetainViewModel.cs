using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
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
        public string FuelType { get; set; }
        public int ProductId { get; set; }
        public int? OrderId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int TfxDriverId { get; set; }
        public int? TrackableScheduleId { get; set; }
    }
}
