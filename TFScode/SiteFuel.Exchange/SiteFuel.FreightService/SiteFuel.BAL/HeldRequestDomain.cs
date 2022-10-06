using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class HeldRequestDomain : IHeldRequestDomain
    {
        private IHeldRequestRepository _drRepository;
        public HeldRequestDomain(IHeldRequestRepository drRepository)
        {
            _drRepository = drRepository;
        }

        public async Task<HeldDeliveryRequestsModel> CreateHeldDeliveryRequests(List<HeldDeliveryRequestModel> deliveryRequests)
        {
            var response = new HeldDeliveryRequestsModel();
            try
            {
                response.StatusCode = (int)Status.Success;
                deliveryRequests = deliveryRequests.Where(top => !top.isRecurringSchedule).ToList();
                var valResult = ValidateRegionModel(deliveryRequests);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    if (deliveryRequests.Count > 0)
                    {
                        response = await _drRepository.CreateHeldDeliveryRequests(deliveryRequests);
                    }
                    else
                    {
                        response.StatusCode = (int)Status.Warning;
                    }
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                    response.StatusMessage = valResult.Message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.msgDelReqCreationFailed;
                LogManager.Logger.WriteException("HeldRequestDomain", "CreateHeldDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> EditHeldDeliveryRequest(HeldDeliveryRequestModel model)
        {
            var response = new HeldDeliveryRequestModel();
            try
            {
                response = await _drRepository.EditHeldDeliveryRequest(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "EditHeldDeliveryRequest", ex.Message, ex);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> OverrideCreditCheckApproval(OverrideCreditCheckApprovalModel viewModel)
        {
            var response = new HeldDeliveryRequestModel();
            try
            {
                response = await _drRepository.OverrideCreditCheckApproval(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "OverrideCreditCheckApproval", ex.Message, ex);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> UpdateHeldDrCreditCheckStatus(SalesOrderStatusModel viewModel)
        {
            var response = new HeldDeliveryRequestModel();
            try
            {
                response = await _drRepository.UpdateHeldDrCreditCheckStatus(viewModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "UpdateHeldDrCreditCheckStatus", ex.Message + "viewModel:" + JsonConvert.SerializeObject(viewModel), ex);
            }
            return response;
        }

        public async Task<long> GetHeldDeliveryRequestCount(int companyId)
        {
            long count = 0;
            try
            {
                count = await _drRepository.GetHeldDeliveryRequestCount(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "GetHeldDeliveryRequestCount", ex.Message, ex);
            }
            return count;
        }

        public async Task<List<HeldDeliveryRequestModel>> GetHeldDeliveryRequests(int companyId)
        {
            var response = new List<HeldDeliveryRequestModel>();
            try
            {
                response = await _drRepository.GetHeldDeliveryRequests(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "GetHeldDeliveryRequests", ex.Message, ex);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestModel> GetHeldDeliveryRequestById(string id)
        {
            var response = new HeldDeliveryRequestModel();
            try
            {
                response = await _drRepository.GetHeldDeliveryRequestById(id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "GetHeldDeliveryRequestById", ex.Message, ex);
            }
            return response;
        }

        public async Task<HeldDeliveryRequestsModel> DeleteHeldDr(string id, int userId)
        {
            var response = new HeldDeliveryRequestsModel();
            try
            {
                response = await _drRepository.DeleteHeldDr(id, userId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "DeleteHeldDr", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateHeldDrStatus(string id)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.UpdateHeldDrStatus(id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "UpdateHeldDrStatus", ex.Message + "id:" + id, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateHeldDrValidationStatus(string id, string message)
        {
            var response = new StatusModel();
            try
            {
                response = await _drRepository.UpdateHeldDrValidationStatus(id, message);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("HeldRequestDomain", "UpdateHeldDrValidationStatus", ex.Message + "id:" + id, ex);
            }
            return response;
        }

        private ValidatationResult ValidateRegionModel(List<HeldDeliveryRequestModel> deliveryRequests)
        {
            var result = new ValidatationResult() { IsValid = true };
            var messages = new List<string>();
            foreach (var model in deliveryRequests)
            {
                if (model.CreatedByCompanyId <= 0)
                    messages.Add("CreatedByCompanyId");

                if (model.CreatedOn == DateTimeOffset.MinValue)
                    messages.Add("CreatedOn");

                if (model.CreatedBy <= 0)
                    messages.Add("CreatedBy");

                if (model.DeliveryRequestFor == DeliveryRequestFor.ProductType)
                {
                    if (model.ProductTypeId <= 0)
                        messages.Add("ProductTypeId");
                }
                else if (model.OrderId == null)
                {
                    if (!model.IsTBD)
                    {
                        if (string.IsNullOrWhiteSpace(model.TankId))
                            messages.Add("TankId");

                        if (string.IsNullOrWhiteSpace(model.StorageId))
                            messages.Add("StorageId");
                    }
                }

                if (model.SupplierCompanyId <= 0)
                    messages.Add("SupplierCompanyId");
            }

            if (messages.Any())
            {
                result.IsValid = false;
                result.Message = "Invalid " + string.Join(", ", messages);
            }
            return result;
        }

    }
}
