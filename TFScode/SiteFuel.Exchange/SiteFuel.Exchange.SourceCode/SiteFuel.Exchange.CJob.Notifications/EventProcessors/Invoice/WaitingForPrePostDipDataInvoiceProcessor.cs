using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class WaitingForPrePostDipDataInvoiceProcessor : BaseOrderEventProcessor, IEmailProcessor
    {
        private NotificationInvoiceViewModel viewModel;
        private string userName;
        public EventType EventType => EventType.DdtCreatedAsInvoiceIsWaitingForPrePostDipData;

        public WaitingForPrePostDipDataInvoiceProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceNotificationDetails(notificationEventViewModel.EntityId);

            userName = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(notificationEventViewModel.TriggeredByUserId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
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
                    var subjectText = GetNotificationSubjectText(notification.Subject);
                    foreach (var item in companyAdminList)
                    {
                        if (item.Id != viewModel.SupplierUser.Id)
                        {
                            notification.Subject = subjectText;
                            notification.BodyText = GetNotificationBodyText(bodyText, item.FirstName);
                            SendNotificationForDefaultEvent(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("WaitingForPrePostDipDataInvoiceProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier);
                    notification.Subject = GetNotificationSubjectText(notification.Subject);
                    notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.SupplierUser.FirstName);
                    SendNotification(viewModel.SupplierUser.Email, notification);

                    if (!viewModel.DropAdditionalDetails.Any(t => t.IsTpoOrder))
                    {
                        notification = GetNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer);
                        notification.Subject = GetNotificationSubjectText(notification.Subject);
                        notification.BodyText = GetNotificationBodyText(notification.BodyText, viewModel.BuyerUser.FirstName);
                        SendNotification(viewModel.BuyerUser.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("WaitingForPrePostDipDataInvoiceProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            int orderId = viewModel.DropAdditionalDetails.Select(t => t.OrderId).FirstOrDefault();
            if (companyType == CompanyType.Buyer)
                callbackUrl = $"~/Buyer/Order/Details/{orderId}";
            else if (companyType == CompanyType.Supplier)
                callbackUrl = $"~/Supplier/Order/Details/{orderId}";

            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            var pdfContent = GetPdfFileContent(viewModel.Id, (int)companyType, (companyType == CompanyType.Supplier && viewModel.SendAttachmentToSupplier) || (companyType == CompanyType.Buyer && viewModel.SendAttachmentToBuyer));
            if (pdfContent != null)
            {
                notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
            }

            return notification;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            if(!string.IsNullOrEmpty(bodyText))
            {

                bodyText = string.Format(bodyText,
                                        firstName,
                                        viewModel.DropAdditionalDetails.Select(t => t.PoNumber).FirstOrDefault());
            }
            return bodyText;
        }

        private string GetNotificationSubjectText(string subjectText)
        {
            return subjectText;
        }
    }
}