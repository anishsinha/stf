using Newtonsoft.Json;
using SiteFuel.BAL;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.FreightModels;
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
    [ApiExplorerSettings(IgnoreApi = false)]
    public class DsbLoadQueueController : ApiController
    {
        private readonly IDSBLoadQueueDomain _dSBLoadQueueDomain;
        public DsbLoadQueueController(IDSBLoadQueueDomain dSBLoadQueueDomain)
        {
            _dSBLoadQueueDomain = dSBLoadQueueDomain;
        }
        [HttpPost]
        public async Task<StatusModel> Create(List<DSBLoadQueueModel> model)
        {
            var json = JsonConvert.SerializeObject(model);
            using (var tracer = new Tracer("TrueFill.FreightApi::DsbLoadQueueController", $"Create(request: {json})"))
            {
                var response = await _dSBLoadQueueDomain.CreateDsbLoadQueue(model);
                return response;
            }
        }
        [HttpPost]
        public async Task<StatusModel> Delete(List<string> inputData)
        {
            var json = JsonConvert.SerializeObject(inputData);
            using (var tracer = new Tracer("TrueFill.FreightApi::DsbLoadQueueController", $"Delete(request: {json})"))
            {
                var response = await _dSBLoadQueueDomain.DeleteDsbLoadQueue(inputData);
                return response;
            }
        }
    }
}
