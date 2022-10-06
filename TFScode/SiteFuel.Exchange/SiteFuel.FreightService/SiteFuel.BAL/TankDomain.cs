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
    public class TankDomain : ITankDomain
    {
        private readonly ITankRepository _tankRepository;
        public TankDomain(ITankRepository tankRepository)
        {
            _tankRepository = tankRepository;
        }

        public async Task<bool> SaveTankDetails(TankDetailsModel model)
        {
            var result = false;
            try
            {
                result = await _tankRepository.SaveTankDetails(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "SaveTankDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<bool> UpdateTankDetails(TankDetailsModel model)
        {
            var result = false;
            try
            {
                result = await _tankRepository.UpdateTankDetails(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "UpdateTankDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<TankDetailsModel> GetTankDetails(int id)
        {
            TankDetailsModel result = null;
            try
            {
                result = await _tankRepository.GetTankDetails(id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetTankDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<TankDetailListModel>> GetTankList(List<int> tanks)
        {
            var result = new List<TankDetailListModel>();
            try
            {
                result = await _tankRepository.GetTankList(tanks);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetTankList", ex.Message, ex);
            }
            return result;
        }

        public async Task<ScheduleOutputModel> GetTankDetailsBySchedule(List<ScheduleInputModel> scheduleInputDetails)
        {
            var result = new ScheduleOutputModel();
            try
            {
                result = await _tankRepository.GetTankDetailsBySchedule(scheduleInputDetails);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetTankDetailsBySchedule", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<JobTankDetailModel>> GetJobTankList(int jobId)
        {
            var result = new List<JobTankDetailModel>();
            try
            {
                result = await _tankRepository.GetJobTankList(jobId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetJobTankList", ex.Message, ex);
            }
            return result;
        }

        public async Task<TankVolumeAndUllageModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel)
        {
            var result = new TankVolumeAndUllageModel();
            try
            {
                result = await _tankRepository.GetTankVolumeAndUllage(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetTankVolumeAndUllage", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DropQuantityByPrePostDipResponseModel>> GetDropQuantityByPrePostDip(List<DropQuantityByPrePostDipRequestModel> requestModel)
        {
            var response = new List<DropQuantityByPrePostDipResponseModel>();
            try
            {
                response = await _tankRepository.GetDropQuantityByPrePostDip(requestModel);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetDropQuantityByPrePostDip", ex.Message, ex);
            }
            return response;
        }

        public async Task<bool> OrderTankMappings(List<OrderTankMappingViewModel> model)
        {
            var result = false;
            try
            {
                result = await _tankRepository.OrderTankMappings(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "OrderTankMappings", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DropdownDisplayExtended>> GetTankModelType(List<int> companyId)
        {
            var response = await _tankRepository.GetTankModelType(companyId);
            return response;
        }

        public async Task<StatusModel> SaveTankTypeDipChart(TankModalTypeViewModel tankTypes)
        {
            var result = await _tankRepository.SaveTankTypeDipChart(tankTypes);
            return result;
        }
        public async Task<List<TankModalTypeViewModel>> GetTankTypesByCompany(int companyId)
        {
            var result = await _tankRepository.GetTankTypesByCompany(companyId);
            return result;
        }
        public async Task<StatusModel> DeleteTankDipChartById(string id)
        {
            var result = await _tankRepository.DeleteTankDipChartById(id);
            return result;
        }
        public async Task<List<string>> GetAllTankTypeNameForDipChart(int companyId, string searchValue)
        {
            var result = await _tankRepository.GetAllTankTypeNameForDipChart(companyId, searchValue);
            return result;
        }
        public async Task<StatusModel> UpdateTankSequence(TankDetailsModel requestModel)
        {
            var result = await _tankRepository.UpdateTankSequence(requestModel);
            return result;
        }

        public async Task<bool> CheckDuplicateTankSequence(TankDetailsModel requestModel)
        {
            var result = await _tankRepository.CheckDuplicateTankSequence(requestModel);
            return result;
        }
        public async Task<List<JobTankAdditionalDetailModel>> GetJobsTankList(List<int> jobId)
        {
            var result = new List<JobTankAdditionalDetailModel>();
            try
            {
                result = await _tankRepository.GetJobsTankList(jobId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TankDomain", "GetJobsTankList", ex.Message, ex);
            }
            return result;
        }
    }
}
