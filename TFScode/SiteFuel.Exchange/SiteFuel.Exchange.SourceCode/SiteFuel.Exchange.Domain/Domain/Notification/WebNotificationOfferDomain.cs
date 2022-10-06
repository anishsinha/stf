using Newtonsoft.Json;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using SiteFuel.Exchange.ViewModels.Queue;
using SiteFuel.Exchange.ViewModels.ThirdPartyOrder;
using SiteFuel.Exchange.ViewModels.WebNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SiteFuel.Exchange.Domain
{
    public class WebNotificationOfferDomain : BaseDomain
    {
        public WebNotificationOfferDomain() : base(ContextFactory.Current.ConnectionString)
        {

        }

        public WebNotificationOfferDomain(BaseDomain domain) : base(domain)
        {

        }

        //internal void ProcessOfferJsonMessage(NotificationOfferQueMsg offerQueMsg, List<string> errorInfo)
        //{
        //    using (var tracer = new Tracer("WebNotificationOfferDomain", "ProcessOfferJsonMessage"))
        //    {
        //        StringBuilder processMessage = new StringBuilder();

        //        try
        //        {
        //            if (offerQueMsg != null)
        //            {
        //                var offerEntity = Context.DataContext.OfferPricings.Where(t => t.Id == offerQueMsg.OfferId)
        //                                    .SingleOrDefault();
        //                if (offerEntity != null)
        //                {
        //                    var webNotifications = CreateOfferWebNotifications(offerEntity, offerQueMsg);
        //                    if (webNotifications.Any())
        //                    {
        //                        Context.DataContext.WebNotifications.AddRange(webNotifications);
        //                        Context.Commit();
        //                    }
        //                    //Add error info - NEED TO VERIFY FROM RAJEEV
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            if (!(ex is QueueMessageFatalException))
        //                LogManager.Logger.WriteException("WebNotificationOfferDomain", "ProcessOfferJsonMessage", ex.Message, ex);
        //            if (processMessage.Length == 0)
        //            {
        //                processMessage.Append(Constants.RequestError);
        //                errorInfo.Add(processMessage.ToString());
        //            }
        //            throw new QueueMessageFatalException(errorInfo[0], errorInfo);
        //        }
        //    }
        //}

        //private List<WebNotification> CreateOfferWebNotifications(OfferPricing offerPricing, NotificationOfferQueMsg offerQueMsg)
        //{
        //    var viewModelList = new List<WebNotification>();
        //    if (offerPricing != null)
        //    {
        //        if (offerPricing.IsActive)
        //        {
        //            string jsonMsg = GetOfferWebNotificationJson(offerPricing, offerQueMsg.CreatedByCompanyName);

        //            if (offerPricing.OfferTypeId == (int)OfferType.Exclusive)
        //            {
        //                ExclusiveOfferCustomerWebNotifications(offerQueMsg, viewModelList, offerPricing, jsonMsg);
        //                ExclusiveOfferTierWebNotifications(offerPricing.SupplierCompanyId, offerQueMsg, viewModelList, offerPricing, jsonMsg);
        //            }
        //            else
        //            {
        //                MarketOfferWebNotifications(offerQueMsg, viewModelList, offerPricing, jsonMsg);
        //            }
        //        }
        //    }
        //    return viewModelList;
        //}

        //private void MarketOfferWebNotifications(NotificationOfferQueMsg offerQueMsg, List<WebNotification> viewModelList, OfferPricing offerPricing, string jsonMsg)
        //{
        //    var allUsersOfBuyerCompanies = Context.DataContext.Users
        //                                .Where(t => t.IsActive 
        //                                        && t.Company.IsActive 
        //                                        && (t.Company.CompanyTypeId == (int)CompanyType.Buyer || t.Company.CompanyTypeId == (int)CompanyType.BuyerAndSupplier))
        //                                .Select(t => new { t.Id, CompanyId = t.CompanyId.Value, t.Company.CompanyTypeId }).Distinct().ToList();

        //    foreach (var user in allUsersOfBuyerCompanies)
        //    {
        //        AddWebNotificationsForCustomer(offerPricing, jsonMsg, offerQueMsg.OfferId, viewModelList, user.Id, user.CompanyId, user.CompanyTypeId);
        //    }
        //}

        //private void ExclusiveOfferTierWebNotifications(int offerSupplierCompanyId, NotificationOfferQueMsg offerQueMsg, List<WebNotification> viewModelList, OfferPricing offerPricing, string jsonMsg)
        //{

        //    var tierItemList = offerPricing.OfferPricingItems.Where(t => t.TierId != null && t.CustomerId == null)
        //                                    .Select(t => t.TierId.Value).Distinct().ToList();

        //    var customerList = Context.DataContext.OfferTierMappings
        //                        .Where(t => t.IsActive && t.SupplierCompanyId == offerSupplierCompanyId && tierItemList.Contains(t.TierId))
        //                        .Select(t => t.BuyerCompanyId).Distinct().ToList();

        //    var userList = Context.DataContext.Users.Where(t => t.CompanyId.HasValue && customerList.Contains(t.CompanyId.Value) && t.MstRoles.Any(r => r.Id == (int)UserRoles.BuyerAdmin
        //                                               || r.Id == (int)UserRoles.Buyer
        //                                               || r.Id == (int)UserRoles.Admin)
        //                                               && t.Company.IsActive)
        //                                             .Select(t => new { t.Id, CompanyId = t.CompanyId.Value, t.Company.CompanyTypeId }).ToList();

        //    //var userList = Context.DataContext.Users.Where(t => customerList.Contains(t.Id) && t.MstRoles.Any(r => r.Id == (int)UserRoles.BuyerAdmin
        //    //                                           || r.Id == (int)UserRoles.Buyer
        //    //                                           || r.Id == (int)UserRoles.Admin)
        //    //                                         && t.Company.IsActive
        //    //                                         && t.Company.OfferTierMappings.Any(c => c.SupplierCompanyId == offerSupplierCompanyId
        //    //                                                                                && c.IsActive
        //    //                                                                                && tierItemList.Contains(c.TierId)))
        //    //                                         .Select(t => new { t.Id, CompanyId = t.CompanyId.Value, t.Company.CompanyTypeId }).Distinct().ToList();

        //    foreach (var user in userList)
        //    {
        //        AddWebNotificationsForCustomer(offerPricing, jsonMsg, offerQueMsg.OfferId, viewModelList, user.Id, user.CompanyId, user.CompanyTypeId);
        //    }
        //}

        //private void ExclusiveOfferCustomerWebNotifications(NotificationOfferQueMsg offerQueMsg, List<WebNotification> viewModelList, OfferPricing offerPricing, string jsonMsg)
        //{
        //    var customerItemList = offerPricing.OfferPricingItems
        //                                .Where(t => t.CustomerId != null && t.TierId == null)
        //                                .Select(t => t.CustomerId.Value).Distinct().ToList();

        //    var exclusiveUsersList = Context.DataContext.Users
        //                                .Where(t => t.CompanyId.HasValue && customerItemList.Contains(t.CompanyId.Value) && t.Company.IsActive
        //                                            && t.MstRoles.Any(r => r.Id == (int)UserRoles.BuyerAdmin
        //                                             || r.Id == (int)UserRoles.Buyer
        //                                             || r.Id == (int)UserRoles.Admin))
        //                                .Select(t => new { t.Id, CompanyId = t.CompanyId.Value, t.Company.CompanyTypeId }).Distinct().ToList();

        //    foreach (var user in exclusiveUsersList)
        //    {
        //        AddWebNotificationsForCustomer(offerPricing, jsonMsg, offerQueMsg.OfferId, viewModelList, user.Id, user.CompanyId, user.CompanyTypeId);
        //    }
        //}

        //private void AddWebNotificationsForCustomer(OfferPricing pricingEntity, string jsonMsg, int offerId, List<WebNotification> viewModelList, int userId, int companyId, int companyTypeId)
        //{
        //    viewModelList.Add(new WebNotification()
        //    {
        //        CreatedBy = pricingEntity.CreatedBy,
        //        CreatedDate = pricingEntity.CreatedDate,
        //        CreatedFor = userId,
        //        CreatedForCompanyId = companyId,
        //        CreatedForCompanyTypeId = companyTypeId,
        //        EntityId = offerId,
        //        NotificationTypeId = (int)WebNotificationType.OfferNotification,
        //        JsonMessage = jsonMsg
        //    });
        //}

        //private string GetOfferWebNotificationJson(OfferPricing pricingEntity, string createdByCompanyName)
        //{
        //    var offerNotification = new WebNotificationOfferJson();
        //    HelperDomain helperDomain = new HelperDomain(this);
        //    offerNotification.CreatedByCompanyName = createdByCompanyName;
        //    offerNotification.FuelTypeName = helperDomain.GetProductName(pricingEntity.MstProduct);
        //    offerNotification.NotificaitonText = Resource.OfferWebNotificationText;
        //    offerNotification.FuelTypeId = pricingEntity.FuelTypeId;
        //    string json = JsonConvert.SerializeObject(offerNotification);
        //    return json;
        //}

    }
}