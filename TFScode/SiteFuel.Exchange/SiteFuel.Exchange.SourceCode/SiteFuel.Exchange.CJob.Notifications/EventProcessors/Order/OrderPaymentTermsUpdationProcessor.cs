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
    public class OrderPaymentTermsUpdationProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;
        private string userName;
        public EventType EventType => EventType.OrderPaymentTermsUpdated;
        private List<Attachment> attachment = null;
        public OrderPaymentTermsUpdationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetOrderNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);

            userName = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(notificationEventViewModel.TriggeredByUserId);

            var pdfContent = GetOrderPdfFileContent(viewModel.Id, viewModel.SendOrderAttachmentToBuyer || viewModel.SendOrderAttachmentToSupplier);
            if (pdfContent != null)
            {
                attachment = GetAttachments(pdfContent, viewModel.PoNumber);
            }
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            //Send an email to all admins of the company
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.BuyerUser.Id)
                    {
                        if (attachment != null && viewModel.SendOrderAttachmentToBuyer)
                            notification.Attachments = attachment;

                        notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                        SendNotification(item.Email, notification);
                    }
                }
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                //Send an email to all admins of the company
                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, notificationEventViewModel.EventType);
                if (companyAdminList.Count > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    var bodyText = notification.BodyText;
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            if (attachment != null && viewModel.SendOrderAttachmentToSupplier)
                                notification.Attachments = attachment;

                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                            SendNotification(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderPaymentTermsUpdationProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName);
                    if (attachment != null && viewModel.SendOrderAttachmentToSupplier)
                        notification.Attachments = attachment;
                    SendNotification(viewModel.SupplierUser.Email, notification);
                    notification.Attachments = new List<Attachment>();
                    notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.BuyerUser.FirstName);
                    if (attachment != null && viewModel.SendOrderAttachmentToBuyer)
                        notification.Attachments = attachment;
                    SendNotification(viewModel.BuyerUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("OrderPaymentTermsUpdationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private string GetCallBackURL(CompanyType companyType)
        {
            var callbackUrl = string.Empty;

            if (companyType == CompanyType.Buyer)
            {
                if (viewModel.IsBrokeredOrder)
                    callbackUrl = $"~/Supplier/Order/Details/{viewModel.Id}?isBrokeredRequest = true";
                else
                    callbackUrl = $"~/Buyer/Order/Details/{viewModel.Id}";
            }
            else if (companyType == CompanyType.Supplier)
            {
                callbackUrl = $"~/Supplier/Order/Details/{viewModel.Id}";
            }

            return callbackUrl;
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            string callbackUrl = GetCallBackURL(companyType);
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            bodyText = string.Format(bodyText,
                                    firstName,
                                    userName,
                                    viewModel.PoNumber,
                                    viewModel.NewOrderVersionNumber);
            return bodyText;
        }
    }
}

