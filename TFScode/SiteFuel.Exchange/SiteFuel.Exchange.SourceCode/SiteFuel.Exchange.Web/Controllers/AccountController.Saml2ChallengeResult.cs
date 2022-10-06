using Microsoft.Owin.Security;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Web;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Controllers
{
    public partial class AccountController
    {
        
        internal class Saml2ChallengeResult : HttpUnauthorizedResult
        {
            public string RedirectUri { get; set; }

            public Saml2ChallengeResult(string redirectUri)
            {
                RedirectUri = redirectUri;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                try
                {
                    var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                    context.HttpContext.GetOwinContext().Authentication.Challenge(properties, "Saml2");
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("AccountController", "ExecuteResult", ex.Message + "\nRedirect Url:" + RedirectUri, ex);
                }
            }
        }

       

    }
    
}