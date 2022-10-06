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
    public class InvoiceApprovedAprovalWorkflowDisabledProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        private new NotificationJobViewModel viewModel;

        public EventType EventType => EventType.InvoiceAndDropTicketApprovalWorkFlowDisabled;

        public InvoiceApprovedAprovalWorkflowDisabledProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetJobNotificationDetails(notificationEventViewModel.EntityId);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            var callbackUrl = $"~/Account/Login";
            //Send an email to all admins of the company
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.CompanyId, notificationEventViewModel.EventType);
            if (companyAdminList.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceAndDropTicketApprovalWorkFlowDisabled_Admins, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    //to avoid duplicate emails in case Admin is creator of job
                    if (item.Id != viewModel.ApproverUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                $"{item.FirstName} {item.LastName}",
                                                  viewModel.Name,
                                                $"{viewModel.ApproverUser.FirstName} {viewModel.ApproverUser.LastName}");

                        SendNotification(item.Email, notification, true);
                    }
                }
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
            var callbackUrl = $"~/Account/Login";
            //send an email to all contact persons
            if (viewModel.AssignedTo.Count > 0)
            {
                var notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceAndDropTicketApprovalWorkFlowDisabled_Admins, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                var bodyText = notification.BodyText;
                foreach (var item in viewModel.AssignedTo)
                {
                    if (item.Id != viewModel.ApproverUser.Id)
                    {
                        notification.BodyText = string.Format(bodyText,
                                                    $"{item.FirstName} {item.LastName}",
                                                    viewModel.Name,
                                                    $"{viewModel.ApproverUser.FirstName} {viewModel.ApproverUser.LastName}");

                        SendNotification(item.Email, notification);
                    }
                }
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    //Send an email to job creator
                    var callbackUrl = $"~/Account/Login";
                    var notification = notificationDomain.GetNotificationContent(EventSubType.InvoiceAndDropTicketApprovalWorkFlowDisabled_Approver, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);
                    notification.BodyText = string.Format(notification.BodyText,
                                                    $"{viewModel.ApproverUser.FirstName} {viewModel.ApproverUser.LastName}",
                                                    viewModel.Name);

                    SendNotification(viewModel.ApproverUser.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceApprovedAprovalWorkflowDisabledProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
