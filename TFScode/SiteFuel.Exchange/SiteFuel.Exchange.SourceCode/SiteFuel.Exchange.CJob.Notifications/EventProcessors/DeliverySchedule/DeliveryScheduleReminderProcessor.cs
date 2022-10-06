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
	public class DeliveryScheduleReminderProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationDeliveryRequestViewModel viewModel;

        public EventType EventType => EventType.DeliveryRequestReminder;

        public DeliveryScheduleReminderProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                var orders = notificationDomain.GetOrdersForDeliverySchedule(notificationEventViewModel.EntityId);
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestReminder_Buyer, _serverUrl, string.Empty, (int)notificationEventViewModel.EventType);
                foreach (var order in orders)
                {
                    viewModel = notificationDomain.GetDeliveryReminderNotificationDetails(notificationEventViewModel.EntityId, order);
                    if (viewModel.Id > 0)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                                        $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                        viewModel.SupplierCompanyName,
                                                        viewModel.WeekDay,
                                                        viewModel.Date,
                                                        viewModel.Time,
                                                        viewModel.PoNumber,
                                                        $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                        viewModel.SupplierContactNumber,
                                                        viewModel.DriverName);

                        if (SendNotification(viewModel.Buyer.Email, notification))
                        {
                            //Send an email to company admins
                            SendDeliveryRequestReminderEmailToBuyerCompanyAdmins(viewModel, string.Empty, (int)notificationEventViewModel.EventType);

                            //Send an email to supplier
                            SendDeliveryRequestReminderEmailToSupplier(viewModel, string.Empty, (int)notificationEventViewModel.EventType);

                            //Send an email to supplier company admins
                            SendDeliveryRequestReminderEmailToSupplierCompanyAdmins(viewModel, string.Empty, (int)notificationEventViewModel.EventType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleReminderProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendDeliveryRequestReminderEmailToBuyerCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestReminder_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Buyer.Id)
                    {
                        notification.BodyText = string.Format(notification.BodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             viewModel.SupplierCompanyName,
                                                             viewModel.WeekDay,
                                                             viewModel.Date,
                                                             viewModel.Time,
                                                             viewModel.PoNumber,
                                                             $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                             viewModel.SupplierContactNumber);
                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendDeliveryRequestReminderEmailToSupplier(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestReminder_Supplier, _serverUrl, callbackUrl, eventTypeId);

            if (viewModel.Id > 0)
            {
                var quantityDelivered = ($"{viewModel.Quantity.GetPreciseValue().GetCommaSeperatedValue()} {viewModel.UoM}").ToString();
                notification.BodyText = string.Format(notification.BodyText,
                                                             $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                             viewModel.BuyerCompanyName,
                                                             viewModel.PoNumber,
                                                             quantityDelivered,
                                                             viewModel.Date,
                                                             viewModel.Time);
                SendNotification(viewModel.Supplier.Email, notification);
            }
        }

        private void SendDeliveryRequestReminderEmailToSupplierCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, int eventTypeId)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestReminder_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);

                var quantityDelivered = ($"{viewModel.Quantity.GetPreciseValue().GetCommaSeperatedValue()} {viewModel.UoM}").ToLower();
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Supplier.Id)
                    {
                        //<p>Hello {0},<br/><br/>This is reminder that your colleague has a delivery request for {1} for PO# {2} for {3} gallons on {4} at {5}.</p>
                        notification.BodyText = string.Format(notification.BodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             viewModel.BuyerCompanyName,
                                                             viewModel.PoNumber,
                                                             quantityDelivered,
                                                             viewModel.Date,
                                                             viewModel.Time);
                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
