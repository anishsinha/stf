using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class WebNotificationDomain : BaseDomain
    {
        public WebNotificationDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }
        public WebNotificationDomain(SiteFuelUow SiteFuelDbContext) : base(SiteFuelDbContext)
        {
        }

        //public async Task<WebNotificationViewModel> GetWebNotifications(int userId, int currentWebNotificationId)
        //{
        //    WebNotificationViewModel newWebNotification = new WebNotificationViewModel();
        //    if (userId > 0)
        //    {
        //        var notifications = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetWebNotificationsForUser(userId);
        //        if (notifications.Any())
        //        {
        //            var firstRecord = notifications.First();
        //            if (firstRecord != null)
        //            {
        //                newWebNotification.LatestId = notifications.Max(t => t.Id);
        //                newWebNotification.InvoiceNotificationCount = firstRecord.NewInvoiceNotifications;
        //                newWebNotification.OfferNotificationCount = firstRecord.NewOfferNotifications;
        //                newWebNotification.DispatchNotificationCount = firstRecord.NewDispatchNotifications;
        //                SetInvoiceNotifications(notifications, newWebNotification);
        //                SetOfferNotifications(notifications, newWebNotification);
        //                SetDispatchNotifications(notifications, newWebNotification);
        //            }
        //        }
        //    }
        //    return newWebNotification;
        //}

        //private void SetInvoiceNotifications(List<UspWebNotificationViewModel> uspWebNotifications, WebNotificationViewModel newWebNotification)
        //{
        //    var notifications = uspWebNotifications.Where(t => t.NotificationTypeId == (int)WebNotificationType.InvoiceNotification).ToList();
        //    foreach (var item in notifications)
        //    {
        //        var invoiceNotification = new InvoiceWebNotificaiton();
        //        invoiceNotification.Id = item.Id;
        //        invoiceNotification.JsonMessage = item.JsonMessage;
        //        invoiceNotification.NotificationDetails = JsonConvert.DeserializeObject<WebNotificationInvoiceJson>(item.JsonMessage);
        //        invoiceNotification.EntityId = item.EntityId;
        //        invoiceNotification.NotificationTypeId = item.NotificationTypeId;
        //        invoiceNotification.CreatedDate = item.CreatedDate.Date.ToString(Resource.lblDateFormat);
        //        invoiceNotification.IsNotificationRead = item.IsNotificationRead;
        //        newWebNotification.InvoiceWebNotifications.Add(invoiceNotification);
        //    }
        //}

        //private void SetOfferNotifications(List<UspWebNotificationViewModel> uspWebNotifications, WebNotificationViewModel newWebNotification)
        //{
        //    var notifications = uspWebNotifications.Where(t => t.NotificationTypeId == (int)WebNotificationType.OfferNotification).ToList();
        //    foreach (var item in notifications)
        //    {
        //        var offerNotification = new OfferWebNotification();
        //        offerNotification.Id = item.Id;
        //        offerNotification.JsonMessage = item.JsonMessage;
        //        offerNotification.NotificationDetails = JsonConvert.DeserializeObject<WebNotificationOfferJson>(item.JsonMessage);
        //        offerNotification.EntityId = item.EntityId;
        //        offerNotification.NotificationTypeId = item.NotificationTypeId;
        //        offerNotification.CreatedDate = item.CreatedDate.Date.ToString(Resource.lblDateFormat);
        //        offerNotification.IsNotificationRead = item.IsNotificationRead;
        //        newWebNotification.OfferWebNotifications.Add(offerNotification);
        //    }
        //}
        //private void SetDispatchNotifications(List<UspWebNotificationViewModel> uspWebNotifications, WebNotificationViewModel newWebNotification)
        //{
        //    var notifications = uspWebNotifications.Where(t => t.NotificationTypeId == (int)WebNotificationType.DispatchNotification).ToList();
        //    foreach (var item in notifications)
        //    {
        //        var dispatchNotification = new DispatchWebNotification();
        //        dispatchNotification.Id = item.Id;
        //        dispatchNotification.JsonMessage = item.JsonMessage;
        //        dispatchNotification.NotificationDetails = JsonConvert.DeserializeObject<WebNotificationOfferJson>(item.JsonMessage);
        //        dispatchNotification.EntityId = item.EntityId;
        //        dispatchNotification.NotificationTypeId = item.NotificationTypeId;
        //        dispatchNotification.CreatedDate = item.CreatedDate.Date.ToString(Resource.lblDateFormat);
        //        dispatchNotification.IsNotificationRead = item.IsNotificationRead;
        //        newWebNotification.DispatchWebNotifications.Add(dispatchNotification);
        //    }
        //}
        //public async Task<int> SetWebNotificationsAsRead(int userId, int notificationType)
        //{
        //    int response = 0;
        //    if (userId > 0)
        //    {
        //        using (var transaction = Context.DataContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                response = Context.DataContext.Database
        //                            .ExecuteSqlCommand("UPDATE WebNotifications SET IsNotificationRead = {0} WHERE IsNotificationRead = 0 AND CreatedFor = {1} AND NotificationTypeId = {2}"
        //                                            , 1, userId, notificationType);

        //                await Context.CommitAsync();
        //                transaction.Commit();
        //                return response;
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                LogManager.Logger.WriteException("SetWebNotificationsAsRead", "WebNotificationDomain", ex.Message, ex);
        //            }
        //        }

        //    }
        //    return response;
        //}
    }
}
