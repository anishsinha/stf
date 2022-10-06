using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ScheduleBuilder;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class ScheduleBuilderDomain : IScheduleBuilderDomain
    {
        private readonly IScheduleBuilderRepository _scheduleBuilderRepository;
        public ScheduleBuilderDomain(IScheduleBuilderRepository scheduleBuilderRepository)
        {
            _scheduleBuilderRepository = scheduleBuilderRepository;
        }

        public async Task<List<DropdownDisplayExtended>> GetRegions(int userId)
        {
            var response = new List<DropdownDisplayExtended>();
            try
            {
                response = await _scheduleBuilderRepository.GetRegions(userId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRegions", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> CheckAndLockDrs(LockDrModel model)
        {
            var response = new StatusModel();
            try
            {
                response = await _scheduleBuilderRepository.CheckAndLockDrs(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CheckAndLockDrs", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> CheckAndReleaseDrs(LockDrModel model)
        {
            var response = new StatusModel();
            try
            {
                response = await _scheduleBuilderRepository.CheckAndReleaseDrs(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CheckAndReleaseDrs", ex.Message, ex);
            }
            return response;
        }

        public async Task<RegionDetailModel> GetRegionDetails(string regionId)
        {
            RegionDetailModel response = null;
            try
            {
                response = await _scheduleBuilderRepository.GetRegionDetails(regionId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetRegionDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<int>> GetCarrierSuppliersBySiteId(int jobId, int carrierCompanyId)
        {
            var response = new List<int>();
            try
            {
                response = await _scheduleBuilderRepository.GetCarrierSuppliersBySiteId(jobId, carrierCompanyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetCarrierSuppliersBySiteId", ex.Message, ex);
            }
            return response;
        }

        public async Task<ScheduleBuilderViewModel> GetScheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, string scheduleBuilderId, int sbDsbView = 2, bool IsDsbDriverSchedule = false)
        {
            ScheduleBuilderViewModel response;
            try
            {
                response = await _scheduleBuilderRepository.GetScheduleBuilderDetails(companyId, userId, regionId, date, sbView, scheduleBuilderId, sbDsbView, IsDsbDriverSchedule);
            }
            catch (Exception ex)
            {
                response = new ScheduleBuilderViewModel();
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetScheduleBuilderDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DSBSaveModel>> SaveScheduleBuilder(List<DSBSaveModel> scheduleBuilders)
        {
            var response = new List<DSBSaveModel>();
            try
            {
                response = await _scheduleBuilderRepository.SaveScheduleBuilder(scheduleBuilders);
            }
            catch (Exception ex)
            {
                response = scheduleBuilders;
                response.ForEach(t => t.StatusCode = (int)Status.Failed);
                response.ForEach(t => t.StatusMessage = Resource.valMessageErrorOccurred);
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "SaveSheduleBuilder", ex.Message, ex);
            }
            return response;
        }

        public async Task<DSBSaveModel> AssignDriverAndTrailer(DSBSaveModel scheduleBuilder)
        {
            var response = new DSBSaveModel();
            try
            {
                response = await _scheduleBuilderRepository.AssignDriverAndTrailer(scheduleBuilder);
            }
            catch (Exception ex)
            {
                response = scheduleBuilder;
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "AssignDriverAndTrailer", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<DSBSaveModel>> DeleteTrip(List<DSBSaveModel> scheduleBuilders)
        {
            var response = new List<DSBSaveModel>();
            try
            {
                response = await _scheduleBuilderRepository.DeleteTrip(scheduleBuilders);
            }
            catch (Exception ex)
            {
                response = scheduleBuilders;
                response.ForEach(t => t.StatusCode = (int)Status.Failed);
                response.ForEach(t => t.StatusMessage = Resource.valMessageErrorOccurred);
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "DeleteTrip", ex.Message, ex);
            }
            return response;
        }

        public async Task<ScheduleBuilderResponseModel> IsValidTimeStamp(string selectedDate, string regionId, int companyId, long lastTimeStamp)
        {
            var response = new ScheduleBuilderResponseModel();
            try
            {
                response = await _scheduleBuilderRepository.IsValidTimeStamp(selectedDate, regionId, companyId, lastTimeStamp);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "IsValidTimeStamp", ex.Message, ex);
            }
            return response;
        }

        //public async Task<DSBSaveModel> UpdateDeletedRequests(DSBSaveModel scheduleBuilder)
        //{
        //    var response = new DSBSaveModel();
        //    try
        //    {
        //        response = await _scheduleBuilderRepository.UpdateDeletedRequests(scheduleBuilder);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.StatusCode = (int)Status.Failed;
        //        LogManager.Logger.WriteException("ScheduleBuilderDomain", "UpdateDeletedRequests", ex.Message, ex);
        //    }
        //    return response;
        //}
        public async Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, List<string> trailerTypeId, string regionId, DateTimeOffset selectedDate)
        {
            var result = new List<Exchange.Utilities.DropdownDisplayItem>();
            try
            {
                result = await _scheduleBuilderRepository.GetAllDrivers(companyId, trailerTypeId, regionId, selectedDate);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetAllDrivers", ex.Message, ex);
            }
            return result;
        }
        public async Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetails(int companyId, List<string> trailerTypeId, string regionId, DateTimeOffset selectedDate)
        {
            var result = new List<DriverAdditionalDetailsViewModel>();
            try
            {
                result = await _scheduleBuilderRepository.GetAllDriverDetailsAsync(companyId, trailerTypeId, regionId, selectedDate);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetAllDriverDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, DateTimeOffset selectedDate)
        {
            var result = new List<DriverScheduleViewModel>();
            try
            {
                result = await _scheduleBuilderRepository.GetSelectedDateDriverScheduleByDriverId(driverId, selectedDate);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetSelectedDateDriverScheduleByDriverId", ex.Message, ex);
            }
            return result;
        }
        public async Task<List<DeliveryRequestViewModel>> ValidateTrailerJobCompatibility(List<TrailerModel> trailers, List<DeliveryRequestViewModel> deliveryRequests)
        {
            var response = new List<DeliveryRequestViewModel>();
            try
            {
                response = await _scheduleBuilderRepository.ValidateTrailerJobCompatibility(trailers, deliveryRequests);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "ValidateTrailerJobCompatibility", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TrailerJobNonCompatibleDrs>> ValidateTrailerJobCompatibilityForLoadQueue(List<TrailersDeliveryRequestViewModel> models)
        {
            var response = new List<TrailerJobNonCompatibleDrs>();
            try
            {
                response = await _scheduleBuilderRepository.ValidateTrailerJobCompatibilityForLoadQueue(models);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "ValidateTrailerJobCompatibility", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> CancelSchedules(CancelScheduleModel model)
        {
            var response = new StatusModel();
            try
            {
                response = await _scheduleBuilderRepository.CancelSchedules(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelSchedules", ex.Message, ex);
            }
            return response;
        }

        public async Task<DSBSaveModel> IsValidScheduleBuilder(DSBSaveModel scheduleBuilder)
        {
            var response = new DSBSaveModel();
            try
            {
                response = await _scheduleBuilderRepository.IsValidScheduleBuilder(scheduleBuilder);
            }
            catch (Exception ex)
            {
                response = scheduleBuilder;
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.valMessageErrorOccurred;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "IsValidScheduleBuilder", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<ScheduleBuilderViewModel>> GetScheduleBuildersByDrIds(List<string> drIds)
        {
            var response = new List<ScheduleBuilderViewModel>();
            response = await _scheduleBuilderRepository.GetScheduleBuildersByDrIds(drIds);
            return response;
        }

        public async Task<DSBSaveModel> GetLoads(CreateScheduleModel scheduleInput)
        {
            var response = new DSBSaveModel();
            try
            {
                response = await _scheduleBuilderRepository.GetLoads(scheduleInput);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                response.StatusMessage = Resource.errMsgProcessRequestFailed;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetLoads", ex.Message, ex);
            }
            return response;
        }

        public async Task<DSBSaveModel> CreateSchedules(DSBSaveModel scheduleInput)
        {
            return await _scheduleBuilderRepository.CreateSchedules(scheduleInput);
        }

        public async Task<UnassignDriverViewModel> UnAssignDriverFromShift(UnassignDriverViewModel removeDriver)
        {
            return await _scheduleBuilderRepository.UnAssignDriverFromShift(removeDriver);
        }

        public async Task<List<CreateRecurringDRViewModel>> GetRecurringScheduleDetails(string dayOfWeek, int currentDay, string date)
        {
            List<CreateRecurringDRViewModel> response = new List<CreateRecurringDRViewModel>();
            try
            {
                response = await _scheduleBuilderRepository.GetRecurringScheduleDetails(dayOfWeek, currentDay, date);

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetReurringScheduleDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<ScheduleBuilderViewModel> GetScheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, string scheduleBuilderId, List<RecurringShiftDetails> recurringShiftDetails, string scheduleBuilderViewId, bool IsBackgroundJobScheduleCreation, bool IsDsbDriverSchedule)
        {
            ScheduleBuilderViewModel response = null;
            try
            {
                response = await _scheduleBuilderRepository.GetScheduleBuilderDetails(companyId, userId, regionId, date, sbView, sbDsbView, scheduleBuilderId, recurringShiftDetails, scheduleBuilderViewId, IsBackgroundJobScheduleCreation, IsDsbDriverSchedule);
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Status.Failed;
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetScheduleBuilderDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<TrailerCompartmentDetail>> GetTrailerCompartmentDetails(List<string> Id)
        {
            List<TrailerCompartmentDetail> response = new List<TrailerCompartmentDetail>();
            try
            {
                response = await _scheduleBuilderRepository.GetTrailerCompartmentDetails(Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetTrailerCompartmentDetails", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(List<string> Id)
        {
            List<TrailerFuelRetainViewModel> response = new List<TrailerFuelRetainViewModel>();
            try
            {
                response = await _scheduleBuilderRepository.GetTrailerFuelRetainDetails(Id);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetTrailerFuelRetainDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<DrFilterPreferencesModel> SaveDrFilterPreferences(DrFilterPreferencesModel model)
        {
            DrFilterPreferencesModel response = new DrFilterPreferencesModel();
            try
            {
                response = await _scheduleBuilderRepository.SaveDrFilterPreferences(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "SaveDrFilterPreferences", ex.Message, ex);
            }
            return response;
        }
        public async Task<DrFilterPreferencesModel> GetDrFilterPreferences(int userId)
        {
            DrFilterPreferencesModel response = new DrFilterPreferencesModel();
            try
            {
                response = await _scheduleBuilderRepository.GetDrFilterPreferences(userId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetDrFilterPreferences", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetails(int companyId, List<string> trailerTypeId, string regionId, string otherRegion, DateTimeOffset selectedDate, string shiftId, bool IsDsbDriverSchedule = false)
        {
            var result = new List<DriverAdditionalDetailsViewModel>();
            try
            {
                result = await _scheduleBuilderRepository.GetAllDriverDetailsAsync(companyId, trailerTypeId, regionId, otherRegion, selectedDate, shiftId, IsDsbDriverSchedule);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetAllDriverDetails", ex.Message, ex);
            }
            return result;
        }
        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, DateTimeOffset selectedDate, string shiftId)
        {
            var result = new List<DriverScheduleViewModel>();
            try
            {
                result = await _scheduleBuilderRepository.GetSelectedDateDriverScheduleByDriverId(driverId, selectedDate, shiftId);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetSelectedDateDriverScheduleByDriverId", ex.Message, ex);
            }
            return result;
        }
        public async Task<ResetDeliveryGroupScheduleModel> RemoveScheduleBuilderDrs(ResetDeliveryGroupScheduleModel model)
        {
            return await _scheduleBuilderRepository.RemoveScheduleBuilderDrs(model);
        }

        public async Task<StatusModel> AddOptionalPickup(List<DSBColumnOptionalPickupInfoModel> dSBColumnOptionalPickupInfoModel)
        {
            var result = new StatusModel();
            try
            {
                result = await _scheduleBuilderRepository.AddOptionalPickup(dSBColumnOptionalPickupInfoModel);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "AddOptionalPickup", ex.Message, ex);
            }
            return result;
        }

        public async Task<List<DSBColumnOptionalPickupInfoModel>> GetOptionalPickupDetails(DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickup)
        {
            var result = new List<DSBColumnOptionalPickupInfoModel>();
            try
            {
                result = await _scheduleBuilderRepository.GetOptionalPickupDetails(dSBColumnOptionalPickup);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetOptionalPickupDetails", ex.Message, ex);
            }
            return result;
        }

        public async Task<StatusModel> DeleteOptionalPickupDetails(string Id)
        {
            var result = new StatusModel();
            try
            {
                result = await _scheduleBuilderRepository.DeleteOptionalPickupDetails(Id);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "DeleteOptionalPickupDetails", ex.Message, ex);
            }
            return result;
        }
        public async Task<StatusModel> UpdateDROptionalPickupInfo(List<ScheduleOptionalPickupModel> scheduleOptionalPickups)
        {
            var result = new StatusModel();
            try
            {
                result = await _scheduleBuilderRepository.UpdateDROptionalPickupInfo(scheduleOptionalPickups);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "UpdateDROptionalPickupInfo", ex.Message, ex);
            }
            return result;
        }
        public async Task<List<PreBOLRetainDeliveryDetailsModel>> GetPreLoadDSRetainInfo(List<PreBOLRetainModel> PreLoadBolDRs)
        {
            var result = new List<PreBOLRetainDeliveryDetailsModel>();
            try
            {
                result = await _scheduleBuilderRepository.GetPreLoadDSRetainInfo(PreLoadBolDRs);
                return result;
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("ScheduleBuilderDomain", "GetPreLoadDSRetainInfo", ex.Message, ex);
            }
            return result;
        }
        public async Task<List<DSBSaveModel>> CancelScheduleBuilder(List<DSBSaveModel> scheduleBuilders)
        {
            var response = new List<DSBSaveModel>();
            try
            {
                response = await _scheduleBuilderRepository.CancelScheduleBuilder(scheduleBuilders);
            }
            catch (Exception ex)
            {
                response = scheduleBuilders;
                response.ForEach(t => t.StatusCode = (int)Status.Failed);
                response.ForEach(t => t.StatusMessage = Resource.valMessageErrorOccurred);
                LogManager.Logger.WriteException("ScheduleBuilderDomain", "CancelScheduleBuilder", ex.Message, ex);
            }
            return response;
        }
        public async Task<ResetDeliveryGroupScheduleModel> RemoveDeliverySchedule(ResetDeliveryGroupScheduleModel model)
        {
            return await _scheduleBuilderRepository.RemoveDeliverySchedule(model);
        }
        
    }
}
