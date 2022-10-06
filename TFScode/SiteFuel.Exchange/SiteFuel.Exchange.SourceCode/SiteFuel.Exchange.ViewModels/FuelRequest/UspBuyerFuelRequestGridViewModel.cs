using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspBuyerFuelRequestGridViewModel
    {
        public int FuelRequestId { get; set; }

        public string RequestNumber { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string JobName { get; set; }

        public int JobId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string StateCode { get; set; }

        public string ZipCode { get; set; }

        public string FuelType { get; set; }

        public decimal Gallons { get; set; }

        public string PricePerGallon { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactNumber { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public bool IsCounterOfferAvailable { get; set; }

        public int TotalCount { get; set; }

        public string DeliveryType { get; set; }

        public UoM UoM { get; set; }
        public int AcceptedCompanyId { get; set; }
    }
}
