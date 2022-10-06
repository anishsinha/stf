using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightApi.Attributes;
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
    public class TractorController : ApiController
    {
        // GET: Tractor
        private readonly ITractorDomain _tractorDomain;
        public TractorController(ITractorDomain tractorDomain)
        {
            _tractorDomain = tractorDomain;
        }
        [HttpGet]
        public async Task<List<TractorDetailViewModel>> GetAllTractorDetails(int companyId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TractorController", $"GetAllTractorDetails(companyId:{companyId})"))
            {
                var response = await _tractorDomain.GetAllTractorDetails(companyId);
                return response;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> SaveTractorDetails(TractorDetailViewModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TractorController", $"SaveTractorDetails(request:{json})"))
            {
                var response = await _tractorDomain.SaveTractorDetails(requestModel);
                return response;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> UpdateTractorDetails(TractorDetailViewModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TractorController", $"UpdateTractorDetails(request:{json})"))
            {
                StatusModel response = await _tractorDomain.UpdateTractorDetails(requestModel);
                return response;
            }
        }
        [ValidateToken]
        [HttpPost]
        public async Task<StatusModel> DeleteTractor(TractorDetailViewModel requestModel)
        {
            var json = JsonConvert.SerializeObject(requestModel);
            using (var tracer = new Tracer("TrueFill.FreightApi::TractorController", $"DeleteTractor(request:{json})"))
            {
                StatusModel response = await _tractorDomain.DeleteTractor(requestModel);
                return response;
            }
        }
        [HttpGet]
        public async Task<List<SiteFuel.Exchange.Utilities.DropdownDisplayItem>> GetAllDrivers(int companyId,string trailerTypeId)
        {
            using (var tracer = new Tracer("TrueFill.FreightApi::TractorController", $"GetAllDrivers(companyId:{companyId},trailerTypeId:{trailerTypeId})"))
            {
                var response = await _tractorDomain.GetAllDrivers(companyId,trailerTypeId);
                return response;
            }
        }
        
    }
}