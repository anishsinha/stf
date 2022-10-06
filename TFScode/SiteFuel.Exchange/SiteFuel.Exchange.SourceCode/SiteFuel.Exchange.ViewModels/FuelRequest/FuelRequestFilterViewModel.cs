using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class FuelRequestFilterViewModel : BaseInputViewModel
    {
        public FuelRequestFilterViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = FuelRequestFilterType.All;
            BrodcastType = BroadcastType.All;
        }

        public int JobId { get; set; }

        public int FuelRequestId { get; set; }

        public BroadcastType BrodcastType { get; set; }

        public FuelRequestFilterType Filter { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }
    }
}
