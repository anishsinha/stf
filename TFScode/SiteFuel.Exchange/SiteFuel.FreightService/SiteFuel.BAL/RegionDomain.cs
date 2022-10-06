using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Dispatcher;
using SiteFuel.FreightRepository;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class RegionDomain : IRegionDomain
    {
        private IRegionRepository _regionRepository;
        public RegionDomain(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<bool> DeleteAllRegions()
        {
            var response = false;
            try
            {
                response = await _regionRepository.DeleteAllRegions();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "DeleteAllRegions", ex.Message, ex);
            }
            return response;
        }

        public async Task<RegionResponseModel> CreateRegion(RegionViewModel model)
        {
            var response = new RegionResponseModel();
            try
            {
                var valResult = ValidateRegionModel(model);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _regionRepository.CreateRegion(model);
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? string.IsNullOrWhiteSpace(response.StatusMessage) ? "Success" : response.StatusMessage : string.IsNullOrWhiteSpace(response.StatusMessage) ? "Failed" : response.StatusMessage;
                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("RegionDomain", "CreateRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<RegionResponseModel> UpdateRegion(RegionViewModel model)
        {
            var response = new RegionResponseModel();
            try
            {
                var valResult = ValidateRegionModel(model);
                response.StatusMessage = valResult.Message;
                if (valResult.IsValid)
                {
                    response = await _regionRepository.UpdateRegion(model);
                    response.StatusMessage = response.StatusCode == (int)Status.Success ? string.IsNullOrWhiteSpace(response.StatusMessage) ? "Success" : response.StatusMessage : string.IsNullOrWhiteSpace(response.StatusMessage) ? "Failed" : response.StatusMessage;

                }
                else
                {
                    response.StatusCode = (int)Status.Failed;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("RegionDomain", "UpdateRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<string> GetRegionName(string regionId)
        {
            string response = null;
            try
            {
                response = await _regionRepository.GetRegionName(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegionName", ex.Message, ex);
            }
            return response;
        }

        public async Task<RegionViewModel> GetRegion(int companyId, string regionId)
        {
            RegionViewModel response = null;
            try
            {
                response = await _regionRepository.GetRegion(companyId, regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegion", ex.Message, ex);
            }
            return response;
        }

        public List<RegionViewModel> GetRegions(int companyId)
        {
            var response = new List<RegionViewModel>();
            try
            {
                response = _regionRepository.GetRegions(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegions", ex.Message, ex);
            }
            return response;
        }

        private ValidatationResult ValidateRegionModel(RegionViewModel model)
        {
            var result = new ValidatationResult() { IsValid = true };
            try
            {
                var messages = new List<string>();
                if (string.IsNullOrWhiteSpace(model.Name))
                    messages.Add("Region name required");

                if (model.SlotPeriod <= 0)
                    messages.Add("Slot period required");

                if (model.CompanyId <= 0)
                    messages.Add("Invalid CompanyId");

                if (!model.States.Any())
                    messages.Add("Please select state(s) to create region");

                if (model.CreatedOn == DateTimeOffset.MinValue)
                    messages.Add("Invalid CreatedOn");

                if (model.CreatedBy <= 0)
                    messages.Add("Invalid CreatedBy");

                var isJobExists = _regionRepository.IsJobExistsInOtherRegion(model);
                if (isJobExists)
                {
                    messages.Add("Selected job(s) already mapped to other region.");
                }
                List<string> isDriverExists = _regionRepository.IsDriverAlreadyExists(model);
                if (isDriverExists.Any())
                {
                    messages.Add("Selected driver(s) already mapped to other region. " + string.Join(",", isDriverExists));
                }
                if (messages.Any())
                {
                    result.IsValid = false;
                    result.Message = string.Join(", ", messages);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "ValidateRegionModel", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusModel> DeleteRegion(string regionId, int deletedBy)
        {
            var response = new StatusModel();
            try
            {
                response = await _regionRepository.DeleteRegion(regionId, deletedBy);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "DeleteRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DispatcherDashboardRegionModel>> GetDispatcherDashboardRegions(int companyId, int dispatcherId)
        {
            var response = new List<DispatcherDashboardRegionModel>();
            try
            {
                response = await _regionRepository.GetDispatcherDashboardRegions(companyId, dispatcherId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDispatcherDashboardRegions", ex.Message, ex);
            }
            return response;
        }

       

        public async Task<StatusModel> AssignTPOJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            var response = new StatusModel();
            try
            {
                response = await _regionRepository.AssignTPOJobToRegion(jobToUpdate);
            }
            catch(Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "AssignTPOJobToRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<string> GetRegionIdForJob(int jobId, int companyId)
        {
            string response = string.Empty;
            try
            {
                response=  await _regionRepository.GetRegionIdForJob(jobId, companyId);
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("RegionRepository", "GetRegionIdForJob", ex.Message, ex);
            }
            return response;

        }

        public async Task<List<QuickEntryDRModels>> GetJobsForAllRegions(int companyId)
        {
            List<QuickEntryDRModels> response = new List<QuickEntryDRModels>();
            try
            {
                response = await _regionRepository.GetJobsForAllRegions(companyId);
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("RegionRepository", "GetJobsForAllRegions", ex.Message, ex);
            }
            return response;

        }

        public List<int> GetDriverDetailsByCompanyId(int companyId, int dispatcherId, string regionID)
        {
            var response = new List<int>();
            try
            {
                response = _regionRepository.GetDriverDetailsByCompanyId(companyId, dispatcherId, regionID);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDriverDetailsByCompanyId", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetRegionsDdl(int companyId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = _regionRepository.GetRegionsDdl(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegionsDdl", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetCarriersAssignedToRegion(string regionId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = _regionRepository.GetCarriersAssignedToRegion(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetCarriersAssignedToRegion", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayExtendedItem> GetDispatchersAssignedToRegion(List<string> regionId)
        {
            var response = new List<DropdownDisplayExtendedItem>();
            try
            {
                response = _regionRepository.GetDispatchersAssignedToRegion(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDispatchersAssignedToRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetJobsAssignedToRegions(int companyId)
        {
            var response = new List<int>();
            try
            {
                response = await _regionRepository.GetJobsAssignedToRegions(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDriverDetailsByCompanyId", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetJobsAssignedToDriver(int driverId)
        {
            var response = new List<int>();
            try
            {
                response = await _regionRepository.GetJobsAssignedToDriver(driverId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetJobsAssignedToDriver", ex.Message, ex);
            }
            return response;

        }

        public async Task<List<string>> GetDispatcherRegionIds(int companyId, int dispatcherId)
        {
            var response = new List<string>();
            try
            {
                response = await _regionRepository.GetDispatcherRegionIds(companyId, dispatcherId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetDispatcherRegionIds", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RegionJobsModel>> GetRegionsForJobs(RegionInputModel request)
        {
            var response = new List<RegionJobsModel>();
            try
            {
                response =  await _regionRepository.GetRegionsForJobs(request);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegionsForJobs", ex.Message, ex);
            }
            return response;
        }
        public List<TfxCarrierDropdownDisplayItem> GetRegionCarriers(string regionId)
        {
            var response = new List<TfxCarrierDropdownDisplayItem>();
            try
            {
                response = _regionRepository.GetRegionCarriers(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegionCarriers", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> AddRegionSchedule(RegionScheduleModel model)
        {
            var response = new StatusModel();
            try
            {
                response=  await _regionRepository.AddRegionSchedule(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "AddRegionSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<RegionScheduleModel>> getRegionShiftSchedule(string regionId, string routeId)
        {
            var response = new List<RegionScheduleModel>();
            try
            {
                response = await _regionRepository.GetRegionShiftSchedule(regionId, routeId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "getRegionShiftSchedule", ex.Message, ex);
            }
            return response;
        }

        public List<RegionScheduleMappingViewModel> getRegionShiftSchedule(string regionId, int scheduleType)
        {
            var response = new List<RegionScheduleMappingViewModel>();
            try
            {
                response = _regionRepository.GetSchedulesByRegion(regionId, scheduleType);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "getRegionShiftSchedule", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<CarrierRegionModel>> GetAllCarrierRegions(List<DropdownDisplayExtendedItem> carriers)
        {
            var response = new List<CarrierRegionModel>();
            try
            {
                response = await _regionRepository.GetAllCarrierRegions(carriers);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetCarriersRegions", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> RemoveDriverFromRegion(RegionDriverRemoveModel model)
        {
            var response = new StatusModel();
            try
            {
                response = await _regionRepository.RemoveDriverFromRegion(model);

            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("RegionDomain", "RemoveDriverFromRegion", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<InvitedDriverResponseModel>> CheckInvitedDriverScheduleExists(List<RegionDriverRemoveModel> model)
        {
            var response = new List<InvitedDriverResponseModel>();
            try
            {
                response = await _regionRepository.CheckInvitedDriverScheduleExists(model);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "CheckInvitedDriverScheduleExists", ex.Message, ex);
            }
            return response;
        }
        public async Task<RegionFavProductModel> GetRegionFavouriteProducts(int? jobId, string regionId, int companyId)
        {
            var response = new RegionFavProductModel();
            try
            {
                response = await _regionRepository.GetRegionFavouriteProducts(jobId, regionId, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "GetRegionFavouriteProducts", ex.Message, ex);
            }
            return response;
        }

        public bool IsPublishedDR(int companyId, string productIds, string orderIds)
        {
            bool response = false;
            try
            {
                response = _regionRepository.IsPublishedDR(companyId, productIds, orderIds);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("RegionDomain", "IsPublishedDR", ex.Message, ex);
            }
            return response;
        }
    }
}
