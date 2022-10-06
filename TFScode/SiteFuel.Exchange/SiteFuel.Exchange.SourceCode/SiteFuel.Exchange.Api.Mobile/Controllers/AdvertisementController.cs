using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
    public class AdvertisementController : ApiBaseController
    {
        [HttpPost]
        public async Task<RequestPriceOutputViewModel> RequestPrice(RequestPriceViewModel viewModel)
        {
            var response = new RequestPriceOutputViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    viewModel.RequestDateTime = DateTimeOffset.Now;
                    response = await ContextFactory.Current.GetDomain<AdvertisementDomain>().RequestPriceAsync(viewModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdvertisementController", "RequestPrice", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        public async Task<ResponseViewModel> RequestFuel(RequestFuelViewModel viewModel)
        {
            var response = new ResponseViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    viewModel.RequestDateTime = DateTimeOffset.Now;
                    response = await ContextFactory.Current.GetDomain<AdvertisementDomain>().RequestFuelAsync(viewModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AdvertisementController", "RequestFuel", ex.Message, ex);
            }
            return response;
        }
    }
}