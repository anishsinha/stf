using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class DeliveryScheduleUpdationProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationDeliveryRequestViewModel viewModel;

        public EventType EventType => EventType.DeliveryRequestUpdated;

        public DeliveryScheduleUpdationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetDeliveryRequestNotificationDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            var callbackUrl = $"~/Buyer/Order/Details?id={viewModel.OrderId}";
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            var deliveryWindow = notificationDomain.GetModifiedScheduleDetails(viewModel.CurrentSchedules, viewModel.PreviousSchedules);
            //Send an email to buyer company admins
            SendDeliveryRequestUpdatedEmailToBuyerCompanyAdmins(viewModel, callbackUrl, deliveryWindow, (int)notificationEventViewModel.EventType);
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            var deliveryWindow = notificationDomain.GetModifiedScheduleDetails(viewModel.CurrentSchedules, viewModel.PreviousSchedules);
            //Send an email to supplier company admins
            SendDeliveryRequestUpdatedEmailToSupplierCompanyAdmins(viewModel, callbackUrl, deliveryWindow, (int)notificationEventViewModel.EventType);
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.UserRole == UserRoles.Buyer)
                {
                    ProcessDeliveryRequestUpdatedByBuyer(notificationEventViewModel, viewModel);
                }
                else
                {
                    ProcessDeliveryRequestUpdatedBySupplier(notificationEventViewModel, viewModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleUpdationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryRequestUpdatedByBuyer(NotificationEventViewModel notificationEvent, NotificationDeliveryRequestViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Order/Details?id={viewModel.OrderId}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestUpdated_Supplier, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                    var deliveryWindow = notificationDomain.GetModifiedScheduleDetails(viewModel.CurrentSchedules, viewModel.PreviousSchedules);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                    viewModel.BuyerCompanyName,
                                                    viewModel.PoNumber,
                                                    deliveryWindow);

                    notification.Subject = string.Format(notification.Subject, $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}", viewModel.BuyerCompanyName, viewModel.PoNumber);

                    SendNotification(viewModel.Supplier.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleUpdationProcessor", "ProcessDeliveryRequestUpdatedByBuyer", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryRequestUpdatedBySupplier(NotificationEventViewModel notificationEvent, NotificationDeliveryRequestViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Buyer/Order/Details?id={viewModel.OrderId}";
                    NotificationViewModel notification;
                    if (viewModel.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestUpdatedForTPO_Buyer, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                        notification.BodyButtonUrl = string.Empty;
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestUpdated_Buyer, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                    }
                    var deliveryWindow = notificationDomain.GetModifiedScheduleDetails(viewModel.CurrentSchedules, viewModel.PreviousSchedules);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    viewModel.SupplierCompanyName,
                                                    viewModel.PoNumber,
                                                    deliveryWindow);

                    notification.Subject = string.Format(notification.Subject, $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}", viewModel.SupplierCompanyName, viewModel.PoNumber);

                    SendNotification(viewModel.Buyer.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleUpdationProcessor", "ProcessDeliveryRequestUpdatedBySupplier", "Exception Details : ", ex);
            }
        }

        private void SendDeliveryRequestUpdatedEmailToBuyerCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, string deliveryWindow, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestUpdatedForTPO_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                    notification.BodyButtonUrl = string.Empty;
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestUpdated_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Buyer.Id)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                             viewModel.PoNumber,
                                                             deliveryWindow);
                        notification.Subject = string.Format(notification.Subject, $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}", viewModel.PoNumber);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendDeliveryRequestUpdatedEmailToSupplierCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, string deliveryWindow, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestUpdated_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Supplier.Id)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                             viewModel.PoNumber,
                                                             deliveryWindow);

                        notification.Subject = string.Format(notification.Subject, $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}", viewModel.PoNumber);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
