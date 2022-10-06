using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightApi.Attributes;
using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class TankController : ApiController
    {
        private readonly ITankDomain _tankDomain;

        public TankController(ITankDomain tankDomain)
        {
            _tankDomain = tankDomain;
        }

        [HttpGet]
        public async Task<TankDetailsModel> GetTankDetails(int assetId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"GetTankDetails(assetId:{assetId})"))
            {
                TankDetailsModel response = await _tankDomain.GetTankDetails(assetId);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<ScheduleOutputModel> GetTankDetailsBySchedule(List<ScheduleInputModel> scheduleInputDetails)
        {
            var json = JsonConvert.SerializeObject(scheduleInputDetails);
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"GetTankDetailsBySchedule(input:{json})"))
            {
                ScheduleOutputModel response = await _tankDomain.GetTankDetailsBySchedule(scheduleInputDetails);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<TankDetailListModel>> GetTankList(List<int> tanks)
        {
            List<TankDetailListModel> response = await _tankDomain.GetTankList(tanks);
            return response;
        }

        [HttpGet]
        public async Task<List<JobTankDetailModel>> GetJobTankList(int jobId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"GetJobTankList(jobId:{jobId})"))
            {
                List<JobTankDetailModel> response = await _tankDomain.GetJobTankList(jobId);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<TankVolumeAndUllageModel> GetTankVolumeAndUllage(TankVolumeAndUllageInputModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"GetTankVolumeAndUllage(request:{json})"))
            {
                TankVolumeAndUllageModel response = await _tankDomain.GetTankVolumeAndUllage(requestModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<DropQuantityByPrePostDipResponseModel>> GetDropQuantityByPrePostDip(List<DropQuantityByPrePostDipRequestModel> requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"GetDropQuantityByPrePostDip(request:{json})"))
            {
                List<DropQuantityByPrePostDipResponseModel> response = await _tankDomain.GetDropQuantityByPrePostDip(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> SaveTankDetails(TankDetailsModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"SaveTankDetails(request:{json})"))
            {
                bool response = await _tankDomain.SaveTankDetails(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> UpdateTankDetails(TankDetailsModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"UpdateTankDetails(request:{json})"))
            {
                bool response = await _tankDomain.UpdateTankDetails(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<bool> UpdateOrderTankMapping(List<OrderTankMappingViewModel> requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"UpdateOrderTankMapping(request:{json})"))
            {
                bool response = await _tankDomain.OrderTankMappings(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<List<DropdownDisplayExtended>> GetTankModelType(List<int> companyId)
        {
            using (var tracer = new Tracer("TankController", $"GetTankModelType(companyId:{string.Join(",", companyId)})"))
            {
                var response = await _tankDomain.GetTankModelType(companyId);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> SaveTankTypes(TankModalTypeViewModel tankTypes)
        {
            var response = new StatusModel();
            try
            {
                var json = JsonConvert.SerializeObject(tankTypes);
                using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"SaveTankTypes(request:{json})"))
                {
                    response = await _tankDomain.SaveTankTypeDipChart(tankTypes);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("TankController", "SaveTankTypes", ex.Message, ex);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<TankModalTypeViewModel>> GetTankTypesByCompany(int companyId)
        {
            var response = new List<TankModalTypeViewModel>();
            try
            {
                using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"UpdateOrderTankMapping(companyId:{companyId})"))
                {
                    response = await _tankDomain.GetTankTypesByCompany(companyId);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("TankController", "SaveTankTypes", ex.Message, ex);
                return response;
            }
        }
        [HttpGet]
        public async Task<StatusModel> DeleteTankDipChartById(string id)
        {
            var response = new StatusModel();
            try
            {
                using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"DeleteTankDipChartById(companyId:{id})"))
                {
                    response = await _tankDomain.DeleteTankDipChartById(id);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("TankController", "DeleteTankDipChartById", ex.Message, ex);
                return response;
            }
        }
        [HttpGet]
        public async Task<List<string>> GetAllTankTypeNameForDipChart(int companyId, string searchValue)
        {
            var response = new List<string>();
            try
            {
                var json = JsonConvert.SerializeObject(new { searchValue, companyId });
                using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"GetAllTankTypeNameForDipChart(request:{json})"))
                {
                    response = await _tankDomain.GetAllTankTypeNameForDipChart(companyId, searchValue);
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("TankController", "GetAllTankTypeNameForDipChart", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        public async Task<StatusModel> UpdateTankSequence(TankDetailsModel requestModel)
        {
            var response = new StatusModel();
            try
            {
                var json = JsonConvert.SerializeObject(requestModel);
                using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"UpdateTankSequence(request:{json})"))
                {
                    response = await _tankDomain.UpdateTankSequence(requestModel);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("TankController", "UpdateTankSequence", ex.Message, ex);
                return response;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<bool> CheckDuplicateTankSequence(TankDetailsModel requestModel)
        {
            var response = true;
            try
            {
                var json = JsonConvert.SerializeObject(requestModel);
                using (var tracer = new Tracer("TrueFill.FreightApi::TankController", $"CheckDuplicateTankSequence(request:{json})"))
                {
                    response = await _tankDomain.CheckDuplicateTankSequence(requestModel);
                    return response;
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Logger.WriteException("TankController", "CheckDuplicateTankSequence", ex.Message, ex);
                return response;
            }
        }
        [HttpPost]
        public async Task<List<JobTankAdditionalDetailModel>> GetJobsTankList(List<int> jobId)
        {
            List<JobTankAdditionalDetailModel> response = await _tankDomain.GetJobsTankList(jobId);
            return response;
        }
    }
}
