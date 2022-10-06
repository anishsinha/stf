using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Common;
using SiteFuel.Exchange.ViewModels.MobileAPI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace SiteFuel.Exchange.Domain
{
    public class PushNotificationDomain : BaseDomain
    {
        public PushNotificationDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public PushNotificationDomain(BaseDomain domain) : base(domain)
        {
        }

        public StatusViewModel PushNotification(MessageViewModel messageViewModel, int appTypeId, int oId = 0, int scheduleId = 0, List<int> userIds = null)
        {
            var response = new StatusViewModel();
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                string jsonDataFormat = string.Empty;
                if (messageViewModel.NotificationCode > 0)
                {
                    if ((appTypeId == (int)AppType.BuyerApp) ||(appTypeId == (int)AppType.NFNBuyer) || (appTypeId == (int)AppType.HandabandBuyer))
                    {
                        var payloadResponse = new
                        {
                            registration_ids = messageViewModel.FCMAppIds,
                            notification = new
                            {
                                title = messageViewModel.Title,
                                body = messageViewModel.Body,
                                sound = messageViewModel.Sound,
                                content_available = true,
                                show_in_foreground = true,
                                priority = "high",
                            },
                            data = new
                            {
                                orderId = oId,
                                deliveryScheduleId = scheduleId,
                                code = messageViewModel.NotificationCode,
                                content_available = true,
                                show_in_foreground = true,
                                priority = "high",
                                channelId = "sitefuel-channel"
                            }
                        };
                        jsonDataFormat = Newtonsoft.Json.JsonConvert.SerializeObject(payloadResponse);
                    }
                    else
                    {
                        var data = new
                        {
                            registration_ids = messageViewModel.FCMAppIds,
                            notification = new
                            {
                                title = messageViewModel.Title,
                                body = messageViewModel.Body,
                                sound = messageViewModel.Sound,
                            },
                            data = new
                            {
                                code = messageViewModel.NotificationCode,
                                messageCode = messageViewModel.MessageCode,
                                notification = new
                                {
                                    title = messageViewModel.Title,
                                    body = messageViewModel.Body,
                                    sound = messageViewModel.Sound,
                                },
                                sbDetails = messageViewModel.Data
                            },
                            content_available = true
                        };

                        jsonDataFormat = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    }
                }
                else
                {
                    var data = new
                    {
                        registration_ids = messageViewModel.FCMAppIds,
                        notification = new
                        {
                            title = messageViewModel.Title,
                            body = messageViewModel.Body,
                            sound = messageViewModel.Sound
                        },
                        content_available = true
                    };

                    jsonDataFormat = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }

                var serverKey = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingFCMServerKey);
                var senderId = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingFCMSenderId);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonDataFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (StreamReader tReader = new StreamReader(tResponse.GetResponseStream()))
                        {
                            String responseFromFirebaseServer = tReader.ReadToEnd();

                            FcmResponse fcmResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FcmResponse>(responseFromFirebaseServer);
                            if (fcmResponse.success >= 1)
                            {
                                response.StatusCode = Status.Success;
                                response.StatusMessage = Resource.errMessageSuccess;
                                SavePushNotificationLog(userIds, messageViewModel.NotificationCode, appTypeId, jsonDataFormat);
                            }
                            else if (fcmResponse.failure >= 1)
                            {
                                response.StatusCode = Status.Failed;
                                response.StatusMessage = Resource.errMessageFailed;
                                LogManager.Logger.WriteDebug("PushNotificationDomain", "Request-PushNotification", jsonDataFormat);
                                LogManager.Logger.WriteDebug("PushNotificationDomain", "Response-PushNotification", responseFromFirebaseServer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "PushNotification", ex.Message, ex);
            }
            return response;
        }

        public async Task NotificationToApprover_DropCompleted(int orderId)
        {
            try
            {
                //notify to approval user
                MessageViewModel messageViewModel = new MessageViewModel();
                var notifyUsers = new List<int>();
                notifyUsers.Add((int)FCMAppUserTypes.ApprovalUser);
                var orderDetails = await ContextFactory.Current.GetDomain<OrderDomain>().GetOrderDetailsAsync(orderId);
                messageViewModel.Title = Resource.notification_OrderDropped_Title;
                messageViewModel.Body = string.Format(Resource.notificationToBuyer_OrderDropped_Body, orderDetails.PoNumber);
                await NotificationToBuyer(messageViewModel, orderId, 0, notifyUsers);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "NotificationToApprover_DropCompleted", ex.Message, ex);
            }
        }

        public async Task<StatusViewModel> NotificationToBuyer(MessageViewModel messageViewModel, int orderId, int deliveryScheduleId = 0, List<int> notifyUsers = null)
        {
            var response = new StatusViewModel();
            try
            {
                if (messageViewModel.Notify)
                {
                    if (notifyUsers == null)
                    {
                        notifyUsers = new List<int>();
                        notifyUsers.Add((int)FCMAppUserTypes.FuelRequestCreatedBy);
                        notifyUsers.Add((int)FCMAppUserTypes.AssignedUser);
                    }

                    List<int> userIds = new List<int>();
                    var appType = await GetBrandedAppTypeByOrderId(orderId);
                    messageViewModel.FCMAppIds = await GetBuyerFCMAppId(orderId, notifyUsers, userIds, (int)appType);
                    if (messageViewModel.FCMAppIds.Count > 0)
                    {
                        response = PushNotification(messageViewModel, (int)appType, orderId, deliveryScheduleId, userIds);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "NotificationToBuyer", ex.Message, ex);
            }
            return response;
        }

        public async Task<StatusViewModel> NotificationToDriver(DriverNotificationViewModel viewModel)
        {
            var response = new StatusViewModel();
            response.StatusCode = Status.Failed;
            response.StatusMessage = Resource.errMessageFailedNotification;

            try
            {
                if (viewModel.Message.Notify)
                {
                    List<int> userIds = new List<int>();
                    viewModel.Message.FCMAppIds = await GetDriverFCMAppId(viewModel.DriverIds, userIds);
                    if (viewModel.Message.FCMAppIds.Any())
                    {
                        var notificationDomain = new PushNotificationDomain(this);
                        response = notificationDomain.PushNotification(viewModel.Message, (int)AppType.DriverApp, userIds: viewModel.DriverIds);
                    }
                    else
                    {
                        response.StatusMessage = Resource.errMessageDriverNotLoggedInApp;
                    }

                    if (response.StatusCode == Status.Success)
                    {
                        response.StatusCode = Status.Success;
                        response.StatusMessage = Resource.errMessagelSuccessDriverNotification;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "NotificationToDriver", ex.Message, ex);
            }
            return response;
        }

        public async Task<List<string>> GetDriverFCMAppId(List<int> driverIds, List<int> userIds)
        {
            using (var tracer = new Tracer("PushNotificationDomain", "GetDriverFCMAppId"))
            {
                List<string> response = new List<string>();
                try
                {
                    foreach (var item in driverIds)
                    {
                        var applocation = await Context.DataContext.AppLocations.Where(t => t.UserId == item).OrderByDescending(t => t.UpdatedDate).FirstOrDefaultAsync();
                        if (applocation != null && !applocation.IsUserLogout)
                        {
                            response.Add(applocation.FCMAppId);
                            userIds.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("PushNotificationDomain", "GetDriverFCMAppId", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task<List<string>> GetBuyerFCMAppId(int orderId, List<int> notifyUsers, List<int> userIds, int appType)
        {
            using (var tracer = new Tracer("PushNotificationDomain", "GetBuyerFCMAppId"))
            {
                List<string> response = new List<string>();
                try
                {
                    var order = await Context.DataContext.Orders.Include(t => t.FuelRequest).Include(t => t.FuelRequest.Job).Include(t => t.FuelRequest.Job.Users).SingleOrDefaultAsync(t => t.Id == orderId);
                    if (order != null)
                    {
                        if (notifyUsers.Contains((int)FCMAppUserTypes.AssignedUser))
                        {
                            var assignedUsers = order.FuelRequest.Job.Users.Where(t => t.IsActive).Select(t => t.Id).ToList();

                            foreach (var userId in assignedUsers)
                            {
                                // var applocation = Context.DataContext.AppLocations.Where(t => t.User.Id == userId && t.AppTypeId == (int)AppType.BuyerApp).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                                var applocation = Context.DataContext.AppLocations.Where(t => t.User.Id == userId && t.AppTypeId == appType).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                                if (applocation != null && !applocation.IsUserLogout)
                                {
                                    response.Add(applocation.FCMAppId);
                                    userIds.Add(userId);
                                }
                            }
                        }

                        if (notifyUsers.Contains((int)FCMAppUserTypes.FuelRequestCreatedBy))
                        {
                            int buyerId = order.FuelRequest.CreatedBy;
                            // var frCreatedUser = Context.DataContext.AppLocations.Where(t => t.UserId == buyerId && t.AppTypeId == (int)AppType.BuyerApp).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                            var frCreatedUser = Context.DataContext.AppLocations.Where(t => t.UserId == buyerId && t.AppTypeId == appType).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                            if (frCreatedUser != null && !frCreatedUser.IsUserLogout)
                            {
                                response.Add(frCreatedUser.FCMAppId);
                                userIds.Add(buyerId);
                            }
                        }

                        if (notifyUsers.Contains((int)FCMAppUserTypes.ApprovalUser))
                        {
                            var approvalUsers = order.FuelRequest.Job.JobXApprovalUsers.ToList();
                            foreach (var user in approvalUsers)
                            {
                                //var applocation = Context.DataContext.AppLocations.Where(t => t.UserId == user.UserId && t.AppTypeId == (int)AppType.BuyerApp).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                                var applocation = Context.DataContext.AppLocations.Where(t => t.UserId == user.UserId && t.AppTypeId == appType).OrderByDescending(x => x.UpdatedDate).FirstOrDefault();
                                if (applocation != null && !applocation.IsUserLogout)
                                {
                                    response.Add(applocation.FCMAppId);
                                    userIds.Add(user.UserId);
                                }
                            }
                        }

                        response = response.Distinct().ToList();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("PushNotificationDomain", "GetBuyerFCMAppId", ex.Message, ex);
                }

                return response;
            }
        }

        public async Task PushNotificationMessage(ComposeMessageViewModel viewModel)
        {
            var driverNotificationViewModel = new DriverNotificationViewModel();
            driverNotificationViewModel.DriverIds = viewModel.Recipients;
            driverNotificationViewModel.Message.Body = viewModel.PlainTextMessage;
            driverNotificationViewModel.Message.Title = viewModel.Subject;
            await NotificationToDriver(driverNotificationViewModel);
        }

        public async Task<StatusViewModel> PushNotificationReassignDriver(Order order, DeliverySchedule deliverySchedule, Nullable<int> driverId, int previousDriver)
        {
            var response = new StatusViewModel();
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var orderNumber = order.PoNumber;

            //Notification to previous assigned driver
            if (previousDriver != -1)
            {
                driverNotificationViewModel.DriverIds.Add(previousDriver);
                driverNotificationViewModel.Message.Title = Resource.notificationToDriver_RemovedFromDelivery_Title;
                driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToDriver_RemovedFromDelivery_Body, orderNumber);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Reassign;
                response = await NotificationToDriver(driverNotificationViewModel);
            }

            //Notification to new assigned driver
            if (driverId.HasValue)
            {
                driverNotificationViewModel = new DriverNotificationViewModel();
                driverNotificationViewModel.DriverIds.Add(driverId.Value);

                var startTime = string.Empty;
                var endTime = string.Empty;
                var date = string.Empty;
                if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.OneTimeDelivery)
                {
                    startTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString();
                    endTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();
                    date = order.FuelRequest.FuelRequestDetail.StartDate.Date.ToString(Resource.constFormatDate);
                }
                else if (order.FuelRequest.FuelRequestDetail.DeliveryTypeId == (int)DeliveryType.MultipleDeliveries)
                {
                    startTime = Convert.ToDateTime(deliverySchedule.StartTime.ToString()).ToShortTimeString();
                    endTime = Convert.ToDateTime(deliverySchedule.EndTime.ToString()).ToShortTimeString();
                    date = deliverySchedule.Date.ToString(Resource.constFormatDate);
                }

                driverNotificationViewModel.Message.Title = Resource.notificationToDriver_AssignedToDelivery_Title;
                driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToDriver_AssignedToDelivery_Body, orderNumber, date, $"{startTime} {Resource.lblSingleHyphen} {endTime}");
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.NewScheduleAssign;
                response = await NotificationToDriver(driverNotificationViewModel);
            }
            return response;
        }

        public async Task<StatusViewModel> PushSbChangesNotificationToDriver(List<ScheduleNotificationModel> sbChangesModel, string userName)
        {
            var response = new StatusViewModel();
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var drivers = sbChangesModel.Where(t => t.DriverId != null).GroupBy(t => t.DriverId.Value);
            foreach (var driver in drivers)
            {
                if (!driver.Any())
                {
                    continue;
                }
                driverNotificationViewModel.DriverIds.Add(driver.Key);
                if (driver.All(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.New || t.ScheduleStatus == (int)DeliveryScheduleStatus.Accepted))
                {
                    driverNotificationViewModel.Message.Title = Resource.notificationToDriver_DeliveryGroupAssigned_Title;
                    driverNotificationViewModel.Message.Body = Resource.notificationToDriver_DeliveryGroupAssigned_Body;
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.NewDeliveryGroupAssign;
                    driverNotificationViewModel.Message.MessageCode = (int)MessageCodes.NewDeliveryGroupAssign;
                }
                else if (driver.All(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.Canceled))
                {
                    driverNotificationViewModel.Message.Title = Resource.notificationToDriver_DeliveryGroupDeleted_Title;
                    driverNotificationViewModel.Message.Body = Resource.notificationToDriver_DeliveryGroupDeleted_Body;
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.DeliveryGroupDeleted;
                    driverNotificationViewModel.Message.MessageCode = (int)MessageCodes.DeliveryGroupDeleted;
                }
                else if (driver.All(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.Missed))
                {
                    driverNotificationViewModel.Message.Title = Resource.notificationToDriver_DeliveryGroupDeleted_Title;
                    driverNotificationViewModel.Message.Body = Resource.notificationToDriver_DeliveryGroupMissed_Body;
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.DeliveryGroupDeleted;
                    driverNotificationViewModel.Message.MessageCode = (int)MessageCodes.DeliveryGroupMissed;
                }
                else
                {
                    driverNotificationViewModel.Message.Title = Resource.notificationToDriver_DeliveryGroupModified_Title;
                    driverNotificationViewModel.Message.Body = Resource.notificationToDriver_DeliveryGroupModified_Body;
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.DeliveryGroupModified;
                    driverNotificationViewModel.Message.MessageCode = (int)MessageCodes.DeliveryGroupModified;
                }
                driverNotificationViewModel.Message.Data = driver.ToList();
                var requestJson = JsonConvert.SerializeObject(driverNotificationViewModel);
                response = await NotificationToDriver(driverNotificationViewModel);
                var responseModelJson = JsonConvert.SerializeObject(response);
                if (response.StatusMessage == Resource.errMessageFailed)
                {
                    response.StatusMessage = Resource.errMessagePushNotificationFailed;
                }
                LogManager.Logger.WriteAPIInfo(userName, "PushNotificationDomain", "PushSbChangesNotificationToDriver", requestJson, responseModelJson, 0, "Azure-AppService", DateTime.Now, DateTime.Now);
            }
            response = await NotificationToPreviousDriver(sbChangesModel, response, driverNotificationViewModel, userName);
            return response;
        }

        private async Task<StatusViewModel> NotificationToPreviousDriver(List<ScheduleNotificationModel> sbChangesModel, StatusViewModel response, DriverNotificationViewModel driverNotificationViewModel, string userName)
        {
            List<int> currentDrivers = sbChangesModel.Where(t => t.DriverId.HasValue).Select(t => t.DriverId.Value).ToList();
            var previousDrivers = sbChangesModel.Where(t => t.PreviousDriverId != null && !currentDrivers.Contains(t.PreviousDriverId.Value)).GroupBy(t => t.PreviousDriverId);
            foreach (var previousDriver in previousDrivers)
            {
                driverNotificationViewModel.DriverIds.Add(previousDriver.Key.Value);
                if (previousDriver.All(t => t.ScheduleStatus == (int)DeliveryScheduleStatus.Canceled))
                {
                    driverNotificationViewModel.Message.Title = Resource.notificationToDriver_DeliveryGroupUnAssigned_Title;
                    driverNotificationViewModel.Message.Body = Resource.notificationToDriver_DeliveryGroupUnAssigned_Body;
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.DeliveryGroupDeleted;
                    driverNotificationViewModel.Message.MessageCode = (int)MessageCodes.DeliveryGroupUnassigned;
                }
                else
                {
                    driverNotificationViewModel.Message.Title = Resource.notificationToDriver_DeliveryGroupModified_Title;
                    driverNotificationViewModel.Message.Body = Resource.notificationToDriver_DeliveryGroupModified_Body;
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.DeliveryGroupModified;
                    driverNotificationViewModel.Message.MessageCode = (int)MessageCodes.DeliveryGroupModified;
                }
                driverNotificationViewModel.Message.Data = previousDriver.ToList();
                var requestJson = JsonConvert.SerializeObject(driverNotificationViewModel);
                response = await NotificationToDriver(driverNotificationViewModel);
                var responseModelJson = JsonConvert.SerializeObject(response);
                LogManager.Logger.WriteAPIInfo(userName, "PushNotificationDomain", "NotificationToPreviousDriver", requestJson, responseModelJson, 0, "Azure-AppService", DateTime.Now, DateTime.Now);
            }
            if (response.StatusMessage == Resource.errMessageFailed)
            {
                response.StatusMessage = Resource.errMessagePushNotificationFailed;
            }
            return response;
        }

        public InActiveDriverViewModel EmailNotificationForInActiveDriver(NotificationEventViewModel notification)
        {
            var driverMessage = JsonConvert.DeserializeObject<InActiveDriverMessageViewModel>(notification.JsonMessage);

            var order = Context.DataContext.Orders.FirstOrDefault(t => t.Id == driverMessage.OrderId && t.IsActive);
            var driver = Context.DataContext.Users.FirstOrDefault(t => t.Id == notification.EntityId);
            var response = new InActiveDriverViewModel()
            {
                CompanyName = order.Company.Name,
                DriverFirstName = driver.FirstName,
                DriverLastName = driver.LastName,
                PONumber = order.PoNumber,
                StartDate = driverMessage.DeliveryDate,
                CompanyId = order.Company.Id,
                DriverInvitedDate = driver.CreatedDate
            };
            return response;
        }

        public async Task PushNotificationScheduleCancel(UserContext user, DeliveryScheduleXTrackableSchedule trackableSchedule, bool isBuyer)
        {
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var messageViewModel = new MessageViewModel();
            var order = trackableSchedule.Order;
            var orderNumber = order.PoNumber;
            var startTime = Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToShortTimeString();
            var endTime = Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToShortTimeString();

            //Notification to driver
            if (trackableSchedule.DriverId.HasValue)
            {
                driverNotificationViewModel.DriverIds.Add(trackableSchedule.DriverId.Value);
                driverNotificationViewModel.Message.Title = Resource.notification_CancelSchedule_Title;
                driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToDriver_CancelSchedule_Body, $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}", orderNumber, startTime, endTime);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Cancel;
                await NotificationToDriver(driverNotificationViewModel);
            }

            //Notification to supplier (If buyer cancel's, send notification to supplier)
            if (isBuyer)
            {
                driverNotificationViewModel = new DriverNotificationViewModel();
                driverNotificationViewModel.DriverIds.Add(order.AcceptedBy);
                driverNotificationViewModel.Message.Title = Resource.notification_CancelSchedule_Title;
                driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToSupplier_CancelSchedule_Body, orderNumber, $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}", startTime, endTime, user.Name, user.CompanyName);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Cancel;
                await NotificationToDriver(driverNotificationViewModel);
            }

            //Notification to buyer (If supplier cancel's, send notification to buyer)
            if (!isBuyer)
            {
                messageViewModel.Title = Resource.notification_CancelSchedule_Title;
                messageViewModel.Body = string.Format(Resource.notificationToBuyer_CancelSchedule_Body, orderNumber, order.FuelRequest.Job.Name, startTime, endTime);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Cancel;
                await NotificationToBuyer(messageViewModel, order.Id);
            }
        }

        public async Task PushNotificationOrderCancel(UserContext user, Order order, bool isBuyer)
        {
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var messageViewModel = new MessageViewModel();
            var orderNumber = order.PoNumber;
            var startTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.StartTime.ToString()).ToShortTimeString();
            var endTime = Convert.ToDateTime(order.FuelRequest.FuelRequestDetail.EndTime.ToString()).ToShortTimeString();

            //Notification to driver
            var assignedDriver = order.OrderXDrivers.SingleOrDefault(t => t.IsActive);
            if (assignedDriver != null)
            {
                driverNotificationViewModel.DriverIds.Add(assignedDriver.DriverId);
                driverNotificationViewModel.Message.Title = Resource.notification_CancelSchedule_Title;
                driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToDriver_CancelSchedule_Body, $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}", orderNumber, startTime, endTime);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Cancel;
                await NotificationToDriver(driverNotificationViewModel);
            }

            //Notification to supplier (If buyer cancel's, send notification to supplier)
            if (isBuyer)
            {
                driverNotificationViewModel = new DriverNotificationViewModel();
                driverNotificationViewModel.DriverIds.Add(order.AcceptedBy);
                driverNotificationViewModel.Message.Title = Resource.notification_CancelSchedule_Title;
                driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToSupplier_CancelSchedule_Body, orderNumber, $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}", startTime, endTime, user.Name, user.CompanyName);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Cancel;
                await NotificationToDriver(driverNotificationViewModel);
            }

            //Notification to buyer (If supplier cancel's, send notification to buyer)
            if (!isBuyer)
            {
                messageViewModel.Title = Resource.notification_CancelSchedule_Title;
                messageViewModel.Body = string.Format(Resource.notificationToBuyer_CancelSchedule_Body, orderNumber, order.FuelRequest.Job.Name, startTime, endTime);
                driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Cancel;
                await NotificationToBuyer(messageViewModel, order.Id);
            }
        }

        public async Task PushNotificationSbChangesToDriver()
        {
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var messageViewModel = new MessageViewModel();
            messageViewModel.Title = Resource.notification_DeliveryRescheduled_Title;
            //messageViewModel.Body = string.Format(Resource.notificationToBuyer_RescheduledToDifferentDate_Body, orderNumber, order.FuelRequest.Job.Name, startTime, endTime, rescheduleDate, rescheduleStartTime, rescheduleEndTime);
            driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Reschedule;
            await NotificationToDriver(driverNotificationViewModel);
        }

        public async Task PushNotificationRescheduleDeliverySchedule(RescheduleDeliveryViewModel viewModel, DeliveryScheduleXTrackableSchedule trackableSchedule)
        {
            var driverNotificationViewModel = new DriverNotificationViewModel();
            var messageViewModel = new MessageViewModel();
            var order = trackableSchedule.Order;
            var orderNumber = order.PoNumber;
            var startTime = Convert.ToDateTime(trackableSchedule.StartTime.ToString()).ToShortTimeString();
            var endTime = Convert.ToDateTime(trackableSchedule.EndTime.ToString()).ToShortTimeString();

            var rescheduleStartTime = viewModel.StartTime;
            var rescheduleEndTime = viewModel.EndTime;
            var rescheduleDate = viewModel.DeliveryDate.Date.ToString(Resource.constFormatDate);

            if (trackableSchedule.DriverId.HasValue)
            {
                if (viewModel.DeliveryDate.Date != trackableSchedule.Date.Date)
                {
                    //Notification to driver : Rescheduled to different date
                    driverNotificationViewModel.DriverIds.Add(trackableSchedule.DriverId.Value);
                    driverNotificationViewModel.Message.Title = Resource.notification_DeliveryRescheduled_Title;
                    driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToDriver_RescheduledToDifferentDate_Body, $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}", orderNumber, startTime, endTime, rescheduleDate, rescheduleStartTime, rescheduleEndTime);
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Reschedule;
                    await NotificationToDriver(driverNotificationViewModel);

                    //Notification to buyer : Rescheduled to different date
                    messageViewModel = new MessageViewModel();
                    messageViewModel.Title = Resource.notification_DeliveryRescheduled_Title;
                    messageViewModel.Body = string.Format(Resource.notificationToBuyer_RescheduledToDifferentDate_Body, orderNumber, order.FuelRequest.Job.Name, startTime, endTime, rescheduleDate, rescheduleStartTime, rescheduleEndTime);
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Reschedule;
                    await NotificationToBuyer(messageViewModel, order.Id);
                }
                else
                {
                    //Notification to driver : Rescheduled to different time
                    driverNotificationViewModel.DriverIds.Add(trackableSchedule.DriverId.Value);
                    driverNotificationViewModel.Message.Title = Resource.notification_DeliveryRescheduled_Title;
                    driverNotificationViewModel.Message.Body = string.Format(Resource.notificationToDriver_RescheduledToDifferentTime_Body, $"{order.FuelRequest.User.FirstName} {order.FuelRequest.User.LastName}", orderNumber, startTime, endTime, rescheduleStartTime, rescheduleEndTime);
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Reschedule;
                    await NotificationToDriver(driverNotificationViewModel);

                    //Notification to buyer : Rescheduled to different time
                    messageViewModel.Title = Resource.notification_DeliveryRescheduled_Title;
                    messageViewModel.Body = string.Format(Resource.notificationToBuyer_RescheduledToDifferentTime_Body, orderNumber, order.FuelRequest.Job.Name, startTime, endTime, rescheduleStartTime, rescheduleEndTime);
                    driverNotificationViewModel.Message.NotificationCode = (int)NotificationCode.Reschedule;
                    await NotificationToBuyer(messageViewModel, order.Id);
                }
            }
        }

        public void SavePushNotificationLog(List<int> userIds, int notificationCode, int appTypeId, string jsonMessage)
        {
            try
            {
                if (userIds != null && userIds.Any())
                {
                    List<PushNotificationLog> pushNotificationLogs = new List<PushNotificationLog>();
                    foreach (var item in userIds)
                    {
                        PushNotificationLog pushNotificationLog = new PushNotificationLog();
                        pushNotificationLog.AppTypeId = appTypeId;
                        pushNotificationLog.NotificationCode = notificationCode;
                        pushNotificationLog.UserId = item;
                        pushNotificationLog.IsActive = true;
                        pushNotificationLog.IsRead = false;
                        pushNotificationLog.JsonMessage = jsonMessage;
                        pushNotificationLog.CreatedDate = DateTimeOffset.UtcNow;
                        pushNotificationLogs.Add(pushNotificationLog);
                    }
                    if (pushNotificationLogs != null && pushNotificationLogs.Any())
                    {
                        Context.DataContext.PushNotificationLogs.AddRange(pushNotificationLogs);
                        Context.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "SavePushNotificationLog", ex.Message, ex);
            }

        }
        public async Task<IntegerResponseModel> GetUnreadNotificationsCount(int userId, int notificationCode = 0, DateTimeOffset? createdDate = null, int appTypeId= 0)
        {
            IntegerResponseModel response = new IntegerResponseModel();
           
           
            try
            {
                response.Result = await Context.DataContext.PushNotificationLogs.Where(t => t.UserId == userId && t.IsActive && !t.IsRead && t.AppTypeId == appTypeId
                                                                                            && (notificationCode == 0 || t.NotificationCode == notificationCode)
                                                                                            && (createdDate == null || DbFunctions.TruncateTime(t.CreatedDate) == DbFunctions.TruncateTime(createdDate.Value))).CountAsync();
                response.StatusCode = Status.Success;
                response.StatusMessage = Status.Success.ToString();
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "GetUnreadNotificationCount", ex.Message, ex);
            }
            return response;
        }
        public async Task<List<PushNotificationViewModel>> GetPushNotificationLogs(int userId, int notificationCode = 0, DateTimeOffset? createdDate = null,int appTypeId = 0)
        {
            var response = new List<PushNotificationViewModel>();
            try
            {
                var pushNotificationLogs = await Context.DataContext.PushNotificationLogs.Where(t => t.UserId == userId && t.IsActive && t.AppTypeId == appTypeId
                                                                                            && (notificationCode == 0 || t.NotificationCode == notificationCode)
                                                                                            && (createdDate == null || DbFunctions.TruncateTime(t.CreatedDate) == DbFunctions.TruncateTime(createdDate.Value))).ToListAsync();
                if (pushNotificationLogs != null && pushNotificationLogs.Any())
                {
                    //Marked the notification status as Read while sending the list of notifications
                    pushNotificationLogs.Where(t => t.IsRead == false).ToList().ForEach(t1 => t1.IsRead = true);
                    Context.CommitAsync();

                    var model = new PushNotificationViewModel();
                    foreach (var item in pushNotificationLogs)
                    {
                        model = JsonConvert.DeserializeObject<PushNotificationViewModel>(item.JsonMessage);
                        model.Id = item.Id;
                        model.IsRead = item.IsRead;
                        response.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "GetPushNotificationLogs", ex.Message, ex);
            }
            return response;
        }
        public async Task<StatusViewModel> ClearNotificationForBuyerApp(ClearNotificationInputModel inputModel)
        {
            var response = new StatusViewModel();
            try
            {
                var pushNotificationLogs = await Context.DataContext.PushNotificationLogs.Where(t => t.UserId == inputModel.UserId && inputModel.NotificationIds.Contains(t.Id)).ToListAsync();
                if (pushNotificationLogs != null && pushNotificationLogs.Any())
                {
                    pushNotificationLogs.ForEach(t => t.IsActive = false);
                    Context.Commit();
                }
                response.StatusCode = Status.Success;
                response.StatusMessage = Resource.errMessageSuccess;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "ClearNotificationForBuyerApp", ex.Message, ex);
            }
            return response;
        }

        public async Task<AppType> GetBrandedAppTypeByOrderId(int orderId)
        {
            var appType = AppType.BuyerApp;
            try
            {
                var filters = new List<string>
                {
                    Constants.NFNSupplierCompanyId,
                    Constants.HandaBandSupplierCompanyId                   
                };
                var companyConfigs = Context.DataContext.MstAppSettings.Where(t => filters.Contains(t.Key)).ToList();

                string strNfnSupplierCompanyId = companyConfigs.Where(t => t.Key == Constants.NFNSupplierCompanyId).Select(t => t.Value).FirstOrDefault();
                string strHandaBandSupplierCompanyId = companyConfigs.Where(t => t.Key == Constants.HandaBandSupplierCompanyId).Select(t => t.Value).FirstOrDefault();
                int NFNSupplierCompanyId = 0;
                int HandaBandSupplierCompanyId = 0;
                var orderAcceptedCompanyId = await Context.DataContext.Orders.Where(t => t.Id == orderId).Select(t => t.AcceptedCompanyId).FirstOrDefaultAsync();
                NFNSupplierCompanyId  = int.Parse(strNfnSupplierCompanyId);
                HandaBandSupplierCompanyId = int.Parse(strHandaBandSupplierCompanyId);
                if (orderAcceptedCompanyId == NFNSupplierCompanyId)
                {
                    appType = AppType.NFNBuyer;
                }
                else if (orderAcceptedCompanyId == HandaBandSupplierCompanyId)
                {
                    appType = AppType.HandabandBuyer;
                }                
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PushNotificationDomain", "GetBrandedAppTypeByOrderId", ex.Message, ex);
            }
            return appType;
        }
        public class FcmResponse
        {
            public long multicast_id { get; set; }
            public int success { get; set; }
            public int failure { get; set; }
            public int canonical_ids { get; set; }
            public List<FcmResult> results { get; set; }
        }

        public class FcmResult
        {
            public string message_id { get; set; }
        }
    }
}
