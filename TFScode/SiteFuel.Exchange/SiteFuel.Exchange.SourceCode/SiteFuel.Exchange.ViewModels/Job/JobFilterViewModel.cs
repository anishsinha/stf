using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobFilterViewModel : BaseInputViewModel
    {
        public JobFilterViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = JobFilterType.All;
        }

        public int Id { get; set; }

        public JobFilterType Filter { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }
    }
}
