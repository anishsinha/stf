using SiteFuel.BAL;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using SiteFuel.PricingService.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.PricingService.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class CurrentCostController : ApiController
    {
        private readonly ICurrentCostDomain _currentCostDomain;

        public CurrentCostController(ICurrentCostDomain currentCostDomain)
        {
            _currentCostDomain = currentCostDomain;
        }
        
        [HttpPost]
        public async Task<CurrentCostResponseModel> UpdateSupplierCostToPriceDetail(CurrentCostRequestModel requestModel)
        {
            CurrentCostResponseModel response = await _currentCostDomain.UpdateSupplierCostToPriceDetail(requestModel);
            return response;
        }
    }
}
