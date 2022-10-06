using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using Newtonsoft.Json;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.Domain
{
    public class WebNotificationInvoiceDomain : BaseDomain
    {
        public WebNotificationInvoiceDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public WebNotificationInvoiceDomain(BaseDomain domain) : base(domain)
        {
        }

        //internal void ProcessInvoiceJsonMessage(NotificationInvoiceQueMsg invoiceQueMsg, List<string> errorInfo)
        //{
        //    using (var tracer = new Tracer("WebNotificationInvoiceDomain", "ProcessInvoiceJsonMessage"))
        //    {
        //        StringBuilder processMessage = new StringBuilder();

        //        try
        //        {
        //            var invoiceObj = Context.DataContext.Invoices.Where(t => t.Id == invoiceQueMsg.InvoiceId)
        //                                .Select(t => new { t.Id,
        //                                                    t.CreatedBy,
        //                                                    t.CreatedDate,
        //                                                    t.Order.BuyerCompanyId,
        //                                                    t.Order.BuyerCompany.CompanyTypeId,
        //                                                    t.Order.BuyerCompany.Users })
        //                                .SingleOrDefault();

        //            if (invoiceObj != null)
        //            {
        //                List<WebNotification> invoiceWebNotifications = new List<WebNotification>();
        //                string invoiceJson = GetInvoiceWebNotificationJson(invoiceObj.Id, invoiceQueMsg);
        //                var buyerCompanyUsers = invoiceObj.Users
        //                                .Where(t => t.IsActive && 
        //                                (t.MstRoles.Any(r => r.Id == (int)UserRoles.Admin 
        //                                                || r.Id == (int)UserRoles.Buyer 
        //                                                || r.Id == (int)UserRoles.BuyerAdmin)))
        //                                 .Select(t => t.Id).Distinct().ToList();
                       
        //                foreach (var userId in buyerCompanyUsers)
        //                {
        //                    invoiceWebNotifications.Add(new WebNotification()
        //                    {
        //                        CreatedBy = invoiceObj.CreatedBy,
        //                        CreatedDate = invoiceObj.CreatedDate,
        //                        CreatedFor = userId,
        //                        CreatedForCompanyId = invoiceObj.BuyerCompanyId,
        //                        CreatedForCompanyTypeId = invoiceObj.CompanyTypeId,
        //                        EntityId = invoiceQueMsg.InvoiceId,
        //                        IsNotificationRead = false,
        //                        NotificationTypeId = (int)WebNotificationType.InvoiceNotification,
        //                        JsonMessage = invoiceJson
        //                    });
        //                }

        //                if(invoiceWebNotifications.Any())
        //                {
        //                    Context.DataContext.WebNotifications.AddRange(invoiceWebNotifications);
        //                    Context.Commit();
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (!(ex is QueueMessageFatalException))
        //                LogManager.Logger.WriteException("WebNotificationInvoiceDomain", "ProcessInvoiceJsonMessage", ex.Message, ex);
        //            if (processMessage.Length == 0)
        //            {
        //                processMessage.Append(Constants.RequestError);
        //                errorInfo.Add(processMessage.ToString());
        //            }
        //            throw new QueueMessageFatalException(errorInfo[0], errorInfo);
        //        }
        //    }
        //}

        //private string GetInvoiceWebNotificationJson(int invoiceId, NotificationInvoiceQueMsg invoiceQueMsg)
        //{
        //    var jsonViewModel = new WebNotificationInvoiceJson();
        //    jsonViewModel.CreatedByCompanyName = invoiceQueMsg.CreatedByCompanyName;
        //    jsonViewModel.OrderNumber = invoiceQueMsg.OrderNumber;
        //    jsonViewModel.NotificaitonText = Resource.InvoiceWebNotificationText;
        //    jsonViewModel.InvoiceId = invoiceId;
        //    jsonViewModel.InvoiceNumber = invoiceQueMsg.InvoiceNumber;
        //    string json = JsonConvert.SerializeObject(jsonViewModel);
        //    return json;
        //}
    }
}