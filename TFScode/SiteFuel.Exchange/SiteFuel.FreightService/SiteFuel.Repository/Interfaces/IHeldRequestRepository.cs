using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IHeldRequestRepository
    {
        Task<HeldDeliveryRequestsModel> CreateHeldDeliveryRequests(List<HeldDeliveryRequestModel> model);
        Task<long> GetHeldDeliveryRequestCount(int companyId);
        Task<HeldDeliveryRequestModel> GetHeldDeliveryRequestById(string id);
        Task<List<HeldDeliveryRequestModel>> GetHeldDeliveryRequests(int companyId);
        Task<HeldDeliveryRequestsModel> DeleteHeldDr(string id, int userId);
        Task<StatusModel> UpdateHeldDrStatus(string id);
        Task<HeldDeliveryRequestModel> EditHeldDeliveryRequest(HeldDeliveryRequestModel model);
        Task<HeldDeliveryRequestModel> UpdateHeldDrCreditCheckStatus(SalesOrderStatusModel viewModel);
        Task<StatusModel> UpdateHeldDrValidationStatus(string id, string message);
        Task<HeldDeliveryRequestModel> OverrideCreditCheckApproval(OverrideCreditCheckApprovalModel viewModel);
    }
}
