using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.TJob.DeliveryAlerts
{
    public class TriggeredRecurringSchedule
    {
        static int defaultScheduleBuilderView = 2;
        public TriggeredRecurringSchedule()
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "TriggeredRecurringSchedule"))
            {
                //Register Context
                ContextFactory.Register(new ApplicationContext());

            }
        }
        public async Task<bool> ProcessrecurringscheduleAsync()
        {
            using (var tracer = new Tracer("TriggeredDeliveryAlerts", "ProcessrecurringscheduleAsync"))
            {
                try
                {
                    int intervalDay = -1;
                    int.TryParse(ConfigurationManager.AppSettings["PollingIntervalDay"].ToString(), out intervalDay);
                    if (intervalDay > -1)
                    {
                        for (int i = 0; i <= intervalDay; i++)
                        {


                            List<RecurringScheduleGroupInfo> recurringScheduleGroupInfos = new List<RecurringScheduleGroupInfo>();
                            string date;
                            FreightServiceDomain freightServiceDomain;
                            ScheduleBuilderDomain scheduleBuilderDomain;
                            List<RecurringSchedulesDetails> recurringSchedulesDetails;
                            GetRecurringScheduleDetails(out date, out freightServiceDomain, out scheduleBuilderDomain, out recurringSchedulesDetails, i);
                            if (recurringSchedulesDetails.Count > 0)
                            {
                                var checkOpenOrders = recurringSchedulesDetails.Select(x => x.OrderId).Distinct().ToList();

                                recurringSchedulesDetails = await scheduleBuilderDomain.RemoveCloseOrderSchedules(recurringSchedulesDetails, checkOpenOrders);
                                foreach (var recurringSCItem in recurringSchedulesDetails)
                                {
                                    GroupingRegionSchedules(recurringScheduleGroupInfos, date, recurringSCItem);
                                }
                                foreach (var recurringSchedule in recurringScheduleGroupInfos)
                                {
                                    await ProcessRecurringSchedule(freightServiceDomain, scheduleBuilderDomain, recurringSchedule);
                                }

                            }
                            else
                            {
                                LogManager.Logger.WriteInfo($"CJob.Workflow, ProcessrecurringscheduleAsync, RecurringSchedule .Record Processed: {recurringSchedulesDetails.Count() > 0}", "Main", "End");
                            }
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    LogManager.Logger.WriteException("TriggeredDeliveryAlerts", "ProcessrecurringscheduleAsync", "Exception Details : ", ex);
                    return false;
                }

            }
        }

        private static async Task ProcessRecurringSchedule(FreightServiceDomain freightServiceDomain, ScheduleBuilderDomain scheduleBuilderDomain, RecurringScheduleGroupInfo recurringSchedule)
        {
            UserContext userContext;
            List<RecurringShiftDetails> shiftInformation;
            AssignUserandShiftInfo(recurringSchedule, out userContext, out shiftInformation);
            var scheduleBuilder = await scheduleBuilderDomain.GetRecurringScheduleBuilderData(recurringSchedule.RegionId, recurringSchedule.Date, string.Empty, defaultScheduleBuilderView, userContext, shiftInformation, recurringSchedule.ScheduleBuilderId);
            if (scheduleBuilder != null)
            {

                var deliveryRequests = recurringSchedule.RecurringSchedulesDetails.SelectMany(top => top.DeliveryRequests).ToList();

                var createDeliveryRequests = await freightServiceDomain.CreateDeliveryRequest(deliveryRequests);
                if (createDeliveryRequests.Count > 0)
                {
                    foreach (var recitem in recurringSchedule.RecurringSchedulesDetails)
                    {
                        var deliveryData = createDeliveryRequests.Where(top => top.RecurringScheduleId == recitem.Id).ToList();
                        if (deliveryData != null)
                        {
                            recitem.DeliveryRequests.Clear();
                            if (recitem.ShiftInfo != null)
                            {
                               bool isDeliveryScheduleAssign= AssignShiftInformation(scheduleBuilder, recitem, deliveryData);
                                if(!isDeliveryScheduleAssign)
                                {
                                    //delete the delivery request that created.
                                    await scheduleBuilderDomain.DeleteDeliveryRequests(deliveryData.Select(x => x.Id).ToList());
                                }
                            }
                        }
                    }
                    if (!scheduleBuilder.Trips.Any(x => x.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published))
                    {
                        await SaveScheduleBuilderAsync(scheduleBuilderDomain, recurringSchedule, userContext, scheduleBuilder);
                    }
                    else
                    {
                        await PublishScheduleBuilderAsync(scheduleBuilderDomain, recurringSchedule, userContext, scheduleBuilder);

                    }
                }

            }
        }

        private static void GetRecurringScheduleDetails(out string date, out FreightServiceDomain freightServiceDomain, out ScheduleBuilderDomain scheduleBuilderDomain, out List<RecurringSchedulesDetails> recurringSchedulesDetails, int intervalDay)
        {
            DateTime processDay = DateTime.UtcNow.AddDays(intervalDay);
            Console.WriteLine("Process Day :" + processDay.ToString());
            string dayOfWeek = ((int)processDay.DayOfWeek == 0 ? 7 : (int)processDay.DayOfWeek).ToString();
            int currentDay = processDay.Day;
            date = processDay.ToString(Resource.constFormatDate);
            freightServiceDomain = ContextFactory.Current.GetDomain<FreightServiceDomain>();
            scheduleBuilderDomain = ContextFactory.Current.GetDomain<ScheduleBuilderDomain>();
            recurringSchedulesDetails = freightServiceDomain.GetRecurringScheduleDetails(dayOfWeek, currentDay, date).Result;
            Console.WriteLine("Process Day Completed:" + processDay.ToString());
        }

        private static void GroupingRegionSchedules(List<RecurringScheduleGroupInfo> recurringScheduleGroupInfos, string date, RecurringSchedulesDetails recurringSCItem)
        {
            var recordFound = recurringScheduleGroupInfos.Where(top => top.RegionId == recurringSCItem.RegionId).FirstOrDefault();
            if (recordFound != null)
            {
                recordFound.RecurringSchedulesDetails.Add(recurringSCItem);
            }
            else
            {
                RecurringScheduleGroupInfo scheduleGroupInfo = new RecurringScheduleGroupInfo();
                scheduleGroupInfo.RegionId = recurringSCItem.RegionId;
                scheduleGroupInfo.Date = date;
                scheduleGroupInfo.UserId = recurringSCItem.TfxUserId;
                scheduleGroupInfo.CompanyId = recurringSCItem.TfxCompanyId;
                scheduleGroupInfo.ScheduleBuilderId = recurringSCItem.ScheduleBuilderId;
                scheduleGroupInfo.RecurringSchedulesDetails.Add(recurringSCItem);
                recurringScheduleGroupInfos.Add(scheduleGroupInfo);
            }
        }

        private static void AssignUserandShiftInfo(RecurringScheduleGroupInfo recurringSchedule, out UserContext userContext, out List<RecurringShiftDetails> shiftInformation)
        {
            userContext = new UserContext();
            userContext.Id = recurringSchedule.UserId;
            userContext.CompanyId = recurringSchedule.CompanyId;
            shiftInformation = recurringSchedule.RecurringSchedulesDetails.GroupBy(g => new { g.ShiftInfo.ShiftId, g.ShiftInfo.ShiftIndex, g.ShiftInfo.DriverRowIndex })
                   .Select(g => new RecurringShiftDetails
                   {
                       ShiftId = g.Key.ShiftId,
                       DriverRowIndex = g.Key.DriverRowIndex,
                       ShiftIndex = g.Key.ShiftIndex,
                       DriverColIndex = g.Max(p => p.ShiftInfo.DriverColIndex)
                   }).ToList();
        }

        private static async Task PublishScheduleBuilderAsync(ScheduleBuilderDomain scheduleBuilderDomain, RecurringScheduleGroupInfo recurringSchedule, UserContext userContext, ScheduleBuilderViewModel scheduleBuilder)
        {
            var dsbSaveModel = scheduleBuilder.ToDsbSaveModel();
            dsbSaveModel.Trips = new List<TripViewModel>();
            dsbSaveModel.Trips.AddRange(scheduleBuilder.Trips);
            var saveScheduleBuilder = await scheduleBuilderDomain.PublishScheduleBuilder(dsbSaveModel, userContext);
            if (saveScheduleBuilder.StatusCode == (int)Status.Success)
            {
                LogManager.Logger.WriteInfo($"CJob.Workflow, ProcessrecurringscheduleAsync, RecurringSchedule-PublishSchedule .Created for Region : {recurringSchedule.RegionId}", "Main", "End");
            }
            else
            {
                LogManager.Logger.WriteError($"CJob.Workflow, ProcessrecurringscheduleAsync, RecurringSchedule-PublishSchedule" +
                    $" .Failed Created for Region : {recurringSchedule.RegionId + ":" + saveScheduleBuilder.StatusMessage}", "Main", "End");
            }
        }

        private static async Task SaveScheduleBuilderAsync(ScheduleBuilderDomain scheduleBuilderDomain, RecurringScheduleGroupInfo recurringSchedule, UserContext userContext, ScheduleBuilderViewModel scheduleBuilder)
        {
            var dsbSaveModel = scheduleBuilder.ToDsbSaveModel();
            dsbSaveModel.Trips = new List<TripViewModel>();
            dsbSaveModel.Trips.AddRange(scheduleBuilder.Trips);
            var saveScheduleBuilder = await scheduleBuilderDomain.SaveScheduleBuilder(dsbSaveModel, userContext);
            if (saveScheduleBuilder.StatusCode == (int)Status.Success)
            {
                LogManager.Logger.WriteInfo($"CJob.Workflow, ProcessrecurringscheduleAsync, RecurringSchedule .Created for Region : {recurringSchedule.RegionId}", "Main", "End");
            }
            else
            {
                LogManager.Logger.WriteError($"CJob.Workflow, ProcessrecurringscheduleAsync, RecurringSchedule .Failed Created for Region : {recurringSchedule.RegionId + ":" + saveScheduleBuilder.StatusMessage}", "Main", "End");
            }
        }

        private static bool AssignShiftInformation(ScheduleBuilderViewModel scheduleBuilder, RecurringSchedulesDetails recitem, List<DeliveryRequestViewModel> delData)
        {
            bool isDeliveryScheduleAssign = true;
            var shiftInfo = recitem.ShiftInfo;
            var shiftTripDetails = scheduleBuilder.Trips.Where(top => (top.DriverColIndex) == shiftInfo.DriverColIndex
                      && top.ShiftId == shiftInfo.ShiftId && (top.DriverRowIndex) == shiftInfo.DriverRowIndex).FirstOrDefault();
            if (shiftTripDetails != null)
            {
                if (shiftTripDetails.DeliveryGroupPrevStatus == DeliveryGroupStatus.Published)
                {
                    shiftTripDetails.TripStatus = TripStatus.Modified;
                    shiftTripDetails.DeliveryGroupStatus = DeliveryGroupStatus.Published;
                    recitem.DeliveryRequests.AddRange(delData);
                    recitem.DeliveryRequests.ForEach(t => t.Status = (int)DeliveryReqStatus.ScheduleCreated);
                    shiftTripDetails.DeliveryRequests.AddRange(recitem.DeliveryRequests);
                    string message = "Recurring ID :" + recitem;
                    LogManager.Logger.WriteInfo("ProcessRecurringSchedule", "ProcessRecurringSchedule-Published", message);
                }
                else
                {
                    shiftTripDetails.TripStatus = TripStatus.Modified;
                    shiftTripDetails.DeliveryGroupStatus = DeliveryGroupStatus.Draft;
                    shiftTripDetails.DeliveryGroupPrevStatus = DeliveryGroupStatus.None;
                    recitem.DeliveryRequests.AddRange(delData);
                    recitem.DeliveryRequests.ForEach(t => { t.ScheduleStatus = 14; t.Status = (int)DeliveryReqStatus.Draft; }); // Set ScheduleStatus to New DR
                    shiftTripDetails.DeliveryRequests.AddRange(recitem.DeliveryRequests);
                    string message = "Recurring ID :" + recitem;
                    LogManager.Logger.WriteInfo("ProcessRecurringSchedule", "ProcessRecurringSchedule-Saved", message);
                }
             
            }
            else
            {
                isDeliveryScheduleAssign = false;
            }
            return isDeliveryScheduleAssign;
        }
    }
}
