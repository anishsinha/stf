using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationCounterOfferCreatedViewModel : BaseNotificationViewModel
    {
        public int Id { get; set; }
        public string FuelRequestNumber { get; set; }
        public UserRoles CreatorRole { get; set; }
        public NotificationUserViewModel Buyer { get; set; }
        public NotificationUserViewModel Supplier { get; set; }
        public int BuyerCompanyId { get; set; }
        public string BuyerCompanyName { get; set; }
    }
}
