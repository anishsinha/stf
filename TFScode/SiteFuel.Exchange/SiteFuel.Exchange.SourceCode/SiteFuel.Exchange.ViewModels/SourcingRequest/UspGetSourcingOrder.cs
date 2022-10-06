using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspGetSourcingOrder
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }
        public string JobName { get; set; }
        public string Customer { get; set; }
        public string FuelType { get; set; }
        public decimal Quantity { get; set; }
        public int StatusId { get; set; }

    }
}
