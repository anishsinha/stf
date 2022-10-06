using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFuel.BAL;
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
    public class PricingRequestController : ApiController
    {
        private readonly IPricingRequestDomain _pricingReqDomain;

        public PricingRequestController(IPricingRequestDomain pricingReqDomain)
        {
            _pricingReqDomain = pricingReqDomain;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<CustomResponseModel> SaveRequestDetails(JObject responseString)
        {
            PricingRequestViewModel requestModel = JsonConvert.DeserializeObject<PricingRequestViewModel>(responseString.ToString());
            CustomResponseModel response;
            response = await _pricingReqDomain.SaveRequestDetails(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<CustomResponseModel> UpdateRequestDetails(JObject responseString)
        {
            PricingRequestViewModel requestModel = JsonConvert.DeserializeObject<PricingRequestViewModel>(responseString.ToString());
            CustomResponseModel response;
            response = await _pricingReqDomain.UpdateRequestDetails(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<CustomResponseModel> UpdateSourceRegion(JObject responseString)
        {
            SourceRegionPricingRequestModel requestModel = JsonConvert.DeserializeObject<SourceRegionPricingRequestModel>(responseString.ToString());
            CustomResponseModel response;
            response = await _pricingReqDomain.UpdateSourceRegion(requestModel);
            return response;
        }
        [ValidateToken]
        [HttpPost]
        public async Task<List<int>> GetPriceDetailIdsBySourceAsync(RequestPriceBySourceInputViewModel inputModel)
        {

            List<int> response = await _pricingReqDomain.GetPriceDetailIdsBySourceAsync(inputModel);
            return response;
        }

        //[Cache(TimeDuration = 300)]
        [ValidateToken]
        [HttpPost]
        public async Task<PricingCodesResponseModel> GetPricingCodesAsync(PricingCodesRequestModel requestModel)
        {
            var response = await _pricingReqDomain.GetPricingCodesAsync(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<PricingRequestDetailResponseModel> GetPricingRequestDetailByIdAsync(PricingRequestViewModel requestModel)
        {
            var response = await _pricingReqDomain.GetPricingRequestDetailByIdAsync(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<IntResponseModel> GetFilterPriceDetailsByPricingType(FilterPricingRequestViewModel requestModel)
        {
            var response = await _pricingReqDomain.GetFilterPriceDetailsByPricingType(requestModel);
            return response;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<PricingDetailResponseModelForExchangeAPI> GetPricingDetailsByIdList(List<int> requestPriceDetailIds)
        {
            var response = await _pricingReqDomain.GetPricingDetailsByIdList(requestPriceDetailIds);
            return response;
        }
    }
}
