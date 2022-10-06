using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightApi.Attributes;
using SiteFuel.FreightModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace TrueFill.FreightApi.Controllers
{
    #if DEBUG
        [ApiExplorerSettings(IgnoreApi = false)]
    #else
        [ApiExplorerSettings(IgnoreApi = true)]
    #endif
    public class SalesController : ApiController
    {
        private readonly ISalesDomain _salesDomain;
        public SalesController(ISalesDomain salesDomain)
        {
            _salesDomain = salesDomain;
        }

        [ValidateToken]
        [HttpPost]
        public async Task<SalesDataResponseModel> GetSalesData(SalesDataRequestModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::SalesController", $"GetTankDetailsBySchedule(input:{json})"))
            {
                var response = await _salesDomain.GetSalesData(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<LocationTanksResponseModel> GetLocationTanks(LocationTanksRequestModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::SalesController", $"GetLocationTanks(input:{json})"))
            {
                var response = await _salesDomain.GetLocationTanks(requestModel);
                return response;
            }
        }

        [HttpGet]
        public async Task<SalesGraphRespDataModel> GetSalesGraphData(int jobId, int noOfDays)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::SalesController", $"GetSalesGraphData(jobId:{jobId}, noOfDays:{noOfDays})"))
            {
                var response = await _salesDomain.GetSalesGraphData(jobId, noOfDays);
                return response;
            }
        }
        
        [HttpGet]
        public async Task<DeliveryDetailsRespModel> GetExistingSchedules(int jobId, int productTypeId, int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::SalesController", $"GetExistingSchedules(jobId:{jobId}, productTypeId:{productTypeId})"))
            {
                var response = await _salesDomain.GetExistingSchedules(jobId, productTypeId, companyId);
                return response;
            }
        }

        [HttpPost]
        public async Task<InventoryDataResponseModel> GetInventoryDataForDashboard(InventoryDataViewModel model)
        {
            var response = await _salesDomain.GetInventoryDataForDashboard(model);
            return response;
        }
    }
}
