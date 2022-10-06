using SiteFuel.Exchange.ViewModels.InvoiceExceptions;
using System.Collections.Generic;

namespace SiteFuel.Exchange.ViewModels
{
    public class TrailerFuelRetainException
    {       
        public CompanyExceptionModel companyExceptionModel { get; set; }
        public List<TruckDetailViewModel> truckDetailViewModels { get; set; }
    }
}
