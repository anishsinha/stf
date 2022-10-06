using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels.Notification;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SiteFuel.Exchange.ViewModels
{
    public class NotificationTemplateViewModel : BaseNotificationViewModel
    {
        public NotificationTemplateViewModel()
        {
            Template = new TemplateViewModel();
        }

        public int CompanyTypeId { get; set; }

        public TemplateViewModel Template { get; set; }

    }

    public class TemplateViewModel
    {
        public int id { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string ButtonText { get; set; }

        public int NotificationType { get; set; }
    }
}
