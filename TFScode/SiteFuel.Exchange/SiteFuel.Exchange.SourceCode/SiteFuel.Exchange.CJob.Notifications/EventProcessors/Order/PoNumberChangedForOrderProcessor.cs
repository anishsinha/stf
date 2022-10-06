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
    public class PoNumberChangedForOrderProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationOrderViewModel viewModel;
        private string userName;
        public EventType EventType => EventType.PoNumberChangedForSingleDeliveryOrder;

        public PoNumberChangedForOrderProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetOrderNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.EventType);

            userName = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(notificationEventViewModel.TriggeredByUserId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;

            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;

                //Send an email to all admins of the company
                //List<NotificationUserViewModel> companyAdminList;
                //if (viewModel.IsTpoOrder)
                //    companyAdminList = notificationDomain.GetTpoCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
                //else
                //    companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
                if (!viewModel.IsTpoOrder)
                {
                    var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, notificationEventViewModel.EventType);
                    if (companyAdminList.Count > 0)
                    {
                        var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, notificationEventViewModel.JsonMessage);
                        var bodyText = notification.BodyText;
                        var subjectText = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                        foreach (var item in companyAdminList)
                        {
                            if (item.Id != viewModel.BuyerUser.Id)
                            {
                                notification.Subject = subjectText;
                                notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                                SendNotificationForDefaultEvent(item.Email, notification);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PoNumberChangedForOrderProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
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
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, notificationEventViewModel.JsonMessage);
                    var bodyText = notification.BodyText;
                    var subjectText = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            notification.Subject = subjectText;
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName, notificationEventViewModel.JsonMessage);
                            SendNotificationForDefaultEvent(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PoNumberChangedForOrderProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, notificationEventViewModel.JsonMessage);
                    notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName, notificationEventViewModel.JsonMessage);
                    SendNotification(viewModel.SupplierUser.Email, notification);

                    if (!viewModel.IsTpoOrder)
                    {
                        notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, notificationEventViewModel.JsonMessage);
                        notification.Subject = GetNotificationSubjectText(notification.Subject, notificationEventViewModel.JsonMessage);
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.BuyerUser.FirstName, notificationEventViewModel.JsonMessage);
                        SendNotification(viewModel.BuyerUser.Email, notification);
                        //if (viewModel.IsTpoOrder)
                        //    SendNotificationForDefaultEvent(viewModel.BuyerUser.Email, notification);
                        //else
                        //    SendNotification(viewModel.BuyerUser.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("PoNumberChangedForOrderProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType, string jsonMessage)
        {
            var callbackUrl = string.Empty;

            if (companyType == CompanyType.Buyer)
                callbackUrl = $"~/Buyer/Order/Details/{viewModel.Id}";
            else if (companyType == CompanyType.Supplier)
                callbackUrl = $"~/Supplier/Order/Details/{viewModel.Id}";

            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            var message = JsonConvert.DeserializeObject<OrderMessageViewModel>(jsonMessage);
            if (message.InvoiceId > 0)
            {
                var pdfContent = GetPdfFileContent(message.InvoiceId, (int)companyType, (companyType == CompanyType.Supplier && viewModel.SendInvoiceAttachmentToSupplier) || (companyType == CompanyType.Buyer && viewModel.SendInvoiceAttachmentToBuyer));
                if (pdfContent != null)
                {
                    notification.Attachments = GetAttachments(pdfContent, message.InvoiceNumber);
                }
            }

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

            if (message.InvoiceId > 0)
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 1);
            else
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 1);

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