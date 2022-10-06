using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
    #else
        [ApiExplorerSettings(IgnoreApi = true)]
    #endif

    [ValidateToken]

    public class CustomerController : ApiBaseController
    {
        [HttpPost]
        [ApiLog(Enabled = true, TPDLogEnabled = true)]
        public async Task<ApiResponseViewModel> Create(TPDCustomerViewModel viewModel)
        {
            ApiResponseViewModel response;
            if (viewModel == null)
            {
                response = new ApiResponseViewModel();

                response.Messages.Add(new ApiCodeMessages()
                {
                    Code = Constants.ApiCodeRQ01,
                    Message = "Invalid request object",
                    
                });
                response.Status = Status.Failed;
                return response;
            }

            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<OnboardingDomain>().CreateCustomerFromApi(viewModel, token);
            return response;


        }

    }
}