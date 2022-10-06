using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
	public class FuelRequestAcceptedProcessor : BaseFuelRequestEventProcessor, IEmailProcessor
    {
		private NotificationFuelRequestStatusViewModel viewModel;

        public EventType EventType => EventType.FuelRequestAccepted;

        private List<Attachment> attachment = null;

        public FuelRequestAcceptedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
		{
			notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

			viewModel = notificationDomain.GetFuelRequestStatusNotificationDetails(notificationEventViewModel.EntityId);

            var pdfContent = GetOrderPdfFileContent(viewModel.OrderId, viewModel.SendOrderAttachmentToBuyer || viewModel.SendOrderAttachmentToSupplier);
            if (pdfContent != null)
            {
                attachment = GetAttachments(pdfContent, viewModel.OrderNumber);
            }
        }

		public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
            string callbackUrl = string.Empty;
            if (viewModel.TypeId == (int)FuelRequestType.FuelRequest)
            {
                callbackUrl = $"~/Buyer/Order/Details/{viewModel.OrderId}";
            }
            else
            {
                callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
            }
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            //Send an email to company admins
            SendFuelRequestAcceptedEmailToCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType, EventSubType.FuelRequestAccepted_OwnerCompanyAdmin);
        }

		public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
		{
			viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

            var callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
            //send email to FR accepted supplier
            var notification = notificationDomain.GetNotificationContent(EventSubType.FuelRequestAccepted_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
            //to avoid duplicate emails in case Admin is creator of job
            notification.BodyText = string.Format(notification.BodyText,
                                        $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                        viewModel.FuelRequestNumber,
                                        viewModel.OrderNumber);
            if (attachment != null && viewModel.SendOrderAttachmentToSupplier)
                notification.Attachments = attachment;

            SendNotification(viewModel.Supplier.Email, notification);

            SendFuelRequestAcceptedEmailToSupplierCompanyAdmins(viewModel, callbackUrl, (int)notificationEventViewModel.EventType, EventSubType.FuelRequestAccepted_SupplierCompanyAdmin);
        }

		public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
		{
            try
            {
                if (viewModel.Id > 0)
                {
                    //Send an email to FR creator
                    string callbackUrl = string.Empty;
                    if (viewModel.TypeId == (int)FuelRequestType.FuelRequest)
                    {
                        callbackUrl = $"~/Buyer/Order/Details/{viewModel.OrderId}";
                    }
                    else
                    {
                        callbackUrl = $"~/Supplier/Order/Details/{viewModel.OrderId}";
                    }
                    var notification = notificationDomain.GetNotificationContent(EventSubType.FuelRequestAccepted_Owner, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);

                    if (attachment != null && viewModel.SendOrderAttachmentToBuyer)
                        notification.Attachments = attachment;

                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.Creator.FirstName} {viewModel.Creator.LastName}",
                                                    viewModel.FuelRequestNumber,
                                                    $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                    viewModel.SupplierCompanyName,
                                                    viewModel.OrderNumber);

                    SendNotification(viewModel.Creator.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("FuelRequestAcceptedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendFuelRequestAcceptedEmailToCompanyAdmins(NotificationFuelRequestStatusViewModel viewModel, string callbackUrl, int eventTypeId, EventSubType eventSubType)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.CompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(eventSubType, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator
                    if (item.Id != viewModel.Creator.Id)
                    {
                        if (attachment != null && viewModel.SendOrderAttachmentToBuyer)
                            notification.Attachments = attachment;

                        notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.FuelRequestNumber,
                                                                $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                                viewModel.SupplierCompanyName,
                                                                viewModel.OrderNumber);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        private void SendFuelRequestAcceptedEmailToSupplierCompanyAdmins(NotificationFuelRequestStatusViewModel viewModel, string callbackUrl, int eventTypeId, EventSubType eventSubType)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(eventSubType, _serverUrl, callbackUrl, eventTypeId);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.Supplier.Id)
                    {
                        if (attachment != null && viewModel.SendOrderAttachmentToSupplier)
                            notification.Attachments = attachment;

                        notification.BodyText = string.Format(bodyText,
                                                                $"{item.FirstName} {item.LastName}",
                                                                viewModel.FuelRequestNumber,
                                                                $"{viewModel.Supplier.FirstName} {viewModel.Supplier.LastName}",
                                                                viewModel.OrderNumber);

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }
    }
}
