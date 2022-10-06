using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class ExternalSupplierViewModel : StatusViewModel
    {
        public ExternalSupplierViewModel()
        {
            CompanyDetails = new ExternalCompanyViewModel();
            ContactPersonDetails = new ContactPersonViewModel();
            CompanyAddress = new LocationsViewModel();
            OtherLocationsAndServices = new List<LocationsViewModel>();
        }

        public ExternalSupplierViewModel(Status status)
            : base(status)
        {
            CompanyDetails = new ExternalCompanyViewModel();
            ContactPersonDetails = new ContactPersonViewModel();
            CompanyAddress = new LocationsViewModel();
            OtherLocationsAndServices = new List<LocationsViewModel>();
        }

        public ExternalCompanyViewModel CompanyDetails { get; set; }

        public ContactPersonViewModel ContactPersonDetails { get; set; }

        public LocationsViewModel CompanyAddress { get; set; }

        public List<LocationsViewModel> OtherLocationsAndServices { get; set; }
    }
}
