using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
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
    public class TimeCardController : ApiBaseController
    {
        [HttpPost]
        public async Task<TimeCardOutputRequestViewModel> SetTimeCardAction(TimeCardInputRequestViewModel timeCardInputViewModel)
        {
            using (var tracer = new Tracer("TimeCardController", "SetTimeCardAction"))
            {
                if (!string.IsNullOrEmpty(timeCardInputViewModel.UserTimeZone))
                {
                    LogManager.Logger.WriteDebug("TimeCardController", "SetTimeCardAction", timeCardInputViewModel.UserTimeZone);
                }
                var response = new TimeCardOutputRequestViewModel();
                if(timeCardInputViewModel!=null && timeCardInputViewModel.UserId > 0)
                {
                    if (timeCardInputViewModel.ActionId > 0)
                    {
                        response = await ContextFactory.Current.GetDomain<TimeCardDomain>().SetTimeCardAction(timeCardInputViewModel);
                    }
                    else
                    {
                        response.StatusMessage = Core.StringResources.Resource.errMessageInvalidActionId;
                    }
                }
                return response;
            }
        }

        [HttpGet]
        public async Task<TimeCardOutputRequestViewModel> GetTimeCardAction([FromUri] TimeCardInputRequestViewModel timeCardInputViewModel)
        {
            using (var tracer = new Tracer("TimeCardController", "GetTimeCardAction"))
            {
                var response = new TimeCardOutputRequestViewModel();
                if (timeCardInputViewModel != null && timeCardInputViewModel.UserId > 0)
                {
                    response = await ContextFactory.Current.GetDomain<TimeCardDomain>().GetTimeCardAction(timeCardInputViewModel);
                }
                return response;
            }
        }

    }
}
