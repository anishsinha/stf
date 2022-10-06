using SiteFuel.Exchange.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteFuel.Exchange.ViewModels
{
    public class CounterOfferViewModel : BaseViewModel
    {
        public CounterOfferViewModel()
        {
            InstanceInitialize();
        }

        public CounterOfferViewModel(Status status)
            : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Utilities.Status.Failed)
        {
            Id = 0;
            PreviousCounterOfferDetails = new FuelRequestViewModel(status);
            CurrentCounterOfferDetails = new FuelRequestViewModel(status);
            FuelRequest = new FuelRequestViewModel(status);
            IsAcceptVisible = false;
            IsDeclineVisible = false;
            IsCounterOfferVisible = false;
            IsCancelVisible = false;
            IsCloneVisible = false;
        }

        public int Id { get; set; }

        public int Status { get; set; }

        public bool IsAcceptVisible { get; set; }

        public bool IsDeclineVisible { get; set; }

        public bool IsCounterOfferVisible { get; set; }

        public bool IsCancelVisible { get; set; }

        public bool IsCloneVisible { get; set; }

        public string Buyer { get; set; }

        public string Supplier { get; set; }

        public FuelRequestViewModel PreviousCounterOfferDetails { get; set; }

        public FuelRequestViewModel CurrentCounterOfferDetails { get; set; }

        public FuelRequestViewModel FuelRequest { get; set; }
    }
}