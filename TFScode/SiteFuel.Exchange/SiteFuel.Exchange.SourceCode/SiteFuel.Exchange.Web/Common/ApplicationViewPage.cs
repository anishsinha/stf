using SiteFuel.Exchange.Utilities;
using System.Configuration;
using System.Security.Claims;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Common
{
    public abstract class ApplicationViewPage<TModel> : WebViewPage<TModel>
    {
        protected ApplicationUser CurrentUser
        {
            get
            {
                return new ApplicationUser(this.User as ClaimsPrincipal);
            }
        }
    }

    public abstract class ApplicationViewPage : ApplicationViewPage<dynamic>
    {
    }

    public class SiteKeys
    {
        public static string StyleVersion
        {
            get
            {
                return "<link href=\"{0}?v=" + ConfigHelperMethods.GetConfigSetting(ApplicationConstants.AppVersion) + "\" rel=\"stylesheet\"/>";
            }
        }
        public static string ScriptVersion
        {
            get
            {
                return "<script src=\"{0}?v=" + ConfigHelperMethods.GetConfigSetting(ApplicationConstants.AppVersion) + "\"></script>";
            }
        }
    }
}