using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.EmailManager;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
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

    public class UtilityController : ApiBaseController
    {
        [HttpPost]
        public async Task<ResponseViewModel> SendEmail(EmailMessageViewModel viewModel)
        {
            var response = new ResponseViewModel();
            try
            {
                if (ModelState.IsValid && Validate(viewModel.Recaptcha))
                {
                    var serverUrl = ContextFactory.Current.GetDomain<HelperDomain>().GetServerUrl();
                    var salesTeamEmails = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingSalesMailingList);
                    var emailTemplate = ContextFactory.Current.GetDomain<HelperDomain>().GetApplicationEventNotificationTemplate();
                    var emailModel = new ApplicationEventNotificationViewModel
                    {
                        To = salesTeamEmails.Split(';').ToList(),
                        Subject = viewModel.Subject,
                        CompanyLogo = ContextFactory.Current.GetDomain<HelperDomain>().GetAbsoluteServerUrl(serverUrl, Resource.email_HeaderLogo),
                        BodyText = $@"<p>Name : {viewModel.FirstName} {viewModel.LastName}</p>
                                    <p>Email Address : {viewModel.Email}</p>
                                    <p>Phone Number : {viewModel.PhoneNumber}</p>
                                    <p><b>Message : </b> {viewModel.Message}</p>"
                    };
                    await ContextFactory.Current.GetDomain<EmailDomain>().SendEmail(emailTemplate, emailModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("UtilityController", "SendEmail", ex.Message, ex);
            }
            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetAppSetting(AppSettingRequestViewModel request)
        {
            var result = ContextFactory.Current.GetDomain<ApplicationDomain>().GetAppSetting(request);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);

            SaveDebugInfo(request);
            SaveDebugInfo(result);

            return response;
        }

        private bool Validate(string encodedResponse)
        {
            if (string.IsNullOrEmpty(encodedResponse))
            {
                return false;
            }

            var secret = "6LdWXR4UAAAAAL922IMaX3DrIjFFl-tDSR9vfT54";
            using (var client = new System.Net.WebClient())
            {
                var googleReply = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={encodedResponse}");

                return JsonConvert.DeserializeObject<RecaptchaResponseViewModel>(googleReply).Success;
            }
        }
    }
}