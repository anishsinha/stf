using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightModels;
using System;
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
    public class ForecastingController : ApiController
    {
        private readonly IForecastingDomain _forecastingDomain;

        public ForecastingController(IForecastingDomain forecastingDomain)
        {
            _forecastingDomain = forecastingDomain;
        }

        [HttpGet]
        public async Task<List<ForecastingTankDetailsModel>> GetForecastingTankDetails(int jobId, string tankId, string storageId, string uOM,string jobTimeZone)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetForecastingTankDetails(jobId:{jobId},tankId:{tankId},storageId:{storageId},uOM:{uOM})"))
            {
                var response = await _forecastingDomain.GetForecastingTankDetails(jobId, tankId, storageId, uOM, jobTimeZone);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<ForecastingEstimatedUsageModel>> GetForecastingTankEstimatedUsageDetails(int jobId, string startDate, string endDate, string tankId, string storageId, string uOM)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetForecastingTankEstimatedUsageDetails(jobId:{jobId},startDate:{startDate},endDate:{endDate},tankId:{tankId},storageId:{storageId},uOM:{uOM})"))
            {
                var response = await _forecastingDomain.GetForecastingTankEstimatedUsageDetails(jobId, startDate, endDate, tankId, storageId, uOM);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<ForecastingInventoryModel>> GetForecastingTankInventoryDetails(int jobId, string tankId, string storageId, string uOM)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetForecastingTankInventoryDetails(jobId:{jobId},tankId:{tankId},storageId:{storageId},uOM:{uOM})"))
            {
                var response = await _forecastingDomain.GetForecastingTankInventoryDetails(jobId, tankId, storageId, uOM);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<ForecastingDeliveryModel>> GetForecastingTankDeliveryDetails(ForecastingTankDeliveryModel forecastingTankDeliveryModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetForecastingTankDeliveryDetails(jobId:{forecastingTankDeliveryModel.jobId},tankId:{forecastingTankDeliveryModel.tankId},storageId:{forecastingTankDeliveryModel.storageId},uOM:{forecastingTankDeliveryModel.uOM})"))
            {
                var response = await _forecastingDomain.GetForecastingTankDeliveryDetails(forecastingTankDeliveryModel.jobId, forecastingTankDeliveryModel.tankId, forecastingTankDeliveryModel.storageId, forecastingTankDeliveryModel.uOM);
                return response;
            }
        }

        [HttpPost]
        public async Task<List<ForecastingExistingScheduleModel>> GetForecastingTankScheduleDetails(ForecastingTankDeliveryModel forecastingTankDeliveryModel)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetForecastingTankScheduleDetails(jobId:{forecastingTankDeliveryModel.jobId},tankId:{forecastingTankDeliveryModel.tankId},storageId:{forecastingTankDeliveryModel.storageId},uOM:{forecastingTankDeliveryModel.uOM})"))
            {
                var response = await _forecastingDomain.GetForecastingTankScheduleDetails(forecastingTankDeliveryModel.jobId, forecastingTankDeliveryModel.tankId, forecastingTankDeliveryModel.storageId, forecastingTankDeliveryModel.uOM);
                return response;
            }
        }

        [HttpPost]
        public async Task<ForecastingTankDataForChartModel> GetForecastingTankDataForChart(ForecastingTankDeliveryModel forecastingTankDeliveryModel)
        {
            DateTime currentDate = DateTime.Now;
            DateTime.TryParse(forecastingTankDeliveryModel.startDtTm, out currentDate);
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetForecastingTankDataForChart(jobId:{forecastingTankDeliveryModel.jobId},startDtTm:{forecastingTankDeliveryModel.startDtTm},tankId:{forecastingTankDeliveryModel.tankId},storageId:{forecastingTankDeliveryModel.storageId},uOM:{forecastingTankDeliveryModel.uOM})"))
            {
                var response = await _forecastingDomain.GetForecastingTankDataForChart(forecastingTankDeliveryModel.jobId, currentDate, forecastingTankDeliveryModel.tankId, forecastingTankDeliveryModel.storageId, forecastingTankDeliveryModel.uOM);
                return response;
            }
        }
        [HttpPost]
        public async Task<TankRetainInfo> CalculateTankRetainWindowInfo(UspRetainWindowInfo tankRetainInfo)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"CalculateTankRetainWindowInfo(tankId:{tankRetainInfo.tankId},storageId:{tankRetainInfo.storageId})"))
            {
                var response = await _forecastingDomain.CalculateTankRetainWindowInfo(tankRetainInfo);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> SaveDsbNotification(DsbNotificationModel dsbNotification)
        {
            var json = JsonConvert.SerializeObject(dsbNotification);
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"SaveDsbNotification(request:{json})"))
            {
                var response = await _forecastingDomain.SaveDsbNotification(dsbNotification);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<DsbNotificationModel>> GetDsbNotification(string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetDsbNotification(regionId:{regionId})"))
            {
                var response = await _forecastingDomain.GetDsbNotification(regionId);
                return response;
            }
        }

        [HttpGet]
        public async Task<int> GetDsbNotificationCount(string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"GetDsbNotificationCount(regionId:{regionId})"))
            {
                var response = await _forecastingDomain.GetDsbNotificationCount(regionId);
                return response;
            }
        }

        [HttpPost]
        public StatusModel UpdateDsbNotificationStatus([FromBody]string id)
        {
            var json = JsonConvert.SerializeObject(id);
            using (var tracer = new Tracer("TrueFill.FreightApi::ForecastingController", $"UpdateDsbNotificationStatus(request:{json})"))
            {
                var response = _forecastingDomain.UpdateDsbNotificationStatus(id);
                return response;
            }
        }
    }
}
