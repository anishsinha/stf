using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFilterViewModel : BaseInputViewModel
    {
        public Currency Currency { get; set; }

        public int CountryId { get; set; }
    }
}
