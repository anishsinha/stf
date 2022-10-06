using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    public class SmsController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<StatusViewModel> UpdateSmsStatus(SmsResponseViewModel viewModel)
        {
            using (var tracer = new Tracer("SmsController", "UpdateSmsStatus"))
            {
                StatusViewModel response = new StatusViewModel();
                try
                {
                    if (viewModel != null)
                    {
                        var jsonResp = JsonConvert.SerializeObject(viewModel);
                        LogManager.Logger.WriteDebug("SmsController", "UpdateSmsStatus", jsonResp);
                        await ContextFactory.Current.GetDomain<NotificationLogDomain>().UpdateSmsStatus(viewModel);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("SmsController", "UpdateSmsStatus", ex.Message, ex);
                }
                return response;
            }
        }
    }
}