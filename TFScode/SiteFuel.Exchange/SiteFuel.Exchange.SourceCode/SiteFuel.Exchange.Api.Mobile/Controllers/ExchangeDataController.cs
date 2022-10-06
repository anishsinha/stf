using Newtonsoft.Json;
using SiteFuel.Exchange.Api.Mobile.Attributes;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
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
    public class ExchangeDataController : ApiBaseController
    {
        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<LocationResponseModel> GetLocationsAsBuyer(string fromDate = "", string toDate = "")
        {
            var response = new LocationResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<JobDomain>().GetBuyerLocations(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<LocationResponseModel> GetLocationsAsSupplier(string fromDate = "", string toDate = "")
        {
            var response = new LocationResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<JobDomain>().GetCustomerLocations(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<AssetResponseModel> GetAssetsAsBuyer(string fromDate = "", string toDate = "")
        {
            var response = new AssetResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<AssetDomain>().GetAssets(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<AssetResponseModel> GetAssetsAsSupplier(string fromDate = "", string toDate = "")
        {
            var response = new AssetResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<AssetDomain>().GetCustomerAssets(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<AssetResponseModel> GetTanksAsBuyer(string fromDate = "", string toDate = "")
        {
            var response = new AssetResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTanks(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<AssetResponseModel> GetTanksAsSupplier(string fromDate = "", string toDate = "")
        {
            var response = new AssetResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<AssetDomain>().GetCustomerTanks(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<FuelRequestResponseModel> GetFuelRequestsAsBuyer(string fromDate = "", string toDate = "")
        {
            var response = new FuelRequestResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestsAsBuyer(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<FuelRequestResponseModel> GetFuelRequestsAsSupplier(string fromDate = "", string toDate = "")
        {
            var response = new FuelRequestResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<FuelRequestDomain>().GetFuelRequestsAsSupplier(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<OrderAPIResponseModel> GetOrdersAsBuyer(string fromDate = "", string toDate = "")
        {
            var response = new OrderAPIResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrdersAsBuyer(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        //[ApiLog(Enabled = true)]
        public async Task<OrderAPIResponseModel> GetOrdersAsSupplier(string fromDate = "", string toDate = "")
        {
            var response = new OrderAPIResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrdersAsSupplier(token, fromDate, toDate);

            return response;
        }

        [HttpGet]
        public async Task<TankInventoryResponseModel> GetTankReadings()
        {
            var response = new TankInventoryResponseModel();
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            response = await ContextFactory.Current.GetDomain<AssetDomain>().GetTankInventoryDetails(token, true);

            return response;
        }
    }
}
