using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class BrokerFuelRequestDataTableViewModel : DataTableAjaxPostModel
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int OrderId { get; set; }

        public Currency Currency { get; set; }

        public int CountryId { get; set; }
    }
}
