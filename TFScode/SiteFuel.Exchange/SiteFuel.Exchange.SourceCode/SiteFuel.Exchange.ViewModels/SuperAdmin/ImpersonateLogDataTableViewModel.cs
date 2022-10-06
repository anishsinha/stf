using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;
namespace SiteFuel.Exchange.ViewModels
{
    public class ImpersonateLogDataTableViewModel : DataTableAjaxPostModel
    {
        public ImpersonateLogDataTableViewModel()
        {
        }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int? ImpersonatedBy { get; set; }
    }
}
