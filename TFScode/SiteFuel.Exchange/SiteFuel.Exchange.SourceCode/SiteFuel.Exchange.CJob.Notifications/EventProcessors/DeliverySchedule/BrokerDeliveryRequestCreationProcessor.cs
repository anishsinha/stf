using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
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
    public class BrokerDeliveryRequestCreationProcessor : BaseDeliveryScheduleEventProcessor, IEmailProcessor
    {
        private NotificationDeliveryRequestViewModel viewModel;

        public EventType EventType => EventType.BrokerDeliveryRequestCreated;
        private string userName;
        private BrokerDeliveryRequestMessageViewModel message;
        public BrokerDeliveryRequestCreationProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            message = JsonConvert.DeserializeObject<BrokerDeliveryRequestMessageViewModel>(notificationEventViewModel.JsonMessage);
            if (string.IsNullOrEmpty(message.BlendedGroupId))
            {
                viewModel = Task.Run(() => notificationDomain.GetTankDeliveryRequestNotificationDetails(message.EntityId)).Result;
            }
            else
            {
                viewModel = Task.Run(() => notificationDomain.GetBlendedTankDeliveryRequestNotificationDetails(message.BlendedGroupId)).Result;
            }
            userName = ContextFactory.Current.GetDomain<HelperDomain>().GetUserNameById(notificationEventViewModel.TriggeredByUserId);
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
                if (viewModel.JobId > 0)
                {
                    var carrierEmails = notificationDomain.GetUserEmailsForBrokeredDR(message.AssignedToCompanyId);
                    foreach (var item in carrierEmails)
                    {
                        var notification = GetNotificationContent(notificationEventViewModel.EventType, message.CompanyType);
                        if (!viewModel.IsBlendedRequest)
                        {
                            notification.Subject = GetNotificationSubjectText(notification.Subject);
                            notification.BodyText = GetNotificationBodyText(notification.BodyText, item.Name);
                        }
                        else
                        {
                            notification.Subject = GetNotificationSubjectText(Resource.emailSubjectBlendedBrokeredDetails);
                            notification.BodyText = GetBlendedNotificationBodyText(Resource.emailTemplateBlendedBrokeredDetails, item.Name);
                        }
                        SendNotificationForDefaultEvent(item.Code, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("BrokerDeliveryRequestCreationProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            return notification;
        }

        private string GetNotificationSubjectText(string subjectText)
        {
            subjectText = string.Format(subjectText, viewModel.UniqueOrderNo, viewModel.JobName);
            return subjectText;
        }

        private string GetNotificationBodyText(string bodyText, string firstName)
        {
            string quantity = string.Empty;
            if (viewModel.QuantityId == 0 || viewModel.QuantityId == (int)ScheduleQuantityType.Quantity)
                quantity = viewModel.Quantity + " " + viewModel.UoM.ToString();
            else
                quantity = EnumHelperMethods.GetDisplayName((ScheduleQuantityType)viewModel.QuantityId);

            bodyText = string.Format(bodyText,
                                    firstName,
                                    userName,
                                    viewModel.UniqueOrderNo,
                                    viewModel.BuyerCompanyName,
                                    viewModel.JobName,
                                    viewModel.TankName,
                                    viewModel.ProductType,
                                    viewModel.FuelType,
                                    quantity,
                                    viewModel.URLDetails
                                    );
            return bodyText;
        }
        private string GetBlendedNotificationBodyText(string bodyText, string firstName)
        {
            string productInfo = string.Empty;
            StringBuilder sb = new StringBuilder();
            if (viewModel.BlendedProductDetails.Any())
            {
                sb.Append("<table width='100%'>");
                sb.Append("<thead><tr><th width='35%' style='text-align: left;'>Product Type</th><th width='35%' style='text-align: left;'>Fuel Type</th><th width='30%' style='text-align: left;'>Quantity</th></tr></thead>");
                sb.Append("<tbody>");
                foreach (var item in viewModel.BlendedProductDetails)
                {

                    sb.Append("<tr style='font-size: 14px;color: #4a4a4a;font-family: open sans,sans-serif'>");
                    sb.Append("<td>" + item.ProductType + "</td>");
                    sb.Append("<td>" + item.FuelType + "</td>");
                    sb.Append("<td>" + item.Quantity + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody>");
                sb.Append("</table>");
                productInfo = sb.ToString();
            }
            bodyText = string.Format(bodyText,
                                    firstName,
                                    userName,
                                    viewModel.BuyerCompanyName,
                                    viewModel.JobName,
                                    viewModel.TankName,
                                    viewModel.UniqueOrderNo,
                                    productInfo,
                                    viewModel.URLDetails
                                    );
            return bodyText;
        }
    }
}
