using SiteFuel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IPricingRequestDomain
    {
        Task<CustomResponseModel> SaveRequestDetails(PricingRequestViewModel pricingrequestviewmodel);
        Task<CustomResponseModel> UpdateRequestDetails(PricingRequestViewModel pricingrequestviewmodel);
        Task<PricingCodesResponseModel> GetPricingCodesAsync(PricingCodesRequestModel requestModel);

        Task<PricingRequestDetailResponseModel> GetPricingRequestDetailByIdAsync(PricingRequestViewModel requestModel);
        Task<List<int>> GetPriceDetailIdsBySourceAsync(RequestPriceBySourceInputViewModel inputModel);
        Task<IntResponseModel> GetFilterPriceDetailsByPricingType(FilterPricingRequestViewModel requestModel);
        Task<PricingDetailResponseModelForExchangeAPI> GetPricingDetailsByIdList(List<int> requestPriceDetailIds);
        Task<CustomResponseModel> UpdateSourceRegion(SourceRegionPricingRequestModel model);
    }
}
