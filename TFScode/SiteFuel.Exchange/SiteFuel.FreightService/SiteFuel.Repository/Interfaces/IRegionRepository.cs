using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Dispatcher;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.FreightRepository
{
    public interface IRegionRepository
    {
        Task<bool> DeleteAllRegions();
        Task<RegionResponseModel> CreateRegion(RegionViewModel model);
        Task<RegionResponseModel> UpdateRegion(RegionViewModel model);
        Task<string> GetRegionName(string regionId);
        Task<RegionViewModel> GetRegion(int companyId, string regionId);
        List<RegionViewModel> GetRegions(int companyId);
        Task<StatusModel> DeleteRegion(string regionId, int deletedBy);
        bool IsJobExistsInOtherRegion(RegionViewModel model);
        Task<List<DispatcherDashboardRegionModel>> GetDispatcherDashboardRegions(int companyId, int dispatcherId);   
        List<int> GetDriverDetailsByCompanyId(int companyId, int dispatcherId,string regionID);
        List<string> IsDriverAlreadyExists(RegionViewModel model);
        Task<StatusModel> AssignTPOJobToRegion(JobToRegionAssignViewModel jobToUpdate);
        Task<string> GetRegionIdForJob(int jobId, int companyId);
        Task<List<QuickEntryDRModels>> GetJobsForAllRegions(int companyId);
        List<DropdownDisplayExtendedItem> GetRegionsDdl(int companyId);
        List<DropdownDisplayExtendedItem> GetCarriersAssignedToRegion(string regionId);
        List<DropdownDisplayExtendedItem> GetDispatchersAssignedToRegion(List<string> regionId);
        Task<List<int>> GetJobsAssignedToRegions(int companyId);
        Task<List<DropdownDisplayExtendedItem>> GetAllDriversInRegion(string regionId);
        Task<StatusModel> UpdateDriverAssignmentForRegion(List<SiteFuel.MdbDataAccess.Collections.DropdownDisplayItem> driversList, string regionId, int companyId);
        Task<List<RegionJobsModel>> GetRegionsForJobs(RegionInputModel request);
        Task<List<int>> GetJobsAssignedToDriver(int driverId);
        Task<List<string>> GetDispatcherRegionIds(int companyId, int dispatcherId);
        List<TfxCarrierDropdownDisplayItem> GetRegionCarriers(string regionId);
        Task<StatusModel> AddRegionSchedule(RegionScheduleModel model);
        Task<List<RegionScheduleModel>> GetRegionShiftSchedule(string regionId, string routeId);
        List<RegionScheduleMappingViewModel> GetSchedulesByRegion(string regionId, int scheduleType);
        Task<List<CarrierRegionModel>> GetAllCarrierRegions(List<DropdownDisplayExtendedItem> carriers);
        Task<StatusModel> RemoveDriverFromRegion(RegionDriverRemoveModel model);
        Task<List<InvitedDriverResponseModel>> CheckInvitedDriverScheduleExists(List<RegionDriverRemoveModel> model);
        Task<RegionFavProductModel> GetRegionFavouriteProducts(int? jobId, string regionId, int companyId);
        bool IsPublishedDR(int companyId, string entityId, string orderIds);
    }
}
