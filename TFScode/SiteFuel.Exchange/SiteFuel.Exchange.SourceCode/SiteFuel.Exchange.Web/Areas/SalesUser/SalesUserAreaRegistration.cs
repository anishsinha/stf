using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.SalesUser
{
    public class SalesUserAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SalesUser";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SalesUser_default",
                "SalesUser/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}