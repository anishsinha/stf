using SiteFuel.Exchange.Web.Attributes;
using System.Web.Mvc;

namespace SiteFuel.Exchange.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeRoleAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
