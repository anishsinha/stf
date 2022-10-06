using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class UsersFilterViewModel
    {
        public UsersFilterViewModel()
        {
            InstanceInitialize();
        }

        private void InstanceInitialize()
        {
            Filter = CompanyUsersFilterType.All;
        }

        public int Id { get; set; }

        public CompanyUsersFilterType Filter { get; set; }
    }
}
