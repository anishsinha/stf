using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.Web.Controllers;
using System;
using System.Configuration;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SiteFuel.Exchange.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //To optimize the performance
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } });

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Enabling or Disabling Bundling and Minification
            BundleTable.EnableOptimizations = false;

            //This is commented because we have implemented cache busting
            //#if DEBUG
            //            BundleTable.EnableOptimizations = false;
            //#else
            //            BundleTable.EnableOptimizations = true;
            //#endif
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ApplicationSecurityConstants.UserId;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var httpContext = ((MvcApplication)sender).Context;
                var currentController = string.Empty;
                var currentAction = string.Empty;
                var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

                if (currentRouteData != null)
                {
                    if (currentRouteData.Values[ApplicationConstants.Controller] != null && !String.IsNullOrEmpty(currentRouteData.Values[ApplicationConstants.Controller].ToString()))
                    {
                        currentController = currentRouteData.Values[ApplicationConstants.Controller].ToString();
                    }

                    if (currentRouteData.Values[ApplicationConstants.Action] != null && !String.IsNullOrEmpty(currentRouteData.Values[ApplicationConstants.Action].ToString()))
                    {
                        currentAction = currentRouteData.Values[ApplicationConstants.Action].ToString();
                    }
                }

                var ex = Server.GetLastError();
                var routeData = new RouteData();
                var action = "GenericError";
                var controller = "Error";

                if (ex is HttpException)
                {
                    var httpEx = ex as HttpException;

                    switch (httpEx.GetHttpCode())
                    {
                        case 404:
                            action = "NotFound";
                            break;

                            //Add others if any
                    }
                }

                httpContext.ClearError();
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
                httpContext.Response.TrySkipIisCustomErrors = true;

                routeData.Values[ApplicationConstants.Area] = string.Empty;
                routeData.Values[ApplicationConstants.Action] = action;
                routeData.Values[ApplicationConstants.Controller] = controller;
                routeData.Values[ApplicationConstants.Exception] = new HandleErrorInfo(ex, currentController, currentAction);

                LogManager.Logger.WriteException(currentController, currentAction, ex.Message, ex);

                IController errorController = new ErrorController();
                HttpContextWrapper wrapper = new HttpContextWrapper(httpContext);
                var rc = new RequestContext(wrapper, routeData);
                errorController.Execute(rc);
                httpContext.Response.End();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("MvcApplication", "Application_Error", ex.Message, ex);
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            ClaimsIdentity identity = ((ClaimsIdentity)User.Identity);
            Claim claim = identity.FindFirst(ApplicationSecurityConstants.Email);
            if (claim != null && identity.FindFirst(ClaimTypes.Name) == null)
            {
                string email = claim.Value;
                identity.AddClaim(new Claim(ClaimTypes.Name, email));
            }
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                Response.Headers["X-FRAME-OPTIONS"] = ConfigurationManager.AppSettings.Get("XFRAMEOPTIONS");
            }
            catch
            {
            }
        }
    }
}
