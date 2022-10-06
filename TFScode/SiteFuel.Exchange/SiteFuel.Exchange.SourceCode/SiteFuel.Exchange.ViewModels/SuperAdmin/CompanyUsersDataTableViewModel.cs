using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyUsersDataTableViewModel : DataTableAjaxPostModel
    {
        public CompanyUsersDataTableViewModel()
        {
            StatusFilter = CompanyUsersFilterType.All;
            RoleFilter = SiteFuelUserFilterType.All;
        }

        public int CompanyId { get; set; }

        public CompanyUsersFilterType StatusFilter { get; set; }

        public SiteFuelUserFilterType RoleFilter { get; set; }
    }
}
