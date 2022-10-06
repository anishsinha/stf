using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class CarrierDomain : ICarrierDomain
    {
        private ICarrierRepository _carrierRepository;
        public CarrierDomain(ICarrierRepository carrierRepository)
        {
            _carrierRepository = carrierRepository;
        }

        public List<CarrierViewModel> GetSupplierCarriers(int companyId, int carrierCompanyId)
        {
            var response = new List<CarrierViewModel>();
            try
            {
                response = _carrierRepository.GetSupplierCarriers(companyId, carrierCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetSupplierCarriers", ex.Message, ex);
            }
            return response;
        }
        public CarrierJobDetailsViewModel GetCarrierUsers(int companyId)
        {
            var response = new CarrierJobDetailsViewModel();
            try
            {
                response = _carrierRepository.GetCarrierUsers(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetCarrierUsers", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> AssignToSupplier(List<CarrierViewModel> supplierCarriers)
        {
            var response = new StatusModel();
            try
            {
                var valResult = ValidateAssignCarrierModel(supplierCarriers);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _carrierRepository.AssignToSupplier(supplierCarriers);
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? "Success" : "Failed";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "AssignToSupplier", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> UpdateAssignedCarriers(CarrierViewModel carrier)
        {
            var response = new StatusModel();
            try
            {
                var valResult = ValidateCarrierModel(carrier);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _carrierRepository.UpdateAssignedCarriers(carrier);
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? "Success" : "Failed";
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = "error occurred";
                LogManager.Logger.WriteException("CarrierDomain", "UpdateAssignedCarriers", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> DeleteAssignedCarriers(CarrierViewModel carrier)
        {
            var response = new StatusModel();
            try
            {
                var valResult = ValidateCarrierModel(carrier);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _carrierRepository.DeleteAssignedCarriers(carrier);
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? "Success" : "Failed";
                }
            }
            catch (Exception ex)
            {
                response.StatusMessage = "error occurred";
                LogManager.Logger.WriteException("CarrierDomain", "DeleteAssignedCarriers", ex.Message, ex);
            }
            return response;
        }

        private ValidatationResult ValidateAssignCarrierModel(List<CarrierViewModel> model)
        {
            var result = new ValidatationResult() { IsValid = true };
            var messages = new List<string>();

            if (model.Count == 0)
                messages.Add("atlease one carrier is required");

            if (model.Any(t => t.Carrier == null))
                messages.Add("Carrier is required");

            if (model.Any(t => t.CreatedOn == DateTimeOffset.MinValue))
                messages.Add("CreatedOn");

            if (model.Any(t => t.CreatedBy <= 0))
                messages.Add("CreatedBy");

            if (messages.Any())
            {
                result.IsValid = false;
                result.Message = "Invalid " + string.Join(", ", messages);
            }
            return result;
        }

        private ValidatationResult ValidateCarrierModel(CarrierViewModel model)
        {
            var result = new ValidatationResult() { IsValid = true };
            var messages = new List<string>();

            if (model == null)
                messages.Add("carrier is required");

            if (model != null && model.Carrier.Id == 0)
                messages.Add("Carrier Code is required");

            if (model.CreatedOn == DateTimeOffset.MinValue)
                messages.Add("CreatedOn");

            if (model.CreatedBy <= 0)
                messages.Add("CreatedBy");

            if (messages.Any())
            {
                result.IsValid = false;
                result.Message = "Invalid " + string.Join(", ", messages);
            }
            return result;
        }

        public async Task<StatusModel> AssignCarrierToJob(CarrierViewModel model)
        {
            var response = new StatusModel();
            try
            {
                var valResult = ValidateCarrierModel(model);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _carrierRepository.AssignCarrierToJob(model);
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? "Success" : "Failed";
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "AssignCarrierToJob", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<int>> GetCarriersJobs(int carrierCompanyId, int customerCompanyId)
        {
            var response = new List<int>();
            try
            {
                response = await _carrierRepository.GetCarriersJobs(carrierCompanyId, customerCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetCarriersJobs", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DipTestRequestModel>> GetCarrierDetailsByJob(List<int> jobIds)
        {
            var response = new List<DipTestRequestModel>();
            try
            {
                response = await _carrierRepository.GetCarrierDetailsByJob(jobIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CarrierDomain", "GetCarrierDetailsByJob", ex.Message, ex);
            }
            return response;
        }
    }
}
