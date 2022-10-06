using Microsoft.Practices.Unity;
using SiteFuel.BAL;
using SiteFuel.FreightRepository;
using SiteFuel.MdbDataAccess.DbContext;
using SiteFuel.Repository;
using System.Data.Entity;
using System.Web.Http;
using TrueFill.DemandCaptureDataAccess;
using Unity.WebApi;

namespace TrueFill.FreightApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var mdbContext = new MdbContext();
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ITankDomain, TankDomain>();
            container.RegisterType<ITankRepository, TankRepository>();

            container.RegisterType<IRegionDomain, RegionDomain>();
            container.RegisterType<IRegionRepository, RegionRepository>();

            container.RegisterType<IJobDomain, JobDomain>();
            container.RegisterType<IJobRepository, JobRepository>();

            container.RegisterType<IDriverDomain, DriverDomain>();
            container.RegisterType<IDriverRepository, DriverRepository>();

            container.RegisterType<IShiftDomain, ShiftDomain>();
            container.RegisterType<IShiftRepository, ShiftRepository>();

            container.RegisterType<IDemandCaptureRepository, DemandCaptureRepository>();
            container.RegisterType<IDemandCaptureDomain, DemandCaptureDomain>();

            container.RegisterType<ICarrierDomain, CarrierDomain>();
            container.RegisterType<ICarrierRepository, CarrierRepository>();

            container.RegisterType<IDeliveryRequestRepository, DeliveryRequestRepository>();
            container.RegisterType<IDeliveryRequestDomain, DeliveryRequestDomain>();

            container.RegisterType<ITruckRepository, TruckRepository>();
            container.RegisterType<ITruckDomain, TruckDomain>();

            container.RegisterType<ITractorRepository, TractorRepository>();
            container.RegisterType<ITractorDomain, TractorDomain>();

            container.RegisterType<IScheduleBuilderDomain, ScheduleBuilderDomain>();
            container.RegisterType<IScheduleBuilderRepository, ScheduleBuilderRepository>();

            container.RegisterType<IRouteInformationDomain, RouteInformationDomain>();
            container.RegisterType<IRouteInformationRepository, RouteInformationRepository>();

            container.RegisterType<ISalesDomain, SalesDomain>();
            container.RegisterType<ISalesRepository, SalesRepository>();

            container.RegisterType<ITrailerFuelRetainDomain, TrailerFuelRetainDomain>();
            container.RegisterType<ITrailerFuelRetainRepository, TrailerFuelRetainRepository>();

            container.RegisterType<IForecastingDomain, ForecastingDomain>();
            container.RegisterType<IForecastingRepository, ForecastingRepository>();

            container.RegisterType<IDSBLoadQueueDomain, DSBLoadQueueDomain>();
            container.RegisterType<IDSBLoadQueueRepository, DSBLoadQueueRepository>();

            container.RegisterType<IDRCarrierSequenceDomain, DRCarrierSequenceDomain>();
            container.RegisterType<IDRCarrierSequenceRepository, DRCarrierSequenceRepository>();

            container.RegisterType<IHeldRequestDomain, HeldRequestDomain>();
            container.RegisterType<IHeldRequestRepository, HeldRequestRepository>();

            //context register for dependancy injection.
            container.RegisterInstance<MdbContext>(mdbContext);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}