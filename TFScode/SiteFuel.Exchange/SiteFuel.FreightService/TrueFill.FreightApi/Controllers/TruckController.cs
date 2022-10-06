using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.FreightApi.Attributes;
using SiteFuel.FreightModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Text;

namespace SiteFuel.FreightApi.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class TruckController : ApiController
    {
        private readonly ITruckDomain _truckDomain;

        public TruckController(ITruckDomain truckDomain)
        {
            _truckDomain = truckDomain;
        }

        [HttpGet]
        public async Task<List<TruckDetailViewModel>> GetAllTruckDetails(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"GetAllTruckDetails(companyId:{companyId})"))
            {
                var response = await _truckDomain.GetAllTruckDetails(companyId);
                return response;
            }
        }
        [HttpGet]
        public async Task<DropdownDisplayExtended> GetTruckRegionDetails(string truckId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"GetTruckRegionDetails(truckId:{truckId})"))
            {
                var response = await _truckDomain.GetTruckRegionDetails(truckId);
                return response;
            }
        }
        [HttpGet]
        public async Task<List<TruckDetailViewModel>> GetAllTruckRetainFuelDetails(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"GetAllTruckRetainFuelDetails(companyId:{companyId})"))
            {
                var response = await _truckDomain.GetAllTruckRetainFuelDetails(companyId);
                return response;
            }
        }        

        [HttpGet]
        public async Task<TruckDetailViewModel> GetTruckDetails(string truckId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"GetTruckDetails(truckId:{truckId})"))
            {
                var response = await _truckDomain.GetTruckDetails(truckId);
                return response;
            }
        }
        
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> SaveTruckDetails(TruckDetailViewModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"SaveTruckDetails(request:{json})"))
            {
                var response = await _truckDomain.SaveTruckDetails(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> UpdatetruckDetails(TruckDetailViewModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"UpdatetruckDetails(request:{json})"))
            {
                StatusModel response = await _truckDomain.UpdateTruckDetails(requestModel);
                return response;
            }
        }

        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> DeleteTruck(TruckDetailViewModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"DeleteTruck(request:{json})"))
            {
                StatusModel response = await _truckDomain.DeleteTruck(requestModel);
                return response;
            }
        }

        [HttpGet]
        public List<ExternalVehicleMappingViewModel> GetVehiclesForExternalMapping(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"GetVehiclesForExternalMapping(companyId:{companyId})"))
            {
                var response = _truckDomain.GetVehiclesForExternalMapping(companyId);
                return response;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> SaveExternalVehicleMapping(ExternalVehicleMappingViewModel viewModel)
        {
            var json = JsonConvert.SerializeObject(viewModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"SaveExternalVehicleMapping(request:{json})"))
            {
                var result = await _truckDomain.SaveExternalVehicleMapping(viewModel);
                return result;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> SaveBulkUploadVehicleMapping(ExternalVehicleMappingInputModel inputModel)
        {
            var json = JsonConvert.SerializeObject(inputModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TruckController", $"SaveBulkUploadVehicleMapping(request:{json})"))
            {
                var result = await _truckDomain.SaveBulkUploadVehicleMapping(inputModel.UserId, inputModel.ListExternalVehicles);
                return result;
            }
        }
    }
}
