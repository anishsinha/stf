using Newtonsoft.Json;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightModels;
using SiteFuel.FreightRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace TrueFill.FreightApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = false)]
    public class RouteInfoController : ApiController
    {
        private readonly IRouteInformationDomain _routeInfoDomain;
        public RouteInfoController(IRouteInformationDomain routeInfoDomain)
        {
            _routeInfoDomain = routeInfoDomain;
        }
        [HttpPost]
        public async Task<StatusModel> Create(RouteInformationModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"Create(request: {json})"))
            {
                var response = await _routeInfoDomain.CreateRouteInformation(model);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> Update(RouteInformationModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"Uodate(request: {json})"))
            {
                var response = await _routeInfoDomain.UpdateRouteInformation(model);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> Delete(RouteInformationModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"Delete(request: {model})"))
            {
                var response = await _routeInfoDomain.DeleteRouteInformation(model.Id, model.RegionId, model.CreatedBy);
                return response;
            }
        }
        [HttpGet]
        public List<SiteFuel.FreightModels.DropdownDisplayItem> GetLocationDetails(int companyId, string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetLocationDetails(request: {regionId})"))
            {
                var response = _routeInfoDomain.GetRegionLocationDetails(companyId, regionId);
                return response;
            }
        }
        [HttpGet]
        public List<RouteInformationModel> GetRouteInfoDetails(int companyId, string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetRouteInfoDetails(request: {regionId}),companyId:{companyId}"))
            {
                var response = _routeInfoDomain.GetRouteInformations(companyId, regionId);
                return response;
            }
        }
        [HttpGet]
        public List<DropdownDisplayExtended> GetRouteInfoDetails(string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetRouteInfoDetails-TPO(request: {regionId})"))
            {
                var response = _routeInfoDomain.GetRouteInformations(regionId);
                return response;
            }
        }
        [HttpPost]
        public List<RouteCustomerLocationModel> GetRouteInfoDetails(List<string> regionId)
        {
            var json = JsonConvert.SerializeObject(regionId);
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetRouteInfoDetails-CustomerLocation(request: {json})"))
            {
                var response = _routeInfoDomain.GetRouteInformations(regionId);
                return response;
            }
        }
        [HttpPost]
        public Task<StatusModel> AssignTPOJobToRoute(JobToRegionAssignViewModel jobToUpdate)
        {
            var json = JsonConvert.SerializeObject(jobToUpdate);
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"AssignTPOJobToRoute(JobToRegionAssignViewModel:{json})"))
            {
                var response = _routeInfoDomain.AssignTPOJobToRoute(jobToUpdate);
                return response;
            }
        }
        [HttpGet]
        public List<SiteFuel.FreightModels.DropdownDisplayItem> GetLocationDetails(string Id, string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetLocationDetails-Edit(request: {Id})"))
            {
                var response = _routeInfoDomain.GetRouteLocationDetails(Id, regionId);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> UpdateShiftInfo(RouteInformationModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"UpdateShiftInfo(request: {json})"))
            {
                var response = await _routeInfoDomain.UpdateShiftInfo(model);
                return response;
            }
        }
        [HttpPost]
        public List<InvoiceRouteInfo> GetInvoiceRouteInfo(List<string> deliveryReqId)
        {
            var json = JsonConvert.SerializeObject(deliveryReqId);
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetInvoiceRouteInfo(request: {json})"))
            {
                var response = _routeInfoDomain.GetInvoiceRouteInfo(deliveryReqId);
                return response;
            }
        }
        [HttpGet]
        public async Task<string> GetRouteIdForJob(int jobId, int companyId,string regionId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::RouteInfoController", $"GetRouteIdForJob(jobId:{jobId},companyId:{companyId},regionId:{regionId})"))
            {
                var response = await _routeInfoDomain.GetRouteIdForJob(jobId, companyId, regionId);
                return response;
            }

        }
    }
}
