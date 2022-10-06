using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestViewModel : BaseCultureViewModel
    {
        public FuelRequestViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            Job = new JobSelectionViewModel(status);
            FuelDetails = new FuelDetailsViewModel(status);
            FuelDeliveryDetails = new FuelDeliveryDetailsViewModel(status);
            FuelOfferDetails = new FuelOfferDetailsViewModel(status);
            CounterOfferDetails = new CounterOfferDetailsViewModel(status);
            FuelRequestResale = new FuelRequestResaleViewModel(status);
            IsCounterOffer = false;
            IsCounterOfferExists = false;
            IsSupplierCounterOfferExists = false;
            CounterOfferSupplierId = 0;
            TPOSupplierId = 0;
            HideButtons = false;
        }

        public int Id { get; set; }

        public int CompanyId { get; set; }

        public bool IsCounterOffer { get; set; }

        public bool IsCounterOfferExists { get; set; }

        public bool IsSupplierCounterOfferExists { get; set; }

        public int CounterOfferSupplierId { get; set; }

        public int TPOSupplierId { get; set; }

        [Display(Name = nameof(Resource.lblPoNumber), ResourceType = typeof(Resource))]
        [Remote("IsValidPONumber", "Validation", AreaReference.UseRoot, AdditionalFields = "Id,CompanyId,IsCounterOffer", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = nameof(Resource.valMessageAlreadyExist))]
        public string ExternalPoNumber { get; set; }

        public string RequestNumber { get; set; }

        public int FuelRequestTypeId { get; set; }

        public JobSelectionViewModel Job { get; set; }

        public FuelDetailsViewModel FuelDetails { get; set; }

        public CounterOfferDetailsViewModel CounterOfferDetails { get; set; }

        public FuelDeliveryDetailsViewModel FuelDeliveryDetails { get; set; }

        public FuelOfferDetailsViewModel FuelOfferDetails { get; set; }

        public FuelRequestResaleViewModel FuelRequestResale { get; set; }

        public bool HideButtons { get; set; }

        public bool IsBrokeredCounterOffer { get; set; }

        public string PoNumber { get; set; }

        public bool IsOrderActive { get; set; }

        public int OrderId { get; set; }
        public int JobCountryId { get; set; } // added new as, if we use Coutnryviewmodel - it gives req error while posting form        
    }
}
