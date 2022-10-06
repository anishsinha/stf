using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class WallyBoardDomain : BaseDomain
    {
        public WallyBoardDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public WallyBoardDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<WhereIsMyDriverViewModel>> GetOnGoingLoadsForMapView(UserContext userContext, BuyerWhereIsMyDriverInputModel input)
        {
            List<WhereIsMyDriverViewModel> response = new List<WhereIsMyDriverViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var driverLocations = await spDomain.GetOnGoingLoadsForMapAsync(userContext.CompanyId, input);
                response = driverLocations.Select(t => t.ToViewModel(null)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("WallyBoardDomain", "GetOnGoingLoadsForMapView", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<WhereIsMyDriverViewModel>> GetBuyerLoadsForGrid(UserContext userContext, BuyerWhereIsMyDriverInputModel input, DataTableSearchModel requestModel)
        {
            List<WhereIsMyDriverViewModel> response = new List<WhereIsMyDriverViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var driverLocations = await spDomain.GetBuyerLoadsAsync(userContext.CompanyId, input, requestModel);
                response = driverLocations.Select(t => t.ToViewModel(null)).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("WallyBoardDomain", "GetBuyerLoadsForGrid", ex.Message, ex);
            }
            return response;
        }

        public async Task<WhereIsMyDriverBuyerAppViewModel> GetOnGoingLoadsForBuyerAppAsync(WhereIsMyDriverBuyerAppInputModel input)
        {
            WhereIsMyDriverBuyerAppViewModel response = new WhereIsMyDriverBuyerAppViewModel();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                var driverLocations = await spDomain.GetOnGoingLoadsForBuyerAppAsync(input);
                if (driverLocations != null && driverLocations.Any())
                {
                    foreach (var job in driverLocations.GroupBy(x => x.JobId))
                    {
                        var firstItem = job.FirstOrDefault();

                        var location = new WhereIsMyDriverBuyerAppLocation();

                        location.JobId = firstItem.JobId;
                        location.JobName = firstItem.JobName;
                        location.Address = new WhereIsMyDriverBuyerAppLocationsAddress()
                        {
                            Address = firstItem.Location,
                            CityName = firstItem.JobCity,
                            Latitude = firstItem.JobLatitude,
                            Longitude = firstItem.JobLongitude,
                            StateId = firstItem.JobStateId,
                            StateName = firstItem.JobState
                        };
                        foreach (var schedule in job.GroupBy(x => x.TrackableScheduleId))
                        {
                            if (schedule != null && schedule.Any())
                            {
                                var driverschedule = schedule.Select(t => new WhereIsMyDriverBuyerAppLocationsSchedules()
                                {
                                    CarrierCompany = t.CarrierCompany,
                                    CarrierCompanyId = t.CarrierCompanyId,
                                    DeliveryScheduleId = t.DeliveryScheduleId,
                                    SupplierCompany = t.SupplierCompany,
                                    SupplierCompanyId = t.SupplierCompanyId,
                                    TrackableScheduleId = t.TrackableScheduleId,
                                    OrderId = t.OrderId,
                                    EnrouteDeliveryStatus = t.EnrouteDeliveryStatus,
                                }).FirstOrDefault();

                                foreach (var driver in schedule.ToList())
                                {
                                    var scheduleDriver = new WhereIsMyDriverBuyerAppDriverDetails()
                                    {
                                        FirstName = driver.FirstName,
                                        DriverId = driver.DriverId ?? 0,
                                        LastName = driver.LastName,
                                        Longitude = driver.DriverLongitude,
                                        Latitude = driver.DriverLatitude,
                                        PhoneNumber = driver.PhoneNumber,
                                        ETA = driver.ETA,
                                        LastUpdatedDate = driver.AppLastUpdatedDate,
                                        IsOnline = driver.IsOnline,
                                        AllowCustomerDriverChat = driver.AllowCustomerDriverChat
                                    };
                                    driverschedule.Drivers.Add(scheduleDriver);
                                }
                                location.Schedules.Add(driverschedule);
                            }
                        }
                        response.Locations.Add(location);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("WallyBoardDomain", "GetOnGoingLoadsForBuyerAppAsync", ex.Message, ex);
            }
            return response;
        }
    }
}
