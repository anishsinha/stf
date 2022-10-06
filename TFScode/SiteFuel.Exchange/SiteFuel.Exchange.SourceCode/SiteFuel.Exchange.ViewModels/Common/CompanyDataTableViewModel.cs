using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyDataTableViewModel : DataTableAjaxPostModel
    {
        public CompanyDataTableViewModel()
        {
            filter = CompanyFilterType.All;
        }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public CompanyFilterType filter { get; set; }
    }
}
