using System;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFuelRequestViewModel : BaseCultureViewModel
    {
        public BrokerFuelRequestViewModel()
        {
            InstanceInitialize();
        }

        public BrokerFuelRequestViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }
        private void InstanceInitialize(Status status = Status.Failed)
        {
            Details = new BrokerFuelRequestDetailsViewModel();
            Terms = new BrokerFuelRequestTermsViewModel();
            Type = (int)FuelRequestType.BrokeredFuelRequest;
        }

        public BrokerFuelRequestDetailsViewModel Details { get; set; }
        public BrokerFuelRequestTermsViewModel Terms { get; set; }
        public int FuelRequestId { get; set; }
        public int Type { get; set; }
        public int CompanyId { get; set; }
        public int ParentId { get; set; }
        public DateTimeOffset ParentStartDate { get; set; }
        public DateTimeOffset? ParentEndDate { get; set; }
        public DateTimeOffset? ParentExpiryDate { get; set; }
        public bool IsCounterOfferExists { get; set; }

        public int StatusId { get; set; }
        public string RequestNumber { get; set; }
    }
}
