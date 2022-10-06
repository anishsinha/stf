using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class QuoteRequestDataTableModel : BaseInputViewModel
    {
        public int JobId { get; set; }

        public QuoteRequestFilterType Filter { get; set; } = QuoteRequestFilterType.All;

        public Currency Currency { get; set; }

        public int CountryId { get; set; }
    }
}