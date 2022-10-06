using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.ViewModels
{
    public class CompanyInformationViewModel : StatusViewModel
    {
        public CompanyInformationViewModel()
        {
            InstanceInitialize();            
        }

        public CompanyInformationViewModel(Status status)
            : base(status)
        {
            InstanceInitialize();
        }
		
        private void InstanceInitialize()
        {
            Company = new CompanyViewModel();
        }

        public CompanyViewModel Company { get; set; }
    }
}
