using SiteFuel.Exchange.Core.StringResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestGridViewModel : StatusViewModel, IEqualityComparer<FuelRequestGridViewModel>
    {
        public FuelRequestGridViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestGridViewModel(Utilities.Status status)
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            IsCounterOfferDeclinedByBuyer = false;
            IsCounterOfferAvailable = false;
        }

        public string RequestNumber { get; set; }

        public int FuelRequestId { get; set; }

        public string JobName { get; set; }

        public int JobId { get; set; }

        public string Address { get; set; }

        public string FuelType { get; set; }

        public int FuelTypeId { get; set; }

        public string OnSiteContact { get; set; }

        public string GallonsNeeded { get; set; }

        public string PricePerGallon { get; set; }

        public string ContactPerson { get; set; }

        public string Status { get; set; }

        public decimal TotalGallonsDeliveredTillNow { get; set; }

        public int StatusId { get; set; }

        public string Distance { get; set; }

        public string Customer { get; set; }

        public string StartDate { get; set; }

        public string PoNumber { get; set; }

        public int OrderId { get; set; }

        public bool IsCounterOfferAvailable { get; set; }

        public bool IsCounterOfferPendingOnBuyer { get; set; }

        public bool IsCounterOfferPendingOnSupplier { get; set; }

        public bool IsCounterOfferDeclinedByBuyer { get; set; }
        public string StateAndZip { get; set; }

        public int TotalCount { get; set; }

        public decimal FrTotalDollarValue { get; set; }

        public string DeliveryType { get; set; }

        public bool IsOnboardingComplete { get; set; }

        public int OnboardedTypeId { get; set; }

        public UoM UoM { get; set; }

        public bool IsMarineLocation { get; set; }

        public int? AcknowledgementId { get; set; }
        public int AcceptedCompanyId { get; set; }

        public bool Equals(FuelRequestGridViewModel request1, FuelRequestGridViewModel request2)
        {
            return request1.FuelRequestId == request2.FuelRequestId;
        }

        public int GetHashCode(FuelRequestGridViewModel obj)
        {
            return obj.FuelRequestId.GetHashCode();
        }
    }
}
