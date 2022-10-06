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
	public class DeliveryRequestRescheduledProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationDeliveryRequestViewModel viewModel;

        public EventType EventType => EventType.DeliveryRequestRescheduled;

        public DeliveryRequestRescheduledProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetDeliveryRequestNotificationDetails(notificationEventViewModel.EntityId);
        }

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            var callbackUrl = $"~/Buyer/Order/Details/{viewModel.OrderId}";
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled && t.CreatedBy == viewModel.Buyer.Id);
            var deliveryWindow = notificationDomain.GetRescheduledScheduleDetails(newSchedules);

            //Send an email to buyer company admins
            SendDeliveryRequestRescheduledEmailToBuyerCompanyAdmins(viewModel, callbackUrl, deliveryWindow.ToString());
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled && t.CreatedBy == viewModel.Supplier.Id);
            var deliveryWindow = notificationDomain.GetRescheduledScheduleDetails(newSchedules);

            //Send an email to supplier company admins
            SendDeliveryRequestRescheduledEmailToSupplierCompanyAdmins(viewModel, callbackUrl, deliveryWindow.ToString());
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.UserRole == UserRoles.Buyer)
                {
                    ProcessDeliveryRequestRescheduledByBuyer(notificationEventViewModel, viewModel);
                }
                else
                {
                    ProcessDeliveryRequestRescheduledBySupplier(notificationEventViewModel, viewModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRescheduledProcessor", "ProcessDeliveryRequestCreated", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryRequestRescheduledByBuyer(NotificationEventViewModel notificationEvent, NotificationDeliveryRequestViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestRescheduled_Supplier, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                    var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled && t.CreatedBy == viewModel.Buyer.Id);

                    var deliveryWindow = notificationDomain.GetRescheduledScheduleDetails(newSchedules);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                    viewModel.BuyerCompanyName,
                                                    viewModel.PoNumber,
                                                    deliveryWindow.ToString());

                    notification.Subject = string.Format(notification.Subject, $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}", viewModel.BuyerCompanyName, viewModel.PoNumber);

                    SendNotification(viewModel.Supplier.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRescheduledProcessor", "ProcessDeliveryRequestCreatedByBuyer", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryRequestRescheduledBySupplier(NotificationEventViewModel notificationEvent, NotificationDeliveryRequestViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Buyer/Order/Details/{viewModel.OrderId}";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestRescheduled_Buyer, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                    var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.Rescheduled && t.CreatedBy == viewModel.Supplier.Id);

                    var deliveryWindow = notificationDomain.GetRescheduledScheduleDetails(newSchedules);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    viewModel.SupplierCompanyName,
                                                    viewModel.PoNumber,
                                                    deliveryWindow.ToString());

                    notification.Subject = string.Format(notification.Subject, $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}", viewModel.SupplierCompanyName, viewModel.PoNumber);

                    SendNotification(viewModel.Buyer.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryRequestRescheduledProcessor", "ProcessDeliveryRequestCreatedBySupplier", "Exception Details : ", ex);
            }
        }

        private void SendDeliveryRequestRescheduledEmailToBuyerCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, string deliveryWindow)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestRescheduled_BuyerCompanyAdmin, _serverUrl, callbackUrl);

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

        private void SendDeliveryRequestRescheduledEmailToSupplierCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, string deliveryWindow)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestRescheduled_SupplierCompanyAdmin, _serverUrl, callbackUrl);

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
