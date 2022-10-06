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
	public class DeliveryScheduleCreationProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
		private NotificationDeliveryRequestViewModel viewModel;

        public EventType EventType => EventType.DeliveryRequestCreated;

        public DeliveryScheduleCreationProcessor()
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

            var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New && t.CreatedBy == viewModel.Supplier.Id);
            var deliveryWindow = notificationDomain.GetAddedScheduleDetails(newSchedules);
            var deliveryWindowForSms = notificationDomain.GetAddedScheduleDetailsForSms(newSchedules);

            //Send an email to buyer company admins
            if (deliveryWindowForSms != string.Empty)
                SendDeliveryRequestCreatedEmailToBuyerCompanyAdmins(viewModel, callbackUrl, deliveryWindow.ToString(), (int)notificationEventViewModel.EventType, deliveryWindowForSms);
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
            
            var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New && t.CreatedBy == viewModel.Buyer.Id);

            var deliveryWindow = notificationDomain.GetAddedScheduleDetails(newSchedules);
            var deliveryWindowForSms = notificationDomain.GetAddedScheduleDetailsForSms(newSchedules);

            //Send an email to supplier company admins
            if (deliveryWindowForSms != string.Empty)
                SendDeliveryRequestCreatedEmailToSupplierCompanyAdmins(viewModel, callbackUrl, deliveryWindow.ToString(), (int)notificationEventViewModel.EventType, deliveryWindowForSms);
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.UserRole == UserRoles.Buyer)
                {
                    ProcessDeliveryRequestCreatedByBuyer(notificationEventViewModel, viewModel);
                }
                else
                {
                    ProcessDeliveryRequestCreatedBySupplier(notificationEventViewModel, viewModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryRequestCreatedByBuyer(NotificationEventViewModel notificationEvent, NotificationDeliveryRequestViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";

                    //Get Email Details
                    var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestCreated_Supplier, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                    var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New && t.CreatedBy == viewModel.Buyer.Id);
                    var deliveryWindow = notificationDomain.GetAddedScheduleDetails(newSchedules);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                    viewModel.BuyerCompanyName,
                                                    viewModel.PoNumber,
                                                    deliveryWindow.ToString());

                    notification.Subject = string.Format(notification.Subject, $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}", viewModel.BuyerCompanyName, viewModel.PoNumber);

                    //Get SMS Details
                    var notificationForSms = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Buyer, _serverUrl, callbackUrl);
                    var deliveryWindowForSms = notificationDomain.GetAddedScheduleDetailsForSms(newSchedules);
                    notification.SmsText = GetSupplierNotificationSmsText(notificationForSms.SmsText, deliveryWindowForSms);

                    SendNotification(viewModel.Supplier.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleCreationProcessor", "ProcessDeliveryRequestCreatedByBuyer", "Exception Details : ", ex);
            }
        }

        public void ProcessDeliveryRequestCreatedBySupplier(NotificationEventViewModel notificationEvent, NotificationDeliveryRequestViewModel viewModel)
        {
            try
            {
                NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Buyer/Order/Details/{viewModel.OrderId}";
                    NotificationViewModel notification;

                    //Get Email Details
                    if (viewModel.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestCreatedForTPO_Buyer, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                        notification.BodyButtonUrl = string.Empty;
                    }
                    else
                    {
                        notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestCreated_Buyer, _serverUrl, callbackUrl, (int)notificationEvent.EventType);
                    }
                    var newSchedules = viewModel.CurrentSchedules.Where(t => t.StatusId == (int)DeliveryScheduleStatus.New && t.CreatedBy == viewModel.Supplier.Id);

                    var deliveryWindow = notificationDomain.GetAddedScheduleDetails(newSchedules);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    viewModel.SupplierCompanyName,
                                                    viewModel.PoNumber,
                                                    deliveryWindow.ToString());

                    notification.Subject = string.Format(notification.Subject, $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}", viewModel.SupplierCompanyName, viewModel.PoNumber);

                    //Get SMS Details
                    var notificationForSms = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Buyer, _serverUrl, callbackUrl);
                    var deliveryWindowForSms = notificationDomain.GetAddedScheduleDetailsForSms(newSchedules);
                    notification.SmsText = GetBuyerNotificationSmsText(notificationForSms.SmsText, deliveryWindowForSms);

                    SendNotification(viewModel.Buyer.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DeliveryScheduleCreationProcessor", "ProcessDeliveryRequestCreatedBySupplier", "Exception Details : ", ex);
            }
        }

        private string GetBuyerNotificationSmsText(string smsText,string deliveryWindow)
        {
            smsText = string.Format(smsText,
                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                    viewModel.SupplierCompanyName,
                                    deliveryWindow,
                                    viewModel.PoNumber
                                    );

            return smsText;
        }

        private string GetSupplierNotificationSmsText(string smsText, string deliveryWindow)
        {
            smsText = string.Format(smsText,
                                    $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                    viewModel.BuyerCompanyName,
                                    deliveryWindow,
                                    viewModel.PoNumber
                                    );

            return smsText;
        }

        private void SendDeliveryRequestCreatedEmailToBuyerCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, string deliveryWindow, int eventTypeId, string deliveryWindowForSms)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                NotificationViewModel notification;
                if (viewModel.FuelRequestTypeId == (int)FuelRequestType.ThirdPartyRequest)
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestCreatedForTPO_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                    notification.BodyButtonUrl = string.Empty;
                }
                else
                {
                    notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestCreated_BuyerCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);
                }

                //Get SMS Details
                var notificationForSms = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Buyer, _serverUrl, callbackUrl);
                
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Buyer.Id)
                    {
                        //Email Details
                        notification.BodyText = string.Format(notification.BodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}",
                                                             viewModel.PoNumber,
                                                             deliveryWindow);

                        notification.Subject = string.Format(notification.Subject, $"{viewModel.Buyer.FirstName} {viewModel.Buyer.LastName}", viewModel.PoNumber);

                        //SMS Details
                        notification.SmsText = GetBuyerNotificationSmsText(notificationForSms.SmsText, deliveryWindowForSms);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendDeliveryRequestCreatedEmailToSupplierCompanyAdmins(NotificationDeliveryRequestViewModel viewModel, string callbackUrl, string deliveryWindow, int eventTypeId,string deliveryWindowForSms)
        {
            NotificationDomain notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.DeliveryRequestCreated_SupplierCompanyAdmin, _serverUrl, callbackUrl, eventTypeId);

                //Get SMS Details
                var notificationForSms = notificationDomain.GetEmailNotificationContent(EventType, CompanyType.Supplier, _serverUrl, callbackUrl);

                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Supplier.Id)
                    {
                        //Email Details
                        notification.BodyText = string.Format(notification.BodyText,
                                                             $"{item.FirstName} {item.LastName}",
                                                             $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                             viewModel.PoNumber,
                                                             deliveryWindow);

                        notification.Subject = string.Format(notification.Subject, $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}", viewModel.PoNumber);

                        //SMS Details
                        notification.SmsText = GetSupplierNotificationSmsText(notificationForSms.SmsText, deliveryWindowForSms);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
