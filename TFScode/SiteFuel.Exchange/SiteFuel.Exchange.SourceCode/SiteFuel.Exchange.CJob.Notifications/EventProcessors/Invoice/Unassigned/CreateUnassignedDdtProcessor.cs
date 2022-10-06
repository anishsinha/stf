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
    public class CreateUnassignedDdtProcessor : BaseInvoiceEventProcessor, IEmailProcessor
    {
        public EventType EventType => EventType.UnassignedDdtCreate;

        public CreateUnassignedDdtProcessor()
        {
        }

        public override void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvoiceDetailsForUnassignedDdtNotification(notificationEventViewModel.EntityId);
            _doNotSendInvoiceAttachment = viewModel.IsPartOfStatement || viewModel.IsProFormaPo || (!viewModel.SendAttachmentToBuyer && !viewModel.SendAttachmentToSupplier);
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
                if (viewModel.Id > 0 && viewModel.InvoiceType == (int)InvoiceType.DigitalDropTicketMobileApp)
                {
                    var callbackUrl = $"~/Supplier/Invoice/Details?id={viewModel.Id}";
                    NotificationViewModel notification;
                    notification = notificationDomain.GetNotificationContent(EventSubType.CreateUnassignedDdt_Supplier, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType);

                    notification.Subject = string.Format(notification.Subject, viewModel.InvoiceNumber);

                    //// as discussed with QA team, unassigned DDT email should go to company admins
                    var companyAdminList = notificationDomain.GetEmailSubscribedCompanyAdmins(viewModel.SupplierCompanyId, EventType);
                    if (companyAdminList.Count > 0)
                    {
                        var bodyText = notification.BodyText;
                        var quantityDelivered = ($"{viewModel.DropAdditionalDetails.Sum(t => t.DropQuantity).GetPreciseValue().GetCommaSeperatedValue()} {viewModel.DropAdditionalDetails.Select(t => t.UoM).FirstOrDefault()}").ToLower();
                        foreach (var item in companyAdminList)
                        {
                            notification.BodyText = string.Format(bodyText,
                                                    $"{item.FirstName} {item.LastName}",
                                                    viewModel.DriverName,
                                                    quantityDelivered,
                                                    viewModel.DropDate,
                                                    viewModel.DropStartTime,
                                                    viewModel.DropEndTime,
                                                    viewModel.InvoiceNumber);

                            SendNotification(item.Email, notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CreateUnassignedDdtProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
