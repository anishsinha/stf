using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Driver;
using SiteFuel.FreightRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SiteFuel.BAL
{
    public class DriverDomain : IDriverDomain
    {
        private IDriverRepository _driverRepository;
        public DriverDomain(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<StatusModel> CreateDriver(DriverObjectModel model)
        {
            var response = await _driverRepository.CreateDriver(model);
            return response;
        }

        public async Task<StatusModel> UpdateDriver(DriverObjectModel model)
        {
            var response = new StatusModel();
            try
            {
                response = await _driverRepository.UpdateDriver(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "UpdateDriver", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> DeleteDriver(int driverId, int companyId)
        {
            var response = new StatusModel();
            try
            {
                response = await _driverRepository.DeleteDriver(driverId, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "DeleteDriver", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverObjectModel> GetDriver(int driverId, int companyId)
        {
            DriverObjectModel response = null;
            try
            {
                response = await _driverRepository.GetDriver(driverId, companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "GetDriver", ex.Message, ex);
            }
            return response;
        }
        public async Task<DriverObjectModel> GetDriverById(int driverId)
        {
            DriverObjectModel response = null;
            try
            {
                response = await _driverRepository.GetDriverById(driverId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "GetDriverById", ex.Message, ex);
            }
            return response;
        }

        public async Task<DriverAdditionalDetailsModel> GetDriverAdditionalDetails(int tfxdriverId)
        {
            DriverAdditionalDetailsModel response = null;
            try
            {
                response = await _driverRepository.GetDriverAdditionalDetails(tfxdriverId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "GetDriverAdditionalDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TrailerRetainDetails>> getTrailerFuelDetails(RetainRequets retainRequets)
        {
            List<TrailerRetainDetails> response = null;
            try
            {
                response = await _driverRepository.GetTrailerFuelReatinDetails(retainRequets);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "getTrailerFuelDetails", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusModel> AddDriverSchedule(DriverScheduleMappingViewModel model)
        {
            var response = await _driverRepository.AddDriverSchedule(model);
            return response;
        }

        public async Task<StatusModel> AddTrailerSchedule(TrailerScheduleModel model)
        {
            var response = await _driverRepository.AddTrailerSchedule(model);
            return response;
        }

        public async Task<List<DriverObjectModel>> GetAllDrivers(int companyId)
        {
            List<DriverObjectModel> response = null;
            try
            {
                response = await _driverRepository.GetAllDrivers(companyId);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "GetAllDrivers", ex.Message, ex);
            }
            return response;
        }


        public async Task<DriverScheduleUpdateModel> UpdateDriverSchedule(List<DriverScheduleMappingViewModel> model)
        {
            var response = new DriverScheduleUpdateModel();
            try
            {
                response = await _driverRepository.UpdateDriverSchedule(model);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "UpdateDriverSchedule", ex.Message, ex);
            }
            return response;
        }
        public async Task<DriverScheduleUpdateModel> DeleteAllSchedulesOfDriver(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels)
        {
            var response = new DriverScheduleUpdateModel();
            try
            {
                response = await _driverRepository.DeleteAllSchedulesOfDriver(driverScheduleMappingViewModels);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "DeleteAllSchedulesOfDriver", ex.Message, ex);
            }
            return response;
        }     

        public List<DriverScheduleMappingViewModel> GetShiftByDrivers(string driverList,int scheduleType)
        {
            var response = new List<DriverScheduleMappingViewModel>();
            try
            {
                response = _driverRepository.GetShiftByDrivers(driverList, scheduleType);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DriverDomain", "GetShiftByDrivers", ex.Message, ex);
            }
            return response;
        }
    }
}
