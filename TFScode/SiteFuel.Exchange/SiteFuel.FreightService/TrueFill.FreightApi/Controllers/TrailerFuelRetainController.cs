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
    [ApiExplorerSettings(IgnoreApi = false)]
    public class TrailerFuelRetainController : ApiController
    {
        private readonly ITrailerFuelRetainDomain _trailerFuelRetainDomain;
        public TrailerFuelRetainController(ITrailerFuelRetainDomain trailerFuelRetainDomain)
        {
            _trailerFuelRetainDomain = trailerFuelRetainDomain;
        }
        [HttpGet]
        public async Task<List<TrailerFuelRetainViewModel>> GetTrailerFuelRetainDetails(string trailerId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TrailerFuelRetainController", $"TrailerFuelRetainViewModel(trailerId:{trailerId})"))
            {
                var response = await _trailerFuelRetainDomain.GetTrailerFuelRetainDetails(trailerId);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> SaveTrailerFuelRetain(List<TrailerFuelRetainViewModel> requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TrailerFuelRetainController", $"SaveTrailerFuelRetain(request:{json})"))
            {
                var response = await _trailerFuelRetainDomain.SaveTrailerFuelRetain(requestModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> ResetFuelRetainDetails(TruckDetailViewModel truckDetailViewModel)
        {   
            using (var tracer = new Tracer("TrueFill.FreightApi::TrailerFuelRetainController", $"ResetFuelRetainDetails(truckDetailViewModel:{truckDetailViewModel})"))
            {
                var response = await _trailerFuelRetainDomain.ResetTrailerFuelRetained(truckDetailViewModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> UpdateRetainFuelDetails(TruckDetailViewModel truckDetailViewModel)
        {          
            using (var tracer = new Tracer("TrueFill.FreightApi::TrailerFuelRetainController", $"UpdateFuelRetainDetails(truckDetailViewModel:{truckDetailViewModel})"))
            {
                var response = await _trailerFuelRetainDomain.UpdateTrailerFuelRetained(truckDetailViewModel);
                return response;
            }
        }

        [HttpPost]
        public async Task<StatusModel> ConfirmTrailerFuelRetainedException(TruckDetailViewModel truckDetailViewModel)
        {          
            using (var tracer = new Tracer("TrueFill.FreightApi::TrailerFuelRetainController", $"ConfirmRetainFuelException(truckDetailViewModel:{truckDetailViewModel})"))
            {
              var response = await _trailerFuelRetainDomain.ConfirmTrailerFuelRetainedException(truckDetailViewModel);
                return response;
            }
        }
    }
}
