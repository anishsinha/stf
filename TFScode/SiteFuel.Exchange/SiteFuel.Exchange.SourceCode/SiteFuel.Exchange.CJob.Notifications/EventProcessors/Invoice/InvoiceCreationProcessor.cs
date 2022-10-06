using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class InvoiceCreationProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.InvoiceCreated;

        public InvoiceCreationProcessor()
        {
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            Thread.Sleep(300);
            try
            {
                if (viewModel.DeliveryInstructionsExists && (viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.None || viewModel.InvoiceNotificationPreferenceId == (int)InvoiceNotificationPreferenceTypes.SendOnlyBillingFile))
                {
                    return;
                }
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
                GetInvoiceTypeId(viewModel);
                var companyType = viewModel.IsBrokeredInvoice ? CompanyType.Supplier : CompanyType.Buyer;
                var callbackUrl = $"~/{companyType.ToString()}/Invoice/Details?id={viewModel.Id}";
                NotificationViewModel notification = GetBuyerNotificationContent(notificationEventViewModel.EventType, CompanyType.Buyer, callbackUrl);

                SendInvoiceCreatedEmailToBuyerUsers(notification);

                //Send an email to Buyer Company admins
                SendInvoiceCreatedEmailToBuyerCompanyAdmins(notification);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreationProcessor", "SendDefaultBuyerEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.Id > 0)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification = GetSupplierNotificationContent(notificationEventViewModel.EventType, CompanyType.Supplier, callbackUrl);
                    var bodyText = notification.BodyText;
                    notification.BodyText = GetSupplierNotificationBody(notification.BodyText, viewModel.SupplierUser.FirstName);
                    if (SendNotification(viewModel.SupplierUser.Email, notification))
                    {
                        notification.BodyText = bodyText;
                        SendInvoiceCreatedEmailToSupplierUsers(notification);
                        //send email to company admins
                        SendInvoiceCreatedEmailToCompanyAdmins(notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvoiceCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private void SendInvoiceCreatedEmailToSupplierUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.SupplierAccountingUsers)
            {
                notification.BodyText = GetSupplierNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }


        private void SendInvoiceCreatedEmailToCompanyAdmins(NotificationViewModel notification)
        {
            var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    if (item.Id != viewModel.SupplierUser.Id)
                    {
                        notification.BodyText = GetSupplierNotificationBody(notification.BodyText, item.FirstName);
                        SendNotification(item.Email, notification);
                        notification.BodyText = bodyText;
                    }
                }
            }
        }

        private void SendInvoiceCreatedEmailToBuyerUsers(NotificationViewModel notification)
        {
            var bodyText = notification.BodyText;
            foreach (var item in viewModel.UsersAssignedToJob.Where(t => t.Id != viewModel.BuyerUser.Id))
            {
                notification.BodyText = GetBuyerNotificationBody(notification.BodyText, item.FirstName);
                SendNotification(item.Email, notification);
                notification.BodyText = bodyText;
            }
        }

        private void SendInvoiceCreatedEmailToBuyerCompanyAdmins(NotificationViewModel notification)
        {
            List<NotificationUserViewModel> companyAdminList;
            if (viewModel.DeliveryInstructionsExists)
            {
                companyAdminList = notificationDomain.GetEmailSubscribedTpoBuyerAdmins(viewModel.BuyerCompanyId, EventType);
            }
            else
            {
                companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.BuyerCompanyId, EventType);
            }
            if (companyAdminList.Count > 0)
            {
                var bodyText = notification.BodyText;
                foreach (var item in companyAdminList)
                {
                    notification.BodyText = GetBuyerNotificationBody(notification.BodyText, item.FirstName);
                    SendNotification(item.Email, notification);
                    notification.BodyText = bodyText;
                }
            }
        }

        private string GetBuyerNotificationBody(string bodyText, string firstName)
        {
            StringBuilder poWithQuantiy = GetPoWithQuantity();
            var POWithExceedingQuantity = viewModel.DropAdditionalDetails.Where(t => t.IsExceedingQuantity == true).Select(t => t.PoNumber).ToList();
            bodyText = string.Format(bodyText,
                                    firstName, viewModel.SupplierCompanyName,
                                    viewModel.JobName,
                                    $"{viewModel.DayOfWeek} {viewModel.DropDate}",
                                    viewModel.DropStartTime, viewModel.DropEndTime,
                                    poWithQuantiy,
                                    $"{GetInvoiceType()} {viewModel.InvoiceNumber}",
                                    viewModel.DueDate.ToString(Resource.constFormatDate),
                                    POWithExceedingQuantity.Count() > 0 ? string.Join(", ", POWithExceedingQuantity) : "--");

            bodyText = IsInvoiceCreated() && !viewModel.IsPartOfStatement ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.DropAdditionalDetails.Any(t => t.IsExceedingQuantity) ? notificationDomain.ReplaceBodyContent(bodyText, 2) : notificationDomain.RemoveBodyContent(bodyText, 2);
            if (viewModel.IsProFormaPo && viewModel.IsInvoice)
            {
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 3);
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 4);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 3);
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 4);
            }
            bodyText = viewModel.ReplaceInvoiceWithDdt ? notificationDomain.RemoveBodyContent(bodyText, 5) : notificationDomain.ReplaceBodyContent(bodyText, 5);
            return bodyText;
        }

        private StringBuilder GetPoWithQuantity()
        {
            StringBuilder poWithQuantiy = new StringBuilder();
            if (viewModel.InvoiceType == (int)InvoiceType.TankRental || viewModel.InvoiceType == (int)InvoiceType.Balance)
            {
                foreach (var order in viewModel.DropAdditionalDetails)
                {
                    poWithQuantiy.Append($"for order \"{order.PoNumber}\"");
                }
            }
            else
            {
                foreach (var order in viewModel.DropAdditionalDetails)
                {
                    var quantity = (order.UoM == UoM.MetricTons) ? order.ConvertedQuantity : order.DropQuantity.ToString();
                    poWithQuantiy.Append($" with a quantity of {quantity} {order.UoM} for order \"{order.PoNumber}\"");
                }
            }

            return poWithQuantiy;
        }

        private NotificationViewModel GetSupplierNotificationContent(EventType eventType, CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);

            var pdfContent = GetPdfFileContent(viewModel.Id, (int)CompanyType.Supplier, viewModel.SendAttachmentToSupplier);
            if (pdfContent != null)
            {
                notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
            }
            string invoiceType = GetInvoiceType();
            notification.Subject = string.Format(notification.Subject, $"{invoiceType} {viewModel.InvoiceNumber}");
            notification.BodyButtonText = string.Format(notification.BodyButtonText, GetInvoiceType());
            return notification;
        }

        private NotificationViewModel GetBuyerNotificationContent(EventType eventType, CompanyType companyType, string callbackUrl)
        {
            NotificationViewModel notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            byte[] pdfContent = null;
            if (viewModel.ReplaceInvoiceWithDdt)//in case of tpo and invoice notification preference is ddt
            {
                notification = ReplaceInvoiceContentWithDdt(notification);
                pdfContent = GetDdtPdfFileFromInvoice(viewModel.Id, (int)companyType, viewModel.SendAttachmentToBuyer);
            }
            else
            {
                pdfContent = GetPdfFileContent(viewModel.Id, (int)companyType, viewModel.SendAttachmentToBuyer);
            }
            if (pdfContent != null)
            {
                notification.Attachments = GetAttachments(pdfContent, viewModel.InvoiceNumber);
            }
            string invoiceType = GetInvoiceType();
            notification.Subject = string.Format(notification.Subject, $"{invoiceType} {viewModel.InvoiceNumber}");
            notification.BodyButtonText = string.Format(notification.BodyButtonText, GetInvoiceType());
            return notification;
        }

        private string GetSupplierNotificationBody(string bodyText, string firstName)
        {
            StringBuilder poWithQuantiy = GetPoWithQuantity();
            var POWithExceedingQuantity = viewModel.DropAdditionalDetails.Where(t => t.IsExceedingQuantity == true).Select(t => t.PoNumber).ToList();
            bodyText = string.Format(bodyText,
                                    firstName,
                                    viewModel.JobName,
                                    $"{viewModel.DayOfWeek} {viewModel.DropDate}",
                                    viewModel.DropStartTime, viewModel.DropEndTime,
                                    poWithQuantiy,
                                    $"{GetInvoiceType()} {viewModel.InvoiceNumber}",
                                    viewModel.DueDate.ToString(Resource.constFormatDate),
                                    POWithExceedingQuantity.Count() > 0 ? string.Join(", ", POWithExceedingQuantity) : "--"
                                    );
            bodyText = IsInvoiceCreated() && !viewModel.IsPartOfStatement ? notificationDomain.ReplaceBodyContent(bodyText, 1) : notificationDomain.RemoveBodyContent(bodyText, 1);
            bodyText = viewModel.DropAdditionalDetails.Any(t => t.IsExceedingQuantity) ? notificationDomain.ReplaceBodyContent(bodyText, 2) : notificationDomain.RemoveBodyContent(bodyText, 2);
            if (viewModel.IsProFormaPo && viewModel.IsInvoice)
            {
                bodyText = notificationDomain.ReplaceBodyContent(bodyText, 3);
            }
            else
            {
                bodyText = notificationDomain.RemoveBodyContent(bodyText, 3);
            }
            return bodyText;
        }

        private bool IsInvoiceCreated()
        {
            return viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketManual && viewModel.InvoiceType != (int)InvoiceType.DigitalDropTicketMobileApp;
        }

        private string GetInvoiceType()
        {
            return IsInvoiceCreated() ? Resource.lblInvoice : Resource.lblDDT;
        }
    }
}
