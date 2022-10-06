using Newtonsoft.Json;
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
    public class PoNumberChangedProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;
        private string userName;
        public EventType EventType => EventType.PoNumberChanged;
        private List<Attachment> attachment = null;
        public PoNumberChangedProcessor()
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

            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
                if (companyAdminList.Count > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                    var bodyText = notification.BodyText;
                    var subjectText = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.BuyerUser.Id)
                        {
                            if (attachment != null && viewModel.SendOrderAttachmentToBuyer)
                                notification.Attachments = attachment;
                            notification.Subject = subjectText;
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                            SendNotificationForDefaultEvent(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PoNumberChangedProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                    var subjectText = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            if (attachment != null && viewModel.SendOrderAttachmentToSupplier)
                                notification.Attachments = attachment;
                            notification.Subject = subjectText;
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                            SendNotificationForDefaultEvent(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PoNumberChangedProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName, notificationEventViewModel.JsonMessage);
                    if (attachment != null && viewModel.SendOrderAttachmentToSupplier)
                        notification.Attachments = attachment;
                    SendNotification(viewModel.SupplierUser.Email, notification);

                    notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                    notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.BuyerUser.FirstName, notificationEventViewModel.JsonMessage);
                    notification.Attachments = new List<Attachment>();
                    if (attachment != null && viewModel.SendOrderAttachmentToBuyer)
                        notification.Attachments = attachment;
                    SendNotification(viewModel.BuyerUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PoNumberChangedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;

            if (companyType == CompanyType.Buyer)
                callbackUrl = $"~/Buyer/Order/Details/{viewModel.Id}";
            else if (companyType == CompanyType.Supplier)
                callbackUrl = $"~/Supplier/Order/Details/{viewModel.Id}";

            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName, string jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<OrderMessageViewModel>(jsonMessage);

            bodyText = string.Format(bodyText,
                                    firstName,
                                    userName,
                                    message.PreviousPoNumber,
                                    viewModel.PoNumber);
            return bodyText;
        }

        private string GetNotificationSubjectText(string subjectText, string jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<OrderMessageViewModel>(jsonMessage);

            subjectText = string.Format(subjectText,
                                    message.PreviousPoNumber,
                                    viewModel.PoNumber);
            return subjectText;
        }
    }
}