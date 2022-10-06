using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IScheduleBuilderDomain
    {
        Task<List<DropdownDisplayExtended>> GetRegions(int userId);
        Task<RegionDetailModel> GetRegionDetails(string regionId);
        Task<List<int>> GetCarrierSuppliersBySiteId(int jobId, int carrierCompanyId);
        Task<ScheduleBuilderViewModel> GetScheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, string scheduleBuilderId, int sbDsbView, bool IsDsbDriverSchedule);
        Task<List<DSBSaveModel>> SaveScheduleBuilder(List<DSBSaveModel> scheduleBuilders);
        Task<StatusModel> CheckAndLockDrs(LockDrModel model);
        Task<StatusModel> CheckAndReleaseDrs(LockDrModel model);
        Task<ScheduleBuilderResponseModel> IsValidTimeStamp(string selectedDate, string regionId, int companyId, long lastTimeStamp);
        Task<List<DSBSaveModel>> DeleteTrip(List<DSBSaveModel> scheduleBuilders);
        //Task<DSBSaveModel> UpdateDeletedRequests(DSBSaveModel scheduleBuilder);
        Task<DSBSaveModel> AssignDriverAndTrailer(DSBSaveModel scheduleBuilder);
        Task<List<Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId, List<string> trailerTypeId, string regionId, DateTimeOffset selectedDate);
        Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetails(int companyId, List<string> trailerTypeId, string regionId, DateTimeOffset selectedDate);
        Task<StatusModel> CancelSchedules(CancelScheduleModel model);
        Task<DSBSaveModel> IsValidScheduleBuilder(DSBSaveModel scheduleBuilder);
        Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, DateTimeOffset selectedDate);
        Task<List<FreightModels.DeliveryRequestViewModel>> ValidateTrailerJobCompatibility(List<TrailerModel> trailers, List<FreightModels.DeliveryRequestViewModel> deliveryRequests);
        Task<List<TrailerJobNonCompatibleDrs>> ValidateTrailerJobCompatibilityForLoadQueue(List<TrailersDeliveryRequestViewModel> models);
        Task<List<ScheduleBuilderViewModel>> GetScheduleBuildersByDrIds(List<string> drIds);
        Task<DSBSaveModel> CreateSchedules(DSBSaveModel scheduleInput);
        Task<DSBSaveModel> GetLoads(CreateScheduleModel scheduleInput);
        Task<UnassignDriverViewModel> UnAssignDriverFromShift(UnassignDriverViewModel removeDriver);
        Task<List<CreateRecurringDRViewModel>> GetRecurringScheduleDetails(string dayOfWeek, int currentDay, string date);
        Task<ScheduleBuilderViewModel> GetScheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, int sbDsbView, string scheduleBuilderId, List<RecurringShiftDetails> recurringShiftDetails, string scheduleBuilderViewId, bool IsBackgroundJobScheduleCreation, bool IsDsbDriverSchedule);
        Task<List<TrailerCompartmentDetail>> GetTrailerCompartmentDetails(List<string> Id);
        Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(List<string> Id);
        Task<DrFilterPreferencesModel> SaveDrFilterPreferences(DrFilterPreferencesModel model);
        Task<DrFilterPreferencesModel> GetDrFilterPreferences(int userId);
        Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetails(int companyId, List<string> trailerTypeId, string regionId, string otherRegion, DateTimeOffset selectedDate, string shiftId, bool IsDsbDriverSchedule = false);
        Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, DateTimeOffset selectedDate, string shiftId);
        Task<ResetDeliveryGroupScheduleModel> RemoveScheduleBuilderDrs(ResetDeliveryGroupScheduleModel model);
        Task<StatusModel> AddOptionalPickup(List<DSBColumnOptionalPickupInfoModel> dSBColumnOptionalPickupInfoModel);
        Task<List<DSBColumnOptionalPickupInfoModel>> GetOptionalPickupDetails(DSBColumnOptionalPickupInfoModel dSBColumnOptionalPickup);
        Task<StatusModel> DeleteOptionalPickupDetails(string Id);
        Task<StatusModel> UpdateDROptionalPickupInfo(List<ScheduleOptionalPickupModel> scheduleOptionalPickups);
        Task<List<PreBOLRetainDeliveryDetailsModel>> GetPreLoadDSRetainInfo(List<PreBOLRetainModel> PreLoadBolDRs);
        Task<List<DSBSaveModel>> CancelScheduleBuilder(List<DSBSaveModel> scheduleBuilders);
        Task<ResetDeliveryGroupScheduleModel> RemoveDeliverySchedule(ResetDeliveryGroupScheduleModel model);
    }
}
