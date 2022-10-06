using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class InvitedUserAddedProcessor : BaseUserEventProcessor, IEmailProcessor
    {
        private InvitedUserNotificationViewModel viewModel;

        public EventType EventType => EventType.InvitedUserAdded;

        public InvitedUserAddedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();

            viewModel = notificationDomain.GetInvitedUserAdded(notificationEventViewModel.EntityId);
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
                if (viewModel.User.Id > 0)
                {
                    var callbackUrl = $"~/Account/RegisterExternalCompanyUser?id={viewModel.User.Id}";
                    var supplierURL = string.Empty;
                    if (!string.IsNullOrEmpty(notificationEventViewModel.JsonMessage))
                    {
                        var message = JsonConvert.DeserializeObject<AddUserMessageViewModel>(notificationEventViewModel.JsonMessage);
                        supplierURL = message.SupplierURL;
                        callbackUrl = $"~/Account/RegisterExternalCompanyUser?id={viewModel.User.Id}&supplierURL=" + supplierURL;
                    }
                    var notification = notificationDomain.GetNotificationContent(EventSubType.InvitedUserAdded_InvitedUser, _serverUrl, callbackUrl, (int)notificationEventViewModel.EventType, supplierURL);
                    notification.ApplicationTemplateId = notificationEventViewModel.ApplicationTemplateId;
                    notification.Subject = string.Format(notification.Subject, notificationEventViewModel.BrandedCompanyName);
                    notification.BodyText = string.Format(notification.BodyText,
                                                $"{viewModel.User.FirstName} {viewModel.User.LastName}",
                                                viewModel.InvitedByName,
                                                viewModel.InvitedByName, notificationEventViewModel.BrandedCompanyName);

                    SendNotificationForDefaultEvent(viewModel.User.Email, notification);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("InvitedUserAddedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }
    }
}
