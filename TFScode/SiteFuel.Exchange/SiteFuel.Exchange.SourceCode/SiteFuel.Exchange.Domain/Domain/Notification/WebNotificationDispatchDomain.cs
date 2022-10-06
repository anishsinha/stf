using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.Domain
{
    public class WebNotificationDispatchDomain : BaseDomain
    {
        public WebNotificationDispatchDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public WebNotificationDispatchDomain(BaseDomain domain) : base(domain)
        {
        }

        //internal void ProcessDispatchJsonMessage(NotificationDispatchLocationViewModel dispatchQueMsg, List<string> errorInfo)
        //{
        //    using (var tracer = new Tracer("WebNotificationDispatchDomain", "ProcessDispatchJsonMessage"))
        //    {
        //        StringBuilder processMessage = new StringBuilder();
        //        try
        //        {
        //            if (dispatchQueMsg.OrderId > 0)
        //            {
        //                List<WebNotification> dispatchWebNotifications = new List<WebNotification>();
        //                var dispatchOrder = Context.DataContext.Orders.Select(x => new
        //                {
        //                    x.Id,
        //                    x.BuyerCompanyId,
        //                    jobCompanyId = x.FuelRequest.Job.CompanyId,
        //                    SupplierUsers = x.Company.Users.Where(x1 => x1.IsActive && x1.MstRoles.Any(r => r.Id == (int)UserRoles.Admin || r.Id == (int)UserRoles.Supplier))
        //                                                                .Select(t1 => new { t1.Id, t1.CompanyId, t1.Company.CompanyTypeId }),
        //                    BuyerCompanyUsers = x.BuyerCompany.Users.Where(x1 => x1.IsActive && x1.MstRoles.Any(r => r.Id == (int)UserRoles.Admin || r.Id == (int)UserRoles.Buyer))
        //                                                                .Select(t1 => new { t1.Id, t1.CompanyId, t1.Company.CompanyTypeId }),
        //                    UserAssignedToJob = x.FuelRequest.Job.Users.Where(x1 => x1.IsActive && x1.MstRoles.Any(r => r.Id == (int)UserRoles.Buyer))
        //                                                                .Select(t1 => new { t1.Id, t1.CompanyId, t1.Company.CompanyTypeId })
        //                }).SingleOrDefault(t => t.Id == dispatchQueMsg.OrderId);

        //               var allUsers = dispatchOrder.SupplierUsers;
        //                if (dispatchOrder.BuyerCompanyId == dispatchOrder.jobCompanyId)
        //                {
        //                    allUsers = allUsers.Concat(dispatchOrder.BuyerCompanyUsers).Concat(dispatchOrder.UserAssignedToJob);
        //                }
        //                else
        //                {
        //                    allUsers = allUsers.Concat(dispatchOrder.BuyerCompanyUsers);
        //                }
        //                if (allUsers.Count() > 0)
        //                {
        //                    var jsonMessage = GetDipatchWebNotificationJson(dispatchQueMsg);
        //                    foreach (var user in allUsers.Distinct())
        //                    {
        //                        dispatchWebNotifications.Add(new WebNotification()
        //                        {
        //                            CreatedBy = dispatchQueMsg.CreatedByUserId,
        //                            CreatedDate = DateTimeOffset.Now,
        //                            CreatedFor = user.Id,
        //                            CreatedForCompanyId = user.CompanyId ?? 0,
        //                            CreatedForCompanyTypeId = user.CompanyTypeId,
        //                            EntityId = dispatchQueMsg.OrderId,
        //                            IsNotificationRead = false,
        //                            NotificationTypeId = (int)WebNotificationType.DispatchNotification,
        //                            JsonMessage = jsonMessage
        //                        });
        //                    }
        //                    Context.DataContext.WebNotifications.AddRange(dispatchWebNotifications);
        //                    Context.Commit();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (!(ex is QueueMessageFatalException))
        //                LogManager.Logger.WriteException("WebNotificationDispatchDomain", "ProcessDispatchJsonMessage", ex.Message, ex);
        //            if (processMessage.Length == 0)
        //            {
        //                processMessage.Append(Constants.RequestError);
        //                errorInfo.Add(processMessage.ToString());
        //            }
        //            throw new QueueMessageFatalException(errorInfo[0], errorInfo);
        //        }
        //    }
        //}

        //private string GetDipatchWebNotificationJson(NotificationDispatchLocationViewModel dispatchQueMsg)
        //{
        //    var jsonViewModel = new WebNotificationDispatchJson();
        //    jsonViewModel.CreatedByCompanyName = dispatchQueMsg.CreatedByCompanyName;
        //    jsonViewModel.OrderNumber = dispatchQueMsg.OrderNumber;
        //    jsonViewModel.OrderId = dispatchQueMsg.OrderId;
        //    jsonViewModel.DriverId = dispatchQueMsg.CreatedByUserId;
        //    jsonViewModel.NotificaitonText = GetNotificationText(dispatchQueMsg);
        //    string json = JsonConvert.SerializeObject(jsonViewModel);
        //    return json;
        //}

        private string GetNotificationText(NotificationDispatchLocationViewModel dispatchQueMsg)
        {
            if (dispatchQueMsg.DispatchNotificationType == DispatchNotificationType.EnrouteStatus)
            {
                switch (dispatchQueMsg.Status)
                {
                    case EnrouteDeliveryStatus.OnTheWayToTerminal:
                        return $"{dispatchQueMsg.CreatedByUserName} is on his way to Terminal {dispatchQueMsg.CurrentTerminalName}";

                    case EnrouteDeliveryStatus.ArrivedAtTerminal:
                        return $"{dispatchQueMsg.CreatedByUserName} has arrived at Terminal {dispatchQueMsg.CurrentTerminalName}";

                    case EnrouteDeliveryStatus.OnTheWayToJob:
                        return $"{dispatchQueMsg.CreatedByUserName} is on his way to Job {dispatchQueMsg.JobName}";

                    case EnrouteDeliveryStatus.ArrivedAtJob:
                        return $"{dispatchQueMsg.CreatedByUserName} has arrived at Job {dispatchQueMsg.JobName}";
                }
            }
            else if (dispatchQueMsg.DispatchNotificationType == DispatchNotificationType.TerminalChange)
            {
                return $"{dispatchQueMsg.CreatedByUserName} has changed the Terminal from {dispatchQueMsg.PreviousTerminalName} to {dispatchQueMsg.CurrentTerminalName}";
            }
            else if (dispatchQueMsg.DispatchNotificationType == DispatchNotificationType.Reschedule)
            {
                return $"Your Delivery has been rescheduled for Order {dispatchQueMsg.OrderNumber}";
            }
            else if (dispatchQueMsg.DispatchNotificationType == DispatchNotificationType.CancelDelivery)
            {
                return $"Your Delivery has been cancelled for Order {dispatchQueMsg.OrderNumber}";
            }
            return string.Empty;
        }
    }
}