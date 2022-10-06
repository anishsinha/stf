using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Domain;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.CJob.Notifications.EventProcessors
{
    public class DipTestNotUpdatedProcessor : BaseDispatchEventProcessor, IEmailProcessor
    {
        private NotificationDipTestViewModel viewModel;
        public EventType EventType => EventType.DipTestNotUpdated;
        public DipTestNotUpdatedProcessor()
        {
        }

        public void Initialize(NotificationEventViewModel notificationEventViewModel)
        {
            notificationDomain = ContextFactory.Current.GetDomain<NotificationDomain>();
            viewModel = notificationDomain.GetDipTestNotificationDetails(notificationEventViewModel.EntityId, notificationEventViewModel.JsonMessage);
        }

        public override void SendDefaultBuyerEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultBuyerEmailRecievers = defaultRecievers;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DipTestNotUpdatedProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendDefaultSupplierEmailForEvent(NotificationEventViewModel notificationEventViewModel, List<NotificationUserViewModel> defaultRecievers)
        {
            try
            {
                viewModel.DefaultSupplierEmailRecievers = defaultRecievers;
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DipTestNotUpdatedProcessor", "SendDefaultSupplierEmailForEvent", ex.Message, ex);
            }
        }

        public override void SendEmail(NotificationDomain notificationDomain, NotificationEventViewModel notificationEventViewModel)
        {
            try
            {
                if (viewModel.DipTest != null && viewModel.DipTest.Any())
                {
                    var notification = GetNotificationContent(notificationEventViewModel.EventType, viewModel.CompanyType);
                    var bodyText = notification.BodyText;
                    foreach (var user in viewModel.CompanyUsers)
                    {
                        notification.BodyText = string.Format(bodyText, $"{user.FirstName} {user.LastName}");
                        SendNotification(user.Email, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("DipTestNotUpdatedProcessor", "SendEmail", "Exception Details : ", ex);
            }
        }

        private NotificationViewModel GetNotificationContent(EventType eventType, CompanyType companyType)
        {
            var callbackUrl = string.Empty;
            var notification = notificationDomain.GetEmailNotificationContent(eventType, companyType, _serverUrl, callbackUrl);
            notification.BodyText += GetDiptestTableData();
            return notification; 
        }

        private string GetDiptestTableData()
        {
            var response = $"</br></br><table border='1' cellpadding='5' cellspacing='0' style='width:100%'><tr><th>{Resource.lblCustomerName}</th><th>{Resource.lblLocation}</th><th>{Resource.gridColumnJobID}</th><th>{Resource.lblTankName}</th><th>{Resource.lblInventoryDataCaptureMethod}</th><th>{Resource.gridColumnLastInventoryReading}</th><th>{Resource.gridColumnLastUpdated}</th></tr>";
            foreach (var dipTest in viewModel.DipTest)
            {
                response += $"<tr><td>{dipTest.Customer}</td><td>{dipTest.Location}</td><td>{dipTest.SiteId}</td><td>{dipTest.TankName}</td><td>{dipTest.DisplayDiptestMethod}</td><td>{dipTest.LastInventory}</td><td>{dipTest.LastUpdatedOn}</td></tr>";
            }
            response += "</table>";
            return response;
        }
    }
}