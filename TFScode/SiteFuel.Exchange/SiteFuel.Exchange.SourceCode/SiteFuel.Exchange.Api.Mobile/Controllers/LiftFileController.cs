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
    public class LiftFileController : ApiBaseController
    {
        [HttpPost]
        [ApiLog(Enabled = true)]
        public async Task<ApiResponseViewModel> Validate(LiftFileValidateRequest viewModel)
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var response = await ContextFactory.Current.GetDomain<LFVDomain>().SaveLiftFileRecords(viewModel, token);
            return response;
        }

        [HttpGet]
        public async Task<LiftFileStatusResponseViewModel> GetStatus()
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var response = await ContextFactory.Current.GetDomain<LFVDomain>().GetStatus(token);
            return response;
        }

        [HttpPut]
        public async Task<LiftFileStatusResponseViewModel> PushFailedRecords(string bolList)
        {
            if(!string.IsNullOrWhiteSpace(bolList))
            {
                var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
                var response = await ContextFactory.Current.GetDomain<LFVDomain>().PushFailedRecords(token, bolList);
                return response;
            }
            return null;
        }

        [HttpGet]
        public async Task<bool> PostFailedCalls()
        {
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var response = await ContextFactory.Current.GetDomain<LFVDomain>().RetryFailedPostCalls(token);
            return response;
        }

        [HttpPut]
        public async Task<ApiResponseViewModel> LiftTest(LiftFileStatusResponseViewModel responseViewModel)
        {
            var response = new ApiResponseViewModel() { Status = Status.Success };
            response.Messages.Add(new ApiCodeMessages()
            {
                Code = Constants.ApiCodeRS01,
                Message = $"records received successfully from TFX"
            });

            return response;
        }

        //  [HttpGet]
        //public async Task<List<LFRecordsGridViewModel>> GetLiftFileRecordsWithMissingTFXDeliveryDetails()
        //{
        //    using (var tracer = new Tracer("LiftFileController", "GetLiftFileRecordsWithMissingTFXDeliveryDetails"))
        //    {
        //        var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
        //        var apiUserContext = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserContextFromTokenAsync(token);
        //        var response = new List<LFRecordsGridViewModel>();
        //        if (apiUserContext !=null)
        //        {
        //             response = await ContextFactory.Current.GetDomain<LFVDomain>().GetLiftFileRecordsWithMissingTFXDeliveryDetails(apiUserContext);

        //        }
        //        return response;
        //    }

        //}

        //public async Task<List<LFRecordsGridViewModel>> GetTFXDeliveryDetailsWithMissingLiftFileRecords()
        //{
        //    using (var tracer = new Tracer("LiftFileController", "GetTFXDeliveryDetailsWithMissingLiftFileRecords"))
        //    {
        //        var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
        //        var apiUserContext = await ContextFactory.Current.GetDomain<AuthenticationDomain>().GetUserContextFromTokenAsync(token);
        //        var response = new List<LFRecordsGridViewModel>();
        //        if (apiUserContext != null)
        //        {
        //            response = await ContextFactory.Current.GetDomain<LFVDomain>().GetTFXDeliveryDetailsWithMissingLiftFileRecords(apiUserContext);

        //        }
        //        return response;
        //    }

        //}

        [HttpGet]
        public async Task<bool> GetBadgeListFromBadgeManagementAPI()
        {
            var response = false;
            var token = ActionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
            var authDomain = new AuthenticationDomain();
            var apiUserContext = await authDomain.GetUserContextFromTokenAsync(token);
            if (apiUserContext !=null)
            {
             response = await ContextFactory.Current.GetDomain<LFVDomain>().GetBadgelistFromParkLandBadgeManagermentApi();
            }
            return response;
        }

    }
}
