using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspBrokerFuelRequestGridViewModel
    {
        public int FuelRequestId { get; set; }

        public string Customer { get; set; }

        public string RequestNumber { get; set; }

        public int? OrderId { get; set; }

        public string PONumber { get; set; }

        public string Address { get; set; }

        public string FuelType { get; set; }

        public string ContactPerson { get; set; }

        public decimal OrderedGallons { get; set; }

        public int QuantityTypeId { get; set; }

        public string PricePerGallon { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public bool IsCounterOfferAvailable { get; set; }

        public int TotalCount { get; set; }

    }

    public class BrokerOrderDetails
    {
        public int FuelRequestId { get; set; }
        public int? OrderId { get; set; }
        public string AcceptedCompanyName { get; set; }
        public int AcceptedCompanyId { get; set; }
    }
}