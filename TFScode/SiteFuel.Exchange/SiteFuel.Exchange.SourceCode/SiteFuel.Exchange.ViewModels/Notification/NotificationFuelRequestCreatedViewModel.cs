using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationFuelRequestCreatedViewModel : NotificationFuelRequestViewModel
    {
        public List<NotificationUserViewModel> Suppliers { get; set; }
    }
}
