using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiteFuel.Exchange.Core.Logger;

namespace SiteFuel.Exchange.Domain
{
    public class TimeCardDomain : BaseDomain
    {
        public TimeCardDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TimeCardDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public async Task<TimeCardOutputRequestViewModel> SetTimeCardAction(TimeCardInputRequestViewModel timeCardInputViewModel)
        {
            TimeCardOutputRequestViewModel response = new TimeCardOutputRequestViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var userDetails = Context.DataContext.Users.SingleOrDefault(t => t.Id == timeCardInputViewModel.UserId);
                    if (userDetails != null)
                    {
                        var timeCardSetting = await ContextFactory.Current.GetDomain<SettingsDomain>().GetTimeCardSettings(userDetails.Company.Id);
                        if (timeCardSetting.IsTimeCardEnabled)
                        {
                            var timeCardEntry = new TimeCardEntry();
                            response.IsTimeCardEnabled = timeCardSetting.IsTimeCardEnabled;

                            if (timeCardInputViewModel.ActionId <= (int)TimeCardAction.BreakStart || timeCardInputViewModel.ActionId == (int)TimeCardAction.LunchStart ||
                                timeCardInputViewModel.ActionId == (int)TimeCardAction.FuelDeliveryStart || timeCardInputViewModel.ActionId == (int)TimeCardAction.PickUpFuelStart)
                            {
                                timeCardInputViewModel.ToEntity(timeCardEntry);
                                Context.DataContext.TimeCardEntries.Add(timeCardEntry);
                                response.CurrentActionId = timeCardEntry.ActionId;
                            }
                            else
                            {
                                var existingEntry = Context.DataContext.TimeCardEntries
                                            .Where(t => t.DriverId == timeCardInputViewModel.UserId && t.ActionEndDate == null)
                                            .OrderByDescending(t => t.CreatedDate).FirstOrDefault();

                                if (existingEntry != null)
                                {
                                    existingEntry.CreatedDate = DateTimeOffset.Now;
                                    existingEntry.ActionEndDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeCardInputViewModel.UserTimeZone);
                                    Context.DataContext.Entry(existingEntry).State = EntityState.Modified;
                                    response.CurrentActionId = (int)TimeCardAction.ClockIn;
                                }
                            }

                            await Context.CommitAsync();
                            transaction.Commit();

                            if (timeCardInputViewModel.ActionId == 2)
                            {
                                response.Summary = await GetTimeCardLastSessionSummary(timeCardInputViewModel.UserId);
                                response.CurrentActionId = 0;
                            }

                            response.StatusCode = Status.Success;
                            response.StatusMessage = Constants.Success;
                        }
                        else
                        {
                            response.StatusCode = Status.Success;
                            response.StatusMessage = Constants.TimeCardDisabledMessage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("TimeCardDomain", "SetTimeCardAction", ex.Message, ex);
                }
            }
            return response;
        }

        /// <summary>
        /// Get action summary of last/current session only
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<List<TimeCardActionSummary>> GetTimeCardLastSessionSummary(int userId)
        {
            var response = new List<TimeCardActionSummary>();
            try
            {
                var timeCardEntries = await (from entries in Context.DataContext.TimeCardEntries
                                             where entries.DriverId == userId
                                             orderby entries.CreatedDate descending
                                             select entries).ToListAsync();

                if (timeCardEntries != null && timeCardEntries.Count > 0)
                {
                    //get index of first clock in
                    var lastClockInIndex = timeCardEntries.FirstOrDefault(t => t.ActionId == (int)TimeCardAction.ClockIn);
                    if (lastClockInIndex != null)
                    {
                        timeCardEntries = timeCardEntries.Take(timeCardEntries.IndexOf(lastClockInIndex) + 1).ToList();
                        if (timeCardEntries != null && timeCardEntries.Count > 0)
                        {
                            foreach (var item in timeCardEntries)
                            {
                                var actionSummary = new TimeCardActionSummary();
                                actionSummary.ActionId = item.ActionId;
                                switch (item.ActionId)
                                {
                                    case (int)TimeCardAction.ClockIn:
                                        actionSummary.ActionName = Resource.lblClockIn;
                                        break;
                                    case (int)TimeCardAction.BreakStart:
                                    case (int)TimeCardAction.BreakEnd:
                                        actionSummary.ActionName = Resource.lblBreak;
                                        break;
                                    case (int)TimeCardAction.LunchStart:
                                    case (int)TimeCardAction.LunchEnd:
                                        actionSummary.ActionName = Resource.lblLunch;
                                        break;
                                    case (int)TimeCardAction.FuelDeliveryStart:
                                    case (int)TimeCardAction.FuelDeliveryEnd:
                                        actionSummary.ActionName = Resource.lblFuelDelivery;
                                        break;
                                    case (int)TimeCardAction.PickUpFuelStart:
                                    case (int)TimeCardAction.PickUpFuelEnd:
                                        actionSummary.ActionName = Resource.lblPickUpFuel;
                                        break;
                                    case (int)TimeCardAction.ClockOut:
                                        actionSummary.ActionName = Resource.lblClockOut;
                                        break;
                                }

                                actionSummary.StartTime = null;
                                actionSummary.EndTime = null;

                                actionSummary.UtcStartTime = item.ActionStartDate.HasValue ? item.ActionStartDate.Value.ToUnixTimeMilliseconds() : 0;
                                actionSummary.UtcEndTime = item.ActionEndDate.HasValue ? item.ActionEndDate.Value.ToUnixTimeMilliseconds() : 0;

                                if (item.ActionStartDate.HasValue && item.ActionEndDate.HasValue)
                                {
                                    var duration = item.ActionEndDate.Value.DateTime.Subtract(item.ActionStartDate.Value.DateTime);
                                    actionSummary.Duration = duration.Duration().GetDurationInHoursAndMinutes();
                                }
                                else
                                {
                                    actionSummary.Duration = string.Empty;
                                }

                                response.Add(actionSummary);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TimeCardDomain", "GetTimeCardAction", ex.Message, ex);
            }
            return response;
        }

        public async Task<TimeCardOutputRequestViewModel> GetTimeCardAction(TimeCardInputRequestViewModel timeCardInputViewModel)
        {
            TimeCardOutputRequestViewModel response = new TimeCardOutputRequestViewModel();
            try
            {
                //check if time card is enabled by supplier admin
                var timeCardEntry = await Context.DataContext.TimeCardEntries
                                    .Where(t => t.DriverId == timeCardInputViewModel.UserId)
                                    .OrderByDescending(t => t.CreatedDate).FirstOrDefaultAsync();

                var userdetails = Context.DataContext.Users.SingleOrDefault(t => t.Id == timeCardInputViewModel.UserId);
                if (userdetails != null)
                {
                    var timeCardSetting = await ContextFactory.Current.GetDomain<SettingsDomain>().GetTimeCardSettings(userdetails.Company.Id);
                    if (timeCardEntry != null)
                    {
                        response = timeCardEntry.ToViewModel(response);
                    }
                    response.IsTimeCardEnabled = timeCardSetting.IsTimeCardEnabled;
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Constants.Success;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TimeCardDomain", "GetTimeCardAction", ex.Message, ex);
            }
            return response;
        }

        public async Task<TimeCardOutputRequestViewModel> CheckCurrentAction(TimeCardInputRequestViewModel timeCardInputViewModel)
        {
            TimeCardOutputRequestViewModel response = new TimeCardOutputRequestViewModel();

            try
            {
                var timeCardEntry = await Task.Run(() => Context.DataContext.TimeCardEntries.Where(t => t.DriverId == timeCardInputViewModel.UserId).OrderByDescending(t => t.CreatedDate).FirstOrDefault());
                if (timeCardEntry != null)
                {
                    response.CurrentActionId = timeCardEntry.ActionId;
                    response.CurrentActionName = timeCardEntry.MstTimeCardAction.Name;

                    if (timeCardEntry.ActionId == timeCardInputViewModel.ActionId)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Constants.Success;
                    }
                    else
                    {
                        response.StatusCode = Status.Failed;
                        //??
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TimeCardDomain", "CheckCurrentAction", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<TimeCardActionSummary>> GetTimeCardGridDataAsync(int companyId, int driverId, string startDate, string endDate, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            var response = await GetTimeCardActionSummaryForDriverAsync(driverId, startDate, endDate, currency, countryId);
            return response;
        }

        private async Task<List<TimeCardActionSummary>> GetTimeCardActionSummaryForDriverAsync(int userId, string startDate, string endDate, Currency currency, int countryId)
        {
            var response = new List<TimeCardActionSummary>();
            try
            {
                var start = startDate.GetFilterStartDateInDateTimeOffset();
                var end = endDate.GetFilterEndDateInDateTimeOffset();
                var helperDomain = ContextFactory.Current.GetDomain<HelperDomain>();

                var timeCardEntries = await (from entries in Context.DataContext.TimeCardEntries
                                             where entries.DriverId == userId && ((entries.ActionStartDate != null && entries.ActionStartDate >= start && entries.ActionStartDate <= end)
                                             || (entries.ActionStartDate == null && entries.ActionEndDate != null && entries.ActionEndDate >= start && entries.ActionEndDate <= end)
                                             || (entries.ActionStartDate == null && entries.ActionEndDate == null && entries.CreatedDate >= start && entries.CreatedDate <= end))
                                             orderby entries.ActionGroup descending
                                             select entries).ToListAsync();

                if (timeCardEntries != null && timeCardEntries.Count > 0)
                {
                    decimal prevLat = 0, prevLong = 0;
                    foreach (var item in timeCardEntries)
                    {
                        var actionSummary = new TimeCardActionSummary();
                        actionSummary.Id = item.Id;
                        actionSummary.ActionId = item.ActionId;
                        switch (item.ActionId)
                        {
                            case (int)TimeCardAction.ClockIn:
                                actionSummary.ActionName = Resource.lblClockIn;
                                break;
                            case (int)TimeCardAction.BreakStart:
                            case (int)TimeCardAction.BreakEnd:
                                actionSummary.ActionName = Resource.lblBreak;
                                break;
                            case (int)TimeCardAction.LunchStart:
                            case (int)TimeCardAction.LunchEnd:
                                actionSummary.ActionName = Resource.lblLunch;
                                break;
                            case (int)TimeCardAction.FuelDeliveryStart:
                            case (int)TimeCardAction.FuelDeliveryEnd:
                                actionSummary.ActionName = Resource.lblFuelDelivery;
                                break;
                            case (int)TimeCardAction.PickUpFuelStart:
                            case (int)TimeCardAction.PickUpFuelEnd:
                                actionSummary.ActionName = Resource.lblPickUpFuel;
                                break;
                            case (int)TimeCardAction.ClockOut:
                                actionSummary.ActionName = Resource.lblClockOut;
                                break;
                        }
                        DateTimeOffset? eventDate;
                        if (item.ActionStartDate.HasValue)
                            eventDate = item.ActionStartDate.Value;
                        else if (item.ActionEndDate.HasValue)
                            eventDate = item.ActionEndDate.Value;
                        else
                            eventDate = item.CreatedDate;

                        actionSummary.ActionDate = eventDate.Value.DateTime.Date.ToString(Resource.constFormatDate);
                        actionSummary.StartTime = item.ActionStartDate.HasValue ? item.ActionStartDate.Value.GetTimeInHhMmFormat() : Resource.lblHyphen;
                        actionSummary.EndTime = item.ActionEndDate.HasValue ? item.ActionEndDate.Value.GetTimeInHhMmFormat() : Resource.lblHyphen;

                        actionSummary.UtcActionDate = eventDate.Value.ToUnixTimeMilliseconds();
                        actionSummary.UtcStartTime = item.ActionStartDate.HasValue ? item.ActionStartDate.Value.ToUnixTimeMilliseconds() : 0;
                        actionSummary.UtcEndTime = item.ActionEndDate.HasValue ? item.ActionEndDate.Value.ToUnixTimeMilliseconds() : 0;

                        if (item.ActionStartDate.HasValue && item.ActionEndDate.HasValue)
                        {
                            var duration = item.ActionEndDate.Value.DateTime.Subtract(item.ActionStartDate.Value.DateTime);
                            actionSummary.Duration = duration.Duration().GetDurationInHoursAndMinutes();
                        }
                        if (string.IsNullOrEmpty(actionSummary.Duration))
                        {
                            actionSummary.Duration = Resource.lblHyphen;
                        }
                        actionSummary.Distance = string.Format($"{0} {Resource.lblMiles}");
                        actionSummary.UserLocation = item.UserLocation;
                        if (prevLat != 0 && prevLong != 0)
                        {
                            var distance = helperDomain.CalculateDistance(prevLat, prevLong, item.Latitude, item.Longitude);
                            actionSummary.Distance = string.Format($"{distance.GetPreciseValue(2)} {Resource.lblMiles}");
                        }
                        prevLat = item.Latitude; prevLong = item.Longitude;
                        response.Add(actionSummary);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TimeCardDomain", "GetTimeCardActionSummaryForDriverAsync", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<USP_TimeCardActionSummaryForAllDrivers>> GetTimeCardActionSummaryForAllDrivers(List<int> userIds, int companyId, string startDate, string endDate, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            using (var tracer = new Tracer("TimeCardDomain", "GetTimeCardActionSummaryForAllDrivers"))
            {
                var response = new List<USP_TimeCardActionSummaryForAllDrivers>();
                try
                {
                    StringBuilder drivers = new StringBuilder();
                    string strDrivers = string.Empty;

                    foreach (var driver in userIds)
                    {
                        drivers.Append(driver).Append(",");
                    }
                    strDrivers = drivers.ToString().Substring(0, drivers.Length - 1);
                    response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetTimeCardActionSummaryForAllDrivers(companyId, strDrivers, startDate, endDate);
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("TimeCardDomain", "GetTimeCardActionSummaryForAllDrivers", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<int>> GetTimeCardEntriesAsync()
        {
            var timeCardEntries = await Context.DataContext.TimeCardEntries.Where(t => t.UserLocation == null && t.Latitude != 0 && t.Longitude != 0)
                                                                            .Select(t => t.Id).ToListAsync();
            return timeCardEntries;
        }

        public async Task UpdateUserLocationAsync(int id)
        {
            //using (var tracer = new Tracer("TimeCardDomain", "UpdateUserLocationAsync"))
            //{
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var timeCardEntry = await Context.DataContext.TimeCardEntries.SingleOrDefaultAsync(t => t.Id == id);
                    var point = GoogleApiDomain.GetAddress(Convert.ToDouble(timeCardEntry.Latitude), Convert.ToDouble(timeCardEntry.Longitude));
                    if (point != null)
                    {
                        timeCardEntry.UserLocation = $"{point.Address}, {point.City}, {point.StateCode}, {point.ZipCode}";
                        Context.DataContext.Entry(timeCardEntry).State = EntityState.Modified;
                        await Context.CommitAsync();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("TimeCardDomain", "UpdateUserLocationAsync", ex.Message, ex);
                }
            }
            //}
        }
    }
}