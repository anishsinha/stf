using Microsoft.Owin;
using Owin;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;

[assembly: OwinStartupAttribute(typeof(SiteFuel.Exchange.Web.Startup))]
namespace SiteFuel.Exchange.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ContextFactory.Register(new ApplicationContext());
            ConfigureAuth(app);
          
        }
    }
}
