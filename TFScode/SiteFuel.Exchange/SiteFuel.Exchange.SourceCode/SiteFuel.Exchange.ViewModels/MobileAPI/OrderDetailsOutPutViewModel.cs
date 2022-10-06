using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderDetailsOutPutViewModel 
    {
        public OrderDetailsOutPutViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DeliveryDetails = new FuelDeliveryDetailsViewModel();
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
        }

        public int Id { get; set; }

        public string FuelRequestType { get; set; }

        public string FuelType { get; set; }

        public string PoNumber { get; set; }

        public decimal GallonsOrdered { get; set; }

        public decimal GallonsDelivered { get; set; }

        public int QuantityTypeId { get; set; }

        public decimal GallonsRemaining { get; set; }

        public string PricePerGallon { get; set; }

        public int UnitOfMeasurement { get; set; }

        public int Currency { get; set; }

        public FuelDeliveryDetailsViewModel DeliveryDetails { get; set; }

        public List<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }
    }
}
