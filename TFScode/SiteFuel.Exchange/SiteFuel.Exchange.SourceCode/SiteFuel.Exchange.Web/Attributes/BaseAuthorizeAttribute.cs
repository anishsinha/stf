using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SiteFuel.Exchange.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    class BaseAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            try
            {
                var returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {
                    var user = new ApplicationUser((ClaimsPrincipal)filterContext.HttpContext.User);
                    var errorMessage = $@"Unauthorized exception details =>
                                            User : {user.Name}
                                            Company Type : {user.CompanyTypeId}
                                            Roles : {string.Join("", user.Roles.Select(t => Enum.GetName(typeof(UserRoles), (UserRoles)t)))}
                                            URL : {returnUrl}";
                    LogManager.Logger.WriteError("BaseAuthorizeAttribute", "HandleUnauthorizedRequest", errorMessage);

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { ApplicationConstants.Area, "" },
                        { ApplicationConstants.Action, "Index" },
                        { ApplicationConstants.Controller, "Unauthorized" }
                        }
                    );
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { ApplicationConstants.Area, "" },
                        { ApplicationConstants.Action, "Login" },
                        { ApplicationConstants.Controller, "Account" },
                        { ApplicationConstants.ReturnUrl, returnUrl }
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BaseAuthorizeAttribute", "HandleUnauthorizedRequest", ex.Message, ex);
            }
        }
    }
}