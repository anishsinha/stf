using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationRequestFuelViewModel : BaseNotificationViewModel
    {
        public NotificationRequestFuelViewModel()
        {
            EmailRecipients = new List<string>();
        }

        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CompanyName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string FuelType { get; set; }

        public string Quantity { get; set; }

        public string PricePerGallon { get; set; }

        public string Zipcode { get; set; }

        public List<string> EmailRecipients { get; set; }

        public Currency Currency { get; set; }

        public UoM UoM { get; set; }
    }
}
