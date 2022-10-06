using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Buyer
{
    public class BuyerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Buyer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Buyer_default",
                "Buyer/{controller}/{action}/{id}",
                new { action = "DashboardNew", id = UrlParameter.Optional }
            );
        }
    }
}