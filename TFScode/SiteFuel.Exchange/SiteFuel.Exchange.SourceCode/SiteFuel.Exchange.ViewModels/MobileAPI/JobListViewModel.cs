using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class JobListViewModel 
    {
        public JobListViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public int JobId { get; set; }

        public string Name { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int UnitOfMeasurement { get; set; }
        
        public int Currency { get; set; }

        public int CountryId { get; set; }

        public bool IsRetailJob { get; set; }
        public bool IsMarine { get; set; }
    }
}
