using SiteFuel.Exchange.Core.Utilities;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.cXML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Web
{
    /// <summary>
    /// Summary description for Punchout
    /// </summary>
    public class Punchout : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            var requestContentType = context.Request.ContentType;
            if (requestContentType.Equals(ApplicationConstants.ContentTypeXml, StringComparison.OrdinalIgnoreCase))
            {
                string xmlDocumentString = string.Empty; string responseText = string.Empty;
                using (StreamReader streamReader = new StreamReader(context.Request.InputStream))
                {
                    xmlDocumentString = streamReader.ReadToEnd();
                    Logger.LogManager.Logger.WriteInfo("Punchout", "ProcessRequest", "Request=>" + xmlDocumentString);
                }

                PunchoutRequest punchoutRequest = null;
                if (!string.IsNullOrWhiteSpace(xmlDocumentString))
                {
                    punchoutRequest = XmlSerialization.Deserialize<PunchoutRequest>(xmlDocumentString);
                }

                if (punchoutRequest != null && !string.IsNullOrWhiteSpace(punchoutRequest.Header.Sender.Credential.Identity) &&
                   !string.IsNullOrWhiteSpace(punchoutRequest.Header.Sender.Credential.SharedSecret))
                {
                    LoginViewModel loginViewModel = new LoginViewModel
                    {
                        Email = punchoutRequest.Header.Sender.Credential.Identity,
                        Password = punchoutRequest.Header.Sender.Credential.SharedSecret
                    };
                    AuthenticationDomain authenticationDomain = new AuthenticationDomain();
                    var userViewModel = Task.Run(() => authenticationDomain.PasswordSignInAsync(loginViewModel)).Result;
                    if (userViewModel.StatusCode == AuthStatus.Success)
                    {
                        var userToken = Task.Run(() => authenticationDomain.GenerateAuthTokenAsync(userViewModel.Id, userViewModel.Email, string.Empty)).Result;
                        if (!string.IsNullOrWhiteSpace(userToken.Token))
                        {
                            var punchoutResponse = GetPunchoutResponse(context, punchoutRequest, userToken);
                            responseText = XmlSerialization.Serialize(punchoutResponse);
                        }
                    }
                    else
                    {
                        responseText = HttpStatusCode.Unauthorized.ToString();
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                }
                else
                {
                    responseText = HttpStatusCode.BadRequest.ToString();
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                context.Response.ContentType = requestContentType;
                context.Response.Write(responseText);
                Logger.LogManager.Logger.WriteInfo("Punchout", "ProcessRequest", "Response=>" + responseText);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static PunchoutResponse GetPunchoutResponse(HttpContext context, PunchoutRequest punchoutRequest, ConfirmationToken userToken)
        {
            PunchoutResponse punchoutResponse = new PunchoutResponse();
            punchoutResponse.Lang = punchoutRequest.Lang;
            punchoutResponse.PayloadID = punchoutRequest.PayloadID;
            punchoutResponse.Timestamp = DateTimeOffset.Now.ToString();
            punchoutResponse.Response.PunchOutSetupResponse.StartPage.URL = GetStartPageUrl(context, punchoutRequest, userToken);
            return punchoutResponse;
        }

        private static string GetStartPageUrl(HttpContext context, PunchoutRequest punchoutRequest, ConfirmationToken userToken)
        {
            var url = context.Request.Url;
            var formPost = punchoutRequest.Request.PunchOutSetupRequest.BrowserFormPost.URL;
			var punchoutCookie = punchoutRequest.Request.PunchOutSetupRequest.BuyerCookie;
            var encodedToken = GetEncodedToken($"{userToken.Token}||{formPost}||{punchoutCookie}");
            return $"{url.Scheme}://{url.Authority}/Account/PunchoutStart?token={encodedToken}";
        }

        private static string GetEncodedToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(token);
                token = Convert.ToBase64String(bytes);
            }
            return token;
        }
    }
}