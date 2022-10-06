using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.Dispatcher;
using SiteFuel.MdbDataAccess.Collections;
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
    public class RegionController : ApiController
    {
        private readonly IRegionDomain _regionDomain;
        private readonly IShiftDomain _shiftDomain;

        public RegionController(IRegionDomain regionDomain, IShiftDomain shiftDomain)
        {
            _regionDomain = regionDomain;
            _shiftDomain = shiftDomain;
        }

        [HttpPost]
        public async Task<StatusModel> Create(RegionViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"Create(request: {json})"))
            {
                var response = await _regionDomain.CreateRegion(model);
                if (response.StatusCode == (int)Status.Success)
                {
                    if (model.Shifts != null && model.Shifts.Count > 0)
                    {
                        model.Shifts.ForEach(t => t.RegionId = response.RegionId);
                        model.Shifts = await _shiftDomain.CreateShifts(model.Shifts);
                    }
                }
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> Update(RegionViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"Update(request: {json})"))
            {
                var response = await _regionDomain.UpdateRegion(model);
                if (model.Shifts != null && response.StatusCode == (int)Status.Success)
                {
                    await _shiftDomain.DeleteShifts(model.CompanyId, model.Id);
                    if (model.Shifts != null && model.Shifts.Count > 0)
                    {
                        model.Shifts.ForEach(t => t.RegionId = model.Id);
                        model.Shifts = await _shiftDomain.CreateShifts(model.Shifts);
                    }
                }
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> Delete(string regionId, int deletedBy)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"Delete(regionId:{regionId}, deletedBy:{deletedBy})"))
            {
                var response = await _regionDomain.DeleteRegion(regionId, deletedBy);
                return response;
            }
        }
        [HttpGet]
        public async Task<string> GetRegionName(string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegionName(regionId:{regionId})"))
            {
                var response = await _regionDomain.GetRegionName(regionId);
                return response;
            }
        }

        [HttpGet]
        public async Task<RegionViewModel> GetRegion(int companyId, string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegion(companyId:{companyId},regionId:{regionId})"))
            {
                var response = await _regionDomain.GetRegion(companyId, regionId);
                return response;
            }
        }

        [HttpGet]
        public List<RegionViewModel> GetRegions(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegions(companyId:{companyId})"))
            {
                var response = _regionDomain.GetRegions(companyId);
                return response;
            }
        }

        [HttpGet]
        public List<DropdownDisplayExtendedItem> GetShiftDdl(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetShiftDdl(companyId:{companyId})"))
            {
                var response = _shiftDomain.GetShiftDdl(companyId);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<DispatcherDashboardRegionModel>> GetDispatcherDashboardRegions(int companyId, int dispatcherId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetDispatcherDashboardRegions(companyId:{companyId},dispatcherId:{dispatcherId})"))
            {
                var response = await _regionDomain.GetDispatcherDashboardRegions(companyId, dispatcherId);
                return response;
            }
        }

        [HttpPost]
        public Task<StatusModel> AssignTPOJobToRegion(JobToRegionAssignViewModel jobToUpdate)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"AssignTPOJobToRegion(JobToRegionAssignViewModel:{jobToUpdate})"))
            {
                var response = _regionDomain.AssignTPOJobToRegion(jobToUpdate);
                return response;
            }
        }
        public async Task<string> GetRegionIdForJob(int jobId, int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegionIdForJob(jobId:{jobId},companyId:{companyId})"))
            {
                var response = await _regionDomain.GetRegionIdForJob(jobId, companyId);
                return response;
            }

        }

        public async Task<List<QuickEntryDRModels>> GetJobsForAllRegions(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetJobsForAllRegions(companyId:{companyId})"))
            {
                var response = await _regionDomain.GetJobsForAllRegions(companyId);
                return response;
            }

        }

        [HttpGet]
        public List<int> GetDriverDetailsByCompanyId(int companyId, int dispatcherId, string regionID)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetDriverDetailsByCompanyId(companyId:{companyId})"))
            {
                var response = _regionDomain.GetDriverDetailsByCompanyId(companyId, dispatcherId, regionID);
                return response;
            }
        }

        [HttpGet]
        public List<DropdownDisplayExtendedItem> GetRegionsDdl(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegionsDdl(companyId:{companyId})"))
            {
                var response = _regionDomain.GetRegionsDdl(companyId);
                return response;
            }
        }

        [HttpGet]
        public List<DropdownDisplayExtendedItem> GetCarriersAssignedToRegion(string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetCarriersAssignedToRegion(regionId:{regionId})"))
            {
                var response = _regionDomain.GetCarriersAssignedToRegion(regionId);
                return response;
            }
        }

        [HttpPost]
        public List<DropdownDisplayExtendedItem> GetDispatchersAssignedToRegion(List<string> regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetDispatchersAssignedToRegion(regionId:{regionId})"))
            {
                var response = _regionDomain.GetDispatchersAssignedToRegion(regionId);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<int>> GetJobsAssignedToRegions(int companyId)
        {

            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetJobsAssignedToRegions(companyId:{companyId})"))
            {
                var response = await _regionDomain.GetJobsAssignedToRegions(companyId);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<int>> GetJobsAssignedToDriver(int driverId)
        {

            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetJobsAssignedToDriver(driverId:{driverId})"))
            {
                var response = await _regionDomain.GetJobsAssignedToDriver(driverId);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<RegionJobsModel>> GetRegionsForJobs(RegionInputModel request)
        {
            var json = JsonConvert.SerializeObject(request);
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegionsForJobs(request:{json})"))
            {
                var response = await _regionDomain.GetRegionsForJobs(request);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<string>> GetDispatcherRegionIds(int companyId, int dispatcherId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetDispatcherRegionIds(companyId:{companyId},dispatcherId:{dispatcherId})"))
            {
                var response = await _regionDomain.GetDispatcherRegionIds(companyId, dispatcherId);
                return response;
            }
        }
        [HttpGet]
        public List<TfxCarrierDropdownDisplayItem> GetRegionCarriers(string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegionCarriers(regionId:{regionId})"))
            {
                var response = _regionDomain.GetRegionCarriers(regionId);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> AddRegionSchedule(RegionScheduleModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"AddRegionSchedule(request:{json})"))
            {
                model.IsActive = true;
                model.IsDeleted = false;
                var response = await _regionDomain.AddRegionSchedule(model);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<RegionScheduleModel>> getRegionShiftSchedule(string regionId, string routeId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"getRegionShiftSchedule(request:regionId:{regionId},routeId:{routeId})"))
            {
                var response = await _regionDomain.getRegionShiftSchedule(regionId, routeId);
                return response;
            }
        }

        [HttpGet]
        public List<RegionScheduleMappingViewModel> getRegionShiftSchedule(string regionId, int scheduleType)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"getRegionShiftSchedule(request:regionId:{regionId},scheduleType:{scheduleType})"))
            {
                var response = _regionDomain.getRegionShiftSchedule(regionId, scheduleType);
                return response;
            }
        }

        [HttpPost]
        public Task<List<CarrierRegionModel>> GetAllCarrierRegions(List<DropdownDisplayExtendedItem> carriers)
        {
            var response = _regionDomain.GetAllCarrierRegions(carriers);
            return response;
        }
        [HttpPost]
        public Task<StatusModel> RemoveDriverFromRegion(RegionDriverRemoveModel model)
        {
            var response = _regionDomain.RemoveDriverFromRegion(model);
            return response;
        }
        [HttpPost]
        public Task<List<InvitedDriverResponseModel>> CheckInvitedDriverScheduleExists(List<RegionDriverRemoveModel> model)
        {
            var response = _regionDomain.CheckInvitedDriverScheduleExists(model);
            return response;
        }
        [HttpGet]
        public async Task<RegionFavProductModel> GetRegionFavouriteProducts(int? jobId, string regionId, int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"GetRegionFavouriteProducts(jobId:{jobId},regionId:{regionId},companyId:{companyId})"))
            {
                var response = await _regionDomain.GetRegionFavouriteProducts(jobId, regionId, companyId);
                return response;
            }
        }
       
        [HttpPost]
        public bool IsPublishedDR(int companyId, string productIds, string orderIds)
        {
            var response = _regionDomain.IsPublishedDR(companyId, productIds, orderIds);
            return response;
        }
    }
}
