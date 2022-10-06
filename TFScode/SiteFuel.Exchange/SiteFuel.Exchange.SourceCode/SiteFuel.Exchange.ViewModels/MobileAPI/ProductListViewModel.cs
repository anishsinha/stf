using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductDisplayGroupId { get; set; }

        public int CountryId { get; set; }
    }
}
