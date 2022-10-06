using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Carrier
{
    public class CarrierAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Carrier";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Carrier_default",
                "Carrier/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}