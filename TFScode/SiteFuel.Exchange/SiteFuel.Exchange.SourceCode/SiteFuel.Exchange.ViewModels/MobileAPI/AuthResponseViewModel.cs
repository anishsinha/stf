using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.ViewModels
{
    public class AuthResponseViewModel
    {
        public AuthResponseViewModel(AuthStatus status = AuthStatus.Failed)
        {
            if (status == AuthStatus.Success)
            {
                StatusCode = AuthStatus.Success;
                StatusMessage = Resource.errMessageSuccess;
            }
            else
            {
                StatusCode = AuthStatus.Failed;
                StatusMessage = Resource.errMessageFailed;
            }
        }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public bool IsBuyerTPOCreated { get; set; }

        public int CompanyDefaultCountry { get; set; }

        public string Token { get; set; }

        public string UserName { get; set; }

        public AuthStatus StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public int BrandedCompanyId { get; set; }
    }
}
