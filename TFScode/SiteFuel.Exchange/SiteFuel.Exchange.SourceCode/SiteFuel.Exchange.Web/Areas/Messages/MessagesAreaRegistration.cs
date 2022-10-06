using System.Web.Mvc;

namespace SiteFuel.Exchange.Web.Areas.Messages
{
    public class MessagesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Messages";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Messages_default",
                "Messages/{controller}/{action}/{id}",
                new { action = "Messages", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Messages_dispatch",
                "Messages/{controller}/{action}",
                new { action = "Messages", type = UrlParameter.Optional, queryType= UrlParameter.Optional }
            );
        }
    }
}