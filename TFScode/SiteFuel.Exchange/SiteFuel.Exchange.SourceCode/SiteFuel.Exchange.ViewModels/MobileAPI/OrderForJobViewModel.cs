using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class OrderForJobViewModel
    {
        public OrderForJobViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public int OrderId { get; set; }

        public int QuantityTypeId { get; set; }

        public string FuelType { get; set; }

        public string Status { get; set; }

        public string PoNumber { get; set; }

        public string PricePerGallon { get; set; }

        public decimal GallonsOrdered { get; set; }

        public decimal GallonsDelivered { get; set; }

        public DateTimeOffset StartDate { get; set; }

    }


    public class JobOrderDetails
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int AcceptedCompanyId { get; set; }
        public int BuyerCompanyId { get; set; }
        public string PoNumber { get; set; }
        public int ProductTypeId { get; set; }
    }
}
