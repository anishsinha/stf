using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Hauler
{
    public class HaulerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Hauler";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Hauler_default",
                "Hauler/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}