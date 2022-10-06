using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SiteFuel.FreightApi.Attributes
{
    public class ValidateTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Any(t => t.Key.ToLower() == ApplicationConstants.Token))
                {
                    var token = actionContext.Request.Headers.FirstOrDefault(t => t.Key.ToLower() == ApplicationConstants.Token).Value.First();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var isValid = token.Equals(ApplicationConstants.Token);
                        if (!isValid)
                        {
                            OnAuthenticationFailure(1);
                        }
                    }
                    else
                    {
                        OnAuthenticationFailure(2);
                    }
                }
                else
                {
                    OnAuthenticationFailure(3);
                }
            }
            catch(Exception ex)
            {
                LogManager.Logger.WriteException("ValidateTokenAttribute", "OnActionExecuting", ex.Message, ex);
            }
        }

        private void OnAuthenticationFailure(int errorCode)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            switch (errorCode)
            {
                case 1:
                    response.Content = new StringContent($"Auth token invalid or expired");
                    break;
                case 2:
                    response.Content = new StringContent($"Auth token empty");
                    break;
                case 3:
                    response.Content = new StringContent($"Auth token missing");
                    break;
            }
            throw new HttpResponseException(response);
        }
    }
}