using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Foolproof;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestResaleViewModel : BaseViewModel
    {
        public FuelRequestResaleViewModel()
        {
            InstanceInitialize();
        }

        public FuelRequestResaleViewModel(Status status) 
            : base(status)
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            DifferentFuelPrices = new List<DifferentFuelPriceViewModel>();
            FuelPricing = new FuelPricingViewModel();
            ResaleCustomer = new List<FuelRequestResaleCustomerViewModel>();
        }

        public FuelPricingViewModel FuelPricing { get; set; }

        public List<DifferentFuelPriceViewModel> DifferentFuelPrices { get; set; }

        public bool IsDropTicketEnabled { get; set; }

        public List<FuelRequestResaleCustomerViewModel> ResaleCustomer { get; set; }
    }
}
