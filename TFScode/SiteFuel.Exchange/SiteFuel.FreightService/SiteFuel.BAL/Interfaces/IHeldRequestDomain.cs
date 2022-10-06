using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IHeldRequestDomain
    {
        Task<HeldDeliveryRequestsModel> CreateHeldDeliveryRequests(List<HeldDeliveryRequestModel> deliveryRequests);
        Task<long> GetHeldDeliveryRequestCount(int companyId);
        Task<HeldDeliveryRequestsModel> DeleteHeldDr(string id, int userId);
        Task<StatusModel> UpdateHeldDrStatus(string id);
        Task<HeldDeliveryRequestModel> GetHeldDeliveryRequestById(string id);
        Task<HeldDeliveryRequestModel> EditHeldDeliveryRequest(HeldDeliveryRequestModel model);
        Task<List<HeldDeliveryRequestModel>> GetHeldDeliveryRequests(int companyId);
        Task<HeldDeliveryRequestModel> UpdateHeldDrCreditCheckStatus(SalesOrderStatusModel viewModel);
        Task<StatusModel> UpdateHeldDrValidationStatus(string id, string message);
        Task<HeldDeliveryRequestModel> OverrideCreditCheckApproval(OverrideCreditCheckApprovalModel viewModel);
    }
}
