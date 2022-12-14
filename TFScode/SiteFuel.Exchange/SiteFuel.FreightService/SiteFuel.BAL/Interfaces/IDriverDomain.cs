using SiteFuel.Exchange.ViewModels;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public interface IDriverDomain
    {
        Task<StatusModel> CreateDriver(DriverObjectModel model);
        Task<StatusModel> UpdateDriver(DriverObjectModel model);
        Task<StatusModel> DeleteDriver(int driverId, int companyId);
        Task<DriverObjectModel> GetDriver(int driverId, int companyId);
        Task<DriverObjectModel> GetDriverById(int driverId);
        Task<DriverAdditionalDetailsModel> GetDriverAdditionalDetails(int tfxDriverId);
        Task<StatusModel> AddDriverSchedule(DriverScheduleMappingViewModel model);
        Task<StatusModel> AddTrailerSchedule(TrailerScheduleModel model);
        Task<List<DriverObjectModel>> GetAllDrivers(int companyId);
        Task<DriverScheduleUpdateModel> UpdateDriverSchedule(List<DriverScheduleMappingViewModel> model);
        List<DriverScheduleMappingViewModel> GetShiftByDrivers(string driverList, int scheduleType);        
        Task<DriverScheduleUpdateModel> DeleteAllSchedulesOfDriver(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels);
        Task<List<TrailerRetainDetails>> getTrailerFuelDetails(RetainRequets retainRequets);
    }
}
