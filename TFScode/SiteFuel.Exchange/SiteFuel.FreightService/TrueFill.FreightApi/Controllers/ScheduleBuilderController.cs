using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightApi.Attributes;
using SiteFuel.FreightModels;
using SiteFuel.FreightModels.ScheduleBuilder;
using System;
using System.Collections.Generic;
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
    public class ScheduleBuilderController : ApiController
    {
        private readonly IScheduleBuilderDomain _scheduleBuilderDomain;

        public ScheduleBuilderController(IScheduleBuilderDomain scheduleBuilderDomain)
        {
            _scheduleBuilderDomain = scheduleBuilderDomain;
        }

        [HttpGet]
        public async Task<List<DropdownDisplayExtended>> GetRegions(int userId)
        {
            List<DropdownDisplayExtended> response = await _scheduleBuilderDomain.GetRegions(userId);
            return response;
        }

        [HttpGet]
        public async Task<RegionDetailModel> GetRegionDetails(string regionId)
        {
            RegionDetailModel response = await _scheduleBuilderDomain.GetRegionDetails(regionId);
            return response;
        }

        [HttpGet]
        public async Task<List<int>> GetCarrierSuppliersBySiteId(int jobId, int carrierCompanyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetRegions(siteId:{jobId}, carrierCompanyId:{carrierCompanyId})"))
            {
                List<int> response = await _scheduleBuilderDomain.GetCarrierSuppliersBySiteId(jobId, carrierCompanyId);
                return response;
            }
        }

        [HttpGet]
        public async Task<ScheduleBuilderViewModel> GetSheduleBuilderDetails(int companyId, int userId, string regionId, string date, int sbView, string scheduleBuilderId, int sbDsbView = 1, bool IsDsbDriverSchedule = false)
        {
            ScheduleBuilderViewModel response = await _scheduleBuilderDomain.GetScheduleBuilderDetails(companyId, userId, regionId, date, sbView, scheduleBuilderId, sbDsbView, IsDsbDriverSchedule);
            return response;
        }

        [HttpGet]
        public async Task<ScheduleBuilderResponseModel> IsValidTimeStamp(string selectedDate, string regionId, int companyId, long lastTimeStamp)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"IsValidTimeStamp(selectedDate:{selectedDate}, regionId:{regionId}, companyId:{companyId}, lastTimeStamp:{lastTimeStamp})"))
            {               
                ScheduleBuilderResponseModel response = await _scheduleBuilderDomain.IsValidTimeStamp(selectedDate, regionId, companyId, lastTimeStamp);
                return response;
            }
        }

        [HttpPost]
        [ValidateToken]
        public async Task<StatusModel> CheckAndLockDrs(LockDrModel model)
        {
            var response = await _scheduleBuilderDomain.CheckAndLockDrs(model);
            return response;
        }

        [HttpPost]
        [ValidateToken]
        public async Task<StatusModel> CheckAndReleaseDrs(LockDrModel model)
        {
            var response = await _scheduleBuilderDomain.CheckAndReleaseDrs(model);
            return response;
        }


        [HttpPost]
        [ValidateToken]
        public async Task<List<DSBSaveModel>> SaveScheduleBuilder(List<DSBSaveModel> scheduleBuilders)
        {
            var response = await _scheduleBuilderDomain.SaveScheduleBuilder(scheduleBuilders);
            return response;
        }

        [HttpPost]
        [ValidateToken]
        public async Task<StatusModel> CancelSchedules(CancelScheduleModel model)
        {
            var jsonString = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"CancelSchedules(model:{jsonString})"))
            {
                var response = await _scheduleBuilderDomain.CancelSchedules(model);
                return response;
            }
        }

        [HttpPost]
        [ValidateToken]
        public async Task<List<DSBSaveModel>> DeleteTrip(List<DSBSaveModel> models)
        {
            var response = await _scheduleBuilderDomain.DeleteTrip(models);
            return response;
        }

        //[HttpPost]
        //[ValidateToken]
        //public async Task<DSBSaveModel> UpdateDeletedRequests(DSBSaveModel scheduleBuilder)
        //{
        //    var response = await _scheduleBuilderDomain.UpdateDeletedRequests(scheduleBuilder);
        //    return response;
        //}

        [HttpPost]
        [ValidateToken]
        public async Task<DSBSaveModel> AssignDriverAndTrailer(DSBSaveModel scheduleBuilder)
        {
            var response = await _scheduleBuilderDomain.AssignDriverAndTrailer(scheduleBuilder);
            return response;
        }

        [HttpPost]
        [ValidateToken]
        public async Task<List<SiteFuel.Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(DriversViewModel driversViewModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetAllDrivers(companyId:{driversViewModel.companyId},regionId:{driversViewModel.regionId})"))
            {
                var response = await _scheduleBuilderDomain.GetAllDrivers(driversViewModel.companyId, driversViewModel.trailerTypeId, driversViewModel.regionId, Convert.ToDateTime(driversViewModel.selectedDate));
                return response;
            }
        }

        [HttpPost]
        [ValidateToken]
        public async Task<List<DriverAdditionalDetailsViewModel>> GetAllDriverDetails(DriversViewModel driversViewModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetAllDriverDetails(companyId:{driversViewModel.companyId},regionId:{driversViewModel.regionId})"))
            {
                var response = await _scheduleBuilderDomain.GetAllDriverDetails(driversViewModel.companyId, driversViewModel.trailerTypeId, driversViewModel.regionId, Convert.ToDateTime(driversViewModel.selectedDate));
                return response;
            }
        }

        [HttpGet]
        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverId(int driverId, string selectedDate)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetSelectedDateDriverScheduleByDriverId"))
            {
                var response = await _scheduleBuilderDomain.GetSelectedDateDriverScheduleByDriverId(driverId, Convert.ToDateTime(selectedDate));
                return response;
            }
        }

        [HttpPost]
        [ValidateToken]
        public async Task<List<DeliveryRequestViewModel>> ValidateTrailerJobCompatibility(TrailersDeliveryRequestViewModel trailersDeliveryRequestViewModel)
        {
            var jsontrailers = JsonConvert.SerializeObject(trailersDeliveryRequestViewModel.trailers);
            var jsondeliveryRequests = JsonConvert.SerializeObject(trailersDeliveryRequestViewModel.deliveryRequests);
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"ValidateTrailerJobCompatibility(trailers:{jsontrailers},deliveryRequests:{jsondeliveryRequests})"))
            {
                var response = await _scheduleBuilderDomain.ValidateTrailerJobCompatibility(trailersDeliveryRequestViewModel.trailers, trailersDeliveryRequestViewModel.deliveryRequests);
                return response;
            }
        }


        [HttpPost]
        [ValidateToken]
        public async Task<List<TrailerJobNonCompatibleDrs>> ValidateTrailerJobCompatibilityForLoadQueue(List<TrailersDeliveryRequestViewModel> models)
        {

            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"ValidateTrailerJobCompatibilityForLoadQueue(models:{models})"))
            {
                return await _scheduleBuilderDomain.ValidateTrailerJobCompatibilityForLoadQueue(models);
            }
        }

        [HttpPost]
        [ValidateToken]
        public async Task<DSBSaveModel> IsValidScheduleBuilder(DSBSaveModel scheduleBuilder)
        {            
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"IsValidScheduleBuilder(Id:{scheduleBuilder.Id},RegionId:{scheduleBuilder.RegionId}),UserId:{scheduleBuilder.UserId},Date:{scheduleBuilder.Date}"))
            {
                var response = await _scheduleBuilderDomain.IsValidScheduleBuilder(scheduleBuilder);               
                return response;
            }
        }

        [HttpPost]
        public async Task<List<ScheduleBuilderViewModel>> GetScheduleBuildersByDrIds(List<string> drIds)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetScheduleBuildersByDrIds: " + string.Join(",", drIds)))
            {
                var response = await _scheduleBuilderDomain.GetScheduleBuildersByDrIds(drIds);
                return response;
            }
        }

        [HttpPost]
        public async Task<DSBSaveModel> GetLoads(CreateScheduleModel scheduleInput)
        {
            var response = await _scheduleBuilderDomain.GetLoads(scheduleInput);
            return response;
        }

        [HttpPost]
        [ValidateToken]
        public async Task<DSBSaveModel> CreateSchedules(DSBSaveModel scheduleInput)
        {
            var response = await _scheduleBuilderDomain.CreateSchedules(scheduleInput);
            return response;
        }

        [HttpPost]
        public async Task<UnassignDriverViewModel> UnAssignDriverFromShift(UnassignDriverViewModel removeDriver)
        {
            var response = await _scheduleBuilderDomain.UnAssignDriverFromShift(removeDriver);
            return response;
        }
        [HttpGet]
        public async Task<List<CreateRecurringDRViewModel>> RecurringScheduleDetails(string dayOfWeek, int currentDay, string date)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RecurringScheduleDetails", $"RecurringScheduleDetails"))
            {
                var response = await _scheduleBuilderDomain.GetRecurringScheduleDetails(dayOfWeek, currentDay, date);
                return response;
            }
        }

        [HttpPost]
        public async Task<ScheduleBuilderViewModel> GetRecurringScheduleBuilderDetails(RecurringScheduleBuilder recurringScheduleBuilder)
        {
            ScheduleBuilderViewModel response = await _scheduleBuilderDomain.GetScheduleBuilderDetails(recurringScheduleBuilder.CompanyId, recurringScheduleBuilder.UserId, recurringScheduleBuilder.RegionId, recurringScheduleBuilder.Date, recurringScheduleBuilder.View, recurringScheduleBuilder.DsbView, recurringScheduleBuilder.ScheduleBuilderId, recurringScheduleBuilder.ShiftInformation, recurringScheduleBuilder.ScheduleBuilderViewId, recurringScheduleBuilder.IsBackgroundJobScheduleCreation, recurringScheduleBuilder.IsDsbDriverSchedule);
            return response;
        }
        [HttpPost]
        public async Task<List<TrailerCompartmentDetail>> GetTrailerCompartmentDetails(List<string> Id)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetTrailerCompartmentDetails"))
            {
                var response = await _scheduleBuilderDomain.GetTrailerCompartmentDetails(Id);
                return response;
            }
        }
        [HttpPost]
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(List<string> Id)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetTrailerFuelRetainDetails"))
            {
                var response = await _scheduleBuilderDomain.GetTrailerFuelRetainDetails(Id);
                return response;
            }
        }
        [HttpPost]
        public async Task<DrFilterPreferencesModel> SaveDrFilterPreferences(DrFilterPreferencesModel model)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"SaveDrFilterPreferences"))
            {
                var response = await _scheduleBuilderDomain.SaveDrFilterPreferences(model);
                return response;
            }
        }
        [HttpGet]
        public async Task<DrFilterPreferencesModel> GetDrFilterPreferences(int userId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetDrFilterPreferences"))
            {
                var response = await _scheduleBuilderDomain.GetDrFilterPreferences(userId);
                return response;
            }
        }

        [HttpPost]
        [ValidateToken]
        public async Task<List<DriverAdditionalDetailsViewModel>> GetShiftDriverDetails(DriversViewModel driversViewModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetAllDriverDetails(companyId:{driversViewModel.companyId},regionId:{driversViewModel.regionId},,IsDsbDriverSchedule:{driversViewModel.IsDsbDriverSchedule})"))
            {
                var response = await _scheduleBuilderDomain.GetAllDriverDetails(driversViewModel.companyId, driversViewModel.trailerTypeId, driversViewModel.regionId, driversViewModel.otherRegion, Convert.ToDateTime(driversViewModel.selectedDate), driversViewModel.shiftId, driversViewModel.IsDsbDriverSchedule);
                return response;
            }
        }
        [HttpGet]
        public async Task<List<DriverScheduleViewModel>> GetSelectedDateDriverScheduleByDriverIdGridView(int driverId, string selectedDate, string shiftId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ScheduleBuilderController", $"GetSelectedDateDriverScheduleByDriverIdGridView"))
            {
                var response = await _scheduleBuilderDomain.GetSelectedDateDriverScheduleByDriverId(driverId, Convert.ToDateTime(selectedDate), shiftId);
                return response;
            }
        }
        [HttpPost]
        public async Task<ResetDeliveryGroupScheduleModel> RemoveScheduleBuilderDrs(ResetDeliveryGroupScheduleModel model)
        {
            var response = await _scheduleBuilderDomain.RemoveScheduleBuilderDrs(model);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> AddOptionalPickup(List<DSBColumnOptionalPickupInfoModel> dSBColumnOptionalPickupInfo)
        {
            var response = await _scheduleBuilderDomain.AddOptionalPickup(dSBColumnOptionalPickupInfo);
            return response;
        }
        [HttpPost]
        public async Task<List<DSBColumnOptionalPickupInfoModel>> GetOptionalPickupDetails(DSBColumnOptionalPickupInfoModel model)
        {
            var response = await _scheduleBuilderDomain.GetOptionalPickupDetails(model);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> DeleteOptionalPickupDetails(DSBColumnOptionalPickupInfoModel optionalPickupInfoModel)
        {
            var response = await _scheduleBuilderDomain.DeleteOptionalPickupDetails(optionalPickupInfoModel.Id);
            return response;
        }
        [HttpPost]
        public async Task<StatusModel> UpdateDROptionalPickupInfo(List<ScheduleOptionalPickupModel> scheduleOptionalPickups)
        {
            var response = await _scheduleBuilderDomain.UpdateDROptionalPickupInfo(scheduleOptionalPickups);
            return response;
        }
        [HttpPost]
        public async Task<List<PreBOLRetainDeliveryDetailsModel>> GetPreLoadDSRetainInfo(List<PreBOLRetainModel> PreLoadBolDRs)
        {
            var response =  await _scheduleBuilderDomain.GetPreLoadDSRetainInfo(PreLoadBolDRs);
            return response;
        }
        [HttpPost]
        [ValidateToken]
        public async Task<List<DSBSaveModel>> CancelScheduleBuilder(List<DSBSaveModel> scheduleBuilders)
        {
            var response = await _scheduleBuilderDomain.CancelScheduleBuilder(scheduleBuilders);
            return response;
        }
        [HttpPost]
        public async Task<ResetDeliveryGroupScheduleModel> RemoveDeliverySchedule(ResetDeliveryGroupScheduleModel model)
        {
            var response = await _scheduleBuilderDomain.RemoveDeliverySchedule(model);
            return response;
        }
    }
}
