using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.MobileAPI.TPD;
using SiteFuel.Exchange.ViewModels.Queue;
//using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public class DispatchDomain : BaseDomain
    {
        public DispatchDomain()
            : base(ContextFactory.Current.ConnectionString)
        {
        }

        public DispatchDomain(BaseDomain domain)
            : base(domain)
        {
        }

        public DispatchViewModel GetDispatchDetails(int companyId)
        {
            using (var tracer = new Tracer("DispatchDomain", "GetDispatchDetails"))
            {
                DispatchViewModel dispatch = new DispatchViewModel();
                try
                {
                    var company = Context.DataContext.Companies.SingleOrDefault(t => t.Id == companyId);
                    if (company != null)
                    {
                        dispatch.IsTimeCardEnabled = company.IsTimeCardEnabled;
                        dispatch.Drivers = ContextFactory.Current.GetDomain<HelperDomain>().GetAllDrivers(companyId);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DispatchDomain", "GetDispatchDetails", ex.Message, ex);
                }

                return dispatch;
            }
        }

        public Task GetOrdersForRouteGroupAsync(object companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<MessageViewModel> GetDriverNotificationDetails(int userId)
        {
            var message = new MessageViewModel();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == userId);
                if (user != null)
                {
                    message.Body = string.Format(Resource.lblDriverNotificationBody, $"{user.FirstName} {user.LastName}");
                    message.Title = Resource.lblDriverNotificationTitle;
                    message.NotificationCode = (int)NotificationCode.Start;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDriverNotificationDetails", ex.Message, ex);
            }

            return message;
        }

        public async Task<ScheduleViewModel> GetDeliverySchedulesforDriversAsync(int companyId, List<int> driverIds, DateTimeOffset currentDate, string startDate, string endDate, bool isAllData, Currency currency = Currency.USD, int countryId = (int)Country.USA, string searchText = "")
        {
            using (var tracer = new Tracer("DispatchDomain", "GetDeliverySchedulesforDriversAsync"))
            {
                var schedules = new ScheduleViewModel();
                List<Usp_SchedulesforDriversGridViewModel> deliverySchedules;
                List<ScheduleMapDataViewModel> mapData = new List<ScheduleMapDataViewModel>();
                try
                {
                    string strDrivers = string.Empty;
                    if (driverIds != null)
                    {
                        strDrivers = String.Join(",", driverIds);

                        var driverschedules = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetDeliverySchedulesforDriversAsync(companyId, isAllData ? "-1" : strDrivers, startDate, endDate, currency, countryId, searchText);
                        var orderIds = driverschedules.Select(t => t.OrderId);
                        driverschedules = driverschedules.Where(t => t.ParentId == 0 || !orderIds.Contains(t.ParentId)).ToList();
                        if (driverIds != null && driverIds.Count > 0 && !isAllData)
                        {
                            mapData = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetSchedulesMapDataAsync(strDrivers);
                            mapData.ForEach(t => t.IsDriver = true);
                        }
                        deliverySchedules = driverschedules.Where(t => t.IsPastSchedule == 1).ToList();
                        var futureScheduleList = new List<Usp_SchedulesforDriversGridViewModel>();
                        var futureSchedules = driverschedules.Where(t => t.IsPastSchedule == 0);
                        deliverySchedules = deliverySchedules.Where(t => !(t.Id == null && t.TrackableScheduleId == null && futureSchedules.Any(t1 => t1.OrderId == t.OrderId))).ToList();
                        if (futureSchedules.Any())
                        {
                            DateTime scheduleStartDate = Convert.ToDateTime(startDate).Date;
                            DateTime scheduleEndDate = Convert.ToDateTime(endDate).Date;
                            var trackableSchedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDelivered())
                                                                                                .Where(t => t.Order.AcceptedCompanyId == companyId
                                                                                                            && t.IsActive
                                                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                                                            && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled
                                                                                                            && !t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active))
                                                                                                .Select(t => new { t.OrderId, t.Quantity })
                                                                                                .ToList();

                            var invoices = Context.DataContext.Invoices.Where(t => ((t.Order == null && t.User.Company.Id == companyId)
                                                                                        || (t.Order.AcceptedCompanyId == companyId))
                                                                                        && t.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active
                                                                                        && t.IsActive && !t.IsBuyPriceInvoice)
                                                                       .Select(t => new { t.OrderId, t.DroppedGallons })
                                                                       .ToList();
                            DateTimeOffset scheduleDate, maxDate;
                            decimal orderAmount, droppedGallons, remainingGallons, dropGallons = 0;
                            int datediff, scheduleDays;
                            DateTimeOffset filterEndDate = Convert.ToDateTime(endDate).Date;

                            var orders = futureSchedules.GroupBy(t => t.OrderId);
                            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
                            foreach (var order in orders)
                            {
                                futureScheduleList = new List<Usp_SchedulesforDriversGridViewModel>();
                                droppedGallons = invoices.Where(t => t.OrderId == order.Key).Sum(t => t.DroppedGallons) +
                                                    trackableSchedules.Where(t => t.OrderId == order.Key).Sum(t => t.Quantity);
                                maxDate = order.First().DeliveryEndDate ?? (order.First().JobEndDate ?? DateTimeOffset.MaxValue);
                                if (maxDate >= filterEndDate)
                                {
                                    maxDate = filterEndDate;
                                }
                                orderAmount = ((order.First().OrderClosingThreshold ?? 100) * order.First().MaxQuantity) / 100;

                                remainingGallons = orderAmount - droppedGallons;
                                if (remainingGallons > 0)
                                {
                                    var jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.ToList()[0].JobTimeZone);
                                    DateTime maxExistingTscheduleDate = jobLocationTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor - 1);
                                    foreach (var schedule in order)
                                    {
                                        dropGallons = 0;
                                        if (schedule.ScheduleTypeId == (int)DeliveryScheduleType.SpecificDates && schedule.ScheduleDate.Date > maxExistingTscheduleDate.Date)
                                        {
                                            futureScheduleList.Add(schedule);
                                        }
                                        else if (schedule.ScheduleTypeId != (int)DeliveryScheduleType.SpecificDates)
                                        {
                                            scheduleDays = trackableScheduleDomain.GetDaysToAdd(schedule.ScheduleTypeId);
                                            if ((schedule.ScheduleDate.Date < jobLocationTime.Date || (schedule.ScheduleDate.Date == jobLocationTime.Date && schedule.ScheduleEndTime < jobLocationTime.DateTime.TimeOfDay)) || schedule.ScheduleDate.Date != maxExistingTscheduleDate.Date)
                                            {
                                                datediff = Math.Abs(schedule.ScheduleDate.Subtract(maxExistingTscheduleDate).Days) % scheduleDays;
                                                scheduleDate = datediff == 0 ? maxExistingTscheduleDate.AddDays(scheduleDays) : maxExistingTscheduleDate.AddDays(scheduleDays - datediff);
                                            }
                                            else
                                            {
                                                scheduleDate = schedule.ScheduleDate;
                                            }
                                            while (dropGallons <= remainingGallons && scheduleDate.Date <= maxDate && scheduleDate.Date > maxExistingTscheduleDate.Date)
                                            {
                                                var newSchedule = new Usp_SchedulesforDriversGridViewModel(schedule);
                                                newSchedule.Date = scheduleDate.ToString(Resource.constFormatDate);
                                                newSchedule.ScheduleDate = scheduleDate;
                                                futureScheduleList.Add(newSchedule);
                                                dropGallons += newSchedule.Quantity;
                                                scheduleDate = scheduleDate.AddDays(scheduleDays);
                                            }
                                        }
                                    }
                                    decimal sum = 0;
                                    futureScheduleList = futureScheduleList.OrderBy(t => t.ScheduleDate).ToList();
                                    var finalList = (from schedule in futureScheduleList
                                                     where (sum += schedule.Quantity) <= remainingGallons
                                                     select schedule).ToList();
                                    decimal totalQuantity = finalList.Sum(t => t.Quantity);
                                    if (futureScheduleList.Count > finalList.Count && totalQuantity < remainingGallons)
                                    {
                                        finalList.Add(futureScheduleList[finalList.Count]);
                                    }
                                    deliverySchedules = deliverySchedules.Union(finalList).ToList();
                                }
                            }
                            deliverySchedules = deliverySchedules.Where(t => t.ScheduleDate.Date >= scheduleStartDate && t.ScheduleDate.Date <= scheduleEndDate)
                                            .OrderBy(t => t.ScheduleDate).ThenBy(t => t.DeliveryWindow).ToList();
                        }
                        if (driverIds.Count == 1 || isAllData)
                        {
                            var jobLocations = deliverySchedules.GroupBy(t => t.JobId, (key, g) => g.First())
                                                                .Select(t => new ScheduleMapDataViewModel()
                                                                {
                                                                    Name = t.Customer,
                                                                    Latitude = t.Latitude,
                                                                    Longitude = t.Longitude,
                                                                    IsDriver = false
                                                                });
                            mapData = mapData.Union(jobLocations).ToList();
                        }
                        mapData = mapData.Where(t => !(t.Latitude == 0 && t.Longitude == 0)).ToList();
                        schedules.MapData = mapData;
                        schedules.GridData = deliverySchedules;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DispatchDomain", "GetDeliverySchedulesforDriversAsync", ex.Message, ex);
                }

                return schedules;
            }
        }

        public async Task<List<DriverDropDetailsGridViewModel>> GetCurrentDriverDropDetailsOld(List<int> driverId, Currency currency, int countryId)
        {
            List<DriverDropDetailsGridViewModel> response = new List<DriverDropDetailsGridViewModel>();
            var helperDomain = new HelperDomain(this);
            var orderDomain = new OrderDomain(this);
            try
            {
                foreach (var currentDropItem in driverId)
                {
                    int currentDropDeliveryScheduleId = 0;
                    int currentDropOrderId = 0;

                    var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == currentDropItem);

                    var driverDetails = new DriverDropDetailsGridViewModel();
                    var applocation = user.AppLocations.OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                    if (applocation != null)
                    {
                        currentDropOrderId = applocation.OrderId.HasValue ? applocation.OrderId.Value : 0;
                        currentDropDeliveryScheduleId = applocation.DeliveryScheduleId.HasValue ? applocation.DeliveryScheduleId.Value : 0;

                        var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == currentDropOrderId && t.FuelRequest.Currency == currency && t.FuelRequest.Job.CountryId == countryId);
                        if (order != null)
                        {
                            var job = order.FuelRequest.Job;
                            var latestSchedules = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                            if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries &&
                                latestSchedules != null && latestSchedules.Any())
                            {
                                var schedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDelivered()).
                                                Where(t => t.IsActive && t.OrderId == order.Id && t.DeliveryScheduleId == currentDropDeliveryScheduleId && !t.Invoices.Any() &&
                                                t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled &&
                                                t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled && t.DriverId == user.Id).OrderByDescending(t => t.Id).FirstOrDefault();
                                if (schedule != null)
                                {
                                    driverDetails.DriverName = $"{user.FirstName} {user.LastName}";
                                    driverDetails.DriverId = user.Id;
                                    driverDetails.PhoneNumber = $"{user.PhoneNumber}";
                                    driverDetails.ScheduleDate = schedule.Date.ToString(Resource.constFormatDate);
                                    driverDetails.ScheduleTime = Convert.ToDateTime(schedule.StartTime.ToString()).ToShortTimeString() + Resource.lblSingleHyphen + Convert.ToDateTime(schedule.EndTime.ToString()).ToShortTimeString();
                                    driverDetails.PONumber = order.PoNumber;
                                    driverDetails.FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct);
                                    driverDetails.Quantity = helperDomain.GetQuantityRequested(schedule.Quantity);
                                    driverDetails.Customer = order.BuyerCompany.Name;
                                    driverDetails.DeliverySchedule = Resource.lblYes;
                                    driverDetails.Currency = order.FuelRequest.Currency;
                                    driverDetails.CountryId = job.CountryId;
                                    driverDetails.CountryCode = job.MstCountry.Code;
                                    driverDetails.Location = job.LocationType == JobLocationTypes.Various ? job.MstState.Code : job.Address + ", " + job.City + ", " + job.MstState.Code + " " + job.ZipCode;
                                    if (schedule.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Accepted)
                                    {
                                        driverDetails.Status = Resource.lblScheduled;
                                    }
                                    else
                                    {
                                        driverDetails.Status = schedule.MstDeliveryScheduleStatus.Name;
                                    }
                                    driverDetails.DeliveryScheduleId = schedule.DeliveryScheduleId;
                                    driverDetails.TrackableScheduleId = schedule.Id;
                                    driverDetails.OrderId = order.Id;
                                    driverDetails.IsFtlOrder = order.IsFTL;
                                    driverDetails.EnrouteStatus = applocation.StatusId ?? 0;
                                    driverDetails.Carrier = new CarrierViewModel();
                                    if (schedule.CarrierId.HasValue)
                                    {
                                        driverDetails.Carrier.Id = schedule.CarrierId.Value;
                                        driverDetails.Carrier.Name = schedule.Carrier.Name;
                                    }
                                    response.Add(driverDetails);
                                }
                            }
                            else
                            {
                                driverDetails.DriverName = $"{user.FirstName} {user.LastName}";
                                driverDetails.DriverId = user.Id;
                                driverDetails.PhoneNumber = $"{user.PhoneNumber}";
                                driverDetails.ScheduleDate = order.FuelRequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate);
                                driverDetails.ScheduleTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString() + Resource.lblSingleHyphen
                                                           + Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
                                driverDetails.PONumber = order.PoNumber;
                                driverDetails.FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct);
                                driverDetails.Quantity = helperDomain.GetQuantityRequested(order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity);
                                driverDetails.Customer = order.BuyerCompany.Name;

                                driverDetails.Location = job.LocationType == JobLocationTypes.Various ? job.MstState.Code : job.Address + ", " + job.City + ", " + job.MstState.Code + " " + job.ZipCode;
                                driverDetails.Status = Resource.lblOpen;
                                driverDetails.DeliverySchedule = Resource.lblNo;
                                driverDetails.DeliveryScheduleId = 0;
                                driverDetails.TrackableScheduleId = 0;
                                driverDetails.OrderId = order.Id;
                                driverDetails.IsFtlOrder = order.IsFTL;
                                driverDetails.Currency = order.FuelRequest.Currency;
                                driverDetails.CountryId = job.CountryId;
                                driverDetails.CountryCode = job.MstCountry.Code;
                                driverDetails.EnrouteStatus = applocation.StatusId ?? 0;
                                driverDetails.Carrier = new CarrierViewModel();
                                if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.CarrierId.HasValue)
                                {
                                    driverDetails.Carrier.Id = order.OrderAdditionalDetail.CarrierId.Value;
                                    driverDetails.Carrier.Name = order.OrderAdditionalDetail.Carrier.Name;
                                }

                                response.Add(driverDetails);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetCurrentDriverDropDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DriverDropDetailsGridViewModel>> GetCurrentDriverDropDetails(List<int> driverIds, Currency currency = Currency.USD, int countryId = (int)Country.USA , int companyId = 0)
        {
            List<DriverDropDetailsGridViewModel> response = new List<DriverDropDetailsGridViewModel>();
            try
            {
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var helperDomain = new HelperDomain(this);
                var dropDetails = await storedProcedureDomain.GetCurrentDriverDropDetails(driverIds, (int)currency, countryId, companyId);
                var trackableScheduleIds = dropDetails.Where(t => t.TrackableScheduleId != null).Select(t => t.TrackableScheduleId).ToList();
                var appLocations = Context.DataContext.AppLocations.Where(t => trackableScheduleIds.Contains(t.TrackableScheduleId)).ToList();
                foreach (var item in dropDetails)
                {
                    var driverDetails = new DriverDropDetailsGridViewModel();
                    driverDetails.DriverName = item.DriverName;
                    driverDetails.DriverId = item.DriverId;
                    driverDetails.PhoneNumber = item.PhoneNumber;
                    driverDetails.PONumber = item.PoNumber;
                    driverDetails.FuelType = item.FuelType;
                    driverDetails.Customer = item.BuyerCompanyName;
                    driverDetails.Currency = item.Currency;
                    driverDetails.CountryId = item.CountryId;
                    driverDetails.CountryCode = item.CountryCode;
                    driverDetails.Location = item.LocationType == JobLocationTypes.Various ? item.JobStateCode : item.JobAddress + ", " + item.JobCity + ", " + item.JobStateCode + " " + item.JobZipCode;
                    driverDetails.JobLatitude = item.JobLatitude;
                    driverDetails.JobLongitude = item.JobLongitude;
                    driverDetails.ScheduleDate = item.ScheduleDate.ToString(Resource.constFormatDate);
                    driverDetails.ScheduleTime = item.ScheduleStartTime + Resource.lblSingleHyphen + item.ScheduleEndTime;
                    driverDetails.Quantity = helperDomain.GetQuantityRequested(item.ScheduleQuantity);
                    if (item.TrackableScheduleId.HasValue)
                    {
                        driverDetails.TrackableScheduleId = item.TrackableScheduleId.Value;
                        driverDetails.DeliveryScheduleId = item.DeliveryScheduleId;
                        driverDetails.DeliverySchedule = Resource.lblYes;

                        var apploc = appLocations.FirstOrDefault(t => t.TrackableScheduleId == item.TrackableScheduleId);
                        if (apploc != null)
                            driverDetails.EnrouteStatus = apploc.StatusId.HasValue ? apploc.StatusId.Value : 0;

                        if (item.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Accepted)
                            driverDetails.Status = Resource.lblScheduled;
                        else
                            driverDetails.Status = item.DeliveryScheduleStatus;
                    }
                    else
                    {
                        driverDetails.Status = Resource.lblOpen;
                        driverDetails.DeliverySchedule = Resource.lblNo;
                        driverDetails.DeliveryScheduleId = 0;
                        driverDetails.TrackableScheduleId = 0;
                    }
                    driverDetails.OrderId = item.OrderId;
                    driverDetails.IsFtlOrder = item.IsFTL;
                    
                    driverDetails.Carrier = new CarrierViewModel();
                    driverDetails.Carrier.Id = item.CarrierId ?? 0;
                    driverDetails.Carrier.Name = item.CarrierName;
                    response.Add(driverDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetCurrentDriverDropDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DriverDropDetailsGridViewModel>> GetNextDriverDropDetailsOld(List<int> driverId, Currency currency = Currency.USD, int countryId = (int)Country.USA)
        {
            List<DriverDropDetailsGridViewModel> finalResponse = new List<DriverDropDetailsGridViewModel>();
            var helperDomain = new HelperDomain(this);
            var orderDomain = new OrderDomain(this);
            try
            {
                DateTimeOffset deliveryDate = DateTime.Now;
                string timeZoneName = string.Empty;
                foreach (var nextDropItem in driverId)
                {
                    var response = new List<DriverDropDetailsGridViewModel>();
                    var user = await Context.DataContext.Users.Include(t => t.Orders).SingleOrDefaultAsync(t => t.Id == nextDropItem);
                    if (user != null)
                    {
                        var driverAssignedOrder = user.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                        if (driverAssignedOrder != null)
                        {
                            timeZoneName = driverAssignedOrder.FuelRequest.Job.TimeZoneName;
                        }
                        else
                        {
                            var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.DriverId == nextDropItem).OrderByDescending(t => t.Id).FirstOrDefault();
                            if (trackableSchedule != null)
                            {
                                timeZoneName = trackableSchedule.Order.FuelRequest.Job.TimeZoneName;
                            }
                        }
                        deliveryDate = deliveryDate.ToTargetDateTimeOffset(timeZoneName);

                        DateTimeOffset deliveryStartDate = deliveryDate.Date;
                        DateTimeOffset deliveryEndDate = deliveryStartDate.AddDays(1);
                        var applocation = user.AppLocations.OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                        int currentDropOrderId = 0;
                        int currentDropDeliveryScheduleId = 0;
                        if (applocation != null && applocation.OrderId.HasValue)
                        {
                            currentDropOrderId = applocation.OrderId ?? applocation.OrderId.Value;
                            currentDropDeliveryScheduleId = applocation.DeliveryScheduleId ?? applocation.DeliveryScheduleId.Value;
                        }

                        var allOrders = Context.DataContext.Orders.Include(t => t.OrderDeliverySchedules)
                                                                    .Include("OrderDeliverySchedules.DeliverySchedule").Include(t => t.FuelRequest.Job)
                                                                    .Where(t => t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open
                                                                          && t.AcceptedCompanyId == user.Company.Id && t.FuelRequest.Currency == currency && t.FuelRequest.Job.CountryId == countryId);
                        var parentOrders = allOrders.Select(t => t.Id).ToList();
                        allOrders = allOrders.Where(t => t.ParentId == null || !parentOrders.Contains(t.ParentId ?? 0));

                        foreach (var order in allOrders)
                        {
                            var driverDetails = new DriverDropDetailsGridViewModel();

                            var latestSchedules = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
                            if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries &&
                               latestSchedules != null && latestSchedules.Count > 0)
                            {
                                var allschedules = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDelivered()).
                                       Where(t => t.IsActive && t.OrderId == order.Id && t.DeliveryScheduleId != currentDropDeliveryScheduleId && !t.Invoices.Any() &&
                                       t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled &&
                                       t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled && t.Date >= deliveryStartDate && t.Date < deliveryEndDate && t.DriverId == user.Id);
                                foreach (var schedule in allschedules)
                                {
                                    driverDetails.DriverName = $"{user.FirstName} {user.LastName}";
                                    driverDetails.DriverId = user.Id;
                                    driverDetails.PhoneNumber = $"{user.PhoneNumber}";
                                    driverDetails.ScheduleDate = schedule.Date.ToString(Resource.constFormatDate);
                                    driverDetails.StartTime = schedule.StartTime;
                                    driverDetails.ScheduleTime = Convert.ToDateTime(schedule.StartTime.ToString()).ToShortTimeString() + Resource.lblSingleHyphen
                                                                   + Convert.ToDateTime(schedule.EndTime.ToString()).ToShortTimeString();
                                    driverDetails.PONumber = order.PoNumber;
                                    driverDetails.FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct);
                                    driverDetails.Quantity = helperDomain.GetQuantityRequested(schedule.Quantity);
                                    driverDetails.Customer = order.BuyerCompany.Name;
                                    driverDetails.Currency = order.FuelRequest.Currency;

                                    var job = order.FuelRequest.Job;
                                    driverDetails.CountryId = job.CountryId;
                                    driverDetails.CountryCode = job.MstCountry.Code;
                                    driverDetails.Location = job.LocationType == JobLocationTypes.Various ? job.MstState.Code : job.Address + ", " + job.City + ", " + job.MstState.Code + " " + job.ZipCode;
                                    driverDetails.JobLatitude = job.Latitude;
                                    driverDetails.JobLongitude = job.Longitude;
                                    if (schedule.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Accepted)
                                    {
                                        driverDetails.Status = Resource.lblScheduled;
                                    }
                                    else
                                    {
                                        driverDetails.Status = schedule.MstDeliveryScheduleStatus.Name;
                                    }
                                    driverDetails.DeliverySchedule = Resource.lblYes;
                                    driverDetails.DeliveryScheduleId = schedule.DeliveryScheduleId;
                                    driverDetails.TrackableScheduleId = schedule.Id;
                                    driverDetails.OrderId = order.Id;
                                    driverDetails.IsFtlOrder = order.IsFTL;
                                    driverDetails.Carrier = new CarrierViewModel();
                                    if (schedule.CarrierId.HasValue)
                                    {
                                        driverDetails.Carrier.Id = schedule.CarrierId.Value;
                                        driverDetails.Carrier.Name = schedule.Carrier.Name;
                                    }

                                    response.Add(driverDetails);
                                }
                            }
                            else if (order.FuelRequest.FuelRequestDetail.StartDate.Date >= deliveryStartDate && order.FuelRequest.FuelRequestDetail.StartDate.Date < deliveryEndDate
                                    && order.OrderXDrivers.Where(t1 => t1.IsActive).Select(t1 => t1.DriverId).Contains(user.Id) && currentDropOrderId != order.Id)
                            {
                                driverDetails.DriverName = $"{user.FirstName} {user.LastName}";
                                driverDetails.DriverId = user.Id;
                                driverDetails.PhoneNumber = $"{user.PhoneNumber}";
                                driverDetails.ScheduleDate = order.FuelRequest.FuelRequestDetail.StartDate.ToString(Resource.constFormatDate);
                                driverDetails.StartTime = order.FuelRequest.FuelRequestDetail.StartTime;
                                driverDetails.ScheduleTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString() + Resource.lblSingleHyphen
                                                           + Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
                                driverDetails.PONumber = order.PoNumber;
                                driverDetails.FuelType = helperDomain.GetProductName(order.FuelRequest.MstProduct);
                                driverDetails.Quantity = helperDomain.GetQuantityRequested(order.BrokeredMaxQuantity ?? order.FuelRequest.MaxQuantity);
                                driverDetails.Customer = order.BuyerCompany.Name;
                                driverDetails.Currency = order.FuelRequest.Currency;
                                var job = order.FuelRequest.Job;
                                driverDetails.CountryCode = job.MstCountry.Code;
                                driverDetails.CountryId = job.CountryId;
                                driverDetails.Location = job.Address + ", " + job.City + ", " + job.MstState.Code + " " + job.ZipCode;
                                driverDetails.JobLatitude = job.Latitude;
                                driverDetails.JobLongitude = job.Longitude;
                                driverDetails.Status = Resource.lblOpen;
                                driverDetails.DeliverySchedule = Resource.lblNo;
                                driverDetails.DeliveryScheduleId = 0;
                                driverDetails.TrackableScheduleId = 0;
                                driverDetails.OrderId = order.Id;
                                driverDetails.IsFtlOrder = order.IsFTL;
                                driverDetails.Carrier = new CarrierViewModel();
                                if (order.OrderAdditionalDetail != null && order.OrderAdditionalDetail.CarrierId.HasValue)
                                {
                                    driverDetails.Carrier.Id = order.OrderAdditionalDetail.CarrierId.Value;
                                    driverDetails.Carrier.Name = order.OrderAdditionalDetail.Carrier.Name;
                                }

                                response.Add(driverDetails);
                            }
                        }

                        if (response.Count > 0)
                        {
                            var driverDropDetails = response.OrderBy(t => t.StartTime).FirstOrDefault();
                            finalResponse.Add(driverDropDetails);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetNextDriverDropDetails", ex.Message, ex);
            }

            return finalResponse;
        }

        public async Task<List<DriverDropDetailsGridViewModel>> GetNextDriverDropDetails(List<int> driverIds, Currency currency = Currency.USD, int countryId = (int)Country.USA,int companyId = 0)
        {
            List<DriverDropDetailsGridViewModel> response = new List<DriverDropDetailsGridViewModel>();
            try
            {
                DateTimeOffset deliveryDate = DateTime.Now;
                string timeZoneName = string.Empty;
                var driverId = driverIds.FirstOrDefault();

                var user = await Context.DataContext.Users.Include(t => t.Orders).SingleOrDefaultAsync(t => t.Id == driverId);
                if (user != null)
                {
                    var driverAssignedOrder = user.Orders.OrderByDescending(t => t.Id).FirstOrDefault();
                    if (driverAssignedOrder != null)
                    {
                        timeZoneName = driverAssignedOrder.FuelRequest.Job.TimeZoneName;
                    }
                }
                deliveryDate = deliveryDate.ToTargetDateTimeOffset(timeZoneName);
                var storedProcedureDomain = new StoredProcedureDomain(this);
                var helperDomain = new HelperDomain(this);
                var dropDetails = await storedProcedureDomain.GetNextDriverDropDetails(driverIds, (int)currency, countryId, companyId, deliveryDate);
                foreach (var item in dropDetails)
                {
                    var driverDetails = new DriverDropDetailsGridViewModel();
                    driverDetails.DriverName = item.DriverName;
                    driverDetails.DriverId = item.DriverId;
                    driverDetails.PhoneNumber = item.PhoneNumber;
                    driverDetails.PONumber = item.PoNumber;
                    driverDetails.FuelType = item.FuelType;
                    driverDetails.Customer = item.BuyerCompanyName;
                    driverDetails.Currency = item.Currency;
                    driverDetails.CountryId = item.CountryId;
                    driverDetails.CountryCode = item.CountryCode;
                    driverDetails.Location = item.LocationType == JobLocationTypes.Various ? item.JobStateCode : item.JobAddress + ", " + item.JobCity + ", " + item.JobStateCode + " " + item.JobZipCode;
                    driverDetails.JobLatitude = item.JobLatitude;
                    driverDetails.JobLongitude = item.JobLongitude;
                    driverDetails.ScheduleDate = item.ScheduleDate.ToString(Resource.constFormatDate);
                    driverDetails.ScheduleTime = item.ScheduleStartTime + Resource.lblSingleHyphen + item.ScheduleEndTime;
                    driverDetails.Quantity = helperDomain.GetQuantityRequested(item.ScheduleQuantity);
                    if (item.TrackableScheduleId.HasValue)
                    {
                        driverDetails.TrackableScheduleId = item.TrackableScheduleId.Value;
                        driverDetails.DeliveryScheduleId = item.DeliveryScheduleId;
                        driverDetails.DeliverySchedule = Resource.lblYes;

                        if (item.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Accepted)
                            driverDetails.Status = Resource.lblScheduled;
                        else
                            driverDetails.Status = item.DeliveryScheduleStatus;
                    }
                    else
                    {
                        driverDetails.Status = Resource.lblOpen;
                        driverDetails.DeliverySchedule = Resource.lblNo;
                        driverDetails.DeliveryScheduleId = 0;
                        driverDetails.TrackableScheduleId = 0;
                    }
                    driverDetails.OrderId = item.OrderId;
                    driverDetails.IsFtlOrder = item.IsFTL;
                    driverDetails.EnrouteStatus = item.AppLocationStatusId ?? 0;
                    driverDetails.Carrier = new CarrierViewModel();
                    driverDetails.Carrier.Id = item.CarrierId ?? 0;
                    driverDetails.Carrier.Name = item.CarrierName;
                    response.Add(driverDetails);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetNextDriverDropDetails", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<DriverLocationViewModel>> GetDriverLocationAsync(List<int> driverId, int enrouteStatus)
        {
            List<DriverLocationViewModel> response = new List<DriverLocationViewModel>();
            try
            {
                var spDomain = new StoredProcedureDomain(this);
                response = await spDomain.GetDistpatchDriverLocationsAsync(driverId, enrouteStatus);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDriverLocationAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryDetailsViewModel> GetDriverDetailsAsync(int driverId)
        {
            var response = new DeliveryDetailsViewModel();
            try
            {
                var user = await Context.DataContext.Users.SingleOrDefaultAsync(t => t.Id == driverId);
                if (user != null)
                {
                    response.DriverName = $"{user.FirstName} {user.LastName}";
                    response.PhoneNumber = $"{user.PhoneNumber}";

                    var applocation = user.AppLocations.OrderByDescending(t => t.UpdatedDate).FirstOrDefault();
                    if (applocation != null)
                    {
                        response.DriverLatitude = applocation.Latitude;
                        response.DriverLongitude = applocation.Longitude;
                        if (applocation.OrderId.HasValue)
                        {
                            var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == applocation.OrderId.Value);
                            if (order != null)
                            {
                                response.JobLatitude = order.FuelRequest.Job.Latitude;
                                response.JobLongitude = order.FuelRequest.Job.Longitude;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDriverDetailsAsync", ex.Message, ex);
            }
            return response;
        }

        //Remove Current Drop when Supplier Reassign,Reschedule and Cancels Delivery Schedule
        public async Task<StatusViewModel> RemoveOnMyWay(int driverId, int? orderId, int statusId, int? trackableScheduleId, int deliveryScheduleId = 0)
        {
            var response = new StatusViewModel();
            try
            {
                var appLocation = Context.DataContext.AppLocations.Where(x => x.UserId == driverId).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                if (appLocation != null)
                {
                    var enrouteViewModel = new EnrouteDeliveryViewModel();
                    enrouteViewModel.UserId = driverId;
                    enrouteViewModel.OrderId = orderId;
                    enrouteViewModel.DeliveryScheduleId = deliveryScheduleId;
                    enrouteViewModel.TrackableScheduleId = trackableScheduleId;
                    enrouteViewModel.StatusId = statusId;

                    var enrouteDeliveryHistory = enrouteViewModel.ToEntity();

                    using (var transaction = Context.DataContext.Database.BeginTransaction())
                    {
                        appLocation.OrderId = null;
                        appLocation.DeliveryScheduleId = null;
                        appLocation.TrackableScheduleId = null;
                        appLocation.StatusId = statusId;

                        Context.DataContext.Entry(appLocation).State = EntityState.Modified;
                        await Context.CommitAsync();

                        Context.DataContext.EnrouteDeliveryHistories.Add(enrouteDeliveryHistory);

                        await Context.CommitAsync();
                        transaction.Commit();

                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessageSuccess;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Dispatch", "RemoveOnMyWay", ex.Message, ex);
            }

            return response;
        }

        public async Task<List<UspCompletedDeliveriesViewModel>> GetCompletedDeliveriesForSupplierAsync(int companyId, List<int> driverIds, string startDate, string endDate, Currency currency = Currency.None, int countryId = (int)Country.All)
        {
            using (var tracer = new Tracer("DispatchDomain", "GetCompletedDeliveriesForSupplierAsync"))
            {
                List<UspCompletedDeliveriesViewModel> deliveries = new List<UspCompletedDeliveriesViewModel>();
                try
                {
                    string strDrivers = string.Empty;
                    if (driverIds != null)
                    {
                        strDrivers = String.Join(",", driverIds);
                    }
                    else
                    {
                        strDrivers = "-1";
                    }
                    deliveries = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetCompletedDeliveriesForSupplierAsync(companyId, strDrivers, startDate, endDate, currency, countryId);
                    deliveries.ForEach(t => t.DisplayUoM = t.UoM.ToString());
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("DispatchDomain", "GetCompletedDeliveriesForSupplierAsync", ex.Message, ex);
                }

                return deliveries;
            }
        }

        public async Task<StatusViewModel> ReassignDriver(UserContext userContext, int orderId, Nullable<int> scheduleId, Nullable<int> tscheduleId, Nullable<int> driverId, int previousDriver)
        {
            using (var tracer = new Tracer("DispatchDomain", "ReassignDriver"))
            {
                StatusViewModel response = new StatusViewModel();

                try
                {
                    if (driverId == -1)
                    {
                        driverId = null;
                    }
                    if (scheduleId == null || scheduleId == 0)
                    {
                        OrderDomain orderDomain = new OrderDomain(this);
                        await orderDomain.AssignDriverToOrder(userContext, orderId, driverId);

                        if (previousDriver != -1)
                        {
                            var dispatchDomain = new DispatchDomain(this);
                            await dispatchDomain.RemoveOnMyWay(previousDriver, orderId, (int)EnrouteDeliveryStatus.Reassigned, tscheduleId, scheduleId.HasValue ? scheduleId.Value : 0);
                        }
                    }
                    else
                    {
                        HelperDomain helperDomain = new HelperDomain(this);
                        using (var transaction = Context.DataContext.Database.BeginTransaction())
                        {
                            try
                            {
                                var deliverySchedule = await Context.DataContext.DeliverySchedules.SingleOrDefaultAsync(t => t.Id == scheduleId);
                                var order = await Context.DataContext.Orders.SingleOrDefaultAsync(t => t.Id == orderId);
                                var deliveryschedules = Context.DataContext.DeliverySchedules.Where(t => t.GroupId == deliverySchedule.GroupId);
                                var trackableSchedule = deliverySchedule.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == tscheduleId);
                                if (tscheduleId == null || trackableSchedule.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates)
                                {
                                    foreach (var schedule in deliveryschedules)
                                    {
                                        helperDomain.AssignDeliveryLevelDriver(schedule, userContext.Id, driverId, orderId, true);
                                    }
                                }
                                else if (tscheduleId != null && trackableSchedule != null && trackableSchedule.DriverId != driverId)
                                {
                                    trackableSchedule.DriverId = driverId;
                                    Context.Commit();
                                }
                                transaction.Commit();

                                var newsfeedDomain = new NewsfeedDomain(this);
                                if (driverId != null && previousDriver != -1)
                                {
                                    await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.SupplierDeliveryDriverReassigned);
                                }
                                if (driverId == null && previousDriver != -1)
                                {
                                    await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.SupplierDeliveryDriverUnassigned);
                                }
                                if (driverId != null && previousDriver == -1)
                                {
                                    await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, NewsfeedEvent.SupplierDeliveryDriverAssigned);
                                }

                                var notificationDomain = new PushNotificationDomain(this);
                                response = await notificationDomain.PushNotificationReassignDriver(order, deliverySchedule, driverId, previousDriver);
                                if (response.StatusCode == Status.Success)
                                {
                                    response.StatusMessage = Resource.errMessageNotificationSentSuccess;
                                }
                                else if (response.StatusMessage != Resource.errMessageDriverNotLoggedInApp)
                                {
                                    response.StatusMessage = Resource.errMessageNotificationSentFailed;
                                }

                                if (previousDriver != -1)
                                {
                                    var dispatchDomain = new DispatchDomain(this);
                                    await dispatchDomain.RemoveOnMyWay(previousDriver, orderId, (int)EnrouteDeliveryStatus.Reassigned, tscheduleId, deliverySchedule.Id);
                                }
                            }
                            catch
                            {
                                transaction.Rollback();
                            }
                        }
                    }

                    //Send response
                    response.StatusCode = Status.Success;
                }
                catch (Exception ex)
                {
                    response.StatusMessage = scheduleId == null ? Resource.errMessageAssignDrivertoOrderFailed : Resource.errMessageAssignDrivertoScheduleFailed;

                    LogManager.Logger.WriteException("DispatchDomain", "ReassignDriver", ex.Message, ex);
                }

                return response;
            }
        }

        public RescheduleDeliveryViewModel GetDeliveryScheduleByTrackableScheduleId(int trackableScheduleId)
        {
            RescheduleDeliveryViewModel deliverySchedule = new RescheduleDeliveryViewModel();
            try
            {
                var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => t.Id == trackableScheduleId).Select(t => new
                {
                    t.DeliveryScheduleId,
                    t.Id,
                    t.OrderId,
                    t.Date,
                    t.StartTime,
                    t.EndTime,
                    t.Quantity,
                    t.DriverId,
                    t.LoadCode,
                    t.SupplierContract,
                    t.SupplierSourceId,
                    TimeZoneName = t.Order.FuelRequest.Job.TimeZoneName,
                    IsFTL = t.Order.IsFTL,
                    Carrier = t.Carrier,
                    FrEndDate = t.Order.FuelRequest.FuelRequestDetail.EndDate,
                    JobEndDate = t.Order.FuelRequest.Job.EndDate,
                    SupplierSourceName = t.SupplierSourceId.HasValue ? t.SupplierSource.Name : "",
                    ScheduleQuantityType = t.QuantityTypeId == null ? ScheduleQuantityType.Quantity : (ScheduleQuantityType)t.QuantityTypeId,
                }).SingleOrDefault();

                if (trackableSchedule != null)
                {
                    deliverySchedule.ScheduleId = trackableSchedule.DeliveryScheduleId;
                    deliverySchedule.TrackableScheduleId = trackableSchedule.Id;
                    deliverySchedule.OrderId = trackableSchedule.OrderId ?? 0;
                    deliverySchedule.DeliveryDate = trackableSchedule.Date;
                    deliverySchedule.StartTime = $"{Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToShortTimeString()}";
                    deliverySchedule.EndTime = $"{Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToShortTimeString()}";
                    deliverySchedule.Quantity = trackableSchedule.Quantity.GetPreciseValue();
                    deliverySchedule.DriverId = trackableSchedule.DriverId;
                    deliverySchedule.JobCurrentTime = DateTimeOffset.Now.ToTargetDateTimeOffset(trackableSchedule.TimeZoneName).DateTime;
                    deliverySchedule.IsFtlOrder = trackableSchedule.IsFTL;
                    deliverySchedule.ScheduleQuantityType = trackableSchedule.ScheduleQuantityType;

                    deliverySchedule.Carrier = new CarrierViewModel();
                    if (trackableSchedule.Carrier != null)
                    {
                        deliverySchedule.Carrier.Id = trackableSchedule.Carrier.Id;
                        deliverySchedule.Carrier.Name = trackableSchedule.Carrier.Name;
                    }

                    if (trackableSchedule.FrEndDate != null)
                    {
                        deliverySchedule.FuelRequestEndDate = trackableSchedule.FrEndDate.Value.Date;
                    }
                    else if (trackableSchedule.JobEndDate != null)
                    {
                        deliverySchedule.JobEndDate = trackableSchedule.JobEndDate.Value.Date;
                    }
                    deliverySchedule.SupplierSource = new SupplierSourceViewModel()
                    {
                        ContractNumber = trackableSchedule.SupplierContract,
                        Id = trackableSchedule.SupplierSourceId,
                        Name = trackableSchedule.SupplierSourceName
                    };
                    deliverySchedule.LoadCode = trackableSchedule.LoadCode;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDeliveryScheduleByTrackableScheduleId", ex.Message, ex);
            }
            return deliverySchedule;
        }

        public async Task NotificationToDriverForScheduleModify(ScheduleEditInputViewModel inputViewModel, DeliverySchedule deliverySchedule, int terminalCurrentLocationId, string timeZone, bool isAddressChanged)
        {
            var pushNotificationDomain = new PushNotificationDomain(this);
            var viewModel = new DriverNotificationViewModel();
            try
            {
                var driverId = 0;
                string poNumber = string.Empty;
                if (inputViewModel.TrackableScheduleId.HasValue)
                {
                    var trackableSchedule = Context.DataContext.DeliveryScheduleXTrackableSchedules.SingleOrDefault(t => t.Id == inputViewModel.TrackableScheduleId.Value);
                    if (trackableSchedule != null)
                    {
                        driverId = trackableSchedule.DriverId ?? trackableSchedule.DriverId.Value;
                        poNumber = trackableSchedule.Order.PoNumber;
                    }
                }
                else
                {
                    var orderDriver = Context.DataContext.OrderXDrivers.SingleOrDefault(t => t.OrderId == inputViewModel.OrderId);
                    if (orderDriver != null)
                    {
                        driverId = orderDriver.DriverId;
                        poNumber = orderDriver.Order.PoNumber;
                    }
                }

                if (driverId > 0)
                {
                    if (terminalCurrentLocationId > 0)
                    {
                        viewModel.Message.Body = string.Format(Resource.notificationToDriver_PickUpLocationChange_Body, poNumber);
                        viewModel.Message.Title = Resource.notificationToDriver_PickUpLocationChange_Title;
                        viewModel.Message.NotificationCode = (int)NotificationCode.PickUpLocationChange;
                        viewModel.DriverIds.Add(driverId);
                        await pushNotificationDomain.NotificationToDriver(viewModel);
                    }
                    if (isAddressChanged)
                    {
                        await NotificationToDriverForSplitLoadAddressModify(inputViewModel, deliverySchedule, timeZone, pushNotificationDomain, viewModel, driverId, poNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderDomain", "NotificationToDriverForPickUpLocationChange", ex.Message, ex);
            }
        }

        public async Task<List<DropdownDisplayExtendedItem>> GetOrdersForRouteGroupAsync(int companyId)
        {
            var orders = new List<DropdownDisplayExtendedItem>();
            try
            {
                orders = await Context.DataContext.Orders.Where(t => t.AcceptedCompanyId == companyId && t.IsActive && t.OrderXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)OrderStatus.Open)
                                                        .Select(t => new DropdownDisplayExtendedItem()
                                                        {
                                                            Id = t.Id,
                                                            Code = t.FuelRequest.Job.IsRetailJob.ToString(),
                                                            Name = t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PoNumber + " | " + (t.FuelRequest.MstProduct.TfxProductId.HasValue ? t.FuelRequest.MstProduct.MstTFXProduct.Name : t.FuelRequest.MstProduct.Name) + " | " + t.BuyerCompany.Name + " | " + t.FuelRequest.Job.Name
                                                        }).OrderByDescending(t => t.Id).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetOrdersForRouteGroupAsync", ex.Message, ex);
            }

            return orders;
        }

        public async Task<List<DeliveryGroupScheduleViewModel>> GetSchedulesForRouteGroupAsync(int orderId, int includeSchedules)
        {
            var trackableSchedules = new List<DeliveryGroupScheduleViewModel>();
            try
            {
                DateTimeOffset currentDate = DateTimeOffset.Now.Date;
                DateTimeOffset endDate = currentDate.AddDays(7);
                var schedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules
                                .Where(t => t.OrderId == orderId && t.IsActive
                                        && (t.DeliveryGroupId == null)
                                        && (
                                            (includeSchedules == 1 && t.Date == currentDate)
                                            ||
                                            (includeSchedules == 2 && t.Date > currentDate && t.Date < endDate)
                                            ||
                                            (includeSchedules == 3 && t.Date >= currentDate && t.Date < endDate)
                                           )
                                      )
                    .Where(Extensions.IsOpenTrackableSchedule())
                    .Select(t => new
                    {
                        t.Id,
                        t.Date,
                        t.StartTime,
                        t.EndTime,
                        t.Quantity,
                        t.QuantityTypeId,
                        t.DeliverySchedule.UoM,
                        t.DeliveryGroupId,
                        t.Order.PoNumber,
                        OrderLevelPickupLocation = t.Order.FuelDispatchLocations.FirstOrDefault(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp && t2.TrackableScheduleId == null),
                        OrderLevelTerminalId = t.Order.TerminalId,
                        OrderLevelTerminalName = t.Order.MstExternalTerminal.Name,
                        PickupLocation = t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t3 => t3.IsActive && t3.LocationType == (int)LocationType.PickUp),
                        TerminalName = t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t3 => t3.IsActive && t3.LocationType == (int)LocationType.PickUp) != null ? t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t3 => t3.IsActive && t3.LocationType == (int)LocationType.PickUp).MstExternalTerminal.Name : string.Empty,
                        JobAddress = new { Address = t.Order.FuelRequest.Job.Address, City = t.Order.FuelRequest.Job.City, StateCode = t.Order.FuelRequest.Job.MstState.Code, ZipCode = t.Order.FuelRequest.Job.ZipCode }
                    })
                    .OrderBy(t => t.Date).ThenBy(t => t.StartTime).ToListAsync();
                schedules.ForEach(t => trackableSchedules.Add(new DeliveryGroupScheduleViewModel
                {
                    Id = t.Id,
                    DeliveryGroupId = t.DeliveryGroupId,
                    IsFutureSchedule = t.Date.Date > currentDate,
                    Name = $"{t.PoNumber} - {t.Date.Date.ToShortDateString()} - " +
                $"{ Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} " +
                $"{Resource.lblTo} {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} - " +
                $"{(t.QuantityTypeId == (int)ScheduleQuantityType.Quantity || t.QuantityTypeId == null ? t.Quantity.ToString(ApplicationConstants.DecimalFormat2) + " " + (t.UoM == UoM.Gallons ? Resource.lblGallons : Resource.lblLitres) : ((ScheduleQuantityType)t.QuantityTypeId).GetDisplayName())} - " +
                $"Drop Location: {t.JobAddress.Address}, {t.JobAddress.City}, {t.JobAddress.StateCode}, {t.JobAddress.ZipCode}",
                    PickupLocation = t.PickupLocation == null
                        ? (t.OrderLevelPickupLocation == null
                            ? new DeliveryGroupPickupViewModel() { TerminalId = t.OrderLevelTerminalId ?? 0, TerminalName = t.OrderLevelTerminalName ?? string.Empty }
                            : t.OrderLevelPickupLocation?.ToPickUpLocationViewModel(t.OrderLevelTerminalName)
                           )
                        : t.PickupLocation?.ToPickUpLocationViewModel(t.TerminalName),
                }));
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetSchedulesForRouteGroupAsync", ex.Message, ex);
            }

            return trackableSchedules;
        }

        public async Task<DeliveryGroupGridViewModel> GetSchedulesForRouteGroupEditAsync(List<int> orderIds, bool includeFutureSchedules, int deliveryGroupId)
        {
            var response = new DeliveryGroupGridViewModel();
            try
            {
                DateTimeOffset currentDate = DateTimeOffset.Now.Date;
                DateTimeOffset endDate = currentDate.AddDays(7);
                var schedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules
                                .Where(t => orderIds.Contains(t.OrderId ?? 0) && t.IsActive && t.Date < endDate
                                        && (t.DeliveryGroupId == null || t.DeliveryGroupId == deliveryGroupId)
                                      )
                                 .Where(Extensions.IsOpenMissedTrackableSchedule())
                                 .Select(t => new
                                 {
                                     t.Id,
                                     t.Date,
                                     t.StartTime,
                                     t.EndTime,
                                     t.Quantity,
                                     t.QuantityTypeId,
                                     t.DeliverySchedule.UoM,
                                     t.DeliveryGroupId,
                                     t.OrderId,
                                     t.Order.PoNumber,
                                     t.LoadCode,
                                     t.DriverId,
                                     t.DeliveryScheduleStatusId,
                                     OrderLevelPickupLocation = t.Order.FuelDispatchLocations.FirstOrDefault(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp && t2.TrackableScheduleId == null),
                                     OrderLevelTerminalId = t.Order.TerminalId,
                                     OrderLevelTerminalName = t.Order.MstExternalTerminal.Name,
                                     PickupLocation = t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t3 => t3.IsActive && t3.LocationType == (int)LocationType.PickUp),
                                     TerminalName = t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault() != null ? t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault().MstExternalTerminal.Name : string.Empty,
                                     JobAddress = new { Address = t.Order.FuelRequest.Job.Address, City = t.Order.FuelRequest.Job.City, StateCode = t.Order.FuelRequest.Job.MstState.Code, ZipCode = t.Order.FuelRequest.Job.ZipCode }
                                 })
                    .OrderBy(t => t.Date).ThenBy(t => t.StartTime).ToListAsync();

                schedules.ForEach(t => response.TrackableSchedules.Add(new DeliveryGroupScheduleViewModel
                {
                    Id = t.Id,
                    OrderId = t.OrderId.Value,
                    DeliveryGroupId = t.DeliveryGroupId,
                    IsSelected = t.DeliveryGroupId == deliveryGroupId,
                    IsFutureSchedule = t.Date.Date > currentDate,
                    Name = $"{t.PoNumber} - {t.Date.Date.ToShortDateString()} - { Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} {Resource.lblTo} {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()}" +
                            $" - { (t.QuantityTypeId == (int)ScheduleQuantityType.Quantity || t.QuantityTypeId == null ? t.Quantity.ToString(ApplicationConstants.DecimalFormat2) + " " + (t.UoM == UoM.Gallons ? Resource.lblGallons : Resource.lblLitres) : ((ScheduleQuantityType)t.QuantityTypeId).GetDisplayName())} - " +
                            $"Drop Location: {t.JobAddress.Address}, {t.JobAddress.City}, {t.JobAddress.StateCode}, {t.JobAddress.ZipCode}",
                    PickupLocation = t.PickupLocation == null
                                        ? (t.OrderLevelPickupLocation == null
                                            ? new DeliveryGroupPickupViewModel() { TerminalId = t.OrderLevelTerminalId ?? 0, TerminalName = t.OrderLevelTerminalName ?? string.Empty }
                                            : t.OrderLevelPickupLocation?.ToPickUpLocationViewModel(t.OrderLevelTerminalName)
                                           )
                                        : t.PickupLocation?.ToPickUpLocationViewModel(t.TerminalName),
                    deliverystatus = t.DeliveryScheduleStatusId
                }));

                var deliveryGroup = await Context.DataContext.DeliveryGroups.Where(t => t.Id == deliveryGroupId)
                                    .Select(t => new
                                    {
                                        t.RouteNote,
                                        t.DriverId,
                                        t.LoadCode,
                                        Pickuplocations = t.FuelDispatchLocations.Where(t1 => t1.LocationType == (int)LocationType.PickUp),
                                        TerminalName = t.FuelDispatchLocations.FirstOrDefault() != null ? t.FuelDispatchLocations.FirstOrDefault().MstExternalTerminal.Name : string.Empty
                                    }).FirstOrDefaultAsync();
                if (deliveryGroup != null)
                {
                    response.RouteNote = deliveryGroup.RouteNote;
                    response.DriverId = deliveryGroup.DriverId;
                    response.LoadCode = deliveryGroup.LoadCode;
                    if (deliveryGroup.Pickuplocations != null && deliveryGroup.Pickuplocations.Any())
                    {
                        int terminalCount = deliveryGroup.Pickuplocations.Where(t => t.TerminalId != null).Select(t => t.TerminalId).Distinct().Count();
                        int locationCount = deliveryGroup.Pickuplocations.Where(t => t.TerminalId == null).Select(t => t.SiteName).Distinct().Count();
                        response.IsCommonForGroup = terminalCount + locationCount == 1;
                        response.PickupLocation = deliveryGroup.Pickuplocations.First().ToPickUpLocationViewModel(deliveryGroup.TerminalName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetSchedulesForRouteGroupEditAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<DeliveryGroupGridViewModel> GetSchedulesForRouteGroupEditAsync(int orderId, bool includeFutureSchedules, int deliveryGroupId)
        {
            var response = new DeliveryGroupGridViewModel();
            try
            {
                DateTimeOffset currentDate = DateTimeOffset.Now.Date;
                DateTimeOffset endDate = currentDate.AddDays(7);
                var schedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules
                                .Where(t => t.OrderId == orderId && t.IsActive
                                        && (t.DeliveryGroupId == null || t.DeliveryGroupId == deliveryGroupId)
                                        && (
                                            (includeFutureSchedules && t.Date > currentDate && t.Date < endDate)
                                            ||
                                            (!includeFutureSchedules && t.Date == currentDate)
                                           )
                                      )
                                .Where(Extensions.IsOpenTrackableSchedule())
                                .Select(t => new
                                {
                                    t.Id,
                                    t.Date,
                                    t.StartTime,
                                    t.EndTime,
                                    t.Quantity,
                                    t.DeliverySchedule.UoM,
                                    t.DeliveryGroupId,
                                    t.OrderId,
                                    t.Order.PoNumber,
                                    t.LoadCode,
                                    t.DriverId,
                                    OrderLevelPickupLocation = t.Order.FuelDispatchLocations.FirstOrDefault(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp && t2.TrackableScheduleId == null),
                                    OrderLevelTerminalId = t.Order.TerminalId,
                                    OrderLevelTerminalName = t.Order.MstExternalTerminal.Name,
                                    PickupLocation = t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t3 => t3.IsActive && t3.LocationType == (int)LocationType.PickUp),
                                    TerminalName = t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault() != null ? t.DeliverySchedule.FuelDispatchLocations.FirstOrDefault().MstExternalTerminal.Name : string.Empty

                                })
                    .OrderBy(t => t.Date).ThenBy(t => t.StartTime).ToListAsync();

                schedules.ForEach(t => response.TrackableSchedules.Add(new DeliveryGroupScheduleViewModel
                {
                    Id = t.Id,
                    OrderId = t.OrderId.Value,
                    DeliveryGroupId = t.DeliveryGroupId,
                    IsSelected = t.DeliveryGroupId == deliveryGroupId,
                    IsFutureSchedule = t.Date.Date > currentDate,
                    Name = $"{t.PoNumber} - {t.Date.Date.ToShortDateString()} - { Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} {Resource.lblTo} {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} - { t.Quantity.ToString(ApplicationConstants.DecimalFormat2)} {(t.UoM == UoM.Gallons ? Resource.lblGallons : Resource.lblLitres)}",
                    PickupLocation = t.PickupLocation == null
                                        ? (t.OrderLevelPickupLocation == null
                                            ? new DeliveryGroupPickupViewModel() { TerminalId = t.OrderLevelTerminalId ?? 0, TerminalName = t.OrderLevelTerminalName ?? string.Empty }
                                            : t.OrderLevelPickupLocation?.ToPickUpLocationViewModel(t.TerminalName)
                                           )
                                        : t.PickupLocation?.ToPickUpLocationViewModel(t.TerminalName),
                }));

                var deliveryGroup = await Context.DataContext.DeliveryGroups.Where(t => t.Id == deliveryGroupId)
                        .Select(t => new
                        {
                            t.RouteNote,
                            t.DriverId,
                            t.LoadCode,
                            Pickuplocation = t.FuelDispatchLocations.FirstOrDefault(),
                            TerminalName = t.FuelDispatchLocations.FirstOrDefault() != null ? t.FuelDispatchLocations.FirstOrDefault().MstExternalTerminal.Name : string.Empty
                        }).FirstOrDefaultAsync();

                if (deliveryGroup != null)
                {
                    response.RouteNote = deliveryGroup.RouteNote;
                    response.DriverId = deliveryGroup.DriverId;
                    response.LoadCode = deliveryGroup.LoadCode;
                    if (deliveryGroup.Pickuplocation != null)
                    {
                        response.IsCommonForGroup = true;
                        response.PickupLocation = deliveryGroup.Pickuplocation.ToPickUpLocationViewModel(deliveryGroup.TerminalName);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetSchedulesForRouteGroupEditAsync", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> CreateDeliveryGroupAsync(DeliveryGroupInputViewModel viewModel, UserContext user)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {

                DeliveryGroup routeGroup = new DeliveryGroup();
                routeGroup.RouteNote = viewModel.RouteNote;
                routeGroup.CompanyId = user.CompanyId;
                routeGroup.LoadCode = viewModel.LoadCode;
                routeGroup.DriverId = viewModel.DriverId.Value;
                routeGroup.CreatedBy = routeGroup.UpdatedBy = user.Id;
                routeGroup.CreatedDate = routeGroup.UpdatedDate = DateTimeOffset.Now;
                routeGroup.IsActive = true;

                Context.DataContext.DeliveryGroups.Add(routeGroup);
                await Context.CommitAsync();

                var scheduelListFromGroup = viewModel.GroupTrackableSchedules.Select(t => t.Id).ToList();
                var trackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => scheduelListFromGroup.Contains(t.Id)).ToListAsync();

                //update existing pickuplocation as inactive
                foreach (var schedule in trackableSchedules)
                {
                    var pickLocations = schedule.DeliverySchedule.FuelDispatchLocations.ToList();
                    foreach (var location in pickLocations)
                    {
                        location.IsActive = false;
                    }
                }
                await Context.CommitAsync();

                foreach (var trackableSchedule in trackableSchedules)
                {
                    trackableSchedule.DeliveryGroupId = routeGroup.Id;
                    trackableSchedule.LoadCode = viewModel.LoadCode;
                    trackableSchedule.DriverId = viewModel.DriverId;
                    trackableSchedule.DeliverySchedule.DeliveryGroupId = routeGroup.Id;
                    SetGroupDeliveryPickupLocation(viewModel, routeGroup, trackableSchedule);
                }

                await Context.CommitAsync();

                if (viewModel.IsCommonForGroup && routeGroup.FuelDispatchLocations != null && routeGroup.FuelDispatchLocations.Any())
                {
                    var updatedDispatchLocation = routeGroup.FuelDispatchLocations.FirstOrDefault(t => t.IsActive);
                    await SaveBulkPlantLocation(updatedDispatchLocation, user.CompanyId);
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageCreatedDeliveryGroup;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "CreateDeliveryGroupAsync", ex.Message, ex);
            }

            return response;
        }

        private void SetGroupDeliveryPickupLocation(DeliveryGroupInputViewModel viewModel, DeliveryGroup routeGroup, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            var schedulepickuplocation = viewModel.GroupTrackableSchedules.SingleOrDefault(t => t.Id == trackableSchedule.Id);
            if (!viewModel.IsCommonForGroup) // saving pickup location here if it is not common for group 
            {

                if (schedulepickuplocation.PickupLocation != null && (schedulepickuplocation.PickupLocation.TerminalId.HasValue || !string.IsNullOrEmpty(schedulepickuplocation.PickupLocation.PickupZipCode)))
                {
                    if (schedulepickuplocation.PickupLocation.PickupCountryId == (int)Country.CAR && schedulepickuplocation.PickupLocation.IsMissingAddress())
                    {
                        var state = Context.DataContext.MstStates.First(t => t.Id == schedulepickuplocation.PickupLocation.PickupStateId.Value);
                        schedulepickuplocation.PickupLocation.PickupAddress = string.IsNullOrWhiteSpace(schedulepickuplocation.PickupLocation.PickupAddress) ? (state.Name ?? Resource.lblCaribbean) : schedulepickuplocation.PickupLocation.PickupAddress;
                        schedulepickuplocation.PickupLocation.PickupCity = string.IsNullOrWhiteSpace(schedulepickuplocation.PickupLocation.PickupCity) ? (state.Name ?? Resource.lblCaribbean) : schedulepickuplocation.PickupLocation.PickupCity;
                        schedulepickuplocation.PickupLocation.PickupZipCode = string.IsNullOrWhiteSpace(schedulepickuplocation.PickupLocation.PickupZipCode) ? (state.Name ?? Resource.lblCaribbean) : schedulepickuplocation.PickupLocation.PickupZipCode;
                    }
                    var schedulepickup = schedulepickuplocation.PickupLocation.ToFuelDispatchLocationEntity();
                    schedulepickup.CreatedBy = routeGroup.UpdatedBy;
                    schedulepickup.CreatedDate = routeGroup.UpdatedDate;
                    schedulepickup.TrackableScheduleId = schedulepickuplocation.Id;
                    schedulepickup.OrderId = trackableSchedule.OrderId;
                    schedulepickup.DeliveryScheduleId = trackableSchedule.DeliveryScheduleId;
                    SetPickuplocationDetails(schedulepickup);
                    trackableSchedule.DeliverySchedule.FuelDispatchLocations.Add(schedulepickup);
                }
            }
            else
            {
                if (viewModel.PickupLocation != null && viewModel.PickupLocation.TerminalId.HasValue || !string.IsNullOrEmpty(viewModel.PickupLocation.PickupZipCode))
                {
                    if (viewModel.PickupLocation.PickupCountryId == (int)Country.CAR && viewModel.PickupLocation.IsMissingAddress())
                    {
                        var state = Context.DataContext.MstStates.First(t => t.Id == viewModel.PickupLocation.PickupStateId.Value);
                        viewModel.PickupLocation.PickupAddress = string.IsNullOrWhiteSpace(viewModel.PickupLocation.PickupAddress) ? (state.Name ?? Resource.lblCaribbean) : viewModel.PickupLocation.PickupAddress;
                        viewModel.PickupLocation.PickupCity = string.IsNullOrWhiteSpace(viewModel.PickupLocation.PickupCity) ? (state.Name ?? Resource.lblCaribbean) : viewModel.PickupLocation.PickupCity;
                        viewModel.PickupLocation.PickupZipCode = string.IsNullOrWhiteSpace(viewModel.PickupLocation.PickupZipCode) ? (state.Name ?? Resource.lblCaribbean) : viewModel.PickupLocation.PickupZipCode;
                    }
                    var grouppickup = viewModel.PickupLocation.ToFuelDispatchLocationEntity();
                    grouppickup.CreatedBy = routeGroup.UpdatedBy;
                    grouppickup.CreatedDate = routeGroup.UpdatedDate;
                    grouppickup.TrackableScheduleId = schedulepickuplocation.Id;
                    grouppickup.DeliveryGroupId = routeGroup.Id;
                    grouppickup.OrderId = trackableSchedule.OrderId;
                    grouppickup.DeliveryScheduleId = trackableSchedule.DeliveryScheduleId;
                    SetPickuplocationDetails(grouppickup);
                    routeGroup.FuelDispatchLocations.Add(grouppickup);
                }
            }
        }

        private void SetPickuplocationDetails(FuelDispatchLocation fuelDispatchLocation)
        {
            if (fuelDispatchLocation.TerminalId.HasValue && fuelDispatchLocation.TerminalId > 0)
            {
                var terminal = Context.DataContext.MstExternalTerminals.Select(x => new
                {
                    x.Id,
                    x.Latitude,
                    x.Longitude,
                    x.CountryCode,
                    x.CountyName,
                    x.Address,
                    x.City,
                    x.Name,
                    x.StateCode,
                    x.StateId,
                    x.ZipCode,
                    x.Currency
                }).SingleOrDefault(t => t.Id == fuelDispatchLocation.TerminalId.Value);

                fuelDispatchLocation.Address = terminal.Address;
                fuelDispatchLocation.City = terminal.City;
                fuelDispatchLocation.StateId = terminal.StateId;
                fuelDispatchLocation.CountryCode = terminal.CountryCode;
                fuelDispatchLocation.StateCode = terminal.StateCode;
                fuelDispatchLocation.Latitude = terminal.Latitude;
                fuelDispatchLocation.Longitude = terminal.Longitude;
                fuelDispatchLocation.StateId = terminal.StateId;
                fuelDispatchLocation.CountyName = terminal.CountyName;
                fuelDispatchLocation.Currency = terminal.Currency;
                fuelDispatchLocation.ZipCode = terminal.ZipCode;

            }
            else
            {
                if (fuelDispatchLocation.Latitude == 0 || fuelDispatchLocation.Longitude == 0 || string.IsNullOrEmpty(fuelDispatchLocation.TimeZoneName))
                    new OrderDomain(this).UpdateFuelDispatchLocationLatLong(fuelDispatchLocation);
            }
        }

        public async Task<StatusViewModel> EditDeliveryGroupAsync(DeliveryGroupGridViewModel viewModel, UserContext user)
        {
            var response = new StatusViewModel(Status.Failed);
            try
            {
                DateTimeOffset currentDate = DateTimeOffset.Now.Date;
                var entity = await Context.DataContext.DeliveryGroups.Where(t => t.Id == viewModel.DeliveryGroupId)
                                .Select(t => new
                                {
                                    DeliveryGroup = t,
                                    t.DeliverySchedules,
                                    TrackableSchedules = t.DeliveryScheduleXTrackableSchedules,
                                    PickupLocations = t.FuelDispatchLocations
                                })
                                    .FirstOrDefaultAsync();
                entity.DeliveryGroup.RouteNote = viewModel.RouteNote;
                entity.DeliveryGroup.LoadCode = viewModel.LoadCode;
                entity.DeliveryGroup.DriverId = viewModel.DriverId.Value;
                entity.DeliveryGroup.UpdatedBy = user.Id;
                entity.DeliveryGroup.UpdatedDate = DateTimeOffset.Now;
                entity.DeliveryGroup.IsActive = true;

                entity.TrackableSchedules.Where(t => t.Date >= currentDate).ToList().ForEach(t => { t.DeliveryGroupId = null; t.LoadCode = null; t.DriverId = null; });
                entity.DeliverySchedules.ToList().ForEach(t => { t.DeliveryGroupId = null; });
                entity.PickupLocations.ToList().ForEach(t => { t.DeliveryGroupId = null; });

                var trackableScheduleList = viewModel.TrackableSchedules.Select(t => t.Id).ToList();
                var trackableSchedules = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(t => trackableScheduleList.Contains(t.Id)).ToListAsync();
                foreach (var trackableSchedule in trackableSchedules)
                {
                    trackableSchedule.DeliveryGroupId = entity.DeliveryGroup.Id;
                    trackableSchedule.LoadCode = viewModel.LoadCode;
                    trackableSchedule.DriverId = viewModel.DriverId;
                    trackableSchedule.DeliverySchedule.DeliveryGroupId = entity.DeliveryGroup.Id;
                    trackableSchedule.DeliveryStatus = (int)DriverAcknowledgementStatus.ReAcknowledgementNeeded;
                    var existingLocation = trackableSchedule.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t => t.IsActive);
                    if (existingLocation != null)
                    {
                        existingLocation.DeliveryScheduleId = null;
                        existingLocation.TrackableScheduleId = null;
                        existingLocation.DeliveryGroupId = null;
                        existingLocation.IsActive = false;
                    }
                    EditGroupDeliveryPickupLocation(viewModel, entity.DeliveryGroup, trackableSchedule);
                }
                await Context.CommitAsync();

                if (viewModel.IsCommonForGroup && entity.DeliveryGroup.FuelDispatchLocations != null && entity.DeliveryGroup.FuelDispatchLocations.Any())
                {
                    var updatedDispatchLocation = entity.DeliveryGroup.FuelDispatchLocations.FirstOrDefault(t => t.IsActive);
                    await SaveBulkPlantLocation(updatedDispatchLocation, user.CompanyId);
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageEditedDeliveryGroup;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "EditDeliveryGroupAsync", ex.Message, ex);
            }

            return response;
        }

        private void EditGroupDeliveryPickupLocation(DeliveryGroupGridViewModel viewModel, DeliveryGroup routeGroup, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            var schedulepickuplocation = viewModel.TrackableSchedules.SingleOrDefault(t => t.Id == trackableSchedule.Id);
            if (!viewModel.IsCommonForGroup)
            {
                if (schedulepickuplocation.PickupLocation != null && (schedulepickuplocation.PickupLocation.TerminalId.HasValue || !string.IsNullOrEmpty(schedulepickuplocation.PickupLocation.PickupZipCode)))
                {
                    var schedulepickup = schedulepickuplocation.PickupLocation.ToFuelDispatchLocationEntity();
                    schedulepickup.CreatedBy = routeGroup.UpdatedBy;
                    schedulepickup.CreatedDate = routeGroup.UpdatedDate;
                    schedulepickup.TrackableScheduleId = schedulepickuplocation.Id;
                    schedulepickup.OrderId = trackableSchedule.OrderId;
                    schedulepickup.DeliveryScheduleId = trackableSchedule.DeliveryScheduleId;
                    SetPickuplocationDetails(schedulepickup);
                    trackableSchedule.DeliverySchedule.FuelDispatchLocations.Add(schedulepickup);
                }
            }
            else
            {
                if (viewModel.PickupLocation != null && viewModel.PickupLocation.TerminalId.HasValue || !string.IsNullOrEmpty(viewModel.PickupLocation.PickupZipCode))
                {
                    var grouppickup = viewModel.PickupLocation.ToFuelDispatchLocationEntity();
                    grouppickup.CreatedBy = routeGroup.UpdatedBy;
                    grouppickup.CreatedDate = routeGroup.UpdatedDate;
                    grouppickup.DeliveryGroupId = routeGroup.Id;
                    grouppickup.TrackableScheduleId = schedulepickuplocation.Id;
                    grouppickup.OrderId = trackableSchedule.OrderId;
                    grouppickup.DeliveryScheduleId = trackableSchedule.DeliveryScheduleId;
                    SetPickuplocationDetails(grouppickup);
                    routeGroup.FuelDispatchLocations.Add(grouppickup);
                }
            }
        }

        public async Task<List<DeliveryGroupGridViewModel>> GetDeliveryGroupsAsync(int companyId, string searchText, int pageSize, int pageNumber, string startDate, string endDate)// fro delivery grid 
        {
            var response = new List<DeliveryGroupGridViewModel>();
            var totalCount = 0;
            searchText = searchText != null ? searchText.Trim().ToLower() : "";

            DateTime scheduleStartDate = string.IsNullOrEmpty(startDate) ? DateTimeOffset.Now.AddDays(1).Date : Convert.ToDateTime(startDate).Date;
            DateTime scheduleEndDate = string.IsNullOrEmpty(endDate) ? DateTimeOffset.Now.AddDays(1).Date : Convert.ToDateTime(endDate).Date;
            try
            {
                var deliveryGroups = await Context.DataContext.DeliveryGroups.Where(t => t.CompanyId == companyId && t.DeliveryScheduleXTrackableSchedules.Any(t1 => t1.Date >= scheduleStartDate && t1.Date <= scheduleEndDate) && t.IsActive)
                                                                                    .Select(t => new
                                                                                    {
                                                                                        t.Id,
                                                                                        t.RouteNote,
                                                                                        t.LoadCode,
                                                                                        t.DriverId,
                                                                                        t.Driver,
                                                                                        PickupDetails = t.FuelDispatchLocations.Any(t1 => t1.DeliveryGroupId == t.Id && t1.IsActive && t1.LocationType == (int)LocationType.PickUp) ? t.FuelDispatchLocations.FirstOrDefault(t1 => t1.DeliveryGroupId == t.Id && t1.IsActive && t1.LocationType == (int)LocationType.PickUp) : null,
                                                                                        PickupTerminalName = t.FuelDispatchLocations.Any(t1 => t1.DeliveryGroupId == t.Id && t1.IsActive && t1.LocationType == (int)LocationType.PickUp) ? t.FuelDispatchLocations.FirstOrDefault(t1 => t1.DeliveryGroupId == t.Id && t1.IsActive && t1.LocationType == (int)LocationType.PickUp).MstExternalTerminal.Name : null,
                                                                                        TrackableSchedules = t.DeliveryScheduleXTrackableSchedules.Where(t1 => t1.IsActive).Select(t1 => new
                                                                                        {
                                                                                            t1.Id,
                                                                                            t1.User.FirstName,
                                                                                            t1.User.LastName,
                                                                                            t1.Order.OrderDetailVersions.FirstOrDefault(t2 => t2.IsActive).PoNumber,
                                                                                            FuelType = t1.Order.FuelRequest.MstProduct.TfxProductId.HasValue ? t1.Order.FuelRequest.MstProduct.MstTFXProduct.Name : t1.Order.FuelRequest.MstProduct.Name,
                                                                                            Customer = t1.Order.BuyerCompany.Name,
                                                                                            t1.Date,
                                                                                            t1.StartTime,
                                                                                            t1.EndTime,
                                                                                            t1.Quantity,
                                                                                            t1.DeliverySchedule.UoM,
                                                                                            JobName = t1.Order.FuelRequest.Job.Name,
                                                                                            Address = t1.Order.FuelRequest.Job.Address,
                                                                                            ZipCode = t1.Order.FuelRequest.Job.ZipCode,
                                                                                            t1.QuantityTypeId,
                                                                                            t1.DeliveryStatus,
                                                                                            SchedulePickupLocation = t1.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp),
                                                                                            SchedulePickupTerminal = t1.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp) != null ? t1.DeliverySchedule.FuelDispatchLocations.FirstOrDefault(t2 => t2.IsActive && t2.LocationType == (int)LocationType.PickUp).MstExternalTerminal.Name : null
                                                                                        }).ToList()
                                                                                    })
                                                                                    .Where(t => searchText == "" || (
                                                                                                    (t.PickupTerminalName != null && t.PickupTerminalName.ToLower().Contains(searchText)) ||
                                                                                                    t.Driver.FirstName.ToLower().Contains(searchText) || t.Driver.LastName.ToLower().Contains(searchText) ||
                                                                                                    (t.Driver.FirstName.ToLower() + " " + t.Driver.LastName.ToLower()).Contains(searchText) ||
                                                                                                    t.TrackableSchedules.Any(
                                                                                                                                t1 => t1.PoNumber.ToLower().Contains(searchText) || t1.FuelType.ToLower().Contains(searchText) ||
                                                                                                                                      t1.Customer.ToLower().Contains(searchText) || t1.JobName.ToLower().Contains(searchText) ||
                                                                                                                                      t1.Quantity.ToString().Contains(searchText) ||
                                                                                                                                      (t1.Address != null && t1.Address.ToLower().Contains(searchText)) ||
                                                                                                                                      (t1.ZipCode != null && t1.ZipCode.ToLower().Contains(searchText))
                                                                                                                            ))
                                                                                     )
                                                                                     .OrderByDescending(t => t.Id)
                                                                                     .ToListAsync();
                totalCount = deliveryGroups != null ? deliveryGroups.Count : 0;
                if (pageSize != -1 && totalCount > pageSize)
                {
                    var skipCount = (pageNumber - 1) * pageSize;
                    deliveryGroups = deliveryGroups.Skip(skipCount).Take(pageSize).ToList();
                }
                if (deliveryGroups != null && deliveryGroups.Count>0)
                {
                    foreach (var deliveryGroup in deliveryGroups)
                    {
                        DeliveryGroupGridViewModel viewModel = new DeliveryGroupGridViewModel();
                        viewModel.DeliveryGroupId = deliveryGroup.Id;
                        if (!string.IsNullOrWhiteSpace(deliveryGroup.RouteNote))
                            viewModel.RouteNote = deliveryGroup.RouteNote;
                        else
                            viewModel.RouteNote = string.Empty;
                        viewModel.DriverId = deliveryGroup.DriverId;
                        viewModel.DriverName = deliveryGroup.Driver.FirstName + " " + deliveryGroup.Driver.LastName;
                        viewModel.LoadCode = deliveryGroup.LoadCode;
                        viewModel.TotalCount = totalCount;

                        if (deliveryGroup.PickupDetails != null)
                        {
                            viewModel.IsCommonForGroup = true;
                            viewModel.PickupLocation = deliveryGroup.PickupDetails.ToPickUpLocationViewModel(deliveryGroup.PickupTerminalName);
                        }

                        viewModel.TrackableSchedules = deliveryGroup.TrackableSchedules.OrderBy(t => t.Date).ThenBy(t => t.StartTime)
                            .Select(t => new DeliveryGroupScheduleViewModel()
                            {
                                Id = t.Id,
                                Name = $"{t.Date.Date.ToShortDateString()} - { Convert.ToDateTime(t.StartTime.ToString()).ToShortTimeString()} " +
                                       $"{Resource.lblTo} {Convert.ToDateTime(t.EndTime.ToString()).ToShortTimeString()} - " +
                                       $"{(t.QuantityTypeId == (int)ScheduleQuantityType.Quantity || t.QuantityTypeId == null ? t.Quantity.ToString(ApplicationConstants.DecimalFormat2) + " " + (t.UoM == UoM.Gallons ? Resource.lblGallons : Resource.lblLitres) : ((ScheduleQuantityType)t.QuantityTypeId).GetDisplayName())} - ",
                                Code = $"{t.PoNumber} | {t.FuelType} | {t.Customer} | {t.JobName}",
                                PickupLocation = t.SchedulePickupLocation?.ToPickUpLocationViewModel(t.SchedulePickupTerminal),
                                deliverystatus = t.DeliveryStatus
                            }).ToList();

                        response.Add(viewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDeliveryGroupsAsync", ex.Message, ex);
            }

            return response;
        }

        private static async Task NotificationToDriverForSplitLoadAddressModify(ScheduleEditInputViewModel inputViewModel, DeliverySchedule deliverySchedule, string timeZone, PushNotificationDomain pushNotificationDomain, DriverNotificationViewModel viewModel, int driverId, string poNumber)
        {
            var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZone);
            var deliveryScheduleDate = inputViewModel.DeliverySchedule.DeliveryDate;
            var startTime = Convert.ToDateTime(inputViewModel.DeliverySchedule.StartTime).TimeOfDay;
            var endTime = Convert.ToDateTime(inputViewModel.DeliverySchedule.EndTime).TimeOfDay;
            if (deliverySchedule.Id > 0)
            {
                deliveryScheduleDate = deliverySchedule.Date;
                startTime = deliverySchedule.StartTime;
                endTime = deliverySchedule.EndTime;
            }
            double dateDiff = deliveryScheduleDate.Date.Add(startTime).Subtract(currentDate.DateTime).TotalHours;
            if (deliveryScheduleDate.Date == currentDate.Date && dateDiff < 0)
            {
                dateDiff = deliveryScheduleDate.Date.Add(endTime).Subtract(currentDate.DateTime).TotalHours;
            }
            if (dateDiff <= 24 && dateDiff > 0)
            {
                viewModel.Message.Body = string.Format(Resource.notificationToDriver_SplitLoadChange_Body, poNumber);
                viewModel.Message.Title = Resource.notificationToDriver_SplitLoadChange_Title;
                viewModel.Message.NotificationCode = (int)NotificationCode.SplitLoadAddressChange;
                viewModel.DriverIds.Add(driverId);
                await pushNotificationDomain.NotificationToDriver(viewModel);
            }
        }

        public async Task<ScheduleEditInputViewModel> GetScheduleDetailsToEditAsync(ScheduleEditRequestViewModel viewModel)
        {
            var scheduleDetails = new ScheduleEditInputViewModel() { OrderId = viewModel.OrderId, DeliveryScheduleId = viewModel.DeliveryScheduleId, TrackableScheduleId = viewModel.TrackableScheduleId, IsFtlOrder = viewModel.IsFtlOrder };
            try
            {
                if (viewModel.IsFtlOrder)
                {
                    scheduleDetails.SplitLoadAddresses = await GetSplitLoadAddressesForSchedule(viewModel.OrderId, viewModel.TrackableScheduleId, viewModel.DeliveryScheduleId);
                    if (scheduleDetails.SplitLoadAddresses.Any())
                    {
                        scheduleDetails.IsSplitLoad = true;
                    }
                }
                scheduleDetails.TerminalDetails = await GetTerminalPickUpLocationAsync(viewModel.OrderId, viewModel.TrackableScheduleId, viewModel.DeliveryScheduleId);

                if (viewModel.TrackableScheduleId.HasValue)
                {
                    //for ltl also add terminal location
                    var deliverySchedule = ContextFactory.Current.GetDomain<DispatchDomain>().GetDeliveryScheduleByTrackableScheduleId(viewModel.TrackableScheduleId.Value);
                    scheduleDetails.DeliverySchedule = deliverySchedule;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetScheduleDetailsToEditAsync", ex.Message, ex);
            }
            return scheduleDetails;
        }

        public async Task<ScheduleEditInputViewModel> GetOrderDetailsToAddScheduleAsync(int orderId)
        {
            var scheduleDetails = new ScheduleEditInputViewModel() { OrderId = orderId };
            try
            {
                var order = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new { t.FuelRequest.FuelRequestDetail, t.OrderAdditionalDetail, t.IsFTL, t.FuelRequest.Currency, t.FuelRequest.Job.TimeZoneName, t.FuelRequest.Job.CountryId, CountryCode = t.FuelRequest.Job.MstCountry.Code, t.TerminalId, t.FuelRequest.UoM, TerminalName = t.MstExternalTerminal.Name }).FirstOrDefaultAsync();

                if (order.IsFTL)
                {
                    scheduleDetails.SplitLoadAddresses = await GetSplitLoadAddressesForSchedule(orderId, null, null);
                    if (scheduleDetails.SplitLoadAddresses.Any())
                    {
                        scheduleDetails.IsSplitLoad = true;
                    }
                }
                SetDeliveryScheduleDetails(order, scheduleDetails);
                scheduleDetails.IsFtlOrder = order.IsFTL;
                scheduleDetails.Currency = order.Currency;
                scheduleDetails.UoM = order.UoM;
                scheduleDetails.EnrouteStatus = (int)EnrouteDeliveryStatus.Unknown;
                scheduleDetails.CountryCode = order.CountryCode;
                scheduleDetails.CountryId = order.CountryId;
                SetPickupLocation(orderId, scheduleDetails);
                if (!scheduleDetails.TerminalDetails.TerminalId.HasValue)
                {
                    scheduleDetails.TerminalDetails.TerminalId = order.TerminalId;
                    scheduleDetails.TerminalDetails.TerminalName = order.TerminalName;
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetOrderDetailsToAddScheduleAsync", ex.Message, ex);
            }
            return scheduleDetails;
        }

        private void SetPickupLocation(int orderId, ScheduleEditInputViewModel scheduleDetails)
        {
            var pickupLocation = Context.DataContext.FuelDispatchLocations.FirstOrDefault(t => t.OrderId == orderId && t.DeliveryScheduleId == null && t.TrackableScheduleId == null && t.LocationType == (int)LocationType.PickUp && t.IsActive);
            if (pickupLocation != null)
            {
                if (pickupLocation.TerminalId.HasValue)
                {
                    scheduleDetails.TerminalDetails.TerminalId = pickupLocation.TerminalId;
                    scheduleDetails.TerminalDetails.TerminalName = pickupLocation.MstExternalTerminal.Name;
                }
                else
                {
                    scheduleDetails.TerminalDetails.AddressDetails.Address = pickupLocation.Address;
                    scheduleDetails.TerminalDetails.AddressDetails.AddressLine2 = pickupLocation.AddressLine2;
                    scheduleDetails.TerminalDetails.AddressDetails.AddressLine3 = pickupLocation.AddressLine3;
                    scheduleDetails.TerminalDetails.AddressDetails.City = pickupLocation.City;
                    scheduleDetails.TerminalDetails.AddressDetails.Country.Code = pickupLocation.CountryCode;
                    scheduleDetails.TerminalDetails.AddressDetails.CountyName = pickupLocation.CountyName;
                    scheduleDetails.TerminalDetails.AddressDetails.Latitude = pickupLocation.Latitude;
                    scheduleDetails.TerminalDetails.AddressDetails.Longitude = pickupLocation.Longitude;
                    scheduleDetails.TerminalDetails.AddressDetails.State.Id = pickupLocation.StateId ?? 0;
                    scheduleDetails.TerminalDetails.AddressDetails.ZipCode = pickupLocation.ZipCode;
                    scheduleDetails.TerminalDetails.AddressDetails.State.Code = pickupLocation.StateCode;
                    scheduleDetails.TerminalDetails.AddressDetails.TimeZoneName = pickupLocation.TimeZoneName;
                    scheduleDetails.TerminalDetails.AddressDetails.SiteName = pickupLocation.SiteName;
                    scheduleDetails.TerminalDetails.AddressDetails.SiteId = pickupLocation.Id;
                    scheduleDetails.TerminalDetails.IsNewLocation = true;
                }
            }
        }

        private void SetDeliveryScheduleDetails(dynamic order, ScheduleEditInputViewModel scheduleDetails)
        {
            scheduleDetails.DeliverySchedule.OrderId = scheduleDetails.OrderId;
            if (order.OrderAdditionalDetail != null)
            {
                if (order.OrderAdditionalDetail.Carrier != null)
                {
                    scheduleDetails.DeliverySchedule.Carrier.Id = order.OrderAdditionalDetail.Carrier.Id;
                    scheduleDetails.DeliverySchedule.Carrier.Name = order.OrderAdditionalDetail.Carrier.Name;
                }
                if (order.OrderAdditionalDetail.SupplierSource != null)
                {
                    scheduleDetails.DeliverySchedule.SupplierSource.ContractNumber = order.OrderAdditionalDetail.SupplierContract;
                    scheduleDetails.DeliverySchedule.SupplierSource.Id = order.OrderAdditionalDetail.SupplierSourceId;
                    scheduleDetails.DeliverySchedule.SupplierSource.Name = order.OrderAdditionalDetail.SupplierSource.Name;
                }
                scheduleDetails.DeliverySchedule.LoadCode = order.OrderAdditionalDetail.LoadCode;
            }
            scheduleDetails.DeliverySchedule.DeliveryDate = order.FuelRequestDetail.StartDate.Date;
            string timeZone = order.TimeZoneName;
            DateTime currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZone).Date;
            if (currentDate > order.FuelRequestDetail.StartDate.Date)
            {
                scheduleDetails.DeliverySchedule.DeliveryDate = currentDate;
            }
            scheduleDetails.DeliverySchedule.StartTime = $"{Convert.ToDateTime(order.FuelRequestDetail.StartTime.ToString()).ToShortTimeString()}";
            scheduleDetails.DeliverySchedule.EndTime = $"{Convert.ToDateTime(order.FuelRequestDetail.EndTime.ToString()).ToShortTimeString()}";
        }

        public async Task<DispatchTerminalViewModel> GetTerminalPickUpLocationAsync(int orderId, int? trackableScheduleId, int? deliveryScheduleId)
        {
            var response = new DispatchTerminalViewModel();

            var prevTerminals = await Context.DataContext.FuelDispatchLocations.Where(t => t.OrderId == orderId && t.LocationType == (int)LocationType.PickUp
                                        && t.IsActive).Select(x => new
                                        {
                                            x.TerminalId,
                                            TerminalName = x.MstExternalTerminal != null ? x.MstExternalTerminal.Name : "",
                                            x.Address,
                                            x.City,
                                            x.CountryCode,
                                            x.CountyName,
                                            x.Latitude,
                                            x.Longitude,
                                            x.StateId,
                                            x.ZipCode,
                                            x.StateCode,
                                            x.TimeZoneName,
                                            x.DeliveryScheduleId,
                                            x.TrackableScheduleId,
                                            x.SiteName,
                                            CountryId = x.StateId.HasValue ? x.MstState.CountryId : 0,
                                            CountryGroupId = x.StateId.HasValue ? x.MstState.CountryGroupId : 0,
                                            SiteId = x.Id
                                        }).ToListAsync();
            var prevAssignTerminal = prevTerminals.FirstOrDefault(t => t.DeliveryScheduleId == deliveryScheduleId && t.TrackableScheduleId == trackableScheduleId);
            if (prevAssignTerminal == null && prevTerminals != null)
            {
                prevAssignTerminal = prevTerminals.FirstOrDefault(t => t.DeliveryScheduleId == null && t.TrackableScheduleId == null);
            }

            if (prevAssignTerminal != null)
            {
                if (prevAssignTerminal.TerminalId.HasValue)
                {
                    response.TerminalId = prevAssignTerminal.TerminalId;
                    response.TerminalName = prevAssignTerminal.TerminalName;
                }
                else
                {
                    response.AddressDetails.Address = prevAssignTerminal.Address;
                    response.AddressDetails.City = prevAssignTerminal.City;
                    response.AddressDetails.Country.Id = prevAssignTerminal.CountryId;
                    response.AddressDetails.Country.Code = prevAssignTerminal.CountryCode;
                    response.AddressDetails.CountyName = prevAssignTerminal.CountyName;
                    response.AddressDetails.Latitude = prevAssignTerminal.Latitude;
                    response.AddressDetails.Longitude = prevAssignTerminal.Longitude;
                    response.AddressDetails.State.Id = prevAssignTerminal.StateId ?? 0;
                    response.AddressDetails.ZipCode = prevAssignTerminal.ZipCode;
                    response.AddressDetails.State.Code = prevAssignTerminal.StateCode;
                    response.AddressDetails.TimeZoneName = prevAssignTerminal.TimeZoneName;
                    response.AddressDetails.SiteName = prevAssignTerminal.SiteName;
                    response.AddressDetails.SiteId = prevAssignTerminal.SiteId;
                    response.AddressDetails.CountryGroup.Id = prevAssignTerminal.CountryGroupId ?? 0;
                    response.IsNewLocation = true;
                }
            }
            else
            {
                var terminal = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => new { t.TerminalId, t.MstExternalTerminal.Name }).FirstOrDefault();
                response.TerminalId = terminal.TerminalId;
                response.TerminalName = terminal.Name;
            }

            return response;
        }

        public async Task UpdatePickUpTerminal(ScheduleEditInputViewModel viewModel, UserContext userContext, List<FuelDispatchLocation> prevAssignTerminal, FuelDispatchLocation terminalLocation, int newDeliveryScheduleId, int newTrackableScheduleId, StatusViewModel response)
        {
            if (newDeliveryScheduleId > 0)
            {
                viewModel.DeliveryScheduleId = newDeliveryScheduleId;
                viewModel.TrackableScheduleId = newTrackableScheduleId;
            }
            var lastTerminal = prevAssignTerminal.FirstOrDefault(t => t.DeliveryScheduleId == viewModel.DeliveryScheduleId && t.TrackableScheduleId == viewModel.TrackableScheduleId);
            if (!viewModel.TerminalDetails.IsNewLocation && lastTerminal != null && lastTerminal.TerminalId.HasValue && viewModel.TerminalDetails.TerminalId == lastTerminal.TerminalId)
            {
                if (response.StatusCode == Status.Failed)
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.errMessageDeliverySchedulesNoNewChangesSaveFailed;
                }
                return;
            }
            var enrouteStatus = Context.DataContext.AppLocations.Where(t => t.OrderId == viewModel.OrderId && t.TrackableScheduleId == viewModel.TrackableScheduleId).OrderByDescending(t => t.Id).Select(t => t.StatusId).FirstOrDefault();
            if (enrouteStatus != null && !(enrouteStatus == (int)EnrouteDeliveryStatus.Unknown || enrouteStatus == (int)EnrouteDeliveryStatus.OnTheWayToTerminal || enrouteStatus == (int)EnrouteDeliveryStatus.ArrivedAtTerminal || enrouteStatus == (int)EnrouteDeliveryStatus.WaitingBeforeFuelPickup))
            {
                response.StatusCode = Status.Failed;
                response.StatusMessage = Resource.errMessageCannotUpdateTerminal;
                return;
            }
            terminalLocation = AddNewPickUpLocation(viewModel.TerminalDetails, userContext, viewModel.OrderId, viewModel.DeliveryScheduleId, viewModel.TrackableScheduleId);

            prevAssignTerminal.Where(t => t.DeliveryScheduleId == viewModel.DeliveryScheduleId && t.TrackableScheduleId == viewModel.TrackableScheduleId)
                            .ToList()
                            .ForEach(t => t.IsActive = false);

            Context.DataContext.FuelDispatchLocations.Add(terminalLocation);
            await Context.CommitAsync();
            if (terminalLocation.Id > 0)
            {
                await SaveBulkPlantLocation(terminalLocation, userContext.CompanyId);
            }
            response.StatusCode = Status.Success;
            response.StatusMessage = Resource.successMessagePickUpTerminalUpdateSuccessfully;
        }

        public List<DropdownDisplayItem> GetCarriers(string Prefix)
        {
            var carriers = new List<DropdownDisplayItem>();
            try
            {
                carriers = Context.DataContext.Carriers.Where(c => c.Name.Contains(Prefix) || Prefix == string.Empty).
                                      Select(t => new DropdownDisplayItem { Name = t.Name, Id = t.Id }).Distinct().Take(10).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetCarriers", ex.Message, ex);
            }
            return carriers;
        }
        public async Task<List<DropdownDisplayItem>> GetBulkPlants(string prefix, int companyId, int orderId = 0)
        {
            var bulkPlantNames = new List<DropdownDisplayItem>();
            try
            {
                var isAutoFreightOrder = FreightPricingMethod.Manual;

                if (orderId > 0)
                {
                    isAutoFreightOrder = Context.DataContext.OrderAdditionalDetails.Where(o => o.OrderId == orderId).Select(p => p.FreightPricingMethod).FirstOrDefault();

                    if (isAutoFreightOrder == FreightPricingMethod.Auto)
                    {
                        bulkPlantNames = await new ExternalPricingDomain().GetBulkPlantsForAutoFreightMethod(orderId, prefix);
                    }
                }

                if (isAutoFreightOrder == FreightPricingMethod.Manual)
                { 
                    bulkPlantNames = Context.DataContext.BulkPlantLocations.Where(n => (n.Name.Contains(prefix.Trim().ToLower()) || (prefix == string.Empty)) && n.CompanyId == companyId && n.IsActive)
                                                                            .OrderBy(t => t.Name)
                                                                            .Select(t => new DropdownDisplayItem { Name = t.Name, Id = t.Id }).GroupBy(t => t.Name).Select(t => t.FirstOrDefault()).ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetBulkPlants", ex.Message, ex);
            }
            return bulkPlantNames;
        }

        //for Frieghttable use
        public object GetBulkPlantsForFreight(string prefix, int companyId, int stateId)
        {
            try
            {
                var fuelBulks = Context.DataContext.BulkPlantLocations.Where(n => n.CompanyId == companyId && n.IsActive && (n.StateId == stateId || stateId == 0)
                                                                        && (n.Name.Contains(prefix.Trim().ToLower()) || (prefix == string.Empty))
                                                                         )
                                                        .Select(t => new
                                                        {
                                                            Name = t.Name,
                                                            Id = (int)t.Id,
                                                            t.Latitude,
                                                            t.Longitude,
                                                            t.ZipCode,
                                                            t.StateId,
                                                            t.CountryCode,
                                                            Address = t.Address + ", " + t.City + ", " + t.StateCode + ", " + t.ZipCode
                                                        })
                                                        .OrderBy(t => t.Name)
                                                        .ToList();
                return fuelBulks;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetBulkPlants", ex.Message + "stateId=" + stateId, ex);
            }
            return null;
        }

        public DispatchAddressViewModel GetBulkPlantDetailsByName(string bulkPlantName, int companyId)
        {
            DispatchAddressViewModel response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(bulkPlantName))
                    return null;

                response = Context.DataContext.BulkPlantLocations
                                                .Where(t => t.CompanyId == companyId && t.IsActive && t.Name.Trim().ToLower() == bulkPlantName.Trim().ToLower())
                                                .Select(x =>
                                                new DispatchAddressViewModel
                                                {
                                                    Address = x.Address,
                                                    AddressLine2 = x.AddressLine2,
                                                    AddressLine3 = x.AddressLine3,
                                                    City = x.City,
                                                    ZipCode = x.ZipCode,
                                                    State = new StateViewModel { Id = x.StateId, Code = x.StateCode },
                                                    Country = new CountryViewModel { Id = x.CountryId, Code = x.CountryCode },
                                                    CountryGroup = new DropdownDisplayExtendedItem { Id= x.MstState.CountryGroupId ?? 0 },
                                                    CountyName = x.CountyName,
                                                    SiteId = x.Id,
                                                    Latitude = x.Latitude,
                                                    Longitude = x.Longitude
                                                }).FirstOrDefault();

                if (response == null)
                {
                    response = new DispatchAddressViewModel();
                }
                if (response.Country.Code == "US")
                {
                    response.Country.Code = "USA";
                }
                else if (response.Country.Code == "CA")
                {
                    response.Country.Code = "CAN";
                }

                response.StatusCode = Status.Success;
                response.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetBulkPlantDetails", ex.Message, ex);
            }

            return response;

        }
        public List<DropdownDisplayItem> GetSupplierSource(string Prefix)
        {
            var suppliers = new List<DropdownDisplayItem>();
            try
            {
                suppliers = Context.DataContext.SupplierSources.Where(c => c.Name.Contains(Prefix) || Prefix == string.Empty).
                                      Select(t => new DropdownDisplayItem { Name = t.Name, Id = t.Id }).Distinct().Take(10).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetSupplierSource", ex.Message, ex);
            }
            return suppliers;
        }

        public async Task<StatusViewModel> ModifyDeliverySchedule(ScheduleEditInputViewModel viewModel, UserContext userContext)
        {
            var response = new StatusViewModel();
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    DeliverySchedule deliverySchedule = new DeliverySchedule();
                    bool isAddressChanged = false;
                    viewModel.UpdatedBy = userContext.Id;
                    FuelDispatchLocation terminalCurrentLocation = new FuelDispatchLocation();
                    var order = await Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId)
                                            .Select(t => new
                                            {
                                                Order = t,
                                                TimeZone = t.FuelRequest.Job.TimeZoneName,
                                                OrderTerminal = t.MstExternalTerminal.Name,
                                                trackableSchedule = t.DeliveryScheduleXTrackableSchedules.FirstOrDefault(t1 => t1.Id == viewModel.TrackableScheduleId),
                                                PrevTerminalLocation = t.FuelDispatchLocations.Where(t1 => t1.LocationType == (int)LocationType.PickUp
                                                                        && ((t1.DeliveryScheduleId == viewModel.DeliveryScheduleId
                                                                            && t1.TrackableScheduleId == viewModel.TrackableScheduleId)
                                                                            || (t1.DeliveryScheduleId == null && t1.TrackableScheduleId == null))
                                                                        && t1.IsActive).ToList()
                                            }).FirstOrDefaultAsync();

                    if (viewModel.IsModifySchedule)
                    {
                        var saveOnlyScheduleAdditionalDetail = await UpdateScheduleAdditionalDetails(viewModel.DeliverySchedule, order.trackableSchedule, userContext, response);
                        if (!saveOnlyScheduleAdditionalDetail)
                        {
                            await RescheduleDeliveryScheduleAsync(viewModel.DeliverySchedule, deliverySchedule, order.trackableSchedule, order.Order, userContext, response);
                        }
                    }
                    int newDeliveryScheduleId = 0, newTrackableScheduleId = 0;
                    if (deliverySchedule.Id > 0)
                    {
                        newDeliveryScheduleId = deliverySchedule.Id;
                        newTrackableScheduleId = deliverySchedule.DeliveryScheduleXTrackableSchedules.Select(x => x.Id).FirstOrDefault();
                    }

                    await UpdatePickUpTerminal(viewModel, userContext, order.PrevTerminalLocation, terminalCurrentLocation, newDeliveryScheduleId, newTrackableScheduleId, response);

                    if (viewModel.IsFtlOrder)
                    {
                        isAddressChanged = await UpdateFuelDispatchLocation(viewModel, newDeliveryScheduleId, newTrackableScheduleId);
                    }

                    transaction.Commit();
                    if (viewModel.IsModifySchedule)
                    {
                        await PostRescheduleEvents(viewModel.DeliverySchedule, userContext, deliverySchedule, order.Order, order.trackableSchedule);
                    }
                    if (terminalCurrentLocation.Id > 0 || isAddressChanged)
                    {
                        //await PostScheduleModifyEvents(viewModel, userContext, order.PrevTerminalLocation, terminalCurrentLocation, deliverySchedule, order.OrderTerminal, order.TimeZone, isAddressChanged);
                        await SaveBulkPlantLocation(terminalCurrentLocation, userContext.CompanyId);
                    }
                    GetModifyScheduleResponse(viewModel, response, deliverySchedule, order.TimeZone, isAddressChanged);
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("DispatchDomain", "ModifyNextDeliverySchedule", ex.Message, ex);
                }
            }

            return response;
        }

        public async Task SaveBulkPlantLocation(FuelDispatchLocation location, int companyId)
        {
            if (location != null && location.StateId > 0 && !location.TerminalId.HasValue)
            {
                var countryId = Context.DataContext.MstStates.Where(t => t.Id == location.StateId).Select(t => t.CountryId).FirstOrDefault();
                var bulkPlantDetail = location.ToBulkPlantLocationEntity(countryId, companyId);
                await SaveBulkPlantIfNotExists(bulkPlantDetail);
            }
        }
        public async Task<int> SaveOptionalPickupBulkPlantLocation(FuelDispatchLocation location, int companyId)
        {
           int bulkPlantId = 0;
            if (location != null && location.StateId > 0 && !location.TerminalId.HasValue)
            {
                var countryId = Context.DataContext.MstStates.Where(t => t.Id == location.StateId).Select(t => t.CountryId).FirstOrDefault();
                var bulkPlantDetail = location.ToBulkPlantLocationEntity(countryId, companyId);
                bulkPlantId= await SaveOptionalPickupBulkPlantIfNotExists(bulkPlantDetail);
            }
            return bulkPlantId;
        }
        public async Task SaveBulkPlantIfNotExists(BulkPlantLocation bulkPlantDetail)
        {
            var IsBulkPlantExist = Context.DataContext.BulkPlantLocations.Any(t => t.IsActive && t.Name.ToLower() == bulkPlantDetail.Name.ToLower()
                                                                                                && t.CompanyId == bulkPlantDetail.CompanyId);
            if (!IsBulkPlantExist)
            {
                Context.DataContext.BulkPlantLocations.Add(bulkPlantDetail);
                await Context.CommitAsync();
            }
        }
        public async Task<int> SaveOptionalPickupBulkPlantIfNotExists(BulkPlantLocation bulkPlantDetail)
        {
            int bulkPlantId = 0;
            try
            {
                var IsBulkPlantExist = Context.DataContext.BulkPlantLocations.FirstOrDefault(t => t.IsActive && t.Name.ToLower() == bulkPlantDetail.Name.ToLower()
                                                                                                && t.CompanyId == bulkPlantDetail.CompanyId);
            if (IsBulkPlantExist == null)
            {
                Context.DataContext.BulkPlantLocations.Add(bulkPlantDetail);
                await Context.CommitAsync();
                bulkPlantId = bulkPlantDetail.Id;
            }
            else
            {
                bulkPlantId = IsBulkPlantExist.Id;
            }
            }
            catch (Exception ex)
            {
                bulkPlantId = -1;
                LogManager.Logger.WriteException("DispatchDomain", "SaveOptionalPickupBulkPlantIfNotExists", ex.Message, ex);
            }
            return bulkPlantId;
        }

        public async Task<DeliveryGroupScheduleViewModel> AddDeliverySchedule(ScheduleEditInputViewModel viewModel, UserContext userContext)
        {
            var response = new DeliveryGroupScheduleViewModel() { OrderId = viewModel.OrderId };
            using (var transaction = Context.DataContext.Database.BeginTransaction())
            {
                try
                {
                    var order = await Context.DataContext.Orders.Where(t => t.Id == viewModel.OrderId).Select(t => new { t.TerminalId, TerminalName = t.MstExternalTerminal.Name, t.FuelRequest.Job.TimeZoneName, t.FuelRequest.Job.Address, t.FuelRequest.Job.City, StateCode = t.FuelRequest.Job.MstState.Code, t.FuelRequest.Job.ZipCode, PoNumber = t.OrderDetailVersions.FirstOrDefault(t1 => t1.IsActive).PoNumber }).FirstOrDefaultAsync();
                    var orderSchedules = await Context.DataContext.OrderVersionXDeliverySchedules.Where(t => t.OrderId == viewModel.OrderId && t.IsActive).ToListAsync();

                    int latestVersion = AddExistingSchedulesToNewVersion(viewModel.OrderId, orderSchedules);

                    DeliverySchedule scheduleEntity = GetCurrentSchedule(viewModel, userContext);
                    await SetCarrierAndSupplierSource(viewModel, userContext, scheduleEntity);
                    Context.DataContext.DeliverySchedules.Add(scheduleEntity);

                    AddCurrentScheduleToNewVersion(viewModel.OrderId, latestVersion, userContext, scheduleEntity);
                    var trackableSchedule = AddTrackableSchedule(viewModel, scheduleEntity);
                    await Context.CommitAsync();

                    FuelDispatchLocation orderPickupLocation;
                    var pickupAddress = GetPickupLocation(viewModel, userContext, order.TerminalId, out orderPickupLocation);
                    if (pickupAddress != null)
                    {
                        pickupAddress.DeliveryScheduleId = scheduleEntity.Id;
                        pickupAddress.TrackableScheduleId = trackableSchedule.Id;
                        Context.DataContext.FuelDispatchLocations.Add(pickupAddress);
                        await Context.CommitAsync();

                        if (pickupAddress.Id > 0)
                        {
                            await SaveBulkPlantLocation(pickupAddress, userContext.CompanyId);
                        }
                    }

                    viewModel.DeliveryScheduleId = scheduleEntity.Id;
                    viewModel.TrackableScheduleId = trackableSchedule.Id;
                    await UpdateFuelDispatchLocation(viewModel, 0, 0);
                    transaction.Commit();

                    ReturnResponse(response, trackableSchedule, orderPickupLocation, pickupAddress, order);
                    response.StatusCode = Status.Success;
                    response.StatusMessage = "Delivery schedule added successfully";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    LogManager.Logger.WriteException("DispatchDomain", "AddDeliverySchedule", ex.Message, ex);
                }
            }

            return response;
        }

        private static void ReturnResponse(DeliveryGroupScheduleViewModel response, DeliveryScheduleXTrackableSchedule trackableSchedule, FuelDispatchLocation orderPickupLocation, FuelDispatchLocation pickupAddress, dynamic order)
        {
            string terminalName = order.TerminalName, timeZone = order.TimeZoneName;
            response.Id = trackableSchedule.Id;
            response.IsFutureSchedule = trackableSchedule.Date.Date > DateTimeOffset.Now.ToTargetDateTimeOffset(timeZone).Date;
            response.Name = $"{order.PoNumber} - {trackableSchedule.Date.Date.ToShortDateString()} - " +
                        $"{ Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToShortTimeString()} " +
                        $"{Resource.lblTo} {Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToShortTimeString()} - " +
                        $"{ (trackableSchedule.QuantityTypeId == (int)ScheduleQuantityType.Quantity ? trackableSchedule.Quantity.ToString(ApplicationConstants.DecimalFormat2) + " " + (trackableSchedule.UoM == UoM.Gallons ? Resource.lblGallons : Resource.lblLitres) : ((ScheduleQuantityType)trackableSchedule.QuantityTypeId).GetDisplayName())} - " +
                        $"Drop Location: {order.Address}, {order.City}, {order.StateCode}, {order.ZipCode}";
            if (pickupAddress != null)
            {
                response.PickupLocation = pickupAddress.ToPickUpLocationViewModel(pickupAddress.SiteName);
            }
            else if (orderPickupLocation != null)
            {
                response.PickupLocation = orderPickupLocation.ToPickUpLocationViewModel(terminalName);
            }
            else
            {
                response.PickupLocation = new DeliveryGroupPickupViewModel() { TerminalId = order.TerminalId ?? 0, TerminalName = order.TerminalName ?? string.Empty };
            }
        }

        private FuelDispatchLocation GetPickupLocation(ScheduleEditInputViewModel viewModel, UserContext userContext, int? orderTerminalId, out FuelDispatchLocation orderPickupLocation)
        {
            PickupLocationType orderPickupAddressType = PickupLocationType.Terminal;
            PickupLocationType schedulePickupAddressType = viewModel.TerminalDetails.IsNewLocation ? PickupLocationType.BulkPlant : PickupLocationType.Terminal;

            orderPickupLocation = Context.DataContext.FuelDispatchLocations.FirstOrDefault(t => t.OrderId == viewModel.OrderId && t.DeliveryScheduleId == null && t.TrackableScheduleId == null && t.IsActive);
            if (orderPickupLocation != null && orderPickupLocation.TerminalId == null)
            {
                orderPickupAddressType = PickupLocationType.BulkPlant;
            }
            if (orderPickupAddressType != schedulePickupAddressType || (schedulePickupAddressType == PickupLocationType.Terminal && viewModel.TerminalDetails.TerminalId != orderTerminalId) || (schedulePickupAddressType == PickupLocationType.BulkPlant && orderPickupLocation!=null && viewModel.TerminalDetails.AddressDetails.SiteName != orderPickupLocation.SiteName))
            {

                if (viewModel.TerminalDetails.AddressDetails.Country.Id == (int)Country.CAR && viewModel.TerminalDetails.AddressDetails.IsMissingAddress())
                {
                    var state = Context.DataContext.MstStates.First(t => t.Id == viewModel.TerminalDetails.AddressDetails.State.Id).ToViewModel();
                    viewModel.TerminalDetails.AddressDetails.Address = string.IsNullOrWhiteSpace(viewModel.TerminalDetails.AddressDetails.Address) ? (state.Name ?? Resource.lblCaribbean) : viewModel.TerminalDetails.AddressDetails.Address;
                    viewModel.TerminalDetails.AddressDetails.City = string.IsNullOrWhiteSpace(viewModel.TerminalDetails.AddressDetails.City) ? (state.Name ?? Resource.lblCaribbean) : viewModel.TerminalDetails.AddressDetails.City;
                    viewModel.TerminalDetails.AddressDetails.CountyName = string.IsNullOrWhiteSpace(viewModel.TerminalDetails.AddressDetails.CountyName) ? (state.Name ?? Resource.lblCaribbean) : viewModel.TerminalDetails.AddressDetails.CountyName;
                    viewModel.TerminalDetails.AddressDetails.ZipCode = string.IsNullOrWhiteSpace(viewModel.TerminalDetails.AddressDetails.ZipCode) ? (state.Name ?? Resource.lblCaribbean) : viewModel.TerminalDetails.AddressDetails.ZipCode;

                    if (viewModel.TerminalDetails.AddressDetails.Latitude == 0 || viewModel.TerminalDetails.AddressDetails.Longitude == 0)
                    {
                        var point = GoogleApiDomain.GetGeocode($"{ viewModel.TerminalDetails.AddressDetails.Address} { viewModel.TerminalDetails.AddressDetails.City} {state.Code} {"CAR"} {viewModel.TerminalDetails.AddressDetails.ZipCode}");
                        if (point != null)
                        {
                            viewModel.TerminalDetails.AddressDetails.Latitude = Convert.ToDecimal(point.Latitude);
                            viewModel.TerminalDetails.AddressDetails.Longitude = Convert.ToDecimal(point.Longitude);
                        }
                    }
                }
                
                return AddNewPickUpLocation(viewModel.TerminalDetails, userContext, viewModel.OrderId, viewModel.DeliveryScheduleId, viewModel.TrackableScheduleId);
            }
            return null;
        }

        private static DeliveryScheduleXTrackableSchedule AddTrackableSchedule(ScheduleEditInputViewModel viewModel, DeliverySchedule scheduleEntity)
        {
            DeliveryScheduleXTrackableSchedule trackableSchedule = new DeliveryScheduleXTrackableSchedule()
            {
                Date = viewModel.DeliverySchedule.DeliveryDate,
                StartTime = Convert.ToDateTime(viewModel.DeliverySchedule.StartTime).TimeOfDay,
                EndTime = Convert.ToDateTime(viewModel.DeliverySchedule.EndTime).TimeOfDay,
                Quantity = viewModel.DeliverySchedule.Quantity,
                QuantityTypeId = (int)viewModel.DeliverySchedule.ScheduleQuantityType,
                IsActive = true,
                OrderId = viewModel.OrderId,
                DeliveryScheduleStatusId = (int)TrackableDeliveryScheduleStatus.Accepted,
                UoM = viewModel.UoM,
                LoadCode = viewModel.DeliverySchedule.LoadCode,
                ShiftStartDate = viewModel.DeliverySchedule.DeliveryDate
            };
            if (viewModel.DeliverySchedule.Carrier != null && viewModel.DeliverySchedule.Carrier.Id > 0)
            {
                trackableSchedule.CarrierId = viewModel.DeliverySchedule.Carrier.Id;
            }
            if (viewModel.DeliverySchedule.SupplierSource != null)
            {
                trackableSchedule.SupplierContract = viewModel.DeliverySchedule.SupplierSource.ContractNumber;
                trackableSchedule.SupplierSourceId = viewModel.DeliverySchedule.SupplierSource.Id;
            }
            scheduleEntity.DeliveryScheduleXTrackableSchedules.Add(trackableSchedule);
            return trackableSchedule;
        }

        public static void AddCurrentScheduleToNewVersion(int orderId, int latestVersion, UserContext userContext, DeliverySchedule scheduleEntity)
        {
            OrderVersionXDeliverySchedule newSchedule = new OrderVersionXDeliverySchedule()
            {
                DeliveryRequestId = scheduleEntity.Id,
                OrderId = orderId,
                Version = latestVersion + 1,
                CreatedBy = userContext.Id,
                CreatedDate = DateTimeOffset.Now,
                IsActive = true
            };
            scheduleEntity.OrderVersionXDeliverySchedules.Add(newSchedule);
        }

        private async Task SetCarrierAndSupplierSource(ScheduleEditInputViewModel viewModel, UserContext userContext, DeliverySchedule scheduleEntity)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.DeliverySchedule.Carrier.Name))
            {
                viewModel.DeliverySchedule.Carrier = await AddCarrierIfNotExists(viewModel.DeliverySchedule.Carrier.Name, userContext.Id, userContext.CompanyId);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.DeliverySchedule.SupplierSource.Name))
            {
                viewModel.DeliverySchedule.SupplierSource = await AddSupplierSourceIfNotExists(viewModel.DeliverySchedule.SupplierSource, userContext.Id, userContext.CompanyId);
            }
            if (viewModel.DeliverySchedule.Carrier != null && viewModel.DeliverySchedule.Carrier.Id > 0)
            {
                scheduleEntity.CarrierId = viewModel.DeliverySchedule.Carrier.Id;
            }
            if (viewModel.DeliverySchedule.SupplierSource != null)
            {
                scheduleEntity.SupplierContract = viewModel.DeliverySchedule.SupplierSource.ContractNumber;
                scheduleEntity.SupplierSourceId = viewModel.DeliverySchedule.SupplierSource.Id;
            }
        }

        private DeliverySchedule GetCurrentSchedule(ScheduleEditInputViewModel viewModel, UserContext userContext)
        {
            int latestGroupNumber = 0;
            if (Context.DataContext.DeliverySchedules.Any())
            {
                latestGroupNumber = Context.DataContext.DeliverySchedules.Max(t => t.GroupId);
            }
            HelperDomain helperDomain = new HelperDomain(this);
            DeliverySchedule scheduleEntity = new DeliverySchedule()
            {
                Type = (int)DeliveryScheduleType.SpecificDates,
                WeekDayId = helperDomain.GetWeekDayId(viewModel.DeliverySchedule.DeliveryDate),
                Date = viewModel.DeliverySchedule.DeliveryDate,
                StartTime = Convert.ToDateTime(viewModel.DeliverySchedule.StartTime).TimeOfDay,
                EndTime = Convert.ToDateTime(viewModel.DeliverySchedule.EndTime).TimeOfDay,
                Quantity = viewModel.DeliverySchedule.Quantity,
                QuantityTypeId = (int)viewModel.DeliverySchedule.ScheduleQuantityType,
                CreatedBy = userContext.Id,
                StatusId = (int)DeliveryScheduleStatus.New,
                IsRescheduled = false,
                UoM = viewModel.UoM,
                LoadCode = viewModel.DeliverySchedule.LoadCode,
                GroupId = latestGroupNumber + 1
            };
            return scheduleEntity;
        }

        //private async Task PostScheduleModifyEvents(ScheduleEditInputViewModel viewModel, UserContext userContext, List<FuelDispatchLocation> prevAssignTerminal, FuelDispatchLocation currentTerminalLocation, DeliverySchedule deliverySchedule, string orderTerminal, string timeZone, bool isAddressChanged)
        //{
        //    await NotificationToDriverForScheduleModify(viewModel, deliverySchedule, currentTerminalLocation.Id, timeZone, isAddressChanged);
        //    if (currentTerminalLocation.Id == 0)
        //        return;
        //    var prevLocation = prevAssignTerminal.Count > 0 ? prevAssignTerminal.LastOrDefault() : null;
        //    NotificationDispatchLocationViewModel dispatchLocation = new NotificationDispatchLocationViewModel()
        //    {
        //        DispatchNotificationType = DispatchNotificationType.TerminalChange,
        //        DeliveryScheduleId = viewModel.DeliveryScheduleId,
        //        TrackableScheduleId = viewModel.TrackableScheduleId,
        //        CurrentTerminalName = GetTerminalLocation(currentTerminalLocation, viewModel.TerminalDetails.IsNewLocation ? viewModel.TerminalDetails.AddressDetails.State.Name : viewModel.TerminalDetails.TerminalName),
        //        PreviousTerminalName = prevLocation != null ? GetTerminalLocation(prevLocation, prevLocation.TerminalId.HasValue ? prevLocation.MstExternalTerminal.Name : prevLocation.MstState.Name) : orderTerminal
        //    };
        //    ProcessDispatchLocationForWebNotifications(dispatchLocation, viewModel.OrderId, userContext.Id);
        //}

        private static void GetModifyScheduleResponse(ScheduleEditInputViewModel viewModel, StatusViewModel response, DeliverySchedule deliverySchedule, string timeZone, bool isAddressChanged)
        {
            if (deliverySchedule.Id > 0)
            {
                var currentDate = DateTimeOffset.Now.ToTargetDateTimeOffset(timeZone);
                double dateDiff = deliverySchedule.Date.Date.Add(deliverySchedule.StartTime).Subtract(currentDate.DateTime).TotalHours;
                if (deliverySchedule.Date.Date == currentDate.Date && dateDiff < 0)
                {
                    dateDiff = deliverySchedule.Date.Date.Add(deliverySchedule.EndTime).Subtract(currentDate.DateTime).TotalHours;
                }
                if (dateDiff <= 24 && dateDiff > 0)
                {
                    response.StatusCode = Status.Warning;
                    response.StatusMessage = Resource.warningMessageSupplierRescheduledDeliveryIn24Hrs;
                }
                else
                {
                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageDeliveryScheduleSuccessfullyUpdated;
                }
            }
            else if (isAddressChanged)
            {
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.successMessageDeliveryScheduleSuccessfullyUpdated;
            }
        }

        public async Task<CarrierViewModel> AddCarrierIfNotExists(string carrier, int userId, int companyId)
        {
            var response = new CarrierViewModel();
            try
            {
                if (!string.IsNullOrEmpty(carrier))
                {
                    var existingCarrier = await Context.DataContext.Carriers.Where(t => t.Name.ToLower() == carrier.Trim().ToLower())
                                            .Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();
                    if (existingCarrier != null)
                    {
                        response.Id = existingCarrier.Id;
                        response.Name = existingCarrier.Name;
                    }
                    else
                    {
                        var newCarrier = new Carrier()
                        {
                            Name = carrier.Trim(),
                            CreatedBy = userId,
                            CompanyId = companyId,
                            CreatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.Carriers.Add(newCarrier);
                        await Context.CommitAsync();
                        response.Id = newCarrier.Id;
                        response.Name = newCarrier.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "AddCarrierIfNotExists", ex.Message, ex);
            }
            return response;
        }

        public async Task<SupplierSourceViewModel> AddSupplierSourceIfNotExists(SupplierSourceViewModel supplierSource, int userId, int companyId)
        {
            var response = new SupplierSourceViewModel() { ContractNumber = supplierSource.ContractNumber };
            try
            {
                if (!string.IsNullOrEmpty(supplierSource.Name))
                {
                    var existingSupplier = await Context.DataContext.SupplierSources.Where(t => t.Name.ToLower() == supplierSource.Name.Trim().ToLower())
                                            .Select(x => new { x.Id, x.Name }).FirstOrDefaultAsync();
                    if (existingSupplier != null)
                    {
                        response.Id = existingSupplier.Id;
                        response.Name = existingSupplier.Name;
                    }
                    else
                    {
                        var newSupplier = new SupplierSource()
                        {
                            Name = supplierSource.Name.Trim(),
                            CreatedBy = userId,
                            CompanyId = companyId,
                            CreatedDate = DateTimeOffset.Now
                        };
                        Context.DataContext.SupplierSources.Add(newSupplier);
                        await Context.CommitAsync();
                        response.Id = newSupplier.Id;
                        response.Name = newSupplier.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "AddSupplierSourceIfNotExists", ex.Message, ex);
            }
            return response;
        }

        private async Task<bool> DeleteExistingSplitLoadAddresses(ScheduleEditInputViewModel viewModel)
        {
            bool isAddressRemoved = false;
            var existingSplitLoadAddresses = await Context.DataContext.FuelDispatchLocations.Where(t => t.OrderId == viewModel.OrderId && t.LocationType == (int)LocationType.Drop && t.DeliveryScheduleId == viewModel.DeliveryScheduleId).ToListAsync();
            var deletedSplitLoadAddresses = existingSplitLoadAddresses;

            if (viewModel.IsSplitLoad)
            {
                deletedSplitLoadAddresses = existingSplitLoadAddresses.Where(t => !viewModel.SplitLoadAddresses.Any(t1 => t1.Id == t.Id)).ToList();
                isAddressRemoved = deletedSplitLoadAddresses.Any();
            }
            foreach (var existingAddress in deletedSplitLoadAddresses)
            {
                if (existingAddress.DeliveryScheduleId == null)
                {
                    Context.DataContext.FuelDispatchLocations.Remove(existingAddress);
                }
                else if (existingAddress.TrackableScheduleId == null && !existingSplitLoadAddresses.Any(t => t.ParentId == existingAddress.Id))
                {
                    if (existingAddress.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates)
                    {
                        existingAddress.IsSkipped = true;
                    }
                    else
                    {
                        var deletedAddress = new FuelDispatchLocation(existingAddress);
                        deletedAddress.TrackableScheduleId = viewModel.TrackableScheduleId;
                        deletedAddress.ParentId = existingAddress.Id;
                        deletedAddress.IsSkipped = true;
                        Context.DataContext.FuelDispatchLocations.Add(deletedAddress);
                    }
                }
                else
                {
                    existingAddress.IsSkipped = true;
                }
            }
            return isAddressRemoved;
        }

        private async Task<bool> UpdateFuelDispatchLocation(ScheduleEditInputViewModel viewModel, int newDeliveryScheduleId, int newTrackableScheduleId)
        {
            var splitLoadAddresses = viewModel.SplitLoadAddresses;
            bool isAddressChanged = false;
            if (newTrackableScheduleId == 0)
            {
                isAddressChanged = await DeleteExistingSplitLoadAddresses(viewModel);
                splitLoadAddresses = viewModel.SplitLoadAddresses.Where(t => t.Id == 0).ToList();
            }
            if (splitLoadAddresses.Any() && viewModel.IsSplitLoad)
            {
                isAddressChanged = true;
                var orderdomain = new OrderDomain(this);

                foreach (var splitLoadAddress in splitLoadAddresses)
                {
                    var location = splitLoadAddress.ToFuelDispatchLocationEntity();
                    location.CreatedBy = viewModel.UpdatedBy;
                    location.OrderId = viewModel.OrderId;
                    location.TrackableScheduleId = newTrackableScheduleId > 0 ? newTrackableScheduleId : viewModel.TrackableScheduleId;
                    location.DeliveryScheduleId = newDeliveryScheduleId > 0 ? newDeliveryScheduleId : viewModel.DeliveryScheduleId;
                    location.Currency = viewModel.Currency;
                    if (location.Latitude == 0 && location.Longitude == 0)
                    {
                        orderdomain.UpdateFuelDispatchLocationLatLong(location);
                    }
                    else if (location.Address == null)
                    {
                        orderdomain.SetSplitLoadAddressByLatLong(location);
                    }
                    Context.DataContext.FuelDispatchLocations.Add(location);
                }
            }
            if (isAddressChanged)
            {
                await Context.CommitAsync();
            }
            return isAddressChanged;
        }
        private async Task<bool> UpdateScheduleAdditionalDetails(RescheduleDeliveryViewModel viewModel, DeliveryScheduleXTrackableSchedule trackableSchedule, UserContext userContext, StatusViewModel response)
        {
            bool isSaved = false;
            if (!string.IsNullOrEmpty(viewModel.Carrier.Name))
            {
                viewModel.Carrier = await AddCarrierIfNotExists(viewModel.Carrier.Name, userContext.Id, userContext.CompanyId);
            }
            if (viewModel.SupplierSource == null)
            {
                viewModel.SupplierSource = new SupplierSourceViewModel();
            }
            else if (!string.IsNullOrEmpty(viewModel.SupplierSource.Name))
            {
                viewModel.SupplierSource = await AddSupplierSourceIfNotExists(viewModel.SupplierSource, userContext.Id, userContext.CompanyId);
            }
            var upatedCarrierId = viewModel.Carrier.Id == 0 ? (int?)null : viewModel.Carrier.Id;
            if (trackableSchedule.Date == viewModel.DeliveryDate && trackableSchedule.StartTime == Convert.ToDateTime(viewModel.StartTime).TimeOfDay && trackableSchedule.EndTime == Convert.ToDateTime(viewModel.EndTime).TimeOfDay)
            {
                if (trackableSchedule.CarrierId == upatedCarrierId
                    && trackableSchedule.SupplierContract == viewModel.SupplierSource.ContractNumber
                    && trackableSchedule.SupplierSourceId == viewModel.SupplierSource.Id
                    && trackableSchedule.LoadCode == viewModel.LoadCode)
                {
                    isSaved = true;
                }
                else
                {
                    trackableSchedule.CarrierId = upatedCarrierId;
                    trackableSchedule.SupplierContract = viewModel.SupplierSource.ContractNumber;
                    trackableSchedule.SupplierSourceId = viewModel.SupplierSource.Id;
                    trackableSchedule.LoadCode = viewModel.LoadCode;

                    if (trackableSchedule.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates)
                    {
                        trackableSchedule.DeliverySchedule.CarrierId = upatedCarrierId;
                        trackableSchedule.DeliverySchedule.SupplierContract = viewModel.SupplierSource.ContractNumber;
                        trackableSchedule.DeliverySchedule.SupplierSourceId = viewModel.SupplierSource.Id;
                        trackableSchedule.DeliverySchedule.LoadCode = viewModel.LoadCode;
                    }

                    Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageDeliveryScheduleSuccessfullyUpdated;

                    isSaved = true;
                }

                if (trackableSchedule.QuantityTypeId != (int)viewModel.ScheduleQuantityType || trackableSchedule.Quantity != viewModel.Quantity)
                {
                    trackableSchedule.Quantity = viewModel.Quantity;
                    trackableSchedule.QuantityTypeId = (int)viewModel.ScheduleQuantityType;

                    if (trackableSchedule.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates)
                    {
                        trackableSchedule.DeliverySchedule.Quantity = viewModel.Quantity;
                        trackableSchedule.DeliverySchedule.QuantityTypeId = (int)viewModel.ScheduleQuantityType;
                        trackableSchedule.DeliverySchedule.StatusId = (int)DeliveryScheduleStatus.Modified;
                    }

                    Context.DataContext.Entry(trackableSchedule).State = EntityState.Modified;
                    await Context.CommitAsync();

                    response.StatusCode = Status.Success;
                    response.StatusMessage = Resource.successMessageDeliveryScheduleSuccessfullyUpdated;

                    isSaved = true;
                }
            }

            return isSaved;
        }

        private async Task RescheduleDeliveryScheduleAsync(RescheduleDeliveryViewModel viewModel, DeliverySchedule deliverySchedule, DeliveryScheduleXTrackableSchedule trackableSchedule, Order order, UserContext userContext, StatusViewModel response)
        {
            var orderDomain = new OrderDomain(this);
            var latestSchedules = orderDomain.GetLatestOrderDeliverySchedule(order.OrderDeliverySchedules);
            var latestVersionId = latestSchedules.Any() ? latestSchedules.FirstOrDefault().Version : 0;

            int latestGroupNumber = 0;
            var deliverySchedules = Context.DataContext.DeliverySchedules;
            if (deliverySchedules.Any())
            {
                latestGroupNumber = deliverySchedules.Max(t => t.GroupId);
            }
            HelperDomain helperDomain = new HelperDomain(this);
            var upatedCarrierId = viewModel.Carrier.Id == 0 ? (int?)null : viewModel.Carrier.Id;
            deliverySchedule.Date = viewModel.DeliveryDate;
            deliverySchedule.StartTime = Convert.ToDateTime(viewModel.StartTime).TimeOfDay;
            deliverySchedule.EndTime = Convert.ToDateTime(viewModel.EndTime).TimeOfDay;
            deliverySchedule.Quantity = viewModel.Quantity;
            deliverySchedule.Type = (int)DeliveryScheduleType.SpecificDates;
            deliverySchedule.GroupId = ++latestGroupNumber;
            deliverySchedule.WeekDayId = helperDomain.GetWeekDayId(viewModel.DeliveryDate);
            deliverySchedule.CreatedBy = userContext.Id;
            deliverySchedule.StatusId = (int)DeliveryScheduleStatus.Rescheduled;
            deliverySchedule.IsRescheduled = true;
            deliverySchedule.RescheduledTrackableId = viewModel.TrackableScheduleId;
            deliverySchedule.CarrierId = upatedCarrierId;
            deliverySchedule.SupplierContract = viewModel.SupplierSource.ContractNumber;
            deliverySchedule.SupplierSourceId = viewModel.SupplierSource.Id;
            deliverySchedule.DeliveryGroupId = trackableSchedule.DeliveryGroupId;
            deliverySchedule.QuantityTypeId = trackableSchedule.QuantityTypeId == null ? (int)ScheduleQuantityType.Quantity : trackableSchedule.QuantityTypeId;

            trackableSchedule.IsActive = false;
            trackableSchedule.DeliveryScheduleStatusId = trackableSchedule.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.Missed ? (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled : (int)TrackableDeliveryScheduleStatus.Rescheduled;
            await Context.CommitAsync();

            latestSchedules.ForEach(t => t.IsActive = false);

            if (viewModel.DriverId != null)
            {
                DeliveryScheduleXDriver driver = new DeliveryScheduleXDriver() { DriverId = viewModel.DriverId ?? 0, AssignedBy = userContext.Id, AssignedDate = DateTimeOffset.Now, IsActive = true };
                deliverySchedule.DeliveryScheduleXDrivers.Add(driver);
            }
            Context.DataContext.DeliverySchedules.Add(deliverySchedule);

            latestSchedules.Where(t => !(t.DeliveryRequestId == trackableSchedule.DeliveryScheduleId
                                                                    && t.DeliverySchedule.Type == (int)DeliveryScheduleType.SpecificDates))
                                                                    .ToList().ForEach(t => order.OrderDeliverySchedules
                                                                    .Add(new OrderVersionXDeliverySchedule()
                                                                    {
                                                                        DeliveryRequestId = t.DeliveryRequestId,
                                                                        OrderId = viewModel.OrderId,
                                                                        Version = latestVersionId + 1,
                                                                        CreatedBy = userContext.Id,
                                                                        CreatedDate = DateTimeOffset.Now,
                                                                        IsActive = true
                                                                    }));

            await Context.CommitAsync();

            order.OrderDeliverySchedules.Add(new OrderVersionXDeliverySchedule()
            {
                DeliveryRequestId = deliverySchedule.Id,
                OrderId = viewModel.OrderId,
                Version = latestVersionId + 1,
                CreatedBy = userContext.Id,
                CreatedDate = DateTimeOffset.Now,
                IsActive = true
            });
            await Context.CommitAsync();

            TrackableScheduleDomain trackableScheduleDomain = new TrackableScheduleDomain(this);
            await trackableScheduleDomain.ProcessTrackableSchedules(new List<DeliverySchedule>() { deliverySchedule }, order, null);

            response.StatusCode = Status.Success;
            response.StatusMessage = Resource.successMessageDeliveryScheduleSuccessfullyUpdated;
        }

        private async Task PostRescheduleEvents(RescheduleDeliveryViewModel viewModel, UserContext userContext, DeliverySchedule deliverySchedule, Order order, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            var newsfeedDomain = new NewsfeedDomain(this);
            var eventId = userContext.IsBuyerCompany || userContext.CompanySubTypeId == CompanyType.Buyer ? NewsfeedEvent.BuyerReschedulesSchedule : NewsfeedEvent.SupplierReschedulesSchedule;
            await newsfeedDomain.SetDeliveryScheduleNewsfeed(userContext, order, eventId);

            //NotificationDispatchLocationViewModel dispatchLocation = new NotificationDispatchLocationViewModel()
            //{
            //    DispatchNotificationType = DispatchNotificationType.Reschedule,
            //    DeliveryScheduleId = deliverySchedule.Id,
            //    TrackableScheduleId = viewModel.TrackableScheduleId
            //};
            //ProcessDispatchLocationForWebNotifications(dispatchLocation, viewModel.OrderId, userContext.Id);
            var pushNotificationDomain = new PushNotificationDomain(this);
            await SendRescheduleMessage(viewModel, userContext, trackableSchedule);
            await pushNotificationDomain.PushNotificationRescheduleDeliverySchedule(viewModel, trackableSchedule);
            if (trackableSchedule.DriverId.HasValue)
            {
                var dispatchDomain = new DispatchDomain(this);
                await dispatchDomain.RemoveOnMyWay(trackableSchedule.DriverId.Value, trackableSchedule.OrderId.Value, (int)EnrouteDeliveryStatus.Rescheduled, trackableSchedule.Id, trackableSchedule.DeliveryScheduleId);
            }
        }

        private async Task SendRescheduleMessage(RescheduleDeliveryViewModel viewmodel, UserContext userContext, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            var helperDomain = new HelperDomain(this);
            var serverUrl = helperDomain.GetServerUrl();

            var quantityScheduled = ($"{trackableSchedule.Quantity.GetPreciseValue().GetCommaSeperatedValue()} {trackableSchedule.UoM}").ToLower();
            var composeMessageViewModel = new ComposeMessageViewModel
            {
                Id = 0,
                ComposeType = AppMessageComposeType.Compose,
                Type = AppMessageQueryType.Order,
                Number = viewmodel.OrderId,
                Subject = string.Format(Resource.emailDeliveryRescheduled_Buyer_SubjectText, userContext.Name,
                                                                            userContext.CompanyName),
                Message = string.Format(Resource.emailDeliveryRescheduled_Buyer_BodyText, $"{trackableSchedule.Order.FuelRequest.User.FirstName} {trackableSchedule.Order.FuelRequest.User.LastName}",
                                                                            userContext.Name,
                                                                            userContext.CompanyName,
                                                                            trackableSchedule.Order.PoNumber,
                                                                            trackableSchedule.Date.ToString(Resource.constFormatDate),
                                                                            $"{Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToString(Resource.constFormat12HourTime)} - {Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToString(Resource.constFormat12HourTime)}",
                                                                            quantityScheduled,
                                                                            viewmodel.DeliveryDate.ToString(Resource.constFormatDate),
                                                                            $"{viewmodel.StartTime} - {viewmodel.EndTime}",
                                                                            quantityScheduled,
                                                                            serverUrl + "Buyer/Order/Details/" + trackableSchedule.OrderId),
                TimeStamp = DateTimeOffset.Now
            };
            composeMessageViewModel.Recipients.Add(trackableSchedule.Order.FuelRequest.User.Id);
            if (trackableSchedule.DriverId != null)
            {
                composeMessageViewModel.Message = string.Format(Resource.emailDeliveryRescheduled_Buyer_BodyText, $"{trackableSchedule.User.FirstName} {trackableSchedule.User.LastName}",
                                                                            userContext.Name,
                                                                            userContext.CompanyName,
                                                                            trackableSchedule.Order.PoNumber,
                                                                            trackableSchedule.Date.ToString(Resource.constFormatDate),
                                                                            $"{Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToString(Resource.constFormat12HourTime)} - {Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToString(Resource.constFormat12HourTime)}",
                                                                            quantityScheduled,
                                                                            viewmodel.DeliveryDate.ToString(Resource.constFormatDate),
                                                                            $"{viewmodel.StartTime} - {viewmodel.EndTime}",
                                                                            quantityScheduled,
                                                                            serverUrl + "Driver/Order/Details/" + trackableSchedule.OrderId);
                composeMessageViewModel.Recipients.Add(trackableSchedule.DriverId ?? 0);
            }
            await ContextFactory.Current.GetDomain<AppMessageDomain>().SaveAppMessageAsync((int)SystemUser.System, composeMessageViewModel);
        }

        public async Task ProcessInActiveDriverNotification()
        {
            try
            {
                var currentDate = DateTimeOffset.Now.Date;
                var CompareDate = DateTimeOffset.Now.Date.AddDays(ApplicationConstants.AfterWeek);
                var mUsers = await Context.DataContext.Users
                    .Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Driver || t1.Id == (int)UserRoles.Supplier) && !t.IsOnboardingComplete && t.DeliveryScheduleXTrackableSchedules.Any(t2 => t2.Date < CompareDate && t2.Date >= currentDate)).ToListAsync();
                var sUsers = await Context.DataContext.Users
                    .Where(t => t.MstRoles.Any(t1 => t1.Id == (int)UserRoles.Driver || t1.Id == (int)UserRoles.Supplier)
                    && !t.IsOnboardingComplete
                    && t.OrderXDrivers.Any(t3 => t3.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery && t3.Order.FuelRequest.FuelRequestDetail.StartDate < CompareDate && t3.Order.FuelRequest.FuelRequestDetail.StartDate >= currentDate)).ToListAsync();
                var notificationDomain = new NotificationDomain(this);
                var allUsers = mUsers.Union(sUsers).ToList();
                foreach (var user in allUsers)
                {
                    var multipledeliveries = new List<InActiveDriverViewModel>();
                    var singleDeliveries = new List<InActiveDriverViewModel>();
                    if (mUsers != null)
                    {
                        multipledeliveries = await Context.DataContext.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDelivered())
                        .Where(t => t.DriverId == user.Id && t.IsActive && t.Date < CompareDate && t.Date >= currentDate && t.IsActive && t.Order.OrderXStatuses.Any(t1 => t1.IsActive && t1.StatusId == (int)OrderStatus.Open))
                        .Select(t2 => new InActiveDriverViewModel { OrderId = t2.OrderId ?? 0, DriverId = t2.DriverId, StartDate = t2.Date }).ToListAsync();
                    }

                    if (sUsers != null)
                    {
                        singleDeliveries = await Context.DataContext.OrderXDrivers.Where(t => t.DriverId == user.Id && t.IsActive && t.Order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery
                                && t.IsActive && t.Order.IsActive
                                && (t.Order.FuelRequest.FuelRequestDetail.StartDate < CompareDate
                                && t.Order.FuelRequest.FuelRequestDetail.StartDate >= currentDate)
                                && t.Order.OrderXStatuses.Any(t2 => t2.IsActive && t2.StatusId == (int)OrderStatus.Open))
                                .Select(t3 => new InActiveDriverViewModel { OrderId = t3.OrderId, DriverId = t3.DriverId, StartDate = t3.Order.FuelRequest.FuelRequestDetail.StartDate }).ToListAsync();
                    }

                    var deliveries = multipledeliveries.Union(singleDeliveries).ToList();

                    foreach (var delivery in deliveries)
                    {
                        var message = new InActiveDriverMessageViewModel() { OrderId = delivery.OrderId, DeliveryDate = delivery.StartDate };
                        var jsonMessage = new JavaScriptSerializer().Serialize(message);
                        var lastNotifications = await Context.DataContext.Notifications.Where(t => t.EventTypeId == (int)EventType.InActiveDriverAssignedToOrder && t.EntityId == user.Id && t.CreatedDate.HasValue && t.CreatedDate.Value < CompareDate).ToListAsync();
                        if (lastNotifications != null && lastNotifications.Count > 0)
                        {
                            bool isSent = false;
                            var driverNotification = new NotificationEventViewModel();
                            foreach (var notification in lastNotifications)
                            {
                                var messages = JsonConvert.DeserializeObject<InActiveDriverMessageViewModel>(notification.JsonMessage);
                                if (messages.OrderId == delivery.OrderId && messages.DeliveryDate == delivery.StartDate)
                                {
                                    isSent = true;
                                    driverNotification = notification.ToViewModel();
                                    break;
                                }
                            }

                            if (!isSent || (isSent && delivery.StartDate.Subtract(DateTime.Now).TotalDays <= 1 && driverNotification.CreatedDate.Value.Subtract(DateTime.Now).TotalDays > 1))
                            {
                                await notificationDomain.AddNotificationEventAsync(
                                                                                              EventType.InActiveDriverAssignedToOrder,
                                                                                              user.Id,
                                                                                              user.Id,
                                                                                              null,
                                                                                              jsonMessage);
                            }
                        }
                        else
                        {
                            await notificationDomain.AddNotificationEventAsync(
                                                                                          EventType.InActiveDriverAssignedToOrder,
                                                                                          user.Id,
                                                                                          user.Id,
                                                                                          null,
                                                                                          jsonMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "ProcessInActiveDriverNotification", ex.Message, ex);
            }
        }

        //public void ProcessDispatchLocationForWebNotifications(NotificationDispatchLocationViewModel dispatchLocation, int orderId, int createdById)
        //{
        //    if (orderId > 0)
        //    {
        //        var queueDomain = new QueueMessageDomain();
        //        var IsBrokeredChain = true;
        //        var parentOrderId = orderId;
        //        while (IsBrokeredChain)
        //        {
        //            var order = Context.DataContext.Orders
        //                         .Select(x => new
        //                         {
        //                             OrderId = x.Id,
        //                             OrderNumber = x.PoNumber,
        //                             JobName = x.FuelRequest.Job.Name,
        //                             CompanyName = x.Company.Name,
        //                             CreatedByName = x.Company.Users.FirstOrDefault(t => t.Id == createdById).FirstName,
        //                             ParentOrderId = x.FuelRequest.FuelRequest1.Orders.Select(t => t.Id).FirstOrDefault()
        //                         }).SingleOrDefault(x1 => x1.OrderId == parentOrderId);
        //            var queueRequest = GetDispatchEnqueueMessageRequest(dispatchLocation, createdById, order);
        //            var queueId = queueDomain.EnqeueMessage(queueRequest);
        //            if (order.ParentOrderId == 0)
        //            {
        //                IsBrokeredChain = false;
        //            }
        //            else
        //            {
        //                parentOrderId = order.ParentOrderId;
        //            }
        //        }
        //    }
        //}

        public int AddExistingSchedulesToNewVersion(int orderId, List<OrderVersionXDeliverySchedule> orderSchedules)
        {
            orderSchedules.ForEach(t => { t.IsActive = false; });
            int latestVersion = orderSchedules.OrderByDescending(t => t.Version).Select(t => t.Version).FirstOrDefault();
            foreach (var schedule in orderSchedules)
            {
                OrderVersionXDeliverySchedule dschedule = new OrderVersionXDeliverySchedule()
                {
                    DeliveryRequestId = schedule.DeliveryRequestId,
                    OrderId = orderId,
                    Version = latestVersion + 1,
                    CreatedBy = schedule.CreatedBy,
                    CreatedDate = schedule.CreatedDate,
                    AdditionalNotes = schedule.AdditionalNotes,
                    IsActive = true
                };
                Context.DataContext.OrderVersionXDeliverySchedules.Add(dschedule);
            }
            return latestVersion;
        }

        //private QueueMessageViewModel GetDispatchEnqueueMessageRequest(NotificationDispatchLocationViewModel dispatchLocation, int createdById, dynamic order)
        //{
        //    if (order != null && order.OrderId > 0)
        //    {
        //        dispatchLocation.CreatedByCompanyName = order.CompanyName;
        //        dispatchLocation.CreatedByUserId = createdById;
        //        dispatchLocation.CreatedByUserName = order.CreatedByName;
        //        dispatchLocation.JobName = order.JobName;
        //        dispatchLocation.OrderId = order.OrderId;
        //        dispatchLocation.OrderNumber = order.OrderNumber;
        //    }

        //    string json = JsonConvert.SerializeObject(dispatchLocation);

        //    return new QueueMessageViewModel()
        //    {
        //        CreatedBy = createdById,
        //        QueueProcessType = QueueProcessType.DispatchLocation,
        //        JsonMessage = json
        //    };
        //}
        private FuelDispatchLocation AddNewPickUpLocation(DispatchTerminalViewModel viewModel, UserContext userContext, int orderId, int? deliveryScheduleId, int? trackableScheduleId)
        {
            if (!viewModel.IsNewLocation && viewModel.TerminalId.HasValue)
            {
                var terminal = Context.DataContext.MstExternalTerminals.Select(x => new
                {
                    x.Id,
                    x.Latitude,
                    x.Longitude,
                    x.CountryCode,
                    x.CountyName,
                    x.Address,
                    x.City,
                    x.Name,
                    x.StateCode,
                    x.StateId,
                    x.ZipCode,
                    x.Currency
                }).SingleOrDefault(t => t.Id == viewModel.TerminalId);

                return new FuelDispatchLocation()
                {
                    OrderId = orderId,
                    Address = terminal.Address,
                    City = terminal.City,
                    CountryCode = terminal.CountryCode,
                    CountyName = terminal.CountyName,
                    CreatedBy = userContext.Id,
                    CreatedDate = DateTimeOffset.Now,
                    Currency = terminal.Currency,
                    DeliveryScheduleId = deliveryScheduleId,
                    IsActive = true,
                    Latitude = terminal.Latitude,
                    Longitude = terminal.Longitude,
                    LocationType = (int)LocationType.PickUp,
                    StateCode = terminal.StateCode,
                    StateId = terminal.StateId,
                    TerminalId = terminal.Id,
                    TrackableScheduleId = trackableScheduleId,
                    ZipCode = terminal.ZipCode,
                    SiteName = terminal.Name
                };
            }
            else
            {
                return new FuelDispatchLocation()
                {
                    OrderId = orderId,
                    Address = viewModel.AddressDetails.Address,
                    City = viewModel.AddressDetails.City,
                    CountryCode = viewModel.AddressDetails.Country.Code,
                    CountyName = viewModel.AddressDetails.CountyName,
                    CreatedBy = userContext.Id,
                    CreatedDate = DateTimeOffset.Now,
                    Currency = viewModel.Currency,
                    DeliveryScheduleId = deliveryScheduleId,
                    IsActive = true,
                    Latitude = viewModel.AddressDetails.Latitude,
                    Longitude = viewModel.AddressDetails.Longitude,
                    LocationType = (int)LocationType.PickUp,
                    StateCode = viewModel.AddressDetails.State.Code,
                    StateId = viewModel.AddressDetails.State.Id,
                    TerminalId = null,
                    TrackableScheduleId = trackableScheduleId,
                    ZipCode = viewModel.AddressDetails.ZipCode,
                    TimeZoneName = viewModel.AddressDetails.TimeZoneName,
                    SiteName = viewModel.AddressDetails.SiteName
                };
            }
        }

        private string GetTerminalLocation(FuelDispatchLocation dispatchLocation, string Name)
        {
            return dispatchLocation.TerminalId.HasValue ? Name : $"{dispatchLocation.Address}, {dispatchLocation.City}, {Name}, {dispatchLocation.ZipCode}";
        }

        public async Task<FuelPickUpLocationViewModel> GetTerminalForTrackableSchedule(int trackableScheduleId)
        {
            try
            {
                var terminal = await Context.DataContext.FuelDispatchLocations.Where(t => t.TrackableScheduleId == trackableScheduleId
                                            && t.IsActive
                                            && t.LocationType == (int)LocationType.PickUp).Select(x => new
                                            {
                                                Id = x.TerminalId ?? 0,
                                                TerminalName = x.MstExternalTerminal != null ? x.MstExternalTerminal.Name : "",
                                                orderTerminalId = x.Order.TerminalId ?? 0,
                                                orderTerminalName = x.Order.MstExternalTerminal != null ? x.Order.MstExternalTerminal.Name : ""
                                            }).FirstOrDefaultAsync();
                if (terminal != null)
                {
                    if (terminal.Id == 0)
                    {
                        return new FuelPickUpLocationViewModel() { TerminalId = terminal.orderTerminalId, TerminalName = terminal.orderTerminalName };
                    }
                    //NEED TO INVESTIGATE WHY DYNAMIC IS NOT WORKING HERE
                    return new FuelPickUpLocationViewModel() { TerminalId = terminal.Id, TerminalName = terminal.TerminalName };
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetTerminalForTrackableSchedule", ex.Message, ex);
            }
            return null;
        }

        public async Task<StatusViewModel> AddSplitDropAddressAsync(ApiDispatchAddressViewModel viewModel)
        {
            var response = new StatusViewModel();
            try
            {
                var dropAddress = GetDispatchLocationDetails(viewModel, (int)LocationType.Drop);
                if (!string.IsNullOrEmpty(viewModel.Address.Address))
                {
                    dropAddress.Address = viewModel.Address.Address;
                }
                var dispatchLocation = dropAddress.ToFuelDispatchLocationEntity();
                dispatchLocation.DropStatus = DropAddressStatus.Pending;
                Context.DataContext.FuelDispatchLocations.Add(dispatchLocation);

                await Context.CommitAsync();

                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
                response.EntityId = dispatchLocation.Id;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "AddSplitDropAddressAsync", ex.Message, ex);
            }
            return response;
        }

        private DispatchLocationViewModel GetDispatchLocationDetails(ApiDispatchAddressViewModel viewModel, int locationType)
        {
            var response = new DispatchLocationViewModel();
            try
            {
                var point = GoogleApiDomain.GetAddress(Convert.ToDouble(viewModel.Address.Latitude), Convert.ToDouble(viewModel.Address.Longitude));
                if (point != null)
                {
                    var location = Context.DataContext.MstStates.Where(t => t.Code.ToLower() == point.StateCode.ToLower()).Select(t => new { stateId = t.Id, CountryCode = t.MstCountry.Code, t.MstCountry.Currency }).FirstOrDefault();
                    if (location != null)
                    {
                        response.StateId = location.stateId;
                        response.CountryCode = location.CountryCode;
                        response.Currency = location.Currency;
                    }

                    response.Address = point.Address;
                    response.City = point.City;
                    response.ZipCode = point.ZipCode;
                    response.CountyName = point.CountyName;
                    response.LocationType = locationType;
                    response.Latitude = viewModel.Address.Latitude;
                    response.Longitude = viewModel.Address.Longitude;
                    response.StateCode = point.StateCode;
                    response.IsValidAddress = true;
                    response.OrderId = viewModel.OrderId;
                    response.CreatedBy = viewModel.UserId;
                    response.CreatedDate = DateTimeOffset.Now;
                    response.DeliveryScheduleId = viewModel.DeliveryScheduleId == 0 ? null : viewModel.DeliveryScheduleId;
                    response.TrackableScheduleId = viewModel.TrackableScheduleId == 0 ? null : viewModel.TrackableScheduleId;
                }
                else
                {
                    LogManager.Logger.WriteDebug("DispatchDomain", "GetDispatchLocationDetails", $"Invalid Address: latitude:-:{viewModel.Address.Latitude} longitude:-{viewModel.Address.Longitude} OrderId:-{viewModel.OrderId} UserId:- {viewModel.UserId}");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DispatchDomain", "GetDispatchLocationDetails", $"Invalid Address: latitude:-:{viewModel.Address.Latitude} longitude:-{viewModel.Address.Longitude} OrderId:-{viewModel.OrderId} UserId:- {viewModel.UserId}", ex);
            }
            return response;
        }

        private async Task<List<SplitLoadAddressViewModel>> GetSplitLoadAddressesForSchedule(int orderId, int? trackableScheduleId, int? deliveryScheduleId)
        {
            return await Context.DataContext.Orders.Where(t => t.Id == orderId).SelectMany(t =>

                t.FuelDispatchLocations.Where(t1 =>
                                                (
                                                    t1.DeliveryScheduleId == deliveryScheduleId
                                                    && (!t1.TrackableScheduleId.HasValue || t1.TrackableScheduleId == trackableScheduleId) && !t1.IsSkipped
                                                    && (!t.FuelDispatchLocations.Any(t2 => t2.ParentId == t1.Id))
                                                )
                                                && !t1.IsJobLocation
                                        && t1.LocationType == (int)LocationType.Drop && t1.IsActive)
                        .Select(t1 => new SplitLoadAddressViewModel()
                        {
                            Id = t1.Id,
                            Address = t1.Address,
                            City = t1.City,
                            StateId = t1.StateId.Value,
                            StateCode = t1.StateCode,
                            CountryId = t.FuelRequest.Job.CountryId,
                            CountryCode = t1.CountryCode,
                            ZipCode = t1.ZipCode,
                            Latitude = t1.Latitude,
                            Longitude = t1.Longitude,
                            CountyName = t1.CountyName,
                            Currency = t1.Currency,
                            TimeZoneName = t1.TimeZoneName,
                            SiteName = t1.SiteName
                        })).ToListAsync();

        }

        public async Task<string> GetTerminalName(int? trackableScheduleId, int orderId)
        {
            string terminalName = string.Empty;
            try
            {
                if (trackableScheduleId.HasValue && trackableScheduleId.Value > 0)
                {
                    bool IsschdefuleInfoAvailable = Context.DataContext.FuelDispatchLocations
                                                    .Where(t => t.TrackableScheduleId == trackableScheduleId &&
                                                    t.IsActive && t.LocationType == (int)LocationType.PickUp).Any();
                    if (IsschdefuleInfoAvailable)
                    {
                        var ScheduleInfo = Context.DataContext.FuelDispatchLocations.Where(t =>
                                          t.TrackableScheduleId == trackableScheduleId && t.IsActive
                                          && t.LocationType == (int)LocationType.PickUp).FirstOrDefault();
                        if (ScheduleInfo.TerminalId != null)
                        {
                            terminalName = ScheduleInfo.MstExternalTerminal.Name;
                        }
                        else
                        {
                            terminalName = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.MstExternalTerminal.Name).SingleOrDefault();
                        }

                    }
                    else
                    {
                        terminalName = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.MstExternalTerminal.Name).SingleOrDefault();
                    }
                }
                else
                {
                    terminalName = Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.MstExternalTerminal.Name).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {

                LogManager.Logger.WriteException("DispatchDomain", "GetTerminalName", ex.Message, ex);
            }
            return terminalName;
        }

        #region TPD API Methods
        public async Task<ApiResponseViewModel> UpdateDeliveryStatusFromAPI(UserContext userContext, TPDScheduleStatusViewModel apiRequestModel)
        {
            var apiResponse = new ApiResponseViewModel();
            try
            {
                var updateLocations = new List<AppLocation>();
                var enrouteHistory = new List<EnrouteDeliveryHistory>();

                List<int> existingAppLocations = new List<int>();

                if (!string.IsNullOrWhiteSpace(apiRequestModel.CarrierOrderID) || apiRequestModel.TFXScheduleID > 0)
                {
                    var tracableScheduleDetails = Context.DataContext.DeliveryScheduleXTrackableSchedules
                                        .Where(t => ((t.Order.AcceptedCompanyId == userContext.CompanyId && t.CarrierOrderId.ToLower() == apiRequestModel.CarrierOrderID.ToLower() && t.CarrierOrderId != null) || (t.Id == apiRequestModel.TFXScheduleID))
                                                //&& t.CarrierOrderId != null        
                                                //&& t.DriverId == userContext.Id 
                                                && t.IsActive
                                                && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                && t.DeliveryScheduleStatusId != (int)DeliveryScheduleStatus.Canceled)
                                        .Select(t => new { t.Id, t.OrderId, t.DeliveryScheduleId, t.DriverId, t.User.Email }).ToList();

                    if (tracableScheduleDetails.Any())
                    {
                        foreach (var item in tracableScheduleDetails)
                        {
                            if (item.DriverId.HasValue)
                            {
                                AppLocationViewModel appLocation = new AppLocationViewModel();
                                appLocation.AppType = AppType.ExternalApiCaller;
                                appLocation.DeliveryScheduleId = item.DeliveryScheduleId;
                                appLocation.FCMAppId = item.Email;
                                appLocation.Latitude = apiRequestModel.DriversLatestLat.GetValue<decimal>();
                                appLocation.Longitude = apiRequestModel.DriversLatestLong.GetValue<decimal>();
                                appLocation.OrderId = item.OrderId;
                                appLocation.TrackableScheduleId = item.Id;
                                appLocation.UserId = item.DriverId.Value;
                                appLocation.ExternalRefID = apiRequestModel.ExternalRefID;

                                var enrouteViewModel = new EnrouteDeliveryViewModel();
                                enrouteViewModel.UserId = item.DriverId.Value;
                                enrouteViewModel.OrderId = item.OrderId;
                                enrouteViewModel.DeliveryScheduleId = item.DeliveryScheduleId;
                                enrouteViewModel.TrackableScheduleId = item.Id;
                                enrouteViewModel.StatusId = apiRequestModel.DeliveryScheduleStatus.GetActualStatus();
                                enrouteHistory.Add(enrouteViewModel.ToEntity());

                                var existingStatus = Context.DataContext.AppLocations
                                                    .Where(t => t.FCMAppId == item.Email && t.OrderId == item.OrderId && t.TrackableScheduleId == item.Id)
                                                    .Select(t => t.Id).FirstOrDefault();
                                if (existingStatus > 0)
                                {
                                    existingAppLocations.Add(existingStatus);
                                }
                                else
                                {
                                    var appLocatioEntity = appLocation.ToEntity();
                                    if (appLocatioEntity != null)
                                    {
                                        appLocatioEntity.StatusId = apiRequestModel.DeliveryScheduleStatus.GetActualStatus();
                                        updateLocations.Add(appLocatioEntity);
                                    }
                                }
                            }
                        }

                        if (updateLocations.Any() || existingAppLocations.Any())
                        {
                            using (var transaction = Context.DataContext.Database.BeginTransaction())
                            {
                                try
                                {
                                    Context.DataContext.EnrouteDeliveryHistories.AddRange(enrouteHistory);
                                    await Context.CommitAsync();

                                    if (existingAppLocations.Any())
                                    {
                                        //update existing locations lat-long, status and update time
                                        Context.DataContext.Database
                                                .ExecuteSqlCommand("UPDATE AppLocations SET StatusId={0}, UpdatedDate={1}, " +
                                                 "Latitude={2}, Longitude={3}, ExternalRefID={4} WHERE ID IN ({5})"
                                                , apiRequestModel.DeliveryScheduleStatus.GetActualStatus(), DateTime.Now,
                                                apiRequestModel.DriversLatestLat.GetValue<decimal>(),
                                                apiRequestModel.DriversLatestLong.GetValue<decimal>(),
                                                apiRequestModel.ExternalRefID,
                                                string.Join<int>(",", existingAppLocations)
                                                );

                                        await Context.CommitAsync();
                                    }

                                    if (updateLocations.Any())
                                    {
                                        Context.DataContext.AppLocations.AddRange(updateLocations);
                                        await Context.CommitAsync();
                                    }

                                    transaction.Commit();

                                    var uniqueTrackScheduleIdList = tracableScheduleDetails.Select(t => t.Id).Distinct().ToList();
                                    var dsbDomain = new ScheduleBuilderDomain(this);
                                    var driverId = tracableScheduleDetails.Select(t => t.DriverId.Value).FirstOrDefault();

                                    foreach (var schedule in uniqueTrackScheduleIdList)
                                    {
                                        dsbDomain.UpdateDeliveryRequestStatus(schedule, apiRequestModel.DeliveryScheduleStatus.GetActualStatus(), driverId);
                                    }

                                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS02, Message = Resource.successMsgDSLoadUpdated });
                                }
                                catch (Exception ex)
                                {
                                    apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProcessRequestFailed });
                                    LogManager.Logger.WriteException("DispatchDomain", "UpdateDeliveryStatusFromAPI", $"{ex.Message} carrierOrderId={apiRequestModel.CarrierOrderID} deliveryScheduleId={apiRequestModel.TFXScheduleID}", ex);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(apiRequestModel.CarrierOrderID))
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRS02,
                                Message = string.Format(Resource.errMsgDeliveryScheduleNotFound, nameof(apiRequestModel.CarrierOrderID), apiRequestModel.CarrierOrderID)
                            });

                        if (apiRequestModel.TFXScheduleID > 0)
                            apiResponse.Messages.Add(new ApiCodeMessages()
                            {
                                Code = Constants.ApiCodeRS02,
                                Message = string.Format(Resource.errMsgDeliveryScheduleNotFound, nameof(apiRequestModel.TFXScheduleID), apiRequestModel.TFXScheduleID)
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Messages.Add(new ApiCodeMessages() { Code = Constants.ApiCodeRS01, Message = Resource.errMsgProcessRequestFailed });
                LogManager.Logger.WriteException("DispatchDomain", "UpdateDeliveryStatusFromAPI", $"{ex.Message} carrierOrderId={apiRequestModel.CarrierOrderID} deliveryScheduleId={apiRequestModel.TFXScheduleID}", ex);
            }
            return apiResponse;
        }
        #endregion

    }
}