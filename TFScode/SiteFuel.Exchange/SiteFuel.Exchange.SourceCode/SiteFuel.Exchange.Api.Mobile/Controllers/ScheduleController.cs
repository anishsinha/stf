using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SiteFuel.Exchange.Api.Mobile.Controllers
{
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    [ValidateToken]

    public class ScheduleController : ApiController
    {
        [HttpPost]
        [ApiLog(Enabled = true, TPDLogEnabled = true)]
        public async Task<ApiResponseViewModel> Create(TPDCreateScheduleViewModel viewModel)
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var response = await ContextFactory.Current.GetDomain<InvoiceTPDDomain>().ValidateAndCreateSchedule(viewModel, token);
            return response;
        }

        [HttpPost]
        [ApiLog(Enabled = true, TPDLogEnabled = true)]
        public async Task<ApiResponseViewModel> StatusUpdate(TPDScheduleStatusViewModel viewModel)
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            ApiResponseViewModel response = await ContextFactory.Current.GetDomain<InvoiceTPDDomain>().UpdateScheduleStatus(viewModel, token);
            return response;
        }
    }
}
