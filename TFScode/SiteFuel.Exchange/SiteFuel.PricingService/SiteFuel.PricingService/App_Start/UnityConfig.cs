using Microsoft.Practices.Unity;
using SiteFuel.BAL;
using SiteFuel.DAL;
using System.Web.Http;
using Unity.WebApi;

namespace SiteFuel.PricingService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IPricingDomain, PricingDomain>();
            container.RegisterType<ICurrencyRateDomain, CurrencyRateDomain>();
            container.RegisterType<IPricingRepository, PricingRepository>();

            container.RegisterType<IPricingRequestDomain, PricingRequestDomain>();
            container.RegisterType<IPricingRequestRepository, PricingRequestRepository>();

            container.RegisterType<ICurrentCostDomain, CurrentCostDomain>();
            container.RegisterType<ICurrentCostRepository, CurrentCostRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}