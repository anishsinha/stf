using SiteFuel.Exchange.Utilities;
using System;

namespace SiteFuel.Exchange.ViewModels
{
    public class UspSupplierFuelRequestGridViewModel
    {
        public int FuelRequestId { get; set; }

        public string Customer { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string FuelType { get; set; }

        public int FuelTypeId { get; set; }

        public decimal GallonsNeeded { get; set; }

        public string PricePerGallon { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public string RequestNumber { get; set; }

        public bool IsCounterOfferAvailable { get; set; }

        public bool IsCounterOfferDeclinedByBuyer { get; set; }

        public double Distance { get; set; }

        public string StatusName { get; set; }

        public decimal DeliveredTillNow { get; set; }

        public int TotalCount { get; set; }

        public decimal FrTotalDollarValue { get; set; }

        public int CounterOfferBuyerStatus { get; set; }

        public int CounterOfferSupplierStatus { get; set; }

        public string DeliveryType { get; set; }

        public bool IsOnboardingComplete { get; set; }

        public int OnboardedTypeId { get; set; }
        public string UoD { get; set; }

        public UoM UoM { get; set; }

        public bool IsMarineLocation { get; set; }

        public int? AcknowledgementId { get; set; }
    }
}
