using SiteFuel.DataAccess.Entities;
using SiteFuel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{
    public interface IPricingRequestRepository
    {
        Task<CustomResponseModel> SaveRequestDetails(RequestPriceDetail priceDetail);
        Task<CustomResponseModel> UpdateRequestDetails(RequestPriceDetail priceDetail);
        Task<CustomResponseModel> SavePricingDetails(List<PricingDetail> priceDetails);

        Task<PricingCodesResponseModel> GetPricingCodesAsync(PricingCodesRequestModel requestModel, int timeout = 30);

        Task<MstPricingCode> GetCodeDetails(int codeId);
        Task<RequestPriceDetail> GetPricingRequestDetailByIdAsync(int id);
        Task<PricingRequestDetailResponseModel> GetPricingRequestDetailByIdAsync(PricingRequestViewModel requestModel, int timeout = 30);
        Task<List<int>> GetRequestPriceDetailIdsByPricingSourceAsync(RequestPriceBySourceInputViewModel inputModel);
        Task<IntResponseModel> GetFilterPriceDetailsByPricingType(FilterPricingRequestViewModel requestModel);
        PricingDetailResponseModelForExchangeAPI GetPricingDetailsByIdList(List<int> requestPriceDetailIds, int timeout = 30);
        Task<CustomResponseModel> UpdateSourceRegion(SourceRegionPricingRequestModel model);
    }
}
