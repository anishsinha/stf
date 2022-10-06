using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace TrueFill.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
        [ApiExplorerSettings(IgnoreApi = true)]
#endif

    public class HeldRequestController : ApiController
    {
        private readonly IHeldRequestDomain _requestDomain;

        public HeldRequestController(IHeldRequestDomain requestDomain)
        {
            _requestDomain = requestDomain;
        }

        [HttpPost]
        public async Task<HeldDeliveryRequestsModel> CreateHeldDeliveryRequests(List<HeldDeliveryRequestModel> deliveryRequests)
        {
            using (var tracer = new Tracer("HeldRequestController", "CreateHeldDeliveryRequests"))
            {
                var response = await _requestDomain.CreateHeldDeliveryRequests(deliveryRequests);
                return response;
            }
        }

        [HttpGet]
        public async Task<HeldDeliveryRequestModel> GetHeldDeliveryRequestById(string id)
        {
            using (var tracer = new Tracer("HeldRequestController", "GetHeldDeliveryRequestById"))
            {
                var response = await _requestDomain.GetHeldDeliveryRequestById(id);
                return response;
            }
        }

        [HttpPost]
        public async Task<HeldDeliveryRequestModel> OverrideCreditCheckApproval(OverrideCreditCheckApprovalModel viewModel)
        {
            return await _requestDomain.OverrideCreditCheckApproval(viewModel);
        }

        [HttpPost]
        public async Task<HeldDeliveryRequestModel> UpdateHeldDrCreditCheckStatus(SalesOrderStatusModel viewModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::HeldRequestController", $"GetHeldDeliveryRequests(viewModel:{JsonConvert.SerializeObject(viewModel)})"))
            {
                var response = await _requestDomain.UpdateHeldDrCreditCheckStatus(viewModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<HeldDeliveryRequestModel> EditHeldDeliveryRequest(HeldDeliveryRequestModel model)
        {
            using (var tracer = new Tracer("HeldRequestController", $"EditHeldDeliveryRequest(model:{model})"))
            {
                var response = await _requestDomain.EditHeldDeliveryRequest(model);
                return response;
            }
        }

        [HttpGet]
        public async Task<long> GetHeldDeliveryRequestCount(int companyId)
        {
            return await _requestDomain.GetHeldDeliveryRequestCount(companyId);
        }

        [HttpGet]
        public async Task<List<HeldDeliveryRequestModel>> GetHeldDeliveryRequests(int companyId)
        {
            using (var tracer = new Tracer("HeldRequestController", "GetHeldDeliveryRequests"))
            {
                var response = await _requestDomain.GetHeldDeliveryRequests(companyId);
                return response;
            }
        }

        [HttpGet]
        public async Task<HeldDeliveryRequestsModel> DeleteHeldDr(string id, int userId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::HeldRequestController", $"DeleteHeldDr(id:{id})"))
            {
                var response = await _requestDomain.DeleteHeldDr(id, userId);
                return response;
            }
        }

        [HttpGet]
        public async Task<StatusModel> UpdateHeldDrStatus(string id)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::HeldRequestController", $"UpdateHeldDrStatus(id:{id})"))
            {
                var response = await _requestDomain.UpdateHeldDrStatus(id);
                return response;
            }
        }

        [HttpGet]
        public async Task<StatusModel> UpdateHeldDrValidation(string id, string message)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::HeldRequestController", $"UpdateHeldDrValidation(id:{id})"))
            {
                var response = await _requestDomain.UpdateHeldDrValidationStatus(id, message);
                return response;
            }
        }
    }
}
