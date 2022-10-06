using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerMarginViewModel : StatusViewModel
    {
        public BrokerMarginViewModel()
        {
            InstanceInitialize();
        }

        public BrokerMarginViewModel(Status status) : base(status)
        {
            InstanceInitialize(status);
        }

        private void InstanceInitialize(Status status = Status.Failed)
        {
            MarginTypeId = (int)MarginType.NoChange;
        }
        public int MarginTypeId { get; set; }

        public decimal Margin { get; set; }

        public bool IsTierPricing { get; set; }

    }
}
