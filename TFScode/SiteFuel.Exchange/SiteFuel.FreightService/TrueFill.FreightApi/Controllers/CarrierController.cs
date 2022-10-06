using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightModels;
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
    public class CarrierController : ApiController
    {
        private readonly ICarrierDomain _carrierDomain;
        public CarrierController(ICarrierDomain carrierDomain)
        {
            _carrierDomain = carrierDomain;
        }

        [HttpGet]
        public List<CarrierViewModel> GetCarriers(int companyId, int carrierCompanyId = 0)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"GetCarriers(companyId:{companyId},carrierCompanyId:{carrierCompanyId})"))
            {
                var response = _carrierDomain.GetSupplierCarriers(companyId, carrierCompanyId);
                return response;
            }
        }


        [HttpGet]
        public CarrierJobDetailsViewModel GetAssignedCarriers(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"GetAssignedCarriers(companyId:{companyId})"))
            {
                var response = _carrierDomain.GetCarrierUsers(companyId);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> AssignToSupplier(List<CarrierViewModel> model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"AssignToSupplier({json})"))
            {
                var response = await _carrierDomain.AssignToSupplier(model);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> UpdateAssignedCarriers(CarrierViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"UpdateAssignedCarriers({json})"))
            {
                var response = await _carrierDomain.UpdateAssignedCarriers(model);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> DeleteAssignedCarriers(CarrierViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"DeleteAssignedCarriers({json})"))
            {
                var response = await _carrierDomain.DeleteAssignedCarriers(model);
                return response;
            }
        }

        [HttpPost]
        public Task<StatusModel> AssignCarrierToJob(CarrierViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::RegionController", $"AssignTPOJobToRegion(JobToCarrierAssignViewModel:{model})"))
            {
                var response = _carrierDomain.AssignCarrierToJob(model);
                return response;
            }
        }

        [HttpGet]
        public async Task<List<int>> GetCarriersJobs(int carrierCompanyId,int customerCompanyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"GetCarriersJobs(companyId:{carrierCompanyId})"))
            {
                var response = await _carrierDomain.GetCarriersJobs(carrierCompanyId, customerCompanyId);
                return response;
            }
        }
        [HttpPost]
        public async Task<List<DipTestRequestModel>> GetCarrierDetailsByJob(List<int> jobIds)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::CarrierController", $"GetCarrierDetailsByJob(supplierIds:{jobIds})"))
            {
                var response = await _carrierDomain.GetCarrierDetailsByJob(jobIds);
                return response;
            }
        }
    }
}