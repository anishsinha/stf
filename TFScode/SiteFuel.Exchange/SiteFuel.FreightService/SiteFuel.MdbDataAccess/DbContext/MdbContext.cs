using MongoDB.Driver;
using SiteFuel.FreightModels;
using SiteFuel.MdbDataAccess.Collections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.MdbDataAccess.DbContext
{
    public class MdbContext
    {
        public MdbContext()
        {
            var conString = ConfigurationManager.ConnectionStrings["FreightDbConnection"].ConnectionString;
            Client = new MongoClient(conString);
            Database = Client.GetDatabase(ConfigurationManager.AppSettings.Get("FreightDbName"));
            FreightTables = Database.GetCollection<FreightTable>("FreightTables");
            FreightTablePrices = Database.GetCollection<FreightTablePrice>("FreightTablePrices");
            JobAdditionalDetails = Database.GetCollection<JobAdditionalDetail>("JobAdditionalDetails");
            Regions = Database.GetCollection<Region>("Regions");
            Drivers = Database.GetCollection<Driver>("Drivers");
            Shifts = Database.GetCollection<Shift>("Shifts");
            Carriers = Database.GetCollection<Carrier>("SupplierCarriers");
            CarrierJobs = Database.GetCollection<CarrierJob>("CarrierJobs");
            DeliveryRequests = Database.GetCollection<DeliveryRequest>("DeliveryRequests");
            HeldDeliveryRequests = Database.GetCollection<HeldDeliveryRequest>("HeldDeliveryRequests");
            TruckDetails = Database.GetCollection<TruckDetail>("TruckDetails");
            TractorDetails = Database.GetCollection<TractorDetail>("TractorDetails");
            OrderTanks = Database.GetCollection<OrderTankMapping>("OrderTanks");
            ScheduleBuilders = Database.GetCollection<ScheduleBuilder>("ScheduleBuilders");
            TankModalTypes = Database.GetCollection<TankModalType>("TankModalTypes");
            DriverScheduleShiftMapping = Database.GetCollection<DriverScheduleShiftMapping>("DriverScheduleShiftMapping");
            TrailerScheduleMapping = Database.GetCollection<TrailerScheduleMapping>("TrailerScheduleMapping");
            RecurringSchedules = Database.GetCollection<RecurringSchedules>("RecurringSchedules");
            RouteInformations = Database.GetCollection<RouteInformations>("RouteInformations");
            TrailerFuelRetains = Database.GetCollection<TrailerFuelRetain>("TrailerFuelRetain");
            BrokeredDRJobs = Database.GetCollection<BrokeredDRJob>("BrokeredDRJob");
            DsbNotification = Database.GetCollection<DsbNotification>("DsbNotification");
            OttoScheduleInfos = Database.GetCollection<OttoScheduleInfo>("OttoScheduleInfo");
            ExternalVehicleMappings = Database.GetCollection<ExternalVehicleMappings>("ExternalVehicleMappings");
            DSBLoadQueues = Database.GetCollection<DSBLoadQueue>("DSBLoadQueue");
            RegionScheduleMappings = Database.GetCollection<RegionScheduleMapping>("RegionScheduleMapping");
            DrFilterPreferences = Database.GetCollection<DrFilterPreferences>("DrFilterPreferences");
            ForecastingTankInformations = Database.GetCollection<ForecastingTankInformation>("ForecastingTankInformation");
            DRCarrierSequences = Database.GetCollection<DRCarrierSequence>("DRCarrierSequence");
            DSBColumnOptionalPickupInfos = Database.GetCollection<DSBColumnOptionalPickupInfo>("DSBColumnOptionalPickupInfo");
        }
        public MongoClient Client { get; set; }
        public IMongoDatabase Database { get; set; }
        public IMongoCollection<FreightTable> FreightTables { get; set; }
        public IMongoCollection<FreightTablePrice> FreightTablePrices { get; set; }
        public IMongoCollection<JobAdditionalDetail> JobAdditionalDetails { get; set; }
        public IMongoCollection<Region> Regions { get; set; }
        public IMongoCollection<Driver> Drivers { get; set; }
        public IMongoCollection<Shift> Shifts { get; set; }
        public IMongoCollection<Carrier> Carriers { get; set; }
        public IMongoCollection<CarrierJob> CarrierJobs { get; set; }
        public IMongoCollection<DeliveryRequest> DeliveryRequests { get; set; }
        public IMongoCollection<HeldDeliveryRequest> HeldDeliveryRequests { get; set; }
        public IMongoCollection<TruckDetail> TruckDetails { get; set; }
        public IMongoCollection<TractorDetail> TractorDetails { get; set; }
        public IMongoCollection<OrderTankMapping> OrderTanks { get; set; }
        public IMongoCollection<ScheduleBuilder> ScheduleBuilders { get; set; }
        public IMongoCollection<TankModalType> TankModalTypes { get; set; }
        public IMongoCollection<DriverScheduleShiftMapping> DriverScheduleShiftMapping { get; set; }
        public IMongoCollection<TrailerScheduleMapping> TrailerScheduleMapping { get; set; }
        public IMongoCollection<RecurringSchedules> RecurringSchedules { get; set; }
        public IMongoCollection<RouteInformations> RouteInformations { get; set; }
        public IMongoCollection<TrailerFuelRetain> TrailerFuelRetains { get; set; }
        public IMongoCollection<BrokeredDRJob> BrokeredDRJobs { get; set; }
        public IMongoCollection<DsbNotification> DsbNotification { get; set; }
        public IMongoCollection<OttoScheduleInfo> OttoScheduleInfos { get; set; }
        public IMongoCollection<ExternalVehicleMappings> ExternalVehicleMappings { get; set; }
        public IMongoCollection<DSBLoadQueue> DSBLoadQueues { get; set; }
        public IMongoCollection<RegionScheduleMapping> RegionScheduleMappings { get; set; }
        public IMongoCollection<DrFilterPreferences> DrFilterPreferences { get; set; }
        public IMongoCollection<ForecastingTankInformation> ForecastingTankInformations { get; set; }

        public IMongoCollection<DRCarrierSequence> DRCarrierSequences { get; set; }
        public IMongoCollection<DSBColumnOptionalPickupInfo> DSBColumnOptionalPickupInfos { get; set; }
    }
}
