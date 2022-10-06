using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class DriverController : ApiController
    {
        private readonly IDriverDomain _driverDomain;

        public DriverController(IDriverDomain driverDomain)
        {
            _driverDomain = driverDomain;
        }

        [HttpPost]
        public async Task<StatusModel> Create(DriverObjectModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"Create(request:{json})"))
            {
                var response = await _driverDomain.CreateDriver(model);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> Update(DriverObjectModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"Update(request:{json})"))
            {
                var response = await _driverDomain.UpdateDriver(model);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> Delete(int driverId, int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"Delete(driverId:{driverId},companyId:{companyId})"))
            {
                var response = await _driverDomain.DeleteDriver(driverId, companyId);
                return response;
            }
        }

        [HttpGet]
        public async Task<DriverObjectModel> Get(int driverId, int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"Get(driverId:{driverId}, companyId:{companyId})"))
            {
                var response = await _driverDomain.GetDriver(driverId, companyId);
                return response;
            }
        }

        [HttpGet]
        public async Task<DriverObjectModel> GetDriverById(int driverId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"GetDriverById(driverId:{driverId})"))
            {
                var response = await _driverDomain.GetDriverById(driverId);
                return response;
            }
        }
        [HttpGet]
        public async Task<DriverAdditionalDetailsModel> GetGetDriverAdditionalDetails(int driverId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"GetDriverAdditionalDetails(driverId:{driverId})"))
            {
                var response = await _driverDomain.GetDriverAdditionalDetails(driverId);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<TrailerRetainDetails>> GetTrailerRetainDetailsByDriverIds(RetainRequets retainRequets)
        {
            var response = await _driverDomain.getTrailerFuelDetails(retainRequets);
            return response;
        }


        [HttpPost]
        public async Task<StatusModel> AddDriverSchedule(DriverScheduleMappingViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"AddDriverSchedule(request:{json})"))
            {
                model.IsActive = true;
                model.IsDeleted = false;
                var response = await _driverDomain.AddDriverSchedule(model);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> AddTrailerSchedule(TrailerScheduleModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"AddTrailerSchedule(request:{json})"))
            {
                model.IsActive = true;
                model.IsDeleted = false;
                var response = await _driverDomain.AddTrailerSchedule(model);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<DriverObjectModel>> GetAllDrivers(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"GetAllDrivers(companyId:{companyId})"))
            {
                var response = await _driverDomain.GetAllDrivers(companyId);
                return response;
            }
        }


        [HttpPost]
        public async Task<DriverScheduleUpdateModel> UpdateDriverSchedule(List<DriverScheduleMappingViewModel> model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"UpdateDriverSchedule(request:{json})"))
            {
                var response = await _driverDomain.UpdateDriverSchedule(model);
                return response;
            }
        }

        [HttpPost]
        public async Task<DriverScheduleUpdateModel> DeleteAllSchedulesOfDriver(List<DriverScheduleMappingViewModel> driverScheduleMappingViewModels)
        {
            var json = JsonConvert.SerializeObject(driverScheduleMappingViewModels);
            using (var tracer = new Tracer("TrueFill.FreightApi::DriverController", $"DeleteAllSchedulesOfDriver(request:{json})"))
            {
                var response = await _driverDomain.DeleteAllSchedulesOfDriver(driverScheduleMappingViewModels);
                return response;
            }
        }

        [HttpGet]
        public List<DriverScheduleMappingViewModel> GetShiftByDrivers(string driverList, int scheduleType)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetShiftByDrivers(driverList:{driverList})"))
            {
                var response = _driverDomain.GetShiftByDrivers(driverList, scheduleType);
                return response;
            }
        }
    }
}
