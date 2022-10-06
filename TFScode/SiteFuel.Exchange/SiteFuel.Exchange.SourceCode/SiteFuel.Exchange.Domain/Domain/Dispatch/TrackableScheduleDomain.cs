using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class TrackableScheduleDomain : BaseDomain
    {
        public TrackableScheduleDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public TrackableScheduleDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task ProcessTrackableSchedules(IEnumerable<DeliverySchedule> schedules, Order order, List<DeliverySchedule> deletedSchedules = null)
        {
            try
            {
                DateTimeOffset jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.FuelRequest.Job.TimeZoneName);
                DateTime maxDate = jobLocationTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor);
                var trackableScheduleEntity = Context.DataContext.DeliveryScheduleXTrackableSchedules;

                //deleting schedules which are not canceled, not missed and not dropped
                await DeleteTrackableSchedules(schedules, trackableScheduleEntity, order, deletedSchedules);
                foreach (var item in schedules) { item.UoM = order.FuelRequest.UoM; }

                //removing existing schedules from today
                List<DeliveryScheduleXTrackableSchedule> schedulesToDelete;
                foreach (var schedule in schedules)
                {
                    var isValidDate = CheckIfScheduleDateIsWithinAMonth(schedule, jobLocationTime);
                    if (isValidDate)
                    {
                        schedulesToDelete = trackableScheduleEntity.Where(Extensions.IsTrackableScheduleUnDelivered()).Where(t => t.DeliverySchedule != null && t.DeliverySchedule.Id == schedule.Id
                                                                            && !t.Invoices.Any(t1 => t1.IsActive && t1.InvoiceVersionStatusId == (int)InvoiceVersionStatus.Active) && t.OrderId == order.Id
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Missed
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Canceled
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndCanceled
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.Rescheduled
                                                                            && t.DeliveryScheduleStatusId != (int)TrackableDeliveryScheduleStatus.RescheduledMissed).ToList();
                        trackableScheduleEntity.RemoveRange(schedulesToDelete);
                    }
                }
                await Context.CommitAsync();

                var scheduleInput = new ScheduleStatusUpdateInput()
                {
                    OrderId = order.Id,
                    FuelRequestEndDate = order.FuelRequest.FuelRequestDetail.EndDate,
                    JobEndDate = order.FuelRequest.Job.EndDate,
                    TimeZoneName = order.FuelRequest.Job.TimeZoneName,
                    DroppedQuantity = order.Invoices.Where(t1 => t1.IsActiveInvoice && !t1.IsBuyPriceInvoice).Sum(t1 => t1.DroppedGallons)
                                    + order.DeliveryScheduleXTrackableSchedules.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Where(t1 => t1.IsActive
                                                            && !t1.IsDropped
                                                            && !t1.IsScheduleCancelled).Sum(t1 => t1.Quantity),
                    OrderClosingThreshold = order.FuelRequest.OrderClosingThreshold,
                    OrderMaxQuantity = order.BrokeredMaxQuantity,
                    FuelRequestMaxQuantity = order.FuelRequest.MaxQuantity,
                    FuelRequestTypeId = order.FuelRequest.FuelRequestTypeId
                };
                var orderAmount = ((scheduleInput.OrderClosingThreshold ?? 100) * (scheduleInput.OrderMaxQuantity ?? scheduleInput.FuelRequestMaxQuantity)) / 100;
                scheduleInput.RemainingQuantity = orderAmount - scheduleInput.DroppedQuantity;
                //get future schedules for a month and insert into trackable table
                var schedulesToAdd = GetSchedulesForAMonth(schedules.Select(t => t.Id).ToList(), maxDate, scheduleInput);
                await InsertTrackableSchedules(schedulesToAdd, order);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("TrackableScheduleDomain", "ProcessTrackableSchedules", ex.Message, ex);
            }
        }

        private async Task DeleteTrackableSchedules(IEnumerable<DeliverySchedule> schedules, DbSet<DeliveryScheduleXTrackableSchedule> trackableScheduleEntity, Order order, List<DeliverySchedule> deletedSchedules = null)
        {
            if (deletedSchedules != null && deletedSchedules.Count > 0)
            {
                List<int> deletedTrackableSchedules = new List<int>();
                foreach (var deletedschedule in deletedSchedules)
                {
                    IEnumerable<DeliveryScheduleXTrackableSchedule> deletedItems;
                    if (schedules.Any(t => t.GroupId == deletedschedule.GroupId))
                    {
                        deletedItems = trackableScheduleEntity.Where(t => t.OrderId == order.Id && t.DeliveryScheduleId == deletedschedule.Id).ToList();
                    }
                    else
                    {
                        deletedItems = trackableScheduleEntity.Where(t => t.DeliverySchedule != null && t.OrderId == order.Id && t.DeliverySchedule.GroupId == deletedschedule.GroupId).ToList();
                    }
                    deletedItems = deletedItems.Where(Extensions.IsTrackableScheduleUnDeliveredFunc()).Where(t => !t.Invoices.Any(t1 => t1.IsActiveInvoice)
                                                            && !t.IsScheduleMissed
                                                            && !t.IsScheduleCancelled);
                    var schedulesNottoDelete = deletedItems.Where(t => !t.IsActive && (t.DeliveryScheduleStatusId == (int)DeliveryScheduleStatus.Rescheduled || t.DeliveryScheduleStatusId == (int)TrackableDeliveryScheduleStatus.MissedAndRescheduled));

                    foreach (var scheduleNottoDelete in schedulesNottoDelete)
                    {
                        var groupId = Context.DataContext.DeliverySchedules.Where(t => t.Id == scheduleNottoDelete.DeliveryScheduleId).Select(t => t.GroupId).FirstOrDefault();
                        if (schedules.Any(t => t.GroupId == groupId))
                        {
                            deletedItems = deletedItems.Except(schedulesNottoDelete);
                        }
                    }
                    trackableScheduleEntity.RemoveRange(deletedItems);
                    deletedTrackableSchedules.AddRange(deletedItems.Select(t => t.Id));
                }
                //Update rescheduledtrackableid to null when schedule is modifying
                UpdateRescheduledTrackableId(deletedTrackableSchedules);
                await Context.CommitAsync();
            }
        }

        private void UpdateRescheduledTrackableId(List<int> deletedTrackableSchedules)
        {
            var referencedSchedules = Context.DataContext.DeliverySchedules.Where(t => deletedTrackableSchedules.Contains(t.RescheduledTrackableId ?? 0));
            foreach (var dschedule in referencedSchedules)
            {
                dschedule.RescheduledTrackableId = null;
                Context.DataContext.Entry(dschedule).State = EntityState.Modified;
            }
            Context.Commit();
        }

        private async Task InsertTrackableSchedules(List<DeliverySchedule> schedules, Order order)
        {
            foreach (var schedule in schedules)
            {
                var trackableSchedule = new DeliveryScheduleXTrackableSchedule();
                trackableSchedule.UoM = schedule.UoM;
                trackableSchedule.Date = schedule.Date;
                trackableSchedule.ShiftStartDate = schedule.Date;
                trackableSchedule.StartTime = schedule.StartTime;
                trackableSchedule.EndTime = schedule.EndTime;
                trackableSchedule.Quantity = schedule.Quantity;
                trackableSchedule.QuantityTypeId = schedule.QuantityTypeId;
                trackableSchedule.IsActive = true;
                trackableSchedule.DeliveryScheduleId = schedule.Id;
                trackableSchedule.CarrierId = schedule.CarrierId;
                trackableSchedule.SupplierSourceId = schedule.SupplierSourceId;
                trackableSchedule.SupplierContract = schedule.SupplierContract;
                trackableSchedule.LoadCode = schedule.LoadCode;
                trackableSchedule.DeliveryGroupId = schedule.DeliveryGroupId;
                trackableSchedule.DeliveryScheduleStatusId = schedule.StatusId == (int)TrackableDeliveryScheduleStatus.Rescheduled ? (int)TrackableDeliveryScheduleStatus.Rescheduled : (int)TrackableDeliveryScheduleStatus.Accepted;
                trackableSchedule.DriverId = order.Id > 0 && schedule.DeliveryScheduleXDrivers.Any(t => t.IsActive) ? schedule.DeliveryScheduleXDrivers.First(t => t.IsActive).DriverId : (int?)null;
                order.DeliveryScheduleXTrackableSchedules.Add(trackableSchedule);
            }
            await Context.CommitAsync();
        }

        public bool CheckIfScheduleDateIsWithinAMonth(DeliverySchedule schedule, DateTimeOffset jobTime)
        {
            if (schedule.Date.Date < jobTime.Date.AddDays(ApplicationConstants.FutureSchedulesAvailableFor))
                return true;
            else
                return false;
        }

        public bool IsTrackableScheduleExistForToday(ScheduleStatusUpdateInput order, DateTimeOffset scheduleDate, int groupId)
        {
            bool isScheduleExistForToday = false;
            isScheduleExistForToday = Context.DataContext.DeliveryScheduleXTrackableSchedules.Any(t => t.OrderId == order.OrderId && t.Date == scheduleDate && t.DeliverySchedule.GroupId == groupId);
            if (!isScheduleExistForToday && order.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)//Is schedule missed or canceled or delivered by original supplier
            {
                var parentOrder = Context.DataContext.Orders.Where(t => t.Id == order.OrderId).SelectMany(t => t.FuelRequest.FuelRequest1.Orders).Select(t => t.Id).FirstOrDefault();
                if (parentOrder > 0)
                {
                    isScheduleExistForToday = IsScheduleExistInParentOrder(parentOrder, scheduleDate, groupId);
                }
            }
            if (!isScheduleExistForToday && order.FuelRequestTypeId == (int)FuelRequestType.CounteredFuelRequest)//if parent request is counter offer ,verify it's parent is broker request or not
            {
                FuelRequest request = Context.DataContext.Orders.Where(t => t.Id == order.OrderId).Select(t => t.FuelRequest).FirstOrDefault();
                while (request.FuelRequest1 != null && request.FuelRequest1.FuelRequestTypeId != (int)FuelRequestType.CounteredFuelRequest)
                {
                    request = request.FuelRequest1;
                }
                if (request.FuelRequestTypeId == (int)FuelRequestType.BrokeredFuelRequest)
                {
                    var parentOrder = Context.DataContext.Orders.Where(t => t.Id == order.OrderId).SelectMany(t => t.FuelRequest.FuelRequest1.Orders).Select(t => t.Id).FirstOrDefault();
                    if (parentOrder > 0)
                    {
                        isScheduleExistForToday = IsScheduleExistInParentOrder(parentOrder, scheduleDate, groupId);
                    }
                }
            }
            return isScheduleExistForToday;
        }

        private bool IsScheduleExistInParentOrder(int orderId, DateTimeOffset scheduleDate, int groupId)
        {
            return Context.DataContext.DeliveryScheduleXTrackableSchedules.Any(t => t.OrderId == orderId && t.Date == scheduleDate && t.DeliverySchedule.GroupId == groupId);
        }

        public int GetDaysToAdd(int scheduleType)
        {
            int daysToAdd = scheduleType == (int)DeliveryScheduleType.Weekly ? 7 : 14;
            if (scheduleType == (int)DeliveryScheduleType.Monthly)
            {
                daysToAdd = 30;
            }
            return daysToAdd;
        }

        public DateTimeOffset GetScheduleDate(DeliverySchedule schedule, DateTimeOffset jobLocationTime, int scheduleDays)
        {
            DateTimeOffset scheduleDate;
            if (schedule.Date.Date.Add(schedule.EndTime) < jobLocationTime.DateTime)
            {
                int datediff = Math.Abs(schedule.Date.Date.Subtract(jobLocationTime.Date).Days) % scheduleDays;
                scheduleDate = datediff == 0 ? jobLocationTime.Date.AddDays(scheduleDays) : jobLocationTime.Date.AddDays(scheduleDays - datediff);
            }
            else
            {
                scheduleDate = schedule.Date;
            }
            return scheduleDate;
        }

        public List<DeliverySchedule> GetSchedulesForAMonth(List<int> schedules, DateTime maxDate, ScheduleStatusUpdateInput order)
        {
            if (order.RemainingQuantity > 0)
            {
                var futureScheduleList = GetPossibleSchedulesUptoMaxDate(order, schedules, order.RemainingQuantity, maxDate);
                decimal sum = 0;
                futureScheduleList = futureScheduleList.OrderBy(t => t.Date).ToList();
                var schedulesToAdd = (from schedule in futureScheduleList
                                      where (sum += schedule.Quantity) <= order.RemainingQuantity
                                      select schedule).ToList();
                decimal totalQuantity = schedulesToAdd.Sum(t => t.Quantity);
                if (futureScheduleList.Count > schedulesToAdd.Count && totalQuantity < order.RemainingQuantity)
                {
                    schedulesToAdd.Add(futureScheduleList[schedulesToAdd.Count]);
                }
                return schedulesToAdd;//schedules for remaining gallons upto one month
            }
            else
            {
                return new List<DeliverySchedule>();
            }
        }

        private List<DeliverySchedule> GetPossibleSchedulesUptoMaxDate(ScheduleStatusUpdateInput order, List<int> scheduleIds, decimal remainingGallons, DateTime maxDate)
        {
            var futureScheduleList = new List<DeliverySchedule>();
            DateTimeOffset jobLocationTime = DateTimeOffset.Now.ToTargetDateTimeOffset(order.TimeZoneName);
            var dschedules = Context.DataContext.DeliverySchedules.Where(t => scheduleIds.Contains(t.Id)).ToList();
            var jobEndDate = order.JobEndDate.HasValue ? order.JobEndDate.Value.AddDays(1) : (DateTimeOffset?)null;
            var deliveryEndDate = order.FuelRequestEndDate.HasValue ? order.FuelRequestEndDate.Value.AddDays(1) : jobEndDate;
            if (deliveryEndDate.HasValue && deliveryEndDate < maxDate)
                maxDate = deliveryEndDate.Value.DateTime;

            foreach (var schedule in dschedules)
            {
                decimal dropGallons = 0;
                if (schedule.Type == (int)DeliveryScheduleType.SpecificDates && schedule.Date.Date >= jobLocationTime.Date && schedule.Date.Date < maxDate &&
                    !Context.DataContext.DeliveryScheduleXTrackableSchedules.Any(t => t.OrderId == order.OrderId && t.DeliveryScheduleId == schedule.Id))
                {
                    futureScheduleList.Add(schedule);
                }
                else if (schedule.Type != (int)DeliveryScheduleType.SpecificDates)
                {
                    int scheduleDays = GetDaysToAdd(schedule.Type);
                    DateTimeOffset scheduleDate = GetScheduleDate(schedule, jobLocationTime, scheduleDays);
                    while (dropGallons <= remainingGallons && scheduleDate.Date < maxDate)
                    {
                        if (!IsTrackableScheduleExistForToday(order, scheduleDate, schedule.GroupId))
                        {
                            var newSchedule = new DeliverySchedule();
                            newSchedule.UoM = schedule.UoM;
                            newSchedule.Date = scheduleDate;
                            newSchedule.StartTime = schedule.StartTime;
                            newSchedule.EndTime = schedule.EndTime;
                            newSchedule.Id = schedule.Id;
                            newSchedule.Quantity = schedule.Quantity;
                            newSchedule.QuantityTypeId = schedule.QuantityTypeId;
                            newSchedule.CarrierId = schedule.CarrierId;
                            newSchedule.SupplierContract = schedule.SupplierContract;
                            newSchedule.SupplierSourceId = schedule.SupplierSourceId;
                            newSchedule.LoadCode = schedule.LoadCode;
                            if (schedule.StatusId == (int)DeliveryScheduleStatus.Rescheduled)
                            {
                                newSchedule.DeliveryGroupId = schedule.DeliveryGroupId;
                            }
                            schedule.DeliveryScheduleXDrivers.ToList().ForEach(t => newSchedule.DeliveryScheduleXDrivers.Add(t));
                            futureScheduleList.Add(newSchedule);
                            dropGallons += newSchedule.Quantity;
                        }
                        scheduleDate = scheduleDate.AddDays(scheduleDays);
                    }
                }
            }
            return futureScheduleList;
        }
    }
}
