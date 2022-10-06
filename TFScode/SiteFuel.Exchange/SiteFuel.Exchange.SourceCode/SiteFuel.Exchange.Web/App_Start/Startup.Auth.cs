using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Domain;
using System.Security.Claims;
using System;
using Microsoft.Owin.Security;
using SiteFuel.Exchange.Core;
using System.Globalization;
using System.Web.Routing;


namespace SiteFuel.Exchange.Web
{
   
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            ApplicationDomain appDomain = new ApplicationDomain(new BaseDomain(ContextFactory.Current.ConnectionString));

            int cookieExpirationMinutes = appDomain.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingCookieExpirationTime, 300);
            int cookieRevalidationMinutes = appDomain.GetApplicationSettingValue<int>(ApplicationConstants.KeyAppSettingCookieRevalidationTime, 30);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = ApplicationSecurityConstants.ApplicationCookie,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(cookieExpirationMinutes),
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                CookieSecure = CookieSecureOption.Always,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = async context =>
                    {
                        var cookieTimeStamp = context.Identity.FindFirst(c => c.Type == ApplicationSecurityConstants.CookieTimeStamp).Value.GetDateTimeOffsetValue(ApplicationSecurityConstants.CookieTimeStampFormat);
                        if (cookieTimeStamp != DateTimeOffset.MinValue && cookieTimeStamp.AddMinutes(cookieRevalidationMinutes) < DateTimeOffset.Now)
                        {
                            var fingerPrint = context.Identity.FindFirst(c => c.Type == ApplicationSecurityConstants.UserFingerPrint).Value;
                            bool isAuthenticationValid = await new AuthenticationDomain(new BaseDomain(ContextFactory.Current.ConnectionString)).ValidateFingerPrintAsync(fingerPrint);
                            if (!isAuthenticationValid)
                            {
                                context.RejectIdentity();
                                context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
                            }
                            else
                            {
                                context.Identity.RemoveClaim(context.Identity.FindFirst(ApplicationSecurityConstants.CookieTimeStamp));
                                context.Identity.AddClaim(new Claim(ApplicationSecurityConstants.CookieTimeStamp, DateTimeOffset.Now.ToString(ApplicationSecurityConstants.CookieTimeStampFormat, CultureInfo.InvariantCulture)));
                                context.OwinContext.Authentication.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(context.Identity), new AuthenticationProperties { IsPersistent = true });
                            }
                        }
                    }
                }
            }); // Active

            OktaAuth.OktaAuthentication(app);

        }

       
    }
}